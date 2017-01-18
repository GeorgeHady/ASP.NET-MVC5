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
    public class AccountingEntriesController : Controller
    {
        private DB db = new DB();


        public ActionResult Index()
        {
            //var table_AccountingEntries = db.table_AccountingEntries.Include(a => a.AccountingEntries_MTO_FinancialAccounts);
            if (Session["userId"] != null)
            {
                int userId = Convert.ToInt16(Session["userId"]);
                var E = from e in db.table_AccountingEntries
                        join fa in db.table_FinancialAccounts on e.FK_FinancialAccounts_id equals fa.id
                        join p in db.table_Permissions on fa.id equals p.FK_FinancialAccounts_id
                        join u in db.table_users on p.FK_Users_id equals u.id
                        where u.id == userId
                        orderby e.date_inDatabse descending
                        select e;

                return View(E.ToList());
            }
            return RedirectToAction("Login", "UsersView");
        }





       









        #region   Income
        public ActionResult Income()
        {
            if (Session["userId"] != null)
            {
                int userId = Convert.ToInt16(Session["userId"]);
                var useAccounts = from p in db.table_Permissions
                                  join fa in db.table_FinancialAccounts on p.FK_FinancialAccounts_id equals fa.id
                                  join u in db.table_users on p.FK_Users_id equals u.id
                                  where u.id == userId && p.useAccount == true
                                  select fa;
                ViewBag.FK_FinancialAccounts_id = new SelectList(useAccounts, "id", "accountName");
                return View();
            }
            return RedirectToAction("Login", "UsersView");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Income([Bind(Include = "id,FK_FinancialAccounts_id,amount,details,date_byUser")] AccountingEntries Entry)
        {
            if (Session["userId"] != null)
            {
                int userId = Convert.ToInt16(Session["userId"]);
                var useAccounts = from p in db.table_Permissions
                                  join fa in db.table_FinancialAccounts on p.FK_FinancialAccounts_id equals fa.id
                                  join u in db.table_users on p.FK_Users_id equals u.id
                                  where u.id == userId && p.useAccount == true
                                  select fa;
                ViewBag.FK_FinancialAccounts_id = new SelectList(useAccounts, "id", "accountName");

                Entry.entryType = "Income";
                Entry.date_inDatabse = DateTime.Now.ToString("d/M/yyyy hh:mm tt");
                Entry.User = @Session["userName"].ToString();

                if (ModelState.IsValid)
                {
                    db.table_AccountingEntries.Add(Entry);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Error! The process has not been completed, try again.");
                    }

                    return View();
                }
                return View(Entry);
            }
            return RedirectToAction("Login", "UsersView");
        }
        #endregion








        #region   Expense
        public ActionResult Expense()
        {
            if (Session["userId"] != null)
            {
                int userId = Convert.ToInt16(Session["userId"]);
            var useAccounts = from p in db.table_Permissions
                              join fa in db.table_FinancialAccounts on p.FK_FinancialAccounts_id equals fa.id
                              join u in db.table_users on p.FK_Users_id equals u.id
                              where u.id == userId && p.useAccount == true
                              select fa;
            ViewBag.FK_FinancialAccounts_id = new SelectList(useAccounts, "id", "accountName");

            return View();
            }
            return RedirectToAction("Login", "UsersView");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Expense([Bind(Include = "id,FK_FinancialAccounts_id,amount,details,date_byUser")] AccountingEntries Entry)
        {
            if (Session["userId"] != null)
            {
                int userId = Convert.ToInt16(Session["userId"]);
                var useAccounts = from p in db.table_Permissions
                                  join fa in db.table_FinancialAccounts on p.FK_FinancialAccounts_id equals fa.id
                                  join u in db.table_users on p.FK_Users_id equals u.id
                                  where u.id == userId && p.useAccount == true
                                  select fa;
                ViewBag.FK_FinancialAccounts_id = new SelectList(useAccounts, "id", "accountName");

                Entry.entryType = "Expense";
                Entry.date_inDatabse = DateTime.Now.ToString("d/M/yyyy hh:mm tt");
                Entry.User = @Session["userName"].ToString();
                Entry.amount *= -1;

                if (ModelState.IsValid)
                {
                    db.table_AccountingEntries.Add(Entry);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Error! The process has not been completed, try again.");
                    }

                    return View();
                }
                return View(Entry);
            }
            return RedirectToAction("Login", "UsersView");
        }
        #endregion









        #region   Transfer
        public ActionResult Transfer()
        {
            if (Session["userId"] != null)
            {
                int userId = Convert.ToInt16(Session["userId"]);
            var useAccounts = from p in db.table_Permissions
                              join fa in db.table_FinancialAccounts on p.FK_FinancialAccounts_id equals fa.id
                              join u in db.table_users on p.FK_Users_id equals u.id
                              where u.id == userId && p.useAccount == true
                              select fa;
            ViewBag.FROM_FK_FinancialAccounts_id = new SelectList(useAccounts, "id", "accountName");
            ViewBag.TO_FK_FinancialAccounts_id = new SelectList(useAccounts, "id", "accountName");

            return View();
            }
            return RedirectToAction("Login", "UsersView");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Transfer([Bind(Include = "id,amount,details,date_byUser")] AccountingEntries fromEntry, string FROM_FK_FinancialAccounts_id, string TO_FK_FinancialAccounts_id)
        {
            if (Session["userId"] != null)
            {
                int userId = Convert.ToInt16(Session["userId"]);
                var useAccounts = from p in db.table_Permissions
                                  join fa in db.table_FinancialAccounts on p.FK_FinancialAccounts_id equals fa.id
                                  join u in db.table_users on p.FK_Users_id equals u.id
                                  where u.id == userId && p.useAccount == true
                                  select fa;
                ViewBag.FROM_FK_FinancialAccounts_id = new SelectList(useAccounts, "id", "accountName");
                ViewBag.TO_FK_FinancialAccounts_id = new SelectList(useAccounts, "id", "accountName");

                fromEntry.entryType = "Transfer";
                fromEntry.FK_FinancialAccounts_id = Convert.ToInt32(FROM_FK_FinancialAccounts_id);
                fromEntry.date_inDatabse = DateTime.Now.ToString("d/M/yyyy hh:mm tt");
                fromEntry.User = @Session["userName"].ToString();

                Random rnd = new Random();
                fromEntry.TransferEntryCode = (rnd.Next(1, 999999999)).ToString(); 

                fromEntry.amount *= -1;

                if (ModelState.IsValid)
                {
                    db.table_AccountingEntries.Add(fromEntry);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Error! The process has not been completed, try again.");
                    }

                    AccountingEntries toEntry = new AccountingEntries();
                    toEntry = fromEntry;
                    toEntry.FK_FinancialAccounts_id = Convert.ToInt32(TO_FK_FinancialAccounts_id);
                    toEntry.amount *= -1;

                    db.table_AccountingEntries.Add(toEntry);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        try
                        {
                            AccountingEntries deleteEntry = new AccountingEntries();

                            deleteEntry = db.table_AccountingEntries.Where(x => x.TransferEntryCode == fromEntry.TransferEntryCode).FirstOrDefault();
                            db.table_AccountingEntries.Remove(deleteEntry);

                            ModelState.AddModelError("", "Error! The process has not been completed, try again.");
                        }
                        catch
                        {
                            ModelState.AddModelError("", "Error!   An error in the database! You should contact the company for processing.");
                        }
                    }
                    return View();
                }
                return View(fromEntry);
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
