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
using Abacus.Services;

namespace Abacus.Controllers.SysControllers
{
    public class ToolsController : Controller
    {
        private readonly AbacusDBEntities db;
        private USER_ADMINS loggedAdmin;

        public int selectedStationID;

        private readonly TmimaService tmimaService;
        private readonly StationService stationService;
        private readonly EidikotitesService eidikotitesService;
        private readonly SchoolYearService schoolYearService;
        private readonly HoursService hoursService;
        private readonly TmimaCategoryService tmimaCategoryService;
        private readonly PersonnelTypeService personnelTypeService;
        private readonly MetaboliTypeService metaboliTypeService;
        private readonly ApoxorisiTypeService apoxorisiTypeService;
        private readonly PeriferiakiService periferiakiService;
        private readonly UserStationService userStationService;

        private readonly DiaxiristesService diaxiristesService;
        private readonly ProistamenoiService proistamenoiService;
        private readonly DirectorsService directorsService;
        private readonly GenikoiService genikoiService;
        private readonly AntiproedroiService antiproedroiService;
        private readonly DioikitesService dioikitesService;

        public ToolsController()
        {
            db = new AbacusDBEntities();

            tmimaService = new TmimaService(db);
            stationService = new StationService(db);
            eidikotitesService = new EidikotitesService(db);
            schoolYearService = new SchoolYearService(db);
            hoursService = new HoursService(db);
            tmimaCategoryService = new TmimaCategoryService(db);
            personnelTypeService = new PersonnelTypeService(db);
            metaboliTypeService = new MetaboliTypeService(db);
            apoxorisiTypeService = new ApoxorisiTypeService(db);
            periferiakiService = new PeriferiakiService(db);
            userStationService = new UserStationService(db);

            diaxiristesService = new DiaxiristesService(db);
            proistamenoiService = new ProistamenoiService(db);
            directorsService = new DirectorsService(db);
            genikoiService = new GenikoiService(db);
            antiproedroiService = new AntiproedroiService(db);
            dioikitesService = new DioikitesService(db);
        }

        protected override void Dispose(bool disposing)
        {
            tmimaService.Dispose();
            stationService.Dispose();
            eidikotitesService.Dispose();
            schoolYearService.Dispose();
            hoursService.Dispose();
            tmimaCategoryService.Dispose();
            personnelTypeService.Dispose();
            metaboliTypeService.Dispose();
            apoxorisiTypeService.Dispose();
            periferiakiService.Dispose();
            userStationService.Dispose();

            diaxiristesService.Dispose();
            proistamenoiService.Dispose();
            directorsService.Dispose();
            genikoiService.Dispose();
            antiproedroiService.Dispose();
            dioikitesService.Dispose();

            base.Dispose(disposing);
        }


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

            PopulatePeriferiakes();
            return View();
        }

