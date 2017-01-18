using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Home_Budget_Manage.Models
{
    public class UsersView
    {
    }

    [System.Web.Mvc.ValidateAntiForgeryToken]
    public class RegisterView
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


        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match!")]
        public string ConfirmPassword { get; set; }
    }


    public class EditUserInfoView
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
    }


    public class ChangePasswordView
    {
        [Key]
        public int id { get; set; }


        [Display(Name = "Current password")]
        [Required(ErrorMessage = "Enter Current password.")]
        [DataType(DataType.Password)]
        public string password { get; set; }



        [Display(Name = "New password")]
        [Required(ErrorMessage = "Enter New password.")]
        [DataType(DataType.Password)]
        public string newPassword { get; set; }


        [Display(Name = "Confirm new password")]
        [DataType(DataType.Password)]
        [Compare("newPassword", ErrorMessage = "The new password and confirmation password do not match!")]
        public string newConfirmPassword { get; set; }
    }


    //[System.Web.Mvc.ValidateAntiForgeryToken]
    [Table("Accounts Summary")]
    public class AccountsSummary
    {
        [Key]
        public int id { get; set; }


        [Display(Name = "Account Name")]
        public string accountName { get; set; }


        [Display(Name = "Account Type")]
        public string accountType { get; set; }


        [Display(Name = "Account Balance")]
        [DataType(DataType.Currency)]
        public decimal accountBalance { get; set; }

    }
}
