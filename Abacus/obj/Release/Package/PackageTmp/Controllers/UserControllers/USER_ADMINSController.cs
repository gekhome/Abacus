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

namespace Abacus.Controllers.UserControllers
{
    public class USER_ADMINSController : Controller
    {
        private AbacusDBEntities db = new AbacusDBEntities();
        private USER_ADMINS loggedAdmin;
        //private int prokirixiId;
        Common c = new Common();

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
               // var roles = user.ROLES.Select(m => m.ROLE_NAME).ToArray();

                AdminPrincipalSerializeModel serializeModel = new AdminPrincipalSerializeModel();
                serializeModel.UserId = model.USER_ID;
                serializeModel.Username = model.USERNAME;
                serializeModel.FullName = model.FULLNAME;
                //serializeModel.roles = roles;

                string userData = JsonConvert.SerializeObject(serializeModel);
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1, user.USERNAME, DateTime.Now, DateTime.Now.AddMinutes(Kerberos.TICKET_TIMEOUT_MINUTES), false, userData);
                string encTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                Response.Cookies.Add(faCookie);

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

            List<UserAdminViewModel> adminVM = GetAdminList();
            return View(adminVM);
        }

        public List<UserAdminViewModel> GetAdminList()
        {
            var admins = (from a in db.USER_ADMINS
                          select new UserAdminViewModel
                          {
                              USER_ID = a.USER_ID,
                              USERNAME = a.USERNAME,
                              PASSWORD = a.PASSWORD,
                              FULLNAME = a.FULLNAME,
                              CREATEDATE = a.CREATEDATE,
                              ISACTIVE = a.ISACTIVE ?? false
                          }).ToList();
            
            return admins;
        }

        public USER_ADMINS GetLoginAdmin()
        {
            loggedAdmin = db.USER_ADMINS.Where(u => u.USERNAME == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();
            //ViewBag.loggedUser = loggedAdmin.USERNAME;
            //ViewBag.loggedAdmin = db.USER_ADMINS.Find(loggedAdmin.USERNAME);
            ViewBag.loggedUser = loggedAdmin.FULLNAME;
            return loggedAdmin;
        }

        #region Grid CRUD Functions

        [HttpPost]
        public ActionResult List_Read([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<USER_ADMINS> userAdmins = db.USER_ADMINS;
            var model = (from ua in userAdmins
                         select new UserAdminViewModel
                         {
                             USER_ID = ua.USER_ID,
                             USERNAME = ua.USERNAME,
                             PASSWORD = ua.PASSWORD,
                             FULLNAME = ua.FULLNAME,
                             ISACTIVE = ua.ISACTIVE ?? false,
                             CREATEDATE = (DateTime)ua.CREATEDATE
                         }).ToList();
            DataSourceResult result = model.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult List_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<UserAdminViewModel> userAdmins)
        {
            var results = new List<UserAdminViewModel>();
            foreach (var userAdmin in userAdmins)
            {
                if (userAdmin != null && ModelState.IsValid)
                {
                    USER_ADMINS newUserAdmin = new USER_ADMINS()
                    {
                        USERNAME = userAdmin.USERNAME,
                        PASSWORD = userAdmin.PASSWORD,
                        FULLNAME = userAdmin.FULLNAME,
                        ISACTIVE = userAdmin.ISACTIVE,
                        CREATEDATE = userAdmin.CREATEDATE
                    };
                    db.USER_ADMINS.Add(newUserAdmin);
                    db.SaveChanges();
                    results.Add(userAdmin);
                }
            }
            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult List_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<UserAdminViewModel> userAdmins)
        {
            if (userAdmins != null && ModelState.IsValid)
            {
                foreach (var userAdmin in userAdmins)
                {
                    USER_ADMINS modifieduserAdmin = db.USER_ADMINS.Where(mod => mod.USER_ID.Equals(userAdmin.USER_ID)).FirstOrDefault();
                    modifieduserAdmin.USER_ID = userAdmin.USER_ID;
                    modifieduserAdmin.USERNAME = userAdmin.USERNAME;
                    modifieduserAdmin.PASSWORD = userAdmin.PASSWORD;
                    modifieduserAdmin.FULLNAME = userAdmin.FULLNAME;
                    modifieduserAdmin.ISACTIVE = userAdmin.ISACTIVE;
                    modifieduserAdmin.CREATEDATE = userAdmin.CREATEDATE;
                    db.Entry(modifieduserAdmin).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return Json(userAdmins.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult List_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<UserAdminViewModel> userAdmins)
        {
            if (userAdmins.Any())
            {
                foreach (var userAdmin in userAdmins)
                {
                    if (userAdmin != null)
                    {
                        USER_ADMINS modifiedAdmin = db.USER_ADMINS.Find(userAdmin.USER_ID);
                        db.Entry(modifiedAdmin).State = EntityState.Deleted;
                        db.USER_ADMINS.Remove(modifiedAdmin);
                        db.SaveChanges();
                    }
                }
            }
            return Json(userAdmins.ToDataSourceResult(request, ModelState));
        }

        #endregion

    }
}
