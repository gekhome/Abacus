using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using Abacus.DAL;
using Abacus.Models;
using Abacus.BPM;
using Abacus.Notification;

namespace Abacus.Controllers.SysControllers
{
    public class ToolsController : Controller
    {
        private AbacusDBEntities db = new AbacusDBEntities();
        private USER_ADMINS loggedAdmin;
        Common c = new Common();
        Kerberos k = new Kerberos();

        public int selectedStationID;

        #region ΣΤΑΘΜΟΙ - ΣΤΟΙΧΕΙΑ (16-07-2019)

        public ActionResult StationDataList()
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

            populatePeriferiakes();
            return View();
        }

        public ActionResult Station_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = vmStationsFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Station_Create([DataSourceRequest] DataSourceRequest request, StationsGridViewModel data)
        {
            var newData = new StationsGridViewModel();

            var existingSchool = db.ΣΥΣ_ΣΤΑΘΜΟΙ.Where(s => s.ΕΠΩΝΥΜΙΑ == data.ΕΠΩΝΥΜΙΑ).ToList();

            if (existingSchool.Count > 0) ModelState.AddModelError("", "Ο σταθμός αυτός υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                ΣΥΣ_ΣΤΑΘΜΟΙ entity = new ΣΥΣ_ΣΤΑΘΜΟΙ()
                {
                    ΕΠΩΝΥΜΙΑ = data.ΕΠΩΝΥΜΙΑ,
                    ΠΕΡΙΦΕΡΕΙΑΚΗ = data.ΠΕΡΙΦΕΡΕΙΑΚΗ,
                    ΥΠΕΥΘΥΝΟΣ = data.ΥΠΕΥΘΥΝΟΣ
                };
                db.ΣΥΣ_ΣΤΑΘΜΟΙ.Add(entity);
                db.SaveChanges();
                data.ΣΤΑΘΜΟΣ_ΚΩΔ = entity.ΣΤΑΘΜΟΣ_ΚΩΔ;
                newData = RefreshStationFromDB(entity.ΣΤΑΘΜΟΣ_ΚΩΔ);
            }

            var result = new JsonResult();
            result.Data = new[] { newData }.ToDataSourceResult(request, ModelState);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Station_Update([DataSourceRequest] DataSourceRequest request, StationsGridViewModel data)
        {
            var newData = new StationsGridViewModel();

            if (data != null && ModelState.IsValid)
            {
                ΣΥΣ_ΣΤΑΘΜΟΙ entity = db.ΣΥΣ_ΣΤΑΘΜΟΙ.Find(data.ΣΤΑΘΜΟΣ_ΚΩΔ);
                entity.ΕΠΩΝΥΜΙΑ = data.ΕΠΩΝΥΜΙΑ;
                entity.ΠΕΡΙΦΕΡΕΙΑΚΗ = data.ΠΕΡΙΦΕΡΕΙΑΚΗ;
                entity.ΥΠΕΥΘΥΝΟΣ = data.ΥΠΕΥΘΥΝΟΣ;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newData = RefreshStationFromDB(entity.ΣΤΑΘΜΟΣ_ΚΩΔ);
            }

            var result = new JsonResult();
            result.Data = new[] { newData }.ToDataSourceResult(request, ModelState);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Station_Destroy([DataSourceRequest] DataSourceRequest request, StationsGridViewModel data)
        {
            if (data != null)
            {
                ΣΥΣ_ΣΤΑΘΜΟΙ entity = db.ΣΥΣ_ΣΤΑΘΜΟΙ.Find(data.ΣΤΑΘΜΟΣ_ΚΩΔ);
                if (entity != null)
                {
                    db.Entry(entity).State = EntityState.Deleted;
                    db.ΣΥΣ_ΣΤΑΘΜΟΙ.Remove(entity);
                    db.SaveChanges();
                }
            }
            var result = new JsonResult();
            result.Data = new[] { data }.ToDataSourceResult(request, ModelState);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public StationsGridViewModel RefreshStationFromDB(int stationId)
        {
            var data = (from d in db.ΣΥΣ_ΣΤΑΘΜΟΙ
                        where d.ΣΤΑΘΜΟΣ_ΚΩΔ == stationId
                        select new StationsGridViewModel
                        {
                            ΣΤΑΘΜΟΣ_ΚΩΔ = d.ΣΤΑΘΜΟΣ_ΚΩΔ,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΠΕΡΙΦΕΡΕΙΑΚΗ = d.ΠΕΡΙΦΕΡΕΙΑΚΗ,
                            ΥΠΕΥΘΥΝΟΣ = d.ΥΠΕΥΘΥΝΟΣ
                        }).FirstOrDefault();

            return data;
        }

        public List<StationsGridViewModel> vmStationsFromDB()
        {

            var data = (from d in db.ΣΥΣ_ΣΤΑΘΜΟΙ
                        orderby d.ΠΕΡΙΦΕΡΕΙΑΚΗ, d.ΕΠΩΝΥΜΙΑ
                        select new StationsGridViewModel
                        {
                            ΣΤΑΘΜΟΣ_ΚΩΔ = d.ΣΤΑΘΜΟΣ_ΚΩΔ,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΠΕΡΙΦΕΡΕΙΑΚΗ = d.ΠΕΡΙΦΕΡΕΙΑΚΗ,
                            ΥΠΕΥΘΥΝΟΣ = d.ΥΠΕΥΘΥΝΟΣ
                        }).ToList();

            return data;
        }

        #region ΒΝΣ DATA FORM (16-07-2019)

        public ActionResult StationEdit(int stationId)
        {
            //check if user is unauthenticated to redirect him
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

            StationsViewModel station = GetStationViewModelFromDB(stationId);
            if (station == null)
            {
                return HttpNotFound();
            }

            StationsViewModel stationData = GetStationViewModelFromDB(stationId);
            return View(stationData);
        }

        [HttpPost]
        public ActionResult StationEdit(int stationId, StationsViewModel svm)
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

            if (ModelState.IsValid)
            {
                ΣΥΣ_ΣΤΑΘΜΟΙ entity = db.ΣΥΣ_ΣΤΑΘΜΟΙ.Find(stationId);

                entity.ΕΠΩΝΥΜΙΑ = svm.ΕΠΩΝΥΜΙΑ.Trim();
                entity.ΓΡΑΜΜΑΤΕΙΑ = svm.ΓΡΑΜΜΑΤΕΙΑ;
                entity.ΥΠΕΥΘΥΝΟΣ = svm.ΥΠΕΥΘΥΝΟΣ.Trim();
                entity.ΥΠΕΥΘΥΝΟΣ_ΦΥΛΟ = svm.ΥΠΕΥΘΥΝΟΣ_ΦΥΛΟ;
                entity.ΤΑΧ_ΔΙΕΥΘΥΝΣΗ = svm.ΤΑΧ_ΔΙΕΥΘΥΝΣΗ.Trim();
                entity.ΤΗΛΕΦΩΝΑ = svm.ΤΗΛΕΦΩΝΑ;
                entity.ΦΑΞ = svm.ΦΑΞ;
                entity.EMAIL = svm.EMAIL.Trim();
                entity.ΚΙΝΗΤΟ = svm.ΚΙΝΗΤΟ;
                if (svm.ΑΝΑΠΛΗΡΩΤΗΣ != null) entity.ΑΝΑΠΛΗΡΩΤΗΣ = svm.ΑΝΑΠΛΗΡΩΤΗΣ.Trim();
                entity.ΑΝΑΠΛΗΡΩΤΗΣ_ΦΥΛΟ = svm.ΑΝΑΠΛΗΡΩΤΗΣ_ΦΥΛΟ;
                if (svm.ΔΙΑΧΕΙΡΙΣΤΗΣ != null) entity.ΔΙΑΧΕΙΡΙΣΤΗΣ = svm.ΔΙΑΧΕΙΡΙΣΤΗΣ.Trim();
                entity.ΔΙΑΧΕΙΡΙΣΤΗΣ_ΦΥΛΟ = svm.ΔΙΑΧΕΙΡΙΣΤΗΣ_ΦΥΛΟ;
                entity.ΠΕΡΙΦΕΡΕΙΑΚΗ = svm.ΠΕΡΙΦΕΡΕΙΑΚΗ;
                entity.ΠΕΡΙΦΕΡΕΙΑ = svm.ΠΕΡΙΦΕΡΕΙΑ;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                // Notify here
                this.ShowMessage(MessageType.Success, "Η αποθήκευση ολοκληρώθηκε με επιτυχία.");
                return View(svm);
            }
            this.ShowMessage(MessageType.Error, "Η αποθήκευση ακυρώθηκε λόγω σφαλμάτων καταχώρησης.");
            return View(svm);
        }

        public StationsViewModel GetStationViewModelFromDB(int stationId)
        {
            StationsViewModel stationData;

            stationData = (from s in db.ΣΥΣ_ΣΤΑΘΜΟΙ
                           where s.ΣΤΑΘΜΟΣ_ΚΩΔ == stationId
                           select new StationsViewModel
                            {
                                ΣΤΑΘΜΟΣ_ΚΩΔ = s.ΣΤΑΘΜΟΣ_ΚΩΔ,
                                ΕΠΩΝΥΜΙΑ = s.ΕΠΩΝΥΜΙΑ,
                                ΥΠΕΥΘΥΝΟΣ = s.ΥΠΕΥΘΥΝΟΣ,
                                ΥΠΕΥΘΥΝΟΣ_ΦΥΛΟ = s.ΥΠΕΥΘΥΝΟΣ_ΦΥΛΟ,
                                ΤΑΧ_ΔΙΕΥΘΥΝΣΗ = s.ΤΑΧ_ΔΙΕΥΘΥΝΣΗ,
                                ΤΗΛΕΦΩΝΑ = s.ΤΗΛΕΦΩΝΑ,
                                ΓΡΑΜΜΑΤΕΙΑ = s.ΓΡΑΜΜΑΤΕΙΑ,
                                ΦΑΞ = s.ΦΑΞ,
                                EMAIL = s.EMAIL,
                                ΚΙΝΗΤΟ = s.ΚΙΝΗΤΟ,
                                ΑΝΑΠΛΗΡΩΤΗΣ = s.ΑΝΑΠΛΗΡΩΤΗΣ,
                                ΑΝΑΠΛΗΡΩΤΗΣ_ΦΥΛΟ = s.ΑΝΑΠΛΗΡΩΤΗΣ_ΦΥΛΟ,
                                ΔΙΑΧΕΙΡΙΣΤΗΣ = s.ΔΙΑΧΕΙΡΙΣΤΗΣ,
                                ΔΙΑΧΕΙΡΙΣΤΗΣ_ΦΥΛΟ = s.ΔΙΑΧΕΙΡΙΣΤΗΣ_ΦΥΛΟ,
                                ΠΕΡΙΦΕΡΕΙΑΚΗ = s.ΠΕΡΙΦΕΡΕΙΑΚΗ,
                                ΠΕΡΙΦΕΡΕΙΑ = s.ΠΕΡΙΦΕΡΕΙΑ
                            }).FirstOrDefault();

            return stationData;
        }

        #endregion

        public ActionResult StationDataPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_ADMINS");
            }
            else
            {
                loggedAdmin = GetLoginAdmin();
                return View();
            }
        }

