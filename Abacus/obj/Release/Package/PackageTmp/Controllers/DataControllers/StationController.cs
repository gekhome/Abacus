using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.IO;
using System.Text;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
    
using Abacus.DAL;
using Abacus.Models;
using Abacus.BPM;
using Abacus.Notification;


namespace Abacus.Controllers.DataControllers
{
    public class StationController : Controller
    {
        private AbacusDBEntities db = new AbacusDBEntities();
        private USER_STATIONS loggedStation;
        private string StationName;
        private int stationId;

        Common c = new Common();
        Kerberos k = new Kerberos();

        public ActionResult Index(string notify = null)
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
            return View();
        }

        #region ΒΝΣ DATA FORM (27-07-2019)

        public ActionResult StationDataForm()
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

            StationsViewModel station = GetStationViewModelFromDB(stationId);
            if (station == null)
            {
                return HttpNotFound();
            }

            StationsViewModel stationData = GetStationViewModelFromDB(stationId);
            return View(stationData);
        }

        [HttpPost]
        public ActionResult StationDataForm(StationsViewModel svm)
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

        #region ΤΜΗΜΑΤΑ ΒΝΣ (27-07-2019)

        public ActionResult StationTmimaList()
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
            populateSchoolYears();
            populateTmimaCategories();
            populateNumbersLatin();
            return View();
        }

        #region TMHMATA GRID CRUD FUNCTIONS

        public ActionResult Tmima_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = GetTmimaViewModelFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Tmima_Create([DataSourceRequest] DataSourceRequest request, TmimaViewModel data)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

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
        public ActionResult Tmima_Update([DataSourceRequest] DataSourceRequest request, TmimaViewModel data)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

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

        public List<TmimaViewModel> GetTmimaViewModelFromDB()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

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

        #region --- SETUPS REGION ---

        #region ΚΑΤΗΓΟΡΙΕΣ ΤΜΗΜΑΤΩΝ (27-07-2019)

        public ActionResult TmimaCategories()
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

        #region ΕΙΔΙΚΟΤΗΤΕΣ-ΚΛΑΔΟΙ ΠΡΟΣΩΠΙΚΟΥ (19-07-2019)

        public ActionResult EidikotitesList()
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

        public ActionResult EidikotitesPrint()
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
                return View();
            }
        }

        #endregion ΕΙΔΙΚΟΤΗΤΕΣ ΠΡΟΣΩΠΙΚΟΥ

        #region ΚΑΤΗΓΟΡΙΕΣ ΠΡΟΣΩΠΙΚΟΥ (27-07-2019)

        public ActionResult PersonnelTypes()
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

        public ActionResult MetabolesTypes()
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

        #region ΕΙΔΗ ΑΠΟΧΩΡΗΣΕΩΝ ΠΡΟΣΩΠΙΚΟΥ

        public ActionResult ApoxorisiTypes()
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

        #region ΣΧΟΛΙΚΑ ΕΤΗ (27-07-2019)

        public ActionResult SchoolYearsList()
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

        #region ΩΡΕΣ ΠΡΟΣΕΛΕΥΣΗΣ-ΑΠΟΧΩΡΗΣΗΣ (27-07-2019)

        public ActionResult HoursList()
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

        #region ΠΕΡΙΦΕΡΕΙΑΚΕΣ ΔΙΕΥΘΥΝΣΕΙΣ (27-07-2019)

        public ActionResult PeriferiakesList()
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

        #region ΠΕΡΙΦΕΡΕΙΕΣ-ΔΗΜΟΙ

        public ActionResult PeriferiesDimoi()
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
            }
            return View();
        }

        public ActionResult Periferies_Read([DataSourceRequest] DataSourceRequest request)
        {
            var periferies = db.ΣΥΣ_ΠΕΡΙΦΕΡΕΙΕΣ.Select(p => new PeriferiaViewModel()
            {
                PERIFERIA_ID = p.PERIFERIA_ID,
                PERIFERIA_NAME = p.PERIFERIA_NAME
            });
            return Json(periferies.ToDataSourceResult(request));
        }

        public ActionResult Dimoi_Read([DataSourceRequest] DataSourceRequest request, int periferiaId)
        {
            var dimoi = db.ΣΥΣ_ΔΗΜΟΙ.Where(o => o.DIMOS_PERIFERIA == periferiaId).Select(p => new DimosViewModel()
            {
                DIMOS_ID = p.DIMOS_ID,
                DIMOS = p.DIMOS,
                DIMOS_PERIFERIA = p.DIMOS_PERIFERIA
            });
            return Json(dimoi.ToDataSourceResult(request));
        }

        public ActionResult PeriferiesPrint()
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
                return View();
            }
        }

        #endregion

        #endregion -------------------------

        #region ΒΡΕΦΟΝΗΠΙΑ - ΣΤΟΙΧΕΙΑ ΚΑΙ ΕΓΓΡΑΦΕΣ (20-07-2019)

        public ActionResult ChildrenData(string notify = null)
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

            if (!StationsExist() || !SchoolYearsExist() || !TmimataExist())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει πρώτα να ορίσετε ΒΝΣ, σχολικά έτη και τμήματα των ΒΝΣ στις Ρυθμίσεις.";
                return RedirectToAction("Index", "Station", new { notify = msg });
            }

            populateTmimata();
            populateSchoolYears();
            return View();
        }

        public List<ChildGridViewModel> GetChildDataFromDB()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΠΑΙΔΙΑ
                        where d.ΒΝΣ == stationId
                        orderby d.ΕΠΩΝΥΜΟ, d.ΟΝΟΜΑ
                        select new ChildGridViewModel
                        {
                            CHILD_ID = d.CHILD_ID,
                            ΑΜ = d.ΑΜ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΟ = d.ΕΠΩΝΥΜΟ,
                            ΟΝΟΜΑ = d.ΟΝΟΜΑ
                        }).ToList();

            return (data);
        }

        public ChildGridViewModel RefreshChildDataFromDB(int recordId)
        {
            var data = (from d in db.ΠΑΙΔΙΑ
                        where d.CHILD_ID == recordId
                        select new ChildGridViewModel
                        {
                            CHILD_ID = d.CHILD_ID,
                            ΑΜ = d.ΑΜ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΟ = d.ΕΠΩΝΥΜΟ,
                            ΟΝΟΜΑ = d.ΟΝΟΜΑ
                        }).FirstOrDefault();

            return (data);
        }

        public List<ChildTmimaViewModel> GetChildTmimaFromDB(int childId)
        {
            var data = (from d in db.ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ
                        where d.ΠΑΙΔΙ_ΚΩΔ == childId
                        orderby d.ΗΜΝΙΑ_ΕΓΓΡΑΦΗ
                        select new ChildTmimaViewModel
                        {
                            ΕΓΓΡΑΦΗ_ΚΩΔ = d.ΕΓΓΡΑΦΗ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΠΑΙΔΙ_ΚΩΔ = d.ΠΑΙΔΙ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΤΜΗΜΑ = d.ΤΜΗΜΑ,
                            ΗΜΝΙΑ_ΕΓΓΡΑΦΗ = d.ΗΜΝΙΑ_ΕΓΓΡΑΦΗ,
                            ΗΜΝΙΑ_ΠΕΡΑΣ = d.ΗΜΝΙΑ_ΠΕΡΑΣ
                        }).ToList();

            return data;
        }

        public ChildTmimaViewModel RefreshChildTmimaFromDB(int recordId)
        {
            var data = (from d in db.ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ
                        where d.ΕΓΓΡΑΦΗ_ΚΩΔ == recordId
                        select new ChildTmimaViewModel
                        {
                            ΕΓΓΡΑΦΗ_ΚΩΔ = d.ΕΓΓΡΑΦΗ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΠΑΙΔΙ_ΚΩΔ = d.ΠΑΙΔΙ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΤΜΗΜΑ = d.ΤΜΗΜΑ,
                            ΗΜΝΙΑ_ΕΓΓΡΑΦΗ = d.ΗΜΝΙΑ_ΕΓΓΡΑΦΗ,
                            ΗΜΝΙΑ_ΠΕΡΑΣ = d.ΗΜΝΙΑ_ΠΕΡΑΣ
                        }).FirstOrDefault();

            return data;
        }

        public ActionResult FilteredTmima_Read(int schoolyearId = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΤΜΗΜΑ
                        where d.ΣΧΟΛΙΚΟ_ΕΤΟΣ == schoolyearId && d.ΒΝΣ == stationId
                        orderby d.ΟΝΟΜΑΣΙΑ
                        select new TmimaSelectorViewModel
                        {
                            ΤΜΗΜΑ_ΚΩΔ = d.ΤΜΗΜΑ_ΚΩΔ,
                            ΟΝΟΜΑΣΙΑ = d.ΟΝΟΜΑΣΙΑ
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #region CHILDREN GRID CRUD

        public ActionResult Children_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<ChildGridViewModel> students = GetChildDataFromDB();

            var result = new JsonResult();
            result.Data = students.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Children_Create([DataSourceRequest] DataSourceRequest request, ChildGridViewModel data)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var newdata = new ChildGridViewModel();

            if (!k.ValidatePrimaryKeyChild((int)data.ΑΜ, stationId)) ModelState.AddModelError("", "Ο Α.Μ. που δώθηκε είναι ήδη καταχωρημένος για το σταθμό αυτό.");
            if (ModelState.IsValid)
            {
                ΠΑΙΔΙΑ entity = new ΠΑΙΔΙΑ()
                {
                    ΑΜ = data.ΑΜ,
                    ΒΝΣ = stationId,
                    ΕΠΩΝΥΜΟ = data.ΕΠΩΝΥΜΟ.Trim(),
                    ΟΝΟΜΑ = data.ΟΝΟΜΑ.Trim(),
                };
                db.ΠΑΙΔΙΑ.Add(entity);
                db.SaveChanges();
                data.CHILD_ID = entity.CHILD_ID;
                newdata = RefreshChildDataFromDB(data.CHILD_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Children_Update([DataSourceRequest] DataSourceRequest request, ChildGridViewModel data)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var newdata = new ChildGridViewModel();

            if (ModelState.IsValid)
            {
                ΠΑΙΔΙΑ entity = db.ΠΑΙΔΙΑ.Find(data.CHILD_ID);

                entity.ΑΜ = data.ΑΜ;
                entity.ΒΝΣ = stationId;
                entity.ΕΠΩΝΥΜΟ = data.ΕΠΩΝΥΜΟ;
                entity.ΟΝΟΜΑ = data.ΟΝΟΜΑ;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newdata = RefreshChildDataFromDB(entity.CHILD_ID);
            }

            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Children_Destroy([DataSourceRequest] DataSourceRequest request, ChildGridViewModel data)
        {
            if (data != null)
            {
                ΠΑΙΔΙΑ entity = db.ΠΑΙΔΙΑ.Find(data.CHILD_ID);
                if (!k.CanDeleteChild(data.CHILD_ID)) ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί το παιδί αυτό διότι υπάρχουν εγγραφές του σε τμήματα.");
                if (ModelState.IsValid)
                {
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.ΠΑΙΔΙΑ.Remove(entity);
                        db.SaveChanges();
                    }
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CHILDREN EGRAFES GRID CRUD

        public ActionResult Egrafes_Read([DataSourceRequest] DataSourceRequest request, int childId = 0)
        {
            var vms = GetChildTmimaFromDB(childId);

            var result = new JsonResult();
            result.Data = vms.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Egrafes_Create([DataSourceRequest] DataSourceRequest request, ChildTmimaViewModel data, int childId = 0)
        {
            ChildTmimaViewModel newdata = new ChildTmimaViewModel();
            int stationId = c.GetStationFromfChildID(childId);

            if (childId > 0 && stationId > 0)
            {
                var existingData = db.ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ.Where(s => s.ΠΑΙΔΙ_ΚΩΔ == childId && s.ΤΜΗΜΑ == data.ΤΜΗΜΑ).ToList();
                if (existingData.Count > 0) ModelState.AddModelError("", "Υπάρχει ήδη καταχώρηση του παιδιού στο τμήμα αυτό.");

                if (data != null && ModelState.IsValid)
                {
                    ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ entity = new ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ()
                    {
                        ΠΑΙΔΙ_ΚΩΔ = childId,
                        ΒΝΣ = stationId,
                        ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                        ΤΜΗΜΑ = data.ΤΜΗΜΑ,
                        ΗΜΝΙΑ_ΕΓΓΡΑΦΗ = data.ΗΜΝΙΑ_ΕΓΓΡΑΦΗ,
                        ΗΜΝΙΑ_ΠΕΡΑΣ = data.ΗΜΝΙΑ_ΠΕΡΑΣ
                    };
                    db.ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ.Add(entity);
                    db.SaveChanges();
                    data.ΕΓΓΡΑΦΗ_ΚΩΔ = entity.ΕΓΓΡΑΦΗ_ΚΩΔ;   // grid requires the new generated id
                    newdata = RefreshChildTmimaFromDB(entity.ΕΓΓΡΑΦΗ_ΚΩΔ);
                }
            }
            else
            {
                ModelState.AddModelError("", "Πρέπει να προηγηθεί επιλογή κάποιου παιδιού για εγγραφή του σε τμήμα.");
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Egrafes_Update([DataSourceRequest] DataSourceRequest request, ChildTmimaViewModel data, int childId = 0)
        {
            ChildTmimaViewModel newdata = new ChildTmimaViewModel();
            int stationId = c.GetStationFromfChildID(childId);

            if (childId > 0 && stationId > 0)
            {
                if (data != null && ModelState.IsValid)
                {
                    ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ entity = db.ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ.Find(data.ΕΓΓΡΑΦΗ_ΚΩΔ);

                    entity.ΠΑΙΔΙ_ΚΩΔ = childId;
                    entity.ΒΝΣ = stationId;
                    entity.ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
                    entity.ΤΜΗΜΑ = data.ΤΜΗΜΑ;
                    entity.ΗΜΝΙΑ_ΕΓΓΡΑΦΗ = data.ΗΜΝΙΑ_ΕΓΓΡΑΦΗ;
                    entity.ΗΜΝΙΑ_ΠΕΡΑΣ = data.ΗΜΝΙΑ_ΠΕΡΑΣ;

                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                    newdata = RefreshChildTmimaFromDB(entity.ΕΓΓΡΑΦΗ_ΚΩΔ);
                }
            }
            else
            {
                ModelState.AddModelError("", "Πρέπει πρώτα να γίνει επιλογή κάποιου παιδιού. Η ενημέρωση ακυρώθηκε.");
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Egrafes_Destroy([DataSourceRequest] DataSourceRequest request, ChildTmimaViewModel data)
        {
            if (data != null)
            {
                ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ entity = db.ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ.Find(data.ΕΓΓΡΑΦΗ_ΚΩΔ);
                if (entity != null)
                {
                    db.Entry(entity).State = EntityState.Deleted;
                    db.ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ.Remove(entity);
                    db.SaveChanges();
                }
            }

            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CHILDREN DATA FORM

        public ActionResult ChildrenEdit(int? childId)
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
            if (!(childId > 0))
            {
                string msg = "Ο κωδικός παιδιού δεν είναι έγκυρος. Η εγγραφή πρέπει να έχει αποθηκευτεί πρώτα.";
                return RedirectToAction("ErrorData", "Station", new { notify = msg });
            }
            int childID = (int)childId;

            ChildViewModel child = GetChildViewModelFromDB(childID);
            if (child == null)
            {
                string msg = "Παρουσιάστηκε πρόβλημα εύρεσης των δεδομένων παιδιού.";
                return RedirectToAction("ErrorData", "Station", new { notify = msg });
            }

            return View(child);
        }

        [HttpPost]
        public ActionResult ChildrenEdit(int childId, ChildViewModel cvm)
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

            if (cvm == null)
            {
                string msg = "Παρουσιάστηκε πρόβλημα εύρεσης των δεδομένων παιδιού.";
                return RedirectToAction("ErrorData", "Station", new { notify = msg });
            }
            if (cvm != null && ModelState.IsValid)
            {
                ΠΑΙΔΙΑ entity = db.ΠΑΙΔΙΑ.Find(childId);

                if (entity != null)
                {
                    entity.ΑΜ = cvm.ΑΜ;
                    entity.ΒΝΣ = stationId;
                    entity.ΕΠΩΝΥΜΟ = cvm.ΕΠΩΝΥΜΟ.Trim();
                    entity.ΟΝΟΜΑ = cvm.ΟΝΟΜΑ.Trim();
                    entity.ΠΑΤΡΩΝΥΜΟ = cvm.ΠΑΤΡΩΝΥΜΟ.Trim();
                    entity.ΜΗΤΡΩΝΥΜΟ = cvm.ΜΗΤΡΩΝΥΜΟ.Trim();
                    entity.ΦΥΛΟ = cvm.ΦΥΛΟ;
                    entity.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ = cvm.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ;
                    entity.ΑΦΜ = cvm.ΑΦΜ.HasValue() ? cvm.ΑΦΜ.Trim() : string.Empty;
                    entity.ΑΜΚΑ = cvm.ΑΜΚΑ;
                    entity.ΔΙΕΥΘΥΝΣΗ = cvm.ΔΙΕΥΘΥΝΣΗ;
                    entity.ΤΗΛΕΦΩΝΑ = cvm.ΤΗΛΕΦΩΝΑ;
                    entity.EMAIL = cvm.EMAIL;
                    entity.ΕΙΣΟΔΟΣ_ΗΜΝΙΑ = cvm.ΕΙΣΟΔΟΣ_ΗΜΝΙΑ;
                    entity.ΕΞΟΔΟΣ_ΗΜΝΙΑ = cvm.ΕΞΟΔΟΣ_ΗΜΝΙΑ;
                    entity.ΕΝΕΡΓΟΣ = cvm.ΕΝΕΡΓΟΣ;
                    entity.ΗΛΙΚΙΑ = c.CalculateChildAge(cvm);
                    entity.AGE = c.ChildAgeDecimal(cvm.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ, cvm.ΕΙΣΟΔΟΣ_ΗΜΝΙΑ);
                    entity.ΠΑΡΑΤΗΡΗΣΕΙΣ = cvm.ΠΑΡΑΤΗΡΗΣΕΙΣ;

                    db.Entry(entity).State = EntityState.Modified;
                    string ErrorMsg = ValidateChildFields(entity);
                    if (String.IsNullOrEmpty(ErrorMsg))
                    {
                        db.SaveChanges();
                        this.ShowMessage(MessageType.Success, "Η αποθήκευση ολοκληρώθηκε με επιτυχία.");
                        ChildViewModel newStudent = GetChildViewModelFromDB(childId);
                        return View(newStudent);
                    }
                    else
                    {
                        this.ShowMessage(MessageType.Error, "Η αποθήκευση απέτυχε λόγω επικύρωσης δεδομένων. " + ErrorMsg);
                        return View(cvm);
                    }
                }
            }
            this.ShowMessage(MessageType.Error, "Η αποθήκευση ακυρώθηκε λόγω σφαλμάτων καταχώρησης.");
            return View(cvm);
        }

        public ChildViewModel GetChildViewModelFromDB(int childId)
        {
            var data = (from d in db.ΠΑΙΔΙΑ
                        where d.CHILD_ID == childId
                        select new ChildViewModel
                        {
                            CHILD_ID = d.CHILD_ID,
                            ΑΜ = d.ΑΜ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΟ = d.ΕΠΩΝΥΜΟ,
                            ΟΝΟΜΑ = d.ΟΝΟΜΑ,
                            ΠΑΤΡΩΝΥΜΟ = d.ΠΑΤΡΩΝΥΜΟ,
                            ΜΗΤΡΩΝΥΜΟ = d.ΜΗΤΡΩΝΥΜΟ,
                            ΦΥΛΟ = d.ΦΥΛΟ ?? 0,
                            ΗΜΝΙΑ_ΓΕΝΝΗΣΗ = d.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ,
                            ΑΦΜ = d.ΑΦΜ,
                            ΑΜΚΑ = d.ΑΜΚΑ,
                            ΔΙΕΥΘΥΝΣΗ = d.ΔΙΕΥΘΥΝΣΗ,
                            ΤΗΛΕΦΩΝΑ = d.ΤΗΛΕΦΩΝΑ,
                            EMAIL = d.EMAIL,
                            ΗΛΙΚΙΑ = d.ΗΛΙΚΙΑ,
                            AGE = d.AGE ?? 0.0m,
                            ΕΝΕΡΓΟΣ = d.ΕΝΕΡΓΟΣ ?? true,
                            ΕΙΣΟΔΟΣ_ΗΜΝΙΑ = d.ΕΙΣΟΔΟΣ_ΗΜΝΙΑ,
                            ΕΞΟΔΟΣ_ΗΜΝΙΑ = d.ΕΞΟΔΟΣ_ΗΜΝΙΑ,
                            ΠΑΡΑΤΗΡΗΣΕΙΣ = d.ΠΑΡΑΤΗΡΗΣΕΙΣ
                        }
                        ).FirstOrDefault();

            return (data);
        }

        public string ValidateChildFields(ΠΑΙΔΙΑ s)
        {
            //int err = 0;
            //int tempi = 0;
            string errMsg = "";

            //if (!c.ValidateBirthdate(s)) errMsg += "-> Η ημ/νια γέννησης είναι εκτός λογικών ορίων. ";
            if (!c.CheckAFM(s.ΑΦΜ)) errMsg += "-> Το ΑΦΜ δεν είναι έγκυρο. ";
            return (errMsg);
        }

        #endregion

        #endregion ΒΡΕΦΟΝΗΠΙΑ - ΣΤΟΙΧΕΙΑ ΚΑΙ ΕΓΓΡΑΦΕΣ

        #region ΜΗΤΡΩΟ ΒΡΕΦΟΝΗΠΙΩΝ

        public ActionResult ChildrenInfoList(string notify = null)
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

            IEnumerable<sqlChildInfoViewModel> cvm = GetChildrenInfo();

            if (cvm.Count() == 0)
            {
                string msg = "Δεν υπάρχουν καταχωρημένα παιδιά για την εμφάνιση του μητρώου.";
                return RedirectToAction("Index", "Station", new { notify = msg });
            }
            sqlChildInfoViewModel child = cvm.First();

            if (notify != null)
            {
                this.ShowMessage(MessageType.Info, notify);
            }
            return View(child);
        }

        public ActionResult ChildrenInfo_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<sqlChildInfoViewModel> data = GetChildrenInfo();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public ActionResult EgrafesInfo_Read(int childId, [DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<sqlEgrafesInfoViewModel> egrafes = GetEgrafesInfo(childId);

            var result = new JsonResult();
            result.Data = egrafes.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<sqlChildInfoViewModel> GetChildrenInfo()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.sqlCHILDREN_INFO
                        where d.ΣΤΑΘΜΟΣ_ΚΩΔ == stationId
                        orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ
                        select new sqlChildInfoViewModel
                        {
                            CHILD_ID = d.CHILD_ID,
                            ΑΜ = d.ΑΜ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΠΕΡΙΦΕΡΕΙΑΚΗ = d.ΠΕΡΙΦΕΡΕΙΑΚΗ,
                            ΟΝΟΜΑΤΕΠΩΝΥΜΟ = d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ,
                            ΠΑΤΡΩΝΥΜΟ = d.ΠΑΤΡΩΝΥΜΟ,
                            ΜΗΤΡΩΝΥΜΟ = d.ΜΗΤΡΩΝΥΜΟ,
                            ΦΥΛΟ = d.ΦΥΛΟ,
                            ΗΜΝΙΑ_ΓΕΝΝΗΣΗ = d.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ,
                            ΗΛΙΚΙΑ = d.ΗΛΙΚΙΑ,
                            ΔΙΕΥΘΥΝΣΗ = d.ΔΙΕΥΘΥΝΣΗ,
                            ΤΗΛΕΦΩΝΑ = d.ΤΗΛΕΦΩΝΑ,
                            EMAIL = d.EMAIL
                        }).ToList();

            return (data);
        }

        public List<sqlEgrafesInfoViewModel> GetEgrafesInfo(int childId)
        {
            var data = (from d in db.sqlEGRAFES_INFO
                        orderby d.ΗΜΝΙΑ_ΕΓΓΡΑΦΗ
                        where d.ΠΑΙΔΙ_ΚΩΔ == childId
                        select new sqlEgrafesInfoViewModel
                        {
                            ΕΓΓΡΑΦΗ_ΚΩΔ = d.ΕΓΓΡΑΦΗ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΠΑΙΔΙ_ΚΩΔ = d.ΠΑΙΔΙ_ΚΩΔ,
                            ΤΜΗΜΑ_ΟΝΟΜΑ = d.ΤΜΗΜΑ_ΟΝΟΜΑ,
                            ΗΜΝΙΑ_ΕΓΓΡΑΦΗ = d.ΗΜΝΙΑ_ΕΓΓΡΑΦΗ,
                            ΗΜΝΙΑ_ΠΕΡΑΣ = d.ΗΜΝΙΑ_ΠΕΡΑΣ
                        }).ToList();

            return (data);
        }

        public PartialViewResult GetChildRecord(int childId)
        {
            var childModel = new sqlChildInfoViewModel();

            IEnumerable<sqlChildInfoViewModel> children = GetChildrenInfo();

            var child = children.Where(e => e.CHILD_ID == childId).FirstOrDefault();

            //Set default child records
            childModel.CHILD_ID = child.CHILD_ID;
            childModel.ΑΜ = child.ΑΜ;
            childModel.ΒΝΣ = child.ΒΝΣ;
            childModel.ΟΝΟΜΑΤΕΠΩΝΥΜΟ = child.ΟΝΟΜΑΤΕΠΩΝΥΜΟ;
            childModel.ΠΕΡΙΦΕΡΕΙΑΚΗ = child.ΠΕΡΙΦΕΡΕΙΑΚΗ;
            childModel.ΠΑΤΡΩΝΥΜΟ = child.ΠΑΤΡΩΝΥΜΟ;
            childModel.ΜΗΤΡΩΝΥΜΟ = child.ΜΗΤΡΩΝΥΜΟ;
            childModel.ΔΙΕΥΘΥΝΣΗ = child.ΔΙΕΥΘΥΝΣΗ;
            childModel.ΤΗΛΕΦΩΝΑ = child.ΤΗΛΕΦΩΝΑ;
            childModel.EMAIL = child.EMAIL;
            childModel.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ = child.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ;
            childModel.ΗΛΙΚΙΑ = child.ΗΛΙΚΙΑ;
            childModel.ΦΥΛΟ = child.ΦΥΛΟ;

            return PartialView("ChildrenInfoPartial", childModel);
        }

        #endregion

        #region ΤΜΗΜΑΤΑ ΒΡΕΦΟΝΗΠΙΩΝ

        public ActionResult Tmimata_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = GetTmimataInfoFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public ActionResult TmimaChildren(string notify = null)
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

            if (!SchoolYearsExist())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει πρώτα να καταχωρηθούν σχολικά έτη.";
                return RedirectToAction("Index", "Station", new { notify = msg });
            }

            populateSchoolYears();
            return View();
        }

        public ActionResult TmimaChildren_Read([DataSourceRequest] DataSourceRequest request, int tmimaId = 0)
        {
            List<sqlTmimaChildViewModel> data = GetTmimaChildren(tmimaId);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<sqlTmimaChildViewModel> GetTmimaChildren(int tmimaId = 0)
        {
            var data = (from d in db.sqlTMIMA_CHILDREN
                        where d.ΤΜΗΜΑ == tmimaId
                        orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ
                        select new sqlTmimaChildViewModel
                        {
                            CHILD_ID = d.CHILD_ID,
                            ΑΜ = d.ΑΜ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΟΝΟΜΑΤΕΠΩΝΥΜΟ = d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ,
                            ΠΑΤΡΩΝΥΜΟ = d.ΠΑΤΡΩΝΥΜΟ,
                            ΦΥΛΟ = d.ΦΥΛΟ,
                            ΗΛΙΚΙΑ = d.ΗΛΙΚΙΑ,
                            ΗΜΝΙΑ_ΕΓΓΡΑΦΗ = d.ΗΜΝΙΑ_ΕΓΓΡΑΦΗ
                        }).ToList();

            return (data);
        }

        public ActionResult DocKatastasiGenikiPrint(int tmimaId)
        {
            var data = (from d in db.sqlTMIMATA_INFO
                        where d.ΤΜΗΜΑ_ΚΩΔ == tmimaId
                        select new sqlTmimaInfoViewModel
                        {
                            ΤΜΗΜΑ_ΚΩΔ = d.ΤΜΗΜΑ_ΚΩΔ,
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            ΒΝΣ = d.ΒΝΣ
                        }).FirstOrDefault();

            return View(data);
        }

        #endregion

        #region --- ΠΡΟΣΩΠΙΚΟ ΣΤΑΘΜΟΥ ---

        #region ΠΡΟΣΩΠΙΚΟ (20-07-2019)

        public ActionResult PersonnelData(string notify = null)
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

            if (!StationsExist() || !PersonnelTypesExist() || !TmimataExist() || !SchoolYearsExist())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει πρώτα να ορίσετε ΒΝΣ, κατηγορίες προσωπικού, τμήματα και σχολικά έτη στις Ρυθμίσεις.";
                return RedirectToAction("Index", "Station", new { notify = msg });
            }

            populateStations();
            populatePersonnelTypes();
            populateSchoolYears();
            populateTmimata();
            return View();
        }

        public PersonnelGridViewModel RefreshPersonnelFromDB(int recordId)
        {
            var data = (from d in db.ΠΡΟΣΩΠΙΚΟ
                        where d.PERSONNEL_ID == recordId
                        select new PersonnelGridViewModel
                        {
                            PERSONNEL_ID = d.PERSONNEL_ID,
                            ΜΗΤΡΩΟ = d.ΜΗΤΡΩΟ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΟ = d.ΕΠΩΝΥΜΟ,
                            ΟΝΟΜΑ = d.ΟΝΟΜΑ,
                            ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ = d.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ ?? 0
                        }).FirstOrDefault();

            return (data);
        }

        public List<PersonnelGridViewModel> GetPersonnelFromDB()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΠΡΟΣΩΠΙΚΟ
                        where d.ΒΝΣ == stationId
                        orderby d.ΕΠΩΝΥΜΟ, d.ΟΝΟΜΑ
                        select new PersonnelGridViewModel
                        {
                            PERSONNEL_ID = d.PERSONNEL_ID,
                            ΜΗΤΡΩΟ = d.ΜΗΤΡΩΟ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΟ = d.ΕΠΩΝΥΜΟ,
                            ΟΝΟΜΑ = d.ΟΝΟΜΑ,
                            ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ = d.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ ?? 0
                        }).ToList();

            return (data);
        }

        public List<EducatorTmimaViewModel> GetEducatorTmimaFromDB(int personId)
        {
            var data = (from d in db.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ
                        where d.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ == personId
                        orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ, d.ΤΜΗΜΑ_ΚΩΔ
                        select new EducatorTmimaViewModel
                        {
                            RECORD_ID = d.RECORD_ID,
                            ΒΝΣ = d.ΒΝΣ,
                            ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ = d.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΤΜΗΜΑ_ΚΩΔ = d.ΤΜΗΜΑ_ΚΩΔ
                        }).ToList();

            return data;
        }

        public EducatorTmimaViewModel RefreshEducatorTmimaFromDB(int recordId)
        {
            var data = (from d in db.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ
                        where d.RECORD_ID == recordId
                        select new EducatorTmimaViewModel
                        {
                            RECORD_ID = d.RECORD_ID,
                            ΒΝΣ = d.ΒΝΣ,
                            ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ = d.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΤΜΗΜΑ_ΚΩΔ = d.ΤΜΗΜΑ_ΚΩΔ
                        }).FirstOrDefault();

            return data;
        }


        #region PERSONNEL GRID CRUD

        public ActionResult Personnel_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<PersonnelGridViewModel> data = GetPersonnelFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Personnel_Create([DataSourceRequest] DataSourceRequest request, PersonnelGridViewModel data)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var newdata = new PersonnelGridViewModel();

            if (!k.ValidatePrimaryKeyPerson((int)data.ΜΗΤΡΩΟ, (int)data.ΒΝΣ)) 
                ModelState.AddModelError("", "Ο Α.Μ. που δώθηκε είναι ήδη καταχωρημένος για το σταθμό αυτό.");
            if (!k.IsValidAM(data))
                ModelState.AddModelError("", "Ο Α.Μ. μπορεί να έχει μηδενική τιμή μόνο για ασκούμενους.");

            if (ModelState.IsValid)
            {
                ΠΡΟΣΩΠΙΚΟ entity = new ΠΡΟΣΩΠΙΚΟ()
                {
                    ΜΗΤΡΩΟ = data.ΜΗΤΡΩΟ,
                    ΒΝΣ = stationId,
                    ΕΠΩΝΥΜΟ = data.ΕΠΩΝΥΜΟ.Trim(),
                    ΟΝΟΜΑ = data.ΟΝΟΜΑ.Trim(),
                    ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ = data.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ
                };
                db.ΠΡΟΣΩΠΙΚΟ.Add(entity);
                db.SaveChanges();
                data.PERSONNEL_ID = entity.PERSONNEL_ID;
                newdata = RefreshPersonnelFromDB(data.PERSONNEL_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Personnel_Update([DataSourceRequest] DataSourceRequest request, PersonnelGridViewModel data)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var newdata = new PersonnelGridViewModel();

            if (!k.IsValidAM(data))
                ModelState.AddModelError("", "Ο Α.Μ. μπορεί να έχει μηδενική τιμή μόνο για ασκούμενους.");

            if (ModelState.IsValid)
            {
                ΠΡΟΣΩΠΙΚΟ entity = db.ΠΡΟΣΩΠΙΚΟ.Find(data.PERSONNEL_ID);

                entity.ΜΗΤΡΩΟ = data.ΜΗΤΡΩΟ;
                entity.ΒΝΣ = stationId;
                entity.ΕΠΩΝΥΜΟ = data.ΕΠΩΝΥΜΟ.Trim();
                entity.ΟΝΟΜΑ = data.ΟΝΟΜΑ.Trim();
                entity.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ = data.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newdata = RefreshPersonnelFromDB(entity.PERSONNEL_ID);
            }

            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Personnel_Destroy([DataSourceRequest] DataSourceRequest request, PersonnelGridViewModel data)
        {
            if (data != null)
            {
                ΠΡΟΣΩΠΙΚΟ entity = db.ΠΡΟΣΩΠΙΚΟ.Find(data.PERSONNEL_ID);
                if (!k.CanDeletePerson(data.PERSONNEL_ID)) ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί ο εργαζόμενος διότι είναι σε χρήση σε τμήματα ή/και στο πρόγραμμα.");
                if (ModelState.IsValid)
                {
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.ΠΡΟΣΩΠΙΚΟ.Remove(entity);
                        db.SaveChanges();
                    }
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region EDUCATOR_TMIMA GRID CRUD

        public ActionResult EducatorTmima_Read([DataSourceRequest] DataSourceRequest request, int personId = 0)
        {
            var data = GetEducatorTmimaFromDB(personId);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EducatorTmima_Create([DataSourceRequest] DataSourceRequest request, EducatorTmimaViewModel data, int personId = 0)
        {
            EducatorTmimaViewModel newdata = new EducatorTmimaViewModel();
            int stationId = c.GetStationFromfPersonID(personId);

            if (!c.IsEducator(personId))
            {
                ModelState.AddModelError("", "Ο επιλεγμένος εργαζόμενος δεν είναι παιδαγωγός. Η καταχώρηση ακυρώθηκε.");
                return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }

            if (personId > 0 && stationId > 0)
            {
                var existingData = db.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ.Where(s => s.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ == personId && s.ΤΜΗΜΑ_ΚΩΔ == data.ΤΜΗΜΑ_ΚΩΔ).ToList();
                if (existingData.Count > 0) ModelState.AddModelError("", "Υπάρχει ήδη καταχώρηση του παιδαγωγού στο τμήμα αυτό.");

                if (data != null && ModelState.IsValid)
                {
                    ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ entity = new ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ()
                    {
                        ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ = personId,
                        ΒΝΣ = stationId,
                        ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                        ΤΜΗΜΑ_ΚΩΔ = data.ΤΜΗΜΑ_ΚΩΔ,
                    };
                    db.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ.Add(entity);
                    db.SaveChanges();
                    data.RECORD_ID = entity.RECORD_ID;   // grid requires the new generated id
                    newdata = RefreshEducatorTmimaFromDB(entity.RECORD_ID);
                }
            }
            else
            {
                ModelState.AddModelError("", "Πρέπει να προηγηθεί επιλογή κάποιου παιδαγωγού για ανάθεση σε τμήμα.");
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EducatorTmima_Update([DataSourceRequest] DataSourceRequest request, EducatorTmimaViewModel data, int personId = 0)
        {
            EducatorTmimaViewModel newdata = new EducatorTmimaViewModel();
            int stationId = c.GetStationFromfPersonID(personId);

            if (!c.IsEducator(personId))
            {
                ModelState.AddModelError("", "Ο επιλεγμένος εργαζόμενος δεν είναι παιδαγωγός. Η καταχώρηση ακυρώθηκε.");
                return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }

            if (personId > 0 && stationId > 0)
            {
                if (data != null && ModelState.IsValid)
                {
                    ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ entity = db.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ.Find(data.RECORD_ID);

                    entity.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ = personId;
                    entity.ΒΝΣ = stationId;
                    entity.ΣΧΟΛΙΚΟ_ΕΤΟΣ = data.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
                    entity.ΤΜΗΜΑ_ΚΩΔ = data.ΤΜΗΜΑ_ΚΩΔ;

                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                    newdata = RefreshEducatorTmimaFromDB(entity.RECORD_ID);
                }
            }
            else
            {
                ModelState.AddModelError("", "Πρέπει πρώτα να γίνει επιλογή κάποιου παιδαγωγού. Η ενημέρωση ακυρώθηκε.");
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EducatorTmima_Destroy([DataSourceRequest] DataSourceRequest request, EducatorTmimaViewModel data)
        {
            if (data != null)
            {
                ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ entity = db.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ.Find(data.RECORD_ID);
                if (entity != null)
                {
                    db.Entry(entity).State = EntityState.Deleted;
                    db.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ.Remove(entity);
                    db.SaveChanges();
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PERSONNEL DATA FORM

        public ActionResult PersonnelEdit(int? personId)
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
            if (!(personId > 0))
            {
                string msg = "Ο κωδικός εργαζομένου δεν είναι έγκυρος. Η εγγραφή πρέπει να έχει αποθηκευτεί πρώτα.";
                return RedirectToAction("ErrorData", "Station", new { notify = msg });
            }

            int personID = (int)personId;
            PersonnelViewModel person = GetPersonViewModelFromDB(personID);
            if (person == null)
            {
                string msg = "Παρουσιάστηκε πρόβλημε εύρεσης του εργαζομένου.";
                return RedirectToAction("ErrorData", "Station", new { notify = msg });
            }

            return View(person);
        }

        [HttpPost]
        public ActionResult PersonnelEdit(int personId, PersonnelViewModel pvm)
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

            if (pvm != null && ModelState.IsValid)
            {
                ΠΡΟΣΩΠΙΚΟ entity = db.ΠΡΟΣΩΠΙΚΟ.Find(personId);
                if (entity != null)
                {
                    entity.ΜΗΤΡΩΟ = pvm.ΜΗΤΡΩΟ;
                    entity.ΑΦΜ = pvm.ΑΦΜ;
                    entity.ΒΝΣ = stationId;
                    entity.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ = pvm.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ;
                    entity.ΚΛΑΔΟΣ = pvm.ΚΛΑΔΟΣ;
                    entity.ΕΙΔΙΚΟΤΗΤΑ = pvm.ΕΙΔΙΚΟΤΗΤΑ;
                    entity.ΕΠΩΝΥΜΟ = pvm.ΕΠΩΝΥΜΟ.Trim();
                    entity.ΟΝΟΜΑ = pvm.ΟΝΟΜΑ.Trim();
                    entity.ΠΑΤΡΩΝΥΜΟ = pvm.ΠΑΤΡΩΝΥΜΟ != null ? pvm.ΠΑΤΡΩΝΥΜΟ.Trim() : pvm.ΠΑΤΡΩΝΥΜΟ;
                    entity.ΜΗΤΡΩΝΥΜΟ = pvm.ΜΗΤΡΩΝΥΜΟ != null ? pvm.ΜΗΤΡΩΝΥΜΟ.Trim() : pvm.ΜΗΤΡΩΝΥΜΟ;
                    entity.ΦΥΛΟ_ΚΩΔ = pvm.ΦΥΛΟ_ΚΩΔ;
                    entity.ΕΤΟΣ_ΓΕΝΝΗΣΗ = pvm.ΕΤΟΣ_ΓΕΝΝΗΣΗ;
                    entity.ΔΙΕΥΘΥΝΣΗ = pvm.ΔΙΕΥΘΥΝΣΗ;
                    entity.ΤΗΛΕΦΩΝΑ = pvm.ΤΗΛΕΦΩΝΑ;
                    entity.EMAIL = pvm.EMAIL;
                    entity.ΗΛΙΚΙΑ = c.CalculatePersonAge(pvm);
                    entity.ΒΑΘΜΟΣ = pvm.ΒΑΘΜΟΣ;
                    entity.ΑΠΟΦΑΣΗ_ΦΕΚ = pvm.ΑΠΟΦΑΣΗ_ΦΕΚ;
                    entity.ΠΑΡΑΤΗΡΗΣΕΙΣ = pvm.ΠΑΡΑΤΗΡΗΣΕΙΣ;
                    entity.ΑΠΟΧΩΡΗΣΕ = pvm.ΑΠΟΧΩΡΗΣΕ;
                    entity.ΑΠΟΧΩΡΗΣΗ_ΗΜΝΙΑ = pvm.ΑΠΟΧΩΡΗΣΗ_ΗΜΝΙΑ;
                    entity.ΑΠΟΧΩΡΗΣΗ_ΑΙΤΙΑ = pvm.ΑΠΟΧΩΡΗΣΗ_ΑΙΤΙΑ;

                    db.Entry(entity).State = EntityState.Modified;
                    string ErrorMsg = k.ValidatePersonFields(entity);
                    if (String.IsNullOrEmpty(ErrorMsg))
                    {
                        db.SaveChanges();
                        this.ShowMessage(MessageType.Success, "Η αποθήκευση ολοκληρώθηκε με επιτυχία.");
                        PersonnelViewModel newPerson = GetPersonViewModelFromDB(personId);
                        return View(newPerson);
                    }
                    else
                    {
                        this.ShowMessage(MessageType.Error, "Η αποθήκευση απέτυχε λόγω επικύρωσης δεδομένων. " + ErrorMsg);
                        return View(pvm);
                    }
                }
            }
            this.ShowMessage(MessageType.Error, "Η αποθήκευση ακυρώθηκε λόγω σφαλμάτων καταχώρησης.");
            return View(pvm);
        }

        public PersonnelViewModel GetPersonViewModelFromDB(int personId)
        {

            var data = (from d in db.ΠΡΟΣΩΠΙΚΟ
                        where d.PERSONNEL_ID == personId
                        select new PersonnelViewModel
                        {
                            PERSONNEL_ID = d.PERSONNEL_ID,
                            ΒΝΣ = d.ΒΝΣ,
                            ΜΗΤΡΩΟ = d.ΜΗΤΡΩΟ,
                            ΑΦΜ = d.ΑΦΜ,
                            ΕΠΩΝΥΜΟ = d.ΕΠΩΝΥΜΟ,
                            ΟΝΟΜΑ = d.ΟΝΟΜΑ,
                            ΦΥΛΟ_ΚΩΔ = d.ΦΥΛΟ_ΚΩΔ,
                            ΠΑΤΡΩΝΥΜΟ = d.ΠΑΤΡΩΝΥΜΟ,
                            ΜΗΤΡΩΝΥΜΟ = d.ΜΗΤΡΩΝΥΜΟ,
                            ΔΙΕΥΘΥΝΣΗ = d.ΔΙΕΥΘΥΝΣΗ,
                            ΤΗΛΕΦΩΝΑ = d.ΤΗΛΕΦΩΝΑ,
                            EMAIL = d.EMAIL,
                            ΕΤΟΣ_ΓΕΝΝΗΣΗ = d.ΕΤΟΣ_ΓΕΝΝΗΣΗ,
                            ΗΛΙΚΙΑ = d.ΗΛΙΚΙΑ,
                            ΚΛΑΔΟΣ = d.ΚΛΑΔΟΣ,
                            ΕΙΔΙΚΟΤΗΤΑ = d.ΕΙΔΙΚΟΤΗΤΑ,
                            ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ = d.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ,
                            ΑΠΟΦΑΣΗ_ΦΕΚ = d.ΑΠΟΦΑΣΗ_ΦΕΚ,
                            ΒΑΘΜΟΣ = d.ΒΑΘΜΟΣ,
                            ΠΑΡΑΤΗΡΗΣΕΙΣ = d.ΠΑΡΑΤΗΡΗΣΕΙΣ,
                            ΑΠΟΧΩΡΗΣΕ = d.ΑΠΟΧΩΡΗΣΕ ?? false,
                            ΑΠΟΧΩΡΗΣΗ_ΑΙΤΙΑ = d.ΑΠΟΧΩΡΗΣΗ_ΑΙΤΙΑ,
                            ΑΠΟΧΩΡΗΣΗ_ΗΜΝΙΑ = d.ΑΠΟΧΩΡΗΣΗ_ΗΜΝΙΑ
                        }).FirstOrDefault();

            return (data);
        }


        #endregion

        #endregion

        #region ΜΗΤΡΩΟ ΠΡΟΣΩΠΙΚΟΥ

        public ActionResult PersonnelInfoList(string notify = null)
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

            IEnumerable<sqlPersonInfoViewModel> pvm = GetPersonnelInfo();
            if (pvm.Count() == 0)
            {
                string msg = "Δεν υπάρχουν καταχωρημένοι εργαζόμενοι για την εμφάνιση του μητρώου.";
                return RedirectToAction("Index", "Station", new { notify = msg });
            }
            sqlPersonInfoViewModel person = pvm.First();

            if (notify != null)
            {
                this.ShowMessage(MessageType.Info, notify);
            }
            return View(person);
        }

        public ActionResult PersonnelInfo_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<sqlPersonInfoViewModel> data = GetPersonnelInfo();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public ActionResult EducatorTmimaInfo_Read(int personId, [DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<sqlEducatorTmimaInfoViewModel> data = GetEducatorTmimaInfo(personId);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<sqlPersonInfoViewModel> GetPersonnelInfo()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.sqlPERSONNEL_INFO
                        where d.ΣΤΑΘΜΟΣ_ΚΩΔ == stationId
                        orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ
                        select new sqlPersonInfoViewModel
                        {
                            PERSONNEL_ID = d.PERSONNEL_ID,
                            ΜΗΤΡΩΟ = d.ΜΗΤΡΩΟ,
                            ΑΦΜ = d.ΑΦΜ,
                            EIDIKOTITA_CODE = d.EIDIKOTITA_CODE,
                            EIDIKOTITA_TEXT = d.EIDIKOTITA_TEXT,
                            ΟΝΟΜΑΤΕΠΩΝΥΜΟ = d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ,
                            ΠΑΤΡΩΝΥΜΟ = d.ΠΑΤΡΩΝΥΜΟ,
                            ΜΗΤΡΩΝΥΜΟ = d.ΜΗΤΡΩΝΥΜΟ,
                            ΦΥΛΟ = d.ΦΥΛΟ,
                            PROSOPIKO_TEXT = d.PROSOPIKO_TEXT,
                            ΗΛΙΚΙΑ = d.ΗΛΙΚΙΑ,
                            ΔΙΕΥΘΥΝΣΗ = d.ΔΙΕΥΘΥΝΣΗ,
                            ΤΗΛΕΦΩΝΑ = d.ΤΗΛΕΦΩΝΑ,
                            EMAIL = d.EMAIL,
                            ΒΝΣ_ΕΠΩΝΥΜΙΑ = d.ΒΝΣ_ΕΠΩΝΥΜΙΑ,
                            ΕΤΟΣ_ΓΕΝΝΗΣΗ = d.ΕΤΟΣ_ΓΕΝΝΗΣΗ,
                            ΣΤΑΘΜΟΣ_ΚΩΔ = d.ΣΤΑΘΜΟΣ_ΚΩΔ,
                            ΠΕΡΙΦΕΡΕΙΑΚΗ = d.ΠΕΡΙΦΕΡΕΙΑΚΗ
                        }).ToList();

            return (data);
        }

        public List<sqlEducatorTmimaInfoViewModel> GetEducatorTmimaInfo(int personId)
        {
            var data = (from d in db.sqlEDUCATOR_TMIMA_INFO
                        orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ, d.ΤΜΗΜΑ_ΟΝΟΜΑ
                        where d.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ == personId
                        select new sqlEducatorTmimaInfoViewModel
                        {
                            RECORD_ID = d.RECORD_ID,
                            ΒΝΣ = d.ΒΝΣ,
                            ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ = d.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ,
                            ΤΜΗΜΑ_ΟΝΟΜΑ = d.ΤΜΗΜΑ_ΟΝΟΜΑ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                        }).ToList();

            return (data);
        }

        public PartialViewResult GetPersonnelRecord(int personId)
        {
            var personModel = new sqlPersonInfoViewModel();

            IEnumerable<sqlPersonInfoViewModel> persons = GetPersonnelInfo();

            var person = persons.Where(e => e.PERSONNEL_ID == personId).FirstOrDefault();

            //Set default child records
            personModel.PERSONNEL_ID = person.PERSONNEL_ID;
            personModel.ΜΗΤΡΩΟ = person.ΜΗΤΡΩΟ;
            personModel.ΑΦΜ = person.ΑΦΜ;
            personModel.EIDIKOTITA_CODE = person.EIDIKOTITA_CODE;
            personModel.EIDIKOTITA_TEXT = person.EIDIKOTITA_TEXT;
            personModel.ΟΝΟΜΑΤΕΠΩΝΥΜΟ = person.ΟΝΟΜΑΤΕΠΩΝΥΜΟ;
            personModel.ΠΑΤΡΩΝΥΜΟ = person.ΠΑΤΡΩΝΥΜΟ;
            personModel.ΜΗΤΡΩΝΥΜΟ = person.ΜΗΤΡΩΝΥΜΟ;
            personModel.ΦΥΛΟ = person.ΦΥΛΟ;
            personModel.PROSOPIKO_TEXT = person.PROSOPIKO_TEXT;
            personModel.ΗΛΙΚΙΑ = person.ΗΛΙΚΙΑ;
            personModel.ΔΙΕΥΘΥΝΣΗ = person.ΔΙΕΥΘΥΝΣΗ;
            personModel.ΤΗΛΕΦΩΝΑ = person.ΤΗΛΕΦΩΝΑ;
            personModel.EMAIL = person.EMAIL;
            personModel.ΒΝΣ_ΕΠΩΝΥΜΙΑ = person.ΒΝΣ_ΕΠΩΝΥΜΙΑ;
            personModel.ΕΤΟΣ_ΓΕΝΝΗΣΗ = person.ΕΤΟΣ_ΓΕΝΝΗΣΗ;
            personModel.ΣΤΑΘΜΟΣ_ΚΩΔ = person.ΣΤΑΘΜΟΣ_ΚΩΔ;
            personModel.ΠΕΡΙΦΕΡΕΙΑΚΗ = person.ΠΕΡΙΦΕΡΕΙΑΚΗ;

            return PartialView("PersonnelInfoPartial", personModel);
        }

        #endregion

        #region ΑΝΑΘΕΣΕΙΣ ΠΑΙΔΑΓΩΓΩΝ

        public ActionResult AnatheseisData(string notify = null)
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

            if (!SchoolYearsExist() || !EducatorsExist())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει πρώτα να καταχωρηθούν σχολικά έτη και παιδαγωγοί.";
                return RedirectToAction("Index", "Station", new { notify = msg });
            }

            populateEducators();
            populateSchoolYears();
            return View();
        }

        public List<sqlTmimaInfoViewModel> GetTmimataInfoFromDB()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.sqlTMIMATA_INFO
                        where d.ΒΝΣ == stationId
                        orderby d.ΣΧΟΛΙΚΟ_ΕΤΟΣ descending, d.ΤΜΗΜΑ_ΟΝΟΜΑ
                        select new sqlTmimaInfoViewModel
                        {
                            ΤΜΗΜΑ_ΚΩΔ = d.ΤΜΗΜΑ_ΚΩΔ,
                            ΤΜΗΜΑ_ΟΝΟΜΑ = d.ΤΜΗΜΑ_ΟΝΟΜΑ,
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            ΒΝΣ = d.ΒΝΣ,
                            ΒΝΣ_ΟΝΟΜΑ = d.ΒΝΣ_ΟΝΟΜΑ,
                            ΠΕΡΙΦΕΡΕΙΑΚΗ = d.ΠΕΡΙΦΕΡΕΙΑΚΗ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ
                        }).ToList();

            return (data);
        }

        public List<EducatorTmimaViewModel> GetTmimaEducator(int tmimaId)
        {
            var data = (from d in db.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ
                        where d.ΤΜΗΜΑ_ΚΩΔ == tmimaId
                        orderby d.ΠΡΟΣΩΠΙΚΟ.ΕΠΩΝΥΜΟ, d.ΠΡΟΣΩΠΙΚΟ.ΟΝΟΜΑ
                        select new EducatorTmimaViewModel
                        {
                            RECORD_ID = d.RECORD_ID,
                            ΒΝΣ = d.ΒΝΣ,
                            ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ = d.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΤΜΗΜΑ_ΚΩΔ = d.ΤΜΗΜΑ_ΚΩΔ
                        }).ToList();

            return data;
        }

        public TmimaViewModel GetTmimaDataFromDB(int tmimaId)
        {
            var data = (from d in db.ΤΜΗΜΑ
                        where d.ΤΜΗΜΑ_ΚΩΔ == tmimaId
                        select new TmimaViewModel
                        {
                            ΤΜΗΜΑ_ΚΩΔ = d.ΤΜΗΜΑ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΟΝΟΜΑΣΙΑ = d.ΟΝΟΜΑΣΙΑ
                        }).FirstOrDefault();

            return (data);
        }

        #region TMIMA-EDUCATOR GRID CRUD

        public ActionResult TmimaEducator_Read([DataSourceRequest] DataSourceRequest request, int tmimaId = 0)
        {
            var data = GetTmimaEducator(tmimaId);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TmimaEducator_Create([DataSourceRequest] DataSourceRequest request, EducatorTmimaViewModel data, int tmimaId = 0)
        {
            EducatorTmimaViewModel newdata = new EducatorTmimaViewModel();

            TmimaViewModel tmima = GetTmimaDataFromDB(tmimaId);

            if (tmimaId > 0 && tmima != null)
            {
                var existingData = db.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ.Where(s => s.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ == data.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ && s.ΤΜΗΜΑ_ΚΩΔ == tmimaId).ToList();
                if (existingData.Count > 0) ModelState.AddModelError("", "Υπάρχει ήδη καταχώρηση του παιδαγωγού στο τμήμα αυτό.");

                if (data != null && ModelState.IsValid)
                {
                    ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ entity = new ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ()
                    {
                        ΤΜΗΜΑ_ΚΩΔ = tmimaId,
                        ΒΝΣ = tmima.ΒΝΣ,
                        ΣΧΟΛΙΚΟ_ΕΤΟΣ = tmima.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                        ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ = data.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ
                    };
                    db.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ.Add(entity);
                    db.SaveChanges();
                    data.RECORD_ID = entity.RECORD_ID;   // grid requires the new generated id
                    newdata = RefreshEducatorTmimaFromDB(entity.RECORD_ID);
                }
            }
            else
            {
                ModelState.AddModelError("", "Πρέπει να προηγηθεί επιλογή κάποιου τμήματος. Η ενημέρωση ακυρώθηκε.");
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TmimaEducator_Update([DataSourceRequest] DataSourceRequest request, EducatorTmimaViewModel data, int tmimaId = 0)
        {
            EducatorTmimaViewModel newdata = new EducatorTmimaViewModel();

            TmimaViewModel tmima = GetTmimaDataFromDB(tmimaId);

            if (tmimaId > 0 && tmima != null)
            {
                if (data != null && ModelState.IsValid)
                {
                    ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ entity = db.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ.Find(data.RECORD_ID);

                    entity.ΤΜΗΜΑ_ΚΩΔ = tmimaId;
                    entity.ΒΝΣ = tmima.ΒΝΣ;
                    entity.ΣΧΟΛΙΚΟ_ΕΤΟΣ = tmima.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
                    entity.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ = data.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ;

                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                    newdata = RefreshEducatorTmimaFromDB(entity.RECORD_ID);
                }
            }
            else
            {
                ModelState.AddModelError("", "Πρέπει πρώτα να γίνει επιλογή κάποιου τμήματος. Η ενημέρωση ακυρώθηκε.");
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TmimaEducator_Destroy([DataSourceRequest] DataSourceRequest request, EducatorTmimaViewModel data)
        {
            if (data != null)
            {
                ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ entity = db.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ.Find(data.RECORD_ID);
                if (entity != null)
                {
                    db.Entry(entity).State = EntityState.Deleted;
                    db.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ.Remove(entity);
                    db.SaveChanges();
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region ΜΕΤΑΒΟΛΕΣ ΠΡΟΣΩΠΙΚΟΥ - ΚΑΤΑΧΩΡΗΣΗ

        public ActionResult PersonnelMetaboles(string notify = null)
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

            if (!PersonnelTypesExist() || !MetabolesTypesExist() || !SchoolYearsExist())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει πρώτα να ορίσετε κατηγορίες προσωπικού, είδη μεταβολών και σχολικά έτη στις Ρυθμίσεις.";
                return RedirectToAction("Index", "Station", new { notify = msg });
            }

            populatePersonnelTypes();
            populateMetabolesTypes();
            return View();
        }

        #region GRID CRUD FUNCTIONS

        public ActionResult Metaboli_Read([DataSourceRequest] DataSourceRequest request, int schoolyearId = 0, int personId = 0)
        {
            List<PersonnelMetaboliViewModel> data = GetPersonnelMetabolesFromDB(schoolyearId, personId);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Metaboli_Create([DataSourceRequest] DataSourceRequest request, PersonnelMetaboliViewModel data, int schoolyearId = 0, int personId = 0)
        {
            var newdata = new PersonnelMetaboliViewModel();

            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (!(schoolyearId > 0) || !(personId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε εργαζόμενο και σχολικό έτος. Η καταχώρηση ακυρώθηκε.");

            if (data.ΗΜΝΙΑ_ΑΠΟ > data.ΗΜΝΙΑ_ΕΩΣ)
                ModelState.AddModelError("", "Η αρχική ημερομηνία δεν πρέπει να είναι μεγαλύτερη της τελικής. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ entity = new ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ()
                {
                    ΥΠΑΛΛΗΛΟΣ_ΚΩΔ = personId,
                    ΣΧΟΛΙΚΟ_ΕΤΟΣ = schoolyearId,
                    ΒΝΣ = stationId,
                    ΗΜΝΙΑ_ΑΠΟ = data.ΗΜΝΙΑ_ΑΠΟ,
                    ΗΜΝΙΑ_ΕΩΣ = data.ΗΜΝΙΑ_ΕΩΣ,
                    ΜΕΤΑΒΟΛΗ_ΕΙΔΟΣ = data.ΜΕΤΑΒΟΛΗ_ΕΙΔΟΣ,
                    ΜΕΤΑΒΟΛΗ_ΜΗΝΑΣ = data.ΗΜΝΙΑ_ΑΠΟ.Value.Month,
                    ΗΜΕΡΕΣ = c.WeekDays((DateTime)data.ΗΜΝΙΑ_ΑΠΟ, (DateTime)data.ΗΜΝΙΑ_ΕΩΣ)
                };
                db.ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ.Add(entity);
                db.SaveChanges();
                data.ΜΕΤΑΒΟΛΗ_ΚΩΔ = entity.ΜΕΤΑΒΟΛΗ_ΚΩΔ;
                newdata = RefreshPersonnelMetabolesFromDB(data.ΜΕΤΑΒΟΛΗ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Metaboli_Update([DataSourceRequest] DataSourceRequest request, PersonnelMetaboliViewModel data, int schoolyearId = 0, int personId = 0)
        {
            var newdata = new PersonnelMetaboliViewModel();

            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (!(schoolyearId > 0) || !(personId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε εργαζόμενο και σχολικό έτος. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ entity = db.ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ.Find(data.ΜΕΤΑΒΟΛΗ_ΚΩΔ);

                entity.ΥΠΑΛΛΗΛΟΣ_ΚΩΔ = personId;
                entity.ΣΧΟΛΙΚΟ_ΕΤΟΣ = schoolyearId;
                entity.ΒΝΣ = stationId;
                entity.ΗΜΝΙΑ_ΑΠΟ = data.ΗΜΝΙΑ_ΑΠΟ;
                entity.ΗΜΝΙΑ_ΕΩΣ = data.ΗΜΝΙΑ_ΕΩΣ;
                entity.ΜΕΤΑΒΟΛΗ_ΕΙΔΟΣ = data.ΜΕΤΑΒΟΛΗ_ΕΙΔΟΣ;
                entity.ΜΕΤΑΒΟΛΗ_ΜΗΝΑΣ = data.ΗΜΝΙΑ_ΑΠΟ.Value.Month;
                entity.ΗΜΕΡΕΣ = c.WeekDays((DateTime)data.ΗΜΝΙΑ_ΑΠΟ, (DateTime)data.ΗΜΝΙΑ_ΕΩΣ);

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newdata = RefreshPersonnelMetabolesFromDB(entity.ΜΕΤΑΒΟΛΗ_ΚΩΔ);
            }

            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Metaboli_Destroy([DataSourceRequest] DataSourceRequest request, PersonnelMetaboliViewModel data)
        {
            if (data != null)
            {
                ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ entity = db.ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ.Find(data.ΜΕΤΑΒΟΛΗ_ΚΩΔ);
                if (ModelState.IsValid)
                {
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ.Remove(entity);
                        db.SaveChanges();
                    }
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        public PersonnelMetaboliViewModel RefreshPersonnelMetabolesFromDB(int recordId)
        {
            var data = (from d in db.ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ
                        where d.ΜΕΤΑΒΟΛΗ_ΚΩΔ == recordId
                        orderby d.ΗΜΝΙΑ_ΑΠΟ descending
                        select new PersonnelMetaboliViewModel
                        {
                            ΜΕΤΑΒΟΛΗ_ΚΩΔ = d.ΜΕΤΑΒΟΛΗ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΗΜΕΡΕΣ = d.ΗΜΕΡΕΣ,
                            ΗΜΝΙΑ_ΑΠΟ = d.ΗΜΝΙΑ_ΑΠΟ,
                            ΗΜΝΙΑ_ΕΩΣ = d.ΗΜΝΙΑ_ΕΩΣ,
                            ΜΕΤΑΒΟΛΗ_ΕΙΔΟΣ = d.ΜΕΤΑΒΟΛΗ_ΕΙΔΟΣ,
                            ΜΕΤΑΒΟΛΗ_ΜΗΝΑΣ = d.ΜΕΤΑΒΟΛΗ_ΜΗΝΑΣ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΥΠΑΛΛΗΛΟΣ_ΚΩΔ = d.ΥΠΑΛΛΗΛΟΣ_ΚΩΔ
                        }).FirstOrDefault();

            return (data);
        }

        public List<PersonnelMetaboliViewModel> GetPersonnelMetabolesFromDB(int schoolyearId = 0, int personId = 0)
        {
            var data = (from d in db.ΠΡΟΣΩΠΙΚΟ_ΜΕΤΑΒΟΛΕΣ
                        where d.ΣΧΟΛΙΚΟ_ΕΤΟΣ == schoolyearId && d.ΥΠΑΛΛΗΛΟΣ_ΚΩΔ == personId
                        orderby d.ΗΜΝΙΑ_ΑΠΟ descending
                        select new PersonnelMetaboliViewModel
                        {
                            ΜΕΤΑΒΟΛΗ_ΚΩΔ = d.ΜΕΤΑΒΟΛΗ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΗΜΕΡΕΣ = d.ΗΜΕΡΕΣ,
                            ΗΜΝΙΑ_ΑΠΟ = d.ΗΜΝΙΑ_ΑΠΟ,
                            ΗΜΝΙΑ_ΕΩΣ = d.ΗΜΝΙΑ_ΕΩΣ,
                            ΜΕΤΑΒΟΛΗ_ΕΙΔΟΣ = d.ΜΕΤΑΒΟΛΗ_ΕΙΔΟΣ,
                            ΜΕΤΑΒΟΛΗ_ΜΗΝΑΣ = d.ΜΕΤΑΒΟΛΗ_ΜΗΝΑΣ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΥΠΑΛΛΗΛΟΣ_ΚΩΔ = d.ΥΠΑΛΛΗΛΟΣ_ΚΩΔ
                        }).ToList();

            return (data);
        }

        public int GetStationFromPersonId(int personId)
        {
            int stationId = 0;

            var data = (from d in db.ΠΡΟΣΩΠΙΚΟ where d.PERSONNEL_ID == personId select d).FirstOrDefault();
            if (data != null) stationId = (int)data.ΒΝΣ;

            return (stationId);
        }

        public ActionResult MetabolesPrint()
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

                var data = (from d in db.ΣΥΣ_ΣΤΑΘΜΟΙ
                            where d.ΣΤΑΘΜΟΣ_ΚΩΔ == stationId
                            select new StationsViewModel
                            {
                                ΣΤΑΘΜΟΣ_ΚΩΔ = d.ΣΤΑΘΜΟΣ_ΚΩΔ,
                                ΠΕΡΙΦΕΡΕΙΑΚΗ = d.ΠΕΡΙΦΕΡΕΙΑΚΗ
                            }).FirstOrDefault();

                return View(data);
            }
        }

        #endregion

        #endregion --- ΠΡΟΣΩΠΙΚΟ ΣΤΑΘΜΟΥ ---

        #region --- ΜΕΤΑΒΟΛΕΣ ΜΗΝΙΑΙΑ ΕΚΘΕΣΗ ---

        public ActionResult MetabolesMonthRep(string notify = null)
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

            if (!PersonnelExists() || !MonthsExist() || !SchoolYearsExist())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει πρώτα να καταχωρηθούν εργαζόμενοι, μήνες και σχολικά έτη στις Ρυθμίσεις.";
                return RedirectToAction("Index", "Station", new { notify = msg });
            }

            populateEmployees();
            populateSchoolYears();
            populateMonths();
            return View();
        }

        #region GRID CRUD FUNCTIONS

        public ActionResult MetaboliRep_Read([DataSourceRequest] DataSourceRequest request, int schoolyearId = 0, int monthId = 0)
        {
            List<MetabolesReportViewModel> data = GetMetabolesReportFromDB(schoolyearId, monthId);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MetaboliRep_Create([DataSourceRequest] DataSourceRequest request, MetabolesReportViewModel data, int schoolyearId = 0, int monthId = 0)
        {
            var newdata = new MetabolesReportViewModel();
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (!(schoolyearId > 0) || !(monthId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε σχολικό έτος και μήνα. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                METABOLES_REPORT entity = new METABOLES_REPORT()
                {
                    BNS = stationId,
                    SCHOOL_YEAR = schoolyearId,
                    METABOLI_MONTH = monthId,
                    EMPLOYEE_ID = data.EMPLOYEE_ID,
                    METABOLI_YEAR = data.METABOLI_YEAR,
                    METABOLI_TEXT = data.METABOLI_TEXT,
                    METABOLI_DAYS = data.METABOLI_DAYS
                };
                db.METABOLES_REPORT.Add(entity);
                db.SaveChanges();
                data.RECORD_ID = entity.RECORD_ID;
                newdata = RefreshMetabolesReportFromDB(data.RECORD_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MetaboliRep_Update([DataSourceRequest] DataSourceRequest request, MetabolesReportViewModel data, int schoolyearId = 0, int monthId = 0)
        {
            var newdata = new MetabolesReportViewModel();
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (!(schoolyearId > 0) || !(monthId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε σχολικό έτος και μήνα. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                METABOLES_REPORT entity = db.METABOLES_REPORT.Find(data.RECORD_ID);

                entity.BNS = stationId;
                entity.SCHOOL_YEAR = schoolyearId;
                entity.METABOLI_MONTH = monthId;
                entity.EMPLOYEE_ID = data.EMPLOYEE_ID;
                entity.METABOLI_YEAR = data.METABOLI_YEAR;
                entity.METABOLI_TEXT = data.METABOLI_TEXT;
                entity.METABOLI_DAYS = data.METABOLI_DAYS;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newdata = RefreshMetabolesReportFromDB(entity.RECORD_ID);
            }

            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MetaboliRep_Destroy([DataSourceRequest] DataSourceRequest request, MetabolesReportViewModel data)
        {
            if (data != null)
            {
                METABOLES_REPORT entity = db.METABOLES_REPORT.Find(data.RECORD_ID);
                if (ModelState.IsValid)
                {
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.METABOLES_REPORT.Remove(entity);
                        db.SaveChanges();
                    }
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        public MetabolesReportViewModel RefreshMetabolesReportFromDB(int recordId)
        {
            var data = (from d in db.METABOLES_REPORT
                        where d.RECORD_ID == recordId
                        select new MetabolesReportViewModel
                        {
                            RECORD_ID = d.RECORD_ID,
                            BNS = d.BNS,
                            EMPLOYEE_ID = d.EMPLOYEE_ID,
                            SCHOOL_YEAR = d.SCHOOL_YEAR,
                            METABOLI_DAYS = d.METABOLI_DAYS,
                            METABOLI_MONTH = d.METABOLI_MONTH,
                            METABOLI_YEAR = d.METABOLI_YEAR,
                            METABOLI_TEXT = d.METABOLI_TEXT
                        }).FirstOrDefault();

            return (data);
        }

        public List<MetabolesReportViewModel> GetMetabolesReportFromDB(int schoolyearId = 0, int monthId = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.METABOLES_REPORT
                        orderby d.ΠΡΟΣΩΠΙΚΟ.ΕΠΩΝΥΜΟ, d.ΠΡΟΣΩΠΙΚΟ.ΟΝΟΜΑ
                        where d.BNS == stationId && d.SCHOOL_YEAR == schoolyearId && d.METABOLI_MONTH == monthId
                        select new MetabolesReportViewModel
                        {
                            RECORD_ID = d.RECORD_ID,
                            BNS = d.BNS,
                            EMPLOYEE_ID = d.EMPLOYEE_ID,
                            SCHOOL_YEAR = d.SCHOOL_YEAR,
                            METABOLI_DAYS = d.METABOLI_DAYS,
                            METABOLI_MONTH = d.METABOLI_MONTH,
                            METABOLI_YEAR = d.METABOLI_YEAR,
                            METABOLI_TEXT = d.METABOLI_TEXT
                        }).ToList();

            return (data);
        }


        #region ΛΕΙΤΟΥΡΓΙΕΣ ΔΗΜΙΟΥΡΓΙΑΣ-ΕΝΗΜΕΡΩΣΗΣ-ΕΚΤΥΠΩΣΗΣ ΠΙΝΑΚΑ ΜΗΝΙΑΙΩΝ ΜΕΤΑΒΟΛΩΝ

        public ActionResult CreateMetabolesTable(int schoolyearId, int monthId)
        {
            string msg = "Η δημιουργία του πίνακα μηνιαίων μεταβολών του προσωπικού ολοκληρώθηκε.";

            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var source1 = (from d in db.srcPERSONNEL_DATA where d.ΒΝΣ == stationId orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ select d).ToList();
            if (source1.Count() == 0)
            {
                msg = "Δεν βρέθηκε καταχωρημένο προσωπικό για το σταθμό αυτό. Η δημιουργία του πίνακα είναι αδύνατη.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            var source2 = (from d in db.srcMETABOLES_REPORT where d.ΒΝΣ == stationId && d.ΣΧΟΛΙΚΟ_ΕΤΟΣ == schoolyearId && d.ΜΕΤΑΒΟΛΗ_ΜΗΝΑΣ == monthId select d).ToList();
            if (source2.Count() == 0)
            {
                msg = "Δεν βρέθηκαν καταχωρημένες μεταβολές για το σχολικό έτος και μήνα αυτό. Η δημιουργία του πίνακα είναι αδύνατη.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            List<METABOLES_REPORT> target = (from d in db.METABOLES_REPORT where d.BNS == stationId && d.SCHOOL_YEAR == schoolyearId && d.METABOLI_MONTH == monthId select d).ToList();
            if (target.Count > 0)
            {
                msg = "Ο πίνακας μεταβολών έχει ήδη εγγραφές για το σχολικό έτος και μήνα αυτό. Για ενημέρωση πατήστε 'Ενημέρωση πίνακα'";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                foreach (var person in source1)
                {
                    METABOLES_REPORT entity = new METABOLES_REPORT()
                    {
                        SCHOOL_YEAR = schoolyearId,
                        METABOLI_MONTH = monthId,
                        BNS = person.ΒΝΣ,
                        EMPLOYEE_ID = person.PERSONNEL_ID,
                        METABOLI_DAYS = c.GenerateAbsenceDays(person.PERSONNEL_ID, schoolyearId, monthId),
                        METABOLI_YEAR = c.GenerateMetabolesYear(person.PERSONNEL_ID, schoolyearId, monthId),
                        METABOLI_TEXT = c.GenerateMetabolesText(person.PERSONNEL_ID, schoolyearId, monthId)
                    };
                    db.METABOLES_REPORT.Add(entity);
                    db.SaveChanges();
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateMetabolesTable(int schoolyearId, int monthId)
        {
            string msg = "Η ενημέρωση του πίνακα μηνιαίων μεταβολών του προσωπικού ολοκληρώθηκε.";

            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var source1 = (from d in db.srcPERSONNEL_DATA where d.ΒΝΣ == stationId orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ select d).ToList();
            if (source1.Count() == 0)
            {
                msg = "Δεν βρέθηκε καταχωρημένο προσωπικό για το σταθμό αυτό. Η ενημέρωση του πίνακα είναι αδύνατη.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            var source2 = (from d in db.srcMETABOLES_REPORT where d.ΒΝΣ == stationId && d.ΣΧΟΛΙΚΟ_ΕΤΟΣ == schoolyearId && d.ΜΕΤΑΒΟΛΗ_ΜΗΝΑΣ == monthId select d).ToList();
            if (source2.Count() == 0)
            {
                msg = "Δεν βρέθηκαν καταχωρημένες μεταβολές για το σχολικό έτος και μήνα αυτό. Η ενημέρωση του πίνακα είναι αδύνατη.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            List<METABOLES_REPORT> target = (from d in db.METABOLES_REPORT where d.BNS == stationId && d.SCHOOL_YEAR == schoolyearId && d.METABOLI_MONTH == monthId select d).ToList();
            if (target.Count == 0)
            {
                msg = "Ο πίνακας μεταβολών δεν έχει εγγραφές για το σχολικό έτος και μήνα αυτό. Για δημιουργία πατήστε 'Δημιουργία πίνακα'";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // First, delete all records
                foreach (var item in target)
                {
                    METABOLES_REPORT entity = db.METABOLES_REPORT.Find(item.RECORD_ID);
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.METABOLES_REPORT.Remove(entity);
                        db.SaveChanges();
                    }
                }
                // Next, re-create records
                foreach (var person in source1)
                {
                    METABOLES_REPORT entity = new METABOLES_REPORT()
                    {
                        SCHOOL_YEAR = schoolyearId,
                        METABOLI_MONTH = monthId,
                        BNS = person.ΒΝΣ,
                        EMPLOYEE_ID = person.PERSONNEL_ID,
                        METABOLI_DAYS = c.GenerateAbsenceDays(person.PERSONNEL_ID, schoolyearId, monthId),
                        METABOLI_YEAR = c.GenerateMetabolesYear(person.PERSONNEL_ID, schoolyearId, monthId),
                        METABOLI_TEXT = c.GenerateMetabolesText(person.PERSONNEL_ID, schoolyearId, monthId)
                    };
                    db.METABOLES_REPORT.Add(entity);
                    db.SaveChanges();
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DestroyMetabolesTable(int schoolyearId, int monthId)
        {
            string msg = "Η διαγραφή του πίνακα μεταβολών για το σχολικό έτος και μήνα ολοκληρώθηκε.";

            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.METABOLES_REPORT where d.BNS == stationId && d.SCHOOL_YEAR == schoolyearId && d.METABOLI_MONTH == monthId select d).ToList();
            if (data.Count == 0)
            {
                msg = "Δεν βρέθηκαν καταχωρημένες μεταβολές για το σχολικό έτος και μήνα αυτό. Η διαγραφή του πίνακα ακυρώθηκε.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            foreach (var item in data)
            {
                METABOLES_REPORT entity = db.METABOLES_REPORT.Find(item.RECORD_ID);
                if (entity != null)
                {
                    db.Entry(entity).State = EntityState.Deleted;
                    db.METABOLES_REPORT.Remove(entity);
                    db.SaveChanges();
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MetabolesMonthPrint(int schoolyearId, int monthId)
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

            var data = (from d in db.METABOLES_REPORT
                        where d.BNS == stationId && d.SCHOOL_YEAR == schoolyearId && d.METABOLI_MONTH == monthId
                        select new MetabolesReportViewModel
                        {
                            BNS = d.BNS,
                            SCHOOL_YEAR = d.SCHOOL_YEAR,
                            METABOLI_MONTH = d.METABOLI_MONTH
                        }).FirstOrDefault();

            return View(data);
        }

        #endregion

        #endregion --- ΜΕΤΑΒΟΛΕΣ ΜΗΝΙΑΙΑ ΕΚΘΕΣΗ ---


        #region ΩΡΟΛΟΓΙΟ ΠΡΟΓΡΑΜΜΑ (24-7-2019)

        public ActionResult ProgrammaDay(string notify = null)
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

            if (!HoursExist() || !PersonnelExists())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει πρώτα να καταχωρηθούν το προσωπικό και οι ώρες προσέλευσης,αποχώρησης.";
                return RedirectToAction("Index", "Station", new { notify = msg });
            }

            populatePersonnel();
            populateHours();
            return View();
        }

        public List<ProgrammaDayViewModel> GetProgrammaDayFromDB(DateTime theDate)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ
                        where d.STATION_ID == stationId && d.PROGRAMMA_DATE == theDate
                        orderby d.ΠΡΟΣΩΠΙΚΟ.ΕΠΩΝΥΜΟ, d.ΠΡΟΣΩΠΙΚΟ.ΟΝΟΜΑ
                        select new ProgrammaDayViewModel
                        {
                            STATION_ID = d.STATION_ID,
                            PROGRAMMA_ID = d.PROGRAMMA_ID,
                            PROGRAMMA_DATE = d.PROGRAMMA_DATE,
                            PROGRAMMA_MONTH = d.PROGRAMMA_MONTH,
                            SCHOOLYEARID = d.SCHOOLYEARID,
                            PERSON_ID = d.PERSON_ID,
                            HOUR_START = d.HOUR_START,
                            HOUR_END = d.HOUR_END
                        }).ToList();

            return (data);
        }

        #region PROGRAMMA GRID FUNCTIONS

        public ActionResult Programma_Read([DataSourceRequest] DataSourceRequest request, DateTime? theDate = null, int syearId = 0)
        {
            DateTime paramDate = theDate ?? DateTime.Today;

            var data = GetProgrammaDayFromDB(paramDate);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public ActionResult Programma_Create([DataSourceRequest] DataSourceRequest request,
                           [Bind(Prefix = "models")]IEnumerable<ProgrammaDayViewModel> data, DateTime? theDate = null, int syearId = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (theDate == null || syearId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σχολκό έτος και ημερομηνία.");
                return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }
            if (!c.DateInSelectedSchoolYear((DateTime)theDate, syearId))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.");

            int CurrentSchoolYear = syearId;

            if (data != null && ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ entity = new ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ()
                    {
                        PROGRAMMA_DATE = theDate,
                        STATION_ID = stationId,
                        PERSON_ID = item.PERSON_ID,
                        HOUR_START = item.HOUR_START,
                        HOUR_END = item.HOUR_END,
                        PROGRAMMA_MONTH = theDate.Value.Month,
                        SCHOOLYEARID = CurrentSchoolYear
                    };
                    if (ProgrammaItemExists(entity))
                    {
                        ModelState.AddModelError("", "Έγινε απόπειρα δημιουργίας διπλοεγγραφής. Η διπλή καταχώρηση ακυρώθηκε.");
                        return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                    }
                    string err_msg = ValidateProgramma(item);
                    if (err_msg != "")
                    {
                        ModelState.AddModelError("", err_msg);
                        return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                    }
                    db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ.Add(entity);
                    db.SaveChanges();
                }
            }
            return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Programma_Update([DataSourceRequest] DataSourceRequest request,
                            [Bind(Prefix = "models")]IEnumerable<ProgrammaDayViewModel> data, DateTime? theDate = null, int syearId = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (theDate == null || syearId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σχολικό έτος και ημερομηνία.");
                return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }
            if (!c.DateInSelectedSchoolYear((DateTime)theDate, syearId))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.");

            int CurrentSchoolYear = syearId;

            if (data != null && ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ entity = db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ.Find(item.PROGRAMMA_ID);

                    entity.PROGRAMMA_DATE = theDate;
                    entity.STATION_ID = stationId;
                    entity.PERSON_ID = item.PERSON_ID;
                    entity.HOUR_START = item.HOUR_START;
                    entity.HOUR_END = item.HOUR_END;
                    entity.PROGRAMMA_MONTH = theDate.Value.Month;
                    entity.SCHOOLYEARID = CurrentSchoolYear;
                    string err_msg = ValidateProgramma(item);
                    if (err_msg != "")
                    {
                        ModelState.AddModelError("", err_msg);
                        return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                    }
                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Programma_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ProgrammaDayViewModel> data)
        {
            if (data.Any())
            {
                foreach (var item in data)
                {
                    ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ entity = db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ.Find(item.PROGRAMMA_ID);
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ.Remove(entity);
                        db.SaveChanges();
                    }
                }
            }
            return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ProgrammaDayViewModel RefreshProgrammaViewModel(int recordId)
        {
            var data = (from d in db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ
                        where d.PROGRAMMA_ID == recordId
                        select new ProgrammaDayViewModel
                        {
                            STATION_ID = d.STATION_ID,
                            PROGRAMMA_ID = d.PROGRAMMA_ID,
                            PROGRAMMA_DATE = d.PROGRAMMA_DATE,
                            PROGRAMMA_MONTH = d.PROGRAMMA_MONTH,
                            SCHOOLYEARID = d.SCHOOLYEARID,
                            PERSON_ID = d.PERSON_ID,
                            HOUR_START = d.HOUR_START,
                            HOUR_END = d.HOUR_END
                        }).FirstOrDefault();

            return (data);
        }

        #endregion

        #region PROGRAMMA VALIDATIONS

        public string ValidateProgramma(ProgrammaDayViewModel item)
        {
            string errMsg = "";

            var data1 = (from d in db.ΣΥΣ_ΩΡΕΣ where d.HOUR_ID == item.HOUR_START select d).FirstOrDefault();
            var data2 = (from d in db.ΣΥΣ_ΩΡΕΣ where d.HOUR_ID == item.HOUR_END select d).FirstOrDefault();

            DateTime TimeStart = DateTime.ParseExact(data1.HOUR_TEXT, "HH:mm", CultureInfo.InvariantCulture);
            DateTime TimeEnd = DateTime.ParseExact(data2.HOUR_TEXT, "HH:mm", CultureInfo.InvariantCulture);

            if (TimeStart >= TimeEnd)
                errMsg = "Η ώρα προσέλευσης δεν μπορεί να είναι μεγαλύτερη ή ίση της ώρας αποχώρησης.";

            return errMsg;
        }

        public bool ProgrammaItemExists(ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ item)
        {
            var data = (from d in db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ
                        where d.STATION_ID == item.STATION_ID && d.PROGRAMMA_DATE == item.PROGRAMMA_DATE && d.PERSON_ID == item.PERSON_ID
                        select d).ToList();
            if (data.Count > 0)
            {
                return true;
            }
            else return false;
        }

        public bool CanCreatePersonProgramma(int personId, DateTime theDate)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ where d.STATION_ID == stationId && d.PERSON_ID == personId && d.PROGRAMMA_DATE == theDate select d).ToList();
            if (data.Count > 0) return false;
            else return true;

        }

        #endregion

        #region PROGRAMMA AUTOMATIONS

        public ActionResult LoadStationPersonnel(ProgrammaParameters p)
        {
            string msg = "";

            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;
            p.stationId = stationId;

            if (p.theDate == null || p.syearId == 0)
            {
                msg = "Δεν έχει γίνει επιλογή σχολικού έτους και ημερομηνίας.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            int CurrentSchoolYear = p.syearId;

            // Exclude employees with withdrawal
            var srcData = (from d in db.ΠΡΟΣΩΠΙΚΟ where d.ΒΝΣ == p.stationId && (d.ΑΠΟΧΩΡΗΣΕ != true || d.ΑΠΟΧΩΡΗΣΗ_ΗΜΝΙΑ >= p.theDate)
                           orderby d.ΕΠΩΝΥΜΟ, d.ΟΝΟΜΑ select d).ToList();
            if (srcData.Count > 0)
            {
                foreach (var item in srcData)
                {
                    // first check if record already exists
                    if (CanCreatePersonProgramma(item.PERSONNEL_ID, p.theDate))
                    {
                        ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ target = new ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ()
                        {
                            STATION_ID = p.stationId,
                            PROGRAMMA_DATE = p.theDate,
                            SCHOOLYEARID = CurrentSchoolYear,
                            PERSON_ID = item.PERSONNEL_ID,
                            PROGRAMMA_MONTH = p.theDate.Month,
                            HOUR_START = c.GetFirstHour(),
                            HOUR_END = c.GetLastHour()
                        };
                        db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ.Add(target);
                        db.SaveChanges();
                    }
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TransferDay(ProgrammaParameters p)
        {
            string msg = "";

            if (p.theDate == null || p.syearId == 0)
            {
                msg = "πρέπει πρώτα να επιλέξετε σχολικό έτος και ημερομηνία. Η διαδικασία ακυρώθηκε.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            if (!c.DateInSelectedSchoolYear(p.theDate, p.syearId))
            {
                msg = "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            DateTime srcDate = p.theDate;
            DateTime targetDate = p.theDate.AddDays(1);

            if (targetDate.DayOfWeek == DayOfWeek.Saturday) targetDate = targetDate.AddDays(2);
            else if (targetDate.DayOfWeek == DayOfWeek.Sunday) targetDate = targetDate.AddDays(1);

            if (!c.DateInSelectedSchoolYear(targetDate, p.syearId))
            {
                msg = "Οι ημερομηνία προορισμού είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            var targetData = GetProgrammaDayFromDB(targetDate);
            if (targetData.Count > 0)
            {
                msg = "Βρέθηκε ήδη καταχωρημένο ωρολόγιο πρόγραμμα στην " + targetDate.ToString("dd/MM/yyyy") + ". Η αντιγραφή ακυρώθηκε.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            var sourceData = (from d in db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ
                              where d.STATION_ID == stationId && d.PROGRAMMA_DATE == srcDate
                              orderby d.ΠΡΟΣΩΠΙΚΟ.ΕΠΩΝΥΜΟ, d.ΠΡΟΣΩΠΙΚΟ.ΟΝΟΜΑ ascending
                              select d).ToList();

            if (sourceData.Count() == 0)
            {
                msg = "Δεν βρέθηκε καταχωρημένο ωρολόγιο πρόγραμμα στην ημερομηνία προέλευσης. Δεν υπάρχουν δεδομένα για μεταφορά.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            };

            int CurrentSchoolYear = p.syearId;

            foreach (var d in sourceData)
            {
                ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ target = new ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ()
                {
                    STATION_ID = stationId,
                    PROGRAMMA_DATE = targetDate,
                    PROGRAMMA_MONTH = targetDate.Month,
                    SCHOOLYEARID = CurrentSchoolYear,
                    PERSON_ID = d.PERSON_ID,
                    HOUR_START = d.HOUR_START,
                    HOUR_END = d.HOUR_END
                };
                db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ.Add(target);
                db.SaveChanges();
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TransferWeek(ProgrammaParameters p)
        {
            string msg = "";
            int DaysToAdd = 0;

            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;
            p.stationId = stationId;

            if (p.theDate == null || p.syearId == 0)
            {
                msg = "πρέπει πρώτα να επιλέξετε σχολικό έτος και ημερομηνία. Η διαδικασία ακυρώθηκε.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            var max_date = (from d in db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ where d.STATION_ID == p.stationId select d).Max(d => d.PROGRAMMA_DATE);
            DateTime finalDate = (DateTime)max_date;

            if (finalDate.DayOfWeek == DayOfWeek.Friday) DaysToAdd = -4;
            else if (finalDate.DayOfWeek == DayOfWeek.Thursday) DaysToAdd = -3;
            else if (finalDate.DayOfWeek == DayOfWeek.Wednesday) DaysToAdd = -2;
            else if (finalDate.DayOfWeek == DayOfWeek.Tuesday) DaysToAdd = -1;
            else if (finalDate.DayOfWeek == DayOfWeek.Monday) DaysToAdd = 0;

            DateTime initialDate = finalDate.AddDays(DaysToAdd);
            DateTime targetStartDate = initialDate.AddDays(7);
            DateTime targetFinalDate = targetStartDate.AddDays(4);

            if (!c.DateInSelectedSchoolYear(targetStartDate, p.syearId) || !c.DateInSelectedSchoolYear(targetFinalDate, p.syearId))
            {
                msg = "Οι ημερομηνίες (αρχική ή τελική) προορισμού είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            var srcData = (from d in db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ
                           where d.PROGRAMMA_DATE >= initialDate && d.PROGRAMMA_DATE <= finalDate && d.STATION_ID == p.stationId
                           orderby d.PROGRAMMA_DATE, d.ΠΡΟΣΩΠΙΚΟ.ΕΠΩΝΥΜΟ, d.ΠΡΟΣΩΠΙΚΟ.ΟΝΟΜΑ
                           select d).ToList();

            if (srcData.Count == 0)
            {
                msg = "Δεν βρέθηκε ωρολόγιο πρόγραμμα στην εβδομάδα προέλευσης. Δεν υπάρχουν δεδομένα για αντιγραφή.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            int CurrentSchoolYear = p.syearId;

            foreach (var item in srcData)
            {
                DateTime _targetDate = item.PROGRAMMA_DATE.Value.AddDays(7);
                if (CanCreatePersonProgramma((int)item.PERSON_ID, _targetDate))
                {
                    ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ target = new ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ()
                    {
                        STATION_ID = p.stationId,
                        PROGRAMMA_DATE = _targetDate,
                        PERSON_ID = item.PERSON_ID,
                        PROGRAMMA_MONTH = _targetDate.Month,
                        SCHOOLYEARID = CurrentSchoolYear,
                        HOUR_START = item.HOUR_START,
                        HOUR_END = item.HOUR_END
                    };
                    db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ.Add(target);
                    db.SaveChanges();
                }
            };
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion


        #region ΕΛΕΓΧΟΣ ΩΡΟΛΟΓΙΟΥ ΠΡΟΓΡΑΜΜΑΤΟΣ (25-7-2019)

        public ActionResult ProgrammaDayCheck(string notify = null)
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

            if (!HoursExist() || !PersonnelExists())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει πρώτα να καταχωρηθούν το προσωπικό και οι ώρες προσέλευσης,αποχώρησης.";
                return RedirectToAction("Index", "Station", new { notify = msg });
            }

            populatePersonnel();
            populateHours();
            return View();
        }

        #region GRID CRUD FUNCTIONS

        public ActionResult Programma2_Read([DataSourceRequest] DataSourceRequest request, DateTime? theDate1 = null, DateTime? theDate2 = null, int syearId = 0)
        {

            var data = GetProgrammaDay2FromDB(theDate1, theDate2);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public ActionResult Programma2_Create([DataSourceRequest] DataSourceRequest request, 
                            [Bind(Prefix = "models")]IEnumerable<ProgrammaDayViewModel> data, DateTime? theDate1 = null, DateTime? theDate2 = null, int syearId = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (syearId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σχολκό έτος.");
            }
            int CurrentSchoolYear = syearId;

            ProgrammaDayViewModel newdata = new ProgrammaDayViewModel();

            if (data != null && ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ entity = new ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ()
                    {
                        PROGRAMMA_DATE = item.PROGRAMMA_DATE,
                        STATION_ID = stationId,
                        PERSON_ID = item.PERSON_ID,
                        HOUR_START = item.HOUR_START,
                        HOUR_END = item.HOUR_END,
                        PROGRAMMA_MONTH = item.PROGRAMMA_DATE.Value.Month,
                        SCHOOLYEARID = CurrentSchoolYear
                    };
                    if (ProgrammaItemExists(entity))
                    {
                        ModelState.AddModelError("", "Έγινε απόπειρα δημιουργίας διπλοεγγραφής. Η διπλή καταχώρηση ακυρώθηκε.");
                        return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                    }
                    string err_msg = ValidateProgramma(item);
                    if (err_msg != "")
                    {
                        ModelState.AddModelError("", err_msg);
                        return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                    }
                    db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ.Add(entity);
                    db.SaveChanges();
                }
            }
            return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Programma2_Update([DataSourceRequest] DataSourceRequest request,
                            [Bind(Prefix = "models")]IEnumerable<ProgrammaDayViewModel> data, DateTime? theDate1 = null, DateTime? theDate2 = null, int syearId = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (syearId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σχολκό έτος.");
            }
            int CurrentSchoolYear = syearId;

            ProgrammaDayViewModel newdata = new ProgrammaDayViewModel();

            if (data != null && ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ entity = db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ.Find(item.PROGRAMMA_ID);

                    entity.PROGRAMMA_DATE = item.PROGRAMMA_DATE;
                    entity.PERSON_ID = item.PERSON_ID;
                    entity.HOUR_START = item.HOUR_START;
                    entity.HOUR_END = item.HOUR_END;
                    entity.STATION_ID = stationId;
                    entity.PROGRAMMA_MONTH = item.PROGRAMMA_DATE.Value.Month;
                    entity.SCHOOLYEARID = CurrentSchoolYear;

                    string err_msg = ValidateProgramma(item);
                    if (err_msg != "")
                    {
                        ModelState.AddModelError("", err_msg);
                        return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                    }
                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Programma2_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ProgrammaDayViewModel> data)
        {
            if (data.Any())
            {
                foreach (var item in data)
                {
                    ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ entity = db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ.Find(item.PROGRAMMA_ID);
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ.Remove(entity);
                        db.SaveChanges();
                    }
                }
            }
            return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public List<ProgrammaDayViewModel> GetProgrammaDay2FromDB(DateTime? theDate1 = null, DateTime? theDate2 = null, int syearId = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            List<ProgrammaDayViewModel> data = new List<ProgrammaDayViewModel>();

            if (theDate1 != null && theDate2 != null)
            {
                data = (from d in db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ
                        where d.STATION_ID == stationId && d.PROGRAMMA_DATE >= theDate1 && d.PROGRAMMA_DATE <= theDate2
                        orderby d.PROGRAMMA_DATE, d.ΠΡΟΣΩΠΙΚΟ.ΕΠΩΝΥΜΟ, d.ΠΡΟΣΩΠΙΚΟ.ΟΝΟΜΑ
                        select new ProgrammaDayViewModel
                        {
                            STATION_ID = d.STATION_ID,
                            PROGRAMMA_ID = d.PROGRAMMA_ID,
                            PROGRAMMA_DATE = d.PROGRAMMA_DATE,
                            PROGRAMMA_MONTH = d.PROGRAMMA_MONTH,
                            SCHOOLYEARID = d.SCHOOLYEARID,
                            PERSON_ID = d.PERSON_ID,
                            HOUR_START = d.HOUR_START,
                            HOUR_END = d.HOUR_END
                        }).ToList();
            }
            return (data);
        }

        #endregion GRID CRUD FUNCTIONS

        public ActionResult DeleteProgramma(string theDate1 = null, string theDate2 = null)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            string msg = "";

            DateTime dtDate1, dtDate2;

            bool result1 = DateTime.TryParse(theDate1, out dtDate1);
            bool result2 = DateTime.TryParse(theDate2, out dtDate2);

            if (stationId > 0 && result1 && result2)
            {
                var data = (from d in db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ where d.STATION_ID == stationId && d.PROGRAMMA_DATE >= dtDate1 && d.PROGRAMMA_DATE <= dtDate2 select d).ToList();
                if (data.Count > 0)
                {
                    foreach (var rec in data)
                    {
                        ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ entity = db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ.Find(rec.PROGRAMMA_ID);
                        if (entity != null)
                        {
                            db.Entry(entity).State = EntityState.Deleted;
                            db.ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ.Remove(entity);
                            db.SaveChanges();
                        }
                    }
                    msg = "Η διαγραφή του επιλεγμένου προγράμματος ολοκληρώθηκε με επιτυχία.";
                }
                else
                {
                    msg = "Δεν βρέθηκε καταχωρημένο πρόγραμμα για τον επιλεγμένο σταθμό στις ημερομηνίες αυτές.";
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region ΜΗΝΙΑΙΑ ΚΑΤΑΣΤΑΣΗ ΩΡΩΝ ΠΡΟΣΩΠΙΚΟΥ

        public ActionResult PersonMonthHoursPrint()
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

            GeneralReportParameters p = new GeneralReportParameters();
            if (stationId > 0)
            {
                p.STATION_ID = stationId;
            }
            else
            {
                p.STATION_ID = 0;
            }

            return View(p);
        }


        #endregion ΜΗΝΙΑΙΑ ΚΑΤΑΣΤΑΣΗ ΩΡΩΝ ΠΡΟΣΩΠΙΚΟΥ
        
        #region ΠΑΡΟΥΣΙΕΣ ΠΑΙΔΙΩΝ

        public ActionResult ChildrenParousies(string notify = null)
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

            if (!TmimaChildrenExist() || !SchoolYearsExist())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει να υπάρχουν εγγραφές παιδιών σε τμήματα και καταχωρημένα σχολικά έτη.";
                return RedirectToAction("Index", "Station", new { notify = msg });
            }

            populateChildren();
            populateSchoolYears();
            return View();
        }

        public ActionResult FilteredChild_Read(int tmimaId = 0)
        {
            var data = (from d in db.sqlCHILD_TMIMA_SELECTOR
                        where d.ΤΜΗΜΑ == tmimaId
                        orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ
                        select new ChildTmimaSelectorViewModel
                        {
                            CHILD_ID = d.CHILD_ID,
                            ΟΝΟΜΑΤΕΠΩΝΥΜΟ = d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ,
                            ΤΜΗΜΑ = d.ΤΜΗΜΑ
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public List<ChildParousiaViewModel> GetChildParousiesFromDB(int tmimaId, DateTime theDate)
        {
            var data = (from d in db.ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ
                        where d.TMIMA_ID == tmimaId && d.PAROUSIA_DATE == theDate
                        orderby d.ΠΑΙΔΙΑ.ΕΠΩΝΥΜΟ, d.ΠΑΙΔΙΑ.ΟΝΟΜΑ
                        select new ChildParousiaViewModel
                        {
                            PAROUSIA_ID = d.PAROUSIA_ID,
                            CHILD_ID = d.CHILD_ID,
                            TMIMA_ID = d.TMIMA_ID,
                            PAROUSIA_DATE = d.PAROUSIA_DATE,
                            PAROUSIA_MONTH = d.PAROUSIA_MONTH,
                            PRESENCE = d.PRESENCE ?? true,
                            SCHOOLYEARID = d.SCHOOLYEARID,
                            STATION_ID = d.STATION_ID
                        }).ToList();

            return (data);
        }

        public ChildParousiaViewModel RefreshChildParousies(int recordId)
        {
            var data = (from d in db.ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ
                        where d.PAROUSIA_ID == recordId
                        select new ChildParousiaViewModel
                        {
                            PAROUSIA_ID = d.PAROUSIA_ID,
                            CHILD_ID = d.CHILD_ID,
                            TMIMA_ID = d.TMIMA_ID,
                            PAROUSIA_DATE = d.PAROUSIA_DATE,
                            PAROUSIA_MONTH = d.PAROUSIA_MONTH,
                            PRESENCE = d.PRESENCE ?? true,
                            SCHOOLYEARID = d.SCHOOLYEARID,
                            STATION_ID = d.STATION_ID
                        }).FirstOrDefault();

            return (data);
        }

        #region PAROUSIES GRID FUNCTIONS

        public ActionResult Parousies_Read([DataSourceRequest] DataSourceRequest request, int tmimaId = 0, DateTime? theDate = null)
        {
            DateTime paramDate = theDate ?? DateTime.Today;

            var data = GetChildParousiesFromDB(tmimaId, paramDate);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public ActionResult Parousies_Create([DataSourceRequest] DataSourceRequest request,
                    [Bind(Prefix = "models")]IEnumerable<ChildParousiaViewModel> data, int tmimaId = 0, DateTime? theDate = null)
        {
            int stationId = 0;
            int SchoolYearId = 0;

            if (tmimaId == 0 || theDate == null)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει τμήμα και ημερομηνία. Η διαδικασία ακυρώθηκε.");
            }
            var TmimaData = c.GetTmimaData(tmimaId);
            if (TmimaData != null)
            {
                stationId = (int)TmimaData.ΒΝΣ;
                SchoolYearId = (int)TmimaData.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
            }

            if (data != null && ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ entity = new ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ()
                    {
                        STATION_ID = stationId,
                        SCHOOLYEARID = SchoolYearId,
                        TMIMA_ID = tmimaId,
                        PAROUSIA_DATE = theDate,
                        PAROUSIA_MONTH = theDate.Value.Month,
                        CHILD_ID = item.CHILD_ID,
                        PRESENCE = item.PRESENCE
                    };
                    if (ParousiaItemExists(entity))
                    {
                        ModelState.AddModelError("", "Έγινε απόπειρα δημιουργίας διπλοεγγραφής. Η διπλή καταχώρηση ακυρώθηκε.");
                        return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                    }
                    db.ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ.Add(entity);
                    db.SaveChanges();
                }
            }
            return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Parousies_Update([DataSourceRequest] DataSourceRequest request,
                    [Bind(Prefix = "models")]IEnumerable<ChildParousiaViewModel> data, int tmimaId = 0, DateTime? theDate = null)
        {
            int stationId = 0;
            int SchoolYearId = 0;

            if (tmimaId == 0 || theDate == null)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει τμήμα και ημερομηνία. Η διαδικασία ακυρώθηκε.");
            }
            var TmimaData = c.GetTmimaData(tmimaId);
            if (TmimaData != null)
            {
                stationId = (int)TmimaData.ΒΝΣ;
                SchoolYearId = (int)TmimaData.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
            }

            if (data != null && ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ entity = db.ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ.Find(item.PAROUSIA_ID);

                    entity.STATION_ID = stationId;
                    entity.SCHOOLYEARID = SchoolYearId;
                    entity.TMIMA_ID = tmimaId;
                    entity.PAROUSIA_DATE = theDate;
                    entity.PAROUSIA_MONTH = theDate.Value.Month;
                    entity.CHILD_ID = item.CHILD_ID;
                    entity.PRESENCE = item.PRESENCE;

                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Parousies_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ChildParousiaViewModel> data)
        {
            if (data.Any())
            {
                foreach (var item in data)
                {
                    ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ entity = db.ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ.Find(item.PAROUSIA_ID);
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ.Remove(entity);
                        db.SaveChanges();
                    }
                }
            }
            return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public bool ParousiaItemExists(ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ entity)
        {
            var data = (from d in db.ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ
                        where d.PAROUSIA_DATE == entity.PAROUSIA_DATE && d.TMIMA_ID == entity.TMIMA_ID && d.CHILD_ID == entity.CHILD_ID
                        select d).ToList();
            if (data.Count > 0) return true;
            else return false;
        }

        public ActionResult LoadTmimaChildren(ParousiesParameters p)
        {
            string msg = "";
            int stationId = 0;
            int SchoolYearId = 0;

            if (p.tmimaId == 0 || p.theDate == null)
            {
                msg = "Δεν έχει γίνει επιλογή ενός τμήματος ή/και ημερομηνίας. Η φόρτωση ακυρώθηκε.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            var TmimaData = c.GetTmimaData(p.tmimaId);
            if (TmimaData != null)
            {
                stationId = (int)TmimaData.ΒΝΣ;
                SchoolYearId = (int)TmimaData.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
            }

            var srcData = (from d in db.sqlCHILD_TMIMA_SELECTOR where d.ΤΜΗΜΑ == p.tmimaId orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ select d).ToList();
            if (srcData.Count > 0)
            {
                foreach (var item in srcData)
                {
                    // first check if record already exists
                    if (CanCreateChildParousia(p.tmimaId, item.CHILD_ID, p.theDate))
                    {
                        ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ target = new ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ()
                        {
                            STATION_ID = stationId,
                            SCHOOLYEARID = SchoolYearId,
                            TMIMA_ID = p.tmimaId,
                            PAROUSIA_DATE = p.theDate,
                            PAROUSIA_MONTH = p.theDate.Month,
                            CHILD_ID = item.CHILD_ID,
                            PRESENCE = true
                        };
                        db.ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ.Add(target);
                        db.SaveChanges();
                    }
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public bool CanCreateChildParousia(int tmimaId, int childId, DateTime theDate)
        {
            var data = (from d in db.ΠΑΙΔΙΑ_ΠΑΡΟΥΣΙΕΣ where d.TMIMA_ID == tmimaId && d.CHILD_ID == childId && d.PAROUSIA_DATE == theDate select d).ToList();
            if (data.Count > 0) return false;
            else return true;

        }

        #endregion

        #endregion

        #region ΜΗΝΙΑΙΕΣ ΚΑΤΑΣΤΑΣΕΙΣ ΠΑΡΟΥΣΙΩΝ ΒΡΕΦΟΝΗΠΙΩΝ

        public ActionResult ParousiesMonths(string notify = null)
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

            if (!SchoolYearsExist() || !ParousiesMonthExist())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει να έχουν καταχωρηθεί παρουσίες των παιδιών η/και καταχωρημένα σχολικά έτη.";
                return RedirectToAction("Index", "Station", new { notify = msg });
            }

            populateSchoolYears();
            return View();
        }

        public ActionResult ParousiesMonth_Read([DataSourceRequest] DataSourceRequest request, int tmimaId = 0, int monthId = 0)
        {
            var data = GetParousiesMonth(tmimaId, monthId);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public ActionResult ParousiesMonth_Update([DataSourceRequest] DataSourceRequest request,
                    [Bind(Prefix = "models")]IEnumerable<ParousiesMonthViewModel> data, int tmimaId = 0, int monthId = 0)
        {
            if (tmimaId == 0 || monthId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει τμήμα και μήνα. Η διαδικασία ακυρώθηκε.");
            }
            if (data != null && ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ entity = db.ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ.Find(item.ROW_ID);
                    // only one field can be edited
                    entity.ΠΑΡΑΤΗΡΗΣΕΙΣ = item.ΠΑΡΑΤΗΡΗΣΕΙΣ;

                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ParousiesMonthViewModel RefreshParousiesMonth(int rowId)
        {
            var data = (from d in db.ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ
                        where d.ROW_ID == rowId
                        select new ParousiesMonthViewModel
                        {
                            ROW_ID = d.ROW_ID,
                            CHILD_ID = d.CHILD_ID,
                            PAROUSIA_MONTH = d.PAROUSIA_MONTH,
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            TMIMA_ID = d.TMIMA_ID,
                            ΑΜ = d.ΑΜ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΟΝΟΜΑΤΕΠΩΝΥΜΟ = d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ,
                            ΠΛΗΘΟΣ = d.ΠΛΗΘΟΣ,
                            ΠΑΡΑΤΗΡΗΣΕΙΣ = d.ΠΑΡΑΤΗΡΗΣΕΙΣ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΤΜΗΜΑ_ΟΝΟΜΑ = d.ΤΜΗΜΑ_ΟΝΟΜΑ
                        }).FirstOrDefault();

            return (data);
        }

        public List<ParousiesMonthViewModel> GetParousiesMonth(int tmimaId, int monthId)
        {
            var data = (from d in db.ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ
                        where d.TMIMA_ID == tmimaId && d.PAROUSIA_MONTH == monthId
                        select new ParousiesMonthViewModel
                        {
                            ROW_ID = d.ROW_ID,
                            CHILD_ID = d.CHILD_ID,
                            PAROUSIA_MONTH = d.PAROUSIA_MONTH,
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            TMIMA_ID = d.TMIMA_ID,
                            ΑΜ = d.ΑΜ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΟΝΟΜΑΤΕΠΩΝΥΜΟ = d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ,
                            ΠΛΗΘΟΣ = d.ΠΛΗΘΟΣ,
                            ΠΑΡΑΤΗΡΗΣΕΙΣ = d.ΠΑΡΑΤΗΡΗΣΕΙΣ,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΤΜΗΜΑ_ΟΝΟΜΑ = d.ΤΜΗΜΑ_ΟΝΟΜΑ
                        }).ToList();

            return (data);
        }

        #region ΛΕΙΤΟΥΡΓΙΕΣ ΔΗΜΙΟΥΡΓΙΑΣ-ΕΝΗΜΕΡΩΣΗΣ-ΕΚΤΥΠΩΣΗΣ ΠΙΝΑΚΑ ΠΑΡΟΥΣΙΩΝ

        public ActionResult CreateParousiesTable(int tmimaId, int monthId)
        {
            string msg = "Η δημιουργία του πίνακα μηνιαίων παρουσιών του τμήματος ολοκληρώθηκε.";

            var source = (from d in db.repΠΑΡΟΥΣΙΕΣ_ΜΗΝΑΣ where d.TMIMA_ID == tmimaId && d.PAROUSIA_MONTH == monthId orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ select d).ToList();
            if (source.Count() == 0)
            {
                msg = "Δεν βρέθηκαν καταχωρημένες παρουσίες για το τμήμα και μήνα αυτό. Η δημιουργία του πίνακα είναι αδύνατη.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            List<ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ> target = (from d in db.ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ where d.TMIMA_ID == tmimaId && d.PAROUSIA_MONTH == monthId select d).ToList();
            if (target.Count > 0)
            {
                msg = "Ο πίνακας παρουσιών έχει ήδη εγγραφές για το τμήμα και μήνα αυτό. Για ενημέρωση πατήστε 'Ενημέρωση πίνακα τμήματος'";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                foreach (var item in source)
                {
                    ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ entity = new ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ()
                    {
                        CHILD_ID = item.CHILD_ID,
                        PAROUSIA_MONTH = item.PAROUSIA_MONTH,
                        SCHOOLYEAR_ID = item.SCHOOLYEAR_ID,
                        TMIMA_ID = item.TMIMA_ID,
                        ΑΜ = item.ΑΜ,
                        ΒΝΣ = item.ΒΝΣ,
                        ΟΝΟΜΑΤΕΠΩΝΥΜΟ = item.ΟΝΟΜΑΤΕΠΩΝΥΜΟ,
                        ΠΛΗΘΟΣ = item.ΠΛΗΘΟΣ,
                        ΣΧΟΛΙΚΟ_ΕΤΟΣ = item.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                        ΤΜΗΜΑ_ΟΝΟΜΑ = item.ΤΜΗΜΑ_ΟΝΟΜΑ
                    };
                    db.ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ.Add(entity);
                    db.SaveChanges();
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateParousiesTable(int tmimaId, int monthId)
        {
            string msg = "Η ενημέρωση του πίνακα παρουσιών του τμήματος ολοκληρώθηκε.";

            var source = (from d in db.repΠΑΡΟΥΣΙΕΣ_ΜΗΝΑΣ where d.TMIMA_ID == tmimaId && d.PAROUSIA_MONTH == monthId orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ select d).ToList();
            if (source.Count() == 0)
            {
                msg = "Δεν βρέθηκαν καταχωρημένες παρουσίες για το τμήμα και μήνα αυτό. Η δημιουργία του πίνακα είναι αδύνατη.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            List<ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ> target = (from d in db.ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ where d.TMIMA_ID == tmimaId && d.PAROUSIA_MONTH == monthId select d).ToList();
            if (target.Count == 0)
            {
                msg = "Ο πίνακας παρουσιών δεν έχει εγγραφές για το τμήμα και μήνα αυτό. Για τη δημιουργία των εγγραφών πατήστε 'Δημιουργία πίνακα τμήματος'";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // First delete relevant records
                foreach (var item in target)
                {
                    ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ entity = db.ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ.Find(item.ROW_ID);
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ.Remove(entity);
                        db.SaveChanges();
                    }
                }
                // Secondly, create relevant records
                foreach (var item in source)
                {
                    ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ entity = new ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ()
                    {
                        CHILD_ID = item.CHILD_ID,
                        PAROUSIA_MONTH = item.PAROUSIA_MONTH,
                        SCHOOLYEAR_ID = item.SCHOOLYEAR_ID,
                        TMIMA_ID = item.TMIMA_ID,
                        ΑΜ = item.ΑΜ,
                        ΒΝΣ = item.ΒΝΣ,
                        ΟΝΟΜΑΤΕΠΩΝΥΜΟ = item.ΟΝΟΜΑΤΕΠΩΝΥΜΟ,
                        ΠΛΗΘΟΣ = item.ΠΛΗΘΟΣ,
                        ΣΧΟΛΙΚΟ_ΕΤΟΣ = item.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                        ΤΜΗΜΑ_ΟΝΟΜΑ = item.ΤΜΗΜΑ_ΟΝΟΜΑ
                    };
                    db.ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ.Add(entity);
                    db.SaveChanges();
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DestroyParousiesTable(int tmimaId, int monthId)
        {
            string msg = "Η διαγραφή του πίνακα παρουσιών του τμήματος ολοκληρώθηκε.";

            var data = (from d in db.ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ where d.TMIMA_ID == tmimaId && d.PAROUSIA_MONTH == monthId select d).ToList();
            if (data.Count == 0)
            {
                msg = "Δεν βρέθηκαν καταχωρημένες παρουσίες για το τμήμα και μήνα αυτό. Η διαγραφή του πίνακα ακυρώθηκε.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            foreach (var item in data)
            {
                ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ entity = db.ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ.Find(item.ROW_ID);
                if (entity != null)
                {
                    db.Entry(entity).State = EntityState.Deleted;
                    db.ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ.Remove(entity);
                    db.SaveChanges();
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ParousiesMonthPrint(int tmimaId, int monthId)
        {
            var data = (from d in db.ΠΑΡΟΥΣΙΕΣ_ΜΗΝΕΣ
                        where d.TMIMA_ID == tmimaId && d.PAROUSIA_MONTH == monthId
                        select new ParousiesMonthViewModel
                        {
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            TMIMA_ID = d.TMIMA_ID,
                            ΒΝΣ = d.ΒΝΣ,
                            PAROUSIA_MONTH = d.PAROUSIA_MONTH
                        }).FirstOrDefault();

            return View(data);
        }

        // Έντυπο παρουσιών χωρίς το πεδίο "Παρατηρήσεις"
        public ActionResult ParousiesMonthPrint2(int tmimaId)
        {
            var data = (from d in db.repΠΑΡΟΥΣΙΕΣ_ΜΗΝΑΣ
                        where d.TMIMA_ID == tmimaId
                        select new repParousiesMonthViewModel
                        {
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            TMIMA_ID = d.TMIMA_ID,
                            ΒΝΣ = d.ΒΝΣ,
                        }).FirstOrDefault();

            return View(data);
        }

        #endregion

        #endregion ΜΗΝΙΑΙΕΣ ΚΑΤΑΣΤΑΣΕΙΣ ΠΑΡΟΥΣΙΩΝ ΒΡΕΦΟΝΗΠΙΩΝ

        #region ΑΠΟΥΣΙΕΣ ΑΝΑΛΥΤΙΚΕΣ ΜΗΝΙΑΙΕΣ

        public ActionResult ApousiesDetail(string notify = null)
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

            if (!SchoolYearsExist() || !ParousiesMonthExist())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει να έχουν καταχωρηθεί παρουσίες των παιδιών η/και καταχωρημένα σχολικά έτη.";
                return RedirectToAction("Index", "Station", new { notify = msg });
            }

            populateSchoolYears();
            return View();
        }

        public ActionResult ApousiesDetail_Read([DataSourceRequest] DataSourceRequest request, int tmimaId = 0, int monthId = 0)
        {
            var data = GetApousiesDetail(tmimaId, monthId);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<ApousiesDetailViewModel> GetApousiesDetail(int tmimaId, int monthId)
        {
            var data = (from d in db.sqlCHILD_APOUSIES_DETAIL
                        where d.TMIMA_ID == tmimaId && d.PAROUSIA_MONTH == monthId
                        orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ, d.PAROUSIA_DATE
                        select new ApousiesDetailViewModel
                        {
                            PAROUSIA_ID = d.PAROUSIA_ID,
                            CHILD_ID = d.CHILD_ID,
                            PAROUSIA_MONTH = d.PAROUSIA_MONTH,
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            TMIMA_ID = d.TMIMA_ID,
                            STATION_ID = d.STATION_ID,
                            ΟΝΟΜΑΤΕΠΩΝΥΜΟ = d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ,
                            PAROUSIA_DATE = d.PAROUSIA_DATE,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΤΜΗΜΑ_ΟΝΟΜΑ = d.ΤΜΗΜΑ_ΟΝΟΜΑ,
                            ΜΗΝΑΣ = d.ΜΗΝΑΣ
                        }).ToList();

            return (data);
        }

        public ActionResult ApousiesDetailPrint(int tmimaId, int monthId)
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
            var data = (from d in db.sqlCHILD_APOUSIES_DETAIL
                        where d.TMIMA_ID == tmimaId && d.PAROUSIA_MONTH == monthId
                        select new ApousiesDetailViewModel
                        {
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            TMIMA_ID = d.TMIMA_ID,
                            STATION_ID = d.STATION_ID,
                            PAROUSIA_MONTH = d.PAROUSIA_MONTH
                        }).FirstOrDefault();

            return View(data);
        }

        #endregion

        #region ΑΠΟΥΣΙΕΣ ΣΥΓΚΕΝΤΡΩΤΙΚΕΣ ΜΗΝΙΑΙΕΣ

        public ActionResult ApousiesSum(string notify = null)
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

            if (!SchoolYearsExist() || !ParousiesMonthExist())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει να έχουν καταχωρηθεί παρουσίες των παιδιών η/και καταχωρημένα σχολικά έτη.";
                return RedirectToAction("Index", "Station", new { notify = msg });
            }

            populateSchoolYears();
            return View();
        }

        public ActionResult ApousiesSum_Read([DataSourceRequest] DataSourceRequest request, int tmimaId = 0, int monthId = 0)
        {
            var data = GetApousiesSum(tmimaId, monthId);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<ApousiesSumViewModel> GetApousiesSum(int tmimaId, int monthId)
        {
            var data = (from d in db.sqlCHILD_APOUSIES_SUM
                        where d.TMIMA_ID == tmimaId && d.PAROUSIA_MONTH == monthId
                        orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ
                        select new ApousiesSumViewModel
                        {
                            CHILD_ID = d.CHILD_ID,
                            PAROUSIA_MONTH = d.PAROUSIA_MONTH,
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            TMIMA_ID = d.TMIMA_ID,
                            STATION_ID = d.STATION_ID,
                            ΟΝΟΜΑΤΕΠΩΝΥΜΟ = d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ,
                            ΑPOUSIES = d.ΑPOUSIES,
                            ΣΧΟΛΙΚΟ_ΕΤΟΣ = d.ΣΧΟΛΙΚΟ_ΕΤΟΣ,
                            ΤΜΗΜΑ_ΟΝΟΜΑ = d.ΤΜΗΜΑ_ΟΝΟΜΑ,
                            ΜΗΝΑΣ = d.ΜΗΝΑΣ
                        }).ToList();

            return (data);
        }

        public ActionResult ApousiesSumPrint(int tmimaId, int monthId)
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
            var data = (from d in db.sqlCHILD_APOUSIES_SUM
                        where d.TMIMA_ID == tmimaId && d.PAROUSIA_MONTH == monthId
                        select new ApousiesSumViewModel
                        {
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            TMIMA_ID = d.TMIMA_ID,
                            STATION_ID = d.STATION_ID,
                            PAROUSIA_MONTH = d.PAROUSIA_MONTH
                        }).FirstOrDefault();

            return View(data);
        }

        #endregion


        #region ΓΕΥΜΑΤΑ (ΠΡΩΙΝΟ, ΜΕΣΗΜΕΡΙΑΝΟ, ΒΡΕΦΙΚΟ)

        public ActionResult MealsList(string notify = null)
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
            return View();
        }

        #region ΠΛΕΓΜΑ ΠΡΩΙΝΩΝ ΓΕΥΜΑΤΩΝ

        public ActionResult MealMorning_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = GetMealMorningFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealMorning_Create([DataSourceRequest] DataSourceRequest request, MealMorningViewModel data)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            MealMorningViewModel newdata = new MealMorningViewModel();

            if (data != null && ModelState.IsValid)
            {
                ΓΕΥΜΑΤΑ_ΠΡΩΙ entity = new ΓΕΥΜΑΤΑ_ΠΡΩΙ()
                {
                    ΒΝΣ = stationId,
                    ΠΡΩΙΝΟ = data.ΠΡΩΙΝΟ,
                    ΣΧΟΛΙΟ = data.ΣΧΟΛΙΟ
                };
                db.ΓΕΥΜΑΤΑ_ΠΡΩΙ.Add(entity);
                db.SaveChanges();
                data.ΠΡΩΙΝΟ_ΚΩΔ = entity.ΠΡΩΙΝΟ_ΚΩΔ;   // grid requires the new generated id
                newdata = RefreshMealMorningFromDB(data.ΠΡΩΙΝΟ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealMorning_Update([DataSourceRequest] DataSourceRequest request, MealMorningViewModel data)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            MealMorningViewModel newdata = new MealMorningViewModel();

            if (data != null && ModelState.IsValid)
            {
                ΓΕΥΜΑΤΑ_ΠΡΩΙ entity = db.ΓΕΥΜΑΤΑ_ΠΡΩΙ.Find(data.ΠΡΩΙΝΟ_ΚΩΔ);

                entity.ΠΡΩΙΝΟ = data.ΠΡΩΙΝΟ;
                entity.ΣΧΟΛΙΟ = data.ΣΧΟΛΙΟ;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newdata = RefreshMealMorningFromDB(data.ΠΡΩΙΝΟ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealMorning_Destroy([DataSourceRequest] DataSourceRequest request, MealMorningViewModel data)
        {
            ΓΕΥΜΑΤΑ_ΠΡΩΙ entity = db.ΓΕΥΜΑΤΑ_ΠΡΩΙ.Find(data.ΠΡΩΙΝΟ_ΚΩΔ);

            if (!k.CanDeleteMealMorning(data.ΠΡΩΙΝΟ_ΚΩΔ))
                ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί το γεύμα διότι χρησιμοποιείται ήδη στα διαιτολόγια.");

            if (entity != null && ModelState.IsValid)
            {
                db.Entry(entity).State = EntityState.Deleted;
                db.ΓΕΥΜΑΤΑ_ΠΡΩΙ.Remove(entity);
                db.SaveChanges();
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public MealMorningViewModel RefreshMealMorningFromDB(int recordId)
        {
            var data = (from d in db.ΓΕΥΜΑΤΑ_ΠΡΩΙ
                        where d.ΠΡΩΙΝΟ_ΚΩΔ == recordId
                        select new MealMorningViewModel
                        {
                            ΠΡΩΙΝΟ_ΚΩΔ = d.ΠΡΩΙΝΟ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΠΡΩΙΝΟ = d.ΠΡΩΙΝΟ,
                            ΣΧΟΛΙΟ = d.ΣΧΟΛΙΟ
                        }).FirstOrDefault();

            return (data);
        }

        public List<MealMorningViewModel> GetMealMorningFromDB()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΓΕΥΜΑΤΑ_ΠΡΩΙ
                        where d.ΒΝΣ == stationId
                        orderby d.ΠΡΩΙΝΟ
                        select new MealMorningViewModel
                        {
                            ΠΡΩΙΝΟ_ΚΩΔ = d.ΠΡΩΙΝΟ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΠΡΩΙΝΟ = d.ΠΡΩΙΝΟ,
                            ΣΧΟΛΙΟ = d.ΣΧΟΛΙΟ
                        }).ToList();

            return (data);
        }

        #endregion

        #region ΠΛΕΓΜΑ ΜΕΣΗΜΕΡΙΑΝΩΝ ΓΕΥΜΑΤΩΝ

        public ActionResult MealNoon_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = GetMealNoonFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealNoon_Create([DataSourceRequest] DataSourceRequest request, MealNoonViewModel data)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            MealNoonViewModel newdata = new MealNoonViewModel();

            if (data != null && ModelState.IsValid)
            {
                ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ entity = new ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ()
                {
                    ΒΝΣ = stationId,
                    ΜΕΣΗΜΕΡΙΑΝΟ = data.ΜΕΣΗΜΕΡΙΑΝΟ,
                    ΣΧΟΛΙΟ = data.ΣΧΟΛΙΟ
                };
                db.ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ.Add(entity);
                db.SaveChanges();
                data.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ = entity.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ;   // grid requires the new generated id
                newdata = RefreshMealNoonFromDB(data.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealNoon_Update([DataSourceRequest] DataSourceRequest request, MealNoonViewModel data)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            MealNoonViewModel newdata = new MealNoonViewModel();

            if (data != null && ModelState.IsValid)
            {
                ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ entity = db.ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ.Find(data.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ);

                entity.ΜΕΣΗΜΕΡΙΑΝΟ = data.ΜΕΣΗΜΕΡΙΑΝΟ;
                entity.ΣΧΟΛΙΟ = data.ΣΧΟΛΙΟ;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newdata = RefreshMealNoonFromDB(data.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealNoon_Destroy([DataSourceRequest] DataSourceRequest request, MealNoonViewModel data)
        {
            ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ entity = db.ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ.Find(data.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ);

            if (!k.CanDeleteMealNoon(data.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ))
                ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί το γεύμα διότι χρησιμοποιείται ήδη στα διαιτολόγια.");

            if (entity != null && ModelState.IsValid)
            {
                db.Entry(entity).State = EntityState.Deleted;
                db.ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ.Remove(entity);
                db.SaveChanges();
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public MealNoonViewModel RefreshMealNoonFromDB(int recordId)
        {
            var data = (from d in db.ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ
                        where d.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ == recordId
                        select new MealNoonViewModel
                        {
                            ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ = d.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΜΕΣΗΜΕΡΙΑΝΟ = d.ΜΕΣΗΜΕΡΙΑΝΟ,
                            ΣΧΟΛΙΟ = d.ΣΧΟΛΙΟ
                        }).FirstOrDefault();

            return (data);
        }

        public List<MealNoonViewModel> GetMealNoonFromDB()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ
                        where d.ΒΝΣ == stationId
                        orderby d.ΜΕΣΗΜΕΡΙΑΝΟ
                        select new MealNoonViewModel
                        {
                            ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ = d.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΜΕΣΗΜΕΡΙΑΝΟ = d.ΜΕΣΗΜΕΡΙΑΝΟ,
                            ΣΧΟΛΙΟ = d.ΣΧΟΛΙΟ
                        }).ToList();

            return (data);
        }

        #endregion

        #region ΠΛΕΓΜΑ ΒΡΕΦΙΚΩΝ ΓΕΥΜΑΤΩΝ

        public ActionResult MealBaby_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = GetMealBabyFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealBaby_Create([DataSourceRequest] DataSourceRequest request, MealBabyViewModel data)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            MealBabyViewModel newdata = new MealBabyViewModel();

            if (data != null && ModelState.IsValid)
            {
                ΓΕΥΜΑΤΑ_ΒΡΕΦΗ entity = new ΓΕΥΜΑΤΑ_ΒΡΕΦΗ()
                {
                    ΒΝΣ = stationId,
                    ΒΡΕΦΙΚΟ = data.ΒΡΕΦΙΚΟ,
                    ΣΧΟΛΙΟ = data.ΣΧΟΛΙΟ
                };
                db.ΓΕΥΜΑΤΑ_ΒΡΕΦΗ.Add(entity);
                db.SaveChanges();
                data.ΒΡΕΦΙΚΟ_ΚΩΔ = entity.ΒΡΕΦΙΚΟ_ΚΩΔ;   // grid requires the new generated id
                newdata = RefreshMealBabyFromDB(data.ΒΡΕΦΙΚΟ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealBaby_Update([DataSourceRequest] DataSourceRequest request, MealBabyViewModel data)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            MealBabyViewModel newdata = new MealBabyViewModel();

            if (data != null && ModelState.IsValid)
            {
                ΓΕΥΜΑΤΑ_ΒΡΕΦΗ entity = db.ΓΕΥΜΑΤΑ_ΒΡΕΦΗ.Find(data.ΒΡΕΦΙΚΟ_ΚΩΔ);

                entity.ΒΡΕΦΙΚΟ = data.ΒΡΕΦΙΚΟ;
                entity.ΣΧΟΛΙΟ = data.ΣΧΟΛΙΟ;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newdata = RefreshMealBabyFromDB(data.ΒΡΕΦΙΚΟ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealBaby_Destroy([DataSourceRequest] DataSourceRequest request, MealBabyViewModel data)
        {
            ΓΕΥΜΑΤΑ_ΒΡΕΦΗ entity = db.ΓΕΥΜΑΤΑ_ΒΡΕΦΗ.Find(data.ΒΡΕΦΙΚΟ_ΚΩΔ);

            if (!k.CanDeleteMealBaby(data.ΒΡΕΦΙΚΟ_ΚΩΔ))
                ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί το γεύμα διότι χρησιμοποιείται ήδη στα διαιτολόγια.");

            if (entity != null && ModelState.IsValid)
            {
                db.Entry(entity).State = EntityState.Deleted;
                db.ΓΕΥΜΑΤΑ_ΒΡΕΦΗ.Remove(entity);
                db.SaveChanges();
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public MealBabyViewModel RefreshMealBabyFromDB(int recordId)
        {
            var data = (from d in db.ΓΕΥΜΑΤΑ_ΒΡΕΦΗ
                        where d.ΒΡΕΦΙΚΟ_ΚΩΔ == recordId
                        select new MealBabyViewModel
                        {
                            ΒΡΕΦΙΚΟ_ΚΩΔ = d.ΒΡΕΦΙΚΟ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΒΡΕΦΙΚΟ = d.ΒΡΕΦΙΚΟ,
                            ΣΧΟΛΙΟ = d.ΣΧΟΛΙΟ
                        }).FirstOrDefault();

            return (data);
        }

        public List<MealBabyViewModel> GetMealBabyFromDB()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΓΕΥΜΑΤΑ_ΒΡΕΦΗ
                        where d.ΒΝΣ == stationId
                        orderby d.ΒΡΕΦΙΚΟ
                        select new MealBabyViewModel
                        {
                            ΒΡΕΦΙΚΟ_ΚΩΔ = d.ΒΡΕΦΙΚΟ_ΚΩΔ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΒΡΕΦΙΚΟ = d.ΒΡΕΦΙΚΟ,
                            ΣΧΟΛΙΟ = d.ΣΧΟΛΙΟ
                        }).ToList();

            return (data);
        }

        #endregion

        public ActionResult MealsPrint()
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

                MealPlanParameters mpp = new MealPlanParameters();
                mpp.STATION_ID = stationId;
                mpp.DATE_MEALPLAN = "";

                return View(mpp);
            }
        }

        #endregion


        #region ΔΙΑΙΤΟΛΟΓΙΑ ΣΤΑΘΜΟΥ

        public ActionResult MealsPlanList(string notify = null)
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

            if (!k.MealsMorningExist(stationId) || !k.MealsNoonExist(stationId) || !k.MealsBabyExist(stationId))
            {
                string msg = "Για τα διαιτολόγια πρέπει πρώτα να καταχωρηθούν γεύματα στην επιλογή 'Καταχώρηση γευμάτων'";
                return RedirectToAction("Index", "Station", new { notify = msg });
            }

            if (notify != null) this.ShowMessage(MessageType.Warning, notify);

            populateMealsMorning();
            populateMealsNoon();
            populateMealsBaby();

            return View();
        }

        public ActionResult MealPlan_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = GetMealPlanFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealPlan_Create([DataSourceRequest] DataSourceRequest request, DiaitologioViewModel data)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            DiaitologioViewModel newdata = new DiaitologioViewModel();
            MealPlanData mpd = c.GetMealPlanData(stationId, data.ΗΜΕΡΟΜΗΝΙΑ);

            if (data != null && ModelState.IsValid)
            {
                ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ entity = new ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ()
                {
                    ΣΤΑΘΜΟΣ_ΚΩΔ = stationId,
                    ΗΜΕΡΟΜΗΝΙΑ = data.ΗΜΕΡΟΜΗΝΙΑ,
                    ΓΕΥΜΑ_ΒΡΕΦΗ = data.ΓΕΥΜΑ_ΒΡΕΦΗ,
                    ΓΕΥΜΑ_ΜΕΣΗΜΕΡΙ = data.ΓΕΥΜΑ_ΜΕΣΗΜΕΡΙ,
                    ΓΕΥΜΑ_ΠΡΩΙ = data.ΓΕΥΜΑ_ΠΡΩΙ,
                    ΤΡΟΦΙΜΟΙ = mpd.ΤΡΟΦΙΜΟΙ,
                    ΠΡΟΣΩΠΙΚΟ = mpd.ΠΡΟΣΩΠΙΚΟ,
                    ΠΛΗΘΟΣ = mpd.ΠΛΗΘΟΣ,
                    ΚΟΣΤΟΣ = mpd.ΚΟΣΤΟΣ,
                    ΔΑΠΑΝΗ = mpd.ΔΑΠΑΝΗ
                };
                db.ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ.Add(entity);
                db.SaveChanges();
                data.ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ = entity.ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ;   // grid requires the new generated id
                newdata = RefreshMealPlanFromDB(data.ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealPlan_Update([DataSourceRequest] DataSourceRequest request, DiaitologioViewModel data)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            DiaitologioViewModel newdata = new DiaitologioViewModel();

            MealPlanData mpd = c.GetMealPlanData(stationId, data.ΗΜΕΡΟΜΗΝΙΑ);

            if (data != null && ModelState.IsValid)
            {
                ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ entity = db.ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ.Find(data.ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ);

                entity.ΣΤΑΘΜΟΣ_ΚΩΔ = stationId;
                entity.ΗΜΕΡΟΜΗΝΙΑ = data.ΗΜΕΡΟΜΗΝΙΑ;
                entity.ΓΕΥΜΑ_ΠΡΩΙ = data.ΓΕΥΜΑ_ΠΡΩΙ;
                entity.ΓΕΥΜΑ_ΜΕΣΗΜΕΡΙ = data.ΓΕΥΜΑ_ΜΕΣΗΜΕΡΙ;
                entity.ΓΕΥΜΑ_ΒΡΕΦΗ = data.ΓΕΥΜΑ_ΒΡΕΦΗ;
                entity.ΤΡΟΦΙΜΟΙ = mpd.ΤΡΟΦΙΜΟΙ;
                entity.ΠΡΟΣΩΠΙΚΟ = mpd.ΠΡΟΣΩΠΙΚΟ;
                entity.ΠΛΗΘΟΣ = mpd.ΠΛΗΘΟΣ;
                entity.ΚΟΣΤΟΣ = mpd.ΚΟΣΤΟΣ;
                entity.ΔΑΠΑΝΗ = mpd.ΔΑΠΑΝΗ;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newdata = RefreshMealPlanFromDB(data.ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealPlan_Destroy([DataSourceRequest] DataSourceRequest request, DiaitologioViewModel data)
        {
            ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ entity = db.ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ.Find(data.ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ);

            if (entity != null && ModelState.IsValid)
            {
                db.Entry(entity).State = EntityState.Deleted;
                db.ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ.Remove(entity);
                db.SaveChanges();
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public DiaitologioViewModel RefreshMealPlanFromDB(int rowId)
        {
            var data = (from d in db.ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ
                        where d.ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ == rowId
                        select new DiaitologioViewModel
                        {
                            ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ = d.ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ,
                            ΓΕΥΜΑ_ΒΡΕΦΗ = d.ΓΕΥΜΑ_ΒΡΕΦΗ,
                            ΓΕΥΜΑ_ΜΕΣΗΜΕΡΙ = d.ΓΕΥΜΑ_ΜΕΣΗΜΕΡΙ,
                            ΓΕΥΜΑ_ΠΡΩΙ = d.ΓΕΥΜΑ_ΠΡΩΙ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΣΤΑΘΜΟΣ_ΚΩΔ = d.ΣΤΑΘΜΟΣ_ΚΩΔ,
                            ΤΡΟΦΙΜΟΙ = d.ΤΡΟΦΙΜΟΙ,
                            ΠΡΟΣΩΠΙΚΟ = d.ΠΡΟΣΩΠΙΚΟ,
                            ΠΛΗΘΟΣ = d.ΠΛΗΘΟΣ,
                            ΚΟΣΤΟΣ = d.ΚΟΣΤΟΣ,
                            ΔΑΠΑΝΗ = d.ΔΑΠΑΝΗ
                        }).FirstOrDefault();

            return (data);
        }

        public List<DiaitologioViewModel> GetMealPlanFromDB()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ
                        where d.ΣΤΑΘΜΟΣ_ΚΩΔ == stationId
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ
                        select new DiaitologioViewModel
                        {
                            ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ = d.ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ,
                            ΓΕΥΜΑ_ΒΡΕΦΗ = d.ΓΕΥΜΑ_ΒΡΕΦΗ,
                            ΓΕΥΜΑ_ΜΕΣΗΜΕΡΙ = d.ΓΕΥΜΑ_ΜΕΣΗΜΕΡΙ,
                            ΓΕΥΜΑ_ΠΡΩΙ = d.ΓΕΥΜΑ_ΠΡΩΙ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΣΤΑΘΜΟΣ_ΚΩΔ = d.ΣΤΑΘΜΟΣ_ΚΩΔ,
                            ΤΡΟΦΙΜΟΙ = d.ΤΡΟΦΙΜΟΙ,
                            ΠΡΟΣΩΠΙΚΟ = d.ΠΡΟΣΩΠΙΚΟ,
                            ΠΛΗΘΟΣ = d.ΠΛΗΘΟΣ,
                            ΚΟΣΤΟΣ = d.ΚΟΣΤΟΣ,
                            ΔΑΠΑΝΗ = d.ΔΑΠΑΝΗ
                        }).ToList();

            return (data);
        }

        #region CHILD GRID WITH PERSONS AND FEEDING COST

        public ActionResult SumPersonsFood_Get([DataSourceRequest] DataSourceRequest request, DateTime? theDate = null)
        {

            var data = GetSumPersonsFood2FromDB(theDate);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<SumPersonsTrofeioViewModel> GetSumPersonsFood2FromDB(DateTime? theDate)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            List<SumPersonsTrofeioViewModel> data = new List<SumPersonsTrofeioViewModel>();

            if (theDate != null)
            {
                data = (from d in db.sqlΣΥΝΟΛΟ_ΑΤΟΜΑ_ΤΡΟΦΕΙΟ
                        where d.STATION_ID == stationId && (d.ΗΜΕΡΟΜΗΝΙΑ == theDate)
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ
                        select new SumPersonsTrofeioViewModel
                        {
                            ROWID = d.ROWID,
                            SCHOOLYEARID = d.SCHOOLYEARID,
                            STATION_ID = d.STATION_ID,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΠΑΙΔΙΑ = d.ΠΑΙΔΙΑ,
                            ΠΡΟΣΩΠΙΚΟ = d.ΠΡΟΣΩΠΙΚΟ,
                            ΑΤΟΜΑ = d.ΑΤΟΜΑ,
                            ΚΟΣΤΟΣ_ΗΜΕΡΑ = d.ΚΟΣΤΟΣ_ΗΜΕΡΑ,
                            ΔΑΠΑΝΗ_ΗΜΕΡΑ = d.ΔΑΠΑΝΗ_ΗΜΕΡΑ,
                            ΥΠΟΛΟΙΠΟ = d.ΥΠΟΛΟΙΠΟ
                        }).ToList();
            }
            return (data);
        }

        #endregion

        public ActionResult DiaitologioDayPrint(string theDate)
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

                MealPlanParameters mpp = new MealPlanParameters();
                mpp.STATION_ID = stationId;
                mpp.DATE_MEALPLAN = theDate;

                return View(mpp);
            }
        }

        #endregion

        #region ΔΙΑΙΤΟΛΟΓΙΑ ΕΥΡΕΤΗΡΙΟ

        public ActionResult MealsPlanSearch(string notify = null)
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

            return View();
        }

        public ActionResult MealPlanSearch_Read([DataSourceRequest] DataSourceRequest request, DateTime? theDate1, DateTime? theDate2)
        {
            var data = GetMealPlanSearchFromDB(theDate1, theDate2);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<sqlMealPlanViewModel> GetMealPlanSearchFromDB(DateTime? theDate1, DateTime? theDate2)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.sqlΔΙΑΙΤΟΛΟΓΙΟ_ΗΜΕΡΑ
                        where d.ΣΤΑΘΜΟΣ_ΚΩΔ == stationId && d.ΗΜΕΡΟΜΗΝΙΑ >= theDate1 && d.ΗΜΕΡΟΜΗΝΙΑ <= theDate2
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ
                        select new sqlMealPlanViewModel
                        {
                            ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ = d.ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ,
                            ΠΡΩΙΝΟ = d.ΠΡΩΙΝΟ,
                            ΜΕΣΗΜΕΡΙΑΝΟ = d.ΜΕΣΗΜΕΡΙΑΝΟ,
                            ΒΡΕΦΙΚΟ = d.ΒΡΕΦΙΚΟ,
                            DAY_NUM = d.DAY_NUM,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΣΤΑΘΜΟΣ_ΚΩΔ = d.ΣΤΑΘΜΟΣ_ΚΩΔ,
                            ΤΡΟΦΙΜΟΙ = d.ΤΡΟΦΙΜΟΙ,
                            ΠΡΟΣΩΠΙΚΟ = d.ΠΡΟΣΩΠΙΚΟ,
                            ΠΛΗΘΟΣ = d.ΠΛΗΘΟΣ,
                            ΚΟΣΤΟΣ = d.ΚΟΣΤΟΣ,
                            ΔΑΠΑΝΗ = d.ΔΑΠΑΝΗ
                        }).ToList();

            return (data);
        }

        public ActionResult MealPlanPrint(string theDate1, string theDate2)
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

                DiaitologioParameters dp = new DiaitologioParameters();
                dp.STATION_ID = stationId;
                dp.DATE_START = theDate1;
                dp.DATE_END = theDate2;

                return View(dp);
            }

        }

        #endregion


        #region ΔΑΠΑΝΕΣ ΤΡΟΦΕΙΟΥ, ΚΑΘΑΡΙΟΤΗΤΑΣ ΚΑΙ ΓΕΝΙΚΩΝ

        #region ΠΑΡΑΜΕΤΡΟΙ (ΤΡΟΦΕΙΟ/ΑΤΟΜΟ)

        public ActionResult PersonCostDay(string notify = null)
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

            populateStations();
            return View();
        }

        public ActionResult PersonCost_Read([DataSourceRequest] DataSourceRequest request, int schoolyearId = 0)
        {
            List<PersonCostDayViewModel> data = GetPersonCostDayFromDB(schoolyearId);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<PersonCostDayViewModel> GetPersonCostDayFromDB(int schoolyearId = 0)
        {
            var data = (from d in db.ΤΡΟΦΕΙΟ_ΑΤΟΜΟ
                        where d.SCHOOLYEARID == schoolyearId
                        orderby d.ΣΥΣ_ΣΤΑΘΜΟΙ.ΕΠΩΝΥΜΙΑ
                        select new PersonCostDayViewModel
                        {
                            ID = d.ID,
                            SCHOOLYEARID = d.SCHOOLYEARID,
                            STATION_ID = d.STATION_ID,
                            COST_PERSON = d.COST_PERSON
                        }).ToList();

            return (data);
        }

        #endregion

        #region ΠΑΡΑΜΕΤΡΟΙ - ΚΑΤΗΓΟΡΙΕΣ ΓΕΝΙΚΩΝ ΔΑΠΑΝΩΝ (03-02-2020)

        public ActionResult ExtraCategories()
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

            populateExpenseTypes();
            return View();
        }

        public ActionResult ExtraCategory_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = GetExtraCategoriesFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<ExtraCategoryViewModel> GetExtraCategoriesFromDB()
        {
            var data = (from d in db.ΕΞΤΡΑ_ΚΑΤΗΓΟΡΙΑ
                        select new ExtraCategoryViewModel
                        {
                            ΚΑΤΗΓΟΡΙΑ_ΚΩΔ = d.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΔΑΠΑΝΗ_ΚΩΔ = d.ΔΑΠΑΝΗ_ΚΩΔ
                        }).ToList();

            return data;
        }

        #endregion

        #region ΠΡΟΫΠΟΛΟΓΙΣΜΕΝΑ ΜΗΝΙΑΙΑ ΠΟΣΑ ΔΑΠΑΝΩΝ

        public ActionResult BudgetData(string notify = null)
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

            populateStations();
            return View();
        }

        public ActionResult BudgetMonth_Read([DataSourceRequest] DataSourceRequest request, int schoolyearId = 0, int monthId = 0)
        {
            List<BudgetDataViewModel> data = GetBudgetDataFromDB(schoolyearId, monthId);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<BudgetDataViewModel> GetBudgetDataFromDB(int schoolyearId = 0, int monthId = 0)
        {
            var data = (from d in db.BUDGET_DATA
                        orderby d.ΣΥΣ_ΣΤΑΘΜΟΙ.ΕΠΩΝΥΜΙΑ
                        where d.SCHOOLYEAR_ID == schoolyearId && d.BUDGET_MONTH == monthId
                        select new BudgetDataViewModel
                        {
                            BUDGET_ID = d.BUDGET_ID,
                            STATION_ID = d.STATION_ID,
                            SCHOOLYEAR_ID = d.SCHOOLYEAR_ID,
                            BUDGET_MONTH = d.BUDGET_MONTH,
                            BUDGET_CLEAN = d.BUDGET_CLEAN,
                            BUDGET_OTHER = d.BUDGET_OTHER,
                            BUDGET_FOOD = d.BUDGET_FOOD
                        }).ToList();

            return (data);
        }

        public ActionResult BudgetDataPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                return View();
            }
        }

        #endregion


        #region ΠΡΟΪΟΝΤΑ ΑΝΑ ΚΑΤΗΓΟΡΙΑ ΚΑΤΑΧΩΡΗΣΗ

        public ActionResult ProductData(string notify = null)
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

            populateProductUnits();
            populateExpenseTypes();
            populateVATValues();
            return View();
        }

        public ActionResult Category_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<ProductCategoryViewModel> data = GetProductCategoriesFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<ProductCategoryViewModel> GetProductCategoriesFromDB()
        {
            var data = (from d in db.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ
                        orderby d.ΔΑΠΑΝΗ_ΚΩΔ, d.ΚΑΤΗΓΟΡΙΑ
                        select new ProductCategoryViewModel
                        {
                            ΚΑΤΗΓΟΡΙΑ_ΚΩΔ = d.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΔΑΠΑΝΗ_ΚΩΔ = d.ΔΑΠΑΝΗ_ΚΩΔ
                        }).ToList();

            return (data);
        }

        public ActionResult Product_Read([DataSourceRequest] DataSourceRequest request, int categoryId = 0)
        {
            var data = GetProductsFromDB(categoryId);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Product_Create([DataSourceRequest] DataSourceRequest request, ProductStationViewModel data, int categoryId = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            ProductStationViewModel newdata = new ProductStationViewModel();

            if (categoryId > 0)
            {
                if (data != null && ModelState.IsValid)
                {
                    ΠΡΟΙΟΝΤΑ_ΒΝΣ entity = new ΠΡΟΙΟΝΤΑ_ΒΝΣ()
                    {
                        ΒΝΣ = stationId,
                        ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ = categoryId,
                        ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ = data.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ,
                        ΠΡΟΙΟΝ_ΜΟΝΑΔΑ = data.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ,
                        ΠΡΟΙΟΝ_ΦΠΑ = data.ΠΡΟΙΟΝ_ΦΠΑ
                    };
                    db.ΠΡΟΙΟΝΤΑ_ΒΝΣ.Add(entity);
                    db.SaveChanges();
                    data.ΠΡΟΙΟΝ_ΚΩΔ = entity.ΠΡΟΙΟΝ_ΚΩΔ;   // grid requires the new generated id
                    newdata = RefreshProductFromDB(entity.ΠΡΟΙΟΝ_ΚΩΔ);
                }
            }
            else
            {
                ModelState.AddModelError("", "Πρέπει να προηγηθεί επιλογή κατηγορίας για καταχώρηση του προϊόντος.");
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Product_Update([DataSourceRequest] DataSourceRequest request, ProductStationViewModel data, int categoryId = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            ProductStationViewModel newdata = new ProductStationViewModel();

            if (categoryId > 0)
            {
                if (data != null && ModelState.IsValid)
                {
                    ΠΡΟΙΟΝΤΑ_ΒΝΣ entity = db.ΠΡΟΙΟΝΤΑ_ΒΝΣ.Find(data.ΠΡΟΙΟΝ_ΚΩΔ);

                    entity.ΒΝΣ = stationId;
                    entity.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ = categoryId;
                    entity.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ = data.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ;
                    entity.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ = data.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ;
                    entity.ΠΡΟΙΟΝ_ΦΠΑ = data.ΠΡΟΙΟΝ_ΦΠΑ;

                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                    newdata = RefreshProductFromDB(entity.ΠΡΟΙΟΝ_ΚΩΔ);
                }
            }
            else
            {
                ModelState.AddModelError("", "Πρέπει πρώτα να γίνει επιλογή κατηγορίας προϊόντων. Η ενημέρωση ακυρώθηκε.");
                return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Product_Destroy([DataSourceRequest] DataSourceRequest request, ProductStationViewModel data)
        {
            if (data != null)
            {
                ΠΡΟΙΟΝΤΑ_ΒΝΣ entity = db.ΠΡΟΙΟΝΤΑ_ΒΝΣ.Find(data.ΠΡΟΙΟΝ_ΚΩΔ);
                if (k.CanDeleteProduct(data.ΠΡΟΙΟΝ_ΚΩΔ))
                {
                    if (entity != null)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                        db.ΠΡΟΙΟΝΤΑ_ΒΝΣ.Remove(entity);
                        db.SaveChanges();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Δεν μπορεί να γίνει διαγραφή διότι το προϊόν είναι σε χρήση στις δαπάνες.");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ProductStationViewModel RefreshProductFromDB(int recordId)
        {
            var data = (from d in db.ΠΡΟΙΟΝΤΑ_ΒΝΣ
                        where d.ΠΡΟΙΟΝ_ΚΩΔ == recordId
                        select new ProductStationViewModel
                        {
                            ΠΡΟΙΟΝ_ΚΩΔ = d.ΠΡΟΙΟΝ_ΚΩΔ,
                            ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ = d.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ,
                            ΠΡΟΙΟΝ_ΜΟΝΑΔΑ = d.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ,
                            ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ = d.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ,
                            ΠΡΟΙΟΝ_ΦΠΑ = d.ΠΡΟΙΟΝ_ΦΠΑ ?? 0,
                            ΒΝΣ = d.ΒΝΣ
                        }).FirstOrDefault();

            return data;
        }

        public List<ProductStationViewModel> GetProductsFromDB(int categoryId)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΠΡΟΙΟΝΤΑ_ΒΝΣ
                        where d.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ == categoryId && d.ΒΝΣ == stationId
                        orderby d.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ, d.ΠΡΟΙΟΝ_ΦΠΑ
                        select new ProductStationViewModel
                        {
                            ΠΡΟΙΟΝ_ΚΩΔ = d.ΠΡΟΙΟΝ_ΚΩΔ,
                            ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ = d.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ,
                            ΠΡΟΙΟΝ_ΜΟΝΑΔΑ = d.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ,
                            ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ = d.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ,
                            ΠΡΟΙΟΝ_ΦΠΑ = d.ΠΡΟΙΟΝ_ΦΠΑ ?? 0,
                            ΒΝΣ = d.ΒΝΣ
                        }).ToList();

            return data;
        }

        #endregion

        #region ΕΥΡΕΤΗΡΙΟ ΠΡΟΪΟΝΤΩΝ

        public ActionResult ProductList(string notify = null)
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
            populateVATValues();

            return View();
        }

        public ActionResult ProductList_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = GetProductListFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        // The list contains the station's products
        // sqlPRODUCT_LIST contains table ΠΡΟΙΟΝΤΑ_ΒΝΣ
        public List<sqlProductListViewModel> GetProductListFromDB()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.sqlPRODUCT_LIST
                        where d.ΒΝΣ == stationId
                        orderby d.ΔΑΠΑΝΗ_ΚΩΔ, d.ΚΑΤΗΓΟΡΙΑ, d.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ, d.ΜΟΝΑΔΑ
                        select new sqlProductListViewModel
                        {
                            ΠΡΟΙΟΝ_ΚΩΔ = d.ΠΡΟΙΟΝ_ΚΩΔ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ = d.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ,
                            ΜΟΝΑΔΑ = d.ΜΟΝΑΔΑ,
                            ΚΑΤΗΓΟΡΙΑ_ΚΩΔ = d.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ,
                            ΜΟΝΑΔΑ_ΚΩΔ = d.ΜΟΝΑΔΑ_ΚΩΔ,
                            ΠΡΟΙΟΝ_ΦΠΑ = d.ΠΡΟΙΟΝ_ΦΠΑ,
                            ΒΝΣ = d.ΒΝΣ
                        }).ToList();

            return data;
        }

        public ActionResult ProductsPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;

                GeneralReportParameters rp = new GeneralReportParameters();
                rp.STATION_ID = stationId;

                return View(rp);
            }
        }

        #endregion


        #region ΔΑΠΑΝΕΣ ΤΡΟΦΕΙΟΥ

        public ActionResult FilteredProduct_Read(int categoryId = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.sqlPRODUCT_SELECTOR
                        where d.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ == categoryId && d.STATION_ID == stationId
                        orderby d.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ
                        select new sqlProductSelectorViewModel
                        {
                            ΠΡΟΙΟΝ_ΚΩΔ = d.ΠΡΟΙΟΝ_ΚΩΔ,
                            ΠΡΟΙΟΝ_ΜΟΝΑΔΑ = d.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CostFoodDay(string notify = null)
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

            populateFoodCategories();
            populateProductSelector();
            return View();
        }

        #region ΠΛΕΓΜΑ ΗΜΕΡΗΣΙΟΥ ΚΟΣΤΟΥΣ ΤΡΟΦΕΙΟΥ

        public ActionResult CostFood_Read([DataSourceRequest] DataSourceRequest request, DateTime? theDate = null, int syearId = 0)
        {
            DateTime paramDate = theDate ?? DateTime.Today;

            var data = GetCostDailyFromDB(paramDate);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostFood_Create([DataSourceRequest] DataSourceRequest request, CostFeedingViewModel data, DateTime? theDate = null, int syearId = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            CostFeedingViewModel newdata = new CostFeedingViewModel();

            if (theDate == null || syearId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σχολικό έτος και ημερομηνία.");
            }
            if (!c.DateInSelectedSchoolYear((DateTime)theDate, syearId))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.");

            int CurrentSchoolYear = syearId;

            if (data != null && ModelState.IsValid)
            {
                ΔΑΠΑΝΗ_ΤΡΟΦΗ entity = new ΔΑΠΑΝΗ_ΤΡΟΦΗ()
                {
                    ΗΜΕΡΟΜΗΝΙΑ = theDate,
                    ΒΝΣ = stationId,
                    ΣΧΟΛ_ΕΤΟΣ = CurrentSchoolYear,
                    ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ,
                    ΠΡΟΙΟΝ = data.ΠΡΟΙΟΝ,
                    ΠΟΣΟΤΗΤΑ = data.ΠΟΣΟΤΗΤΑ,
                    ΤΙΜΗ_ΜΟΝΑΔΑ = data.ΤΙΜΗ_ΜΟΝΑΔΑ,
                    ΣΥΝΟΛΟ = data.ΠΟΣΟΤΗΤΑ * data.ΤΙΜΗ_ΜΟΝΑΔΑ
                };
                if (CostFoodExists(entity))
                {
                    ModelState.AddModelError("", "Έγινε απόπειρα δημιουργίας διπλοεγγραφής. Η διπλή καταχώρηση ακυρώθηκε.");
                    return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                }
                string err_msg = ValidateCostFood(data);
                if (err_msg != "")
                {
                    ModelState.AddModelError("", err_msg);
                    return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                }
                db.ΔΑΠΑΝΗ_ΤΡΟΦΗ.Add(entity);
                db.SaveChanges();
                data.ΚΩΔΙΚΟΣ = entity.ΚΩΔΙΚΟΣ;   // grid requires the new generated id
                newdata = RefreshCostDailyFromDB(data.ΚΩΔΙΚΟΣ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostFood_Update([DataSourceRequest] DataSourceRequest request, CostFeedingViewModel data, DateTime? theDate = null, int syearId = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            CostFeedingViewModel newdata = new CostFeedingViewModel();

            if (theDate == null || syearId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σχολικό έτος και ημερομηνία.");
            }
            if (!c.DateInSelectedSchoolYear((DateTime)theDate, syearId))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.");

            int CurrentSchoolYear = syearId;

            if (data != null && ModelState.IsValid)
            {
                ΔΑΠΑΝΗ_ΤΡΟΦΗ entity = db.ΔΑΠΑΝΗ_ΤΡΟΦΗ.Find(data.ΚΩΔΙΚΟΣ);

                entity.ΗΜΕΡΟΜΗΝΙΑ = theDate;
                entity.ΒΝΣ = stationId;
                entity.ΣΧΟΛ_ΕΤΟΣ = CurrentSchoolYear;
                entity.ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ;
                entity.ΠΡΟΙΟΝ = data.ΠΡΟΙΟΝ;
                entity.ΠΟΣΟΤΗΤΑ = data.ΠΟΣΟΤΗΤΑ;
                entity.ΤΙΜΗ_ΜΟΝΑΔΑ = data.ΤΙΜΗ_ΜΟΝΑΔΑ;
                entity.ΣΥΝΟΛΟ = data.ΠΟΣΟΤΗΤΑ * data.ΤΙΜΗ_ΜΟΝΑΔΑ;

                string err_msg = ValidateCostFood(data);
                if (err_msg != "")
                {
                    ModelState.AddModelError("", err_msg);
                    return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                }
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newdata = RefreshCostDailyFromDB(data.ΚΩΔΙΚΟΣ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostFood_Destroy([DataSourceRequest] DataSourceRequest request, CostFeedingViewModel data)
        {
            ΔΑΠΑΝΗ_ΤΡΟΦΗ entity = db.ΔΑΠΑΝΗ_ΤΡΟΦΗ.Find(data.ΚΩΔΙΚΟΣ);
            if (entity != null)
            {
                db.Entry(entity).State = EntityState.Deleted;
                db.ΔΑΠΑΝΗ_ΤΡΟΦΗ.Remove(entity);
                db.SaveChanges();
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public bool CostFoodExists(ΔΑΠΑΝΗ_ΤΡΟΦΗ item)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΔΑΠΑΝΗ_ΤΡΟΦΗ
                        where d.ΒΝΣ == stationId && d.ΗΜΕΡΟΜΗΝΙΑ == item.ΗΜΕΡΟΜΗΝΙΑ && d.ΠΡΟΙΟΝ == item.ΠΡΟΙΟΝ && d.ΠΟΣΟΤΗΤΑ == item.ΠΟΣΟΤΗΤΑ && d.ΤΙΜΗ_ΜΟΝΑΔΑ == item.ΤΙΜΗ_ΜΟΝΑΔΑ
                        select d).ToList();
            if (data.Count > 0)
            {
                return true;
            }
            else return false;
        }

        public string ValidateCostFood(CostFeedingViewModel item)
        {
            string errMsg = "";

            if (item.ΠΟΣΟΤΗΤΑ <= 0 || item.ΤΙΜΗ_ΜΟΝΑΔΑ <= 0)
                errMsg = "Η ποσότητα και η τιμή μονάδας πρέπει να είναι αριθμοί μεγαλύτεροι του μηδενός.";

            return errMsg;
        }

        public CostFeedingViewModel RefreshCostDailyFromDB(int recordId)
        {
            var data = (from d in db.ΔΑΠΑΝΗ_ΤΡΟΦΗ
                        where d.ΚΩΔΙΚΟΣ == recordId
                        select new CostFeedingViewModel
                        {
                            ΚΩΔΙΚΟΣ = d.ΚΩΔΙΚΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΣΧΟΛ_ΕΤΟΣ = d.ΣΧΟΛ_ΕΤΟΣ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΠΡΟΙΟΝ = d.ΠΡΟΙΟΝ,
                            ΠΟΣΟΤΗΤΑ = d.ΠΟΣΟΤΗΤΑ,
                            ΤΙΜΗ_ΜΟΝΑΔΑ = d.ΤΙΜΗ_ΜΟΝΑΔΑ,
                            ΣΥΝΟΛΟ = d.ΣΥΝΟΛΟ
                        }).FirstOrDefault();

            return (data);
        }

        public List<CostFeedingViewModel> GetCostDailyFromDB(DateTime theDate, int syearId = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΔΑΠΑΝΗ_ΤΡΟΦΗ
                        where d.ΒΝΣ == stationId && d.ΗΜΕΡΟΜΗΝΙΑ == theDate
                        orderby d.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ.ΚΑΤΗΓΟΡΙΑ, d.ΠΡΟΙΟΝ
                        select new CostFeedingViewModel
                        {
                            ΚΩΔΙΚΟΣ = d.ΚΩΔΙΚΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΣΧΟΛ_ΕΤΟΣ = d.ΣΧΟΛ_ΕΤΟΣ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΠΡΟΙΟΝ = d.ΠΡΟΙΟΝ,
                            ΠΟΣΟΤΗΤΑ = d.ΠΟΣΟΤΗΤΑ,
                            ΤΙΜΗ_ΜΟΝΑΔΑ = d.ΤΙΜΗ_ΜΟΝΑΔΑ,
                            ΣΥΝΟΛΟ = d.ΣΥΝΟΛΟ
                        }).ToList();

            return (data);
        }

        #endregion

        public ActionResult CostFoodDayPrint(string theDate)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            ExpenseReportParameters rp = new ExpenseReportParameters();
            rp.STATION_ID = stationId;
            rp.DATE_EXPENSE = theDate;

            return View(rp);
        }

        #endregion


        #region ΔΑΠΑΝΕΣ ΚΑΘΑΡΙΟΤΗΤΑΣ

        public ActionResult CostCleaningDay(string notify = null)
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

            populateCleaningCategories();
            populateProductSelector();
            return View();
        }

        #region ΠΛΕΓΜΑ ΗΜΕΡΗΣΙΟΥ ΚΟΣΤΟΥΣ ΚΑΘΑΡΙΟΤΗΤΑΣ

        public ActionResult CostCleaning_Read([DataSourceRequest] DataSourceRequest request, DateTime? theDate = null, int syearId = 0)
        {
            DateTime paramDate = theDate ?? DateTime.Today;

            var data = GetCostCleaningFromDB(paramDate);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostCleaning_Create([DataSourceRequest] DataSourceRequest request, CostCleaningViewModel data, DateTime? theDate = null, int syearId = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            CostCleaningViewModel newdata = new CostCleaningViewModel();

            if (theDate == null || syearId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σχολικό έτος και ημερομηνία.");
            }
            if (!c.DateInSelectedSchoolYear((DateTime)theDate, syearId))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.");

            int CurrentSchoolYear = syearId;

            if (data != null && ModelState.IsValid)
            {
                decimal vat_value = c.GetVATvalueFromProduct(data.ΠΡΟΙΟΝ);

                ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ entity = new ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ()
                {
                    ΗΜΕΡΟΜΗΝΙΑ = theDate,
                    ΒΝΣ = stationId,
                    ΣΧΟΛ_ΕΤΟΣ = CurrentSchoolYear,
                    ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ,
                    ΠΡΟΙΟΝ = data.ΠΡΟΙΟΝ,
                    ΠΟΣΟΤΗΤΑ = data.ΠΟΣΟΤΗΤΑ,
                    ΤΙΜΗ_ΜΟΝΑΔΑ = data.ΤΙΜΗ_ΜΟΝΑΔΑ,
                    ΣΥΝΟΛΟ = data.ΠΟΣΟΤΗΤΑ * data.ΤΙΜΗ_ΜΟΝΑΔΑ
                };
                if (CostCleaningExists(entity))
                {
                    ModelState.AddModelError("", "Έγινε απόπειρα δημιουργίας διπλοεγγραφής. Η διπλή καταχώρηση ακυρώθηκε.");
                    return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                }
                string err_msg = ValidateCostCleaning(data);
                if (err_msg != "")
                {
                    ModelState.AddModelError("", err_msg);
                    return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                }
                db.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ.Add(entity);
                db.SaveChanges();
                data.ΚΩΔΙΚΟΣ = entity.ΚΩΔΙΚΟΣ;   // grid requires the new generated id
                newdata = RefreshCostCleaningFromDB(data.ΚΩΔΙΚΟΣ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostCleaning_Update([DataSourceRequest] DataSourceRequest request, CostCleaningViewModel data, DateTime? theDate = null, int syearId = 0)
        {
            CostCleaningViewModel newdata = new CostCleaningViewModel();
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (theDate == null || syearId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σχολικό έτος και ημερομηνία.");
            }
            if (!c.DateInSelectedSchoolYear((DateTime)theDate, syearId))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.");

            int CurrentSchoolYear = syearId;

            if (data != null && ModelState.IsValid)
            {
                decimal vat_value = c.GetVATvalueFromProduct(data.ΠΡΟΙΟΝ);

                ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ entity = db.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ.Find(data.ΚΩΔΙΚΟΣ);
                entity.ΗΜΕΡΟΜΗΝΙΑ = theDate;
                entity.ΒΝΣ = stationId;
                entity.ΣΧΟΛ_ΕΤΟΣ = CurrentSchoolYear;
                entity.ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ;
                entity.ΠΡΟΙΟΝ = data.ΠΡΟΙΟΝ;
                entity.ΠΟΣΟΤΗΤΑ = data.ΠΟΣΟΤΗΤΑ;
                entity.ΤΙΜΗ_ΜΟΝΑΔΑ = data.ΤΙΜΗ_ΜΟΝΑΔΑ;
                entity.ΣΥΝΟΛΟ = data.ΠΟΣΟΤΗΤΑ * data.ΤΙΜΗ_ΜΟΝΑΔΑ;

                string err_msg = ValidateCostCleaning(data);
                if (err_msg != "")
                {
                    ModelState.AddModelError("", err_msg);
                    return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                }
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newdata = RefreshCostCleaningFromDB(data.ΚΩΔΙΚΟΣ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostCleaning_Destroy([DataSourceRequest] DataSourceRequest request, CostCleaningViewModel data)
        {
            ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ entity = db.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ.Find(data.ΚΩΔΙΚΟΣ);
            if (entity != null)
            {
                db.Entry(entity).State = EntityState.Deleted;
                db.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ.Remove(entity);
                db.SaveChanges();
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public bool CostCleaningExists(ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ item)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ
                        where d.ΒΝΣ == stationId && d.ΗΜΕΡΟΜΗΝΙΑ == item.ΗΜΕΡΟΜΗΝΙΑ && d.ΠΡΟΙΟΝ == item.ΠΡΟΙΟΝ && d.ΠΟΣΟΤΗΤΑ == item.ΠΟΣΟΤΗΤΑ && d.ΤΙΜΗ_ΜΟΝΑΔΑ == item.ΤΙΜΗ_ΜΟΝΑΔΑ
                        select d).ToList();
            if (data.Count > 0)
            {
                return true;
            }
            else return false;
        }

        public string ValidateCostCleaning(CostCleaningViewModel item)
        {
            string errMsg = "";

            if (item.ΠΟΣΟΤΗΤΑ <= 0 || item.ΤΙΜΗ_ΜΟΝΑΔΑ <= 0)
                errMsg = "Η ποσότητα και η τιμή μονάδας πρέπει να είναι αριθμοί μεγαλύτεροι του μηδενός.";

            return errMsg;
        }

        public CostCleaningViewModel RefreshCostCleaningFromDB(int recordId)
        {
            var data = (from d in db.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ
                        where d.ΚΩΔΙΚΟΣ == recordId
                        select new CostCleaningViewModel
                        {
                            ΚΩΔΙΚΟΣ = d.ΚΩΔΙΚΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΣΧΟΛ_ΕΤΟΣ = d.ΣΧΟΛ_ΕΤΟΣ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΠΡΟΙΟΝ = d.ΠΡΟΙΟΝ,
                            ΠΟΣΟΤΗΤΑ = d.ΠΟΣΟΤΗΤΑ,
                            ΤΙΜΗ_ΜΟΝΑΔΑ = d.ΤΙΜΗ_ΜΟΝΑΔΑ,
                            ΣΥΝΟΛΟ = d.ΣΥΝΟΛΟ
                        }).FirstOrDefault();

            return (data);
        }

        public List<CostCleaningViewModel> GetCostCleaningFromDB(DateTime theDate, int syearId = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ
                        where d.ΒΝΣ == stationId && d.ΗΜΕΡΟΜΗΝΙΑ == theDate
                        orderby d.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ.ΚΑΤΗΓΟΡΙΑ, d.ΠΡΟΙΟΝ
                        select new CostCleaningViewModel
                        {
                            ΚΩΔΙΚΟΣ = d.ΚΩΔΙΚΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΣΧΟΛ_ΕΤΟΣ = d.ΣΧΟΛ_ΕΤΟΣ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΠΡΟΙΟΝ = d.ΠΡΟΙΟΝ,
                            ΠΟΣΟΤΗΤΑ = d.ΠΟΣΟΤΗΤΑ,
                            ΤΙΜΗ_ΜΟΝΑΔΑ = d.ΤΙΜΗ_ΜΟΝΑΔΑ,
                            ΣΥΝΟΛΟ = d.ΣΥΝΟΛΟ
                        }).ToList();

            return (data);
        }

        #endregion

        #endregion


        #region ΓΕΝΙΚΕΣ ΔΑΠΑΝΕΣ

        public ActionResult CostOtherDay(string notify = null)
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

            populateOtherCategories();
            populateExtraCategories();
            populateProductSelector();
            return View();
        }

        #region ΠΛΕΓΜΑ #1 ΗΜΕΡΗΣΙΟΥ ΚΟΣΤΟΥΣ ΑΛΛΩΝ ΔΑΠΑΝΩΝ

        public ActionResult CostOther_Read([DataSourceRequest] DataSourceRequest request, DateTime? theDate = null, int syearId = 0)
        {
            DateTime paramDate = theDate ?? DateTime.Today;

            var data = GetCostOtherFromDB(paramDate);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostOther_Create([DataSourceRequest] DataSourceRequest request, CostOtherViewModel data, DateTime? theDate = null, int syearId = 0)
        {
            CostOtherViewModel newdata = new CostOtherViewModel();
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (theDate == null || syearId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σχολικό έτος και ημερομηνία.");
            }
            if (!c.DateInSelectedSchoolYear((DateTime)theDate, syearId))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.");

            int CurrentSchoolYear = syearId;

            if (data != null && ModelState.IsValid)
            {
                decimal vat_value = c.GetVATvalueFromProduct(data.ΠΡΟΙΟΝ);

                ΔΑΠΑΝΗ_ΑΛΛΗ entity = new ΔΑΠΑΝΗ_ΑΛΛΗ()
                {
                    ΗΜΕΡΟΜΗΝΙΑ = theDate,
                    ΒΝΣ = stationId,
                    ΣΧΟΛ_ΕΤΟΣ = CurrentSchoolYear,
                    ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ,
                    ΠΡΟΙΟΝ = data.ΠΡΟΙΟΝ,
                    ΠΟΣΟΤΗΤΑ = data.ΠΟΣΟΤΗΤΑ,
                    ΤΙΜΗ_ΜΟΝΑΔΑ = data.ΤΙΜΗ_ΜΟΝΑΔΑ,
                    ΣΥΝΟΛΟ = data.ΠΟΣΟΤΗΤΑ * data.ΤΙΜΗ_ΜΟΝΑΔΑ
                };
                if (CostOtherExists(entity))
                {
                    ModelState.AddModelError("", "Έγινε απόπειρα δημιουργίας διπλοεγγραφής. Η διπλή καταχώρηση ακυρώθηκε.");
                    return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                }
                string err_msg = ValidateCostOther(data);
                if (err_msg != "")
                {
                    ModelState.AddModelError("", err_msg);
                    return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                }
                db.ΔΑΠΑΝΗ_ΑΛΛΗ.Add(entity);
                db.SaveChanges();
                data.ΚΩΔΙΚΟΣ = entity.ΚΩΔΙΚΟΣ;   // grid requires the new generated id
                newdata = RefreshCostOtherFromDB(data.ΚΩΔΙΚΟΣ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostOther_Update([DataSourceRequest] DataSourceRequest request, CostOtherViewModel data, DateTime? theDate = null, int syearId = 0)
        {
            CostOtherViewModel newdata = new CostOtherViewModel();
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (theDate == null || syearId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σχολικό έτος και ημερομηνία.");
            }
            if (!c.DateInSelectedSchoolYear((DateTime)theDate, syearId))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.");

            int CurrentSchoolYear = syearId;

            if (data != null && ModelState.IsValid)
            {
                decimal vat_value = c.GetVATvalueFromProduct(data.ΠΡΟΙΟΝ);

                ΔΑΠΑΝΗ_ΑΛΛΗ entity = db.ΔΑΠΑΝΗ_ΑΛΛΗ.Find(data.ΚΩΔΙΚΟΣ);
                entity.ΗΜΕΡΟΜΗΝΙΑ = theDate;
                entity.ΒΝΣ = stationId;
                entity.ΣΧΟΛ_ΕΤΟΣ = CurrentSchoolYear;
                entity.ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ;
                entity.ΠΡΟΙΟΝ = data.ΠΡΟΙΟΝ;
                entity.ΠΟΣΟΤΗΤΑ = data.ΠΟΣΟΤΗΤΑ;
                entity.ΤΙΜΗ_ΜΟΝΑΔΑ = data.ΤΙΜΗ_ΜΟΝΑΔΑ;
                entity.ΣΥΝΟΛΟ = data.ΠΟΣΟΤΗΤΑ * data.ΤΙΜΗ_ΜΟΝΑΔΑ;

                string err_msg = ValidateCostOther(data);
                if (err_msg != "")
                {
                    ModelState.AddModelError("", err_msg);
                    return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                }
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newdata = RefreshCostOtherFromDB(data.ΚΩΔΙΚΟΣ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostOther_Destroy([DataSourceRequest] DataSourceRequest request, CostOtherViewModel data)
        {
            ΔΑΠΑΝΗ_ΑΛΛΗ entity = db.ΔΑΠΑΝΗ_ΑΛΛΗ.Find(data.ΚΩΔΙΚΟΣ);
            if (entity != null)
            {
                db.Entry(entity).State = EntityState.Deleted;
                db.ΔΑΠΑΝΗ_ΑΛΛΗ.Remove(entity);
                db.SaveChanges();
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public bool CostOtherExists(ΔΑΠΑΝΗ_ΑΛΛΗ item)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΔΑΠΑΝΗ_ΑΛΛΗ
                        where d.ΒΝΣ == stationId && d.ΗΜΕΡΟΜΗΝΙΑ == item.ΗΜΕΡΟΜΗΝΙΑ && d.ΠΡΟΙΟΝ == item.ΠΡΟΙΟΝ && d.ΠΟΣΟΤΗΤΑ == item.ΠΟΣΟΤΗΤΑ && d.ΤΙΜΗ_ΜΟΝΑΔΑ == item.ΤΙΜΗ_ΜΟΝΑΔΑ
                        select d).ToList();
            if (data.Count > 0)
            {
                return true;
            }
            else return false;
        }

        public string ValidateCostOther(CostOtherViewModel item)
        {
            string errMsg = "";

            if (item.ΠΟΣΟΤΗΤΑ <= 0 || item.ΤΙΜΗ_ΜΟΝΑΔΑ <= 0)
                errMsg = "Η ποσότητα και η τιμή μονάδας πρέπει να είναι αριθμοί μεγαλύτεροι του μηδενός.";

            return errMsg;
        }

        public CostOtherViewModel RefreshCostOtherFromDB(int recordId)
        {
            var data = (from d in db.ΔΑΠΑΝΗ_ΑΛΛΗ
                        where d.ΚΩΔΙΚΟΣ == recordId
                        select new CostOtherViewModel
                        {
                            ΚΩΔΙΚΟΣ = d.ΚΩΔΙΚΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΣΧΟΛ_ΕΤΟΣ = d.ΣΧΟΛ_ΕΤΟΣ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΠΡΟΙΟΝ = d.ΠΡΟΙΟΝ,
                            ΠΟΣΟΤΗΤΑ = d.ΠΟΣΟΤΗΤΑ,
                            ΤΙΜΗ_ΜΟΝΑΔΑ = d.ΤΙΜΗ_ΜΟΝΑΔΑ,
                            ΣΥΝΟΛΟ = d.ΣΥΝΟΛΟ
                        }).FirstOrDefault();

            return (data);
        }

        public List<CostOtherViewModel> GetCostOtherFromDB(DateTime theDate, int syearId = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΔΑΠΑΝΗ_ΑΛΛΗ
                        where d.ΒΝΣ == stationId && d.ΗΜΕΡΟΜΗΝΙΑ == theDate
                        orderby d.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ.ΚΑΤΗΓΟΡΙΑ, d.ΠΡΟΙΟΝ
                        select new CostOtherViewModel
                        {
                            ΚΩΔΙΚΟΣ = d.ΚΩΔΙΚΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΣΧΟΛ_ΕΤΟΣ = d.ΣΧΟΛ_ΕΤΟΣ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΠΡΟΙΟΝ = d.ΠΡΟΙΟΝ,
                            ΠΟΣΟΤΗΤΑ = d.ΠΟΣΟΤΗΤΑ,
                            ΤΙΜΗ_ΜΟΝΑΔΑ = d.ΤΙΜΗ_ΜΟΝΑΔΑ,
                            ΣΥΝΟΛΟ = d.ΣΥΝΟΛΟ
                        }).ToList();

            return (data);
        }

        #endregion

        #region ΠΛΕΓΜΑ #2 ΗΜΕΡΗΣΙΟΥ ΚΟΣΤΟΥΣ ΓΕΝΙΚΩΝ ΔΑΠΑΝΩΝ

        public ActionResult CostGeneral_Read([DataSourceRequest] DataSourceRequest request, DateTime? theDate = null, int syearId = 0)
        {
            DateTime paramDate = theDate ?? DateTime.Today;

            var data = GetCostGeneralFromDB(paramDate);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostGeneral_Create([DataSourceRequest] DataSourceRequest request, CostGeneralViewModel data, DateTime? theDate = null, int syearId = 0)
        {
            CostGeneralViewModel newdata = new CostGeneralViewModel();
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (theDate == null || syearId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σχολικό έτος και ημερομηνία.");
            }
            if (!c.DateInSelectedSchoolYear((DateTime)theDate, syearId))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.");

            int CurrentSchoolYear = syearId;

            if (data != null && ModelState.IsValid)
            {
                ΔΑΠΑΝΗ_ΓΕΝΙΚΗ entity = new ΔΑΠΑΝΗ_ΓΕΝΙΚΗ()
                {
                    ΗΜΕΡΟΜΗΝΙΑ = theDate,
                    ΒΝΣ = stationId,
                    ΣΧΟΛ_ΕΤΟΣ = CurrentSchoolYear,
                    ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ,
                    ΠΕΡΙΓΡΑΦΗ = data.ΠΕΡΙΓΡΑΦΗ,
                    ΣΥΝΟΛΟ = data.ΣΥΝΟΛΟ
                };
                if (CostGeneralExists(entity))
                {
                    ModelState.AddModelError("", "Έγινε απόπειρα δημιουργίας διπλοεγγραφής. Η διπλή καταχώρηση ακυρώθηκε.");
                    return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                }
                string err_msg = ValidateCostGeneral(data);
                if (err_msg != "")
                {
                    ModelState.AddModelError("", err_msg);
                    return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                }
                db.ΔΑΠΑΝΗ_ΓΕΝΙΚΗ.Add(entity);
                db.SaveChanges();
                data.ΚΩΔΙΚΟΣ = entity.ΚΩΔΙΚΟΣ;   // grid requires the new generated id
                newdata = RefreshCostGeneralFromDB(data.ΚΩΔΙΚΟΣ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostGeneral_Update([DataSourceRequest] DataSourceRequest request, CostGeneralViewModel data, DateTime? theDate = null, int syearId = 0)
        {
            CostGeneralViewModel newdata = new CostGeneralViewModel();
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (theDate == null || syearId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σχολικό έτος και ημερομηνία.");
            }
            if (!c.DateInSelectedSchoolYear((DateTime)theDate, syearId))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.");

            int CurrentSchoolYear = syearId;

            if (data != null && ModelState.IsValid)
            {
                ΔΑΠΑΝΗ_ΓΕΝΙΚΗ entity = db.ΔΑΠΑΝΗ_ΓΕΝΙΚΗ.Find(data.ΚΩΔΙΚΟΣ);

                entity.ΗΜΕΡΟΜΗΝΙΑ = theDate;
                entity.ΒΝΣ = stationId;
                entity.ΣΧΟΛ_ΕΤΟΣ = CurrentSchoolYear;
                entity.ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ;
                entity.ΠΕΡΙΓΡΑΦΗ = data.ΠΕΡΙΓΡΑΦΗ;
                entity.ΣΥΝΟΛΟ = data.ΣΥΝΟΛΟ;

                string err_msg = ValidateCostGeneral(data);
                if (err_msg != "")
                {
                    ModelState.AddModelError("", err_msg);
                    return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                }
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                newdata = RefreshCostGeneralFromDB(data.ΚΩΔΙΚΟΣ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostGeneral_Destroy([DataSourceRequest] DataSourceRequest request, CostGeneralViewModel data)
        {
            ΔΑΠΑΝΗ_ΓΕΝΙΚΗ entity = db.ΔΑΠΑΝΗ_ΓΕΝΙΚΗ.Find(data.ΚΩΔΙΚΟΣ);
            if (entity != null)
            {
                db.Entry(entity).State = EntityState.Deleted;
                db.ΔΑΠΑΝΗ_ΓΕΝΙΚΗ.Remove(entity);
                db.SaveChanges();
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public bool CostGeneralExists(ΔΑΠΑΝΗ_ΓΕΝΙΚΗ item)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΔΑΠΑΝΗ_ΓΕΝΙΚΗ
                        where d.ΒΝΣ == stationId && d.ΗΜΕΡΟΜΗΝΙΑ == item.ΗΜΕΡΟΜΗΝΙΑ && d.ΠΕΡΙΓΡΑΦΗ == item.ΠΕΡΙΓΡΑΦΗ && d.ΣΥΝΟΛΟ == item.ΣΥΝΟΛΟ
                        select d).ToList();
            if (data.Count > 0)
            {
                return true;
            }
            else return false;
        }

        public string ValidateCostGeneral(CostGeneralViewModel item)
        {
            string errMsg = "";

            if (item.ΣΥΝΟΛΟ <= 0)
                errMsg = "Το κόσοτς πρέπει να είναι αριθμός μεγαλύτερος του μηδενός.";

            return errMsg;
        }

        public CostGeneralViewModel RefreshCostGeneralFromDB(int recordId)
        {
            var data = (from d in db.ΔΑΠΑΝΗ_ΓΕΝΙΚΗ
                        where d.ΚΩΔΙΚΟΣ == recordId
                        select new CostGeneralViewModel
                        {
                            ΚΩΔΙΚΟΣ = d.ΚΩΔΙΚΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΣΧΟΛ_ΕΤΟΣ = d.ΣΧΟΛ_ΕΤΟΣ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΠΕΡΙΓΡΑΦΗ = d.ΠΕΡΙΓΡΑΦΗ,
                            ΣΥΝΟΛΟ = d.ΣΥΝΟΛΟ
                        }).FirstOrDefault();

            return (data);
        }

        public List<CostGeneralViewModel> GetCostGeneralFromDB(DateTime theDate, int syearId = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΔΑΠΑΝΗ_ΓΕΝΙΚΗ
                        where d.ΒΝΣ == stationId && d.ΗΜΕΡΟΜΗΝΙΑ == theDate
                        orderby d.ΕΞΤΡΑ_ΚΑΤΗΓΟΡΙΑ.ΚΑΤΗΓΟΡΙΑ, d.ΠΕΡΙΓΡΑΦΗ
                        select new CostGeneralViewModel
                        {
                            ΚΩΔΙΚΟΣ = d.ΚΩΔΙΚΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΣΧΟΛ_ΕΤΟΣ = d.ΣΧΟΛ_ΕΤΟΣ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΠΕΡΙΓΡΑΦΗ = d.ΠΕΡΙΓΡΑΦΗ,
                            ΣΥΝΟΛΟ = d.ΣΥΝΟΛΟ
                        }).ToList();

            return (data);
        }

        #endregion

        #endregion


        #region ΕΥΡΕΤΗΡΙΑ ΔΑΠΑΝΩΝ

        // ΔΑΠΑΝΕΣ ΤΡΟΦΕΙΟΥ
        public ActionResult SearchCostFood(string notify = null)
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

            return View();
        }

        public ActionResult CostFoodSearch_Read([DataSourceRequest] DataSourceRequest request, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {

            var data = GetCostFoodSearchFromDB(theDate1, theDate2);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<sqlCostFoodViewModel> GetCostFoodSearchFromDB(DateTime? theDate1, DateTime? theDate2)
        {
            List<sqlCostFoodViewModel> data = new List<sqlCostFoodViewModel>();
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (theDate1 != null && theDate2 != null)
            {
                data = (from d in db.sqlΔΑΠΑΝΗ_ΤΡΟΦΕΙΟ
                        where d.ΒΝΣ == stationId && (d.ΗΜΕΡΟΜΗΝΙΑ >= theDate1 && d.ΗΜΕΡΟΜΗΝΙΑ <= theDate2)
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ, d.ΚΑΤΗΓΟΡΙΑ, d.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ
                        select new sqlCostFoodViewModel
                        {
                            ΚΩΔΙΚΟΣ = d.ΚΩΔΙΚΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΠΡΟΙΟΝ_ΜΟΝΑΔΑ = d.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ,
                            ΠΟΣΟΤΗΤΑ = d.ΠΟΣΟΤΗΤΑ,
                            ΤΙΜΗ_ΜΟΝΑΔΑ = d.ΤΙΜΗ_ΜΟΝΑΔΑ,
                            ΣΥΝΟΛΟ = d.ΣΥΝΟΛΟ
                        }).ToList();
            }
            return (data);
        }

        // ΔΑΠΑΝΕΣ ΚΑΘΑΡΙΟΤΗΤΑΣ
        public ActionResult SearchCostCleaning(string notify = null)
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

            return View();
        }

        public ActionResult CostCleaningSearch_Read([DataSourceRequest] DataSourceRequest request, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {

            var data = GetCostCleaningSearchFromDB(theDate1, theDate2);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<sqlCostCleaningViewModel> GetCostCleaningSearchFromDB(DateTime? theDate1, DateTime? theDate2)
        {
            List<sqlCostCleaningViewModel> data = new List<sqlCostCleaningViewModel>();
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (theDate1 != null && theDate2 != null)
            {
                data = (from d in db.sqlΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ
                        where d.ΒΝΣ == stationId && (d.ΗΜΕΡΟΜΗΝΙΑ >= theDate1 && d.ΗΜΕΡΟΜΗΝΙΑ <= theDate2)
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ, d.ΚΑΤΗΓΟΡΙΑ, d.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ
                        select new sqlCostCleaningViewModel
                        {
                            ΚΩΔΙΚΟΣ = d.ΚΩΔΙΚΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΠΡΟΙΟΝ_ΜΟΝΑΔΑ = d.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ,
                            ΠΟΣΟΤΗΤΑ = d.ΠΟΣΟΤΗΤΑ,
                            ΤΙΜΗ_ΜΟΝΑΔΑ = d.ΤΙΜΗ_ΜΟΝΑΔΑ,
                            ΣΥΝΟΛΟ = d.ΣΥΝΟΛΟ
                        }).ToList();
            }
            return (data);
        }

        // ΔΑΠΑΝΕΣ ΑΛΛΕΣ ΚΑΙ ΓΕΝΙΚΕΣ (2 ΠΛΕΓΜΑΤΑ)
        public ActionResult SearchCostOther(string notify = null)
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

            return View();
        }

        public ActionResult CostOtherSearch_Read([DataSourceRequest] DataSourceRequest request, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {

            var data = GetCostOtherSearchFromDB(theDate1, theDate2);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public ActionResult CostGeneralSearch_Read([DataSourceRequest] DataSourceRequest request, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {

            var data = GetCostGeneralSearchFromDB(theDate1, theDate2);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<sqlCostOtherViewModel> GetCostOtherSearchFromDB(DateTime? theDate1, DateTime? theDate2)
        {
            List<sqlCostOtherViewModel> data = new List<sqlCostOtherViewModel>();
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (theDate1 != null && theDate2 != null)
            {
                data = (from d in db.sqlΔΑΠΑΝΗ_ΑΛΛΗ
                        where d.ΒΝΣ == stationId && (d.ΗΜΕΡΟΜΗΝΙΑ >= theDate1 && d.ΗΜΕΡΟΜΗΝΙΑ <= theDate2)
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ, d.ΚΑΤΗΓΟΡΙΑ, d.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ
                        select new sqlCostOtherViewModel
                        {
                            ΚΩΔΙΚΟΣ = d.ΚΩΔΙΚΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΠΡΟΙΟΝ_ΜΟΝΑΔΑ = d.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ,
                            ΠΟΣΟΤΗΤΑ = d.ΠΟΣΟΤΗΤΑ,
                            ΤΙΜΗ_ΜΟΝΑΔΑ = d.ΤΙΜΗ_ΜΟΝΑΔΑ,
                            ΣΥΝΟΛΟ = d.ΣΥΝΟΛΟ
                        }).ToList();
            }
            return (data);
        }

        public List<sqlCostGeneralViewModel> GetCostGeneralSearchFromDB(DateTime? theDate1, DateTime? theDate2)
        {
            List<sqlCostGeneralViewModel> data = new List<sqlCostGeneralViewModel>();
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (theDate1 != null && theDate2 != null)
            {
                data = (from d in db.sqlΔΑΠΑΝΗ_ΓΕΝΙΚΗ
                        where d.ΒΝΣ == stationId && (d.ΗΜΕΡΟΜΗΝΙΑ >= theDate1 && d.ΗΜΕΡΟΜΗΝΙΑ <= theDate2)
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ, d.ΚΑΤΗΓΟΡΙΑ, d.ΠΕΡΙΓΡΑΦΗ
                        select new sqlCostGeneralViewModel
                        {
                            ΚΩΔΙΚΟΣ = d.ΚΩΔΙΚΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΠΕΡΙΓΡΑΦΗ = d.ΠΕΡΙΓΡΑΦΗ,
                            ΠΟΣΟΤΗΤΑ = d.ΠΟΣΟΤΗΤΑ,
                            ΤΙΜΗ_ΜΟΝΑΔΑ = d.ΤΙΜΗ_ΜΟΝΑΔΑ,
                            ΣΥΝΟΛΟ = d.ΣΥΝΟΛΟ
                        }).ToList();
            }
            return (data);
        }

        // ΔΑΠΑΝΕΣ ΓΕΝΙΚΕΣ ΚΑΙ ΕΚΤΑΚΤΕΣ ΕΝΟΠΟΙΗΜΕΝΕΣ
        public ActionResult SearchCostAllOther(string notify = null)
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

            return View();
        }

        public ActionResult CostAllOtherSearch_Read([DataSourceRequest] DataSourceRequest request, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {

            var data = GetCostAllOtherSearchFromDB(theDate1, theDate2);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<sqlCostAllOtherViewModel> GetCostAllOtherSearchFromDB(DateTime? theDate1, DateTime? theDate2)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            List<sqlCostAllOtherViewModel> data = new List<sqlCostAllOtherViewModel>();

            if (theDate1 != null && theDate2 != null)
            {
                data = (from d in db.sqlΔΑΠΑΝΗ_ΓΕΝΙΚΗ_ΟΛΕΣ
                        where d.ΒΝΣ == stationId && (d.ΗΜΕΡΟΜΗΝΙΑ >= theDate1 && d.ΗΜΕΡΟΜΗΝΙΑ <= theDate2)
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ, d.ΚΑΤΗΓΟΡΙΑ, d.ΠΕΡΙΓΡΑΦΗ
                        select new sqlCostAllOtherViewModel
                        {
                            ROWID = d.ROWID,
                            ΒΝΣ = d.ΒΝΣ,
                            ΣΧΟΛ_ΕΤΟΣ = d.ΣΧΟΛ_ΕΤΟΣ,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ,
                            ΠΕΡΙΓΡΑΦΗ = d.ΠΕΡΙΓΡΑΦΗ,
                            ΕΙΔΟΣ = d.ΕΙΔΟΣ,
                            ΣΥΝΟΛΟ = d.ΣΥΝΟΛΟ
                        }).ToList();
            }
            return (data);
        }


        #region ΕΚΤΥΠΩΣΕΙΣ ΕΥΡΕΤΗΡΙΩΝ ΔΑΠΑΝΩΝ

        public ActionResult CostFoodPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;

                GeneralReportParameters rp = new GeneralReportParameters();
                rp.STATION_ID = stationId;

                return View(rp);
            }
        }

        public ActionResult CostCleaningPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;

                GeneralReportParameters rp = new GeneralReportParameters();
                rp.STATION_ID = stationId;

                return View(rp);
            }
        }

        public ActionResult CostOtherPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;

                GeneralReportParameters rp = new GeneralReportParameters();
                rp.STATION_ID = stationId;

                return View(rp);
            }
        }

        public ActionResult CostMiscPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;

                GeneralReportParameters rp = new GeneralReportParameters();
                rp.STATION_ID = stationId;

                return View(rp);
            }
        }

        public ActionResult CostAllOtherPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;

                GeneralReportParameters rp = new GeneralReportParameters();
                rp.STATION_ID = stationId;

                return View(rp);
            }
        }

        #endregion

        #endregion

        #region ΣΥΓΚΕΝΤΡΩΤΙΚΑ ΣΤΟΙΧΕΙΑ ΔΑΠΑΝΩΝ (ΚΟΣΤΟΛΟΓΙΑ)

        // ΤΡΟΦΕΙΟ
        public ActionResult SumPersonsFoodDay(string notify = null)
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

            return View();
        }

        public ActionResult SumPersonsFood_Read([DataSourceRequest] DataSourceRequest request, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {

            var data = GetSumPersonsFoodFromDB(theDate1, theDate2);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<SumPersonsTrofeioViewModel> GetSumPersonsFoodFromDB(DateTime? theDate1, DateTime? theDate2)
        {
            List<SumPersonsTrofeioViewModel> data = new List<SumPersonsTrofeioViewModel>();
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (theDate1 != null && theDate2 != null)
            {
                data = (from d in db.sqlΣΥΝΟΛΟ_ΑΤΟΜΑ_ΤΡΟΦΕΙΟ
                        where d.STATION_ID == stationId && (d.ΗΜΕΡΟΜΗΝΙΑ >= theDate1 && d.ΗΜΕΡΟΜΗΝΙΑ <= theDate2)
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ
                        select new SumPersonsTrofeioViewModel
                        {
                            ROWID = d.ROWID,
                            SCHOOLYEARID = d.SCHOOLYEARID,
                            STATION_ID = d.STATION_ID,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΠΑΙΔΙΑ = d.ΠΑΙΔΙΑ,
                            ΠΡΟΣΩΠΙΚΟ = d.ΠΡΟΣΩΠΙΚΟ,
                            ΑΤΟΜΑ = d.ΑΤΟΜΑ,
                            ΚΟΣΤΟΣ_ΗΜΕΡΑ = d.ΚΟΣΤΟΣ_ΗΜΕΡΑ,
                            ΔΑΠΑΝΗ_ΗΜΕΡΑ = d.ΔΑΠΑΝΗ_ΗΜΕΡΑ,
                            ΥΠΟΛΟΙΠΟ = d.ΥΠΟΛΟΙΠΟ
                        }).ToList();
            }
            return (data);
        }

        // ΚΑΘΑΡΙΟΤΗΤΑ
        public ActionResult SumCleaningDay(string notify = null)
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

            return View();
        }

        public ActionResult SumCleaningDay_Read([DataSourceRequest] DataSourceRequest request, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {

            var data = GetSumCleaningDayFromDB(theDate1, theDate2);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<SumCleaningDayViewModel> GetSumCleaningDayFromDB(DateTime? theDate1, DateTime? theDate2)
        {
            List<SumCleaningDayViewModel> data = new List<SumCleaningDayViewModel>();
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (theDate1 != null && theDate2 != null)
            {
                data = (from d in db.sqlΣΥΝΟΛΟ_ΚΑΘΑΡΙΟΤΗΤΑ_ΗΜΕΡΑ
                        where d.ΒΝΣ == stationId && (d.ΗΜΕΡΟΜΗΝΙΑ >= theDate1 && d.ΗΜΕΡΟΜΗΝΙΑ <= theDate2)
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ
                        select new SumCleaningDayViewModel
                        {
                            ROWID = d.ROWID,
                            ΣΧΟΛ_ΕΤΟΣ = d.ΣΧΟΛ_ΕΤΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΜΗΝΑΣ = d.ΜΗΝΑΣ,
                            TOTAL_DAY = d.TOTAL_DAY
                        }).ToList();
            }
            return (data);
        }

        // ΓΕΝΙΚΕΣ ΚΑΙ ΕΚΤΑΚΤΕΣ ΔΑΠΑΝΕΣ
        public ActionResult SumOtherExpenseDay(string notify = null)
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

            return View();
        }

        public ActionResult SumOtherExpenseDay_Read([DataSourceRequest] DataSourceRequest request, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {

            var data = GetSumOtherExpenseDayFromDB(theDate1, theDate2);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public ActionResult SumExtraExpenseDay_Read([DataSourceRequest] DataSourceRequest request, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {

            var data = GetSumExtraExpenseDayFromDB(theDate1, theDate2);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<SumOtherExpenseDayViewModel> GetSumOtherExpenseDayFromDB(DateTime? theDate1, DateTime? theDate2)
        {
            List<SumOtherExpenseDayViewModel> data = new List<SumOtherExpenseDayViewModel>();
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (theDate1 != null && theDate2 != null)
            {
                data = (from d in db.sqlΣΥΝΟΛΟ_ΓΕΝΙΚΗ_ΗΜΕΡΑ
                        where d.ΒΝΣ == stationId && (d.ΗΜΕΡΟΜΗΝΙΑ >= theDate1 && d.ΗΜΕΡΟΜΗΝΙΑ <= theDate2)
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ
                        select new SumOtherExpenseDayViewModel
                        {
                            ROWID = d.ROWID,
                            ΣΧΟΛ_ΕΤΟΣ = d.ΣΧΟΛ_ΕΤΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΜΗΝΑΣ = d.ΜΗΝΑΣ,
                            TOTAL_DAY = d.TOTAL_DAY
                        }).ToList();
            }
            return (data);
        }

        public List<SumExtraExpenseDayViewModel> GetSumExtraExpenseDayFromDB(DateTime? theDate1, DateTime? theDate2)
        {
            List<SumExtraExpenseDayViewModel> data = new List<SumExtraExpenseDayViewModel>();
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            if (theDate1 != null && theDate2 != null)
            {
                data = (from d in db.sqlΣΥΝΟΛΟ_ΕΞΤΡΑ_ΗΜΕΡΑ
                        where d.ΒΝΣ == stationId && (d.ΗΜΕΡΟΜΗΝΙΑ >= theDate1 && d.ΗΜΕΡΟΜΗΝΙΑ <= theDate2)
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ
                        select new SumExtraExpenseDayViewModel
                        {
                            ROWID = d.ROWID,
                            ΣΧΟΛ_ΕΤΟΣ = d.ΣΧΟΛ_ΕΤΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΜΗΝΑΣ = d.ΜΗΝΑΣ,
                            TOTAL_DAY = d.TOTAL_DAY
                        }).ToList();
            }
            return (data);
        }

        // ΕΝΟΠΟΙΗΜΕΝΕΣ ΓΕΝΙΚΕΣ ΚΑΙ ΕΚΤΑΚΤΕΣ
        public ActionResult SumAllOtherExpenseDay(string notify = null)
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

            return View();
        }

        public ActionResult SumAllOtherDay_Read([DataSourceRequest] DataSourceRequest request, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {

            var data = GetSumAllOtherExpenseDayFromDB(theDate1, theDate2);

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<SumAllOtherExpenseDayViewModel> GetSumAllOtherExpenseDayFromDB(DateTime? theDate1, DateTime? theDate2)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            List<SumAllOtherExpenseDayViewModel> data = new List<SumAllOtherExpenseDayViewModel>();

            if (theDate1 != null && theDate2 != null)
            {
                data = (from d in db.sqlΣΥΝΟΛΟ_ΑΛΛΕΣ_ΗΜΕΡΑ
                        where d.ΒΝΣ == stationId && (d.ΗΜΕΡΟΜΗΝΙΑ >= theDate1 && d.ΗΜΕΡΟΜΗΝΙΑ <= theDate2)
                        orderby d.ΗΜΕΡΟΜΗΝΙΑ
                        select new SumAllOtherExpenseDayViewModel
                        {
                            ROWID = d.ROWID,
                            ΣΧΟΛ_ΕΤΟΣ = d.ΣΧΟΛ_ΕΤΟΣ,
                            ΒΝΣ = d.ΒΝΣ,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΗΜΕΡΟΜΗΝΙΑ = d.ΗΜΕΡΟΜΗΝΙΑ,
                            ΜΗΝΑΣ = d.ΜΗΝΑΣ,
                            ΕΙΔΟΣ = d.ΕΙΔΟΣ,
                            TOTAL_DAY = d.TOTAL_DAY ?? 0.0M
                        }).ToList();
            }
            return (data);
        }


        #region ΕΚΤΥΠΩΣΕΙΣ

        public ActionResult SumCostFoodPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;

                GeneralReportParameters rp = new GeneralReportParameters();
                rp.STATION_ID = stationId;

                return View(rp);
            }
        }

        public ActionResult SumCostCleaningPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;

                GeneralReportParameters rp = new GeneralReportParameters();
                rp.STATION_ID = stationId;

                return View(rp);
            }
        }

        public ActionResult SumCostOtherPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;

                GeneralReportParameters rp = new GeneralReportParameters();
                rp.STATION_ID = stationId;

                return View(rp);
            }
        }

        public ActionResult SumCostMiscPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;

                GeneralReportParameters rp = new GeneralReportParameters();
                rp.STATION_ID = stationId;

                return View(rp);
            }
        }

        public ActionResult SumCostAllOtherPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;

                GeneralReportParameters rp = new GeneralReportParameters();
                rp.STATION_ID = stationId;

                return View(rp);
            }
        }
        #endregion

        #endregion


        #region ΜΗΝΑΙΑ ΙΣΟΖΥΓΙΑ ΔΑΠΑΝΩΝ

        public ActionResult BalanceMonth(string notify = null)
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
            return View();
        }

        public PartialViewResult LoadBalanceMonth(int schoolyearId = 0, int monthid = 0)
        {
            var balanceModel = new expBalanceMonthViewModel();

            //Set default balance record
            balanceModel = GetBalanceRecord(schoolyearId, monthid);

            return PartialView("BalanceMonthPartial", balanceModel);
        }

        public expBalanceMonthViewModel GetBalanceRecord(int schoolyearId = 0, int monthid = 0)
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.expSTATION_BALANCE_MONTH
                        where d.ΒΝΣ == stationId && d.ΣΧΟΛ_ΕΤΟΣ == schoolyearId && d.ΜΗΝΑΣ_ΑΡΙΘΜΟΣ == monthid
                        select new expBalanceMonthViewModel
                        {
                            ROWID = d.ROWID,
                            ΒΝΣ = d.ΒΝΣ,
                            ΣΧΟΛ_ΕΤΟΣ = d.ΣΧΟΛ_ΕΤΟΣ,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΜΗΝΑΣ_ΑΡΙΘΜΟΣ = d.ΜΗΝΑΣ_ΑΡΙΘΜΟΣ,
                            ΜΗΝΑΣ = d.ΜΗΝΑΣ,
                            ΠΑΙΔΙΑ_ΔΥΝΑΜΗ = d.ΠΑΙΔΙΑ_ΔΥΝΑΜΗ ?? 0,
                            ΠΡΟΣΩΠΙΚΟ_ΔΥΝΑΜΗ = d.ΠΡΟΣΩΠΙΚΟ_ΔΥΝΑΜΗ ?? 0,
                            ΠΛΗΘΟΣ_ΑΤΟΜΑ = d.ΠΛΗΘΟΣ_ΑΤΟΜΑ ?? 0,
                            ΚΟΣΤΟΣ_ΤΡΟΦΕΙΟ = d.ΚΟΣΤΟΣ_ΤΡΟΦΕΙΟ ?? 0.0M,
                            ΚΟΣΤΟΣ_ΚΑΘΑΡΙΟΤΗΤΑ = d.ΚΟΣΤΟΣ_ΚΑΘΑΡΙΟΤΗΤΑ ?? 0.0M,
                            ΚΟΣΤΟΣ_ΓΕΝΙΚΕΣ = d.ΚΟΣΤΟΣ_ΓΕΝΙΚΕΣ ?? 0.0M,
                            ΚΟΣΤΟΣ_ΣΥΝΟΛΟ = d.ΚΟΣΤΟΣ_ΣΥΝΟΛΟ ?? 0.0M,
                            ΔΑΠΑΝΗ_ΤΡΟΦΕΙΟ = d.ΔΑΠΑΝΗ_ΤΡΟΦΕΙΟ ?? 0.0M,
                            ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ = d.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ ?? 0.0M,
                            ΔΑΠΑΝΗ_ΓΕΝΙΚΕΣ = d.ΔΑΠΑΝΗ_ΓΕΝΙΚΕΣ ?? 0.0M,
                            ΔΑΠΑΝΗ_ΣΥΝΟΛΟ = d.ΔΑΠΑΝΗ_ΣΥΝΟΛΟ ?? 0.0M,
                            ΥΠΟΛΟΙΠΟ_ΤΡΟΦΕΙΟ = d.ΥΠΟΛΟΙΠΟ_ΤΡΟΦΕΙΟ ?? 0.0M,
                            ΥΠΟΛΟΙΠΟ_ΚΑΘΑΡΙΟΤΗΤΑ = d.ΥΠΟΛΟΙΠΟ_ΚΑΘΑΡΙΟΤΗΤΑ ?? 0.0M,
                            ΥΠΟΛΟΙΠΟ_ΓΕΝΙΚΕΣ = d.ΥΠΟΛΟΙΠΟ_ΓΕΝΙΚΕΣ ?? 0.0M,
                            ΥΠΟΛΟΙΠΟ_ΣΥΝΟΛΟ = d.ΥΠΟΛΟΙΠΟ_ΣΥΝΟΛΟ ?? 0.0M
                        }).FirstOrDefault();

            if (data == null) data = new expBalanceMonthViewModel();

            return (data);
        }

        public ActionResult BalanceMonthPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                loggedStation = GetLoginStation();
                stationId = (int)loggedStation.STATION_ID;

                GeneralReportParameters rp = new GeneralReportParameters();
                rp.STATION_ID = stationId;

                return View(rp);
            }
        }

        #endregion

        #endregion ΔΑΠΑΝΕΣ ΤΡΟΦΕΙΟΥ, ΚΑΘΑΡΙΟΤΗΤΑΣ ΚΑΙ ΓΕΝΙΚΩΝ


        #region --- ΣΤΑΤΙΣΤΙΚΕΣ ΕΚΘΕΣΕΙΣ ---

        #region ΕΠΙΛΟΓΕΑΣ ΑΝΑΛΥΤΙΚΩΝ ΣΤΟΙΧΕΙΩΝ

        public ActionResult zReportsDetailList()
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
            }

            List<SysReportViewModel> reports = GetReportsDetailFromDB();
            return View(reports);
        }

        public ActionResult ReportSelectorDetail(int reportId = 0)
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
            }
            // logic of report selection here
            if (reportId == 1)
            {
                return RedirectToAction("zReportDemoPrint", "Station");
            }
            else if (reportId == 2)
            {
                return RedirectToAction("zReportDemoPrint", "Station");
            }
            else if (reportId == 3)
            {
                return RedirectToAction("zReportDemoPrint", "Station");
            }
            else if (reportId == 4)
            {
                return RedirectToAction("zReportDemoPrint", "Station");
            }
            else if (reportId == 5)
            {
                return RedirectToAction("zReportDemoPrint", "Station");
            }
            else if (reportId == 6)
            {
                return RedirectToAction("zReportDemoPrint", "Station");
            }
            else if (reportId == 7)
            {
                return RedirectToAction("zChildrenNamesClassesPrint", "Station");
            }
            else if (reportId == 10)
            {
                return RedirectToAction("zPersonnelStation1Print", "Station");
            }
            else if (reportId == 11)
            {
                return RedirectToAction("zPersonnelStation2Print", "Station");
            }
            else
            {
                return RedirectToAction("zReportDemoPrint", "Station");
            }
        }

        public ActionResult ReportsDetail_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = GetReportsDetailFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<SysReportViewModel> GetReportsDetailFromDB()
        {

            var data = (from d in db.ΣΥΣ_ΕΝΤΥΠΑ
                        where d.DOC_CLASS == "DETAIL"
                        orderby d.DOC_NAME
                        select new SysReportViewModel
                        {
                            DOC_ID = d.DOC_ID,
                            DOC_NAME = d.DOC_NAME,
                            DOC_DESCRIPTION = d.DOC_DESCRIPTION,
                            DOC_CLASS = d.DOC_CLASS
                        }).ToList();

            return data;
        }

        #endregion

        #region ΕΠΙΛΟΓΕΑΣ ΣΥΓΚΕΝΤΡΩΤΙΚΩΝ ΣΤΟΙΧΕΙΩΝ

        public ActionResult zReportsSummaryList()
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
            }

            List<SysReportViewModel> reports = GetReportsSummaryFromDB();
            return View(reports);
        }

        public ActionResult ReportSelectorSummary(int reportId = 0)
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
            }
            // logic of report selection here
            if (reportId == 2)
            {
                return RedirectToAction("zReportDemoPrint", "Station");
            }
            else if (reportId == 3)
            {
                return RedirectToAction("zSumChildrenClassesPrint", "Station");
            }
            else if (reportId == 4)
            {
                return RedirectToAction("zStationChildrenClassesPrint", "Station");
            }
            else if (reportId == 5)
            {
                return RedirectToAction("zSumChildrenPeriferiakesPrint", "Station");
            }
            else if (reportId == 6)
            {
                return RedirectToAction("zSumChildrenGenderPrint", "Station");
            }
            else if (reportId == 8)
            {
                return RedirectToAction("zSumChildrenStationPrint", "Station");
            }
            else if (reportId == 9)
            {
                return RedirectToAction("zSumPersonnelStationPrint", "Station");
            }
            else if (reportId == 12)
            {
                return RedirectToAction("zSumPersonnelStation2Print", "Station");
            }
            else
            {
                return RedirectToAction("zReportDemoPrint", "Station");
            }
        }

        public ActionResult ReportsSummary_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = GetReportsSummaryFromDB();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<SysReportViewModel> GetReportsSummaryFromDB()
        {

            var data = (from d in db.ΣΥΣ_ΕΝΤΥΠΑ
                        where d.DOC_CLASS == "SUMMARY"
                        orderby d.DOC_NAME
                        select new SysReportViewModel
                        {
                            DOC_ID = d.DOC_ID,
                            DOC_NAME = d.DOC_NAME,
                            DOC_DESCRIPTION = d.DOC_DESCRIPTION,
                            DOC_CLASS = d.DOC_CLASS
                        }).ToList();

            return data;
        }

        #endregion

        public ActionResult zReportDemoPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                return View();
            }
        }


        #region ΕΚΤΥΠΩΣΕΙΣ ΑΝΑΛΥΤΙΚΑ ΣΤΟΙΧΕΙΑ

        public ActionResult zPersonnelStation1Print()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                return View();
            }
        }

        public ActionResult zPersonnelStation2Print()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                return View();
            }
        }

        public ActionResult zChildrenNamesClassesPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                return View();
            }
        }

        #endregion

        #region ΕΚΤΥΠΩΣΕΙΣ ΣΥΓΚΕΝΤΡΩΤΙΚΑ ΣΤΟΙΧΕΙΑ

        public ActionResult zSumPersonnelStation2Print()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                return View();
            }
        }

        public ActionResult zSumPersonnelStationPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                return View();
            }
        }

        public ActionResult zSumChildrenStationPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                return View();
            }
        }

        public ActionResult zSumChildrenGenderPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                return View();
            }
        }

        public ActionResult zSumChildrenClassesPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                return View();
            }
        }

        public ActionResult zStationChildrenClassesPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                return View();
            }
        }

        public ActionResult zSumChildrenPeriferiakesPrint()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                return RedirectToAction("Login", "USER_STATIONS");
            }
            else
            {
                return View();
            }
        }

        #endregion

        #endregion --- ΣΤΑΤΙΣΤΙΚΕΣ ΕΚΘΕΣΕΙΣ ---


        #region VALIDATORS

        public bool MonthsExist()
        {
            var data = (from d in db.ΣΥΣ_ΜΗΝΕΣ select d).ToList();
            if (data.Count == 0) return false;

            return true;
        }

        public bool MetabolesTypesExist()
        {
            var data = (from d in db.ΣΥΣ_ΜΕΤΑΒΟΛΕΣ select d).ToList();
            if (data.Count == 0) return false;

            return true;
        }

        public bool HoursExist()
        {
            var data = (from d in db.ΣΥΣ_ΩΡΕΣ select d).ToList();
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
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΤΜΗΜΑ where d.ΒΝΣ == stationId select d).ToList();
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
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.sqlCHILD_TMIMA_SELECTOR where d.ΒΝΣ == stationId select d).ToList();
            if (data.Count == 0) return false;

            return true;
        }

        public bool ParousiesMonthExist()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.repΠΑΡΟΥΣΙΕΣ_ΜΗΝΑΣ where d.ΒΝΣ == stationId select d).ToList();
            if (data.Count == 0) return false;

            return true;
        }

        #endregion

        #region POPULATORS

        public void populateMealsMorning()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΓΕΥΜΑΤΑ_ΠΡΩΙ where d.ΒΝΣ == stationId orderby d.ΠΡΩΙΝΟ select d).ToList();
            ViewData["meals_morning"] = data;
            ViewData["defaultMealMorning"] = data.First().ΠΡΩΙΝΟ_ΚΩΔ;
        }

        public void populateMealsNoon()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ where d.ΒΝΣ == stationId orderby d.ΜΕΣΗΜΕΡΙΑΝΟ select d).ToList();
            ViewData["meals_noon"] = data;
            ViewData["defaultMealNoon"] = data.First().ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ;
        }

        public void populateMealsBaby()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΓΕΥΜΑΤΑ_ΒΡΕΦΗ where d.ΒΝΣ == stationId orderby d.ΒΡΕΦΙΚΟ select d).ToList();
            ViewData["meals_baby"] = data;
            ViewData["defaultMealBaby"] = data.First().ΒΡΕΦΙΚΟ_ΚΩΔ;
        }

        public void populateVATValues()
        {
            var data = (from d in db.ΦΠΑ_ΤΙΜΕΣ orderby d.FPA_VALUE select d).ToList();
            ViewData["vat_values"] = data;
        }

        public void populateExpenseTypes()
        {
            var data = (from d in db.ΔΑΠΑΝΕΣ_ΕΙΔΗ orderby d.ΕΙΔΟΣ_ΛΕΚΤΙΚΟ select d).ToList();
            ViewData["expense_types"] = data;
        }

        public void populateProductUnits()
        {
            var data = (from d in db.ΠΡΟΙΟΝ_ΜΟΝΑΔΕΣ orderby d.ΜΟΝΑΔΑ select d).ToList();
            ViewData["units"] = data;
        }

        public void populateFoodCategories()
        {
            var data = (from d in db.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ where d.ΔΑΠΑΝΗ_ΚΩΔ == 1 orderby d.ΚΑΤΗΓΟΡΙΑ select d).ToList();
            ViewData["categories"] = data;
        }

        public void populateCleaningCategories()
        {
            var data = (from d in db.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ where d.ΔΑΠΑΝΗ_ΚΩΔ == 2 orderby d.ΚΑΤΗΓΟΡΙΑ select d).ToList();
            ViewData["categories"] = data;
        }

        public void populateOtherCategories()
        {
            var data = (from d in db.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ where d.ΔΑΠΑΝΗ_ΚΩΔ == 3 orderby d.ΚΑΤΗΓΟΡΙΑ select d).ToList();
            ViewData["other_categories"] = data;
        }

        public void populateExtraCategories()
        {
            var data = (from d in db.ΕΞΤΡΑ_ΚΑΤΗΓΟΡΙΑ orderby d.ΚΑΤΗΓΟΡΙΑ select d).ToList();
            ViewData["extra_categories"] = data;
        }

        public void populateProductSelector()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.sqlPRODUCT_SELECTOR where d.STATION_ID == stationId orderby d.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ select d).ToList();
            ViewData["products_units"] = data;
        }

        public void populateEmployees()
        {
            var data = (from d in db.srcPERSONNEL_DATA orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ select d).ToList();

            ViewData["personnel"] = data;
            ViewData["defaultPersonnel"] = data.First().PERSONNEL_ID;
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
            ViewData["hourFirst"] = hours.First().HOUR_ID;
            ViewData["hourLast"] = hours.Last().HOUR_ID;
        }

        public void populateChildren()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.sqlCHILD_TMIMA_SELECTOR where d.ΒΝΣ == stationId orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ select d).ToList();

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
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var educators = (from d in db.sqlPERSONNEL_SELECTOR where d.ΒΝΣ == stationId && d.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ == 1 orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ select d).ToList();

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
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var tmimata = (from d in db.ΤΜΗΜΑ where d.ΒΝΣ == stationId orderby d.ΟΝΟΜΑΣΙΑ select d).ToList();
            ViewData["tmimata"] = tmimata;
            ViewData["defaultTmima"] = tmimata.First().ΤΜΗΜΑ_ΚΩΔ;
        }

        #endregion

        #region GETTERS

        public JsonResult GetMealsMorning()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΓΕΥΜΑΤΑ_ΠΡΩΙ
                        where d.ΒΝΣ == stationId
                        select new MealMorningViewModel
                        {
                            ΠΡΩΙΝΟ_ΚΩΔ = d.ΠΡΩΙΝΟ_ΚΩΔ,
                            ΠΡΩΙΝΟ = d.ΠΡΩΙΝΟ
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMealsNoon()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ
                        where d.ΒΝΣ == stationId
                        select new MealNoonViewModel
                        {
                            ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ = d.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ,
                            ΜΕΣΗΜΕΡΙΑΝΟ = d.ΜΕΣΗΜΕΡΙΑΝΟ
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMealsBaby()
        {
            loggedStation = GetLoginStation();
            stationId = (int)loggedStation.STATION_ID;

            var data = (from d in db.ΓΕΥΜΑΤΑ_ΒΡΕΦΗ
                        where d.ΒΝΣ == stationId
                        select new MealBabyViewModel
                        {
                            ΒΡΕΦΙΚΟ_ΚΩΔ = d.ΒΡΕΦΙΚΟ_ΚΩΔ,
                            ΒΡΕΦΙΚΟ = d.ΒΡΕΦΙΚΟ
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

        public JsonResult GetStations()
        {
            var data = (from d in db.ΣΥΣ_ΣΤΑΘΜΟΙ
                        orderby d.ΠΕΡΙΦΕΡΕΙΑΚΗ, d.ΕΠΩΝΥΜΙΑ
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

        public JsonResult GetApoxorisiTypes()
        {
            var data = db.ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ.Select(m => new SysApoxorisiViewModel
            {
                ΑΠΟΧΩΡΗΣΗ_ΚΩΔ = m.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ,
                ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ = m.ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ
            }).OrderBy(m => m.ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ);

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

        #endregion

        #region ΧΑΡΤΕΣ GOOGLE

        public ActionResult GoogleMaps()
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
                return View();
            }
        }

        #endregion

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

        #region MISCELLANEOUS VIEWS

        public ActionResult ErrorData(string notify = null)
        {
            if (notify != null) this.ShowMessage(MessageType.Warning, notify);

            return View();
        }

        [AllowAnonymous]
        public ActionResult PageInProgress()
        {
            return View();
        }

        #endregion

    }
}
