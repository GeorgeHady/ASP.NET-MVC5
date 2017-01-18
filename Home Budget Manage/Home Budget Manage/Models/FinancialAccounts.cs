using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Home_Budget_Manage.Models
{
    [System.Web.Mvc.ValidateAntiForgeryToken]
    [Table("Financial Accounts")]
    public class FinancialAccounts
    {
        [Key]
        public int id { get; set; }


        [Display(Name = "Account Name")]
        [Required(ErrorMessage = "Enter Account Name.")]
        public string accountName { get; set; }


        [Display(Name = "Account Type")]
        [Required(ErrorMessage = "Enter Account Type, Eg:bank account, cash, card, .. etc.")]
        public string accountType { get; set; }


        [Display(Name = "Other Details")]
        public string otherDetails { get; set; }


        [Display(Name = "Registration Date")]
        public string registrationDate { get; set; }



        //RelaitionShip with Users_ManyToMany_FinancialAccounts__And__Permissions, this is side "One"
        public List<Permissions> FinancialAccounts_OTM_permissions { get; set; }


        //RelaitionShip with Users_ManyToMany_FinancialAccounts__And__Permissions, this is side "One"
        public List<AccountingEntries> FinancialAccounts_OTM_AccountingEntries { get; set; }
    }

}