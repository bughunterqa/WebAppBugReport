using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebAppBugReport.Data;
using WebAppBugReport.Data.Models;
using WebAppBugReport.Data.ViewModels;
using System.Data.Entity;
using System.Net;


namespace WebAppBugReport.Controllers
{
    public class AccountController : Controller
    {
        AppDbContext db = new AppDbContext();
        public ActionResult Register()
        {
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
               


                User user = null;
                using (AppDbContext db = new AppDbContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Email == model.Email);
                    ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name");
                }
                if (user == null)
                {
                    using (AppDbContext db = new AppDbContext())
                    {
                        db.Users.Add(new User { Name = model.Name, Email = model.Email, Password = model.Password, RoleId = model.RoleId });
                        db.SaveChanges();

                        user = db.Users.Where(u => u.Email == model.Email && u.Password == model.Password).FirstOrDefault();
                    }

                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Email, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с такими учёными данными уже существует в системе");
                }
            }
            return View(model);
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (AppDbContext db = new AppDbContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, true);
                    return RedirectToAction("Index", "Projects");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с такими учёными данными несуществует в системе");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }



    }
}