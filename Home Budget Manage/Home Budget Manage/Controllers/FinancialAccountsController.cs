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
    public class FinancialAccountsController : Controller
    {
        private DB db = new DB();


        public ActionResult Index()
        {
            return View();
        }






        #region Add New Financial Account

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "id,accountName,accountType,otherDetails")] FinancialAccounts modelFinancialAccounts)
        {
            if (Session["userId"] != null)
            {
                if (ModelState.IsValid)
                {
                    modelFinancialAccounts.registrationDate = DateTime.Now.ToString("d/M/yyyy hh:mm tt");
                    db.table_FinancialAccounts.Add(modelFinancialAccounts);

                    Permissions modelPermissions = new Permissions();
                    modelPermissions.FK_Users_id = Convert.ToInt32(Session["userId"]);
                    modelPermissions.FK_FinancialAccounts_id = modelFinancialAccounts.id;
                    modelPermissions.modificationPermission = true;
                    modelPermissions.useAccount = true;
                    db.table_Permissions.Add(modelPermissions);

                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Error! , try again please.");
                        return View();
                    }

                    return RedirectToAction("Index", "Permissions");
                }
                return View(modelFinancialAccounts);
            }
            return RedirectToAction("Login", "UsersView");
        }
        #endregion






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
