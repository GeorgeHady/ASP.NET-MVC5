using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Home_Budget_Manage.Models
{
    [System.Web.Mvc.ValidateAntiForgeryToken]
    [Table("Accounting Entries")]
    public class AccountingEntries
    {
        [Key]
        public int id { get; set; }


        [Required(ErrorMessage = "Select the Financial Account!")]
        public int FK_FinancialAccounts_id { get; set; }


        [Display(Name = "Entry Type")]
        //[Required(ErrorMessage = "Enter Account Entry Type")]
        [EnumDataType(typeof(EntryTypeList))]
        public string entryType { get; set; }


        [Display(Name = "Amount")]
        [Required(ErrorMessage = "Enter Amount")]
        [DataType(DataType.Currency)]
        //[RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal amount { get; set; }


        [Display(Name = "Details")]
        public string details { get; set; }



        [Display(Name = "Date")]
        [Required(ErrorMessage = "Enter Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime date_byUser { get; set; }





        [Display(Name = "User Name")]
        public string User { get; set; }


        [Display(Name = "Date of Entry")]
        public string date_inDatabse { get; set; }


        public string TransferEntryCode { get; set; }


        //RelaitionShip with AccountingEntries, this is side "Many"
        [ForeignKey("FK_FinancialAccounts_id")]
        public virtual FinancialAccounts AccountingEntries_MTO_FinancialAccounts { get; set; }
    }


    public enum EntryTypeList
    {
        Income = 1,
        Expense = 2,
        Transfer = 3
    }

}