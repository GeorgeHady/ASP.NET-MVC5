using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Home_Budget_Manage.Models
{
    public class Permissions
    {
        [Key]
        public int id { get; set; }



        [Required(ErrorMessage = "Select the User!")]
        public int FK_Users_id { get; set; }


        [Required(ErrorMessage = "Select the Financial Account!")]
        public int FK_FinancialAccounts_id { get; set; }




        [Display(Name = "Modify The Permissions")]
        public bool modificationPermission { get; set; }


        [Display(Name = "Use The Account")]
        public bool useAccount { get; set; }




        //RelaitionShip with Users, this is side "Many"
        [ForeignKey("FK_Users_id")]
        public virtual Users permissions_MTO_user { get; set; }


        //RelaitionShip with FinancialAccounts, this is side "Many"
        [ForeignKey("FK_FinancialAccounts_id")]
        public virtual FinancialAccounts permissions_MTO_FinancialAccounts { get; set; }

    }
}