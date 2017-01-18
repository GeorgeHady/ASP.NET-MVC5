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
    public class PermissionsController : Controller
    {
        private DB db = new DB();


        public ActionResult Index()
        {
            if (Session["userId"] != null)
            {
                var table_Permissions = db.table_Permissions
                .Include(p => p.permissions_MTO_FinancialAccounts)
                .Include(p => p.permissions_MTO_user);

                int userId = Convert.ToInt16(Session["userId"]);
                table_Permissions = table_Permissions.Where(x => x.FK_Users_id == userId).Where(x => x.useAccount == true);

                return View(table_Permissions.ToList());
            }
            return RedirectToAction("Login", "UsersView");
        }










        public ActionResult AddUptadeDelete(int? idAccount)
        {
            if (Session["userId"] != null)
            {

                int id = Convert.ToInt16(Session["userId"]);

                var Accounts = from p in db.table_Permissions
                               join fa in db.table_FinancialAccounts on p.FK_FinancialAccounts_id equals fa.id
                               join u in db.table_users on p.FK_Users_id equals u.id
                               where u.id == id && p.modificationPermission == true
                               select fa;

                //var users = db.table_users.Where(x => x.id != i);
                ViewBag.FK_Users_id = new SelectList(db.table_users, "id", "Name");

                if (idAccount != null)
                {
                    Permissions permissionView = db.table_Permissions.Find(idAccount);
                    ViewBag.FK_FinancialAccounts_id = new SelectList(Accounts, "id", "accountName", permissionView.FK_FinancialAccounts_id);

                    return View(permissionView);
                }
                ViewBag.FK_FinancialAccounts_id = new SelectList(Accounts, "id", "accountName");

                return View();
            }
            return RedirectToAction("Login", "UsersView");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUptadeDelete([Bind(Include = "id,FK_Users_id,FK_FinancialAccounts_id,modificationPermission,useAccount")] Permissions permissions, string submitButton)
        {
            if (Session["userId"] != null)
            {
                ViewBag.StatusMessage = null;
                int i = Convert.ToInt16(Session["userId"]);

                var Accounts = from p in db.table_Permissions
                               join fa in db.table_FinancialAccounts on p.FK_FinancialAccounts_id equals fa.id
                               join u in db.table_users on p.FK_Users_id equals u.id
                               where u.id == i && p.modificationPermission == true
                               select fa;
                ViewBag.FK_FinancialAccounts_id = new SelectList(Accounts, "id", "accountName");

                ViewBag.FK_Users_id = new SelectList(db.table_users, "id", "Name");
              
                if (!ModelState.IsValid)
                {
                    return View(permissions);
                }

                var exists = (from p in db.table_Permissions
                              where p.FK_Users_id == permissions.FK_Users_id
                              && p.FK_FinancialAccounts_id == permissions.FK_FinancialAccounts_id
                              select p).FirstOrDefault();

                /////////////////////////////////   Switch button   /////////////////////////////////
                switch (submitButton)
                {
                    case "Add":

                        if (exists != null)
                        {    
                            ModelState.AddModelError("", "The permission that already exists!");
                            return View(permissions);
                        }

                        db.table_Permissions.Add(permissions);
                        db.SaveChanges();

                        ViewBag.StatusMessage = "The permission Added successfully.";
                        return View(permissions);

                    /////////////////////////////////
                    case "Update":

                        if (exists == null)
                        {
                            ModelState.AddModelError("", "The permission that not exists! Click on 'Add'.");
                            return View(permissions);
                        }
                        if(exists.modificationPermission == false)
                        {
                            ModelState.AddModelError("", "You don't have The permission to do Update.");
                            return View(permissions);
                        }
                        var updatePermissions = db.table_Permissions
                            .Where(a => a.FK_Users_id == permissions.FK_Users_id)
                            .Where(a => a.FK_FinancialAccounts_id == permissions.FK_FinancialAccounts_id)
                            .FirstOrDefault();

                        db.Entry(updatePermissions).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.StatusMessage = "The permission Updated successfully.";
                        return View(permissions);

                        /////////////////////////////////
                    case "Delete":

                        if (exists == null)
                        {
                            //ViewBag.StatusMessage = "The permission that not exists!";
                            ModelState.AddModelError("", "The permission that not exists!");
                            return View(permissions);
                        }
                        if (exists.modificationPermission == false)
                        {
                            ModelState.AddModelError("", "You don't have The permission to do Update.");
                            return View(permissions);
                        }

                        var deletePermissions = db.table_Permissions
                            .Where(a => a.FK_Users_id == permissions.FK_Users_id)
                            .Where(a => a.FK_FinancialAccounts_id == permissions.FK_FinancialAccounts_id)
                            .FirstOrDefault();

                        Permissions p = db.table_Permissions.Find(deletePermissions.id);
                        db.table_Permissions.Remove(p);
                        db.SaveChanges();

                        ViewBag.StatusMessage = "The permission Deleted successfully.";
                        return View();

                    /////////////////////////////////
                    default:
                        return View();
                }
            }
            return RedirectToAction("Login", "UsersView");
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
