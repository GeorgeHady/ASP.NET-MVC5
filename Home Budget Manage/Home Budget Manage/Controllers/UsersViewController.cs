using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Home_Budget_Manage.Models;

namespace Home_Budget_Manage.Controllers
{
    public class UsersViewController : Controller
    {
        private DB db = new DB();

        public ActionResult Index()
        {
            if (Session["userId"] != null)
            {
                int id = Convert.ToInt16(Session["userId"]);

                var entries = (from fa in db.table_FinancialAccounts
                               join e in db.table_AccountingEntries on fa.id equals e.FK_FinancialAccounts_id
                               join p in db.table_Permissions on fa.id equals p.FK_FinancialAccounts_id
                               join u in db.table_users on p.FK_Users_id equals u.id
                               where u.id == id
                               group e by new
                               {
                                   fa.accountName,
                                   fa.accountType
                               } into gp
                               select new AccountsSummary
                               {
                                   accountName = gp.Key.accountName,
                                   accountType = gp.Key.accountType,
                                   accountBalance = gp.Sum(u => u.amount)
                               }).ToList();

                return View(entries);

            }
            return RedirectToAction("Login", "UsersView");
        }
             
        
        
        #region Details
        public ActionResult Details(int? id)
        {
            if (Session["StatusMessage"] != null)
            {
                ViewBag.SuccessMessage = Session["StatusMessage"];
                @Session["StatusMessage"] = null;
            }

            if (Session["userId"] != null)
            {
                Users userDetails = db.table_users.Find(Session["userId"]);
                if (userDetails == null)
                {
                    return HttpNotFound();
                }
                return View(userDetails);
            }
            return RedirectToAction("Login");
        }
        #endregion


        //#region Partial Registering New User

        //[HttpGet]
        //public ActionResult RegisterPartial()
        //{
        //    return PartialView();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult RegisterPartial([Bind(Include = "id,Name,email,userName,password,ConfirmPassword")] RegisterView view)
        //{
        //    Users newUser = new Users();

        //    if (ModelState.IsValid)
        //    {
        //        newUser.Name = view.Name;
        //        newUser.email = view.email;
        //        newUser.userName = view.userName;
        //        newUser.password = view.ConfirmPassword;
        //        newUser.registrationDate = DateTime.Now.ToString("d/M/yyyy hh:mm tt");

        //        db.table_users.Add(newUser);
        //        db.SaveChanges();

        //        Session["userId"] = newUser.id;
        //        Session["userName"] = newUser.userName;

        //        Response.Redirect("~/UsersView/details/" + newUser.id);
        //        return null;
        //    }

        //    return PartialView(view);
        //}
        //#endregion


        //#region Login Partial

        //[HttpGet]
        //public ActionResult LoginPartial()
        //{
        //    return PartialView();
        //}

        //[HttpPost]
        //public ActionResult LoginPartial(string email_or_userName, string passwordLogin)
        //{
        //    if (db.table_users.FirstOrDefault(x => x.email == email_or_userName && x.password == passwordLogin) != null)
        //    {
        //        var model = db.table_users.FirstOrDefault(x => x.email == email_or_userName && x.password == passwordLogin);

        //        Session["userId"] = model.id;
        //        Session["userName"] = model.userName;

        //        Response.Redirect("~/UsersView/details/" + model.id);
        //        return null;
        //    }
        //    else if (db.table_users.FirstOrDefault(x => x.userName == email_or_userName && x.password == passwordLogin) != null)
        //    {
        //        var model = db.table_users.FirstOrDefault(x => x.userName == email_or_userName && x.password == passwordLogin);

        //        Session["userId"] = model.id;
        //        Session["userName"] = model.userName;

        //        Response.Redirect("~/UsersView/details/" + model.id);
        //        return null;
        //    }

        //    return PartialView();
        //}
        //#endregion


        #region Login

        [HttpGet]
        public ActionResult Login()
        {
                ViewBag.NotLogin = null;

                if (Session["id"] != null)
                {
                    return RedirectToAction("Details");
                }
                return View();
        }

        [HttpPost]
        public ActionResult Login(string email_or_userName, string password)
        {
            if (Session["userId"] == null)
            {
                if (db.table_users.FirstOrDefault(x => x.email == email_or_userName && x.password == password) != null)
                {
                    var model = db.table_users.FirstOrDefault(x => x.email == email_or_userName && x.password == password);

                    Session["userId"] = model.id;
                    Session["userName"] = model.userName;
                    return RedirectToAction("Details");
                }
                else if (db.table_users.FirstOrDefault(x => x.userName == email_or_userName && x.password == password) != null)
                {
                    var model = db.table_users.FirstOrDefault(x => x.userName == email_or_userName && x.password == password);

                    Session["userId"] = model.id;
                    Session["userName"] = model.userName;
                    return RedirectToAction("Index");
                }
                return View();
            }
            return RedirectToAction("Details");
        }
        #endregion