        public ActionResult Station_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<StationsGridViewModel> data = stationService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Station_Create([DataSourceRequest] DataSourceRequest request, StationsGridViewModel data)
        {
            var newdata = new StationsGridViewModel();

            var existingSchool = db.ΣΥΣ_ΣΤΑΘΜΟΙ.Where(s => s.ΕΠΩΝΥΜΙΑ == data.ΕΠΩΝΥΜΙΑ).Count();
            if (existingSchool > 0) 
                ModelState.AddModelError("", "Ο σταθμός αυτός υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                stationService.Create(data);
                newdata = stationService.Refresh(data.ΣΤΑΘΜΟΣ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Station_Update([DataSourceRequest] DataSourceRequest request, StationsGridViewModel data)
        {
            var newdata = new StationsGridViewModel();

            if (data != null && ModelState.IsValid)
            {
                stationService.Update(data);
                newdata = stationService.Refresh(data.ΣΤΑΘΜΟΣ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Station_Destroy([DataSourceRequest] DataSourceRequest request, StationsGridViewModel data)
        {
            if (data != null)
            {
                stationService.Destroy(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #region ΒΝΣ DATA FORM (16-07-2019)

        public ActionResult StationEdit(int stationId)
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

            StationsViewModel data = new StationsViewModel();
            if (stationId > 0)
            {
                data = stationService.GetRecord(stationId);
            }
            else
            {
                this.ShowMessage(MessageType.Error, "Άκυρος κωδικός Σταθμού. Αποθηκεύστε πρώτα και μετά ανοίξτε την καρτέλα.");
            }
            return View(data);
        }

        [HttpPost]
        public ActionResult StationEdit(int stationId, StationsViewModel data)
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

                entity.ΕΠΩΝΥΜΙΑ = data.ΕΠΩΝΥΜΙΑ.Trim();
                entity.ΓΡΑΜΜΑΤΕΙΑ = data.ΓΡΑΜΜΑΤΕΙΑ;
                entity.ΥΠΕΥΘΥΝΟΣ = data.ΥΠΕΥΘΥΝΟΣ.Trim();
                entity.ΥΠΕΥΘΥΝΟΣ_ΦΥΛΟ = data.ΥΠΕΥΘΥΝΟΣ_ΦΥΛΟ;
                entity.ΤΑΧ_ΔΙΕΥΘΥΝΣΗ = data.ΤΑΧ_ΔΙΕΥΘΥΝΣΗ.Trim();
                entity.ΤΗΛΕΦΩΝΑ = data.ΤΗΛΕΦΩΝΑ;
                entity.ΦΑΞ = data.ΦΑΞ;
                entity.EMAIL = data.EMAIL.Trim();
                entity.ΚΙΝΗΤΟ = data.ΚΙΝΗΤΟ;
                entity.ΑΝΑΠΛΗΡΩΤΗΣ = data.ΑΝΑΠΛΗΡΩΤΗΣ.HasValue() ? data.ΑΝΑΠΛΗΡΩΤΗΣ.Trim() : data.ΑΝΑΠΛΗΡΩΤΗΣ;
                entity.ΑΝΑΠΛΗΡΩΤΗΣ_ΦΥΛΟ = data.ΑΝΑΠΛΗΡΩΤΗΣ_ΦΥΛΟ;
                entity.ΔΙΑΧΕΙΡΙΣΤΗΣ = data.ΔΙΑΧΕΙΡΙΣΤΗΣ.HasValue() ? data.ΔΙΑΧΕΙΡΙΣΤΗΣ.Trim() : data.ΔΙΑΧΕΙΡΙΣΤΗΣ;
                entity.ΔΙΑΧΕΙΡΙΣΤΗΣ_ΦΥΛΟ = data.ΔΙΑΧΕΙΡΙΣΤΗΣ_ΦΥΛΟ;
                entity.ΠΕΡΙΦΕΡΕΙΑΚΗ = data.ΠΕΡΙΦΕΡΕΙΑΚΗ;
                entity.ΠΕΡΙΦΕΡΕΙΑ = data.ΠΕΡΙΦΕΡΕΙΑ;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                // Notify here
                this.ShowMessage(MessageType.Success, "Η αποθήκευση ολοκληρώθηκε με επιτυχία.");
                return View(data);
            }
            this.ShowMessage(MessageType.Error, "Η αποθήκευση ακυρώθηκε λόγω σφαλμάτων καταχώρησης.");
            return View(data);
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
            PopulateSchoolYears();
            PopulateTmimaCategories();
            PopulateNumbersLatin();
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
            IEnumerable<TmimaViewModel> data = tmimaService.Read(stationId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Tmima_Create([DataSourceRequest] DataSourceRequest request, TmimaViewModel data, int stationId)
        {
            var newdata = new TmimaViewModel();

            if (stationId > 0)
            {
                if (data != null && ModelState.IsValid)
                {
                    tmimaService.Create(data, stationId);
                    newdata = tmimaService.Refresh(data.ΤΜΗΜΑ_ΚΩΔ);
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
                    tmimaService.Update(data, stationId);
                    newdata = tmimaService.Refresh(data.ΤΜΗΜΑ_ΚΩΔ);
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
                if (Kerberos.CanDeleteTmima(data.ΤΜΗΜΑ_ΚΩΔ))
                {
                    tmimaService.Destroy(data);
                }
                else
                {
                    ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί το τμήμα αυτό διότι είναι σε χρήση.");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
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
            PopulateKladoi();
            return View();
        }

        public ActionResult Eidikotita_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<EidikotitesViewModel> data = eidikotitesService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Eidikotita_Create([DataSourceRequest] DataSourceRequest request, EidikotitesViewModel data)
        {
            var newdata = new EidikotitesViewModel();

            var existingData = db.ΣΥΣ_ΕΙΔΙΚΟΤΗΤΕΣ.Where(s => s.EIDIKOTITA_TEXT == data.EIDIKOTITA_TEXT).Count();
            if (existingData > 0) 
                ModelState.AddModelError("", "Η ειδικότητα αυτή υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                eidikotitesService.Create(data);
                newdata = eidikotitesService.Refresh(data.EIDIKOTITA_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Eidikotita_Update([DataSourceRequest] DataSourceRequest request, EidikotitesViewModel data)
        {
            var newdata = new EidikotitesViewModel();

            if (data != null)
            {
                eidikotitesService.Update(data);
                newdata = eidikotitesService.Refresh(data.EIDIKOTITA_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Eidikotita_Destroy([DataSourceRequest] DataSourceRequest request, EidikotitesViewModel data)
        {
            if (data != null)
            {
                if (Kerberos.CanDeleteEidikotita(data.EIDIKOTITA_ID))
                {
                    eidikotitesService.Destroy(data);
                }
                else
                {
                    ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί η ειδικότητα διότι είναι σε χρήση.");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
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


        #endregion


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

            return View();
        }

        public ActionResult SchoolYear_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<SysSchoolYearViewModel> data = schoolYearService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SchoolYear_Create([DataSourceRequest] DataSourceRequest request, SysSchoolYearViewModel data)
        {
            var newdata = new SysSchoolYearViewModel();

            var existingdata = db.ΣΥΣ_ΣΧΟΛΙΚΑ_ΕΤΗ.Where(s => s.ΣΧΟΛΙΚΟ_ΕΤΟΣ == data.ΣΧΟΛΙΚΟ_ΕΤΟΣ).Count();
            if (existingdata > 0) 
                ModelState.AddModelError("", "Αυτό το σχολικό έτος υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                schoolYearService.Create(data);

                if (!Kerberos.ValidateCurrentSchoolYear())
                {
                    ModelState.AddModelError("", "Πρέπει να οριστεί ένα και μόνο ένα σχολικό έτος ως το τρέχον.");
                }
                newdata = schoolYearService.Refresh(data.SCHOOLYEAR_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SchoolYear_Update([DataSourceRequest] DataSourceRequest request, SysSchoolYearViewModel data)
        {
            var newdata = new SysSchoolYearViewModel();

            if (data != null && ModelState.IsValid)
            {
                schoolYearService.Update(data);

                if (!Kerberos.ValidateCurrentSchoolYear())
                {
                    ModelState.AddModelError("", "Πρέπει να οριστεί ένα και μόνο ένα σχολικό έτος ως το τρέχον.");
                }
                newdata = schoolYearService.Refresh(data.SCHOOLYEAR_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SchoolYear_Destroy([DataSourceRequest] DataSourceRequest request, SysSchoolYearViewModel data)
        {
            if (data != null)
            {
                if (Kerberos.CanDeleteSchoolYear(data.SCHOOLYEAR_ID))
                {
                    schoolYearService.Destroy(data);
                }
                else
                {
                    ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί το σχολικό έτος διότι είναι σε χρήση.");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
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

            return View();
        }

        public ActionResult Hours_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<HoursViewModel> data = hoursService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Hours_Create([DataSourceRequest] DataSourceRequest request, HoursViewModel data)
        {
            var newdata = new HoursViewModel();

            var existingdata = db.ΣΥΣ_ΩΡΕΣ.Where(s => s.HOUR_TEXT == data.HOUR_TEXT).Count();
            if (existingdata > 0) 
                ModelState.AddModelError("", "Αυτή η ώρα υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                hoursService.Create(data);
                newdata = hoursService.Refresh(data.HOUR_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Hours_Update([DataSourceRequest] DataSourceRequest request, HoursViewModel data)
        {
            var newdata = new HoursViewModel();

            if (data != null && ModelState.IsValid)
            {
                hoursService.Update(data);
                newdata = hoursService.Refresh(data.HOUR_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Hours_Destroy([DataSourceRequest] DataSourceRequest request, HoursViewModel data)
        {
            if (data != null)
            {
                if (Kerberos.CanDeleteHour(data.HOUR_ID))
                {
                    hoursService.Destroy(data);
                }
                else
                {
                    ModelState.AddModelError("", "Η διαγραφή της ώρας αυτής δεν είναι δυνατή διότι είναι σε χρήση.");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
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

            return View();
        }

        public ActionResult TmimaCategory_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<TmimaCategoryViewModel> data = tmimaCategoryService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TmimaCategory_Create([DataSourceRequest] DataSourceRequest request, TmimaCategoryViewModel data)
        {
            var newdata = new TmimaCategoryViewModel();

            var existingdata = db.ΤΜΗΜΑ_ΚΑΤΗΓΟΡΙΑ.Where(s => s.CATEGORY_TEXT == data.CATEGORY_TEXT).Count();
            if (existingdata > 0) 
                ModelState.AddModelError("", "Αυτή η κατηγορία τμήματος υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                tmimaCategoryService.Create(data);
                newdata = tmimaCategoryService.Refresh(data.CATEGORY_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TmimaCategory_Update([DataSourceRequest] DataSourceRequest request, TmimaCategoryViewModel data)
        {
            var newdata = new TmimaCategoryViewModel();

            if (data != null && ModelState.IsValid)
            {
                tmimaCategoryService.Update(data);
                newdata = tmimaCategoryService.Refresh(data.CATEGORY_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TmimaCategory_Destroy([DataSourceRequest] DataSourceRequest request, TmimaCategoryViewModel data)
        {
            if (data != null)
            {
                if (Kerberos.CanDeleteTmimaCategory(data.CATEGORY_ID))
                {
                    tmimaCategoryService.Destroy(data);
                }
                else
                {
                    ModelState.AddModelError("", "Η διαγραφή της κατηγορίας είνα αδύνατη διότι χρησιμοποιείται.");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
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
            IEnumerable<PersonnelTypeViewModel> data = personnelTypeService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PersonnelType_Create([DataSourceRequest] DataSourceRequest request, PersonnelTypeViewModel data)
        {
            var newdata = new PersonnelTypeViewModel();

            var existingdata = db.ΣΥΣ_ΠΡΟΣΩΠΙΚΟ.Where(s => s.PROSOPIKO_TEXT == data.PROSOPIKO_TEXT).Count();
            if (existingdata > 0) 
                ModelState.AddModelError("", "Αυτή η κατηγορία προσωπικού υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                personnelTypeService.Create(data);
                newdata = personnelTypeService.Refresh(data.PROSOPIKO_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PersonnelType_Update([DataSourceRequest] DataSourceRequest request, PersonnelTypeViewModel data)
        {
            var newdata = new PersonnelTypeViewModel();

            if (data != null && ModelState.IsValid)
            {
                personnelTypeService.Update(data);
                newdata = personnelTypeService.Refresh(data.PROSOPIKO_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PersonnelType_Destroy([DataSourceRequest] DataSourceRequest request, PersonnelTypeViewModel data)
        {
            if (data != null)
            {
                if (Kerberos.CanDeletePersonnelType(data.PROSOPIKO_ID))
                {
                    personnelTypeService.Destroy(data);
                }
                else
                {
                    ModelState.AddModelError("", "Η διαγραφή κατηγοριών είναι αδύνατη διότι χρησιμοποιείται.");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
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
            IEnumerable<SysMetabolesViewModel> data = metaboliTypeService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MetaboliType_Create([DataSourceRequest] DataSourceRequest request, SysMetabolesViewModel data)
        {
            var newdata = new SysMetabolesViewModel();

            var existingdata = db.ΣΥΣ_ΜΕΤΑΒΟΛΕΣ.Where(s => s.METABOLI_TEXT == data.METABOLI_TEXT).Count();
            if (existingdata > 0) 
                ModelState.AddModelError("", "Αυτή η κατηγορία προσωπικού υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                metaboliTypeService.Create(data);
                newdata = metaboliTypeService.Refresh(data.METABOLI_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MetaboliType_Update([DataSourceRequest] DataSourceRequest request, SysMetabolesViewModel data)
        {
            var newdata = new SysMetabolesViewModel();

            if (data != null && ModelState.IsValid)
            {
                metaboliTypeService.Update(data);
                newdata = metaboliTypeService.Refresh(data.METABOLI_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MetaboliType_Destroy([DataSourceRequest] DataSourceRequest request, SysMetabolesViewModel data)
        {
            if (data != null)
            {
                if (Kerberos.CanDeleteMetaboliType(data.METABOLI_ID))
                {
                    metaboliTypeService.Destroy(data);
                }
                else
                {
                    ModelState.AddModelError("", "Η διαγραφή του είδους μεταβολής είναι αδύνατη διότι χρησιμοποιείται.");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
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
            IEnumerable<SysApoxorisiViewModel> data = apoxorisiTypeService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Apoxorisi_Create([DataSourceRequest] DataSourceRequest request, SysApoxorisiViewModel data)
        {
            var newdata = new SysApoxorisiViewModel();

            var existingdata = db.ΣΥΣ_ΑΠΟΧΩΡΗΣΕΙΣ.Where(s => s.ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ == data.ΑΠΟΧΩΡΗΣΗ_ΛΕΚΤΙΚΟ).Count();
            if (existingdata > 0) 
                ModelState.AddModelError("", "Αυτή η κατηγορία προσωπικού υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                apoxorisiTypeService.Create(data);
                newdata = apoxorisiTypeService.Refresh(data.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Apoxorisi_Update([DataSourceRequest] DataSourceRequest request, SysApoxorisiViewModel data)
        {
            var newdata = new SysApoxorisiViewModel();

            if (data != null && ModelState.IsValid)
            {
                apoxorisiTypeService.Update(data);
                newdata = apoxorisiTypeService.Refresh(data.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ);
            }

            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Apoxorisi_Destroy([DataSourceRequest] DataSourceRequest request, SysApoxorisiViewModel data)
        {
            if (data != null)
            {
                if (Kerberos.CanDeleteApoxorisiType(data.ΑΠΟΧΩΡΗΣΗ_ΚΩΔ))
                {
                    apoxorisiTypeService.Destroy(data);
                }
                else
                {
                    ModelState.AddModelError("", "Η διαγραφή του είδους αποχώρησης δεν είναι δυνατή γιατί η τιμή αυτή χρησιμοποιείται ήδη.");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
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

            return View();
        }

        public ActionResult Periferiaki_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<SysPeriferiakiViewModel> data = periferiakiService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Periferiaki_Create([DataSourceRequest] DataSourceRequest request, SysPeriferiakiViewModel data)
        {
            var newdata = new SysPeriferiakiViewModel();

            var existingdata = db.ΣΥΣ_ΠΕΡΙΦΕΡΕΙΑΚΕΣ.Where(s => s.ΕΠΩΝΥΜΙΑ_ΠΕΡΙΦΕΡΕΙΑ == data.ΕΠΩΝΥΜΙΑ_ΠΕΡΙΦΕΡΕΙΑ).Count();
            if (existingdata > 0) 
                ModelState.AddModelError("", "Αυτή η περιφερειακή διεύθυνση υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                periferiakiService.Create(data);
                newdata = periferiakiService.Refresh(data.ΚΩΔΙΚΟΣ_ΠΕΡΙΦΕΡΕΙΑ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Periferiaki_Update([DataSourceRequest] DataSourceRequest request, SysPeriferiakiViewModel data)
        {
            var newdata = new SysPeriferiakiViewModel();

            if (data != null && ModelState.IsValid)
            {
                periferiakiService.Update(data);
                newdata = periferiakiService.Refresh(data.ΚΩΔΙΚΟΣ_ΠΕΡΙΦΕΡΕΙΑ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Periferiaki_Destroy([DataSourceRequest] DataSourceRequest request, SysPeriferiakiViewModel data)
        {
            if (data != null)
            {
                if (Kerberos.CanDeletePeriferiaki(data.ΚΩΔΙΚΟΣ_ΠΕΡΙΦΕΡΕΙΑ))
                {
                    periferiakiService.Destroy(data);
                }
                else
                {
                    ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί αυτή η Περιφερειακή διότι χρησιμοποιείται.");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
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
            }).ToList();

            return Json(periferies.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dimoi([DataSourceRequest] DataSourceRequest request, int periferiaId)
        {
            var dimoi = db.ΣΥΣ_ΔΗΜΟΙ.Select(p => new DimosViewModel()
            {
                DIMOS_ID = p.DIMOS_ID,
                DIMOS = p.DIMOS,
                DIMOS_PERIFERIA = p.DIMOS_PERIFERIA
            }).Where(o => o.DIMOS_PERIFERIA == periferiaId).ToList();

            return Json(dimoi.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
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
            if (notify != null) this.ShowMessage(MessageType.Info, notify);

            PopulateStations();

            return View();
        }

        public ActionResult CreatePasswords()
        {
            var stations = (from s in db.USER_STATIONS select s).ToList();

            foreach (var station in stations)
            {
                station.PASSWORD = Common.GeneratePassword() + String.Format("{0:000}", station.STATION_ID);
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
            IEnumerable<UserStationViewModel> data = userStationService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserStation_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<UserStationViewModel> data)
        {
            var results = new List<UserStationViewModel>();
            foreach (var item in data)
            {
                if (item != null && ModelState.IsValid)
                {
                    userStationService.Create(item);
                    results.Add(item);
                }
            }
            return Json(results.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserStation_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<UserStationViewModel> data)
        {
            if (data != null)
            {
                foreach (var item in data)
                {
                    userStationService.Update(item);
                }
            }
            return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserStation_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<UserStationViewModel> data)
        {
            if (data.Any())
            {
                foreach (var item in data)
                {
                    userStationService.Destroy(item);
                }
            }
            return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
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
            var data = GetStationLoginsFromDB();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
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
            var data = GetAdminLoginsFromDB();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
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
            PopulateSchoolYears();
            PopulateGenders();
            return View();
        }

        #region ΔΙΑΧΕΙΡΙΣΤΕΣ

        public ActionResult Diaxiristis_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = diaxiristesService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Diaxiristis_Create([DataSourceRequest] DataSourceRequest request, DiaxiristisViewModel data)
        {
            var newData = new DiaxiristisViewModel();

            var existingdata = db.Δ_ΔΙΑΧΕΙΡΙΣΤΕΣ.Where(s => s.ΟΝΟΜΑΤΕΠΩΝΥΜΟ == data.ΟΝΟΜΑΤΕΠΩΝΥΜΟ).Count();
            if (existingdata > 0)
                ModelState.AddModelError("", "Ο διαχειριστής αυτός υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                diaxiristesService.Create(data);
                newData = diaxiristesService.Refresh(data.ΔΙΑΧΕΙΡΙΣΤΗΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Diaxiristis_Update([DataSourceRequest] DataSourceRequest request, DiaxiristisViewModel data)
        {
            var newData = new DiaxiristisViewModel();

            if (data != null & ModelState.IsValid)
            {
                diaxiristesService.Update(data);
                newData = diaxiristesService.Refresh(data.ΔΙΑΧΕΙΡΙΣΤΗΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Diaxiristis_Destroy([DataSourceRequest] DataSourceRequest request, DiaxiristisViewModel data)
        {
            if (data != null)
            {
                diaxiristesService.Destroy(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion ΔΙΑΧΕΙΡΙΣΤΕΣ

        #region ΠΡΟΪΣΤΑΜΕΝΟΙ

        public ActionResult Proistamenos_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = proistamenoiService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Proistamenos_Create([DataSourceRequest] DataSourceRequest request, ProistamenosViewModel data)
        {
            var newData = new ProistamenosViewModel();

            var existingdata = db.Δ_ΠΡΟΙΣΤΑΜΕΝΟΙ.Where(s => s.ΣΧΟΛΙΚΟ_ΕΤΟΣ == data.ΣΧΟΛΙΚΟ_ΕΤΟΣ).Count();
            if (existingdata > 0)
                ModelState.AddModelError("", "Υπάρχουν ήδη στοιχεία γι' αυτό το σχολικό έτος. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                proistamenoiService.Create(data);
                newData = proistamenoiService.Refresh(data.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Proistamenos_Update([DataSourceRequest] DataSourceRequest request, ProistamenosViewModel data)
        {
            var newData = new ProistamenosViewModel();

            if (data != null & ModelState.IsValid)
            {
                proistamenoiService.Update(data);
                newData = proistamenoiService.Refresh(data.ΠΡΟΙΣΤΑΜΕΝΟΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Proistamenos_Destroy([DataSourceRequest] DataSourceRequest request, ProistamenosViewModel data)
        {
            if (data != null)
            {
                proistamenoiService.Destroy(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion ΠΡΟΪΣΤΑΜΕΝΟΙ

        #region ΔΙΕΥΘΥΝΤΕΣ

        public ActionResult Director_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = directorsService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Director_Create([DataSourceRequest] DataSourceRequest request, DirectorViewModel data)
        {
            var newData = new DirectorViewModel();

            var existingdata = db.Δ_ΔΙΕΥΘΥΝΤΕΣ.Where(s => s.ΣΧΟΛΙΚΟ_ΕΤΟΣ == data.ΣΧΟΛΙΚΟ_ΕΤΟΣ).Count();
            if (existingdata > 0)
                ModelState.AddModelError("", "Υπάρχουν ήδη στοιχεία γι' αυτό το σχολικό έτος. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                directorsService.Create(data);
                newData = directorsService.Refresh(data.ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Director_Update([DataSourceRequest] DataSourceRequest request, DirectorViewModel data)
        {
            var newData = new DirectorViewModel();

            if (data != null & ModelState.IsValid)
            {
                directorsService.Update(data);
                newData = directorsService.Refresh(data.ΔΙΕΥΘΥΝΤΗΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Director_Destroy([DataSourceRequest] DataSourceRequest request, DirectorViewModel data)
        {
            if (data != null)
            {
                directorsService.Destroy(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion ΔΙΕΥΘΥΝΤΕΣ

        #region ΓΕΝΙΚΟΙ ΔΙΕΥΘΥΝΤΕΣ

        public ActionResult Genikos_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = genikoiService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Genikos_Create([DataSourceRequest] DataSourceRequest request, DirectorGeneralViewModel data)
        {
            var newData = new DirectorGeneralViewModel();

            var existingdata = db.Δ_ΔΙΕΥΘΥΝΤΕΣ_ΓΕΝΙΚΟΙ.Where(s => s.ΣΧΟΛΙΚΟ_ΕΤΟΣ == data.ΣΧΟΛΙΚΟ_ΕΤΟΣ).Count();
            if (existingdata > 0)
                ModelState.AddModelError("", "Υπάρχουν ήδη στοιχεία γι' αυτό το σχολικό έτος. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                genikoiService.Create(data);
                newData = genikoiService.Refresh(data.ΓΕΝΙΚΟΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Genikos_Update([DataSourceRequest] DataSourceRequest request, DirectorGeneralViewModel data)
        {
            var newData = new DirectorGeneralViewModel();

            if (data != null & ModelState.IsValid)
            {
                genikoiService.Update(data);
                newData = genikoiService.Refresh(data.ΓΕΝΙΚΟΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Genikos_Destroy([DataSourceRequest] DataSourceRequest request, DirectorGeneralViewModel data)
        {
            if (data != null)
            {
                genikoiService.Destroy(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion ΓΕΝΙΚΟΙ ΔΙΕΥΘΥΝΤΕΣ

        #region ΑΝΤΙΠΡΟΕΔΡΟΙ

        public ActionResult Antiproedros_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = antiproedroiService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Antiproedros_Create([DataSourceRequest] DataSourceRequest request, AntiproedrosViewModel data)
        {
            var newData = new AntiproedrosViewModel();

            var existingdata = db.Δ_ΑΝΤΙΠΡΟΕΔΡΟΙ.Where(s => s.ΣΧΟΛΙΚΟ_ΕΤΟΣ == data.ΣΧΟΛΙΚΟ_ΕΤΟΣ).Count();
            if (existingdata > 0)
                ModelState.AddModelError("", "Υπάρχουν ήδη στοιχεία γι' αυτό το σχολικό έτος. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                antiproedroiService.Create(data);
                newData = antiproedroiService.Refresh(data.ΑΝΤΙΠΡΟΕΔΡΟΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Antiproedros_Update([DataSourceRequest] DataSourceRequest request, AntiproedrosViewModel data)
        {
            var newData = new AntiproedrosViewModel();

            if (data != null & ModelState.IsValid)
            {
                antiproedroiService.Update(data);
                newData = antiproedroiService.Refresh(data.ΑΝΤΙΠΡΟΕΔΡΟΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Antiproedros_Destroy([DataSourceRequest] DataSourceRequest request, AntiproedrosViewModel data)
        {
            if (data != null)
            {
                antiproedroiService.Destroy(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion ΑΝΤΙΠΡΟΕΔΡΟΙ

        #region ΔΙΟΙΚΗΤΕΣ

        public ActionResult Dioikitis_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = dioikitesService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Dioikitis_Create([DataSourceRequest] DataSourceRequest request, DioikitisViewModel data)
        {
            var newData = new DioikitisViewModel();

            var existingdata = db.Δ_ΔΙΟΙΚΗΤΕΣ.Where(s => s.ΣΧΟΛΙΚΟ_ΕΤΟΣ == data.ΣΧΟΛΙΚΟ_ΕΤΟΣ).Count();
            if (existingdata > 0)
                ModelState.AddModelError("", "Υπάρχουν ήδη στοιχεία γι' αυτό το σχολικό έτος. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                dioikitesService.Create(data);
                newData = dioikitesService.Refresh(data.ΔΙΟΙΚΗΤΗΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Dioikitis_Update([DataSourceRequest] DataSourceRequest request, DioikitisViewModel data)
        {
            var newData = new DioikitisViewModel();

            if (data != null & ModelState.IsValid)
            {
                dioikitesService.Update(data);
                newData = dioikitesService.Refresh(data.ΔΙΟΙΚΗΤΗΣ_ΚΩΔ);
            }
            return Json(new[] { newData }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Dioikitis_Destroy([DataSourceRequest] DataSourceRequest request, DioikitisViewModel data)
        {
            if (data != null)
            {
                dioikitesService.Destroy(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion ΔΙΟΙΚΗΤΕΣ

        #endregion


        #region POPULATORS

        public void PopulateStations()
        {
            var stations = (from d in db.ΣΥΣ_ΣΤΑΘΜΟΙ orderby d.ΕΠΩΝΥΜΙΑ select d).ToList();

            ViewData["stations"] = stations;
            ViewData["defaultStation"] = stations.First().ΣΤΑΘΜΟΣ_ΚΩΔ;
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
