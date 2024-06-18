using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Newtonsoft.Json;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Abacus.Models;
using Abacus.BPM;
using Abacus.DAL;
using Abacus.DAL.Security;
using Abacus.Notification;
using Abacus.Services;

namespace Abacus.Controllers.UserControllers
{
    public class USER_ADMINSController : Controller
    {
        private readonly AbacusDBEntities db;
        private USER_ADMINS loggedAdmin;

        private readonly UserAdminService userAdminService;

        public USER_ADMINSController()
        {
            db = new AbacusDBEntities();

            userAdminService = new UserAdminService(db);
        }

        protected override void Dispose(bool disposing)
        {
            userAdminService.Dispose();

            base.Dispose(disposing);
        }

        public ActionResult Login()
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!val1)
            {
                ViewBag.loggedUser = "(χωρίς σύνδεση)";
            }
            else
            {
                loggedAdmin = db.USER_ADMINS.Where(u => u.USERNAME == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();
                if (loggedAdmin != null)
                {
                    ViewBag.loggedUser = loggedAdmin.FULLNAME;
                    //SetLoginStatus(loggedAdmin, true);
                    return RedirectToAction("Index", "Admin");
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "USERNAME,PASSWORD")]  UserAdminViewModel model)
        {
            var user = db.USER_ADMINS.Where(u => u.USERNAME == model.USERNAME && u.PASSWORD == model.PASSWORD).FirstOrDefault();

            if (user != null)
            {
                WriteUserCookie(model);
                SetLoginStatus(user, true);
                LoginRecord(user.USERNAME);

                return RedirectToAction("Index", "Admin");
            }
            ModelState.AddModelError("", "Το όνομα χρήστη ή/και ο κωδ.πρόσβασης δεν είναι σωστά");
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult LogOut([Bind(Include = "ISACTIVE")] USER_ADMINS userAdmin)
        {
            var user = db.USER_ADMINS.Where(u => u.USERNAME == userAdmin.USERNAME && u.PASSWORD == userAdmin.PASSWORD).FirstOrDefault();

            FormsAuthentication.SignOut();
            SetLoginStatus(user, false);

            return RedirectToAction("Index", "Home");
        }

        public void WriteUserCookie(UserAdminViewModel user)
        {
            AdminPrincipalSerializeModel serializeModel = new AdminPrincipalSerializeModel();
            serializeModel.UserId = user.USER_ID;
            serializeModel.Username = user.USERNAME;
            serializeModel.FullName = user.FULLNAME;

            string userData = JsonConvert.SerializeObject(serializeModel);
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                1, user.USERNAME, DateTime.Now, DateTime.Now.AddMinutes(Kerberos.TICKET_TIMEOUT_MINUTES), false, userData);
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(faCookie);
        }

        public void SetLoginStatus(USER_ADMINS user, bool value)
        {
            user.ISACTIVE = value;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
         }

        public void LoginRecord(string username)
        {
            var loginData = (from d in db.ADMIN_LOGINS where d.LOGIN_USERNAME == username select d).FirstOrDefault();

            if (loginData == null)
            {
                ADMIN_LOGINS entity = new ADMIN_LOGINS()
                {
                    LOGIN_USERNAME = username,
                    LOGIN_DATETIME = DateTime.Now
                };
                db.ADMIN_LOGINS.Add(entity);
                db.SaveChanges();
            }
            else
            {
                ADMIN_LOGINS entity = db.ADMIN_LOGINS.Find(loginData.LOGIN_ID);
                entity.LOGIN_DATETIME = DateTime.Now;
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }
        }


        public ActionResult AdminList()
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

        #region Grid CRUD Functions

        [HttpPost]
        public ActionResult Admin_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<UserAdminViewModel> data = userAdminService.Read();

            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Admin_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<UserAdminViewModel> userAdmins)
        {
            var results = new List<UserAdminViewModel>();
            foreach (var userAdmin in userAdmins)
            {
                if (userAdmin != null && ModelState.IsValid)
                {
                    userAdminService.Create(userAdmin);
                    results.Add(userAdmin);
                }
            }
            return Json(results.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Admin_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<UserAdminViewModel> userAdmins)
        {
            if (userAdmins != null && ModelState.IsValid)
            {
                foreach (var userAdmin in userAdmins)
                {
                    userAdminService.Update(userAdmin);
                }
            }

            return Json(userAdmins.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Admin_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<UserAdminViewModel> userAdmins)
        {
            if (userAdmins.Any())
            {
                foreach (var userAdmin in userAdmins)
                {
                    userAdminService.Destroy(userAdmin);
                }
            }
            return Json(userAdmins.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        #endregion

        public USER_ADMINS GetLoginAdmin()
        {
            loggedAdmin = db.USER_ADMINS.Where(u => u.USERNAME == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();
            ViewBag.loggedUser = loggedAdmin.FULLNAME;
            return loggedAdmin;
        }


    }
}