        public ActionResult StationDataPrint2()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_ADMINS");
            }
            else
            {
                loggedAdmin = GetLoginAdmin();
                return View();
            }
        }

        #endregion

        #region ΤΜΗΜΑΤΑ ΒΝΣ (19-07-2019)

        public ActionResult xStationTmimaList()
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
            populateSchoolYears();
            populateTmimaCategories();
            populateNumbersLatin();
            return View();
        }
        
        public List<sqlStationSelectorViewModel> GetStationSelectorFromDB()
        {

            var data = (from d in db.sqlSTATION_SELECTOR
                        orderby d.ΠΕΡΙΦΕΡΕΙΑΚΗ, d.ΕΠΩΝΥΜΙΑ
                        select new sqlStationSelectorViewModel
                        {
                            ΣΤΑΘΜΟΣ_ΚΩΔ = d.ΣΤΑΘΜΟΣ_ΚΩΔ,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΠΕΡΙΦΕΡΕΙΑΚΗ = d.ΠΕΡΙΦΕΡΕΙΑΚΗ,
                            ΕΠΩΝΥΜΙΑ_ΠΕΡΙΦΕΡΕΙΑ = d.ΕΠΩΝΥΜΙΑ_ΠΕΡΙΦΕΡΕΙΑ
                        }).ToList();

            return data;
        }

        public ActionResult StationSelector_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = GetStationSelectorFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        #region TMHMATA GRID CRUD FUNCTIONS

        public ActionResult Tmima_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0)
        {
            var data = GetTmimaViewModelFromDB(stationId);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Tmima_Create([DataSourceRequest] DataSourceRequest request, TmimaViewModel data, int stationId)
        {
            var newdata = new TmimaViewModel();

            if (stationId > 0)
            {
                if (data != null && ModelState.IsValid)
                {
                    ΤΜΗΜΑ entity = new ΤΜΗΜΑ()
                    {
                        ΒΝΣ = stationId,
                        ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                        ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ,
                        ΑΥΞΩΝ_ΑΡΙΘΜΟΣ = data.ΑΥΞΩΝ_ΑΡΙΘΜΟΣ,
                        ΧΑΡΑΚΤΗΡΙΣΜΟΣ = data.ΧΑΡΑΚΤΗΡΙΣΜΟΣ,
                        ΟΝΟΜΑΣΙΑ = c.BuildTmimaName(data)
                    };
                    db.ΤΜΗΜΑ.Add(entity);
                    db.SaveChanges();
                    data.ΤΜΗΜΑ_ΚΩΔ = entity.ΤΜΗΜΑ_ΚΩΔ;
                    newdata = RefreshTmimaFromDB(data.ΤΜΗΜΑ_ΚΩΔ);
                }
            }
            else
            {
                ModelState.AddModelError("", "Πρέπει να προηγηθεί επιλογή ένός σταθμού. Η καταχώρηση ακυρώθηκε.");
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Tmima_Update([DataSourceRequest] DataSourceRequest request, TmimaViewModel data, int stationId)
        {
            var newdata = new TmimaViewModel();

            if (stationId > 0)
            {
                if (data != null & ModelState.IsValid)
                {
                    ΤΜΗΜΑ entity = db.ΤΜΗΜΑ.Find(data.ΤΜΗΜΑ_ΚΩΔ);

                    entity.ΒΝΣ = stationId;
                    entity.ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
                    entity.ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ;
                    entity.ΑΥΞΩΝ_ΑΡΙΘΜΟΣ = data.ΑΥΞΩΝ_ΑΡΙΘΜΟΣ;
                    entity.ΧΑΡΑΚΤΗΡΙΣΜΟΣ = data.ΧΑΡΑΚΤΗΡΙΣΜΟΣ;
                    entity.ΟΝΟΜΑΣΙΑ = c.BuildTmimaName(data);

                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                    newdata = RefreshTmimaFromDB(entity.ΤΜΗΜΑ_ΚΩΔ);
                }
            }
            else
            {
                ModelState.AddModelError("", "Πρέπει πρώτα να γίνει επιλεγή ενός σταθμού. Η ενημέρωση ακυρώθηκε.");
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Tmima_Destroy([DataSourceRequest] DataSourceRequest request, TmimaViewModel data)
        {
            if (data != null)
            {
                if (k.CanDeleteTmima(data.ΤΜΗΜΑ_ΚΩΔ))
                {
                    ΤΜΗΜΑ entity = db.ΤΜΗΜΑ.Find(data.ΤΜΗΜΑ_ΚΩΔ);
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.ΤΜΗΜΑ.Remove(entity);
                        db.SaveChanges();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί το τμήμα αυτό διότι είναι σε χρήση.");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public TmimaViewModel RefreshTmimaFromDB(int recordId)
        {
            var data = (from d in db.ΤΜΗΜΑ
                        where d.ΤΜΗΜΑ_ΚΩΔ == recordId
                        select new TmimaViewModel
                        {
                            ΤΜΗΜΑ_ΚΩΔ = d.ΤΜΗΜΑ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΑΥΞΩΝ_ΑΡΙΘΜΟΣ = d.ΑΥΞΩΝ_ΑΡΙΘΜΟΣ,
                            ΧΑΡΑΚΤΗΡΙΣΜΟΣ = d.ΧΑΡΑΚΤΗΡΙΣΜΟΣ,
                            ΟΝΟΜΑΣΙΑ = d.ΟΝΟΜΑΣΙΑ
                        }).FirstOrDefault();
            return (data);
        }

        public List<TmimaViewModel> GetTmimaViewModelFromDB(int stationId)
        {
            var data = (from d in db.ΤΜΗΜΑ
                        where d.ΒΝΣ == stationId
                        orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ descending, d.ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ, d.ΑΥΞΩΝ_ΑΡΙΘΜΟΣ
                        select new TmimaViewModel
                        {
                            ΤΜΗΜΑ_ΚΩΔ = d.ΤΜΗΜΑ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΑΥΞΩΝ_ΑΡΙΘΜΟΣ = d.ΑΥΞΩΝ_ΑΡΙΘΜΟΣ,
                            ΧΑΡΑΚΤΗΡΙΣΜΟΣ = d.ΧΑΡΑΚΤΗΡΙΣΜΟΣ,
                            ΟΝΟΜΑΣΙΑ = d.ΟΝΟΜΑΣΙΑ
                        }).ToList();
            return (data);
        }

        #endregion

        #endregion TMHMATA GRID CRUD FUNCTIONS

        #region ΕΙΔΙΚΟΤΗΤΕΣ-ΚΛΑΔΟΙ ΠΡΟΣΩΠΙΚΟΥ (19-07-2019)

        public ActionResult xEidikotitesList()
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
            populateKladoi();
            List<EidikotitesViewModel> data = GetEidikotitesFromDB();
            return View(data);
        }

        public ActionResult Eidikotita_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = GetEidikotitesFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Eidikotita_Create([DataSourceRequest] DataSourceRequest request, EidikotitesViewModel data)
        {
            var newdata = new EidikotitesViewModel();

            var existingData = db.ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ.Where(s => s.EIDIKOTITA_TEXT == data.EIDIKOTITA_TEXT).ToList();
            if (existingData.Count > 0) ModelState.AddModelError("", "Η ειδικότητα αυτή υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ entity = new ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ()
                {
                    EIDIKOTITA_CODE = data.EIDIKOTITA_CODE,
                    EIDIKOTITA_TEXT = data.EIDIKOTITA_TEXT,
                    KLADOS = data.KLADOS
                };
                db.ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ.Add(entity);
                db.SaveChanges();
                data.EIDIKOTITA_ID = entity.EIDIKOTITA_ID;
                newdata = RefreshEidikotitaFromDB(data.EIDIKOTITA_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Eidikotita_Update([DataSourceRequest] DataSourceRequest request, EidikotitesViewModel data)
        {
            var newdata = new EidikotitesViewModel();

            if (data != null)
            {
                ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ entity = db.ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ.Find(data.EIDIKOTITA_ID);

                entity.EIDIKOTITA_CODE = data.EIDIKOTITA_CODE;
                entity.EIDIKOTITA_TEXT = data.EIDIKOTITA_TEXT;
                entity.KLADOS = data.KLADOS;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newdata = RefreshEidikotitaFromDB(entity.EIDIKOTITA_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Eidikotita_Destroy([DataSourceRequest] DataSourceRequest request, EidikotitesViewModel data)
        {
            if (data != null)
            {
                if (k.CanDeleteEidikotita(data.EIDIKOTITA_ID))
                {
                    ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ entity = db.ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ.Find(data.EIDIKOTITA_ID);
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ.Remove(entity);
                        db.SaveChanges();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί η ειδικότητα διότι είναι σε χρήση.");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public EidikotitesViewModel RefreshEidikotitaFromDB(int recordId)
        {
            var data = (from d in db.ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ
                        where d.EIDIKOTITA_ID == recordId
                        select new EidikotitesViewModel
                        {
                            EIDIKOTITA_ID = d.EIDIKOTITA_ID,
                            EIDIKOTITA_CODE = d.EIDIKOTITA_CODE,
                            EIDIKOTITA_TEXT = d.EIDIKOTITA_TEXT,
                            KLADOS = d.KLADOS ?? 0
                        }).FirstOrDefault();

            return data;
        }

        public List<EidikotitesViewModel> GetEidikotitesFromDB()
        {
            var data = (from d in db.ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ
                        orderby d.KLADOS, d.EIDIKOTITA_TEXT
                        select new EidikotitesViewModel
                        {
                            EIDIKOTITA_ID = d.EIDIKOTITA_ID,
                            EIDIKOTITA_CODE = d.EIDIKOTITA_CODE,
                            EIDIKOTITA_TEXT = d.EIDIKOTITA_TEXT,
                            KLADOS = d.KLADOS ?? 0
                        }).ToList();

            return data;
        }

        public ActionResult xEidikotitesPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_ADMINS");
            }
            else
            {
                return View();
            }
        }


        #endregion ΝΕΕΣ ΕΙΔΙΚΟΤΗΤΕΣ ΕΚΠΑΙΔΕΥΤΩΝ

        #region ΣΧΟΛΙΚΑ ΕΤΗ (16-07-2019)

        public ActionResult xSchoolYearsList()
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

            List<SysSchoolYearViewModel> vms = vmSchoolYearsFromDB();
            if (vms == null)
            {
                List<SysSchoolYearViewModel> newvms = new List<SysSchoolYearViewModel>();
                return View(newvms);
            }
            return View(vms);
        }

        public ActionResult SchoolYear_Read([DataSourceRequest] DataSourceRequest request)
        {
            var vmt = vmSchoolYearsFromDB();

            var result = new JsonResult();
            result.Data = vmt.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SchoolYear_Create([DataSourceRequest] DataSourceRequest request, SysSchoolYearViewModel data)
        {
            var newData = new SysSchoolYearViewModel();

            var existingdata = db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ.Where(s => s.ΣΧΟΛΙΚΟ_ΕΤΟΣ == data.ΣΧΟΛΙΚΟ_ΕΤΟΣ).ToList();

            if (existingdata.Count > 0) ModelState.AddModelError("", "Αυτό το σχολικό έτος υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ entity = new ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ()
                {
                    ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                    ΗΜΝΙΑ_ΕΝΑΡΞΗ = data.ΗΜΝΙΑ_ΕΝΑΡΞΗ,
                    ΗΜΝΙΑ_ΛΗΞΗ = data.ΗΜΝΙΑ_ΛΗΞΗ,
                    ΤΡΕΧΟΝ_ΕΤΟΣ = data.ΤΡΕΧΟΝ_ΕΤΟΣ
                };
                db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ.Add(entity);
                db.SaveChanges();
                data.SCHOOLYEAR_ID = entity.SCHOOLYEAR_ID;
                if (!ValidateCurrentSchoolYear())
                {
                    ModelState.AddModelError("", "Πρέπει να οριστεί ένα και μόνο ένα σχολικό έτος ως το τρέχον.");
                }
                newData = RefreshSchoolYearFromDB(entity.SCHOOLYEAR_ID);
            }

            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SchoolYear_Update([DataSourceRequest] DataSourceRequest request, SysSchoolYearViewModel data)
        {
            var newData = new SysSchoolYearViewModel();

            if (data != null && ModelState.IsValid)
            {
                ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ entity = db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ.Find(data.SCHOOLYEAR_ID);
                entity.SCHOOLYEAR_ID = data.SCHOOLYEAR_ID;
                entity.ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
                entity.ΗΜΝΙΑ_ΕΝΑΡΞΗ = data.ΗΜΝΙΑ_ΕΝΑΡΞΗ;
                entity.ΗΜΝΙΑ_ΛΗΞΗ = data.ΗΜΝΙΑ_ΛΗΞΗ;
                entity.ΤΡΕΧΟΝ_ΕΤΟΣ = data.ΤΡΕΧΟΝ_ΕΤΟΣ;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                if (!ValidateCurrentSchoolYear())
                {
                    ModelState.AddModelError("", "Πρέπει να οριστεί ένα και μόνο ένα σχολικό έτος ως το τρέχον.");
                }
                newData = RefreshSchoolYearFromDB(entity.SCHOOLYEAR_ID);
            }

            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SchoolYear_Destroy([DataSourceRequest] DataSourceRequest request, SysSchoolYearViewModel data)
        {
            if (data != null)
            {
                if (k.CanDeleteSchoolYear(data.SCHOOLYEAR_ID))
                {
                    ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ entity = db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ.Find(data.SCHOOLYEAR_ID);
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ.Remove(entity);
                        db.SaveChanges();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί το σχολικό έτος διότι είναι σε χρήση.");
                }
            }
            var result = new JsonResult();
            result.Data = new[] { data }.ToDataSourceResult(request, ModelState);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public SysSchoolYearViewModel RefreshSchoolYearFromDB(int schoolyearId)
        {
            var data = (from d in db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ
                        where d.SCHOOLYEAR_ID == schoolyearId
                        orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ
                        select new SysSchoolYearViewModel
                        {
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΗΜΝΙΑ_ΕΝΑΡΞΗ = d.ΗΜΝΙΑ_ΕΝΑΡΞΗ,
                            ΗΜΝΙΑ_ΛΗΞΗ = d.ΗΜΝΙΑ_ΛΗΞΗ,
                            ΤΡΕΧΟΝ_ΕΤΟΣ = d.ΤΡΕΧΟΝ_ΕΤΟΣ ?? false
                        }).FirstOrDefault();

            return data;
        }

        public List<SysSchoolYearViewModel> vmSchoolYearsFromDB()
        {

            var data = (from d in db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ
                        orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ
                        select new SysSchoolYearViewModel
                        {
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΗΜΝΙΑ_ΕΝΑΡΞΗ = d.ΗΜΝΙΑ_ΕΝΑΡΞΗ,
                            ΗΜΝΙΑ_ΛΗΞΗ = d.ΗΜΝΙΑ_ΛΗΞΗ,
                            ΤΡΕΧΟΝ_ΕΤΟΣ = d.ΤΡΕΧΟΝ_ΕΤΟΣ ?? false
                        }).ToList();

            return data;
        }

        public bool ValidateCurrentSchoolYear()
        {
            var data = (from d in db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ where d.ΤΡΕΧΟΝ_ΕΤΟΣ == true select d).ToList();
            if (data.Count > 1) return false;
            else if (data.Count == 0) return false;
            else return true;
        }

        #endregion

        #region ΩΡΕΣ ΠΡΟΣΕΛΕΥΣΗΣ-ΑΠΟΧΩΡΗΣΗΣ (18-07-2019)

        public ActionResult xHoursList()
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

            List<HoursViewModel> vms = vmHoursFromDB();
            if (vms == null)
            {
                List<HoursViewModel> newvms = new List<HoursViewModel>();
                return View(newvms);
            }
            return View(vms);
        }

        public ActionResult Hours_Read([DataSourceRequest] DataSourceRequest request)
        {
            var vmt = vmHoursFromDB();

            var result = new JsonResult();
            result.Data = vmt.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Hours_Create([DataSourceRequest] DataSourceRequest request, HoursViewModel data)
        {
            var newData = new HoursViewModel();

            var existingdata = db.ΣΥΣ_ΩΡΕΣ.Where(s => s.HOUR_TEXT == data.HOUR_TEXT).ToList();

            if (existingdata.Count > 0) ModelState.AddModelError("", "Αυτή η ώρα υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                ΣΥΣ_ΩΡΕΣ entity = new ΣΥΣ_ΩΡΕΣ()
                {
                    HOUR_TEXT = data.HOUR_TEXT,
                };
                db.ΣΥΣ_ΩΡΕΣ.Add(entity);
                db.SaveChanges();
                data.HOUR_ID = entity.HOUR_ID;
                newData = RefreshHourFromDB(entity.HOUR_ID);
            }

            var result = new JsonResult();
            result.Data = new[] { newData }.ToDataSourceResult(request, ModelState);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Hours_Update([DataSourceRequest] DataSourceRequest request, HoursViewModel data)
        {
            var newData = new HoursViewModel();

            if (data != null && ModelState.IsValid)
            {
                ΣΥΣ_ΩΡΕΣ entity = db.ΣΥΣ_ΩΡΕΣ.Find(data.HOUR_ID);
                entity.HOUR_TEXT = data.HOUR_TEXT;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newData = RefreshHourFromDB(entity.HOUR_ID);
            }

            var result = new JsonResult();
            result.Data = new[] { newData }.ToDataSourceResult(request, ModelState);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Hours_Destroy([DataSourceRequest] DataSourceRequest request, HoursViewModel data)
        {
            if (data != null)
            {
                if (k.CanDeleteHour(data.HOUR_ID))
                {
                    ΣΥΣ_ΩΡΕΣ entity = db.ΣΥΣ_ΩΡΕΣ.Find(data.HOUR_ID);
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.ΣΥΣ_ΩΡΕΣ.Remove(entity);
                        db.SaveChanges();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Η διαγραφή ωρών είναι απενεργοποιημένη.");
                }
            }
            var result = new JsonResult();
            result.Data = new[] { data }.ToDataSourceResult(request, ModelState);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public HoursViewModel RefreshHourFromDB(int hourId)
        {
            var data = (from d in db.ΣΥΣ_ΩΡΕΣ
                        where d.HOUR_ID == hourId
                        select new HoursViewModel
                        {
                            HOUR_ID = d.HOUR_ID,
                            HOUR_TEXT = d.HOUR_TEXT,
                        }).FirstOrDefault();

            return data;
        }

        public List<HoursViewModel> vmHoursFromDB()
        {

            var data = (from d in db.ΣΥΣ_ΩΡΕΣ
                        orderby d.HOUR_TEXT
                        select new HoursViewModel
                        {
                            HOUR_ID = d.HOUR_ID,
                            HOUR_TEXT = d.HOUR_TEXT,
                        }).ToList();

            return data;
        }

        #endregion

        #region ΚΑΤΗΓΟΡΙΕΣ ΤΜΗΜΑΤΩΝ (19-07-2019)

        public ActionResult xTmimaCategories()
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

            List<TmimaCategoryViewModel> data = GetTmimaCategoriesFromDB();
            return View(data);
        }

        public ActionResult TmimaCategory_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = GetTmimaCategoriesFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TmimaCategory_Create([DataSourceRequest] DataSourceRequest request, TmimaCategoryViewModel data)
        {
            var newData = new TmimaCategoryViewModel();

            var existingdata = db.ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ.Where(s => s.CATEGORY_TEXT == data.CATEGORY_TEXT).ToList();

            if (existingdata.Count > 0) ModelState.AddModelError("", "Αυτή η κατηγορία τμήματος υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ entity = new ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ()
                {
                    CATEGORY_TEXT = data.CATEGORY_TEXT,
                    AGE_START = data.AGE_START,
                    AGE_END = data.AGE_END
                };
                db.ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ.Add(entity);
                db.SaveChanges();
                data.CATEGORY_ID = entity.CATEGORY_ID;
                newData = RefreshTmimaCategoryFromDB(entity.CATEGORY_ID);
            }

            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TmimaCategory_Update([DataSourceRequest] DataSourceRequest request, TmimaCategoryViewModel data)
        {
            var newData = new TmimaCategoryViewModel();

            if (data != null && ModelState.IsValid)
            {
                ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ entity = db.ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ.Find(data.CATEGORY_ID);
                entity.CATEGORY_TEXT = data.CATEGORY_TEXT;
                entity.AGE_START = data.AGE_START;
                entity.AGE_END = data.AGE_END;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newData = RefreshTmimaCategoryFromDB(entity.CATEGORY_ID);
            }

            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TmimaCategory_Destroy([DataSourceRequest] DataSourceRequest request, TmimaCategoryViewModel data)
        {
            if (data != null)
            {
                if (k.CanDeleteTmimaCategory(data.CATEGORY_ID))
                {
                    ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ entity = db.ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ.Find(data.CATEGORY_ID);
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ.Remove(entity);
                        db.SaveChanges();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Η διαγραφή κατηγοριών τμημάτων είναι απενεργοποιημένη.");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public TmimaCategoryViewModel RefreshTmimaCategoryFromDB(int recordId)
        {
            var data = (from d in db.ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ
                        where d.CATEGORY_ID == recordId
                        select new TmimaCategoryViewModel
                        {
                            CATEGORY_ID = d.CATEGORY_ID,
                            CATEGORY_TEXT = d.CATEGORY_TEXT,
                            AGE_START = d.AGE_START,
                            AGE_END = d.AGE_END
                        }).FirstOrDefault();

            return data;
        }

        public List<TmimaCategoryViewModel> GetTmimaCategoriesFromDB()
        {

            var data = (from d in db.ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ
                        orderby d.CATEGORY_ID
                        select new TmimaCategoryViewModel
                        {
                            CATEGORY_ID = d.CATEGORY_ID,
                            CATEGORY_TEXT = d.CATEGORY_TEXT,
                            AGE_START = d.AGE_START,
                            AGE_END = d.AGE_END
                        }).ToList();

            return data;
        }

        #endregion

        #region ΚΑΤΗΓΟΡΙΕΣ ΠΡΟΣΩΠΙΚΟΥ (19-07-2019)

        public ActionResult xPersonnelTypes()
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
            return View();
        }

        public ActionResult PersonnelType_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = GetPersonnelTypeFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PersonnelType_Create([DataSourceRequest] DataSourceRequest request, PersonnelTypeViewModel data)
        {
            var newData = new PersonnelTypeViewModel();

            var existingdata = db.ΣΥΣ_ΠΡΟΣΩΠΙΚΟ.Where(s => s.PROSOPIKO_TEXT == data.PROSOPIKO_TEXT).ToList();

            if (existingdata.Count > 0) ModelState.AddModelError("", "Αυτή η κατηγορία προσωπικού υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                ΣΥΣ_ΠΡΟΣΩΠΙΚΟ entity = new ΣΥΣ_ΠΡΟΣΩΠΙΚΟ()
                {
                    PROSOPIKO_TEXT = data.PROSOPIKO_TEXT,
                };
                db.ΣΥΣ_ΠΡΟΣΩΠΙΚΟ.Add(entity);
                db.SaveChanges();
                data.PROSOPIKO_ID = entity.PROSOPIKO_ID;
                newData = RefreshPersonnelTypeFromDB(entity.PROSOPIKO_ID);
            }

            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PersonnelType_Update([DataSourceRequest] DataSourceRequest request, PersonnelTypeViewModel data)
        {
            var newData = new PersonnelTypeViewModel();

            if (data != null && ModelState.IsValid)
            {
                ΣΥΣ_ΠΡΟΣΩΠΙΚΟ entity = db.ΣΥΣ_ΠΡΟΣΩΠΙΚΟ.Find(data.PROSOPIKO_ID);
                entity.PROSOPIKO_TEXT = data.PROSOPIKO_TEXT;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newData = RefreshPersonnelTypeFromDB(entity.PROSOPIKO_ID);
            }

            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PersonnelType_Destroy([DataSourceRequest] DataSourceRequest request, PersonnelTypeViewModel data)
        {
            if (data != null)
            {
                if (k.CanDeletePersonnelType(data.PROSOPIKO_ID))
                {
                    ΣΥΣ_ΠΡΟΣΩΠΙΚΟ entity = db.ΣΥΣ_ΠΡΟΣΩΠΙΚΟ.Find(data.PROSOPIKO_ID);
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.ΣΥΣ_ΠΡΟΣΩΠΙΚΟ.Remove(entity);
                        db.SaveChanges();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Η διαγραφή κατηγοριών προσωπικού είναι απενεργοποιημένη.");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public PersonnelTypeViewModel RefreshPersonnelTypeFromDB(int recordId)
        {
            var data = (from d in db.ΣΥΣ_ΠΡΟΣΩΠΙΚΟ
                        where d.PROSOPIKO_ID == recordId
                        select new PersonnelTypeViewModel
                        {
                            PROSOPIKO_ID = d.PROSOPIKO_ID,
                            PROSOPIKO_TEXT = d.PROSOPIKO_TEXT
                        }).FirstOrDefault();

            return data;
        }

        public List<PersonnelTypeViewModel> GetPersonnelTypeFromDB()
        {
            var data = (from d in db.ΣΥΣ_ΠΡΟΣΩΠΙΚΟ
                        select new PersonnelTypeViewModel
                        {
                            PROSOPIKO_ID = d.PROSOPIKO_ID,
                            PROSOPIKO_TEXT = d.PROSOPIKO_TEXT
                        }).ToList();

            return data;
        }

        #endregion

        #region ΕΙΔΗ ΜΕΤΑΒΟΛΩΝ ΠΡΟΣΩΠΙΚΟΥ (27-8-2019)

        public ActionResult xMetabolesTypes()
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
            return View();
        }

        public ActionResult MetaboliType_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = GetMetabolesTypesFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MetaboliType_Create([DataSourceRequest] DataSourceRequest request, SysMetabolesViewModel data)
        {
            var newData = new SysMetabolesViewModel();

            var existingdata = db.ΣΥΣ_ΜΕΤΑΒΟΛΕΣ.Where(s => s.METABOLI_TEXT == data.METABOLI_TEXT).ToList();

            if (existingdata.Count > 0) ModelState.AddModelError("", "Αυτή η κατηγορία προσωπικού υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                ΣΥΣ_ΜΕΤΑΒΟΛΕΣ entity = new ΣΥΣ_ΜΕΤΑΒΟΛΕΣ()
                {
                    METABOLI_TEXT = data.METABOLI_TEXT,
                };
                db.ΣΥΣ_ΜΕΤΑΒΟΛΕΣ.Add(entity);
                db.SaveChanges();
                data.METABOLI_ID = entity.METABOLI_ID;
                newData = RefreshMetaboliTypeFromDB(entity.METABOLI_ID);
            }

            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MetaboliType_Update([DataSourceRequest] DataSourceRequest request, SysMetabolesViewModel data)
        {
            var newData = new SysMetabolesViewModel();

            if (data != null && ModelState.IsValid)
            {
                ΣΥΣ_ΜΕΤΑΒΟΛΕΣ entity = db.ΣΥΣ_ΜΕΤΑΒΟΛΕΣ.Find(data.METABOLI_ID);
                entity.METABOLI_TEXT = data.METABOLI_TEXT;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newData = RefreshMetaboliTypeFromDB(entity.METABOLI_ID);
            }

            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MetaboliType_Destroy([DataSourceRequest] DataSourceRequest request, SysMetabolesViewModel data)
        {
            if (data != null)
            {
                if (k.CanDeleteMetaboliType(data.METABOLI_ID))
                {
                    ΣΥΣ_ΜΕΤΑΒΟΛΕΣ entity = db.ΣΥΣ_ΜΕΤΑΒΟΛΕΣ.Find(data.METABOLI_ID);
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.ΣΥΣ_ΜΕΤΑΒΟΛΕΣ.Remove(entity);
                        db.SaveChanges();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Η διαγραφή ειδών μεταβολών προσωπικού είναι απενεργοποιημένη.");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public SysMetabolesViewModel RefreshMetaboliTypeFromDB(int recordId)
        {
            var data = (from d in db.ΣΥΣ_ΜΕΤΑΒΟΛΕΣ
                        where d.METABOLI_ID == recordId
                        select new SysMetabolesViewModel
                        {
                            METABOLI_ID = d.METABOLI_ID,
                            METABOLI_TEXT = d.METABOLI_TEXT
                        }).FirstOrDefault();

            return data;
        }

        public List<SysMetabolesViewModel> GetMetabolesTypesFromDB()
        {
            var data = (from d in db.ΣΥΣ_ΜΕΤΑΒΟΛΕΣ
                        orderby d.METABOLI_TEXT
                        select new SysMetabolesViewModel
                        {
                            METABOLI_ID = d.METABOLI_ID,
                            METABOLI_TEXT = d.METABOLI_TEXT
                        }).ToList();

            return data;
        }

        #endregion

        #region ΕΙΔΗ ΑΠΟΧΩΡΗΣΕΩΝ ΠΡΟΣΩΠΙΚΟΥ (27-02-2020)

        public ActionResult xApoxorisiTypes()
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
            return View();
        }

        public ActionResult Apoxorisi_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = GetApoxorisiTypesFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Apoxorisi_Create([DataSourceRequest] DataSourceRequest request, SysApoxorisiViewModel data)
        {
            var newData = new SysApoxorisiViewModel();

            var existingdata = db.ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ.Where(s => s.ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ == data.ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ).ToList();

            if (existingdata.Count > 0) ModelState.AddModelError("", "Αυτή η κατηγορία προσωπικού υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ entity = new ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ()
                {
                    ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ = data.ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ,
                };
                db.ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ.Add(entity);
                db.SaveChanges();
                data.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ = entity.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ;
                newData = RefreshApoxorisiTypeFromDB(entity.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ);
            }

            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Apoxorisi_Update([DataSourceRequest] DataSourceRequest request, SysApoxorisiViewModel data)
        {
            var newData = new SysApoxorisiViewModel();

            if (data != null && ModelState.IsValid)
            {
                ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ entity = db.ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ.Find(data.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ);
                entity.ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ = data.ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newData = RefreshApoxorisiTypeFromDB(entity.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ);
            }

            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Apoxorisi_Destroy([DataSourceRequest] DataSourceRequest request, SysApoxorisiViewModel data)
        {
            if (data != null)
            {
                if (k.CanDeleteApoxorisiType(data.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ))
                {
                    ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ entity = db.ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ.Find(data.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ);
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ.Remove(entity);
                        db.SaveChanges();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Η διαγραφή δεν είναι δυνατή γιατί η τιμή αυτή χρησιμοποιείται ήδη.");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public SysApoxorisiViewModel RefreshApoxorisiTypeFromDB(int recordId)
        {
            var data = (from d in db.ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ
                        where d.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ == recordId
                        select new SysApoxorisiViewModel
                        {
                            ΑΠΟΧΩΡΗΣΗ_ΚΩΔ = d.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ,
                            ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ = d.ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ
                        }).FirstOrDefault();

            return data;
        }

        public List<SysApoxorisiViewModel> GetApoxorisiTypesFromDB()
        {
            var data = (from d in db.ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ
                        orderby d.ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ
                        select new SysApoxorisiViewModel
                        {
                            ΑΠΟΧΩΡΗΣΗ_ΚΩΔ = d.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ,
                            ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ = d.ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ
                        }).ToList();

            return data;
        }

        #endregion


        #region ΠΕΡΙΦΕΡΕΙΑΚΕΣ ΔΙΕΥΘΥΝΣΕΙΣ (16-07-2019)

        public ActionResult xPeriferiakesList()
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

            List<SysPeriferiakiViewModel> vms = vmPeriferiakesFromDB();
            if (vms == null)
            {
                List<SysPeriferiakiViewModel> newvms = new List<SysPeriferiakiViewModel>();
                return View(newvms);
            }
            return View(vms);
        }

        public ActionResult Periferiaki_Read([DataSourceRequest] DataSourceRequest request)
        {
            var vmt = vmPeriferiakesFromDB();

            var result = new JsonResult();
            result.Data = vmt.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Perifariaki_Create([DataSourceRequest] DataSourceRequest request, SysPeriferiakiViewModel data)
        {
            var newData = new SysPeriferiakiViewModel();

            var existingdata = db.ΣΥΣ_ΠΕΡΙΦΕΡΕΙΑΚΕΣ.Where(s => s.ΕΠΩΝΥΜΙΑ_ΠΕΡΙΦΕΡΕΙΑ == data.ΕΠΩΝΥΜΙΑ_ΠΕΡΙΦΕΡΕΙΑ).ToList();

            if (existingdata.Count > 0) ModelState.AddModelError("", "Αυτή η περιφερειακή υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                ΣΥΣ_ΠΕΡΙΦΕΡΕΙΑΚΕΣ entity = new ΣΥΣ_ΠΕΡΙΦΕΡΕΙΑΚΕΣ()
                {
                    ΕΠΩΝΥΜΙΑ_ΠΕΡΙΦΕΡΕΙΑ = data.ΕΠΩΝΥΜΙΑ_ΠΕΡΙΦΕΡΕΙΑ,
                    ΤΑΧ_ΔΙΕΥΘΥΝΣΗ = data.ΤΑΧ_ΔΙΕΥΘΥΝΣΗ,
                    ΕΔΡΑ = data.ΕΔΡΑ,
                    ΤΜΗΜΑ = data.ΤΜΗΜΑ,
                    EMAIL = data.EMAIL
                };
                db.ΣΥΣ_ΠΕΡΙΦΕΡΕΙΑΚΕΣ.Add(entity);
                db.SaveChanges();
                data.ΚΩΔΙΚΟΣ_ΠΕΡΙΦΕΡΕΙΑ = entity.ΚΩΔΙΚΟΣ_ΠΕΡΙΦΕΡΕΙΑ;
                newData = RefreshPeriferiakiFromDB(entity.ΚΩΔΙΚΟΣ_ΠΕΡΙΦΕΡΕΙΑ);
            }

            var result = new JsonResult();
            result.Data = new[] { newData }.ToDataSourceResult(request, ModelState);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Periferiaki_Update([DataSourceRequest] DataSourceRequest request, SysPeriferiakiViewModel data)
        {
            var newData = new SysPeriferiakiViewModel();

            if (data != null && ModelState.IsValid)
            {
                ΣΥΣ_ΠΕΡΙΦΕΡΕΙΑΚΕΣ entity = db.ΣΥΣ_ΠΕΡΙΦΕΡΕΙΑΚΕΣ.Find(data.ΚΩΔΙΚΟΣ_ΠΕΡΙΦΕΡΕΙΑ);

                entity.ΚΩΔΙΚΟΣ_ΠΕΡΙΦΕΡΕΙΑ = data.ΚΩΔΙΚΟΣ_ΠΕΡΙΦΕΡΕΙΑ;
                entity.ΕΠΩΝΥΜΙΑ_ΠΕΡΙΦΕΡΕΙΑ = data.ΕΠΩΝΥΜΙΑ_ΠΕΡΙΦΕΡΕΙΑ;
                entity.ΤΑΧ_ΔΙΕΥΘΥΝΣΗ = data.ΤΑΧ_ΔΙΕΥΘΥΝΣΗ;
                entity.ΕΔΡΑ = data.ΕΔΡΑ;
                entity.ΤΜΗΜΑ = data.ΤΜΗΜΑ;
                entity.EMAIL = data.EMAIL;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newData = RefreshPeriferiakiFromDB(entity.ΚΩΔΙΚΟΣ_ΠΕΡΙΦΕΡΕΙΑ);
            }

            var result = new JsonResult();
            result.Data = new[] { newData }.ToDataSourceResult(request, ModelState);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Periferiaki_Destroy([DataSourceRequest] DataSourceRequest request, SysPeriferiakiViewModel data)
        {
            if (data != null)
            {
                if (k.CanDeletePeriferiaki())
                {
                    ΣΥΣ_ΠΕΡΙΦΕΡΕΙΑΚΕΣ entity = db.ΣΥΣ_ΠΕΡΙΦΕΡΕΙΑΚΕΣ.Find(data.ΚΩΔΙΚΟΣ_ΠΕΡΙΦΕΡΕΙΑ);
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.ΣΥΣ_ΠΕΡΙΦΕΡΕΙΑΚΕΣ.Remove(entity);
                        db.SaveChanges();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Η διαγραφή Περιφερειακών είναι απενεργοποιημένη.");
                }
            }
            var result = new JsonResult();
            result.Data = new[] { data }.ToDataSourceResult(request, ModelState);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public SysPeriferiakiViewModel RefreshPeriferiakiFromDB(int recordId)
        {
            var data = (from d in db.ΣΥΣ_ΠΕΡΙΦΕΡΕΙΑΚΕΣ
                        where d.ΚΩΔΙΚΟΣ_ΠΕΡΙΦΕΡΕΙΑ == recordId
                        select new SysPeriferiakiViewModel
                        {
                            ΚΩΔΙΚΟΣ_ΠΕΡΙΦΕΡΕΙΑ = d.ΚΩΔΙΚΟΣ_ΠΕΡΙΦΕΡΕΙΑ,
                            ΕΠΩΝΥΜΙΑ_ΠΕΡΙΦΕΡΕΙΑ = d.ΕΠΩΝΥΜΙΑ_ΠΕΡΙΦΕΡΕΙΑ,
                            ΤΑΧ_ΔΙΕΥΘΥΝΣΗ = d.ΤΑΧ_ΔΙΕΥΘΥΝΣΗ,
                            ΤΗΛΕΦΩΝΑ = d.ΤΗΛΕΦΩΝΑ,
                            FAX = d.FAX,
                            EMAIL = d.EMAIL,
                            ΕΔΡΑ = d.ΕΔΡΑ,
                            ΤΜΗΜΑ = d.ΤΜΗΜΑ
                        }).FirstOrDefault();

            return data;
        }

        public List<SysPeriferiakiViewModel> vmPeriferiakesFromDB()
        {

            var data = (from d in db.ΣΥΣ_ΠΕΡΙΦΕΡΕΙΑΚΕΣ
                        orderby d.ΚΩΔΙΚΟΣ_ΠΕΡΙΦΕΡΕΙΑ
                        select new SysPeriferiakiViewModel
                        {
                            ΚΩΔΙΚΟΣ_ΠΕΡΙΦΕΡΕΙΑ = d.ΚΩΔΙΚΟΣ_ΠΕΡΙΦΕΡΕΙΑ,
                            ΕΠΩΝΥΜΙΑ_ΠΕΡΙΦΕΡΕΙΑ = d.ΕΠΩΝΥΜΙΑ_ΠΕΡΙΦΕΡΕΙΑ,
                            ΤΑΧ_ΔΙΕΥΘΥΝΣΗ = d.ΤΑΧ_ΔΙΕΥΘΥΝΣΗ,
                            ΤΗΛΕΦΩΝΑ = d.ΤΗΛΕΦΩΝΑ,
                            FAX = d.FAX,
                            EMAIL = d.EMAIL,
                            ΕΔΡΑ = d.ΕΔΡΑ,
                            ΤΜΗΜΑ = d.ΤΜΗΜΑ
                        }).ToList();

            return data;
        }


        #endregion

        #region ΠΕΡΙΦΕΡΕΙΕΣ-ΔΗΜΟΙ (16-07-2019)

        public ActionResult xPeriferiesDimoi()
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
            return View();
        }

        public ActionResult Periferies([DataSourceRequest] DataSourceRequest request)
        {
            var periferies = db.ΣΥΣ_ΠΕΡΙΦΕΡΕΙΕΣ.Select(p => new PeriferiaViewModel()
            {
                PERIFERIA_ID = p.PERIFERIA_ID,
                PERIFERIA_NAME = p.PERIFERIA_NAME
            });
            return Json(periferies.ToDataSourceResult(request));
        }

        public ActionResult Dimoi([DataSourceRequest] DataSourceRequest request, int periferiaId)
        {
            var dimoi = db.ΣΥΣ_ΔΗΜΟΙ.Where(o => o.DIMOS_PERIFERIA == periferiaId).Select(p => new DimosViewModel()
            {
                DIMOS_ID = p.DIMOS_ID,
                DIMOS = p.DIMOS,
                DIMOS_PERIFERIA = p.DIMOS_PERIFERIA
            });
            return Json(dimoi.ToDataSourceResult(request));
        }

        public ActionResult xPeriferiesPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_ADMINS");
            }
            else
            {
                loggedAdmin = GetLoginAdmin();
                return View();
            }
        }

        #endregion

        #region ΛΟΓΑΡΙΑΣΜΟΙ ΣΤΑΘΜΩΝ (16-07-2019)

        public void PopulateStations()
        {
            var stations = (from d in db.ΣΥΣ_ΣΤΑΘΜΟΙ select d).ToList();

            ViewData["stations"] = stations;
        }

        public ActionResult UserStations(string notify = null)
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

            PopulateStations();
            List<UserStationViewModel> stationVM = GetStationList();

            if (notify != null) this.ShowMessage(MessageType.Info, notify);

            return View();
        }

        public List<UserStationViewModel> GetStationList()
        {
            List<UserStationViewModel> results = new List<UserStationViewModel>();
            var users = from a in db.USER_STATIONS
                        select new UserStationViewModel
                        {
                            USER_ID = a.USER_ID,
                            USERNAME = a.USERNAME,
                            PASSWORD = a.PASSWORD,
                            STATION_ID = a.STATION_ID ?? 0,
                            ISACTIVE = a.ISACTIVE ?? false
                        };

            results = users.ToList();
            return results;
        }

        public ActionResult CreatePasswords()
        {
            var stations = (from s in db.USER_STATIONS select s).ToList();

            foreach (var station in stations)
            {
                station.PASSWORD = c.GeneratePassword() + String.Format("{0:000}", station.STATION_ID);
                db.Entry(station).State = EntityState.Modified;
                db.SaveChanges();
            }

            string notify = "Η δημιουργία νέων κωδικών σταθμών ολοκληρώθηκε.";
            return RedirectToAction("UserStations", "Tools", new { notify });
        }

        #region Grid CRUD Functions

        [HttpPost]
        public ActionResult UserStation_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<UserStationViewModel> data = GetStationList();
            var users = from a in db.USER_STATIONS
                        select new UserStationViewModel
                        {
                            USER_ID = a.USER_ID,
                            USERNAME = a.USERNAME,
                            PASSWORD = a.PASSWORD,
                            STATION_ID = a.STATION_ID ?? 0,
                            ISACTIVE = a.ISACTIVE ?? false
                        };
            DataSourceResult result = users.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserStation_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<UserStationViewModel> userStations)
        {
            var results = new List<UserStationViewModel>();
            foreach (var userStation in userStations)
            {
                if (userStation != null && ModelState.IsValid)
                {
                    USER_STATIONS newuserStation = new USER_STATIONS()
                    {
                        USERNAME = userStation.USERNAME,
                        PASSWORD = userStation.PASSWORD,
                        STATION_ID = userStation.STATION_ID,
                        ISACTIVE = userStation.ISACTIVE,
                    };
                    db.USER_STATIONS.Add(newuserStation);
                    db.SaveChanges();
                    results.Add(userStation);
                }
            }
            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserStation_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<UserStationViewModel> userStations)
        {
            if (userStations != null)
            {
                foreach (var userStation in userStations)
                {
                    USER_STATIONS entity = db.USER_STATIONS.Where(mod => mod.USER_ID.Equals(userStation.USER_ID)).FirstOrDefault();
                    entity.USER_ID = userStation.USER_ID;
                    entity.USERNAME = userStation.USERNAME;
                    entity.PASSWORD = userStation.PASSWORD;
                    entity.STATION_ID = userStation.STATION_ID;
                    entity.ISACTIVE = userStation.ISACTIVE;
                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return Json(userStations.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserStation_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<UserStationViewModel> userStations)
        {
            if (userStations.Any())
            {
                foreach (var userStation in userStations)
                {
                    if (userStation != null)
                    {
                        USER_STATIONS entity = db.USER_STATIONS.Find(userStation.USER_ID);
                        db.Entry(entity).State = EntityState.Deleted;
                        db.USER_STATIONS.Remove(entity);
                        db.SaveChanges();
                    }
                }
            }
            return Json(userStations.ToDataSourceResult(request, ModelState));
        }

        #endregion

        public ActionResult StationAccountsPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_ADMINS");
            }
            else
            {
                loggedAdmin = GetLoginAdmin();
                return View();
            }
        }

        #endregion

        #region ΕΙΣΟΔΟΙ ΣΤΑΘΜΩΝ (16-07-2019)

        public ActionResult StationLogins()
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
            return View();
        }

        public ActionResult Logins_Read([DataSourceRequest] DataSourceRequest request)
        {
            var vms = GetStationLoginsFromDB();

            var result = new JsonResult();
            result.Data = vms.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<StationLoginsViewModel> GetStationLoginsFromDB()
        {
            var data = (from d in db.sqlSTATION_LOGINS
                        orderby d.LOGIN_DATETIME descending, d.STATION_NAME
                        select new StationLoginsViewModel
                        {
                            LOGIN_ID = d.LOGIN_ID,
                            STATION_NAME = d.STATION_NAME,
                            LOGIN_DATETIME = d.LOGIN_DATETIME
                        }).ToList();

            return data;
        }

        #endregion

        #region ΕΙΣΟΔΟΙ ΔΙΑΧΕΙΡΙΣΤΩΝ (29-02-2020)

        public ActionResult AdminLogins()
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
            return View();
        }

        public ActionResult AdminLogins_Read([DataSourceRequest] DataSourceRequest request)
        {
            var vms = GetAdminLoginsFromDB();

            var result = new JsonResult();
            result.Data = vms.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<AdminLoginsViewModel> GetAdminLoginsFromDB()
        {
            var data = (from d in db.sqlADMIN_LOGINS
                        orderby d.LOGIN_DATETIME descending, d.ADMIN_NAME
                        select new AdminLoginsViewModel
                        {
                            LOGIN_ID = d.LOGIN_ID,
                            ADMIN_NAME = d.ADMIN_NAME,
                            LOGIN_DATETIME = d.LOGIN_DATETIME
                        }).ToList();

            return data;
        }

        #endregion


        #region ΔΙΟΙΚΗΣΗ - ΥΠΟΓΡΑΦΟΝΤΕΣ ΑΠΟΦΑΣΕΙΣ

        public ActionResult Administrators()
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
            populateSchoolYears();
            populateGenders();
            return View();
        }

        #region ΔΙΑΧΕΙΡΙΣΤΕΣ

        public ActionResult Diaxiristis_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = vmDiaxiristesFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Diaxiristis_Create([DataSourceRequest] DataSourceRequest request, DiaxiristisViewModel data)
        {
            var newData = new DiaxiristisViewModel();

            var existingdata = db.Δ_ΔΙΑΧΕΙΡΙΣΤΕΣ.Where(s => s.ΟΝΟΜΑΤΕΠΩΝΥΜΟ == data.ΟΝΟΜΑΤΕΠΩΝΥΜΟ).ToList();
            if (data != null && ModelState.IsValid && (existingdata.Count == 0))
            {
                Δ_ΔΙΑΧΕΙΡΙΣΤΕΣ entity = new Δ_ΔΙΑΧΕΙΡΙΣΤΕΣ()
                {
                    ΟΝΟΜΑΤΕΠΩΝΥΜΟ = data.ΟΝΟΜΑΤΕΠΩΝΥΜΟ,
                    ΟΝΟΜΑΤΕΠΩΝΥΜΟ_ΠΛΗΡΕΣ = data.ΟΝΟΜΑΤΕΠΩΝΥΜΟ_ΠΛΗΡΕΣ,
                    ΤΗΛΕΦΩΝΟ = data.ΤΗΛΕΦΩΝΟ,
                    ΦΑΞ = data.ΦΑΞ,
                    ΦΥΛΟ = data.ΦΥΛΟ
                };
                db.Δ_ΔΙΑΧΕΙΡΙΣΤΕΣ.Add(entity);
                db.SaveChanges();
                data.ΔΙΑΧΕΙΡΙΣΤΗΣ_ΚΩΔ = entity.ΔΙΑΧΕΙΡΙΣΤΗΣ_ΚΩΔ;
                newData = RefreshDiaxiristisFromDB(entity.ΔΙΑΧΕΙΡΙΣΤΗΣ_ΚΩΔ);
            }

            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Diaxiristis_Update([DataSourceRequest] DataSourceRequest request, DiaxiristisViewModel data)
        {
            var newData = new DiaxiristisViewModel();

            if (data != null && ModelState.IsValid)
            {
                Δ_ΔΙΑΧΕΙΡΙΣΤΕΣ entity = db.Δ_ΔΙΑΧΕΙΡΙΣΤΕΣ.Find(data.ΔΙΑΧΕΙΡΙΣΤΗΣ_ΚΩΔ);

                entity.ΔΙΑΧΕΙΡΙΣΤΗΣ_ΚΩΔ = data.ΔΙΑΧΕΙΡΙΣΤΗΣ_ΚΩΔ;
                entity.ΟΝΟΜΑΤΕΠΩΝΥΜΟ = data.ΟΝΟΜΑΤΕΠΩΝΥΜΟ;
                entity.ΟΝΟΜΑΤΕΠΩΝΥΜΟ_ΠΛΗΡΕΣ = data.ΟΝΟΜΑΤΕΠΩΝΥΜΟ_ΠΛΗΡΕΣ;
                entity.ΤΗΛΕΦΩΝΟ = data.ΤΗΛΕΦΩΝΟ;
                entity.ΦΑΞ = data.ΦΑΞ;
                entity.ΦΥΛΟ = data.ΦΥΛΟ;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newData = RefreshDiaxiristisFromDB(entity.ΔΙΑΧΕΙΡΙΣΤΗΣ_ΚΩΔ);
            }

            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Diaxiristis_Destroy([DataSourceRequest] DataSourceRequest request, DiaxiristisViewModel data)
        {
            if (data != null)
            {
                if (k.CanDeleteDiaxiristis(data.ΔΙΑΧΕΙΡΙΣΤΗΣ_ΚΩΔ))
                {
                    Δ_ΔΙΑΧΕΙΡΙΣΤΕΣ entity = db.Δ_ΔΙΑΧΕΙΡΙΣΤΕΣ.Find(data.ΔΙΑΧΕΙΡΙΣΤΗΣ_ΚΩΔ);
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.Δ_ΔΙΑΧΕΙΡΙΣΤΕΣ.Remove(entity);
                        db.SaveChanges();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί αυτός ο διαχειριστής διότι είναι σε χρήση.");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public DiaxiristisViewModel RefreshDiaxiristisFromDB(int recordId)
        {
            var data = (from d in db.Δ_ΔΙΑΧΕΙΡΙΣΤΕΣ
                        where d.ΔΙΑΧΕΙΡΙΣΤΗΣ_ΚΩΔ == recordId
                        select new DiaxiristisViewModel
                        {
                            ΔΙΑΧΕΙΡΙΣΤΗΣ_ΚΩΔ = d.ΔΙΑΧΕΙΡΙΣΤΗΣ_ΚΩΔ,
                            ΟΝΟΜΑΤΕΠΩΝΥΜΟ = d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ,
                            ΟΝΟΜΑΤΕΠΩΝΥΜΟ_ΠΛΗΡΕΣ = d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ_ΠΛΗΡΕΣ,
                            ΤΗΛΕΦΩΝΟ = d.ΤΗΛΕΦΩΝΟ,
                            ΦΑΞ = d.ΦΑΞ,
                            ΦΥΛΟ = d.ΦΥΛΟ
                        }).FirstOrDefault();

            return data;
        }

        public List<DiaxiristisViewModel> vmDiaxiristesFromDB()
        {

            var data = (from d in db.Δ_ΔΙΑΧΕΙΡΙΣΤΕΣ
                        orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ
                        select new DiaxiristisViewModel
                        {
                            ΔΙΑΧΕΙΡΙΣΤΗΣ_ΚΩΔ = d.ΔΙΑΧΕΙΡΙΣΤΗΣ_ΚΩΔ,
                            ΟΝΟΜΑΤΕΠΩΝΥΜΟ = d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ,
                            ΟΝΟΜΑΤΕΠΩΝΥΜΟ_ΠΛΗΡΕΣ = d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ_ΠΛΗΡΕΣ,
                            ΤΗΛΕΦΩΝΟ = d.ΤΗΛΕΦΩΝΟ,
                            ΦΑΞ = d.ΦΑΞ,
                            ΦΥΛΟ = d.ΦΥΛΟ
                        }).ToList();

            return data;
        }

        #endregion ΔΙΑΧΕΙΡΙΣΤΕΣ

        #region ΠΡΟΪΣΤΑΜΕΝΟΙ

        public ActionResult Proistamenos_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = vmProistamenoiFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Proistamenos_Create([DataSourceRequest] DataSourceRequest request, ProistamenosViewModel data)
        {
            var newData = new ProistamenosViewModel();

            var existingdata = db.Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ.Where(s => s.ΣΧΟΛΙΚΟ_ΕΤΟΣ == data.ΣΧΟΛΙΚΟ_ΕΤΟΣ).ToList();
            if (data != null && ModelState.IsValid && (existingdata.Count == 0))
            {
                Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ entity = new Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ()
                {
                    ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                    ΠΡΟΙΣΤΑΜΕΝΟΣ = data.ΠΡΟΙΣΤΑΜΕΝΟΣ,
                    ΦΥΛΟ = data.ΦΥΛΟ
                };
                db.Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ.Add(entity);
                db.SaveChanges();
                data.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ = entity.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ;
                newData = RefreshProistamenosFromDB(entity.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Proistamenos_Update([DataSourceRequest] DataSourceRequest request, ProistamenosViewModel data)
        {
            var newData = new ProistamenosViewModel();

            if (data != null && ModelState.IsValid)
            {
                Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ entity = db.Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ.Find(data.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ);

                entity.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ = data.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ;
                entity.ΠΡΟΙΣΤΑΜΕΝΟΣ = data.ΠΡΟΙΣΤΑΜΕΝΟΣ;
                entity.ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
                entity.ΦΥΛΟ = data.ΦΥΛΟ;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newData = RefreshProistamenosFromDB(entity.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Proistamenos_Destroy([DataSourceRequest] DataSourceRequest request, ProistamenosViewModel data)
        {
            if (data != null)
            {
                Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ entity = db.Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ.Find(data.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ);
                if (entity != null)
                {
                    db.Entry(entity).State = EntityState.Deleted;
                    db.Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ.Remove(entity);
                    db.SaveChanges();
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ProistamenosViewModel RefreshProistamenosFromDB(int recordId)
        {
            var data = (from d in db.Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ
                        where d.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ == recordId
                        select new ProistamenosViewModel
                        {
                            ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ = d.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΠΡΟΙΣΤΑΜΕΝΟΣ = d.ΠΡΟΙΣΤΑΜΕΝΟΣ,
                            ΦΥΛΟ = d.ΦΥΛΟ
                        }).FirstOrDefault();

            return data;
        }

        public List<ProistamenosViewModel> vmProistamenoiFromDB()
        {

            var data = (from d in db.Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ
                        orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ
                        select new ProistamenosViewModel
                        {
                            ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ = d.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΠΡΟΙΣΤΑΜΕΝΟΣ = d.ΠΡΟΙΣΤΑΜΕΝΟΣ,
                            ΦΥΛΟ = d.ΦΥΛΟ
                        }).ToList();

            return data;
        }

        #endregion ΠΡΟΪΣΤΑΜΕΝΟΙ

        #region ΔΙΕΥΘΥΝΤΕΣ

        public ActionResult Director_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = vmDirectorsFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Director_Create([DataSourceRequest] DataSourceRequest request, DirectorViewModel data)
        {
            var newData = new DirectorViewModel();

            var existingdata = db.Δ_ΔΙΕΥΘΥΝΤΕΣ.Where(s => s.ΣΧΟΛΙΚΟ_ΕΤΟΣ == data.ΣΧΟΛΙΚΟ_ΕΤΟΣ).ToList();
            if (data != null && ModelState.IsValid && (existingdata.Count == 0))
            {
                Δ_ΔΙΕΥΘΥΝΤΕΣ entity = new Δ_ΔΙΕΥΘΥΝΤΕΣ()
                {
                    ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                    ΔΙΕΥΘΥΝΤΗΣ = data.ΔΙΕΥΘΥΝΤΗΣ,
                    ΦΥΛΟ = data.ΦΥΛΟ
                };
                db.Δ_ΔΙΕΥΘΥΝΤΕΣ.Add(entity);
                db.SaveChanges();
                data.ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ = entity.ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ;
                newData = RefreshDirectorFromDB(entity.ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Director_Update([DataSourceRequest] DataSourceRequest request, DirectorViewModel data)
        {
            var newData = new DirectorViewModel();

            if (data != null && ModelState.IsValid)
            {
                Δ_ΔΙΕΥΘΥΝΤΕΣ entity = db.Δ_ΔΙΕΥΘΥΝΤΕΣ.Find(data.ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ);

                entity.ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ = data.ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ;
                entity.ΔΙΕΥΘΥΝΤΗΣ = data.ΔΙΕΥΘΥΝΤΗΣ;
                entity.ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
                entity.ΦΥΛΟ = data.ΦΥΛΟ;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newData = RefreshDirectorFromDB(entity.ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Director_Destroy([DataSourceRequest] DataSourceRequest request, DirectorViewModel data)
        {
            if (data != null)
            {
                Δ_ΔΙΕΥΘΥΝΤΕΣ entity = db.Δ_ΔΙΕΥΘΥΝΤΕΣ.Find(data.ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ);
                if (entity != null)
                {
                    db.Entry(entity).State = EntityState.Deleted;
                    db.Δ_ΔΙΕΥΘΥΝΤΕΣ.Remove(entity);
                    db.SaveChanges();
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public DirectorViewModel RefreshDirectorFromDB(int recordId)
        {
            var data = (from d in db.Δ_ΔΙΕΥΘΥΝΤΕΣ
                        where d.ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ == recordId
                        select new DirectorViewModel
                        {
                            ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ = d.ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΔΙΕΥΘΥΝΤΗΣ = d.ΔΙΕΥΘΥΝΤΗΣ,
                            ΦΥΛΟ = d.ΦΥΛΟ
                        }).FirstOrDefault();

            return data;
        }

        public List<DirectorViewModel> vmDirectorsFromDB()
        {
            var data = (from d in db.Δ_ΔΙΕΥΘΥΝΤΕΣ
                        orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ
                        select new DirectorViewModel
                        {
                            ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ = d.ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΔΙΕΥΘΥΝΤΗΣ = d.ΔΙΕΥΘΥΝΤΗΣ,
                            ΦΥΛΟ = d.ΦΥΛΟ
                        }).ToList();

            return data;
        }

        #endregion ΔΙΕΥΘΥΝΤΕΣ

        #region ΓΕΝΙΚΟΙ ΔΙΕΥΘΥΝΤΕΣ

        public ActionResult Genikos_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = vmGenikoiFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Genikos_Create([DataSourceRequest] DataSourceRequest request, DirectorGeneralViewModel data)
        {
            var newData = new DirectorGeneralViewModel();

            var existingdata = db.Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ.Where(s => s.ΣΧΟΛΙΚΟ_ΕΤΟΣ == data.ΣΧΟΛΙΚΟ_ΕΤΟΣ).ToList();
            if (data != null && ModelState.IsValid && (existingdata.Count == 0))
            {
                Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ entity = new Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ()
                {
                    ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                    ΓΕΝΙΚΟΣ = data.ΓΕΝΙΚΟΣ,
                    ΦΥΛΟ = data.ΦΥΛΟ
                };
                db.Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ.Add(entity);
                db.SaveChanges();
                data.ΓΕΝΙΚΟΣ_ΚΩΔ = entity.ΓΕΝΙΚΟΣ_ΚΩΔ;
                newData = RefreshGenikosFromDB(entity.ΓΕΝΙΚΟΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Genikos_Update([DataSourceRequest] DataSourceRequest request, DirectorGeneralViewModel data)
        {
            var newData = new DirectorGeneralViewModel();

            if (data != null && ModelState.IsValid)
            {
                Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ entity = db.Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ.Find(data.ΓΕΝΙΚΟΣ_ΚΩΔ);

                entity.ΓΕΝΙΚΟΣ_ΚΩΔ = data.ΓΕΝΙΚΟΣ_ΚΩΔ;
                entity.ΓΕΝΙΚΟΣ = data.ΓΕΝΙΚΟΣ;
                entity.ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
                entity.ΦΥΛΟ = data.ΦΥΛΟ;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newData = RefreshGenikosFromDB(entity.ΓΕΝΙΚΟΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Genikos_Destroy([DataSourceRequest] DataSourceRequest request, DirectorGeneralViewModel data)
        {
            if (data != null)
            {
                Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ entity = db.Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ.Find(data.ΓΕΝΙΚΟΣ_ΚΩΔ);
                if (entity != null)
                {
                    db.Entry(entity).State = EntityState.Deleted;
                    db.Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ.Remove(entity);
                    db.SaveChanges();
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public DirectorGeneralViewModel RefreshGenikosFromDB(int recordId)
        {
            var data = (from d in db.Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ
                        where d.ΓΕΝΙΚΟΣ_ΚΩΔ == recordId
                        select new DirectorGeneralViewModel
                        {
                            ΓΕΝΙΚΟΣ_ΚΩΔ = d.ΓΕΝΙΚΟΣ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΓΕΝΙΚΟΣ = d.ΓΕΝΙΚΟΣ,
                            ΦΥΛΟ = d.ΦΥΛΟ
                        }).FirstOrDefault();

            return data;
        }

        public List<DirectorGeneralViewModel> vmGenikoiFromDB()
        {

            var data = (from d in db.Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ
                        orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ
                        select new DirectorGeneralViewModel
                        {
                            ΓΕΝΙΚΟΣ_ΚΩΔ = d.ΓΕΝΙΚΟΣ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΓΕΝΙΚΟΣ = d.ΓΕΝΙΚΟΣ,
                            ΦΥΛΟ = d.ΦΥΛΟ
                        }).ToList();

            return data;
        }

        #endregion ΓΕΝΙΚΟΙ ΔΙΕΥΘΥΝΤΕΣ

        #region ΑΝΤΙΠΡΟΕΔΡΟΙ

        public ActionResult Antiproedros_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = vmAntiproedroiFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Antiproedros_Create([DataSourceRequest] DataSourceRequest request, AntiproedrosViewModel data)
        {
            var newData = new AntiproedrosViewModel();

            var existingdata = db.Δ_ΑΝΤΙΠΡΟΕΔΡΟΙ.Where(s => s.ΣΧΟΛΙΚΟ_ΕΤΟΣ == data.ΣΧΟΛΙΚΟ_ΕΤΟΣ).ToList();
            if (data != null && ModelState.IsValid && (existingdata.Count == 0))
            {
                Δ_ΑΝΤΙΠΡΟΕΔΡΟΙ entity = new Δ_ΑΝΤΙΠΡΟΕΔΡΟΙ()
                {
                    ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                    ΑΝΤΙΠΡΟΕΔΡΟΣ = data.ΑΝΤΙΠΡΟΕΔΡΟΣ,
                    ΦΥΛΟ = data.ΦΥΛΟ
                };
                db.Δ_ΑΝΤΙΠΡΟΕΔΡΟΙ.Add(entity);
                db.SaveChanges();
                data.ΑΝΤΙΠΡΟΕΔΡΟΣ_ΚΩΔ = entity.ΑΝΤΙΠΡΟΕΔΡΟΣ_ΚΩΔ;
                newData = RefreshAntiproedrosFromDB(entity.ΑΝΤΙΠΡΟΕΔΡΟΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Antiproedros_Update([DataSourceRequest] DataSourceRequest request, AntiproedrosViewModel data)
        {
            var newData = new AntiproedrosViewModel();

            if (data != null && ModelState.IsValid)
            {
                Δ_ΑΝΤΙΠΡΟΕΔΡΟΙ entity = db.Δ_ΑΝΤΙΠΡΟΕΔΡΟΙ.Find(data.ΑΝΤΙΠΡΟΕΔΡΟΣ_ΚΩΔ);

                entity.ΑΝΤΙΠΡΟΕΔΡΟΣ_ΚΩΔ = data.ΑΝΤΙΠΡΟΕΔΡΟΣ_ΚΩΔ;
                entity.ΑΝΤΙΠΡΟΕΔΡΟΣ = data.ΑΝΤΙΠΡΟΕΔΡΟΣ;
                entity.ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
                entity.ΦΥΛΟ = data.ΦΥΛΟ;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newData = RefreshAntiproedrosFromDB(entity.ΑΝΤΙΠΡΟΕΔΡΟΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Antiproedros_Destroy([DataSourceRequest] DataSourceRequest request, AntiproedrosViewModel data)
        {
            if (data != null)
            {
                Δ_ΑΝΤΙΠΡΟΕΔΡΟΙ entity = db.Δ_ΑΝΤΙΠΡΟΕΔΡΟΙ.Find(data.ΑΝΤΙΠΡΟΕΔΡΟΣ_ΚΩΔ);
                if (entity != null)
                {
                    db.Entry(entity).State = EntityState.Deleted;
                    db.Δ_ΑΝΤΙΠΡΟΕΔΡΟΙ.Remove(entity);
                    db.SaveChanges();
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public AntiproedrosViewModel RefreshAntiproedrosFromDB(int recordId)
        {
            var data = (from d in db.Δ_ΑΝΤΙΠΡΟΕΔΡΟΙ
                        where d.ΑΝΤΙΠΡΟΕΔΡΟΣ_ΚΩΔ == recordId
                        select new AntiproedrosViewModel
                        {
                            ΑΝΤΙΠΡΟΕΔΡΟΣ_ΚΩΔ = d.ΑΝΤΙΠΡΟΕΔΡΟΣ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΑΝΤΙΠΡΟΕΔΡΟΣ = d.ΑΝΤΙΠΡΟΕΔΡΟΣ,
                            ΦΥΛΟ = d.ΦΥΛΟ
                        }).FirstOrDefault();

            return data;
        }

        public List<AntiproedrosViewModel> vmAntiproedroiFromDB()
        {

            var data = (from d in db.Δ_ΑΝΤΙΠΡΟΕΔΡΟΙ
                        orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ
                        select new AntiproedrosViewModel
                        {
                            ΑΝΤΙΠΡΟΕΔΡΟΣ_ΚΩΔ = d.ΑΝΤΙΠΡΟΕΔΡΟΣ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΑΝΤΙΠΡΟΕΔΡΟΣ = d.ΑΝΤΙΠΡΟΕΔΡΟΣ,
                            ΦΥΛΟ = d.ΦΥΛΟ
                        }).ToList();

            return data;
        }

        #endregion ΑΝΤΙΠΡΟΕΔΡΟΙ

        #region ΔΙΟΙΚΗΤΕΣ

        public ActionResult Dioikitis_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = vmDioikitesFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Dioikitis_Create([DataSourceRequest] DataSourceRequest request, DioikitisViewModel data)
        {
            var newData = new DioikitisViewModel();

            var existingdata = db.Δ_ΔΙΟΙΚΗΤΕΣ.Where(s => s.ΣΧΟΛΙΚΟ_ΕΤΟΣ == data.ΣΧΟΛΙΚΟ_ΕΤΟΣ).ToList();
            if (data != null && ModelState.IsValid && (existingdata.Count == 0))
            {
                Δ_ΔΙΟΙΚΗΤΕΣ entity = new Δ_ΔΙΟΙΚΗΤΕΣ()
                {
                    ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                    ΔΙΟΙΚΗΤΗΣ = data.ΔΙΟΙΚΗΤΗΣ,
                    ΦΥΛΟ = data.ΦΥΛΟ
                };
                db.Δ_ΔΙΟΙΚΗΤΕΣ.Add(entity);
                db.SaveChanges();
                data.ΔΙΟΙΚΗΤΗΣ_ΚΩΔ = entity.ΔΙΟΙΚΗΤΗΣ_ΚΩΔ;
                newData = RefreshDioikitisFromDB(entity.ΔΙΟΙΚΗΤΗΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Dioikitis_Update([DataSourceRequest] DataSourceRequest request, DioikitisViewModel data)
        {
            var newData = new DioikitisViewModel();

            if (data != null && ModelState.IsValid)
            {
                Δ_ΔΙΟΙΚΗΤΕΣ entity = db.Δ_ΔΙΟΙΚΗΤΕΣ.Find(data.ΔΙΟΙΚΗΤΗΣ_ΚΩΔ);

                entity.ΔΙΟΙΚΗΤΗΣ_ΚΩΔ = data.ΔΙΟΙΚΗΤΗΣ_ΚΩΔ;
                entity.ΔΙΟΙΚΗΤΗΣ = data.ΔΙΟΙΚΗΤΗΣ;
                entity.ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
                entity.ΦΥΛΟ = data.ΦΥΛΟ;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newData = RefreshDioikitisFromDB(entity.ΔΙΟΙΚΗΤΗΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Dioikitis_Destroy([DataSourceRequest] DataSourceRequest request, DioikitisViewModel data)
        {
            if (data != null)
            {
                Δ_ΔΙΟΙΚΗΤΕΣ entity = db.Δ_ΔΙΟΙΚΗΤΕΣ.Find(data.ΔΙΟΙΚΗΤΗΣ_ΚΩΔ);
                if (entity != null)
                {
                    db.Entry(entity).State = EntityState.Deleted;
                    db.Δ_ΔΙΟΙΚΗΤΕΣ.Remove(entity);
                    db.SaveChanges();
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public DioikitisViewModel RefreshDioikitisFromDB(int recordId)
        {
            var data = (from d in db.Δ_ΔΙΟΙΚΗΤΕΣ
                        where d.ΔΙΟΙΚΗΤΗΣ_ΚΩΔ == recordId
                        select new DioikitisViewModel
                        {
                            ΔΙΟΙΚΗΤΗΣ_ΚΩΔ = d.ΔΙΟΙΚΗΤΗΣ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΔΙΟΙΚΗΤΗΣ = d.ΔΙΟΙΚΗΤΗΣ,
                            ΦΥΛΟ = d.ΦΥΛΟ
                        }).FirstOrDefault();

            return data;
        }

        public List<DioikitisViewModel> vmDioikitesFromDB()
        {
            var data = (from d in db.Δ_ΔΙΟΙΚΗΤΕΣ
                        orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ
                        select new DioikitisViewModel
                        {
                            ΔΙΟΙΚΗΤΗΣ_ΚΩΔ = d.ΔΙΟΙΚΗΤΗΣ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΔΙΟΙΚΗΤΗΣ = d.ΔΙΟΙΚΗΤΗΣ,
                            ΦΥΛΟ = d.ΦΥΛΟ
                        }).ToList();

            return data;
        }

        #endregion ΔΙΟΙΚΗΤΕΣ

        #endregion


        #region POPULATORS

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

        #endregion

        #region GETTERS

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

        #endregion

        #region ΧΑΡΤΕΣ GOOGLE

        public ActionResult xGoogleMaps()
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
            return View();
        }

        #endregion

    }
}
