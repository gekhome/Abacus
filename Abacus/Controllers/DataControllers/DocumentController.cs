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
using Abacus.Services;

namespace Abacus.Controllers.DataControllers
{
    public class DocumentController : Controller
    {
        private readonly AbacusDBEntities db;

        private USER_ADMINS loggedAdmin;
        private USER_STATIONS loggedStation;
        private int stationId;

        private const string UPLOAD_PATH = "~/Uploads/";

        private readonly UploadService uploadService;
        private readonly DocOperatorService docOperatorService;
        private readonly DocMetabolesService docMetabolesService;
        private readonly DocOrologioService docOrologioService;

        public DocumentController()
        {
            db = new AbacusDBEntities();

            uploadService = new UploadService(db);
            docOperatorService = new DocOperatorService(db);
            docMetabolesService = new DocMetabolesService(db);
            docOrologioService = new DocOrologioService(db);
        }

        protected override void Dispose(bool disposing)
        {
            uploadService.Dispose();
            docOperatorService.Dispose();
            docMetabolesService.Dispose();
            docOrologioService.Dispose();

            base.Dispose(disposing);
        }

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
            PopulateSchoolYears();

            return View();
        }

        #region MASTER GRID CRUD FUNCTIONS

        public ActionResult xUpload_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0)
        {
            List<DocUploadsViewModel> data = uploadService.Read(stationId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult xUpload_Create([DataSourceRequest] DataSourceRequest request, DocUploadsViewModel data, int stationId = 0)
        {
            var newdata = new DocUploadsViewModel();

            if (!(stationId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε βρεφονηπιακό σταθμό. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                uploadService.Create(data, stationId);
                newdata = uploadService.Refresh(data.UPLOAD_ID);
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
                uploadService.Update(data, stationId);
                newdata = uploadService.Refresh(data.UPLOAD_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult xUpload_Destroy([DataSourceRequest] DataSourceRequest request, DocUploadsViewModel data)
        {
            if (data != null)
            {
                if (Kerberos.CanDeleteUpload(data.UPLOAD_ID))
                {
                    uploadService.Destroy(data);
                }
                else
                {
                    ModelState.AddModelError("", "Για να γίνει διαγραφή πρέπει πρώτα να διαγραφούν τα σχετικά αρχεία μεταφόρτωσης");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region CHILD GRID (UPLOADED FILEDETAILS)

        public ActionResult UploadFiles_Read([DataSourceRequest] DataSourceRequest request, int uploadId = 0)
        {
            List<DocUploadsFilesViewModel> data = uploadService.GetFiles(uploadId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadFiles_Destroy([DataSourceRequest] DataSourceRequest request, DocUploadsFilesViewModel data)
        {
            if (data != null)
            {
                // First delete the physical file and then the info record. Important!
                DeleteUploadedFile(data.ID);

                uploadService.DeleteFile(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
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
            string uploadPath = UPLOAD_PATH;
            string subfolder = "";

            List<DocUploadsFilesViewModel> fileDetails = new List<DocUploadsFilesViewModel>();

            var upload_info = Common.GetUploadInfo(uploadId);    // returns a tuple with STATION_ID and SCHOOLYEAR_ID
            folder = Common.GetStationUsername(upload_info.Item1);
            subfolder = Common.GetSchoolYearText(upload_info.Item2);

            if (!string.IsNullOrEmpty(folder) && !string.IsNullOrEmpty(subfolder))
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
            string uploadPath = UPLOAD_PATH;
            string subfolder = "";

            var upload_info = Common.GetUploadInfo(uploadId);    // returns a tuple with STATION_ID and SCHOOLYEAR_ID
            folder = Common.GetStationUsername(upload_info.Item1);
            subfolder = Common.GetSchoolYearText(upload_info.Item2);

            if (!string.IsNullOrEmpty(folder) && !string.IsNullOrEmpty(subfolder))
                uploadPath += folder + "/" + subfolder + "/";

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var extension = Path.GetExtension(fileName);

                    Guid file_guid = Common.GetFileGuidFromName(fileName, uploadId);

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
            string p = "";
            string f = "";
            string the_path = UPLOAD_PATH;

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
            string uploadPath = UPLOAD_PATH;
            string subfolder = "";
            string extension = "";

            var data = (from d in db.DOCUPLOADS_FILES where d.ID == file_guid select d).FirstOrDefault();
            if (data != null)
            {
                folder = data.STATION_USER;
                subfolder = data.SCHOOLYEAR_TEXT;
                extension = data.EXTENSION;

                if (!string.IsNullOrEmpty(folder) && !string.IsNullOrEmpty(subfolder))
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


        #endregion


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
            int stationId = (int)GetLoginStation().STATION_ID;

            IEnumerable<DocOperatorViewModel> data = docOperatorService.Read(stationId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DocOperator_Create([DataSourceRequest] DataSourceRequest request, DocOperatorViewModel data)
        {
            int stationId = (int)GetLoginStation().STATION_ID;

            var newData = new DocOperatorViewModel();

            var existingdata = db.ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ.Where(s => s.DOCADMIN_NAME == data.DOCADMIN_NAME).Count();

            if (existingdata > 0) ModelState.AddModelError("", "Αυτός ο χειριστής υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                docOperatorService.Create(data, stationId);
                newData = docOperatorService.Refresh(data.DOCADMIN_ID);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DocOperator_Update([DataSourceRequest] DataSourceRequest request, DocOperatorViewModel data)
        {
            int stationId = (int)GetLoginStation().STATION_ID;

            var newData = new DocOperatorViewModel();

            if (data != null && ModelState.IsValid)
            {
                docOperatorService.Update(data, stationId);
                newData = docOperatorService.Refresh(data.DOCADMIN_ID);
            }

            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DocOperator_Destroy([DataSourceRequest] DataSourceRequest request, DocOperatorViewModel data)
        {
            ModelState.Clear();
            if (data != null)
            {
                if (!Kerberos.CanDeleteOperator(data.DOCADMIN_ID))
                    ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί ο χειριστής διότι χρησιμοποιείται ήδη σε έγγραφα.");

                if (ModelState.IsValid)
                {
                    docOperatorService.Destroy(data);
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
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

            PopulateOperators();
            PopulateYears();
            PopulateMonths();
            PopulatePersonnel();

            return View();
        }

        #region MASTER GRID CRUD FUNCTIONS

        public ActionResult DocMetaboles_Read([DataSourceRequest] DataSourceRequest request, int schoolyearId = 0)
        {
            int stationId = (int)GetLoginStation().STATION_ID;

            IEnumerable<DocMetabolesViewModel> data = docMetabolesService.Read(schoolyearId, stationId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DocMetaboles_Create([DataSourceRequest] DataSourceRequest request, DocMetabolesViewModel data, int schoolyearId = 0)
        {
            int stationId = (int)GetLoginStation().STATION_ID;
            var newdata = new DocMetabolesViewModel();

            if (!(schoolyearId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε σχολικό έτος. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                docMetabolesService.Create(data, schoolyearId, stationId);
                newdata = docMetabolesService.Refresh(data.DOC_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DocMetaboles_Update([DataSourceRequest] DataSourceRequest request, DocMetabolesViewModel data, int schoolyearId = 0)
        {
            var newdata = new DocMetabolesViewModel();
            int stationId = (int)GetLoginStation().STATION_ID;

            if (!(schoolyearId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε σχολικό έτος. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                docMetabolesService.Update(data, schoolyearId, stationId);
                newdata = docMetabolesService.Refresh(data.DOC_ID);
            }

            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DocMetaboles_Destroy([DataSourceRequest] DataSourceRequest request, DocMetabolesViewModel data)
        {
            if (data != null)
            {
                if (ModelState.IsValid)
                {
                    docMetabolesService.Destroy(data);
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region DOCUMENT DATA FORM

        public ActionResult DocMetabolesEdit(int docId)
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

            DocMetabolesViewModel doc = docMetabolesService.GetRecord(docId);
            if (doc == null)
            {
                string msg = "Ο κωδικός του εγγράφου δεν είναι έγκυρος. Η εγγραφή πρέπει να έχει αποθηκευτεί πρώτα στο πλέγμα.";
                return RedirectToAction("ErrorData", "Document", new { notify = msg });
            }

            DocMetabolesViewModel data = docMetabolesService.GetRecord(docId);
            return View(data);
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
                ΕΓΓΡΑΦΟ_ΜΕΤΑΒΟΛΕΣ entity = docMetabolesService.SetRecord(data, docId, stationId);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                // Notify here
                this.ShowMessage(MessageType.Success, "Η αποθήκευση ολοκληρώθηκε με επιτυχία.");
                DocMetabolesViewModel newData = docMetabolesService.GetRecord(entity.DOC_ID);
                return View(newData);
            }
            this.ShowMessage(MessageType.Error, "Η αποθήκευση ακυρώθηκε λόγω σφαλμάτων καταχώρησης.");
            return View(data);
        }


        #endregion

        // Child Grid (METABOLES_REPORT DATA)
        public ActionResult MetabolesReport_Read([DataSourceRequest] DataSourceRequest request, int yearId = 0, int monthId = 0)
        {
            int stationId = (int)GetLoginStation().STATION_ID;

            List<MetabolesReportViewModel> data = docMetabolesService.ReadReport(yearId, monthId, stationId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
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

            PopulateOperators();
            PopulateYears();
            PopulateMonths();
            PopulatePersonnel();

            return View();
        }

        #region MASTER GRID CRUD FUNCTIONS

        public ActionResult DocOrologio_Read([DataSourceRequest] DataSourceRequest request, int schoolyearId = 0)
        {
            int stationId = (int)GetLoginStation().STATION_ID;

            IEnumerable<DocProgrammaViewModel> data = docOrologioService.Read(schoolyearId, stationId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DocOrologio_Create([DataSourceRequest] DataSourceRequest request, DocProgrammaViewModel data, int schoolyearId = 0)
        {
            var newdata = new DocProgrammaViewModel();
            int stationId = (int)GetLoginStation().STATION_ID;

            if (!(schoolyearId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε σχολικό έτος. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                docOrologioService.Create(data, schoolyearId, stationId);
                newdata = docOrologioService.Refresh(data.DOC_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DocOrologio_Update([DataSourceRequest] DataSourceRequest request, DocProgrammaViewModel data, int schoolyearId = 0)
        {
            var newdata = new DocProgrammaViewModel();
            int stationId = (int)GetLoginStation().STATION_ID;

            if (!(schoolyearId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε σχολικό έτος. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                docOrologioService.Update(data, schoolyearId, stationId);
                newdata = docOrologioService.Refresh(data.DOC_ID);
            }

            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DocOrologio_Destroy([DataSourceRequest] DataSourceRequest request, DocProgrammaViewModel data)
        {
            if (data != null)
            {
                if (ModelState.IsValid)
                {
                    docOrologioService.Destroy(data);
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
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

            DocProgrammaViewModel doc = docOrologioService.GetRecord(docId);
            if (doc == null)
            {
                string msg = "Ο κωδικός του εγγράφου δεν είναι έγκυρος. Η εγγραφή πρέπει να έχει αποθηκευτεί πρώτα στο πλέγμα.";
                return RedirectToAction("ErrorData", "Document", new { notify = msg });
            }

            DocProgrammaViewModel data = docOrologioService.GetRecord(docId);
            return View(data);
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
                ΕΓΓΡΑΦΟ_ΠΡΟΓΡΑΜΜΑ entity = docOrologioService.SetRecord(data, docId, stationId);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                // Notify here
                this.ShowMessage(MessageType.Success, "Η αποθήκευση ολοκληρώθηκε με επιτυχία.");
                DocProgrammaViewModel newData = docOrologioService.GetRecord(entity.DOC_ID);
                return View(newData);
            }
            this.ShowMessage(MessageType.Error, "Η αποθήκευση ακυρώθηκε λόγω σφαλμάτων καταχώρησης.");
            return View(data);
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
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;
            }
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
            PopulateSchoolYears();

            return View();
        }

        #region MASTER GRID CRUD FUNCTIONS

        public ActionResult Upload_Read([DataSourceRequest] DataSourceRequest request)
        {
            int stationId = (int)GetLoginStation().STATION_ID;

            List<DocUploadsViewModel> data = uploadService.Read(stationId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload_Create([DataSourceRequest] DataSourceRequest request, DocUploadsViewModel data)
        {
            int stationId = (int)GetLoginStation().STATION_ID;

            var newdata = new DocUploadsViewModel();

            if (!(stationId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε βρεφονηπιακό σταθμό. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                uploadService.Create(data, stationId);
                newdata = uploadService.Refresh(data.UPLOAD_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload_Update([DataSourceRequest] DataSourceRequest request, DocUploadsViewModel data)
        {
            int stationId = (int)GetLoginStation().STATION_ID;

            var newdata = new DocUploadsViewModel();

            if (!(stationId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε βρεφονηπιακό σταθμό. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                uploadService.Update(data, stationId);
                newdata = uploadService.Refresh(data.UPLOAD_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload_Destroy([DataSourceRequest] DataSourceRequest request, DocUploadsViewModel data)
        {
            if (data != null)
            {
                if (Kerberos.CanDeleteUpload(data.UPLOAD_ID))
                {
                    uploadService.Destroy(data);
                }
                else
                {
                    ModelState.AddModelError("", "Για να γίνει διαγραφή πρέπει πρώτα να διαγραφούν τα σχετικά αρχεία μεταφόρτωσης");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
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


        #endregion


        #region VALIDATORS

        public bool HoursExist()
        {
            var data = (from d in db.ΣΥΣ_ΩΡΕΣ select d).Count();
            if (data == 0) return false;

            return true;
        }

        public bool MonthsExist()
        {
            var data = (from d in db.ΣΥΣ_ΜΗΝΕΣ select d).Count();
            if (data == 0) return false;

            return true;
        }

        public bool OperatorsExist()
        {
            int stationId = (int)GetLoginStation().STATION_ID;

            var data = (from d in db.ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ where d.DOCSTATION_ID == stationId select d).Count();
            if (data == 0) return false;

            return true;
        }

        public bool PersonnelExists()
        {
            int stationId = (int)GetLoginStation().STATION_ID;

            var data = (from d in db.ΠΡΟΣΩΠΙΚΟ where d.ΒΝΣ == stationId select d).Count();
            if (data == 0) return false;

            return true;
        }

        public bool EducatorsExist()
        {
            int stationId = (int)GetLoginStation().STATION_ID;

            var data = (from d in db.ΠΡΟΣΩΠΙΚΟ where d.ΒΝΣ == stationId && d.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ == 1 select d).Count();
            if (data == 0) return false;

            return true;
        }

        public bool StationsExist()
        {
            var data = (from d in db.ΣΥΣ_ΣΤΑΘΜΟΙ select d).Count();
            if (data == 0) return false;

            return true;
        }

        public bool PersonnelTypesExist()
        {
            var data = (from d in db.ΣΥΣ_ΠΡΟΣΩΠΙΚΟ select d).Count();
            if (data == 0) return false;

            return true;
        }

        public bool TmimataExist()
        {
            var data = (from d in db.ΤΜΗΜΑ select d).Count();
            if (data == 0) return false;

            return true;
        }

        public bool SchoolYearsExist()
        {
            var data = (from d in db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ select d).Count();
            if (data == 0) return false;

            return true;
        }

        public bool TmimaChildrenExist()
        {
            var data = (from d in db.sqlCHILD_TMIMA_SELECTOR select d).Count();
            if (data == 0) return false;

            return true;
        }

        public bool ParousiesMonthExist()
        {
            var data = (from d in db.repΠΑΡΟΥΣΙΕΣ_ΜΗΝΑΣ select d).Count();
            if (data == 0) return false;

            return true;
        }

        public bool MetabolesTypesExist()
        {
            var data = (from d in db.ΣΥΣ_ΜΕΤΑΒΟΛΕΣ select d).Count();
            if (data == 0) return false;

            return true;
        }

        #endregion


        #region POPULATORS

        public void PopulateOperators()
        {
            int stationId = (int)GetLoginStation().STATION_ID;

            var data = (from d in db.ΕΓΓΡΑΦΟ_ΔΙΑΧΕΙΡΙΣΤΗΣ where d.DOCSTATION_ID == stationId orderby d.DOCADMIN_NAME select d).ToList();

            ViewData["operators"] = data;
            ViewData["defaultOperator"] = data.First().DOCADMIN_ID;
        }

        public void PopulateEmployees()
        {
            int stationId = (int)GetLoginStation().STATION_ID;

            var data = (from d in db.srcPERSONNEL_DATA where d.ΒΝΣ == stationId orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ select d).ToList();

            ViewData["personnel"] = data;
            ViewData["defaultPersonnel"] = data.First().PERSONNEL_ID;
        }

        public void PopulateYears()
        {
            var data = (from d in db.ΣΥΣ_ΕΤΗ orderby d.ΕΤΟΣ_ΚΩΔ select d).ToList();
            ViewData["years"] = data;
        }

        public void PopulateMonths()
        {
            var data = (from d in db.ΣΥΣ_ΜΗΝΕΣ orderby d.ΜΗΝΑΣ_ΚΩΔ select d).ToList();
            ViewData["months"] = data;
        }

        public void PopulateMetabolesTypes()
        {
            var data = (from d in db.ΣΥΣ_ΜΕΤΑΒΟΛΕΣ orderby d.METABOLI_TEXT select d).ToList();

            ViewData["metaboles_types"] = data;
        }

        public void PopulateHours()
        {
            var hours = (from d in db.ΣΥΣ_ΩΡΕΣ orderby d.HOUR_TEXT select d).ToList();

            ViewData["hours"] = hours;
        }

        public void PopulateChildren()
        {
            var data = (from d in db.sqlCHILD_TMIMA_SELECTOR orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ select d).ToList();

            ViewData["children"] = data;
            ViewData["defaultChild"] = data.First().CHILD_ID;
        }

        public void PopulatePersonnel()
        {
            int stationId = (int)GetLoginStation().STATION_ID;

            var data = (from d in db.sqlPERSONNEL_SELECTOR where d.ΒΝΣ == stationId orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ select d).ToList();

            ViewData["persons"] = data;
            ViewData["defaultPerson"] = data.First().PERSONNEL_ID;
        }

        public void PopulateEducators()
        {
            var educators = (from d in db.sqlPERSONNEL_SELECTOR where d.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ == 1 orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ select d).ToList();

            ViewData["educators"] = educators;
            ViewData["defaultEducator"] = educators.First().PERSONNEL_ID;
        }

        public void PopulatePersonnelTypes()
        {
            var personnelTypes = (from k in db.ΣΥΣ_ΠΡΟΣΩΠΙΚΟ select k).ToList();

            ViewData["person_types"] = personnelTypes;
        }

        public void PopulatePeriferiakes()
        {
            var data = (from d in db.ΣΥΣ_ΠΕΡΙΦΕΡΕΙΑΚΕΣ select d).ToList();
            ViewData["periferiakes"] = data;
        }

        public void PopulatePeriferies()
        {
            var data = (from d in db.ΣΥΣ_ΠΕΡΙΦΕΡΕΙΕΣ select d).ToList();
            ViewData["periferies"] = data;
        }

        public void PopulateGenders()
        {
            var data = (from d in db.ΣΥΣ_ΦΥΛΑ select d).ToList();
            ViewData["genders"] = data;
        }

        public void PopulateSchoolYears()
        {
            var data = (from d in db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ select d).ToList();
            ViewData["schoolYears"] = data;
        }

        public void PopulateStations()
        {
            var data = (from d in db.ΣΥΣ_ΣΤΑΘΜΟΙ orderby d.ΕΠΩΝΥΜΙΑ select d).ToList();
            ViewData["stations"] = data;
            ViewData["defaultStation"] = data.First().ΣΤΑΘΜΟΣ_ΚΩΔ;
        }

        public void PopulateKladoi()
        {
            var kladosTypes = (from k in db.ΣΥΣ_ΚΛΑΔΟΙ select k).ToList();

            ViewData["kladoi"] = kladosTypes;
        }

        public void PopulateTmimaCategories()
        {
            var categories = (from k in db.ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ select k).ToList();

            ViewData["tmima_categories"] = categories;
        }

        public void PopulateNumbersLatin()
        {
            var numbers = (from k in db.ΣΥΣ_ΑΑ_ΛΑΤΙΝΙΚΟΙ select k).ToList();

            ViewData["latin_numbers"] = numbers;
        }

        public void PopulateTmimata()
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
            int stationId = (int)GetLoginStation().STATION_ID;

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