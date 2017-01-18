using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Home_Budget_Manage.Models
{
    [System.Web.Mvc.ValidateAntiForgeryToken]
    [Table("Users Informatiom")]
    public class Users
    {
        [Key]
        public int id { get; set; }


        [Display(Name = "Your Full Name")]
        [Required(ErrorMessage = "Enter Your Full Name")]
        public string Name { get; set; }


        [Display(Name = "Email")]
        [Required(ErrorMessage = "Enter Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string email { get; set; }


        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Enter User Name")]
        public string userName { get; set; }


        [Display(Name = "Password")]
        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }


        [Display(Name = "Registration Date")]
        public string registrationDate { get; set; }



        //RelaitionShip with Users_ManyToMany_FinancialAccounts__And__Permissions, this is side "One"
        public List<Permissions> Users_OTM_permissions { get; set; }
        //public ICollection <Users_ManyToMany_FinancialAccounts__And__Permissions> Users_OTM_permissions { get; set; }
    }

}