        #region Registering New User

        [HttpGet]
        public ActionResult Register()
        {
            if (Session["userId"] == null)
            {
                return View();
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "id,Name,email,userName,password,ConfirmPassword")] RegisterView view)
        {
            if (Session["userId"] == null)
            {
                Users newUser = new Users();

                if (ModelState.IsValid)
                {
                    newUser.Name = view.Name;
                    newUser.email = view.email;
                    newUser.userName = view.userName;
                    newUser.password = view.ConfirmPassword;
                    newUser.registrationDate = DateTime.Now.ToString("d/M/yyyy hh:mm tt");

                    db.table_users.Add(newUser);
                    db.SaveChanges();

                    Session["userId"] = newUser.id;
                    Session["userName"] = newUser.userName;

                    return RedirectToAction("Details");
                }

                return View(view);
            }
            return RedirectToAction("Details");
        }
        #endregion


        #region Edit User's Information

        [HttpGet]
        public ActionResult Edit()
        {
            try
            {
                if (Session["userId"] != null)
                {
                    Users userDetails = db.table_users.Find(Session["userId"]);
                    EditUserInfoView view = new EditUserInfoView();
                    view.id = userDetails.id;
                    view.Name = userDetails.Name.ToString();
                    view.email = userDetails.email;
                    view.userName = userDetails.userName;

                    if (view == null)
                    {
                        return HttpNotFound();
                    }
                    return View(view);
                }
                return RedirectToAction("Login");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Name,email,userName")] EditUserInfoView view)
        {
            if (Session["userId"] != null)
            {
                ViewBag.ErrorMassage = null;
                Session["StatusMessage"] = null;

                if (ModelState.IsValid)
                {
                    try
                    {
                        Users userDetails = db.table_users.Find(view.id);

                        userDetails.Name = view.Name;
                        userDetails.email = view.email;
                        userDetails.userName = view.userName;

                        db.Entry(userDetails).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        Session["StatusMessage"] = "Your information has been changed.";
                        return RedirectToAction("Details");
                    }
                    catch
                    {
                        ViewBag.ErrorMassage = "Error";
                    }
                }
                return View(view);
            }
            return RedirectToAction("Login");
        }
        #endregion 


        #region ChangePassword of User

        [HttpGet]
        public ActionResult ChangePassword()
        {
            try
            {
                if (Session["userId"] != null)
                {
                    Users userDetails = db.table_users.Find(Session["userId"]);
                    ChangePasswordView view = new ChangePasswordView();
                    view.id = userDetails.id;

                    //if (view == null)
                    //{
                    //    return HttpNotFound();
                    //}
                    return View(view);
                }
                return RedirectToAction("Login");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword([Bind(Include = "id,password,newPassword,newConfirmPassword")] ChangePasswordView view)
        {
            if (Session["userId"] != null)
            {
                if (ModelState.IsValid)
                {
                    ViewBag.ErrorMassage = null;
                    Session["StatusMessage"] = null;

                    Users userDetails = db.table_users.Find(view.id);
                    if (userDetails.password == view.password)
                    {
                        userDetails.password = view.newConfirmPassword;

                        db.Entry(userDetails).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        Session["StatusMessage"] = "Your password has been changed.";
                        return RedirectToAction("Details");
                    }
                    else
                    {
                        ViewBag.ErrorMassage = "Incorrect password.";
                    }
                }
                ChangePasswordView view1 = new ChangePasswordView();
                view1.id = view.id;
                return View(view1);
            }
            return RedirectToAction("Login");
        }
        #endregion


        #region Delete User
        public ActionResult Delete()
        {
            try
            {
                if (Session["userId"] != null)
                {
                    Users userDetails = db.table_users.Find(Session["userId"]);
                    if (userDetails == null)
                    {
                        return HttpNotFound();
                    }
                    return View(userDetails);
                }
                return RedirectToAction("Login");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string password)
        {
            if (Session["userId"] != null)
            {
                Session["StatusMessage"] = null;
                ViewBag.passStatus = null;

                Users userDetails = db.table_users.Find(Session["userId"]);
                if (password != "" && password != null)
                {
                    if (password == userDetails.password)
                    {
                        db.table_users.Remove(userDetails);
                        db.SaveChanges();

                        Session["StatusMessage"] = "Your account has been deleted.";
                        return RedirectToAction("Index");
                    }
                    ViewBag.passStatus = "Incorrect password.";
                    return View(userDetails);
                }
                ViewBag.passStatus = "Please enter your password to confirm the deletion.";
                return View(userDetails);
            }
            return RedirectToAction("Login");
        }
        #endregion


        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
