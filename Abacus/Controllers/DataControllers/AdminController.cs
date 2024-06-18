using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Globalization;
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
    public class AdminController : Controller
    {
        private readonly AbacusDBEntities db;
        private USER_ADMINS loggedAdmin;

        private readonly TmimaService tmimaService;
        private readonly ChildDataService childDataService;
        private readonly ChildEgrafiService childEgrafiService;
        private readonly ChildInfoService childInfoService;
        private readonly PersonnelService personnelService;
        private readonly EducatorTmimaService educatorTmimaService;
        private readonly ProgrammaService programmaService;
        private readonly ParousiaService parousiaService;
        private readonly MetabolesService metabolesService;
        private readonly MetabolesReportService metabolesReportService;

        private readonly MealMorningService mealMorningService;
        private readonly MealNoonService mealNoonService;
        private readonly MealBabyService mealBabyService;
        private readonly MealPlanService mealPlanService;

        private readonly PersonCostService personCostService;
        private readonly ExtraCategoryService extraCategoryService;
        private readonly BudgetMonthService budgetMonthService;
        private readonly VATScaleService vatScaleService;
        private readonly ProductCategoryService productCategoryService;
        private readonly ProductService productService;
        private readonly ProductStationService productStationService;

        private readonly CostFoodService costFoodService;
        private readonly CostCleaningService costCleaningService;
        private readonly CostOtherService costOtherService;
        private readonly CostGeneralService costGeneralService;

        public AdminController()
        {
            db = new AbacusDBEntities();

            tmimaService = new TmimaService(db);
            childDataService = new ChildDataService(db);
            childEgrafiService = new ChildEgrafiService(db);
            childInfoService = new ChildInfoService(db);
            personnelService = new PersonnelService(db);
            educatorTmimaService = new EducatorTmimaService(db);
            programmaService = new ProgrammaService(db);
            parousiaService = new ParousiaService(db);
            metabolesService = new MetabolesService(db);
            metabolesReportService = new MetabolesReportService(db);

            mealMorningService = new MealMorningService(db);
            mealNoonService = new MealNoonService(db);
            mealBabyService = new MealBabyService(db);
            mealPlanService = new MealPlanService(db);

            personCostService = new PersonCostService(db);
            extraCategoryService = new ExtraCategoryService(db);
            budgetMonthService = new BudgetMonthService(db);
            vatScaleService = new VATScaleService(db);
            productCategoryService = new ProductCategoryService(db);
            productService = new ProductService(db);
            productStationService = new ProductStationService(db);

            costFoodService = new CostFoodService(db);
            costCleaningService = new CostCleaningService(db);
            costOtherService = new CostOtherService(db);
            costGeneralService = new CostGeneralService(db);
        }

        protected override void Dispose(bool disposing)
        {
            tmimaService.Dispose();
            childDataService.Dispose();
            childEgrafiService.Dispose();
            childInfoService.Dispose();
            personnelService.Dispose();
            educatorTmimaService.Dispose();
            programmaService.Dispose();
            parousiaService.Dispose();
            metabolesService.Dispose();
            metabolesReportService.Dispose();

            mealMorningService.Dispose();
            mealNoonService.Dispose();
            mealBabyService.Dispose();
            mealPlanService.Dispose();

            personCostService.Dispose();
            extraCategoryService.Dispose();
            budgetMonthService.Dispose();
            vatScaleService.Dispose();
            productCategoryService.Dispose();
            productService.Dispose();
            productStationService.Dispose();

            costFoodService.Dispose();
            costCleaningService.Dispose();
            costOtherService.Dispose();
            costGeneralService.Dispose();

            base.Dispose(disposing);
        }

        public ActionResult Index(string notify = null)
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
            return View();
        }

        #region --- ΒΡΕΦΟΝΗΠΙΑ ---

        #region ΒΡΕΦΟΝΗΠΙΑ - ΣΤΟΙΧΕΙΑ ΚΑΙ ΕΓΓΡΑΦΕΣ (20-07-2019)

        public ActionResult xChildrenData(string notify = null)
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

            if (!StationsExist() || !SchoolYearsExist() || !TmimataExist())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει πρώτα να ορίσετε ΒΝΣ, σχολικά έτη και τμήματα των ΒΝΣ στις Ρυθμίσεις.";
                return RedirectToAction("Index", "Admin", new { notify = msg });
            }

            PopulateStations();
            PopulateTmimata();
            PopulateSchoolYears();
            return View();
        }

        public ActionResult FilteredTmima_Read(int schoolyearId = 0, int stationId = 0)
        {
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
            IEnumerable<ChildGridViewModel> data = childDataService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Children_Create([DataSourceRequest] DataSourceRequest request, ChildGridViewModel data)
        {
            var newdata = new ChildGridViewModel();

            if (!Kerberos.ValidatePrimaryKeyChild((int)data.ΑΜ, (int)data.ΒΝΣ)) 
                ModelState.AddModelError("", "Ο Α.Μ. που δώθηκε είναι ήδη καταχωρημένος για το σταθμό αυτό.");

            if (ModelState.IsValid)
            {
                childDataService.Create(data);
                newdata = childDataService.Refresh(data.CHILD_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Children_Update([DataSourceRequest] DataSourceRequest request, ChildGridViewModel data)
        {
            var newdata = new ChildGridViewModel();

            if (ModelState.IsValid)
            {
                childDataService.Update(data);
                newdata = childDataService.Refresh(data.CHILD_ID);
            }

            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Children_Destroy([DataSourceRequest] DataSourceRequest request, ChildGridViewModel data)
        {
            if (data != null)
            {
                if (!Kerberos.CanDeleteChild(data.CHILD_ID)) 
                    ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί το παιδί αυτό διότι υπάρχουν εγγραφές του σε τμήματα, ή/και παρουσίες.");

                if (ModelState.IsValid)
                {
                    childDataService.Destroy(data);
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CHILDREN EGRAFES GRID CRUD

        public ActionResult Egrafes_Read([DataSourceRequest] DataSourceRequest request, int childId = 0)
        {
            IEnumerable<ChildTmimaViewModel> data = childEgrafiService.Read(childId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Egrafes_Create([DataSourceRequest] DataSourceRequest request, ChildTmimaViewModel data, int childId = 0)
        {
            ChildTmimaViewModel newdata = new ChildTmimaViewModel();
            int stationId = Common.GetStationFromfChildID(childId);

            if (childId > 0 && stationId > 0)
            {
                var existingData = db.ΠΑΙΔΙΑ_ΕΓΓΡΑΦΕΣ.Where(s => s.ΠΑΙΔΙ_ΚΩΔ == childId && s.ΤΜΗΜΑ == data.ΤΜΗΜΑ).Count();
                if (existingData > 0) 
                    ModelState.AddModelError("", "Υπάρχει ήδη καταχώρηση του παιδιού στο τμήμα αυτό.");

                if (data != null && ModelState.IsValid)
                {
                    childEgrafiService.Create(data, childId, stationId);
                    newdata = childEgrafiService.Refresh(data.ΕΓΓΡΑΦΗ_ΚΩΔ);
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
            int stationId = Common.GetStationFromfChildID(childId);

            if (childId > 0 && stationId > 0)
            {
                if (data != null && ModelState.IsValid)
                {
                    childEgrafiService.Update(data, childId, stationId);
                    newdata = childEgrafiService.Refresh(data.ΕΓΓΡΑΦΗ_ΚΩΔ);
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
                childEgrafiService.Destroy(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CHILDREN DATA FORM

        public ActionResult xChildrenEdit(int childId)
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
            if (!(childId > 0))
            {
                string msg = "Ο κωδικός παιδιού δεν είναι έγκυρος. Η εγγραφή πρέπει να έχει αποθηκευτεί πρώτα.";
                return RedirectToAction("ErrorData", "Admin", new { notify = msg });
            }

            ChildViewModel child = childDataService.GetRecord(childId);
            if (child == null)
            {
                string msg = "Παρουσιάστηκε πρόβλημα εύρεσης των δεδομένων παιδιού.";
                return RedirectToAction("ErrorData", "Admin", new { notify = msg });
            }

            return View(child);
        }

        [HttpPost]
        public ActionResult xChildrenEdit(int childId, ChildViewModel data)
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

            if (data == null)
            {
                string msg = "Παρουσιάστηκε πρόβλημα εύρεσης των δεδομένων παιδιού.";
                return RedirectToAction("ErrorData", "Admin", new { notify = msg });
            }
            int stationId = Common.GetStationFromfChildID(childId);

            if (data != null && ModelState.IsValid)
            {
                ΠΑΙΔΙΑ entity = db.ΠΑΙΔΙΑ.Find(childId);

                if (entity != null)
                {
                    entity.ΑΜ = data.ΑΜ;
                    entity.ΒΝΣ = stationId;
                    entity.ΕΠΩΝΥΜΟ = data.ΕΠΩΝΥΜΟ.Trim();
                    entity.ΟΝΟΜΑ = data.ΟΝΟΜΑ.Trim();
                    entity.ΠΑΤΡΩΝΥΜΟ = data.ΠΑΤΡΩΝΥΜΟ.Trim();
                    entity.ΜΗΤΡΩΝΥΜΟ = data.ΜΗΤΡΩΝΥΜΟ.Trim();
                    entity.ΦΥΛΟ = data.ΦΥΛΟ;
                    entity.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ = data.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ;
                    entity.ΑΦΜ = data.ΑΦΜ.HasValue() ? data.ΑΦΜ.Trim() : string.Empty;
                    entity.ΑΜΚΑ = data.ΑΜΚΑ;
                    entity.ΔΙΕΥΘΥΝΣΗ = data.ΔΙΕΥΘΥΝΣΗ;
                    entity.ΤΗΛΕΦΩΝΑ = data.ΤΗΛΕΦΩΝΑ;
                    entity.EMAIL = data.EMAIL;
                    entity.ΕΙΣΟΔΟΣ_ΗΜΝΙΑ = data.ΕΙΣΟΔΟΣ_ΗΜΝΙΑ;
                    entity.ΕΞΟΔΟΣ_ΗΜΝΙΑ = data.ΕΞΟΔΟΣ_ΗΜΝΙΑ;
                    entity.ΕΝΕΡΓΟΣ = data.ΕΝΕΡΓΟΣ;
                    entity.ΗΛΙΚΙΑ = Common.CalculateChildAge(data);
                    entity.AGE = Common.ChildAgeDecimal(data.ΗΜΝΙΑ_ΓΕΝΝΗΣΗ, data.ΕΙΣΟΔΟΣ_ΗΜΝΙΑ);
                    entity.ΠΑΡΑΤΗΡΗΣΕΙΣ = data.ΠΑΡΑΤΗΡΗΣΕΙΣ;

                    db.Entry(entity).State = EntityState.Modified;
                    string ErrorMsg = Kerberos.ValidateChildFields(entity);
                    if (String.IsNullOrEmpty(ErrorMsg))
                    {
                        db.SaveChanges();
                        this.ShowMessage(MessageType.Success, "Η αποθήκευση ολοκληρώθηκε με επιτυχία.");
                        ChildViewModel newStudent = childDataService.GetRecord(childId);
                        return View(newStudent);
                    }
                    else
                    {
                        this.ShowMessage(MessageType.Error, "Η αποθήκευση απέτυχε λόγω επικύρωσης δεδομένων. " + ErrorMsg);
                        return View(data);
                    }
                }
            }
            this.ShowMessage(MessageType.Error, "Η αποθήκευση ακυρώθηκε λόγω σφαλμάτων καταχώρησης.");
            return View(data);
        }

        #endregion

        #endregion


        #region ΜΗΤΡΩΟ ΒΡΕΦΟΝΗΠΙΩΝ

        public ActionResult xChildrenInfoList(string notify = null)
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

            IEnumerable<sqlChildInfoViewModel> data = childInfoService.ReadGlobal();

            if (data.Count() == 0)
            {
                string msg = "Δεν υπάρχουν καταχωρημένα παιδιά για την εμφάνιση του μητρώου.";
                return RedirectToAction("Index", "Admin", new { notify = msg });
            }
            sqlChildInfoViewModel child = data.First();

            if (notify != null)
            {
                this.ShowMessage(MessageType.Info, notify);
            }
            return View(child);
        }

        public ActionResult ChildrenInfo_Read([DataSourceRequest] DataSourceRequest request, int activeId = 0)
        {
            IEnumerable<sqlChildInfoViewModel> data = childInfoService.ReadGlobal(activeId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EgrafesInfo_Read(int childId, [DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<sqlEgrafesInfoViewModel> data = childInfoService.GetEgrafes(childId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetChildRecord(int childId)
        {
            sqlChildInfoViewModel data = childInfoService.ReadGlobal().Where(e => e.CHILD_ID == childId).FirstOrDefault();

            return PartialView("xChildrenInfoPartial", data);
        }

        #endregion


        #region ΤΜΗΜΑΤΑ ΒΡΕΦΟΝΗΠΙΩΝ

        public ActionResult xTmimaChildren(string notify = null)
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

            if (!SchoolYearsExist())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει πρώτα να καταχωρηθούν σχολικά έτη.";
                return RedirectToAction("Index", "Admin", new { notify = msg });
            }

            PopulateSchoolYears();
            return View();
        }

        public ActionResult Tmimata_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<sqlTmimaInfoViewModel> data = tmimaService.ReadInfo();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult TmimaChildren_Read([DataSourceRequest] DataSourceRequest request, int tmimaId = 0)
        {
            List<sqlTmimaChildViewModel> data = tmimaService.GetChildren(tmimaId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult xDocKatastasiGenikiPrint(int tmimaId)
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

        #endregion


        #region --- ΠΡΟΣΩΠΙΚΟ ---

        #region ΠΡΟΣΩΠΙΚΟ (20-07-2019)

        public ActionResult xPersonnelData(string notify = null)
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

            if (!StationsExist() || !PersonnelTypesExist() || !TmimataExist() || !SchoolYearsExist())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει πρώτα να ορίσετε ΒΝΣ, κατηγορίες προσωπικού, τμήματα και σχολικά έτη στις Ρυθμίσεις.";
                return RedirectToAction("Index", "Admin", new { notify = msg });
            }

            PopulateStations();
            PopulatePersonnelTypes();
            PopulateSchoolYears();
            PopulateTmimata();
            return View();
        }


        #region PERSONNEL GRID CRUD

        public ActionResult Personnel_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<PersonnelGridViewModel> data = personnelService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Personnel_Create([DataSourceRequest] DataSourceRequest request, PersonnelGridViewModel data)
        {
            var newdata = new PersonnelGridViewModel();

            if (!Kerberos.ValidatePrimaryKeyPerson((int)data.ΜΗΤΡΩΟ, (int)data.ΒΝΣ)) 
                ModelState.AddModelError("", "Ο Α.Μ. που δώθηκε είναι ήδη καταχωρημένος για το σταθμό αυτό.");

            if (!Kerberos.IsValidAM(data))
                ModelState.AddModelError("", "Ο Α.Μ. μπορεί να έχει μηδενική τιμή μόνο για ασκούμενους.");

            if (ModelState.IsValid)
            {
                personnelService.Create(data);
                newdata = personnelService.Refresh(data.PERSONNEL_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Personnel_Update([DataSourceRequest] DataSourceRequest request, PersonnelGridViewModel data)
        {
            var newdata = new PersonnelGridViewModel();

            if (!Kerberos.IsValidAM(data))
                ModelState.AddModelError("", "Ο Α.Μ. μπορεί να έχει μηδενική τιμή μόνο για ασκούμενους.");

            if (ModelState.IsValid)
            {
                personnelService.Update(data);
                newdata = personnelService.Refresh(data.PERSONNEL_ID);
            }

            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Personnel_Destroy([DataSourceRequest] DataSourceRequest request, PersonnelGridViewModel data)
        {
            if (data != null)
            {
                if (!Kerberos.CanDeletePerson(data.PERSONNEL_ID)) 
                    ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί ο εργαζόμενος διότι είναι σε χρήση σε τμήματα ή/και στο πρόγραμμα.");

                if (ModelState.IsValid)
                {
                    personnelService.Destroy(data);
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region EDUCATOR_TMIMA GRID CRUD

        public ActionResult EducatorTmima_Read([DataSourceRequest] DataSourceRequest request, int personId = 0)
        {
            IEnumerable<EducatorTmimaViewModel> data = educatorTmimaService.Read(personId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EducatorTmima_Create([DataSourceRequest] DataSourceRequest request, EducatorTmimaViewModel data, int personId = 0)
        {
            EducatorTmimaViewModel newdata = new EducatorTmimaViewModel();
            int stationId = Common.GetStationFromPersonID(personId);

            if (!Common.IsEducator(personId))
            {
                ModelState.AddModelError("", "Ο επιλεγμένος εργαζόμενος δεν είναι παιδαγωγός. Η καταχώρηση ακυρώθηκε.");
                return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }

            if (personId > 0 && stationId > 0)
            {
                var existingData = db.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ.Where(s => s.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ == personId && s.ΤΜΗΜΑ_ΚΩΔ == data.ΤΜΗΜΑ_ΚΩΔ).Count();
                if (existingData > 0) 
                    ModelState.AddModelError("", "Υπάρχει ήδη καταχώρηση του παιδαγωγού στο τμήμα αυτό.");

                if (data != null && ModelState.IsValid)
                {
                    educatorTmimaService.Create(data, personId, stationId);
                    newdata = educatorTmimaService.Refresh(data.RECORD_ID);
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
            int stationId = Common.GetStationFromPersonID(personId);

            if (!Common.IsEducator(personId))
            {
                ModelState.AddModelError("", "Ο επιλεγμένος εργαζόμενος δεν είναι παιδαγωγός. Η καταχώρηση ακυρώθηκε.");
                return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }

            if (personId > 0 && stationId > 0)
            {
                if (data != null && ModelState.IsValid)
                {
                    educatorTmimaService.Update(data, personId, stationId);
                    newdata = educatorTmimaService.Refresh(data.RECORD_ID);
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
                educatorTmimaService.Destroy(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PERSONNEL DATA FORM

        public ActionResult xPersonnelEdit(int personId)
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
            if (!(personId > 0))
            {
                string msg = "Ο κωδικός εργαζομένου δεν είναι έγκυρος. Η εγγραφή πρέπει να έχει αποθηκευτεί πρώτα.";
                return RedirectToAction("ErrorData", "Admin", new { notify = msg });
            }

            PersonnelViewModel person = personnelService.GetRecord(personId);
            if (person == null)
            {
                string msg = "Παρουσιάστηκε πρόβλημε εύρεσης του εργαζομένου.";
                return RedirectToAction("ErrorData", "Admin", new { notify = msg });
            }

            return View(person);
        }

        [HttpPost]
        public ActionResult xPersonnelEdit(int personId, PersonnelViewModel data)
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

            int stationId = Common.GetStationFromPersonID(personId);

            if (data != null && ModelState.IsValid)
            {
                ΠΡΟΣΩΠΙΚΟ entity = db.ΠΡΟΣΩΠΙΚΟ.Find(personId);
                if (entity != null)
                {
                    entity.ΜΗΤΡΩΟ = data.ΜΗΤΡΩΟ;
                    entity.ΑΦΜ = data.ΑΦΜ;
                    entity.ΒΝΣ = stationId;
                    entity.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ = data.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ;
                    entity.ΚΛΑΔΟΣ = data.ΚΛΑΔΟΣ;
                    entity.ΕΙΔΙΚΟΤΗΤΑ = data.ΕΙΔΙΚΟΤΗΤΑ;
                    entity.ΕΠΩΝΥΜΟ = data.ΕΠΩΝΥΜΟ.Trim();
                    entity.ΟΝΟΜΑ = data.ΟΝΟΜΑ.Trim();
                    entity.ΠΑΤΡΩΝΥΜΟ = data.ΠΑΤΡΩΝΥΜΟ != null ? data.ΠΑΤΡΩΝΥΜΟ.Trim() : data.ΠΑΤΡΩΝΥΜΟ;
                    entity.ΜΗΤΡΩΝΥΜΟ = data.ΜΗΤΡΩΝΥΜΟ != null ? data.ΜΗΤΡΩΝΥΜΟ.Trim() : data.ΜΗΤΡΩΝΥΜΟ;
                    entity.ΦΥΛΟ_ΚΩΔ = data.ΦΥΛΟ_ΚΩΔ;
                    entity.ΕΤΟΣ_ΓΕΝΝΗΣΗ = data.ΕΤΟΣ_ΓΕΝΝΗΣΗ;
                    entity.ΔΙΕΥΘΥΝΣΗ = data.ΔΙΕΥΘΥΝΣΗ;
                    entity.ΤΗΛΕΦΩΝΑ = data.ΤΗΛΕΦΩΝΑ;
                    entity.EMAIL = data.EMAIL;
                    entity.ΗΛΙΚΙΑ = Common.CalculatePersonAge(data);
                    entity.ΒΑΘΜΟΣ = data.ΒΑΘΜΟΣ;
                    entity.ΑΠΟΦΑΣΗ_ΦΕΚ = data.ΑΠΟΦΑΣΗ_ΦΕΚ;
                    entity.ΠΑΡΑΤΗΡΗΣΕΙΣ = data.ΠΑΡΑΤΗΡΗΣΕΙΣ;
                    entity.ΑΠΟΧΩΡΗΣΕ = data.ΑΠΟΧΩΡΗΣΕ;
                    entity.ΑΠΟΧΩΡΗΣΗ_ΗΜΝΙΑ = data.ΑΠΟΧΩΡΗΣΗ_ΗΜΝΙΑ;
                    entity.ΑΠΟΧΩΡΗΣΗ_ΑΙΤΙΑ = data.ΑΠΟΧΩΡΗΣΗ_ΑΙΤΙΑ;

                    db.Entry(entity).State = EntityState.Modified;
                    string ErrorMsg = Kerberos.ValidatePersonFields(entity);
                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        db.SaveChanges();
                        this.ShowMessage(MessageType.Success, "Η αποθήκευση ολοκληρώθηκε με επιτυχία.");
                        PersonnelViewModel newPerson = personnelService.GetRecord(personId);
                        return View(newPerson);
                    }
                    else
                    {
                        this.ShowMessage(MessageType.Error, "Η αποθήκευση απέτυχε λόγω επικύρωσης δεδομένων. " + ErrorMsg);
                        return View(data);
                    }
                }
            }
            this.ShowMessage(MessageType.Error, "Η αποθήκευση ακυρώθηκε λόγω σφαλμάτων καταχώρησης.");
            return View(data);
        }

        #endregion

        #endregion


        #region ΜΗΤΡΩΟ ΠΡΟΣΩΠΙΚΟΥ

        public ActionResult xPersonnelInfoList(string notify = null)
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

            IEnumerable<sqlPersonInfoViewModel> data = GetPersonnelInfo();
            if (data.Count() == 0)
            {
                string msg = "Δεν υπάρχουν καταχωρημένοι εργαζόμενοι για την εμφάνιση του μητρώου.";
                return RedirectToAction("Index", "Admin", new { notify = msg });
            }
            sqlPersonInfoViewModel person = data.First();

            if (notify != null)
            {
                this.ShowMessage(MessageType.Info, notify);
            }
            return View(person);
        }

        public ActionResult PersonnelInfo_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<sqlPersonInfoViewModel> data = GetPersonnelInfo();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EducatorTmimaInfo_Read(int personId, [DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<sqlEducatorTmimaInfoViewModel> data = GetEducatorTmimaInfo(personId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public List<sqlPersonInfoViewModel> GetPersonnelInfo()
        {
            var data = (from d in db.sqlPERSONNEL_INFO
                        orderby d.ΒΝΣ_ΕΠΩΝΥΜΙΑ, d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ
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
            sqlPersonInfoViewModel data = GetPersonnelInfo().Where(e => e.PERSONNEL_ID == personId).FirstOrDefault();

            return PartialView("xPersonnelInfoPartial", data);
        }

        #endregion


        #region ΑΝΑΘΕΣΕΙΣ ΠΑΙΔΑΓΩΓΩΝ

        public ActionResult xAnatheseisData(string notify = null)
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

            if (!SchoolYearsExist() || !EducatorsExist())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει πρώτα να καταχωρηθούν σχολικά έτη και παιδαγωγοί.";
                return RedirectToAction("Index", "Admin", new { notify = msg });
            }

            PopulateEducators();
            PopulateSchoolYears();
            return View();
        }

        #region TMIMA-EDUCATOR GRID CRUD

        public ActionResult TmimaEducator_Read([DataSourceRequest] DataSourceRequest request, int tmimaId = 0)
        {
            List<EducatorTmimaViewModel> data = tmimaService.GetEducators(tmimaId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TmimaEducator_Create([DataSourceRequest] DataSourceRequest request, EducatorTmimaViewModel data, int tmimaId = 0)
        {
            EducatorTmimaViewModel newdata = new EducatorTmimaViewModel();

            TmimaViewModel tmima = tmimaService.GetRecord(tmimaId);

            if (tmimaId > 0 && tmima != null)
            {
                var existingData = db.ΤΜΗΜΑ_ΠΑΙΔΑΓΩΓΟΣ.Where(s => s.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ == data.ΠΑΙΔΑΓΩΓΟΣ_ΚΩΔ && s.ΤΜΗΜΑ_ΚΩΔ == tmimaId).Count();
                if (existingData > 0) 
                    ModelState.AddModelError("", "Υπάρχει ήδη καταχώρηση του παιδαγωγού στο τμήμα αυτό.");

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
                    data.RECORD_ID = entity.RECORD_ID;
                    newdata = educatorTmimaService.Refresh(entity.RECORD_ID);
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

            TmimaViewModel tmima = tmimaService.GetRecord(tmimaId);

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
                    newdata = educatorTmimaService.Refresh(entity.RECORD_ID);
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

        public ActionResult xPersonnelMetaboles(string notify = null)
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

            if (!StationsExist() || !PersonnelTypesExist() || !MetabolesTypesExist() || !SchoolYearsExist())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει πρώτα να ορίσετε ΒΝΣ, κατηγορίες προσωπικού, είδη μεταβολών και σχολικά έτη στις Ρυθμίσεις.";
                return RedirectToAction("Index", "Admin", new { notify = msg });
            }

            PopulateStations();
            PopulatePersonnelTypes();
            PopulateMetabolesTypes();
            return View();
        }

        #region GRID CRUD FUNCTIONS

        public ActionResult Metaboli_Read([DataSourceRequest] DataSourceRequest request, int schoolyearId = 0, int personId = 0)
        {
            IEnumerable<PersonnelMetaboliViewModel> data = metabolesService.Read(schoolyearId, personId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Metaboli_Create([DataSourceRequest] DataSourceRequest request, PersonnelMetaboliViewModel data, int schoolyearId = 0, int personId = 0)
        {
            var newdata = new PersonnelMetaboliViewModel();

            if (!(schoolyearId > 0) || !(personId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε εργαζόμενο και σχολικό έτος. Η καταχώρηση ακυρώθηκε.");

            if (data.ΗΜΝΙΑ_ΑΠΟ > data.ΗΜΝΙΑ_ΕΩΣ)
                ModelState.AddModelError("", "Η αρχική ημερομηνία δεν πρέπει να είναι μεγαλύτερη της τελικής. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                metabolesService.Create(data, schoolyearId, personId);
                newdata = metabolesService.Refresh(data.ΜΕΤΑΒΟΛΗ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Metaboli_Update([DataSourceRequest] DataSourceRequest request, PersonnelMetaboliViewModel data, int schoolyearId = 0, int personId = 0)
        {
            var newdata = new PersonnelMetaboliViewModel();

            if (!(schoolyearId > 0) || !(personId > 0)) 
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε εργαζόμενο και σχολικό έτος. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                metabolesService.Update(data, schoolyearId, personId);
                newdata = metabolesService.Refresh(data.ΜΕΤΑΒΟΛΗ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Metaboli_Destroy([DataSourceRequest] DataSourceRequest request, PersonnelMetaboliViewModel data)
        {
            if (data != null)
            {
                metabolesService.Destroy(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult xMetabolesPrint()
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

        #endregion


        #region --- ΜΕΤΑΒΟΛΕΣ ΜΗΝΙΑΙΑ ΕΚΘΕΣΗ ---

        public ActionResult xMetabolesMonthRep(string notify = null)
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

            if (!PersonnelExists() || !MonthsExist() || !SchoolYearsExist())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει πρώτα να καταχωρηθούν εργαζόμενοι, μήνες και σχολικά έτη στις Ρυθμίσεις.";
                return RedirectToAction("Index", "Admin", new { notify = msg });
            }

            PopulateEmployees();
            PopulateSchoolYears();
            PopulateMonths();
            return View();
        }

        #region GRID CRUD FUNCTIONS

        public ActionResult MetaboliReport_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0, int schoolyearId = 0, int monthId = 0)
        {
            List<MetabolesReportViewModel> data = metabolesReportService.Read(stationId, schoolyearId, monthId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MetaboliReport_Create([DataSourceRequest] DataSourceRequest request, MetabolesReportViewModel data, int stationId = 0, int schoolyearId = 0, int monthId = 0)
        {
            var newdata = new MetabolesReportViewModel();

            if (!(stationId > 0) || !(schoolyearId > 0) || !(monthId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε σταθμό, σχολικό έτος και μήνα. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                metabolesReportService.Create(data, stationId, schoolyearId, monthId);
                newdata = metabolesReportService.Refresh(data.RECORD_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MetaboliReport_Update([DataSourceRequest] DataSourceRequest request, MetabolesReportViewModel data, int stationId = 0, int schoolyearId = 0, int monthId = 0)
        {
            var newdata = new MetabolesReportViewModel();

            if (!(stationId > 0) || !(schoolyearId > 0) || !(monthId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε σταθμό, σχολικό έτος και μήνα. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                metabolesReportService.Update(data, stationId, schoolyearId, monthId);
                newdata = metabolesReportService.Refresh(data.RECORD_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MetaboliReport_Destroy([DataSourceRequest] DataSourceRequest request, MetabolesReportViewModel data)
        {
            if (data != null)
            {
                metabolesReportService.Destroy(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region ΛΕΙΤΟΥΡΓΙΕΣ ΔΗΜΙΟΥΡΓΙΑΣ-ΕΝΗΜΕΡΩΣΗΣ-ΕΚΤΥΠΩΣΗΣ ΠΙΝΑΚΑ ΜΗΝΙΑΙΩΝ ΜΕΤΑΒΟΛΩΝ

        public ActionResult CreateMetabolesTable(int stationId, int schoolyearId, int monthId)
        {
            string msg = "Η δημιουργία του πίνακα μηνιαίων μεταβολών του προσωπικού ολοκληρώθηκε.";

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
                foreach(var person in source1) 
                {
                    METABOLES_REPORT entity = new METABOLES_REPORT()
                    {
                        SCHOOL_YEAR = schoolyearId,
                        METABOLI_MONTH = monthId,
                        BNS = person.ΒΝΣ,
                        EMPLOYEE_ID = person.PERSONNEL_ID,
                        METABOLI_DAYS = Common.GenerateAbsenceDays(person.PERSONNEL_ID, schoolyearId, monthId),
                        METABOLI_YEAR = Common.GenerateMetabolesYear(person.PERSONNEL_ID, schoolyearId, monthId),
                        METABOLI_TEXT = Common.GenerateMetabolesText(person.PERSONNEL_ID, schoolyearId, monthId)
                    };
                    db.METABOLES_REPORT.Add(entity);
                    db.SaveChanges();
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateMetabolesTable(int stationId, int schoolyearId, int monthId)
        {
            string msg = "Η ενημέρωση του πίνακα μηνιαίων μεταβολών του προσωπικού ολοκληρώθηκε.";

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
                        METABOLI_DAYS = Common.GenerateAbsenceDays(person.PERSONNEL_ID, schoolyearId, monthId),
                        METABOLI_YEAR = Common.GenerateMetabolesYear(person.PERSONNEL_ID, schoolyearId, monthId),
                        METABOLI_TEXT = Common.GenerateMetabolesText(person.PERSONNEL_ID, schoolyearId, monthId)
                    };
                    db.METABOLES_REPORT.Add(entity);
                    db.SaveChanges();
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DestroyMetabolesTable(int stationId, int schoolyearId, int monthId)
        {
            string msg = "Η διαγραφή του πίνακα μεταβολών για το σχολικό έτος και μήνα ολοκληρώθηκε.";

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

        public ActionResult xMetabolesMonthPrint(int stationId, int schoolyearId, int monthId)
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

        #endregion


        #region --- ΩΡΟΛΟΓΙΟ ΠΡΟΓΡΑΜΜΑ ΠΡΟΣΩΠΙΚΟΥ ---


        #region ΩΡΟΛΟΓΙΟ ΠΡΟΓΡΑΜΜΑ (24-7-2019)

        public ActionResult xProgrammaDay(string notify = null)
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

            if (!HoursExist() || !PersonnelExists())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει πρώτα να καταχωρηθούν το προσωπικό και οι ώρες προσέλευσης,αποχώρησης.";
                return RedirectToAction("Index", "Admin", new { notify = msg });
            }

            PopulatePersonnel();
            PopulateHours();
            return View();
        }

        public ActionResult Stations_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = GetStationData();

            var result = new JsonResult();
            result.Data = data.ToDataSourceResult(request);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public List<sqlStationSelectorViewModel> GetStationData()
        {
            var data = (from d in db.sqlSTATION_SELECTOR
                        orderby d.ΠΕΡΙΦΕΡΕΙΑΚΗ, d.ΕΠΩΝΥΜΙΑ
                        select new sqlStationSelectorViewModel
                        {
                            ΣΤΑΘΜΟΣ_ΚΩΔ = d.ΣΤΑΘΜΟΣ_ΚΩΔ,
                            ΕΠΩΝΥΜΙΑ = d.ΕΠΩΝΥΜΙΑ,
                            ΕΠΩΝΥΜΙΑ_ΠΕΡΙΦΕΡΕΙΑ = d.ΕΠΩΝΥΜΙΑ_ΠΕΡΙΦΕΡΕΙΑ,
                            ΥΠΕΥΘΥΝΟΣ = d.ΥΠΕΥΘΥΝΟΣ,
                            ΠΕΡΙΦΕΡΕΙΑΚΗ = d.ΠΕΡΙΦΕΡΕΙΑΚΗ
                        }).ToList();

            return (data);
        }

        #region PROGRAMMA GRID FUNCTIONS

        public ActionResult Programma_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0, DateTime? theDate = null)
        {
            IEnumerable<ProgrammaDayViewModel> data = programmaService.Read(theDate, stationId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Programma_Create([DataSourceRequest] DataSourceRequest request,
                           [Bind(Prefix = "models")]IEnumerable<ProgrammaDayViewModel> data, int stationId = 0, DateTime? theDate = null)
        {
            int CurrentSchoolYear = Common.GetCurrentSchoolYear();

            if (stationId == 0 || theDate == null)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σταθμό και ημερομηνία. Η διαδικασία ακυρώθηκε.");
                return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }
            if (!Common.DateInCurrentSchoolYear((DateTime)theDate))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    item.PROGRAMMA_DATE = theDate;
                    item.STATION_ID = stationId;
                    item.PROGRAMMA_MONTH = theDate.Value.Month;
                    item.SCHOOLYEARID = CurrentSchoolYear;

                    if (Common.ProgrammaItemExists(item))
                    {
                        ModelState.AddModelError("", "Έγινε απόπειρα δημιουργίας διπλοεγγραφής. Η διπλή καταχώρηση ακυρώθηκε.");
                        return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                    }
                    string err_msg = Common.ValidateProgramma(item);
                    if (err_msg != "")
                    {
                        ModelState.AddModelError("", err_msg);
                        return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                    }
                    programmaService.Create(item, theDate, CurrentSchoolYear, stationId);
                }
            }
            return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Programma_Update([DataSourceRequest] DataSourceRequest request,
                            [Bind(Prefix = "models")]IEnumerable<ProgrammaDayViewModel> data, int stationId = 0, DateTime? theDate = null)
        {
            int CurrentSchoolYear = Common.GetCurrentSchoolYear();

            if (stationId == 0 || theDate == null)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σταθμό και ημερομηνία. Η διαδικασία ακυρώθηκε.");
                return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }
            if (!Common.DateInCurrentSchoolYear((DateTime)theDate))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    string err_msg = Common.ValidateProgramma(item);
                    if (err_msg != "")
                    {
                        ModelState.AddModelError("", err_msg);
                        return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                    }
                    programmaService.Update(item, theDate, CurrentSchoolYear, stationId);
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
                    programmaService.Destroy(item);
                }
            }
            return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PROGRAMMA AUTOMATIONS

        public ActionResult LoadStationPersonnel(ProgrammaParameters p)
        {
            string msg = "";

            int CurrentSchoolYear = Common.GetCurrentSchoolYear();

            if (p.stationId == 0 || p.theDate == null)
            {
                msg = "Δεν έχει γίνει επιλογή ενός βρεφονηπιακού σταθμού ή/και ημερομηνίας.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            if (!Common.DateInCurrentSchoolYear(p.theDate))
            {
                msg = "Η ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            var srcData = (from d in db.ΠΡΟΣΩΠΙΚΟ where d.ΒΝΣ == p.stationId orderby d.ΕΠΩΝΥΜΟ, d.ΟΝΟΜΑ select d).ToList();
            if (srcData.Count > 0)
            {
                foreach (var item in srcData)
                {
                    // first check if record already exists
                    if (Common.CanCreatePersonProgramma(p.stationId, item.PERSONNEL_ID, p.theDate))
                    {
                        ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ target = new ΠΡΟΓΡΑΜΜΑ_ΗΜΕΡΑ()
                        {
                            STATION_ID = p.stationId,
                            PROGRAMMA_DATE = p.theDate,
                            SCHOOLYEARID = CurrentSchoolYear,
                            PERSON_ID = item.PERSONNEL_ID,
                            PROGRAMMA_MONTH = p.theDate.Month,
                            HOUR_START = Common.GetFirstHour(),
                            HOUR_END = Common.GetLastHour()
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

            if (!Common.DateInCurrentSchoolYear(p.theDate))
            {
                msg = "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            int stationId = p.stationId;
            DateTime srcDate = p.theDate;
            DateTime targetDate = p.theDate.AddDays(1);

            if (targetDate.DayOfWeek == DayOfWeek.Saturday) targetDate = targetDate.AddDays(2);
            else if (targetDate.DayOfWeek == DayOfWeek.Sunday) targetDate = targetDate.AddDays(1);

            if (!Common.DateInCurrentSchoolYear(targetDate))
            {
                msg = "Οι ημερομηνία προορισμού είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            var targetData = programmaService.Read(targetDate, stationId);
            if (targetData.Count() > 0)
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
                msg = "Δεν βρέθηκε καταχωρημένο ωρολόγιο πρόγραμμα του ΒΝΣ στην ημερομηνία προέλευσης. Δεν υπάρχουν δεδομένα για μεταφορά.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            };

            int CurrentSchoolYear = Common.GetCurrentSchoolYear();

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

            if (!Common.DateInCurrentSchoolYear(targetStartDate) || !Common.DateInCurrentSchoolYear(targetFinalDate))
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
            int CurrentSchoolYear = Common.GetCurrentSchoolYear();

            foreach (var item in srcData)
            {
                DateTime _targetDate = item.PROGRAMMA_DATE.Value.AddDays(7);
                if(Common.CanCreatePersonProgramma(p.stationId, (int)item.PERSON_ID, _targetDate))
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

        public ActionResult xProgrammaDayCheck(string notify = null)
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

            if (!HoursExist() || !PersonnelExists())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει πρώτα να καταχωρηθούν το προσωπικό και οι ώρες προσέλευσης,αποχώρησης.";
                return RedirectToAction("Index", "Admin", new { notify = msg });
            }

            PopulatePersonnel();
            PopulateHours();
            return View();
        }

        #region GRID CRUD FUNCTIONS

        public ActionResult Programma2_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {
            IEnumerable<ProgrammaDayViewModel> data = programmaService.Read(theDate1, theDate2, stationId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Programma2_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ProgrammaDayViewModel> data, int stationId = 0)
        {
            int CurrentSchoolYear = Common.GetCurrentSchoolYear();

            if (stationId > 0)
            {
                if (data != null && ModelState.IsValid)
                {
                    foreach (var item in data)
                    {
                        item.STATION_ID = stationId;
                        item.PROGRAMMA_MONTH = item.PROGRAMMA_DATE.Value.Month;
                        item.SCHOOLYEARID = CurrentSchoolYear;

                        if (Common.ProgrammaItemExists(item))
                        {
                            ModelState.AddModelError("", "Έγινε απόπειρα δημιουργίας διπλοεγγραφής. Η διπλή καταχώρηση ακυρώθηκε.");
                            return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                        }
                        string err_msg = Common.ValidateProgramma(item);
                        if (err_msg != "")
                        {
                            ModelState.AddModelError("", err_msg);
                            return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                        }
                        programmaService.Create(item, CurrentSchoolYear, stationId);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Πρέπει πρώτα να γίνει επιλογή σταθμού. Η αποθήκευση ακυρώθηκε.");
            }
            return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Programma2_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ProgrammaDayViewModel> data, int stationId = 0)
        {
            int CurrentSchoolYear = Common.GetCurrentSchoolYear();

            if (stationId > 0)
            {
                if (data != null && ModelState.IsValid)
                {
                    foreach (var item in data)
                    {
                        string err_msg = Common.ValidateProgramma(item);
                        if (err_msg != "")
                        {
                            ModelState.AddModelError("", err_msg);
                            return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                        }
                        programmaService.Update(item, CurrentSchoolYear, stationId);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Πρέπει πρώτα να γίνει επιλογή σταθμού. Η αποθήκευση ακυρώθηκε.");
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

        #endregion GRID CRUD FUNCTIONS

        public ActionResult DeleteProgramma(int stationId = 0, string theDate1 = null, string theDate2 = null)
        {
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

        public ActionResult xPersonMonthHoursPrint(int stationId = 0)
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

        #endregion


        #region --- ΠΑΡΟΥΣΙΟΛΟΓΙΟ ΠΑΙΔΙΩΝ ---

        #region ΠΑΡΟΥΣΙΕΣ ΠΑΙΔΙΩΝ

        public ActionResult xChildrenParousies(string notify = null)
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

            if (!TmimaChildrenExist() || !SchoolYearsExist())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει να υπάρχουν εγγραφές παιδιών σε τμήματα και καταχωρημένα σχολικά έτη.";
                return RedirectToAction("Index", "Admin", new { notify = msg });
            }

            PopulateChildren();
            PopulateSchoolYears();
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

        #region PAROUSIES GRID FUNCTIONS

        public ActionResult Parousies_Read([DataSourceRequest] DataSourceRequest request, int tmimaId = 0, DateTime? theDate = null)
        {
            IEnumerable<ChildParousiaViewModel> data = parousiaService.Read(tmimaId, theDate);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Parousies_Create([DataSourceRequest] DataSourceRequest request, 
                    [Bind(Prefix = "models")]IEnumerable<ChildParousiaViewModel> data, int tmimaId = 0, DateTime? theDate = null)
        {
            int stationId = 0;
            int schoolyearId = 0;

            if (tmimaId == 0 || theDate == null)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει τμήμα και ημερομηνία. Η διαδικασία ακυρώθηκε.");
                return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }
            var TmimaData = tmimaService.GetRecord(tmimaId);
            if (TmimaData != null)
            {
                stationId = (int)TmimaData.ΒΝΣ;
                schoolyearId = (int)TmimaData.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
            }
            if (!Common.DateInSelectedSchoolYear((DateTime)theDate, schoolyearId))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του σχολικού έτους του τμήματος. Η διαδικασία ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    item.STATION_ID = stationId;
                    item.SCHOOLYEARID = schoolyearId;
                    item.TMIMA_ID = tmimaId;
                    item.PAROUSIA_DATE = theDate;
                    item.PAROUSIA_MONTH = theDate.Value.Month;

                    if (Common.ParousiaItemExists(item))
                    {
                        ModelState.AddModelError("", "Έγινε απόπειρα δημιουργίας διπλοεγγραφής. Η διπλή καταχώρηση ακυρώθηκε.");
                        return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
                    }
                    parousiaService.Create(item, tmimaId, theDate, schoolyearId, stationId);
                }
            }
            return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Parousies_Update([DataSourceRequest] DataSourceRequest request,
                    [Bind(Prefix = "models")]IEnumerable<ChildParousiaViewModel> data, int tmimaId = 0, DateTime? theDate = null)
        {
            int stationId = 0;
            int schoolyearId = 0;

            if (tmimaId == 0 || theDate == null)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει τμήμα και ημερομηνία. Η διαδικασία ακυρώθηκε.");
                return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }
            var TmimaData = tmimaService.GetRecord(tmimaId);
            if (TmimaData != null)
            {
                stationId = (int)TmimaData.ΒΝΣ;
                schoolyearId = (int)TmimaData.ΣΧΟΛΙΚΟ_ΕΤΟΣ;
            }
            if (!Common.DateInSelectedSchoolYear((DateTime)theDate, schoolyearId))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του σχολικού έτους του τμήματος. Η διαδικασία ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    parousiaService.Update(item, tmimaId, theDate, schoolyearId, stationId);
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
                    parousiaService.Destroy(item);
                }
            }
            return Json(data.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
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
            var TmimaData = tmimaService.GetRecord(p.tmimaId);
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
                    if (Common.CanCreateChildParousia(p.tmimaId, item.CHILD_ID, p.theDate))
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

        #endregion

        #endregion


        #region ΜΗΝΙΑΙΕΣ ΚΑΤΑΣΤΑΣΕΙΣ ΠΑΡΟΥΣΙΩΝ ΒΡΕΦΟΝΗΠΙΩΝ

        public ActionResult xParousiesMonths(string notify = null)
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

            if (!SchoolYearsExist() || !ParousiesMonthExist())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει να έχουν καταχωρηθεί παρουσίες των παιδιών η/και καταχωρημένα σχολικά έτη.";
                return RedirectToAction("Index", "Admin", new { notify = msg });
            }

            PopulateSchoolYears();
            return View();
        }

        public ActionResult ParousiesMonth_Read([DataSourceRequest] DataSourceRequest request, int tmimaId = 0, int monthId = 0)
        {
            IEnumerable<ParousiesMonthViewModel> data = parousiaService.ReadMonth(tmimaId, monthId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
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
                foreach(var item in source) 
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

        public ActionResult xParousiesMonthPrint(int tmimaId, int monthId)
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
        public ActionResult xParousiesMonthPrint2(int tmimaId)
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

        public ActionResult xApousiesDetail(string notify = null)
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

            if (!SchoolYearsExist() || !ParousiesMonthExist())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει να έχουν καταχωρηθεί παρουσίες των παιδιών η/και καταχωρημένα σχολικά έτη.";
                return RedirectToAction("Index", "Admin", new { notify = msg });
            }

            PopulateSchoolYears();
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

        public ActionResult xApousiesDetailPrint(int tmimaId, int monthId)
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

        public ActionResult xApousiesSum(string notify = null)
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

            if (!SchoolYearsExist() || !ParousiesMonthExist())
            {
                string msg = "Για φόρτωση της σελίδας πρέπει να έχουν καταχωρηθεί παρουσίες των παιδιών η/και καταχωρημένα σχολικά έτη.";
                return RedirectToAction("Index", "Admin", new { notify = msg });
            }

            PopulateSchoolYears();
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

        public ActionResult xApousiesSumPrint(int tmimaId, int monthId)
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

        #endregion


        #region ΓΕΥΜΑΤΑ (ΠΡΩΙΝΟ, ΜΕΣΗΜΕΡΙΑΝΟ, ΒΡΕΦΙΚΟ)

        public ActionResult xMealsList(string notify = null)
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
            return View();
        }

        #region ΠΛΕΓΜΑ ΠΡΩΙΝΩΝ ΓΕΥΜΑΤΩΝ

        public ActionResult MealMorning_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0)
        {
            List<MealMorningViewModel> data = mealMorningService.Read(stationId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealMorning_Create([DataSourceRequest] DataSourceRequest request, MealMorningViewModel data, int stationId = 0)
        {
            MealMorningViewModel newdata = new MealMorningViewModel();

            if (stationId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σταθμό. Η διαδικασία ακυρώθηκε.");
            }

            if (data != null && ModelState.IsValid)
            {
                mealMorningService.Create(data, stationId);
                newdata = mealMorningService.Refresh(data.ΠΡΩΙΝΟ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealMorning_Update([DataSourceRequest] DataSourceRequest request, MealMorningViewModel data, int stationId = 0)
        {
            MealMorningViewModel newdata = new MealMorningViewModel();

            if (stationId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σταθμό. Η διαδικασία ακυρώθηκε.");
            }

            if (data != null && ModelState.IsValid)
            {
                mealMorningService.Update(data, stationId);
                newdata = mealMorningService.Refresh(data.ΠΡΩΙΝΟ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealMorning_Destroy([DataSourceRequest] DataSourceRequest request, MealMorningViewModel data)
        {
            if (!Kerberos.CanDeleteMealMorning(data.ΠΡΩΙΝΟ_ΚΩΔ))
                ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί το γεύμα διότι χρησιμοποιείται ήδη στα διαιτολόγια.");

            if (ModelState.IsValid)
            {
                mealMorningService.Destroy(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ΠΛΕΓΜΑ ΜΕΣΗΜΕΡΙΑΝΩΝ ΓΕΥΜΑΤΩΝ

        public ActionResult MealNoon_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0)
        {
            List<MealNoonViewModel> data = mealNoonService.Read(stationId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealNoon_Create([DataSourceRequest] DataSourceRequest request, MealNoonViewModel data, int stationId = 0)
        {
            MealNoonViewModel newdata = new MealNoonViewModel();

            if (stationId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σταθμό. Η διαδικασία ακυρώθηκε.");
            }

            if (data != null && ModelState.IsValid)
            {
                mealNoonService.Create(data, stationId);
                newdata = mealNoonService.Refresh(data.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealNoon_Update([DataSourceRequest] DataSourceRequest request, MealNoonViewModel data, int stationId = 0)
        {
            MealNoonViewModel newdata = new MealNoonViewModel();

            if (stationId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σταθμό. Η διαδικασία ακυρώθηκε.");
            }

            if (data != null && ModelState.IsValid)
            {
                mealNoonService.Update(data, stationId);
                newdata = mealNoonService.Refresh(data.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealNoon_Destroy([DataSourceRequest] DataSourceRequest request, MealNoonViewModel data)
        {
            if (!Kerberos.CanDeleteMealNoon(data.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ))
                ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί το γεύμα διότι χρησιμοποιείται ήδη στα διαιτολόγια.");

            if (ModelState.IsValid)
            {
                mealNoonService.Destroy(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ΠΛΕΓΜΑ ΒΡΕΦΙΚΩΝ ΓΕΥΜΑΤΩΝ

        public ActionResult MealBaby_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0)
        {
            List<MealBabyViewModel> data = mealBabyService.Read(stationId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealBaby_Create([DataSourceRequest] DataSourceRequest request, MealBabyViewModel data, int stationId = 0)
        {
            MealBabyViewModel newdata = new MealBabyViewModel();

            if (stationId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σταθμό. Η διαδικασία ακυρώθηκε.");
            }

            if (data != null && ModelState.IsValid)
            {
                mealBabyService.Create(data, stationId);
                newdata = mealBabyService.Refresh(data.ΒΡΕΦΙΚΟ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealBaby_Update([DataSourceRequest] DataSourceRequest request, MealBabyViewModel data, int stationId = 0)
        {
            MealBabyViewModel newdata = new MealBabyViewModel();

            if (stationId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σταθμό. Η διαδικασία ακυρώθηκε.");
            }

            if (data != null && ModelState.IsValid)
            {
                mealBabyService.Update(data, stationId);
                newdata = mealBabyService.Refresh(data.ΒΡΕΦΙΚΟ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealBaby_Destroy([DataSourceRequest] DataSourceRequest request, MealBabyViewModel data)
        {
            if (!Kerberos.CanDeleteMealBaby(data.ΒΡΕΦΙΚΟ_ΚΩΔ))
                ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί το γεύμα διότι χρησιμοποιείται ήδη στα διαιτολόγια.");

            if (ModelState.IsValid)
            {
                mealBabyService.Destroy(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion


        #region ΔΙΑΙΤΟΛΟΓΙΑ ΣΤΑΘΜΩΝ

        public ActionResult xMealsPlanList(string notify = null)
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

            if (!Kerberos.CanPopulateMeals())
            {
                string msg = "Η επεξεργασία διαιτολογίων είναι αδύνατη διότι δεν υπάρχουν καταχωρημένα γεύματα.";
                return RedirectToAction("Index", "Admin", new { notify = msg });
            }

            if (notify != null) this.ShowMessage(MessageType.Warning, notify);

            PopulateMealsMorning();
            PopulateMealsNoon();
            PopulateMealsBaby();

            return View();
        }

        public ActionResult MealPlan_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0)
        {
            IEnumerable<DiaitologioViewModel> data = mealPlanService.Read(stationId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealPlan_Create([DataSourceRequest] DataSourceRequest request, DiaitologioViewModel data, int stationId = 0)
        {
            DiaitologioViewModel newdata = new DiaitologioViewModel();

            if (stationId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σταθμό. Η διαδικασία ακυρώθηκε.");
            }

            if (data != null && ModelState.IsValid)
            {
                mealPlanService.Create(data, stationId);
                newdata = mealPlanService.Refresh(data.ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealPlan_Update([DataSourceRequest] DataSourceRequest request, DiaitologioViewModel data, int stationId = 0)
        {
            DiaitologioViewModel newdata = new DiaitologioViewModel();

            if (stationId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σταθμό. Η διαδικασία ακυρώθηκε.");
            }

            if (data != null && ModelState.IsValid)
            {
                mealPlanService.Update(data, stationId);
                newdata = mealPlanService.Refresh(data.ΔΙΑΙΤΟΛΟΓΙΟ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MealPlan_Destroy([DataSourceRequest] DataSourceRequest request, DiaitologioViewModel data)
        {
            if (ModelState.IsValid)
            {
                mealPlanService.Destroy(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }


        #region CHILD GRID WITH PERSONS AND FEEDING COST

        public ActionResult SumPersonsFood_Get([DataSourceRequest] DataSourceRequest request, int stationId = 0, DateTime? theDate = null)
        {
            List<SumPersonsTrofeioViewModel> data = mealPlanService.Aggregate(stationId, theDate);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult xDiaitologioDayPrint(int stationId, string theDate)
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

            MealPlanParameters mpp = new MealPlanParameters();
            mpp.STATION_ID = stationId;
            mpp.DATE_MEALPLAN = theDate;

            return View(mpp);
        }

        public ActionResult xMealsPrint(int stationId)
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

            MealPlanParameters mpp = new MealPlanParameters();
            mpp.STATION_ID = stationId;
            mpp.DATE_MEALPLAN = "";

            return View(mpp);
        }

        #endregion


        #region ΔΙΑΙΤΟΛΟΓΙΑ ΕΥΡΕΤΗΡΙΟ

        public ActionResult xMealsPlanSearch(string notify = null)
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

            return View();
        }

        public ActionResult MealPlanSearch_Read([DataSourceRequest] DataSourceRequest request, int? stationId, DateTime? theDate1, DateTime? theDate2)
        {
            List<sqlMealPlanViewModel> data = mealPlanService.Search(stationId, theDate1, theDate2);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult xMealPlanPrint(int stationId, string theDate1, string theDate2)
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

            DiaitologioParameters dp = new DiaitologioParameters();
            dp.STATION_ID = stationId;
            dp.DATE_START = theDate1;
            dp.DATE_END = theDate2;

            return View(dp);
        }

        #endregion


        #region ΔΑΠΑΝΕΣ ΤΡΟΦΕΙΟΥ, ΚΑΘΑΡΙΟΤΗΤΑΣ ΚΑΙ ΓΕΝΙΚΩΝ

        #region ΠΑΡΑΜΕΤΡΟΙ (ΤΡΟΦΕΙΟ/ΑΤΟΜΟ)

        public ActionResult xPersonCostDay(string notify = null)
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

            PopulateStations();
            return View();
        }

        public ActionResult PersonCost_Read([DataSourceRequest] DataSourceRequest request, int schoolyearId = 0)
        {
            IEnumerable<PersonCostDayViewModel> data = personCostService.Read(schoolyearId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PersonCost_Create([DataSourceRequest] DataSourceRequest request, PersonCostDayViewModel data, int schoolyearId = 0)
        {
            var newdata = new PersonCostDayViewModel();

            if (!(schoolyearId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε σχολικό έτος. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                personCostService.Create(data, schoolyearId);
                newdata = personCostService.Refresh(data.ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PersonCost_Update([DataSourceRequest] DataSourceRequest request, PersonCostDayViewModel data, int schoolyearId = 0)
        {
            var newdata = new PersonCostDayViewModel();

            if (!(schoolyearId > 0))
                ModelState.AddModelError("", "Πρέπει πρώτα να επιλέξετε σχολικό έτος. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                personCostService.Update(data, schoolyearId);
                newdata = personCostService.Refresh(data.ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PersonCost_Destroy([DataSourceRequest] DataSourceRequest request, PersonCostDayViewModel data)
        {
            if (data != null)
            {
                if (ModelState.IsValid)
                {
                    personCostService.Destroy(data);
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult TransfeCostPerson(int schoolyearId)
        {
            string msg = personCostService.Transfer(schoolyearId);

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region ΠΑΡΑΜΕΤΡΟΙ - ΚΑΤΗΓΟΡΙΕΣ ΓΕΝΙΚΩΝ ΔΑΠΑΝΩΝ (03-02-2020)

        public ActionResult xExtraCategories()
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
            PopulateExpenseTypes();
            return View();
        }

        public ActionResult ExtraCategory_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<ExtraCategoryViewModel> data = extraCategoryService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ExtraCategory_Create([DataSourceRequest] DataSourceRequest request, ExtraCategoryViewModel data)
        {
            var newdata = new ExtraCategoryViewModel();

            var existingdata = db.ΕΞΤΡΑ_ΚΑΤΗΓΟΡΙΑ.Where(s => s.ΚΑΤΗΓΟΡΙΑ == data.ΚΑΤΗΓΟΡΙΑ).Count();
            if (existingdata > 0) 
                ModelState.AddModelError("", "Αυτή η κατηγορία δαπάνης υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                extraCategoryService.Create(data);
                newdata = extraCategoryService.Refresh(data.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ExtraCategory_Update([DataSourceRequest] DataSourceRequest request, ExtraCategoryViewModel data)
        {
            var newdata = new ExtraCategoryViewModel();

            if (data != null && ModelState.IsValid)
            {
                extraCategoryService.Update(data);
                newdata = extraCategoryService.Refresh(data.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ExtraCategory_Destroy([DataSourceRequest] DataSourceRequest request, ExtraCategoryViewModel data)
        {
            if (data != null)
            {
                if (Kerberos.CanDeleteExtraCategory(data.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ))
                {
                    extraCategoryService.Destroy(data);
                }
                else
                {
                    ModelState.AddModelError("", "Η διαγραφή της κατηγορίας δεν μπορεί να γίνει διότι υπάρχουν καταχωρημένες δαπάνες σε αυτή.");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region ΠΡΟΫΠΟΛΟΓΙΣΜΕΝΑ ΜΗΝΙΑΙΑ ΠΟΣΑ ΔΑΠΑΝΩΝ

        public ActionResult xBudgetData(string notify = null)
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

            PopulateStations();
            return View();
        }

        public ActionResult BudgetMonth_Read([DataSourceRequest] DataSourceRequest request, int schoolyearId = 0, int monthId = 0)
        {
            List<BudgetDataViewModel> data = budgetMonthService.Read(schoolyearId, monthId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BudgetMonth_Create([DataSourceRequest] DataSourceRequest request, BudgetDataViewModel data, int schoolyearId = 0, int monthId = 0)
        {
            var newdata = new BudgetDataViewModel();

            if (schoolyearId == 0 || monthId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει να έχει επιλεγεί σχολικό έτος και μήνας.");
            }

            if (Kerberos.BudgetDataExists(data, schoolyearId, monthId))
            {
                ModelState.AddModelError("", "Έγινε απόπειρα δημιουργίας διπλοεγγραφής. Η καταχώρηση ακυρώθηκε.");
            }

            if (ModelState.IsValid)
            {
                budgetMonthService.Create(data, schoolyearId, monthId);
                newdata = budgetMonthService.Refresh(data.BUDGET_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BudgetMonth_Update([DataSourceRequest] DataSourceRequest request, BudgetDataViewModel data, int schoolyearId = 0, int monthId = 0)
        {
            var newdata = new BudgetDataViewModel();

            if (schoolyearId == 0 || monthId == 0)
            {
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει να έχει επιλεγεί σχολικό έτος και μήνας.");
            }

            if (ModelState.IsValid)
            {
                budgetMonthService.Update(data, schoolyearId, monthId);
                newdata = budgetMonthService.Refresh(data.BUDGET_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BudgetMonth_Destroy([DataSourceRequest] DataSourceRequest request, BudgetDataViewModel data)
        {
            if (data != null)
            {
                budgetMonthService.Destroy(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult TransferBudgetMonth(int schoolyearId = 0, int monthId = 0)
        {
            string msg = budgetMonthService.TransferMonth(schoolyearId, monthId);

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TransferBudgetYear(int schoolyearId = 0)
        {
            string msg = budgetMonthService.TransferYear(schoolyearId);

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult xBudgetDataPrint()
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


        #region ΦΠΑ ΚΛΙΜΑΚΕΣ

        public ActionResult xVATcategories()
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

        public ActionResult Vat_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<VATScaleViewModel> data = vatScaleService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Vat_Create([DataSourceRequest] DataSourceRequest request, VATScaleViewModel data)
        {
            var newdata = new VATScaleViewModel();

            var existingdata = db.ΦΠΑ_ΤΙΜΕΣ.Where(s => s.FPA_VALUE == data.FPA_VALUE).Count();
            if (existingdata > 0)
                ModelState.AddModelError("", "Αυτή η κατηγορία ΦΠΑ υπάρχει ήδη. Η καταχώρηση ακυρώθηκε.");

            if (data != null && ModelState.IsValid)
            {
                vatScaleService.Create(data);
                newdata = vatScaleService.Refresh(data.FPA_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Vat_Update([DataSourceRequest] DataSourceRequest request, VATScaleViewModel data)
        {
            var newdata = new VATScaleViewModel();

            if (data != null && ModelState.IsValid)
            {
                vatScaleService.Update(data);
                newdata = vatScaleService.Refresh(data.FPA_ID);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Vat_Destroy([DataSourceRequest] DataSourceRequest request, VATScaleViewModel data)
        {
            if (data != null)
            {
                if (Kerberos.CanDeleteVATvalue(data.FPA_ID))
                {
                    vatScaleService.Destroy(data);
                }
                else
                {
                    ModelState.AddModelError("", "Η διαγραφή δεν μπορεί να γίνει διότι υπάρχουν προϊόντα με αυτή την τιμή ΦΠΑ.");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult xProductData(string notify = null)
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

            PopulateProductUnits();
            PopulateExpenseTypes();
            PopulateVATValues();
            return View();
        }

        #region ΠΛΕΓΜΑ ΚΑΤΗΓΟΡΙΩΝ ΠΡΟΪΟΝΤΩΝ

        public ActionResult Category_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<ProductCategoryViewModel> data = productCategoryService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Category_Create([DataSourceRequest] DataSourceRequest request, ProductCategoryViewModel data)
        {
            var newdata = new ProductCategoryViewModel();

            if (ModelState.IsValid)
            {
                productCategoryService.Create(data);
                newdata = productCategoryService.Refresh(data.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Category_Update([DataSourceRequest] DataSourceRequest request, ProductCategoryViewModel data)
        {
            var newdata = new ProductCategoryViewModel();

            if (ModelState.IsValid)
            {
                productCategoryService.Update(data);
                newdata = productCategoryService.Refresh(data.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Category_Destroy([DataSourceRequest] DataSourceRequest request, ProductCategoryViewModel data)
        {
            if (data != null)
            {
                if (!Kerberos.CanDeleteCategory(data.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ)) 
                    ModelState.AddModelError("", "Δεν μπορεί να διαγραφεί η κατηγορία διότι υπάρχουν προϊόντα σε αυτήν.");

                if (ModelState.IsValid)
                {
                    productCategoryService.Destroy(data);
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region ΠΛΕΓΜΑ ΠΡΟΪΟΝΤΩΝ

        public ActionResult Product_Read([DataSourceRequest] DataSourceRequest request, int categoryId = 0)
        {
            List<ProductViewModel> data = productService.Read(categoryId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Product_Create([DataSourceRequest] DataSourceRequest request, ProductViewModel data, int categoryId = 0)
        {
            ProductViewModel newdata = new ProductViewModel();

            if (categoryId > 0)
            {
                if (data != null && ModelState.IsValid)
                {
                    productService.Create(data, categoryId);
                    newdata = productService.Refresh(data.ΠΡΟΙΟΝ_ΚΩΔ);
                }
            }
            else
            {
                ModelState.AddModelError("", "Πρέπει να προηγηθεί επιλογή κατηγορίας για καταχώρηση του προϊόντος.");
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Product_Update([DataSourceRequest] DataSourceRequest request, ProductViewModel data, int categoryId = 0)
        {
            ProductViewModel newdata = new ProductViewModel();

            if (categoryId > 0)
            {
                if (data != null && ModelState.IsValid)
                {
                    productService.Update(data, categoryId);
                    newdata = productService.Refresh(data.ΠΡΟΙΟΝ_ΚΩΔ);
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
        public ActionResult Product_Destroy([DataSourceRequest] DataSourceRequest request, ProductViewModel data)
        {
            if (data != null)
            {
                if (Kerberos.CanDeleteProduct(data.ΠΡΟΙΟΝ_ΚΩΔ))
                {
                    productService.Destroy(data);
                }
                else
                {
                    ModelState.AddModelError("", "Δεν μπορεί να γίνει διαγραφή διότι το προϊόν είναι σε χρήση στις δαπάνες.");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region ΕΥΡΕΤΗΡΙΟ ΠΡΟΪΟΝΤΩΝ

        // The list contains the station's products
        // sqlPRODUCT_LIST contains table ΠΡΟΙΟΝΤΑ_ΒΝΣ

        public ActionResult xProductList(string notify = null)
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
            PopulateVATValues();

            return View();
        }

        public ActionResult ProductList_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0)
        {
            List<sqlProductListViewModel> data = productStationService.Search(stationId);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult xProductsPrint()
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


        #region ΔΑΠΑΝΕΣ ΤΡΟΦΕΙΟΥ

        public JsonResult FoodCategories_Read()
        {
            var data = (from d in db.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ
                        where d.ΔΑΠΑΝΗ_ΚΩΔ == 1
                        orderby d.ΚΑΤΗΓΟΡΙΑ
                        select new ProductCategoryViewModel
                        {
                            ΚΑΤΗΓΟΡΙΑ_ΚΩΔ = d.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CleaningCategories_Read()
        {
            var data = (from d in db.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ
                        where d.ΔΑΠΑΝΗ_ΚΩΔ == 2
                        orderby d.ΚΑΤΗΓΟΡΙΑ
                        select new ProductCategoryViewModel
                        {
                            ΚΑΤΗΓΟΡΙΑ_ΚΩΔ = d.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult OtherCategories_Read()
        {
            var data = (from d in db.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ
                        where d.ΔΑΠΑΝΗ_ΚΩΔ == 3
                        orderby d.ΚΑΤΗΓΟΡΙΑ
                        select new ProductCategoryViewModel
                        {
                            ΚΑΤΗΓΟΡΙΑ_ΚΩΔ = d.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExtraCategories_Read()
        {
            var data = (from d in db.ΕΞΤΡΑ_ΚΑΤΗΓΟΡΙΑ
                        where d.ΔΑΠΑΝΗ_ΚΩΔ == 3
                        orderby d.ΚΑΤΗΓΟΡΙΑ
                        select new ExtraCategoryViewModel
                        {
                            ΚΑΤΗΓΟΡΙΑ_ΚΩΔ = d.ΚΑΤΗΓΟΡΙΑ_ΚΩΔ,
                            ΚΑΤΗΓΟΡΙΑ = d.ΚΑΤΗΓΟΡΙΑ
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FilteredProduct_Read(int categoryId = 0, int stationId = 0)
        {
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

        public ActionResult xCostFoodDay(string notify = null)
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

            PopulateFoodCategories();
            PopulateProductSelector();
            return View();
        }

        #region ΠΛΕΓΜΑ ΗΜΕΡΗΣΙΟΥ ΚΟΣΤΟΥΣ ΤΡΟΦΕΙΟΥ

        public ActionResult CostFood_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0, DateTime? theDate = null)
        {
            DateTime paramDate = theDate ?? DateTime.Today;

            IEnumerable<CostFeedingViewModel> data = costFoodService.Read(stationId, paramDate);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostFood_Create([DataSourceRequest] DataSourceRequest request, CostFeedingViewModel data, int stationId = 0, DateTime? theDate = null)
        {
            int CurrentSchoolYear = Common.GetCurrentSchoolYear();

            if (stationId == 0 || theDate == null)
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σταθμό και ημερομηνία. Η διαδικασία ακυρώθηκε.");

            if (!Common.DateInCurrentSchoolYear((DateTime)theDate))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.");

            CostFeedingViewModel newdata = new CostFeedingViewModel()
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
            if (Kerberos.CostFoodExists(newdata))
                ModelState.AddModelError("", "Έγινε απόπειρα δημιουργίας διπλοεγγραφής. Η διπλή καταχώρηση ακυρώθηκε.");
            
            string err_msg = Kerberos.ValidateCostValue(data.ΠΟΣΟΤΗΤΑ, data.ΤΙΜΗ_ΜΟΝΑΔΑ);
            if (err_msg != "")
                ModelState.AddModelError("", err_msg);

            if (data != null && ModelState.IsValid)
            {
                costFoodService.Create(data, stationId, CurrentSchoolYear, (DateTime)theDate);
                newdata = costFoodService.Refresh(data.ΚΩΔΙΚΟΣ);
            }
            else
            {
                return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostFood_Update([DataSourceRequest] DataSourceRequest request, CostFeedingViewModel data, int stationId = 0, DateTime? theDate = null)
        {
            int CurrentSchoolYear = Common.GetCurrentSchoolYear();

            CostFeedingViewModel newdata = new CostFeedingViewModel();

            if (stationId == 0 || theDate == null)
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σταθμό και ημερομηνία. Η διαδικασία ακυρώθηκε.");

            if (!Common.DateInCurrentSchoolYear((DateTime)theDate))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.");

            string err_msg = Kerberos.ValidateCostValue(data.ΠΟΣΟΤΗΤΑ, data.ΤΙΜΗ_ΜΟΝΑΔΑ);
            if (err_msg != "")
                ModelState.AddModelError("", err_msg);

            if (data != null && ModelState.IsValid)
            {
                costFoodService.Update(data, stationId, CurrentSchoolYear, (DateTime)theDate);
                newdata = costFoodService.Refresh(data.ΚΩΔΙΚΟΣ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostFood_Destroy([DataSourceRequest] DataSourceRequest request, CostFeedingViewModel data)
        {
            if (data != null)
            {
                costFoodService.Destroy(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult xCostFoodDayPrint(string theDate, int stationId = 0)
        {
            ExpenseReportParameters rp = new ExpenseReportParameters();

            rp.STATION_ID = stationId;
            rp.DATE_EXPENSE = theDate;

            return View(rp);
        }


        #endregion


        #region ΔΑΠΑΝΕΣ ΚΑΘΑΡΙΟΤΗΤΑΣ

        public ActionResult xCostCleaningDay(string notify = null)
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

            PopulateCleaningCategories();
            PopulateProductSelector();
            return View();
        }

        #region ΠΛΕΓΜΑ ΗΜΕΡΗΣΙΟΥ ΚΟΣΤΟΥΣ ΚΑΘΑΡΙΟΤΗΤΑΣ

        public ActionResult CostCleaning_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0, DateTime? theDate = null)
        {
            DateTime paramDate = theDate ?? DateTime.Today;

            IEnumerable<CostCleaningViewModel> data = costCleaningService.Read(stationId, paramDate);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostCleaning_Create([DataSourceRequest] DataSourceRequest request, CostCleaningViewModel data, int stationId = 0, DateTime? theDate = null)
        {
            int CurrentSchoolYear = Common.GetCurrentSchoolYear();

            if (stationId == 0 || theDate == null)
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σταθμό και ημερομηνία. Η διαδικασία ακυρώθηκε.");
            
            if (!Common.DateInCurrentSchoolYear((DateTime)theDate))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.");

            CostCleaningViewModel newdata = new CostCleaningViewModel()
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

            if (Kerberos.CostCleaningExists(newdata))
                ModelState.AddModelError("", "Έγινε απόπειρα δημιουργίας διπλοεγγραφής. Η διπλή καταχώρηση ακυρώθηκε.");

            string err_msg = Kerberos.ValidateCostValue(data.ΠΟΣΟΤΗΤΑ, data.ΤΙΜΗ_ΜΟΝΑΔΑ);
            if (err_msg != "")
                ModelState.AddModelError("", err_msg);

            if (data != null && ModelState.IsValid)
            {
                costCleaningService.Create(data, stationId, CurrentSchoolYear, (DateTime)theDate);
                newdata = costCleaningService.Refresh(data.ΚΩΔΙΚΟΣ);
            }
            else
            {
                return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostCleaning_Update([DataSourceRequest] DataSourceRequest request, CostCleaningViewModel data, int stationId = 0, DateTime? theDate = null)
        {
            int CurrentSchoolYear = Common.GetCurrentSchoolYear();
            CostCleaningViewModel newdata = new CostCleaningViewModel();

            if (stationId == 0 || theDate == null)
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σταθμό και ημερομηνία. Η διαδικασία ακυρώθηκε.");

            if (!Common.DateInCurrentSchoolYear((DateTime)theDate))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.");

            string err_msg = Kerberos.ValidateCostValue(data.ΠΟΣΟΤΗΤΑ, data.ΤΙΜΗ_ΜΟΝΑΔΑ);
            if (err_msg != "")
                ModelState.AddModelError("", err_msg);

            if (data != null && ModelState.IsValid)
            {
                costCleaningService.Update(data, stationId, CurrentSchoolYear, (DateTime)theDate);
                newdata = costCleaningService.Refresh(data.ΚΩΔΙΚΟΣ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostCleaning_Destroy([DataSourceRequest] DataSourceRequest request, CostCleaningViewModel data)
        {
            if (data != null)
            {
                costCleaningService.Destroy(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion


        #region ΓΕΝΙΚΕΣ ΔΑΠΑΝΕΣ

        public ActionResult xCostOtherDay(string notify = null)
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

            PopulateOtherCategories();
            PopulateExtraCategories();
            PopulateProductSelector();
            return View();
        }

        #region ΠΛΕΓΜΑ #1 ΗΜΕΡΗΣΙΟΥ ΚΟΣΤΟΥΣ ΑΛΛΩΝ ΔΑΠΑΝΩΝ

        public ActionResult CostOther_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0, DateTime? theDate = null)
        {
            DateTime paramDate = theDate ?? DateTime.Today;

            IEnumerable<CostOtherViewModel> data = costOtherService.Read(stationId, paramDate);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostOther_Create([DataSourceRequest] DataSourceRequest request, CostOtherViewModel data, int stationId = 0, DateTime? theDate = null)
        {
            int CurrentSchoolYear = Common.GetCurrentSchoolYear();

            if (stationId == 0 || theDate == null)
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σταθμό και ημερομηνία. Η διαδικασία ακυρώθηκε.");

            if (!Common.DateInCurrentSchoolYear((DateTime)theDate))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.");

            CostOtherViewModel newdata = new CostOtherViewModel()
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

            if (Kerberos.CostOtherExists(newdata))
                ModelState.AddModelError("", "Έγινε απόπειρα δημιουργίας διπλοεγγραφής. Η διπλή καταχώρηση ακυρώθηκε.");

            string err_msg = Kerberos.ValidateCostValue(data.ΠΟΣΟΤΗΤΑ, data.ΤΙΜΗ_ΜΟΝΑΔΑ);
            if (err_msg != "")
                ModelState.AddModelError("", err_msg);

            if (data != null && ModelState.IsValid)
            {
                costOtherService.Create(data, stationId, CurrentSchoolYear, (DateTime)theDate);
                newdata = costOtherService.Refresh(data.ΚΩΔΙΚΟΣ);
            }
            else
            {
                return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostOther_Update([DataSourceRequest] DataSourceRequest request, CostOtherViewModel data, int stationId = 0, DateTime? theDate = null)
        {
            int CurrentSchoolYear = Common.GetCurrentSchoolYear();
            CostOtherViewModel newdata = new CostOtherViewModel();

            if (stationId == 0 || theDate == null)
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σταθμό και ημερομηνία. Η διαδικασία ακυρώθηκε.");

            if (!Common.DateInCurrentSchoolYear((DateTime)theDate))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.");

            string err_msg = Kerberos.ValidateCostValue(data.ΠΟΣΟΤΗΤΑ, data.ΤΙΜΗ_ΜΟΝΑΔΑ);
            if (err_msg != "")
                ModelState.AddModelError("", err_msg);

            if (data != null && ModelState.IsValid)
            {
                costOtherService.Update(data, stationId, CurrentSchoolYear, (DateTime)theDate);
                newdata = costOtherService.Refresh(data.ΚΩΔΙΚΟΣ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostOther_Destroy([DataSourceRequest] DataSourceRequest request, CostOtherViewModel data)
        {
            if (data != null)
            {
                costOtherService.Destroy(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ΠΛΕΓΜΑ #2 ΗΜΕΡΗΣΙΟΥ ΚΟΣΤΟΥΣ ΓΕΝΙΚΩΝ ΔΑΠΑΝΩΝ

        public ActionResult CostGeneral_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0, DateTime? theDate = null)
        {
            DateTime paramDate = theDate ?? DateTime.Today;

            IEnumerable<CostGeneralViewModel> data = costGeneralService.Read(stationId, paramDate);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostGeneral_Create([DataSourceRequest] DataSourceRequest request, CostGeneralViewModel data, int stationId = 0, DateTime? theDate = null)
        {
            int CurrentSchoolYear = Common.GetCurrentSchoolYear();

            if (stationId == 0 || theDate == null)
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σταθμό και ημερομηνία. Η διαδικασία ακυρώθηκε.");

            if (!Common.DateInCurrentSchoolYear((DateTime)theDate))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.");

            CostGeneralViewModel newdata = new CostGeneralViewModel()
            {
                ΗΜΕΡΟΜΗΝΙΑ = theDate,
                ΒΝΣ = stationId,
                ΣΧΟΛ_ΕΤΟΣ = CurrentSchoolYear,
                ΚΑΤΗΓΟΡΙΑ = data.ΚΑΤΗΓΟΡΙΑ,
                ΠΕΡΙΓΡΑΦΗ = data.ΠΕΡΙΓΡΑΦΗ,
                ΣΥΝΟΛΟ = data.ΣΥΝΟΛΟ
            };

            if (Kerberos.CostGeneralExists(newdata))
                ModelState.AddModelError("", "Έγινε απόπειρα δημιουργίας διπλοεγγραφής. Η διπλή καταχώρηση ακυρώθηκε.");

            string err_msg = Kerberos.ValidateCostGeneral(data);
            if (err_msg != "")
                ModelState.AddModelError("", err_msg);

            if (data != null && ModelState.IsValid)
            {
                costGeneralService.Create(data, stationId, CurrentSchoolYear, (DateTime)theDate);
                newdata = costGeneralService.Refresh(data.ΚΩΔΙΚΟΣ);
            }
            else
            {
                return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostGeneral_Update([DataSourceRequest] DataSourceRequest request, CostGeneralViewModel data, int stationId = 0, DateTime? theDate = null)
        {
            int CurrentSchoolYear = Common.GetCurrentSchoolYear();
            CostGeneralViewModel newdata = new CostGeneralViewModel();

            if (stationId == 0 || theDate == null)
                ModelState.AddModelError("", "Για να γίνει η καταχώρηση πρέπει πρώτα να έχετε επιλέξει σταθμό και ημερομηνία. Η διαδικασία ακυρώθηκε.");

            if (!Common.DateInCurrentSchoolYear((DateTime)theDate))
                ModelState.AddModelError("", "Η επιλεγμένη ημερομηνία είναι εκτός του τρέχοντος σχολικού έτους. Η διαδικασία ακυρώθηκε.");

            string err_msg = Kerberos.ValidateCostGeneral(data);
            if (err_msg != "")
                ModelState.AddModelError("", err_msg);

            if (data != null && ModelState.IsValid)
            {
                costGeneralService.Update(data, stationId, CurrentSchoolYear, (DateTime)theDate);
                newdata = costGeneralService.Refresh(data.ΚΩΔΙΚΟΣ);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CostGeneral_Destroy([DataSourceRequest] DataSourceRequest request, CostGeneralViewModel data)
        {
            if (data != null)
            {
                costGeneralService.Destroy(data);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion


        #region ΕΥΡΕΤΗΡΙΑ ΔΑΠΑΝΩΝ

        // ΔΑΠΑΝΕΣ ΤΡΟΦΕΙΟΥ
        public ActionResult xSearchCostFood(string notify = null)
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

            return View();
        }

        public ActionResult CostFoodSearch_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {
            List<sqlCostFoodViewModel> data = costFoodService.Search(stationId, theDate1, theDate2);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        // ΔΑΠΑΝΕΣ ΚΑΘΑΡΙΟΤΗΤΑΣ
        public ActionResult xSearchCostCleaning(string notify = null)
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

            return View();
        }

        public ActionResult CostCleaningSearch_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {
            List<sqlCostCleaningViewModel> data = costCleaningService.Search(stationId, theDate1, theDate2);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        // ΔΑΠΑΝΕΣ ΑΛΛΕΣ ΚΑΙ ΓΕΝΙΚΕΣ (2 ΠΛΕΓΜΑΤΑ)
        public ActionResult xSearchCostOther(string notify = null)
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

            return View();
        }

        public ActionResult CostOtherSearch_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {
            List<sqlCostOtherViewModel> data = costOtherService.Search(stationId, theDate1, theDate2);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CostGeneralSearch_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {
            List<sqlCostGeneralViewModel> data = costGeneralService.Search(stationId, theDate1, theDate2);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        // ΔΑΠΑΝΕΣ ΓΕΝΙΚΕΣ ΚΑΙ ΕΚΤΑΚΤΕΣ ΕΝΟΠΟΙΗΜΕΝΕΣ
        public ActionResult xSearchCostAllOther(string notify = null)
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

            return View();
        }

        public ActionResult CostAllOtherSearch_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {
            var data = GetCostAllOtherSearchFromDB(stationId, theDate1, theDate2);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public List<sqlCostAllOtherViewModel> GetCostAllOtherSearchFromDB(int stationId, DateTime? theDate1, DateTime? theDate2)
        {
            List<sqlCostAllOtherViewModel> data = new List<sqlCostAllOtherViewModel>();

            if (stationId > 0 && theDate1 != null && theDate2 != null)
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

        public ActionResult xCostFoodPrint()
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

        public ActionResult xCostCleaningPrint()
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

        public ActionResult xCostOtherPrint()
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

        public ActionResult xCostMiscPrint()
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

        public ActionResult xCostAllOtherPrint()
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

        #endregion


        #region ΣΥΓΚΕΝΤΡΩΤΙΚΑ ΣΤΟΙΧΕΙΑ ΔΑΠΑΝΩΝ

        // ΤΡΟΦΕΙΟ
        public ActionResult xSumPersonsFoodDay(string notify = null)
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

            return View();
        }

        public ActionResult SumPersonsFood_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {
            List<SumPersonsTrofeioViewModel> data = mealPlanService.Aggregate(stationId, theDate1, theDate2);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        // ΚΑΘΑΡΙΟΤΗΤΑ
        public ActionResult xSumCleaningDay(string notify = null)
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

            return View();
        }

        public ActionResult SumCleaningDay_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {
            List<SumCleaningDayViewModel> data = costCleaningService.Aggregate(stationId, theDate1, theDate2);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        // ΓΕΝΙΚΕΣ ΚΑΙ ΕΚΤΑΚΤΕΣ ΔΑΠΑΝΕΣ
        public ActionResult xSumOtherExpenseDay(string notify = null)
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

            return View();
        }

        public ActionResult SumOtherExpenseDay_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {
            List<SumOtherExpenseDayViewModel> data = costOtherService.Aggregate(stationId, theDate1, theDate2);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SumExtraExpenseDay_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {
            List<SumExtraExpenseDayViewModel> data = costGeneralService.Aggregate(stationId, theDate1, theDate2);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        // ΕΝΟΠΟΙΗΜΕΝΕΣ ΓΕΝΙΚΕΣ ΚΑΙ ΕΚΤΑΚΤΕΣ
        public ActionResult xSumAllOtherExpenseDay(string notify = null)
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

            return View();
        }

        public ActionResult SumAllOtherDay_Read([DataSourceRequest] DataSourceRequest request, int stationId = 0, DateTime? theDate1 = null, DateTime? theDate2 = null)
        {
            var data = GetSumAllOtherExpenseDayFromDB(stationId, theDate1, theDate2);

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public List<SumAllOtherExpenseDayViewModel> GetSumAllOtherExpenseDayFromDB(int stationId, DateTime? theDate1, DateTime? theDate2)
        {
            List<SumAllOtherExpenseDayViewModel> data = new List<SumAllOtherExpenseDayViewModel>();

            if (stationId > 0 && theDate1 != null && theDate2 != null)
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

        #region ΕΚΤΥΠΩΣΕΙΣ ΣΥΓΚΕΝΤΡΩΤΙΚΩΝ ΣΤΟΙΧΕΙΩΝ ΔΑΠΑΝΩΝ

        public ActionResult xSumCostFoodPrint()
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

        public ActionResult xSumCostCleaningPrint()
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

        public ActionResult xSumCostOtherPrint()
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

        public ActionResult xSumCostMiscPrint()
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

        public ActionResult xSumCostAllOtherPrint()
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

        #endregion


        #region ΜΗΝΑΙΑ ΙΣΟΖΥΓΙΑ ΔΑΠΑΝΩΝ

        public ActionResult xBalanceMonth(string notify = null)
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
            return View();
        }

        public PartialViewResult LoadBalanceMonth(int stationId = 0, int schoolyearId = 0, int monthid = 0)
        {
            expBalanceMonthViewModel balanceModel = GetBalanceRecord(stationId, schoolyearId, monthid);

            return PartialView("xBalanceMonthPartial", balanceModel);
        }

        public expBalanceMonthViewModel GetBalanceRecord(int stationId = 0, int schoolyearId = 0, int monthid = 0)
        {
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

        public ActionResult CreateBalanceMonth(int stationId = 0, int schoolyearId = 0, int monthid = 0)
        {
            string msg = "Η δημιουργία του πίνακα μηνιαίων δαπανών ολοκληρώθηκε.";

            var source = (from d in db.expSTATION_BALANCE_MONTH
                          where d.ΒΝΣ == stationId && d.ΣΧΟΛ_ΕΤΟΣ == schoolyearId && d.ΜΗΝΑΣ_ΑΡΙΘΜΟΣ == monthid
                          select d).FirstOrDefault();

            if (source == null)
            {
                msg = "Δεν βρέθηκαν δεδομένα προέλευσης για δημιουργία του πίνακα. Η διαδικασία ακυρώθηκε";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            var target = (from d in db.ΙΣΟΖΥΓΙΟ_ΜΗΝΑΣ where d.STATION_ID == stationId && d.SCHOOLYEAR_ID == schoolyearId && d.MONTH_ID == monthid select d).ToList();
            if (target.Count > 0)
            {
                msg = "Υπάρχουν δεδομένα στον πίνακα προορισμού για αυτό το σταθμό, σχολικό έτος και μήνα. Η διαδικασία ακυρώθηκε.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            // TODO: CREATE RECORD
            ΙΣΟΖΥΓΙΟ_ΜΗΝΑΣ entity = new ΙΣΟΖΥΓΙΟ_ΜΗΝΑΣ()
            {
                STATION_ID = stationId,
                SCHOOLYEAR_ID = schoolyearId,
                MONTH_ID = monthid,
                CHILDREN_NUMBER = source.ΠΑΙΔΙΑ_ΔΥΝΑΜΗ,
                PERSONNEL_NUMBER = source.ΠΡΟΣΩΠΙΚΟ_ΔΥΝΑΜΗ,
                PERSONS_NUMBER = source.ΠΛΗΘΟΣ_ΑΤΟΜΑ,
                COST_CLEAN = source.ΚΟΣΤΟΣ_ΚΑΘΑΡΙΟΤΗΤΑ,
                COST_FEED = source.ΚΟΣΤΟΣ_ΤΡΟΦΕΙΟ,
                COST_OTHER = source.ΚΟΣΤΟΣ_ΓΕΝΙΚΕΣ,
                COST_TOTAL = source.ΚΟΣΤΟΣ_ΤΡΟΦΕΙΟ + source.ΚΟΣΤΟΣ_ΚΑΘΑΡΙΟΤΗΤΑ + source.ΚΟΣΤΟΣ_ΓΕΝΙΚΕΣ,
                EXPENSE_CLEAN = source.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ,
                EXPENSE_FEED = source.ΔΑΠΑΝΗ_ΤΡΟΦΕΙΟ,
                EXPENSE_OTHER = source.ΔΑΠΑΝΗ_ΓΕΝΙΚΕΣ,
                EXPENSE_TOTAL = source.ΔΑΠΑΝΗ_ΤΡΟΦΕΙΟ + source.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ + source.ΔΑΠΑΝΗ_ΓΕΝΙΚΕΣ,
                BALANCE_CLEAN = source.ΥΠΟΛΟΙΠΟ_ΚΑΘΑΡΙΟΤΗΤΑ,
                BALANCE_FEED = source.ΥΠΟΛΟΙΠΟ_ΤΡΟΦΕΙΟ,
                BALANCE_OTHER = source.ΥΠΟΛΟΙΠΟ_ΓΕΝΙΚΕΣ,
                BALANCE_TOTAL = source.ΥΠΟΛΟΙΠΟ_ΤΡΟΦΕΙΟ + source.ΥΠΟΛΟΙΠΟ_ΚΑΘΑΡΙΟΤΗΤΑ + source.ΥΠΟΛΟΙΠΟ_ΓΕΝΙΚΕΣ
            };
            db.ΙΣΟΖΥΓΙΟ_ΜΗΝΑΣ.Add(entity);
            db.SaveChanges();

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateBalanceMonth(int stationId = 0, int schoolyearId = 0, int monthid = 0)
        {
            string msg = "Η ενημέρωση του πίνακα μηνιαίων δαπανών ολοκληρώθηκε.";

            var src = (from d in db.expSTATION_BALANCE_MONTH
                       where d.ΒΝΣ == stationId && d.ΣΧΟΛ_ΕΤΟΣ == schoolyearId && d.ΜΗΝΑΣ_ΑΡΙΘΜΟΣ == monthid
                       select d).FirstOrDefault();

            if (src == null)
            {
                msg = "Δεν βρέθηκαν δεδομένα προέλευσης για ενημέρωση του πίνακα. Η διαδικασία ακυρώθηκε";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            var target = (from d in db.ΙΣΟΖΥΓΙΟ_ΜΗΝΑΣ where d.STATION_ID == stationId && d.SCHOOLYEAR_ID == schoolyearId && d.MONTH_ID == monthid select d).FirstOrDefault();
            if (target == null)
            {
                msg = "Δεν υπάρχουν δεδομένα στον πίνακα προορισμού για αυτά τα στοιχεία. Η ενημέρωση ακυρώθηκε.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            int targetId = target.BALANCE_ID;
            ΙΣΟΖΥΓΙΟ_ΜΗΝΑΣ entity = db.ΙΣΟΖΥΓΙΟ_ΜΗΝΑΣ.Find(targetId);

            entity.STATION_ID = stationId;
            entity.SCHOOLYEAR_ID = schoolyearId;
            entity.MONTH_ID = monthid;
            entity.CHILDREN_NUMBER = src.ΠΑΙΔΙΑ_ΔΥΝΑΜΗ;
            entity.PERSONNEL_NUMBER = src.ΠΡΟΣΩΠΙΚΟ_ΔΥΝΑΜΗ;
            entity.PERSONS_NUMBER = src.ΠΛΗΘΟΣ_ΑΤΟΜΑ;
            entity.COST_CLEAN = src.ΚΟΣΤΟΣ_ΚΑΘΑΡΙΟΤΗΤΑ;
            entity.COST_FEED = src.ΚΟΣΤΟΣ_ΤΡΟΦΕΙΟ;
            entity.COST_OTHER = src.ΚΟΣΤΟΣ_ΓΕΝΙΚΕΣ;
            entity.COST_TOTAL = src.ΚΟΣΤΟΣ_ΤΡΟΦΕΙΟ + src.ΚΟΣΤΟΣ_ΚΑΘΑΡΙΟΤΗΤΑ + src.ΚΟΣΤΟΣ_ΓΕΝΙΚΕΣ;
            entity.EXPENSE_CLEAN = src.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ;
            entity.EXPENSE_FEED = src.ΔΑΠΑΝΗ_ΤΡΟΦΕΙΟ;
            entity.EXPENSE_OTHER = src.ΔΑΠΑΝΗ_ΓΕΝΙΚΕΣ;
            entity.EXPENSE_TOTAL = src.ΔΑΠΑΝΗ_ΤΡΟΦΕΙΟ + src.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ + src.ΔΑΠΑΝΗ_ΓΕΝΙΚΕΣ;
            entity.BALANCE_CLEAN = src.ΥΠΟΛΟΙΠΟ_ΚΑΘΑΡΙΟΤΗΤΑ;
            entity.BALANCE_FEED = src.ΥΠΟΛΟΙΠΟ_ΤΡΟΦΕΙΟ;
            entity.BALANCE_OTHER = src.ΥΠΟΛΟΙΠΟ_ΓΕΝΙΚΕΣ;
            entity.BALANCE_TOTAL = src.ΥΠΟΛΟΙΠΟ_ΤΡΟΦΕΙΟ + src.ΥΠΟΛΟΙΠΟ_ΚΑΘΑΡΙΟΤΗΤΑ + src.ΥΠΟΛΟΙΠΟ_ΓΕΝΙΚΕΣ;

            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteBalanceMonth(int stationId = 0, int schoolyearId = 0, int monthid = 0)
        {
            string msg = "Η διαγραφή του πίνακα μηνιαίων δαπανών ολοκληρώθηκε.";

            var source = (from d in db.ΙΣΟΖΥΓΙΟ_ΜΗΝΑΣ
                          where d.STATION_ID == stationId && d.SCHOOLYEAR_ID == schoolyearId && d.MONTH_ID == monthid
                          select d).FirstOrDefault();

            if (source == null)
            {
                msg = "Δεν βρέθηκαν δεδομένα με τα επιλεγμένα στοιχεία. Η διαγραφή του πίνακα ακυρώθηκε";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            int recordId = source.BALANCE_ID;
            ΙΣΟΖΥΓΙΟ_ΜΗΝΑΣ entity = db.ΙΣΟΖΥΓΙΟ_ΜΗΝΑΣ.Find(recordId);
            if (entity != null)
            {
                db.Entry(entity).State = EntityState.Deleted;
                db.ΙΣΟΖΥΓΙΟ_ΜΗΝΑΣ.Remove(entity);
                db.SaveChanges();
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult xBalanceMonthPrint()
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

        #endregion ΛΕΙΤΟΥΡΓΙΕΣ ΔΑΠΑΝΩΝ ΤΡΟΦΕΙΟΥ, ΚΑΘΑΡΙΟΤΗΤΑΣ ΚΑΙ ΓΕΝΙΚΩΝ


        #region --- ΣΤΑΤΙΣΤΙΚΕΣ ΕΚΘΕΣΕΙΣ ---

        #region ΕΠΙΛΟΓΕΑΣ ΑΝΑΛΥΤΙΚΩΝ ΣΤΟΙΧΕΙΩΝ

        public ActionResult ReportsDetailList()
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

            List<SysReportViewModel> reports = GetReportsDetailFromDB();
            return View(reports);
        }

        public ActionResult ReportSelectorDetail(int reportId = 0)
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
            // logic of report selection here
            if (reportId == 1)
            {
                return RedirectToAction("ReportDemoPrint", "Admin");
            }
            else if (reportId == 2)
            {
                return RedirectToAction("ReportDemoPrint", "Admin");
            }
            else if (reportId == 3)
            {
                return RedirectToAction("ReportDemoPrint", "Admin");
            }
            else if (reportId == 4)
            {
                return RedirectToAction("ReportDemoPrint", "Admin");
            }
            else if (reportId == 5)
            {
                return RedirectToAction("ReportDemoPrint", "Admin");
            }
            else if (reportId == 6)
            {
                return RedirectToAction("ReportDemoPrint", "Admin");
            }
            else if (reportId == 7)
            {
                return RedirectToAction("ChildrenNamesClassesPrint", "Admin");
            }
            else if (reportId == 10)
            {
                return RedirectToAction("PersonnelStation1Print", "Admin");
            }
            else if (reportId == 11)
            {
                return RedirectToAction("PersonnelStation2Print", "Admin");
            }
            else
            {
                return RedirectToAction("ReportDemoPrint", "Admin");
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

        public ActionResult ReportsSummaryList()
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

            List<SysReportViewModel> reports = GetReportsSummaryFromDB();
            return View(reports);
        }

        public ActionResult ReportSelectorSummary(int reportId = 0)
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
            // logic of report selection here
            if (reportId == 2)
            {
                return RedirectToAction("ReportDemoPrint", "Admin");
            }
            else if (reportId == 3)
            {
                return RedirectToAction("SumChildrenClassesPrint", "Admin");
            }
            else if (reportId == 4)
            {
                return RedirectToAction("StationChildrenClassesPrint", "Admin");
            }
            else if (reportId == 5)
            {
                return RedirectToAction("SumChildrenPeriferiakesPrint", "Admin");
            }
            else if (reportId == 6)
            {
                return RedirectToAction("SumChildrenGenderPrint", "Admin");
            }
            else if (reportId == 8)
            {
                return RedirectToAction("SumChildrenStationPrint", "Admin");
            }
            else if (reportId == 9)
            {
                return RedirectToAction("SumPersonnelStationPrint", "Admin");
            }
            else if (reportId == 12)
            {
                return RedirectToAction("SumPersonnelStation2Print", "Admin");
            }
            else
            {
                return RedirectToAction("ReportDemoPrint", "Admin");
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

        public ActionResult ReportDemoPrint()
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


        #region ΕΚΤΥΠΩΣΕΙΣ ΑΝΑΛΥΤΙΚΑ ΣΤΟΙΧΕΙΑ

        public ActionResult PersonnelStation1Print()
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

        public ActionResult PersonnelStation2Print()
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

        public ActionResult ChildrenNamesClassesPrint()
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

        #region ΕΚΤΥΠΩΣΕΙΣ ΣΥΓΚΕΝΤΡΩΤΙΚΑ ΣΤΟΙΧΕΙΑ

        public ActionResult SumPersonnelStation2Print()
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

        public ActionResult SumPersonnelStationPrint()
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

        public ActionResult SumChildrenStationPrint()
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

        public ActionResult SumChildrenGenderPrint()
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

        public ActionResult SumChildrenClassesPrint()
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

        public ActionResult StationChildrenClassesPrint()
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

        public ActionResult SumChildrenPeriferiakesPrint()
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

        public bool PersonnelExists()
        {
            var data = (from d in db.ΠΡΟΣΩΠΙΚΟ select d).Count();
            if (data == 0) return false;

            return true;
        }

        public bool EducatorsExist()
        {
            var data = (from d in db.ΠΡΟΣΩΠΙΚΟ where d.ΠΡΟΣΩΠΙΚΟ_ΕΙΔΟΣ == 1 select d).Count();
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

        public void PopulateMealsMorning()
        {
            var data = (from d in db.ΓΕΥΜΑΤΑ_ΠΡΩΙ orderby d.ΠΡΩΙΝΟ select d).ToList();
            ViewData["meals_morning"] = data;
            ViewData["defaultMealMorning"] = data.First().ΠΡΩΙΝΟ_ΚΩΔ;
        }

        public void PopulateMealsNoon()
        {
            var data = (from d in db.ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ orderby d.ΜΕΣΗΜΕΡΙΑΝΟ select d).ToList();
            ViewData["meals_noon"] = data;
            ViewData["defaultMealNoon"] = data.First().ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ;
        }

        public void PopulateMealsBaby()
        {
            var data = (from d in db.ΓΕΥΜΑΤΑ_ΒΡΕΦΗ orderby d.ΒΡΕΦΙΚΟ select d).ToList();
            ViewData["meals_baby"] = data;
            ViewData["defaultMealBaby"] = data.First().ΒΡΕΦΙΚΟ_ΚΩΔ;
        }

        public void PopulateVATValues()
        {
            var data = (from d in db.ΦΠΑ_ΤΙΜΕΣ orderby d.FPA_VALUE select d).ToList();
            ViewData["vat_values"] = data;
        }

        public void PopulateExpenseTypes()
        {
            var data = (from d in db.ΔΑΠΑΝΕΣ_ΕΙΔΗ orderby d.ΕΙΔΟΣ_ΛΕΚΤΙΚΟ select d).ToList();
            ViewData["expense_types"] = data;
        }

        public void PopulateProductUnits()
        {
            var data = (from d in db.ΠΡΟΙΟΝ_ΜΟΝΑΔΕΣ orderby d.ΜΟΝΑΔΑ select d).ToList();
            ViewData["units"] = data;
        }

        public void PopulateFoodCategories()
        {
            var data = (from d in db.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ where d.ΔΑΠΑΝΗ_ΚΩΔ == 1 orderby d.ΚΑΤΗΓΟΡΙΑ select d).ToList();
            ViewData["categories"] = data;
        }

        public void PopulateCleaningCategories()
        {
            var data = (from d in db.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ where d.ΔΑΠΑΝΗ_ΚΩΔ == 2 orderby d.ΚΑΤΗΓΟΡΙΑ select d).ToList();
            ViewData["categories"] = data;
        }

        public void PopulateOtherCategories()
        {
            var data = (from d in db.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ where d.ΔΑΠΑΝΗ_ΚΩΔ == 3 orderby d.ΚΑΤΗΓΟΡΙΑ select d).ToList();
            ViewData["other_categories"] = data;
        }

        public void PopulateExtraCategories()
        {
            var data = (from d in db.ΕΞΤΡΑ_ΚΑΤΗΓΟΡΙΑ orderby d.ΚΑΤΗΓΟΡΙΑ select d).ToList();
            ViewData["extra_categories"] = data;
        }

        public void PopulateProductSelector()
        {
            var data = (from d in db.sqlPRODUCT_SELECTOR orderby d.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ select d).ToList();
            ViewData["products_units"] = data;
        }

        public void PopulateEmployees()
        {
            var data = (from d in db.srcPERSONNEL_DATA orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ select d).ToList();

            ViewData["personnel"] = data;
            ViewData["defaultPersonnel"] = data.First().PERSONNEL_ID;
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
            ViewData["hourFirst"] = hours.First().HOUR_ID;
            ViewData["hourLast"] = hours.Last().HOUR_ID;
        }

        public void PopulateChildren()
        {
            var data = (from d in db.sqlCHILD_TMIMA_SELECTOR orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ select d).ToList();

            ViewData["children"] = data;
            ViewData["defaultChild"] = data.First().CHILD_ID;
        }

        public void PopulatePersonnel()
        {
            var data = (from d in db.sqlPERSONNEL_SELECTOR orderby d.ΟΝΟΜΑΤΕΠΩΝΥΜΟ select d).ToList();

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
            var tmimata = (from d in db.ΤΜΗΜΑ  orderby d.ΟΝΟΜΑΣΙΑ select d).ToList();
            ViewData["tmimata"] = tmimata;
            ViewData["defaultTmima"] = tmimata.First().ΤΜΗΜΑ_ΚΩΔ;
        }

        #endregion


        #region GETTERS

        public JsonResult GetMealsMorning(int stationId = 0)
        {
            var data = (from d in db.ΓΕΥΜΑΤΑ_ΠΡΩΙ
                        where d.ΒΝΣ == stationId
                        select new MealMorningViewModel
                        {
                            ΠΡΩΙΝΟ_ΚΩΔ = d.ΠΡΩΙΝΟ_ΚΩΔ,
                            ΠΡΩΙΝΟ = d.ΠΡΩΙΝΟ
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMealsNoon(int stationId = 0)
        {
            var data = (from d in db.ΓΕΥΜΑΤΑ_ΜΕΣΗΜΕΡΙ
                        where d.ΒΝΣ == stationId
                        select new MealNoonViewModel
                        {
                            ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ = d.ΜΕΣΗΜΕΡΙΑΝΟ_ΚΩΔ,
                            ΜΕΣΗΜΕΡΙΑΝΟ = d.ΜΕΣΗΜΕΡΙΑΝΟ
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMealsBaby(int stationId = 0)
        {
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

        public USER_ADMINS GetLoginAdmin()
        {
            loggedAdmin = db.USER_ADMINS.Where(u => u.USERNAME == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();
            ViewBag.loggedUser = loggedAdmin.FULLNAME;
            return loggedAdmin;
        }

        #endregion


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


        #region SPECIAL FUNCTIONS

        public ActionResult CreateStationProducts()
        {
            string msg = "Η διαδικασία δημιουργίας λίστας προίόντων για κάθε σταθμό ολοκληρώθηκε.";

            var src = (from d in db.ΠΡΟΙΟΝΤΑ orderby d.ΠΡΟΙΟΝ_ΚΩΔ select d).ToList();

            var target = (from d in db.ΠΡΟΙΟΝΤΑ_ΒΝΣ select d).ToList();
            if (target.Count > 0)
            {
                msg = "Υπάρχουν ήδη προϊόντα στον πίνακα προορισμού. Η διαδικασία ακυρώθηκε.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            for (int sid = 1; sid <= 26; sid++)
            {
                foreach (var item in src)
                {
                    AppendProduct(item, sid);
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public void AppendProduct(ΠΡΟΙΟΝΤΑ item, int sid)
        {
            int offset = 436;

            ΠΡΟΙΟΝΤΑ_ΒΝΣ entity = new ΠΡΟΙΟΝΤΑ_ΒΝΣ()
            {
                ΠΡΟΙΟΝ_ΚΩΔ = item.ΠΡΟΙΟΝ_ΚΩΔ + offset * (sid - 1),
                ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ = item.ΠΡΟΙΟΝ_ΚΑΤΗΓΟΡΙΑ,
                ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ = item.ΠΡΟΙΟΝ_ΛΕΚΤΙΚΟ,
                ΠΡΟΙΟΝ_ΜΟΝΑΔΑ = item.ΠΡΟΙΟΝ_ΜΟΝΑΔΑ,
                ΠΡΟΙΟΝ_ΦΠΑ = item.ΠΡΟΙΟΝ_ΦΠΑ,
                ΒΝΣ = sid
            };
            db.ΠΡΟΙΟΝΤΑ_ΒΝΣ.Add(entity);
            db.SaveChanges();
        }

        public ActionResult UpdateStationExpenses()
        {
            string msg = "Η ενημέρωση των κωδικών προϊόντων στις δαπάνες των σταθμών ολοκληρώθηκε.";

            for (int sid = 1; sid <= 26; sid++)
            {
                UpdateExpensesFood(sid);
                UpdateExpensesClean(sid);
                UpdateExpensesOther(sid);
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public void UpdateExpensesFood(int stationId)
        {
            int offset = 436;

            var target = (from d in db.ΔΑΠΑΝΗ_ΤΡΟΦΗ where d.ΒΝΣ == stationId select d).ToList();
            if (target.Count > 0)
            {
                foreach (var item in target)
                {
                    ΔΑΠΑΝΗ_ΤΡΟΦΗ entity = db.ΔΑΠΑΝΗ_ΤΡΟΦΗ.Find(item.ΚΩΔΙΚΟΣ);
                    entity.ΠΡΟΙΟΝ = entity.ΠΡΟΙΟΝ + offset * (stationId - 1);

                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public void UpdateExpensesClean(int stationId)
        {
            int offset = 436;

            var target = (from d in db.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ where d.ΒΝΣ == stationId select d).ToList();
            if (target.Count > 0)
            {
                foreach (var item in target)
                {
                    ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ entity = db.ΔΑΠΑΝΗ_ΚΑΘΑΡΙΟΤΗΤΑ.Find(item.ΚΩΔΙΚΟΣ);
                    entity.ΠΡΟΙΟΝ = entity.ΠΡΟΙΟΝ + offset * (stationId - 1);

                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public void UpdateExpensesOther(int stationId)
        {
            int offset = 436;

            var target = (from d in db.ΔΑΠΑΝΗ_ΑΛΛΗ where d.ΒΝΣ == stationId select d).ToList();
            if (target.Count > 0)
            {
                foreach (var item in target)
                {
                    ΔΑΠΑΝΗ_ΑΛΛΗ entity = db.ΔΑΠΑΝΗ_ΑΛΛΗ.Find(item.ΚΩΔΙΚΟΣ);
                    entity.ΠΡΟΙΟΝ = entity.ΠΡΟΙΟΝ + offset * (stationId - 1);

                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        #endregion
    }

}