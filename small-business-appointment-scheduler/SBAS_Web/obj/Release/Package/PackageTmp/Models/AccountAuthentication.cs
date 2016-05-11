// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Ye Peng
// Created          : 06-10-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="AccountAuthentication.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;


/// <summary>
/// The Models namespace.
/// </summary>
namespace SBAS_Web.Models
{
    /// <summary>
    /// Class ExternalLoginConfirmationViewModel.
    /// </summary>
    public class ExternalLoginConfirmationViewModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    /// <summary>
    /// Class ExternalLoginListViewModel.
    /// </summary>
    public class ExternalLoginListViewModel
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public string Action { get; set; }
        /// <summary>
        /// Gets or sets the return URL.
        /// </summary>
        /// <value>The return URL.</value>
        public string ReturnUrl { get; set; }
    }

    /// <summary>
    /// Class ManageUserViewModel.
    /// </summary>
    public class ManageUserViewModel
    {
        /// <summary>
        /// Gets or sets the old password.
        /// </summary>
        /// <value>The old password.</value>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        /// <value>The new password.</value>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        /// <value>The confirm password.</value>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// Class ManageUserInfoViewModel.
    /// </summary>
    public class ManageUserInfoViewModel
    {
        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>The name of the company.</value>
        [Display(Name = "Company Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "The {0} must be no greater than {1} characters long.")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "The {0} must be no greater than {1} characters long.")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the address line1.
        /// </summary>
        /// <value>The address line1.</value>
        [Required]
        [Display(Name = "Address Line 1")]
        [StringLength(150, ErrorMessage = "The {0} must be no greater than {1} characters long.")]
        public string AddressLine1 { get; set; }


        /// <summary>
        /// Gets or sets the address line2.
        /// </summary>
        /// <value>The address line2.</value>
        [StringLength(100, ErrorMessage = "The {0} must be no greater than {1} characters long.")]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the selected city identifier.
        /// </summary>
        /// <value>The selected city identifier.</value>
        public long SelectedCityID { get; set; }
        /// <summary>
        /// Gets or sets the selected state identifier.
        /// </summary>
        /// <value>The selected state identifier.</value>
        public long SelectedStateID { get; set; }

        /// <summary>
        /// Gets or sets the city list.
        /// </summary>
        /// <value>The city list.</value>
        [Display(Name = "City")]
        public SelectList CityList { get; set; }
        /// <summary>
        /// Gets or sets the state list.
        /// </summary>
        /// <value>The state list.</value>
        [Display(Name = "State")]
        public SelectList StateList { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>The zip code.</value>
        [Required]
        [Display(Name = "Zip Code")]
        [DataType(DataType.PostalCode)]
        [StringLength(10, ErrorMessage = "The {0} must be no greater than {1} and at least {2} characters long.", MinimumLength = 5)]
        [RegularExpression(@"[\d+]{5,10}", ErrorMessage = "Please enter correct Zip Code")]
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>The phone number.</value>
        [Required]
        [Display(Name = "Phone Number")]
        [Phone]
        [StringLength(20, ErrorMessage = "The {0} must be no greater than {1} and at least {2} characters long.", MinimumLength = 7)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the mobile number.
        /// </summary>
        /// <value>The mobile number.</value>
        [Required]
        [Display(Name = "Mobile Number")]
        [Phone]
        [StringLength(20, ErrorMessage = "The {0} must be no greater than {1} and at least {2} characters long.", MinimumLength = 7)]
        public string MobileNumber { get; set; }

        /// <summary>
        /// Gets or sets the fax number.
        /// </summary>
        /// <value>The fax number.</value>
        [Required]
        [Display(Name = "Fax Number")]
        [Phone]
        [StringLength(20, ErrorMessage = "The {0} must be no greater than {1} and at least {2} characters long.", MinimumLength = 7)]
        public string FaxNumber { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public decimal? Longitude { get; set; }
    }

    /// <summary>
    /// Class LoginViewModel.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [remember me].
        /// </summary>
        /// <value><c>true</c> if [remember me]; otherwise, <c>false</c>.</value>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// Class RegisterViewModel.
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>The name of the company.</value>
        [Required]
        [Display(Name = "Company Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "The {0} must be no greater than {1} characters long.")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "The {0} must be no greater than {1} characters long.")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the address line1.
        /// </summary>
        /// <value>The address line1.</value>
        [Required]
        [Display(Name = "Address Line 1")]
        [StringLength(150, ErrorMessage = "The {0} must be no greater than {1} characters long.")]
        public string AddressLine1 { get; set; }


        /// <summary>
        /// Gets or sets the address line2.
        /// </summary>
        /// <value>The address line2.</value>
        [StringLength(100, ErrorMessage = "The {0} must be no greater than {1} characters long.")]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the selected city identifier.
        /// </summary>
        /// <value>The selected city identifier.</value>
        public long SelectedCityID { get; set; }
        /// <summary>
        /// Gets or sets the selected state identifier.
        /// </summary>
        /// <value>The selected state identifier.</value>
        public long SelectedStateID { get; set; }

        /// <summary>
        /// Gets or sets the city list.
        /// </summary>
        /// <value>The city list.</value>
        [Display(Name = "City")]
        public SelectList CityList { get; set; }
        /// <summary>
        /// Gets or sets the state list.
        /// </summary>
        /// <value>The state list.</value>
        [Display(Name = "State")]
        public SelectList StateList { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>The zip code.</value>
        [Required]
        [Display(Name = "Zip Code")]
        [DataType(DataType.PostalCode)]
        [StringLength(10, ErrorMessage = "The {0} must be no greater than {1} and at least {2} characters long.", MinimumLength = 5)]
        [RegularExpression(@"[\d+]{5,10}", ErrorMessage = "Please enter correct Zip Code")]
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>The phone number.</value>
        [Required]
        [Display(Name = "Phone Number")]
        [Phone]
        [StringLength(20, ErrorMessage = "The {0} must be no greater than {1} and at least {2} characters long.", MinimumLength = 7)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the mobile number.
        /// </summary>
        /// <value>The mobile number.</value>
        [Required]
        [Display(Name = "Mobile Number")]
        [Phone]
        [StringLength(20, ErrorMessage = "The {0} must be no greater than {1} and at least {2} characters long.", MinimumLength = 7)]
        public string MobileNumber { get; set; }

        /// <summary>
        /// Gets or sets the fax number.
        /// </summary>
        /// <value>The fax number.</value>
        [Required]
        [Display(Name = "Fax Number")]
        [Phone]
        [StringLength(20, ErrorMessage = "The {0} must be no greater than {1} and at least {2} characters long.", MinimumLength = 7)]
        public string FaxNumber { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        /// <value>The confirm password.</value>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>The name of the role.</value>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public decimal? Longitude { get; set; }
     }

    /// <summary>
    /// Class RegisterClientViewModel.
    /// </summary>
    public class RegisterClientViewModel
    {

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>The name of the company.</value>
        [Display(Name = "Company Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "The {0} must be no greater than {1} characters long.")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "The {0} must be no greater than {1} characters long.")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the address line1.
        /// </summary>
        /// <value>The address line1.</value>
        [Required]
        [Display(Name = "Address Line 1")]
        [StringLength(150, ErrorMessage = "The {0} must be no greater than {1} characters long.")]
        public string AddressLine1 { get; set; }


        /// <summary>
        /// Gets or sets the address line2.
        /// </summary>
        /// <value>The address line2.</value>
        [StringLength(100, ErrorMessage = "The {0} must be no greater than {1} characters long.")]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the selected city identifier.
        /// </summary>
        /// <value>The selected city identifier.</value>
        public long SelectedCityID { get; set; }
        /// <summary>
        /// Gets or sets the selected state identifier.
        /// </summary>
        /// <value>The selected state identifier.</value>
        public long SelectedStateID { get; set; }

        /// <summary>
        /// Gets or sets the city list.
        /// </summary>
        /// <value>The city list.</value>
        [Display(Name = "City")]
        public SelectList CityList { get; set; }
        /// <summary>
        /// Gets or sets the state list.
        /// </summary>
        /// <value>The state list.</value>
        [Display(Name = "State")]
        public SelectList StateList { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>The zip code.</value>
        [Required]
        [Display(Name = "Zip Code")]
        [DataType(DataType.PostalCode)]
        [StringLength(10, ErrorMessage = "The {0} must be no greater than {1} and at least {2} characters long.", MinimumLength = 5)]
        [RegularExpression(@"[\d+]{5,10}", ErrorMessage = "Please enter correct Zip Code")]
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>The phone number.</value>
        [Required]
        [Display(Name = "Phone Number")]
        [Phone]
        [StringLength(20, ErrorMessage = "The {0} must be no greater than {1} and at least {2} characters long.", MinimumLength = 7)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the mobile number.
        /// </summary>
        /// <value>The mobile number.</value>
        [Required]
        [Display(Name = "Mobile Number")]
        [Phone]
        [StringLength(20, ErrorMessage = "The {0} must be no greater than {1} and at least {2} characters long.", MinimumLength = 7)]
        public string MobileNumber { get; set; }

        /// <summary>
        /// Gets or sets the fax number.
        /// </summary>
        /// <value>The fax number.</value>
        [Required]
        [Display(Name = "Fax Number")]
        [Phone]
        [StringLength(20, ErrorMessage = "The {0} must be no greater than {1} and at least {2} characters long.", MinimumLength = 7)]
        public string FaxNumber { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        /// <value>The confirm password.</value>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>The name of the role.</value>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public decimal? Longitude { get; set; }
    }


    /// <summary>
    /// Class ResetPasswordViewModel.
    /// </summary>
    public class ResetPasswordViewModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        /// <value>The confirm password.</value>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }
    }

    /// <summary>
    /// Class ForgotPasswordViewModel.
    /// </summary>
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    /// <summary>
    /// Class CitListViewModel.
    /// </summary>
    public class CitListViewModel
    {
        /// <summary>
        /// Gets or sets the selected city identifier.
        /// </summary>
        /// <value>The selected city identifier.</value>
        public long SelectedCityID { get; set; }
        /// <summary>
        /// Gets or sets the city list.
        /// </summary>
        /// <value>The city list.</value>
        [Display(Name = "City")]
        public SelectList CityList { get; set; }
    }
}
