using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using Abacus.DAL;
using Abacus.Models;
using Abacus.BPM;
using Abacus.Notification;

namespace Abacus.Controllers.DataControllers
{
    public class DocumentController : Controller
    {
        private AbacusDBEntities db = new AbacusDBEntities();

        private USER_ADMINS loggedAdmin;
        private USER_STATIONS loggedStation;
        private string StationName;
        private int stationId;

        Common c = new Common();
        Kerberos k = new Kerberos();

        #region --- ADMIN SECTION ---

        #region UPLOAD-DOWNLOAD FILES (ADMIN AREA)

        public ActionResult xUploadData(string notify = null)
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                ViewBag.loggedUser = "(χωρίς σύνδεση)";
                return RedirectToAction("Login", "USER_ADMINS");
            }
            else
            {
                loggedAdmin = GetLoginAdmin();
            }
            if (notify != null)
            {
                this.ShowMessage(MessageType.Warning, notify);
            }
            // Combos populators
            populateSchoolYears();

            return View();
        }

        #region MASTER GRID CRUD FUNCTIONS

        public ActionResult xUpload_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0)
        {
            List<DocUploadsViewModel> data = xGetUploadsFromDB(stationId);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult xUpload_Create([DataSourceRequest] DataSourceRequest request, DocUploadsViewModel data, int stationId = 0)
        {
            var newdata = new DocUploadsViewModel();

            if (!(stationId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε βρεφονηπιακό σταθμό. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                DOCUPLOADS entity = new DOCUPLOADS()
                {
                    STATION_ID = stationId,
                    SCHOOLYEAR_ID = data.SCHOOLYEAR_ID,
                    UPLOAD_DATE = data.UPLOAD_DATE,
                    UPLOAD_NAME = data.UPLOAD_NAME,
                    UPLOAD_SUMMARY = data.UPLOAD_SUMMARY
                };
                db.DOCUPLOADS.Add(entity);
                db.SaveChanges();
                data.UPLOAD_ID = entity.UPLOAD_ID;
                newdata = RefreshUploadsFromDB(data.UPLOAD_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult xUpload_Update([DataSourceRequest] DataSourceRequest request, DocUploadsViewModel data, int stationId = 0)
        {
            var newdata = new DocUploadsViewModel();

            if (!(stationId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε βρεφονηπιακό σταθμό. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                DOCUPLOADS entity = db.DOCUPLOADS.Find(data.UPLOAD_ID);

                entity.STATION_ID = stationId;
                entity.SCHOOLYEAR_ID = data.SCHOOLYEAR_ID;
                entity.UPLOAD_DATE = data.UPLOAD_DATE;
                entity.UPLOAD_NAME = data.UPLOAD_NAME;
                entity.UPLOAD_SUMMARY = data.UPLOAD_SUMMARY;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newdata = RefreshUploadsFromDB(entity.UPLOAD_ID);
            }

            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult xUpload_Destroy([DataSourceRequest] DataSourceRequest request, DocUploadsViewModel data)
        {
            if (data != null)
            {
                DOCUPLOADS entity = db.DOCUPLOADS.Find(data.UPLOAD_ID);
                if (entity != null)
                {
                    if (k.CanDeleteUpload(data.UPLOAD_ID))
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.DOCUPLOADS.Remove(entity);
                        db.SaveChanges();
                    }
                    else ModelState.AddModelError("", "Για να γίνει διαγραφή πρέπει πρώτα να διαγραφούν τα σχετικά αρχεία μεταφόρτωσης");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public DocUploadsViewModel RefreshUploadsFromDB(int recordId)
        {
            var data = (from d in db.DOCUPLOADS
                        where d.UPLOAD_ID == recordId
                        select new DocUploadsViewModel
                        {
                            UPLOAD_ID = d.UPLOAD_ID,
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            STATION_ID = d.STATION_ID,
                            UPLOAD_DATE = d.UPLOAD_DATE,
                            UPLOAD_NAME = d.UPLOAD_NAME,
                            UPLOAD_SUMMARY = d.UPLOAD_SUMMARY
                        }).FirstOrDefault();

            return (data);
        }

        public List<DocUploadsViewModel> xGetUploadsFromDB(int stationId = 0)
        {
            var data = (from d in db.DOCUPLOADS
                        where d.STATION_ID == stationId
                        orderby d.SCHOOLYEAR_ID descending, d.UPLOAD_DATE descending
                        select new DocUploadsViewModel
                        {
                            UPLOAD_ID = d.UPLOAD_ID,
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            STATION_ID = d.STATION_ID,
                            UPLOAD_DATE = d.UPLOAD_DATE,
                            UPLOAD_NAME = d.UPLOAD_NAME,
                            UPLOAD_SUMMARY = d.UPLOAD_SUMMARY
                        }).ToList();

            return (data);
        }

        #endregion

        #region CHILD GRID (UPLOADED FILEDETAILS)

        public ActionResult UploadFiles_Read([DataSourceRequest] DataSourceRequest request, int uploadId = 0)
        {
            List<DocUploadsFilesViewModel> data = GetUploadsFilesFromDB(uploadId);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadFiles_Destroy([DataSourceRequest] DataSourceRequest request, DocUploadsFilesViewModel data)
        {
            // TODO: ALSO REMOVE UPLOADED FILES FROM SERVER
            if (data != null)
            {
                DOCUPLOADS_FILES entity = db.DOCUPLOADS_FILES.Find(data.ID);
                if (entity != null)
                {
                    // First delete the physical file and then the info record. Important!
                    DeleteUploadedFile(data.ID);

                    db.Entry(entity).State = EntityState.Deleted;
                    db.DOCUPLOADS_FILES.Remove(entity);
                    db.SaveChanges();
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public List<DocUploadsFilesViewModel> GetUploadsFilesFromDB(int uploadId = 0)
        {
            var data = (from d in db.DOCUPLOADS_FILES
                        where d.UPLOAD_ID == uploadId
                        orderby d.STATION_USER, d.SCHOOLYEAR_TEXT, d.FILENAME
                        select new DocUploadsFilesViewModel
                        {
                            ID = d.ID,
                            UPLOAD_ID = d.UPLOAD_ID,
                            STATION_USER = d.STATION_USER,
                            SCHOOLYEAR_TEXT = d.SCHOOLYEAR_TEXT,
                            FILENAME = d.FILENAME,
                            EXTENSION = d.EXTENSION
                        }).ToList();

            return (data);
        }

        #endregion

        #region UPLOAD FORM WITH SAVE-REMOVE ACTIONS

        public ActionResult xUploadForm(int uploadId, string notify = null)
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                ViewBag.loggedUser = "(χωρίς σύνδεση)";
                return RedirectToAction("Login", "USER_ADMINS");
            }
            else
            {
                loggedAdmin = GetLoginAdmin();
            }
            if (notify != null) this.ShowMessage(MessageType.Warning, notify);            
            if (!(uploadId > 0))
            {
                string msg = "Άκυρος κωδικός μεταφόρτωσης. Πρέπει πρώτα να αποθηκεύσετε την εγγραφή μεταφόρτωσης.";
                return RedirectToAction("ErrorData", "Document", new { notify = msg });
            }
            ViewData["uploadId"] = uploadId;

            return View();
        }

        public ActionResult Upload(IEnumerable<HttpPostedFileBase> files, int uploadId = 0)
        {
            string folder = "";
            string uploadPath = "~/App_Data/";
            string subfolder = "";

            List<DocUploadsFilesViewModel> fileDetails = new List<DocUploadsFilesViewModel>();

            var upload_info = c.GetUploadInfo(uploadId);    // returns a tuple with STATION_ID and SCHOOLYEAR_ID
            folder = c.GetStationUsername(upload_info.Item1);
            subfolder = c.GetSchoolYearText(upload_info.Item2);

            if (!String.IsNullOrEmpty(folder) && !String.IsNullOrEmpty(subfolder))
                uploadPath += folder + "/" + subfolder + "/";

            try
            {
                bool exists = System.IO.Directory.Exists(Server.MapPath(uploadPath));
                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(uploadPath));

                if (files != null)
                {
                    foreach (var file in files)
                    {
                        // Some browsers send file names with full path.
                        // We are only interested in the file name.
                        if (file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            DOCUPLOADS_FILES fileDetail = new DOCUPLOADS_FILES()
                            {
                                FILENAME = fileName.Length > 120 ? fileName.Substring(0, 120) : fileName,
                                EXTENSION = Path.GetExtension(fileName),
                                STATION_USER = folder,
                                SCHOOLYEAR_TEXT = subfolder,
                                UPLOAD_ID = uploadId,
                                ID = Guid.NewGuid()
                            };
                            db.DOCUPLOADS_FILES.Add(fileDetail);
                            db.SaveChanges();

                            var physicalPath = Path.Combine(Server.MapPath(uploadPath), fileDetail.ID + fileDetail.EXTENSION);
                            file.SaveAs(physicalPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Παρουσιάστηκε σφάλμα στη μεταφόρτωση:<br/>" + ex.Message;
                return Content(msg);
            }
            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult Remove(string[] fileNames, int uploadId)
        {
            // The parameter of the Remove action must be called "fileNames"
            string folder = "";
            string uploadPath = "~/App_Data/";
            string subfolder = "";

            var upload_info = c.GetUploadInfo(uploadId);    // returns a tuple with STATION_ID and SCHOOLYEAR_ID
            folder = c.GetStationUsername(upload_info.Item1);
            subfolder = c.GetSchoolYearText(upload_info.Item2);

            if (!String.IsNullOrEmpty(folder) && !String.IsNullOrEmpty(subfolder))
                uploadPath += folder + "/" + subfolder + "/";

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var extension = Path.GetExtension(fileName);

                    Guid file_guid = c.GetFileGuidFromName(fileName, uploadId);

                    string fileToDelete = file_guid + extension;
                    var physicalPath = Path.Combine(Server.MapPath(uploadPath), fileToDelete);

                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                        DeleteUploadFileRecord(file_guid);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        #endregion

        public FileResult Download(Guid file_id)
        {
            String p = "";
            String f = "";
            string the_path = "~/App_Data/";

            var fileinfo = (from d in db.DOCUPLOADS_FILES where d.ID == file_id select d).FirstOrDefault();
            if (fileinfo != null)
            {
                the_path += fileinfo.STATION_USER + "/" + fileinfo.SCHOOLYEAR_TEXT + "/";
                p = fileinfo.ID.ToString() + fileinfo.EXTENSION;
                f = fileinfo.FILENAME;
            }

            return File(Path.Combine(Server.MapPath(the_path), p), System.Net.Mime.MediaTypeNames.Application.Octet, f);
        }

        public ActionResult DeleteUploadFileRecord(Guid file_guid)
        {
            DOCUPLOADS_FILES entity = db.DOCUPLOADS_FILES.Find(file_guid);
            if (entity != null)
            {
                db.Entry(entity).State = EntityState.Deleted;
                db.DOCUPLOADS_FILES.Remove(entity);
                db.SaveChanges();
            }
            return Content("");
        }

        public ActionResult DeleteUploadedFile(Guid file_guid)
        {
            string folder = "";
            string uploadPath = "~/App_Data/";
            string subfolder = "";
            string extension = "";

            var data = (from d in db.DOCUPLOADS_FILES where d.ID == file_guid select d).FirstOrDefault();
            if (data != null)
            {
                folder = data.STATION_USER;
                subfolder = data.SCHOOLYEAR_TEXT;
                extension = data.EXTENSION;

                if (!String.IsNullOrEmpty(folder) && !String.IsNullOrEmpty(subfolder))
                    uploadPath += folder + "/" + subfolder + "/";

                string fileToDelete = file_guid + extension;
                var physicalPath = Path.Combine(Server.MapPath(uploadPath), fileToDelete);
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }
            }
            return Content("");
        }

        #endregion UPLOAD-DOWNLOAD FILES (ADMIN AREA)


        #endregion --- ADMIN SECTION ---


        #region --- STATION SECTION ---


        #region ΧΕΙΡΙΣΤΕΣ ΕΓΓΡΑΦΩΝ ΣΤΑΘΜΩΝ

        public ActionResult DocOperators(string notify = null)
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                ViewBag.loggedUser = "(χωρίς σύνδεση)";
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;
            }
            if (notify != null)
            {
                this.ShowMessage(MessageType.Warning, notify);
            }
            return View();
        }

        public ActionResult DocOperator_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = GetDocOperatorsFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DocOperator_Create([DataSourceRequest] DataSourceRequest request, DocOperatorViewModel data)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var newData = new DocOperatorViewModel();

            var existingdata = db.ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ.Where(s => s.DOCADMIN_NAME == data.DOCADMIN_NAME).ToList();

            if (existingdata.Count > 0) ModelState.AddModelError("", "Αυτός ο χειριστής υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ entity = new ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ()
                {
                    DOCSTATION_ID = stationId,
                    DOCADMIN_NAME = data.DOCADMIN_NAME
                };
                db.ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ.Add(entity);
                db.SaveChanges();
                data.DOCADMIN_ID = entity.DOCADMIN_ID;
                newData = RefreshDocOperatorFromDB(entity.DOCADMIN_ID);
            }

            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DocOperator_Update([DataSourceRequest] DataSourceRequest request, DocOperatorViewModel data)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var newData = new DocOperatorViewModel();

            if (data != null && ModelState.IsValid)
            {
                ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ entity = db.ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ.Find(data.DOCADMIN_ID);

                entity.DOCSTATION_ID = stationId;
                entity.DOCADMIN_NAME = data.DOCADMIN_NAME;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newData = RefreshDocOperatorFromDB(entity.DOCADMIN_ID);
            }

            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DocOperator_Destroy([DataSourceRequest] DataSourceRequest request, DocOperatorViewModel data)
        {
            ModelState.Clear();
            if (data != null)
            {
                ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ entity = db.ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ.Find(data.DOCADMIN_ID);
                if (entity != null)
                {
                    if (!k.CanDeleteOperator(entity.DOCADMIN_ID))
                        ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί ο χειριστής διότι χρησιμοποιείται ήδη σε έγγραφα.");
                    if (ModelState.IsValid)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ.Remove(entity);
                        db.SaveChanges();
                    }
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public DocOperatorViewModel RefreshDocOperatorFromDB(int recordId)
        {
            var data = (from d in db.ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ
                        where d.DOCADMIN_ID == recordId
                        select new DocOperatorViewModel
                        {
                            DOCADMIN_ID = d.DOCADMIN_ID,
                            DOCSTATION_ID = d.DOCSTATION_ID,
                            DOCADMIN_NAME = d.DOCADMIN_NAME
                        }).FirstOrDefault();

            return data;
        }

        public List<DocOperatorViewModel> GetDocOperatorsFromDB()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ
                        orderby d.DOCADMIN_NAME
                        where d.DOCSTATION_ID == stationId
                        select new DocOperatorViewModel
                        {
                            DOCADMIN_ID = d.DOCADMIN_ID,
                            DOCSTATION_ID = d.DOCSTATION_ID,
                            DOCADMIN_NAME = d.DOCADMIN_NAME
                        }).ToList();

            return data;
        }

        #endregion

        #region ΔΙΑΒΙΒΑΣΤΙΚΑ ΥΠΗΡΕΣΙΑΚΩΝ ΜΕΤΑΒΟΛΩΝ

        public ActionResult DocMetaboles(string notify = null)
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                ViewBag.loggedUser = "(χωρίς σύνδεση)";
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;
            }
            if (notify != null)
            {
                this.ShowMessage(MessageType.Warning, notify);
            }
            // Combos populators
            if (!PersonnelExists() || !OperatorsExist())
            {
                string msg = "Για εμφάνιση της σελίδας πρέπει πρώτα να καταχωρήσετε προσωπικό και χειριστές εγγράφων του σταθμού.";
                return RedirectToAction("Index", "Station", new { notify = msg });
            }

            populateOperators();
            populateYears();
            populateMonths();
            populatePersonnel();

            return View();
        }

        #region MASTER GRID CRUD FUNCTIONS

        public ActionResult DocMetaboles_Read([DataSourceRequest] DataSourceRequest request, int schoolyearId = 0)
        {
            List<DocMetabolesViewModel> data = GetDocMetabolesFromDB(schoolyearId);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DocMetaboles_Create([DataSourceRequest] DataSourceRequest request, DocMetabolesViewModel data, int schoolyearId = 0)
        {
            var newdata = new DocMetabolesViewModel();
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (!(schoolyearId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε σχολικό έτος. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ entity = new ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ()
                {
                    SCHOOLYEAR_ID = schoolyearId,
                    STATION_ID = stationId,
                    PERIFERIAKI_ID = GetPeriferiakiFromStation(stationId),
                    ADMIN_ID = data.ADMIN_ID,
                    DOC_YEAR = data.DOC_YEAR,
                    DOC_MONTH = data.DOC_MONTH,
                    DOC_DATE = data.DOC_DATE,
                    DOC_PROTOCOL = data.DOC_PROTOCOL
                };
                db.ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ.Add(entity);
                db.SaveChanges();
                data.DOC_ID = entity.DOC_ID;
                newdata = RefreshDocMetabolesFromDB(data.DOC_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DocMetaboles_Update([DataSourceRequest] DataSourceRequest request, DocMetabolesViewModel data, int schoolyearId = 0)
        {
            var newdata = new DocMetabolesViewModel();
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (!(schoolyearId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε σχολικό έτος. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ entity = db.ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ.Find(data.DOC_ID);

                entity.SCHOOLYEAR_ID = schoolyearId;
                entity.STATION_ID = stationId;
                entity.PERIFERIAKI_ID = GetPeriferiakiFromStation(stationId);
                entity.ADMIN_ID = data.ADMIN_ID;
                entity.DOC_YEAR = data.DOC_YEAR;
                entity.DOC_MONTH = data.DOC_MONTH;
                entity.DOC_DATE = data.DOC_DATE;
                entity.DOC_PROTOCOL = data.DOC_PROTOCOL;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newdata = RefreshDocMetabolesFromDB(entity.DOC_ID);
            }

            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DocMetaboles_Destroy([DataSourceRequest] DataSourceRequest request, DocMetabolesViewModel data)
        {
            if (data != null)
            {
                ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ entity = db.ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ.Find(data.DOC_ID);
                if (ModelState.IsValid)
                {
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ.Remove(entity);
                        db.SaveChanges();
                    }
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public DocMetabolesViewModel RefreshDocMetabolesFromDB(int recordId)
        {
            var data = (from d in db.ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ
                        where d.DOC_ID == recordId
                        select new DocMetabolesViewModel
                        {
                            DOC_ID = d.DOC_ID,
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            STATION_ID = d.STATION_ID,
                            PERIFERIAKI_ID = d.PERIFERIAKI_ID,
                            ADMIN_ID = d.ADMIN_ID,
                            DOC_YEAR = d.DOC_YEAR,
                            DOC_MONTH = d.DOC_MONTH,
                            DOC_DATE = d.DOC_DATE,
                            DOC_PROTOCOL = d.DOC_PROTOCOL,
                            CORRECTION = d.CORRECTION ?? false,
                            CORRECTION_DATE = d.CORRECTION_DATE
                        }).FirstOrDefault();

            return (data);
        }

        public List<DocMetabolesViewModel> GetDocMetabolesFromDB(int schoolyearId = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ
                        where d.SCHOOLYEAR_ID == schoolyearId && d.STATION_ID == stationId
                        orderby d.DOC_DATE descending
                        select new DocMetabolesViewModel
                        {
                            DOC_ID = d.DOC_ID,
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            STATION_ID = d.STATION_ID,
                            PERIFERIAKI_ID = d.PERIFERIAKI_ID,
                            ADMIN_ID = d.ADMIN_ID,
                            DOC_YEAR = d.DOC_YEAR,
                            DOC_MONTH = d.DOC_MONTH,
                            DOC_DATE = d.DOC_DATE,
                            DOC_PROTOCOL = d.DOC_PROTOCOL,
                            CORRECTION = d.CORRECTION ?? false,
                            CORRECTION_DATE = d.CORRECTION_DATE
                        }).ToList();

            return (data);
        }

        #endregion

        #region DOCUMENT DATA FORM

        public ActionResult DocMetabolesEdit(int docId)
        {
            //check if user is unauthenticated to redirect him
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                ViewBag.loggedUser = "(χωρίς σύνδεση)";
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;
            }

            DocMetabolesViewModel doc = GetDocMetabolesViewModel(docId);
            if (doc == null)
            {
                string msg = "Ο κωδικός του εγγράφου δεν είναι έγκυρος. Η εγγραφή πρέπει να έχει αποθηκευτεί πρώτα στο πλέγμα.";
                return RedirectToAction("ErrorData", "Document", new { notify = msg });
            }

            DocMetabolesViewModel docData = GetDocMetabolesViewModel(docId);
            return View(docData);
        }

        [HttpPost]
        public ActionResult DocMetabolesEdit(int docId, DocMetabolesViewModel data)
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                ViewBag.loggedUser = "(χωρίς σύνδεση)";
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;
            }

            if (ModelState.IsValid)
            {
                ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ entity = db.ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ.Find(docId);

                entity.SCHOOLYEAR_ID = data.SCHOOLYEAR_ID;
                entity.STATION_ID = stationId;
                entity.PERIFERIAKI_ID = GetPeriferiakiFromStation(stationId);
                entity.ADMIN_ID = data.ADMIN_ID;
                entity.DOC_YEAR = data.DOC_YEAR;
                entity.DOC_MONTH = data.DOC_MONTH;
                entity.DOC_DATE = data.DOC_DATE;
                entity.DOC_PROTOCOL = data.DOC_PROTOCOL;
                entity.DOC_SXETIKA = data.DOC_SXETIKA;
                entity.CORRECTION = data.CORRECTION;
                entity.CORRECTION_DATE = data.CORRECTION_DATE;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                // Notify here
                this.ShowMessage(MessageType.Success, "Η αποθήκευση ολοκληρώθηκε με επιτυχία.");
                DocMetabolesViewModel newData = GetDocMetabolesViewModel(entity.DOC_ID);
                return View(newData);
            }
            this.ShowMessage(MessageType.Error, "Η αποθήκευση ακυρώθηκε λόγω σφαλμάτων καταχώρησης.");
            return View(data);
        }

        public DocMetabolesViewModel GetDocMetabolesViewModel(int docId)
        {
            DocMetabolesViewModel Data;

            Data = (from d in db.ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ
                    where d.DOC_ID == docId
                    select new DocMetabolesViewModel
                    {
                        DOC_ID = d.DOC_ID,
                        SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                        STATION_ID = d.STATION_ID,
                        PERIFERIAKI_ID = d.PERIFERIAKI_ID,
                        ADMIN_ID = d.ADMIN_ID,
                        DOC_YEAR = d.DOC_YEAR,
                        DOC_MONTH = d.DOC_MONTH,
                        DOC_DATE = d.DOC_DATE,
                        DOC_PROTOCOL = d.DOC_PROTOCOL,
                        DOC_SXETIKA = d.DOC_SXETIKA,
                        CORRECTION = d.CORRECTION ?? false,
                        CORRECTION_DATE = d.CORRECTION_DATE
                    }).FirstOrDefault();

            return Data;
        }

        #endregion

        // Child Grid (METABOLES_REPORT DATA)
        public ActionResult MetabolesReport_Read([DataSourceRequest] DataSourceRequest request, int yearId = 0, int monthId = 0)
        {
            List<MetabolesReportViewModel> data = GetMetabolesReportFromDB(yearId, monthId);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<MetabolesReportViewModel> GetMetabolesReportFromDB(int yearId = 0, int monthId = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.METABOLES_REPORT
                        orderby d.ΠΡΟΣΩΠΙΚΟ.ΕΠΩΝΥΜΟ, d.ΠΡΟΣΩΠΙΚΟ.ΟΝΟΜΑ
                        where d.BNS == stationId && d.METABOLI_YEAR == yearId && d.METABOLI_MONTH == monthId
                        select new MetabolesReportViewModel
                        {
                            RECORD_ID = d.RECORD_ID,
                            BNS = d.BNS,
                            EMPLOYEE_ID = d.EMPLOYEE_ID,
                            SCHOOL_YEAR = d.SCHOOL_YEAR,
                            METABOLI_MONTH = d.METABOLI_MONTH,
                            METABOLI_YEAR = d.METABOLI_YEAR,
                            METABOLI_TEXT = d.METABOLI_TEXT
                        }).ToList();

            return (data);
        }

        public int GetPeriferiakiFromStation(int stationId)
        {
            int periferiaki = 0;
            var data = (from d in db.ΣΥΣ_ΣΤΑΘΜΟΙ where d.ΣΤΑΘΜΟΣ_ΚΩΔ == stationId select d).FirstOrDefault();
            if (data != null) periferiaki = (int)data.ΠΕΡΙΦΕΡΕΙΑΚΗ;

            return (periferiaki);
        }

        public ActionResult DocMetabolesPrint(int docId)
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                ViewBag.loggedUser = "(χωρίς σύνδεση)";
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;
            }
            DocumentParameters parameters = new DocumentParameters();
            var data = (from d in db.ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ where d.DOC_ID == docId select d).FirstOrDefault();
            if (data != null)
            {
                parameters.documentId = data.DOC_ID;
                parameters.stationId = (int)data.STATION_ID;
                parameters.yearId = (int)data.DOC_YEAR;
                parameters.monthId = (int)data.DOC_MONTH;
            }

            return View(parameters);
        }

        public ActionResult srepMetabolesPrint(int stationId = 0, int yearId = 0, int monthId = 0)
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                ViewBag.loggedUser = "(χωρίς σύνδεση)";
                return RedirectToAction("Login", "USER_STATIONS");
            }
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            DocumentParameters p = new DocumentParameters();
            p.stationId = stationId;
            p.yearId = yearId;
            p.monthId = monthId;

            return View(p);
        }

        #endregion

        #region ΔΙΑΒΙΒΑΣΤΙΚΑ ΩΡΟΛΟΓΙΟΥ ΠΡΟΓΡΑΜΜΑΤΟΣ

        public ActionResult DocOrologio(string notify = null)
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                ViewBag.loggedUser = "(χωρίς σύνδεση)";
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;
            }
            if (notify != null)
            {
                this.ShowMessage(MessageType.Warning, notify);
            }
            // Combos populators
            if (!PersonnelExists() || !OperatorsExist())
            {
                string msg = "Για εμφάνιση της σελίδας πρέπει πρώτα να καταχωρήσετε προσωπικό και χειριστές εγγράφων του σταθμού.";
                return RedirectToAction("Index", "Station", new { notify = msg });
            }

            populateOperators();
            populateYears();
            populateMonths();
            populatePersonnel();

            return View();
        }

        #region MASTER GRID CRUD FUNCTIONS

        public ActionResult DocOrologio_Read([DataSourceRequest] DataSourceRequest request, int schoolyearId = 0)
        {
            List<DocProgrammaViewModel> data = GetDocOrologioFromDB(schoolyearId);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DocOrologio_Create([DataSourceRequest] DataSourceRequest request, DocProgrammaViewModel data, int schoolyearId = 0)
        {
            var newdata = new DocProgrammaViewModel();
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (!(schoolyearId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε σχολικό έτος. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ entity = new ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ()
                {
                    SCHOOLYEAR_ID = schoolyearId,
                    STATION_ID = stationId,
                    PERIFERIAKI_ID = GetPeriferiakiFromStation(stationId),
                    ADMIN_ID = data.ADMIN_ID,
                    DOC_YEAR = data.DOC_YEAR,
                    DOC_MONTH = data.DOC_MONTH,
                    DOC_DATE = data.DOC_DATE,
                    DOC_PROTOCOL = data.DOC_PROTOCOL
                };
                db.ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ.Add(entity);
                db.SaveChanges();
                data.DOC_ID = entity.DOC_ID;
                newdata = RefreshDocOrologioFromDB(data.DOC_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DocOrologio_Update([DataSourceRequest] DataSourceRequest request, DocProgrammaViewModel data, int schoolyearId = 0)
        {
            var newdata = new DocProgrammaViewModel();
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (!(schoolyearId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε σχολικό έτος. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ entity = db.ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ.Find(data.DOC_ID);

                entity.SCHOOLYEAR_ID = schoolyearId;
                entity.STATION_ID = stationId;
                entity.PERIFERIAKI_ID = GetPeriferiakiFromStation(stationId);
                entity.ADMIN_ID = data.ADMIN_ID;
                entity.DOC_YEAR = data.DOC_YEAR;
                entity.DOC_MONTH = data.DOC_MONTH;
                entity.DOC_DATE = data.DOC_DATE;
                entity.DOC_PROTOCOL = data.DOC_PROTOCOL;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newdata = RefreshDocOrologioFromDB(entity.DOC_ID);
            }

            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DocOrologio_Destroy([DataSourceRequest] DataSourceRequest request, DocProgrammaViewModel data)
        {
            if (data != null)
            {
                ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ entity = db.ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ.Find(data.DOC_ID);
                if (ModelState.IsValid)
                {
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ.Remove(entity);
                        db.SaveChanges();
                    }
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public DocProgrammaViewModel RefreshDocOrologioFromDB(int recordId)
        {
            var data = (from d in db.ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ
                        where d.DOC_ID == recordId
                        select new DocProgrammaViewModel
                        {
                            DOC_ID = d.DOC_ID,
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            STATION_ID = d.STATION_ID,
                            PERIFERIAKI_ID = d.PERIFERIAKI_ID,
                            ADMIN_ID = d.ADMIN_ID,
                            DOC_YEAR = d.DOC_YEAR,
                            DOC_MONTH = d.DOC_MONTH,
                            DOC_DATE = d.DOC_DATE,
                            DOC_PROTOCOL = d.DOC_PROTOCOL,
                            CORRECTION = d.CORRECTION ?? false,
                            CORRECTION_DATE = d.CORRECTION_DATE
                        }).FirstOrDefault();

            return (data);
        }

        public List<DocProgrammaViewModel> GetDocOrologioFromDB(int schoolyearId = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ
                        where d.SCHOOLYEAR_ID == schoolyearId && d.STATION_ID == stationId
                        orderby d.DOC_DATE descending
                        select new DocProgrammaViewModel
                        {
                            DOC_ID = d.DOC_ID,
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            STATION_ID = d.STATION_ID,
                            PERIFERIAKI_ID = d.PERIFERIAKI_ID,
                            ADMIN_ID = d.ADMIN_ID,
                            DOC_YEAR = d.DOC_YEAR,
                            DOC_MONTH = d.DOC_MONTH,
                            DOC_DATE = d.DOC_DATE,
                            DOC_PROTOCOL = d.DOC_PROTOCOL,
                            CORRECTION = d.CORRECTION ?? false,
                            CORRECTION_DATE = d.CORRECTION_DATE
                        }).ToList();

            return (data);
        }

        #endregion

        #region DOCUMENT DATA FORM

        public ActionResult DocOrologioEdit(int docId)
        {
            //check if user is unauthenticated to redirect him
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                ViewBag.loggedUser = "(χωρίς σύνδεση)";
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;
            }

            DocProgrammaViewModel doc = GetDocOrologioViewModel(docId);
            if (doc == null)
            {
                string msg = "Ο κωδικός του εγγράφου δεν είναι έγκυρος. Η εγγραφή πρέπει να έχει αποθηκευτεί πρώτα στο πλέγμα.";
                return RedirectToAction("ErrorData", "Document", new { notify = msg });
            }

            DocProgrammaViewModel docData = GetDocOrologioViewModel(docId);
            return View(docData);
        }

        [HttpPost]
        public ActionResult DocOrologioEdit(int docId, DocProgrammaViewModel data)
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                ViewBag.loggedUser = "(χωρίς σύνδεση)";
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;
            }

            if (ModelState.IsValid)
            {
                ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ entity = db.ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ.Find(docId);

                entity.SCHOOLYEAR_ID = data.SCHOOLYEAR_ID;
                entity.STATION_ID = stationId;
                entity.PERIFERIAKI_ID = GetPeriferiakiFromStation(stationId);
                entity.ADMIN_ID = data.ADMIN_ID;
                entity.DOC_YEAR = data.DOC_YEAR;
                entity.DOC_MONTH = data.DOC_MONTH;
                entity.DOC_DATE = data.DOC_DATE;
                entity.DOC_PROTOCOL = data.DOC_PROTOCOL;
                entity.DOC_SXETIKA = data.DOC_SXETIKA;
                entity.CORRECTION = data.CORRECTION;
                entity.CORRECTION_DATE = data.CORRECTION_DATE;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                // Notify here
                this.ShowMessage(MessageType.Success, "Η αποθήκευση ολοκληρώθηκε με επιτυχία.");
                DocProgrammaViewModel newData = GetDocOrologioViewModel(entity.DOC_ID);
                return View(newData);
            }
            this.ShowMessage(MessageType.Error, "Η αποθήκευση ακυρώθηκε λόγω σφαλμάτων καταχώρησης.");
            return View(data);
        }

        public DocProgrammaViewModel GetDocOrologioViewModel(int docId)
        {
            DocProgrammaViewModel Data;

            Data = (from d in db.ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ
                    where d.DOC_ID == docId
                    select new DocProgrammaViewModel
                    {
                        DOC_ID = d.DOC_ID,
                        SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                        STATION_ID = d.STATION_ID,
                        PERIFERIAKI_ID = d.PERIFERIAKI_ID,
                        ADMIN_ID = d.ADMIN_ID,
                        DOC_YEAR = d.DOC_YEAR,
                        DOC_MONTH = d.DOC_MONTH,
                        DOC_DATE = d.DOC_DATE,
                        DOC_PROTOCOL = d.DOC_PROTOCOL,
                        DOC_SXETIKA = d.DOC_SXETIKA,
                        CORRECTION = d.CORRECTION ?? false,
                        CORRECTION_DATE = d.CORRECTION_DATE
                    }).FirstOrDefault();

            return Data;
        }

        #endregion

        public ActionResult DocOrologioPrint(int docId)
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                ViewBag.loggedUser = "(χωρίς σύνδεση)";
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;
            }
            DocumentParameters parameters = new DocumentParameters();
            var data = (from d in db.ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ where d.DOC_ID == docId select d).FirstOrDefault();
            if (data != null)
            {
                parameters.documentId = data.DOC_ID;
                parameters.stationId = (int)data.STATION_ID;
                parameters.yearId = (int)data.DOC_YEAR;
                parameters.monthId = (int)data.DOC_MONTH;
            }

            return View(parameters);
        }

        public ActionResult srepOrologioPrint(int stationId = 0, int yearId = 0, int monthId = 0)
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                ViewBag.loggedUser = "(χωρίς σύνδεση)";
                return RedirectToAction("Login", "USER_STATIONS");
            }
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            DocumentParameters p = new DocumentParameters();
            p.stationId = stationId;
            p.yearId = yearId;
            p.monthId = monthId;

            return View(p);
        }

        #endregion

        #region UPLOAD-DOWNLOAD FILES (STATION AREA)

        public ActionResult UploadData(string notify = null)
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                ViewBag.loggedUser = "(χωρίς σύνδεση)";
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;
            }
            if (notify != null)
            {
                this.ShowMessage(MessageType.Warning, notify);
            }
            // Combos populators
            populateSchoolYears();

            return View();
        }

        #region MASTER GRID CRUD FUNCTIONS

        public ActionResult Upload_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<DocUploadsViewModel> data = GetUploadsFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload_Create([DataSourceRequest] DataSourceRequest request, DocUploadsViewModel data)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var newdata = new DocUploadsViewModel();

            if (!(stationId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε βρεφονηπιακό σταθμό. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                DOCUPLOADS entity = new DOCUPLOADS()
                {
                    STATION_ID = stationId,
                    SCHOOLYEAR_ID = data.SCHOOLYEAR_ID,
                    UPLOAD_DATE = data.UPLOAD_DATE,
                    UPLOAD_NAME = data.UPLOAD_NAME,
                    UPLOAD_SUMMARY = data.UPLOAD_SUMMARY
                };
                db.DOCUPLOADS.Add(entity);
                db.SaveChanges();
                data.UPLOAD_ID = entity.UPLOAD_ID;
                newdata = RefreshUploadsFromDB(data.UPLOAD_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload_Update([DataSourceRequest] DataSourceRequest request, DocUploadsViewModel data)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var newdata = new DocUploadsViewModel();

            if (!(stationId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε βρεφονηπιακό σταθμό. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                DOCUPLOADS entity = db.DOCUPLOADS.Find(data.UPLOAD_ID);

                entity.STATION_ID = stationId;
                entity.SCHOOLYEAR_ID = data.SCHOOLYEAR_ID;
                entity.UPLOAD_DATE = data.UPLOAD_DATE;
                entity.UPLOAD_NAME = data.UPLOAD_NAME;
                entity.UPLOAD_SUMMARY = data.UPLOAD_SUMMARY;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newdata = RefreshUploadsFromDB(entity.UPLOAD_ID);
            }

            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload_Destroy([DataSourceRequest] DataSourceRequest request, DocUploadsViewModel data)
        {
            if (data != null)
            {
                DOCUPLOADS entity = db.DOCUPLOADS.Find(data.UPLOAD_ID);
                if (entity != null)
                {
                    if (k.CanDeleteUpload(data.UPLOAD_ID))
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.DOCUPLOADS.Remove(entity);
                        db.SaveChanges();
                    }
                    else ModelState.AddModelError("", "Για να γίνει διαγραφή πρέπει πρώτα να διαγραφούν τα σχετικά αρχεία μεταφόρτωσης");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public List<DocUploadsViewModel> GetUploadsFromDB()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.DOCUPLOADS
                        where d.STATION_ID == stationId
                        orderby d.SCHOOLYEAR_ID descending, d.UPLOAD_DATE descending
                        select new DocUploadsViewModel
                        {
                            UPLOAD_ID = d.UPLOAD_ID,
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            STATION_ID = d.STATION_ID,
                            UPLOAD_DATE = d.UPLOAD_DATE,
                            UPLOAD_NAME = d.UPLOAD_NAME,
                            UPLOAD_SUMMARY = d.UPLOAD_SUMMARY
                        }).ToList();

            return (data);
        }

        #endregion

        // All related functions are common with those in the admin section
        public ActionResult UploadForm(int uploadId, string notify = null)
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                ViewBag.loggedUser = "(χωρίς σύνδεση)";
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;
            }
            if (notify != null) this.ShowMessage(MessageType.Warning, notify);
            if (!(uploadId > 0))
            {
                string msg = "Άκυρος κωδικός μεταφόρτωσης. Πρέπει πρώτα να αποθηκεύσετε την εγγραφή μεταφόρτωσης.";
                return RedirectToAction("ErrorData", "Document", new { notify = msg });
            }
            ViewData["uploadId"] = uploadId;

            return View();
        }

        #endregion UPLOAD-DOWNLOAD FILES (STATION AREA)


        #endregion --- STATION SECTION ---


        #region VALIDATORS

        public bool HoursExist()
        {
            var data = (from d in db.ΣΥΣ_ΩΡΕΣ select d).ToList();
            if (data.Count == 0) return false;

            return true;
        }

        public bool MonthsExist()
        {
            var data = (from d in db.ΣΥΣ_ΜΗΝΕΣ select d).ToList();
            if (data.Count == 0) return false;

            return true;
        }

        public bool OperatorsExist()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ where d.DOCSTATION_ID == stationId select d).ToList();
            if (data.Count == 0) return false;

            return true;
        }

        public bool PersonnelExists()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΠΡΟΣΩΠΙΚΟ where d.ΒΝΣ == stationId select d).ToList();
            if (data.Count == 0) return false;

            return true;
        }

        public bool EducatorsExist()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΠΡΟΣΩΠΙΚΟ where d.ΒΝΣ == stationId && d.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ == 1 select d).ToList();
            if (data.Count == 0) return false;

            return true;
        }

        public bool StationsExist()
        {
            var data = (from d in db.ΣΥΣ_ΣΤΑΘΜΟΙ select d).ToList();
            if (data.Count == 0) return false;

            return true;
        }

        public bool PersonnelTypesExist()
        {
            var data = (from d in db.ΣΥΣ_ΠΡΟΣΩΠΙΚΟ select d).ToList();
            if (data.Count == 0) return false;

            return true;
        }

        public bool TmimataExist()
        {
            var data = (from d in db.ΤΜΗΜΑ select d).ToList();
            if (data.Count == 0) return false;

            return true;
        }

        public bool SchoolYearsExist()
        {
            var data = (from d in db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ select d).ToList();
            if (data.Count == 0) return false;

            return true;
        }

        public bool TmimaChildrenExist()
        {
            var data = (from d in db.sqlCHILD_TMIMA_SELECTOR select d).ToList();
            if (data.Count == 0) return false;

            return true;
        }

        public bool ParousiesMonthExist()
        {
            var data = (from d in db.repΠΑΡΟΥΣΙΕΣ_ΜΗΝΑΣ select d).ToList();
            if (data.Count == 0) return false;

            return true;
        }

        public bool MetabolesTypesExist()
        {
            var data = (from d in db.ΣΥΣ_ΜΕΤΑΒΟΛΕΣ select d).ToList();
            if (data.Count == 0) return false;

            return true;
        }

        #endregion

        #region POPULATORS

        public void populateOperators()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ where d.DOCSTATION_ID == stationId orderby d.DOCADMIN_NAME select d).ToList();

            ViewData["operators"] = data;
            ViewData["defaultOperator"] = data.First().DOCADMIN_ID;
        }

        public void populateEmployees()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.srcPERSONNEL_DATA where d.ΒΝΣ == stationId orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ select d).ToList();

            ViewData["personnel"] = data;
            ViewData["defaultPersonnel"] = data.First().PERSONNEL_ID;
        }

        public void populateYears()
        {
            var data = (from d in db.ΣΥΣ_ΕΤΗ orderby d.ΕΤΟΣ_ΚΩΔ select d).ToList();
            ViewData["years"] = data;
        }


        public void populateMonths()
        {
            var data = (from d in db.ΣΥΣ_ΜΗΝΕΣ orderby d.ΜΗΝΑΣ_ΚΩΔ select d).ToList();
            ViewData["months"] = data;
        }

        public void populateMetabolesTypes()
        {
            var data = (from d in db.ΣΥΣ_ΜΕΤΑΒΟΛΕΣ orderby d.METABOLI_TEXT select d).ToList();

            ViewData["metaboles_types"] = data;
        }

        public void populateHours()
        {
            var hours = (from d in db.ΣΥΣ_ΩΡΕΣ orderby d.HOUR_TEXT select d).ToList();

            ViewData["hours"] = hours;
        }

        public void populateChildren()
        {
            var data = (from d in db.sqlCHILD_TMIMA_SELECTOR orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ select d).ToList();

            ViewData["children"] = data;
            ViewData["defaultChild"] = data.First().CHILD_ID;
        }

        public void populatePersonnel()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.sqlPERSONNEL_SELECTOR where d.ΒΝΣ == stationId orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ select d).ToList();

            ViewData["persons"] = data;
            ViewData["defaultPerson"] = data.First().PERSONNEL_ID;
        }

        public void populateEducators()
        {
            var educators = (from d in db.sqlPERSONNEL_SELECTOR where d.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ == 1 orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ select d).ToList();

            ViewData["educators"] = educators;
            ViewData["defaultEducator"] = educators.First().PERSONNEL_ID;
        }

        public void populatePersonnelTypes()
        {
            var personnelTypes = (from k in db.ΣΥΣ_ΠΡΟΣΩΠΙΚΟ select k).ToList();

            ViewData["person_types"] = personnelTypes;
        }

        public void populatePeriferiakes()
        {
            var data = (from d in db.ΣΥΣ_ΠΕΡΙΦΕΡΕΙΑΚΕΣ select d).ToList();
            ViewData["periferiakes"] = data;
        }

        public void populatePeriferies()
        {
            var data = (from d in db.ΣΥΣ_ΠΕΡΙΦΕΡΕΙΕΣ select d).ToList();
            ViewData["periferies"] = data;
        }

        public void populateGenders()
        {
            var data = (from d in db.ΣΥΣ_ΦΥΛΑ select d).ToList();
            ViewData["genders"] = data;
        }

        public void populateSchoolYears()
        {
            var data = (from d in db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ select d).ToList();
            ViewData["schoolYears"] = data;
        }

        public void populateStations()
        {
            var data = (from d in db.ΣΥΣ_ΣΤΑΘΜΟΙ orderby d.ΕΠΩΝΥΜΙΑ select d).ToList();
            ViewData["stations"] = data;
            ViewData["defaultStation"] = data.First().ΣΤΑΘΜΟΣ_ΚΩΔ;
        }

        public void populateKladoi()
        {
            var kladosTypes = (from k in db.ΣΥΣ_ΚΛΑΔΟΙ select k).ToList();

            ViewData["kladoi"] = kladosTypes;
        }

        public void populateTmimaCategories()
        {
            var categories = (from k in db.ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ select k).ToList();

            ViewData["tmima_categories"] = categories;
        }

        public void populateNumbersLatin()
        {
            var numbers = (from k in db.ΣΥΣ_ΑΑ_ΛΑΤΙΝΙΚΟΙ select k).ToList();

            ViewData["latin_numbers"] = numbers;
        }

        public void populateTmimata()
        {
            var tmimata = (from d in db.ΤΜΗΜΑ orderby d.ΟΝΟΜΑΣΙΑ select d).ToList();
            ViewData["tmimata"] = tmimata;
            ViewData["defaultTmima"] = tmimata.First().ΤΜΗΜΑ_ΚΩΔ;
        }

        #endregion

        #region GETTERS

        public JsonResult GetYears()
        {
            var data = db.ΣΥΣ_ΕΤΗ.Select(m => new SysYearViewModel
            {
                ΕΤΟΣ_ΚΩΔ = m.ΕΤΟΣ_ΚΩΔ,
                ΕΤΟΣ = m.ΕΤΟΣ
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMonths()
        {
            var data = (from d in db.ΣΥΣ_ΜΗΝΕΣ
                        orderby d.ΜΗΝΑΣ_ΚΩΔ
                        select new SysMonthViewModel
                        {
                            ΜΗΝΑΣ_ΚΩΔ = d.ΜΗΝΑΣ_ΚΩΔ,
                            ΜΗΝΑΣ = d.ΜΗΝΑΣ
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOperators()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ
                        where d.DOCSTATION_ID == stationId
                        orderby d.DOCADMIN_NAME
                        select new DocOperatorViewModel
                        {
                            DOCADMIN_ID = d.DOCADMIN_ID,
                            DOCADMIN_NAME = d.DOCADMIN_NAME
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBathmos()
        {
            var data = db.ΣΥΣ_ΒΑΘΜΟΙ.Select(m => new BathmosViewModel
            {
                ΒΑΘΜΟΣ_ΚΩΔ = m.ΒΑΘΜΟΣ_ΚΩΔ,
                ΒΑΘΜΟΣ_ΛΕΚΤΙΚΟ = m.ΒΑΘΜΟΣ_ΛΕΚΤΙΚΟ
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MetabolesTypes_Read()
        {
            var data = (from d in db.ΣΥΣ_ΜΕΤΑΒΟΛΕΣ
                        orderby d.METABOLI_TEXT
                        select new SysMetabolesViewModel
                        {
                            METABOLI_ID = d.METABOLI_ID,
                            METABOLI_TEXT = d.METABOLI_TEXT
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStations()
        {
            var data = (from d in db.ΣΥΣ_ΣΤΑΘΜΟΙ
                        orderby d.ΕΠΩΝΥΜΙΑ
                        select new StationsViewModel
                        {
                            ΣΤΑΘΜΟΣ_ΚΩΔ = d.ΣΤΑΘΜΟΣ_ΚΩΔ,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetKlados()
        {
            var data = db.ΣΥΣ_ΚΛΑΔΟΙ.Select(m => new KladosViewModel
            {
                ΚΛΑΔΟΣ_ΚΩΔ = m.ΚΛΑΔΟΣ_ΚΩΔ,
                ΚΛΑΔΟΣ = m.ΚΛΑΔΟΣ
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPersonEidikotites(int? kladosId)
        {
            var data = (from d in db.ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ
                        where d.KLADOS == kladosId
                        select new EidikotitesViewModel
                        {
                            EIDIKOTITA_ID = d.EIDIKOTITA_ID,
                            EIDIKOTITA_TEXT = d.EIDIKOTITA_TEXT,
                            EIDIKOTITA_CODE = d.EIDIKOTITA_CODE
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPersonTypes()
        {
            var data = db.ΣΥΣ_ΠΡΟΣΩΠΙΚΟ.Select(m => new PersonnelTypeViewModel
            {
                PROSOPIKO_ID = m.PROSOPIKO_ID,
                PROSOPIKO_TEXT = m.PROSOPIKO_TEXT
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSchoolYears()
        {
            var data = db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ.Select(m => new SysSchoolYearViewModel
            {
                SCHOOLYEAR_ID = m.SCHOOLYEAR_ID,
                ΣΧΟΛΙΚΟ_ΕΤΟΣ = m.ΣΧΟΛΙΚΟ_ΕΤΟΣ
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGenders()
        {
            var genders = db.ΣΥΣ_ΦΥΛΑ.Select(p => new SysGenderViewModel
            {
                ΦΥΛΟ_ΚΩΔ = p.ΦΥΛΟ_ΚΩΔ,
                ΦΥΛΟ = p.ΦΥΛΟ
            });

            return Json(genders, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPeriferiakes()
        {
            var data = db.ΣΥΣ_ΠΕΡΙΦΕΡΕΙΑΚΕΣ.Select(p => new SysPeriferiakiViewModel
            {
                ΚΩΔΙΚΟΣ_ΠΕΡΙΦΕΡΕΙΑ = p.ΚΩΔΙΚΟΣ_ΠΕΡΙΦΕΡΕΙΑ,
                ΕΠΩΝΥΜΙΑ_ΠΕΡΙΦΕΡΕΙΑ = p.ΕΠΩΝΥΜΙΑ_ΠΕΡΙΦΕΡΕΙΑ
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPeriferies()
        {
            var data = db.ΣΥΣ_ΠΕΡΙΦΕΡΕΙΕΣ.Select(p => new PeriferiaViewModel
            {
                PERIFERIA_ID = p.PERIFERIA_ID,
                PERIFERIA_NAME = p.PERIFERIA_NAME
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public USER_ADMINS GetLoginAdmin()
        {
            loggedAdmin = db.USER_ADMINS.Where(u => u.USERNAME == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();
            ViewBag.loggedUser = loggedAdmin.FULLNAME;
            return loggedAdmin;
        }

        public USER_STATIONS GetLoginStation()
        {
            loggedStation = db.USER_STATIONS.Where(u => u.USERNAME == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();

            int StationID = loggedStation.STATION_ID ?? 0;
            var _station = (from s in db.sqlUSER_STATION
                            where s.STATION_ID == StationID
                            select new { s.ΕΠΩΝΥΜΙΑ }).FirstOrDefault();

            ViewBag.loggedUser = _station.ΕΠΩΝΥΜΙΑ;
            StationName = _station.ΕΠΩΝΥΜΙΑ;
            return loggedStation;
        }


        #endregion

        public ActionResult ErrorData(string notify = null)
        {
            if (notify != null) this.ShowMessage(MessageType.Warning, notify);

            return View();
        }

    }
}