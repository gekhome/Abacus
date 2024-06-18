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
    public class USER_STATIONSController : Controller
    {
        private readonly AbacusDBEntities db;
        private USER_STATIONS loggedStation;

        public USER_STATIONSController()
        {
            db = new AbacusDBEntities();
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
                loggedStation = db.USER_STATIONS.Where(u => u.USERNAME == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();
                if (loggedStation != null)
                {
                    ViewBag.loggedUser = GetLoginStation();
                    return RedirectToAction("Index", "Station");
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "USERNAME,PASSWORD")]  UserStationViewModel model)
        {
            var user = db.USER_STATIONS.Where(u => u.USERNAME == model.USERNAME && u.PASSWORD == model.PASSWORD).FirstOrDefault();

            if (user != null)
            {
                StationPrincipalSerializeModel serializeModel = new StationPrincipalSerializeModel();
                serializeModel.UserId = model.USER_ID;
                serializeModel.Username = model.USERNAME;
                serializeModel.StationId = model.STATION_ID;

                string userData = JsonConvert.SerializeObject(serializeModel);
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1, user.USERNAME, DateTime.Now, DateTime.Now.AddMinutes(Kerberos.TICKET_TIMEOUT_MINUTES), false, userData);
                string encTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                Response.Cookies.Add(faCookie);

                SetLoginStatus(user, true);
                LoginRecord(user.USERNAME);
                return RedirectToAction("Index", "Station");
            }
            ModelState.AddModelError("", "Το όνομα χρήστη ή/και ο κωδ.πρόσβασης δεν είναι σωστά");
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult LogOut([Bind(Include = "ISACTIVE")] USER_STATIONS userSchool)
        {
            var user = db.USER_STATIONS.Where(u => u.USERNAME == userSchool.USERNAME && u.PASSWORD == userSchool.PASSWORD).FirstOrDefault();

            FormsAuthentication.SignOut();
            SetLoginStatus(user, false);

            return RedirectToAction("Index", "Home");
        }

        public USER_STATIONS GetLoginStation()
        {
            loggedStation = db.USER_STATIONS.Where(u => u.USERNAME == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();

            int StationID = loggedStation.STATION_ID ?? 0;
            var _school = (from s in db.sqlUSER_STATION
                           where s.STATION_ID == StationID
                           select new { s.ΕΠΩΝΥΜΙΑ }).FirstOrDefault();

            ViewBag.loggedUser = _school.ΕΠΩΝΥΜΙΑ;
            return loggedStation;
        }

        public void SetLoginStatus(USER_STATIONS user, bool value)
        {
            db.Entry(user).State = EntityState.Modified;
            user.ISACTIVE = value;
            db.SaveChanges();
        }

        public void LoginRecord(string username)
        {
            var loginData = (from d in db.SYS_LOGINS where d.LOGIN_USERNAME == username select d).FirstOrDefault();

            if (loginData == null)
            {
                SYS_LOGINS entity = new SYS_LOGINS()
                {
                    LOGIN_USERNAME = username,
                    LOGIN_DATETIME = DateTime.Now
                };
                db.SYS_LOGINS.Add(entity);
                db.SaveChanges();
            }
            else
            {
                SYS_LOGINS entity = db.SYS_LOGINS.Find(loginData.LOGIN_ID);
                entity.LOGIN_DATETIME = DateTime.Now;
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

    }
}
