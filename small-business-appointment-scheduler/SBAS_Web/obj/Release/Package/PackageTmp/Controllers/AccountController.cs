// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Ye Peng
// Created          : 05-21-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="AccountController.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
//using System.Web.Security;
using Owin;
using SBAS_Core;
using SBAS_Web.Models;
using SBAS_Core.Model;

/// <summary>
/// The Controllers namespace.
/// </summary>
namespace SBAS_Web.Controllers
{
    /// <summary>
    /// Class AccountController.
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        /// <summary>
        /// The _user manager
        /// </summary>
        private ApplicationUserManager _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        public AccountController()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        public AccountController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        /// <summary>
        /// Gets the user manager.
        /// </summary>
        /// <value>The user manager.</value>
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        /// <summary>
        /// Logins the specified return URL.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

/*
        // POST: /Account/Login        
        [HttpPost]       
        [AllowAnonymous]       
        [ValidateAntiForgeryToken]       
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)   
        {           
            if (ModelState.IsValid)    
            {              
                var user = await UserManager.FindAsync(model.Email, model.Password);        
                if (user != null)               
                {                  
                    await SignInAsync(user, model.RememberMe);        
                    //return RedirectToLocal(returnUrl);                  
                    return RedirectToAction("Index", "Customer");          
                }               
                else             
                {                 
                    ModelState.AddModelError("", "Invalid username or password.");         
                }          
            }           
            // If we got this far, something failed, redisplay form      
            return View(model);      
        }
        */

        
        //
        // POST: /Account/Login
        /// <summary>
        /// Logins the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.Email, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    //return RedirectToLocal(returnUrl);
                    string rolename = SBAS_DAL.SBASUser.GetUserRoleName(user.Id);
                    if (rolename == "Client") 
                    {
                        return RedirectToAction("Index", "Client");                        
                    }
                    else
                    {
                        return RedirectToAction("Index", "Customer");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
         

        //
        // GET: /Account/Register
        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new RegisterViewModel();

            var alist = new SBAS_DAL.Address().GetListofStates();

            var stateList = new SBAS_DAL.Address().GetListofStates().Select(x => new SelectListItem { Value = x.StateId.ToString(), Text = x.StateAbbreviation }).ToArray();

            //model.StateList = new SelectList(stateList, "Value", "Text", 0);

            var alist2 = new SBAS_DAL.Address().GetCitiesByState(alist[0].StateId);

            var citylist = alist2.Select(x => new SelectListItem { Value = x.CityId.ToString(), Text = x.City })
                    .ToArray();
            model.SelectedCityID = 2;
            model.StateList = new SelectList(stateList, "Value", "Text", -1);
            model.SelectedStateID = 2;
            model.CityList = new SelectList(citylist, "Value", "Text", -1);
            return View("Register", model);
        }

        //
        // POST: /Account/Register
        /// <summary>
        /// Registers the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Add user role to Customer
                    var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                    if (!roleManager.RoleExists("Customer"))
                    {
                        var role = new IdentityRole {Name = "Customer"};
                        roleManager.Create(role);
                    }

                    AddNewCustomer(model, user);

                    await SignInAsync(user, isPersistent: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Customer");
                }
                else
                {
                    AddErrors(result);
                }
            }

            var alist = new SBAS_DAL.Address().GetListofStates();

            var stateList = new SBAS_DAL.Address().GetListofStates().Select(x => new SelectListItem { Value = x.StateId.ToString(), Text = x.StateAbbreviation }).ToArray();

            //model.StateList = new SelectList(stateList, "Value", "Text", 0);

            var alist2 = new SBAS_DAL.Address().GetCitiesByState(alist[0].StateId);

            var citylist = alist2.Select(x => new SelectListItem { Value = x.CityId.ToString(), Text = x.City })
                    .ToArray();
            model.StateList = new SelectList(stateList, "Value", "Text", model.SelectedStateID);
            model.CityList = new SelectList(citylist, "Value", "Text", model.SelectedCityID);
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// Adds the new customer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="user">The user.</param>
        private void AddNewCustomer(RegisterViewModel model, ApplicationUser user)
        {
            var currentUser = UserManager.FindByName(user.UserName);
            var roleresult = UserManager.AddToRole(currentUser.Id, "Customer");
            var currentDateTime = DateTime.Now;
            // TIme to create SBAB user here
            // create address at first
            SBAS_Core.Model.Address aAddress = new SBAS_Core.Model.Address();
            aAddress.AddressLine1 = model.AddressLine1;
            aAddress.AddressLine2 = model.AddressLine2;
            aAddress.CityId = model.SelectedCityID;
            aAddress.StateId = model.SelectedStateID;
            aAddress.ZipCode = model.ZipCode;
            aAddress.CreateDateTime = currentDateTime;
            aAddress.UpdateDateTime = currentDateTime;
            aAddress.CreateUser = user.UserName;
            aAddress.UpdateUser = user.UserName;
            SBAS_Core.Google.Geocoder.GeocodeAddress(aAddress);
            new SBAS_DAL.Address().CreateAddress(aAddress);
            

            SBAS_Core.Model.SBASUser aSBASUser = new SBAS_Core.Model.SBASUser();
            aSBASUser.FirstName = model.FirstName;
            aSBASUser.LastName = model.LastName;
            aSBASUser.CompanyName = model.CompanyName;
            aSBASUser.PhoneNumber = model.PhoneNumber;
            aSBASUser.MobileNumber = model.MobileNumber;
            aSBASUser.FaxNumber = model.FaxNumber;
            aSBASUser.AddressId = aAddress.AddressId;
            aSBASUser.FirstStartTime = currentDateTime;
            aSBASUser.CreateDateTime = currentDateTime;
            aSBASUser.UpdateDateTime = currentDateTime;
            aSBASUser.CreateUser = user.UserName;
            aSBASUser.UpdateUser = user.UserName;
            new SBAS_DAL.SBASUser().CreateSBASUserByEmail(model.Email, aSBASUser);

            // A new user has a blank inventory automatically created for them
            new SBAS_DAL.Inventory().CreateInventoryForNewUser(aSBASUser.UserId);
        }

        //
        // GET: /Account/Register
        /// <summary>
        /// Registers the client.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public ActionResult RegisterClient()
        {
            var model = new RegisterClientViewModel();

            var alist = new SBAS_DAL.Address().GetListofStates();

            var stateList = new SBAS_DAL.Address().GetListofStates().Select(x => new SelectListItem { Value = x.StateId.ToString(), Text = x.StateAbbreviation }).ToArray();

            //model.StateList = new SelectList(stateList, "Value", "Text", 0);

            var alist2 = new SBAS_DAL.Address().GetCitiesByState(alist[0].StateId);

            var citylist = alist2.Select(x => new SelectListItem { Value = x.CityId.ToString(), Text = x.City })
                    .ToArray();
            model.SelectedCityID = 2;
            model.StateList = new SelectList(stateList, "Value", "Text", -1);
            model.SelectedStateID = 2;
            model.CityList = new SelectList(citylist, "Value", "Text", -1);
            return View("RegisterClient", model);
        }

        //
        // POST: /Account/Register
        /// <summary>
        /// Registers the client.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterClient(RegisterClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Add user role to Customer
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                    if (!roleManager.RoleExists("Client"))
                    {
                        var role = new IdentityRole {Name = "Client"};
                        roleManager.Create(role);
                    }

                    AddNewClient(model, user);

                    //await SignInAsync(user, isPersistent: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");


                    return RedirectToAction("ClientList", "Customer");
                    //return RedirectToAction(("/Customer/ClientList");
                    //return RedirectToAction("Index", "Customer");
                }
                else
                {
                    AddErrors(result);
                }
            }

            var alist = new SBAS_DAL.Address().GetListofStates();

            var stateList = new SBAS_DAL.Address().GetListofStates().Select(x => new SelectListItem { Value = x.StateId.ToString(), Text = x.StateAbbreviation }).ToArray();

            //model.StateList = new SelectList(stateList, "Value", "Text", 0);

            var alist2 = new SBAS_DAL.Address().GetCitiesByState(alist[0].StateId);

            var citylist = alist2.Select(x => new SelectListItem { Value = x.CityId.ToString(), Text = x.City })
                    .ToArray();
            model.StateList = new SelectList(stateList, "Value", "Text", model.SelectedStateID);
            model.CityList = new SelectList(citylist, "Value", "Text", model.SelectedCityID);

            // If we got this far, something failed, redisplay form
            return View("RegisterClient", model);
        }

        /// <summary>
        /// Adds the new client.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="user">The user.</param>
        private void AddNewClient(RegisterClientViewModel model, ApplicationUser user)
        {
            // Current uesr (customer)
            var currentUser = UserManager.FindByName(User.Identity.GetUserName());
            // New client
            var newClient = UserManager.FindByName(user.UserName);

            var roleresult = UserManager.AddToRole(newClient.Id, "Client");
            var currentDateTime = DateTime.Now;
            // TIme to create SBAB user here
            // create address at first
            var aAddress = new Address();
            aAddress.AddressLine1 = model.AddressLine1;
            aAddress.AddressLine2 = model.AddressLine2;
            aAddress.CityId = model.SelectedCityID;
            aAddress.StateId = model.SelectedStateID;
            aAddress.ZipCode = model.ZipCode;
            aAddress.CreateDateTime = currentDateTime;
            aAddress.UpdateDateTime = currentDateTime;
            aAddress.CreateUser = currentUser.UserName;
            aAddress.UpdateUser = currentUser.UserName;
            SBAS_Core.Google.Geocoder.GeocodeAddress(aAddress);
            new SBAS_DAL.Address().CreateAddress(aAddress);

            var aSBASUser = new SBASUser();
            aSBASUser.FirstName = model.FirstName;
            aSBASUser.LastName = model.LastName;
            aSBASUser.CompanyName = model.CompanyName;
            aSBASUser.PhoneNumber = model.PhoneNumber;
            aSBASUser.MobileNumber = model.MobileNumber;
            aSBASUser.FaxNumber = model.FaxNumber;
            aSBASUser.AddressId = aAddress.AddressId;
            aSBASUser.FirstStartTime = currentDateTime;
            aSBASUser.CreateDateTime = currentDateTime;
            aSBASUser.UpdateDateTime = currentDateTime;
            aSBASUser.CreateUser = currentUser.UserName;
            aSBASUser.UpdateUser = currentUser.UserName;
            
            new SBAS_DAL.SBASUser().CreateSBASUserByEmail(model.Email, aSBASUser);

            //Create relationship in clientlist table
            var cus = new SBAS_DAL.SBASUser().GetSBASUserByEmail(currentUser.Email);
            var cli = new SBAS_DAL.SBASUser().GetSBASUserByEmail(model.Email);
            var clientList = new ClientList {ClientId = cli.UserId, CustomerId = cus.UserId};
            new SBAS_DAL.ClientList().CreateClientList(clientList);
        }

        //
        // GET: /Account/ConfirmEmail
        /// <summary>
        /// Confirms the email.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="code">The code.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            IdentityResult result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }
            else
            {
                AddErrors(result);
                return View();
            }
        }

        //
        // GET: /Account/ForgotPassword
        /// <summary>
        /// Forgots the password.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        /// <summary>
        /// Forgots the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    ModelState.AddModelError("", "The user either does not exist or is not confirmed.");
                    return View();
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        /// <summary>
        /// Forgots the password confirmation.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(/*string code*/)
        {
            /*
            if (code == null)
            {
                return View("Error");
            }
             */

            var user = await UserManager.FindByNameAsync(User.Identity.Name);            
            
            var model = new ResetPasswordViewModel()
            {
                Email = User.Identity.Name,
                Code = UserManager.GeneratePasswordResetToken(user.Id.ToString()),
            };

            return View(model);
        }

        //
        // POST: /Account/ResetPassword
        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "No user found.");
                    return View();
                }
                
                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                //IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, UserManager.GeneratePasswordResetToken(user.Id), model.Password);
                if (result.Succeeded)
                {
                    AuthenticationManager.SignOut();
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                else
                {
                    AddErrors(result);
                    return View();
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ResetPassword
        /// <summary>
        /// Resets the password client.
        /// </summary>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [AllowAnonymous]
        public async Task<ActionResult> ResetPasswordClient(/*string code*/)
        {
            /*
            if (code == null)
            {
                return View("Error");
            }
             */

            var user = await UserManager.FindByNameAsync(User.Identity.Name);

            var model = new ResetPasswordViewModel()
            {
                Email = User.Identity.Name,
                Code = UserManager.GeneratePasswordResetToken(user.Id.ToString()),
            };

            return View(model);
        }

        //
        // POST: /Account/ResetPassword
        /// <summary>
        /// Resets the password client.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPasswordClient(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "No user found.");
                    return View();
                }

                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                //IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, UserManager.GeneratePasswordResetToken(user.Id), model.Password);
                if (result.Succeeded)
                {
                    AuthenticationManager.SignOut();
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                else
                {
                    AddErrors(result);
                    return View();
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        /// <summary>
        /// Resets the password confirmation.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/Disassociate
        /// <summary>
        /// Disassociates the specified login provider.
        /// </summary>
        /// <param name="loginProvider">The login provider.</param>
        /// <param name="providerKey">The provider key.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                await SignInAsync(user, isPersistent: false);
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        /// <summary>
        /// Manages the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        /// <summary>
        /// Manages the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Manage
        /// <summary>
        /// Manages the user information.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ManageUserInfo()
        {
            var model = new ManageUserInfoViewModel();
            var userInfo = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.GetUserName());
            var address = new SBAS_DAL.Address().GetAddressById(userInfo.AddressId);
            var alist = new SBAS_DAL.Address().GetListofStates();
            var stateList = new SBAS_DAL.Address().GetListofStates().Select(x => new SelectListItem { Value = x.StateId.ToString(), Text = x.StateAbbreviation }).ToArray();
            var alist2 = new SBAS_DAL.Address().GetCitiesByState(address.StateId);
            var citylist = alist2.Select(x => new SelectListItem { Value = x.CityId.ToString(), Text = x.City })
                    .ToArray();
                   
            model.CompanyName = userInfo.CompanyName;
            model.FirstName = userInfo.FirstName;
            model.LastName = userInfo.LastName;
            model.AddressLine1 = address.AddressLine1;
            model.AddressLine2 = address.AddressLine2;
            model.SelectedCityID = address.CityId;
            model.StateList = new SelectList(stateList, "Value", "Text", -1);
            model.SelectedStateID = address.StateId;
            model.CityList = new SelectList(citylist, "Value", "Text", -1);
            model.ZipCode = address.ZipCode;
            model.PhoneNumber = userInfo.PhoneNumber;
            model.MobileNumber = userInfo.MobileNumber;
            model.FaxNumber = userInfo.FaxNumber;
            model.Email = User.Identity.GetUserName();
            model.Latitude = address.Latitude;
            model.Longitude = address.Longitude;            
           
            return PartialView("ManageUserInfo", model);  
        }

        //
        // POST: /Account/Manage
        /// <summary>
        /// Manages the user information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageUserInfo(ManageUserInfoViewModel model)
        {
            ViewBag.ReturnUrl = Url.Action("ManageUserInfo");

            UpdateCurrentUserInfo(model);

            var alist = new SBAS_DAL.Address().GetListofStates();
            var stateList = new SBAS_DAL.Address().GetListofStates().Select(x => new SelectListItem { Value = x.StateId.ToString(), Text = x.StateAbbreviation }).ToArray();
            var alist2 = new SBAS_DAL.Address().GetCitiesByState(model.SelectedStateID);
            var citylist = alist2.Select(x => new SelectListItem { Value = x.CityId.ToString(), Text = x.City })
                    .ToArray();

            model.StateList = new SelectList(stateList, "Value", "Text", model.SelectedStateID);
            model.CityList = new SelectList(citylist, "Value", "Text", model.SelectedCityID);

            return PartialView("ManageUserInfo", model);
        }

        //
        // POST: /Account/Manage
        /// <summary>
        /// Manages the client info2.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [HttpPost]        
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageClientInfo2(ManageUserInfoViewModel model)
        {

            ViewBag.ReturnUrl = Url.Action("ManageClientInfo");

            UpdateCurrentUserInfo(model);
        
            /*
            
            var alist = new SBAS_DAL.Address().GetListofStates();

            var stateList = new SBAS_DAL.Address().GetListofStates().Select(x => new SelectListItem { Value = x.StateId.ToString(), Text = x.StateAbbreviation }).ToArray();

            //model.StateList = new SelectList(stateList, "Value", "Text", 0);

            var alist2 = new SBAS_DAL.Address().GetCitiesByState(alist[0].StateId);

            var citylist = alist2.Select(x => new SelectListItem { Value = x.CityId.ToString(), Text = x.City })
                    .ToArray();
            model.StateList = new SelectList(stateList, "Value", "Text", model.SelectedStateID);
            model.CityList = new SelectList(citylist, "Value", "Text", model.SelectedCityID);

            // If we got this far, something failed, redisplay form
             
            return PartialView("ManageClientInfo", model);
            */

            //return Redirect("/Customer/ClientList");

            var alist = new SBAS_DAL.Address().GetListofStates();
            var stateList = new SBAS_DAL.Address().GetListofStates().Select(x => new SelectListItem { Value = x.StateId.ToString(), Text = x.StateAbbreviation }).ToArray();
            var alist2 = new SBAS_DAL.Address().GetCitiesByState(model.SelectedStateID);
            var citylist = alist2.Select(x => new SelectListItem { Value = x.CityId.ToString(), Text = x.City })
                    .ToArray();

            model.StateList = new SelectList(stateList, "Value", "Text", model.SelectedStateID);
            model.CityList = new SelectList(citylist, "Value", "Text", model.SelectedCityID);

            return PartialView("ManageClientInfo", model); 
             
        }

        /// <summary>
        /// Updates the current user information.
        /// </summary>
        /// <param name="model">The model.</param>
        private void UpdateCurrentUserInfo(ManageUserInfoViewModel model)
        {
            // Current uesr
            var currentUser = UserManager.FindByName(User.Identity.GetUserName());
            var currentUserInfo = new SBAS_DAL.SBASUser().GetSBASUserByEmail(model.Email);
            SBAS_Core.Model.Address currentAddress = new SBAS_DAL.Address().GetAddressById(currentUserInfo.AddressId);
            var currentDateTime = DateTime.Now;

            //Update address at first            
            currentAddress.AddressLine1 = model.AddressLine1;
            currentAddress.AddressLine2 = model.AddressLine2;
            currentAddress.CityId = model.SelectedCityID;
            currentAddress.StateId = model.SelectedStateID;
            currentAddress.ZipCode = model.ZipCode;
            currentAddress.UpdateDateTime = currentDateTime;
            currentAddress.UpdateUser = User.Identity.GetUserId();
            SBAS_Core.Google.Geocoder.GeocodeAddress(currentAddress);
            currentAddress = new SBAS_DAL.Address().UpdateAddress(currentAddress);

            //Update user info 
            currentUserInfo.CompanyName = model.CompanyName;
            currentUserInfo.FaxNumber = model.FaxNumber;
            currentUserInfo.FirstName = model.FirstName;
            currentUserInfo.LastName = model.LastName;
            currentUserInfo.MobileNumber = model.MobileNumber;
            currentUserInfo.PhoneNumber = model.PhoneNumber;
            currentUserInfo.UpdateDateTime = currentDateTime;
            currentUserInfo.UpdateUser = User.Identity.GetUserId();
            currentUserInfo = new SBAS_DAL.SBASUser().UpdateSBASUserByEmail(User.Identity.GetUserName(), currentUserInfo);
        }

        /// <summary>
        /// Manages the client information.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult ManageClientInfo(long Id)
        {
            var model = new ManageUserInfoViewModel();
            var userInfo = new SBAS_DAL.SBASUser().GetSBASUserById(Id);
            var address = new SBAS_DAL.Address().GetAddressById(userInfo.AddressId);            
            var alist = new SBAS_DAL.Address().GetListofStates();
            var stateList = new SBAS_DAL.Address().GetListofStates().Select(x => new SelectListItem { Value = x.StateId.ToString(), Text = x.StateAbbreviation }).ToArray();
            var alist2 = new SBAS_DAL.Address().GetCitiesByState(address.StateId);
            var citylist = alist2.Select(x => new SelectListItem { Value = x.CityId.ToString(), Text = x.City })
                    .ToArray();
                       
            model.CompanyName = userInfo.CompanyName;
            model.FirstName = userInfo.FirstName;
            model.LastName = userInfo.LastName;
            model.AddressLine1 = address.AddressLine1;
            model.AddressLine2 = address.AddressLine2;
            model.SelectedCityID = address.CityId;
            model.StateList = new SelectList(stateList, "Value", "Text", -1);
            model.SelectedStateID = address.StateId;
            model.CityList = new SelectList(citylist, "Value", "Text", -1);
            model.SelectedCityID = address.CityId;          
            model.ZipCode = address.ZipCode;
            model.PhoneNumber = userInfo.PhoneNumber;
            model.MobileNumber = userInfo.MobileNumber;
            model.FaxNumber = userInfo.FaxNumber;
            model.Email = UserManager.GetEmail(userInfo.Id);

           return View("ManageClientInfo", model);
        }

        /// <summary>
        /// Deletes the client information.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult DeleteClientInfo(long Id)
        {
            var currentUserInfo = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.GetUserName());
            SBAS_Core.Model.ClientList cl = new ClientList() 
            { 
                ClientId = Id, 
                CustomerId = currentUserInfo.UserId
            };

            bool rtn = new SBAS_DAL.ClientList().DeleteClientList(cl);
            return Redirect("/Customer/ClientList");
        }

        //
        // POST: /Account/ExternalLogin
        /// <summary>
        /// Externals the login.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        /// <summary>
        /// Externals the login callback.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/LinkLogin
        /// <summary>
        /// Links the login.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        /// <summary>
        /// Links the login callback.
        /// </summary>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        /// <summary>
        /// Externals the login confirmation.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);

                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // SendEmail(user.Email, callbackUrl, "Confirm your account", "Please confirm your account by clicking this link");

                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        /// <summary>
        /// Logs the off.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        /// <summary>
        /// Externals the login failure.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        /// <summary>
        /// Removes the account list.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        /// <summary>
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        /// <summary>
        /// The XSRF key
        /// </summary>
        private const string XsrfKey = "XsrfId";

        /// <summary>
        /// Gets the authentication manager.
        /// </summary>
        /// <value>The authentication manager.</value>
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        /// <summary>
        /// sign in as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="isPersistent">if set to <c>true</c> [is persistent].</param>
        /// <returns>Task.</returns>
        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        /// <summary>
        /// Adds the errors.
        /// </summary>
        /// <param name="result">The result.</param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        /// <summary>
        /// Determines whether this instance has password.
        /// </summary>
        /// <returns><c>true</c> if this instance has password; otherwise, <c>false</c>.</returns>
        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="callbackUrl">The callback URL.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        private void SendEmail(string email, string callbackUrl, string subject, string message)
        {
            // For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771
        }

        /// <summary>
        /// Enum ManageMessageId
        /// </summary>
        public enum ManageMessageId
        {
            /// <summary>
            /// The change password success
            /// </summary>
            ChangePasswordSuccess,
            /// <summary>
            /// The set password success
            /// </summary>
            SetPasswordSuccess,
            /// <summary>
            /// The remove login success
            /// </summary>
            RemoveLoginSuccess,
            /// <summary>
            /// The error
            /// </summary>
            Error
        }

        /// <summary>
        /// Redirects to local.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>ActionResult.</returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Class ChallengeResult.
        /// </summary>
        private class ChallengeResult : HttpUnauthorizedResult
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ChallengeResult"/> class.
            /// </summary>
            /// <param name="provider">The provider.</param>
            /// <param name="redirectUri">The redirect URI.</param>
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ChallengeResult"/> class.
            /// </summary>
            /// <param name="provider">The provider.</param>
            /// <param name="redirectUri">The redirect URI.</param>
            /// <param name="userId">The user identifier.</param>
            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            /// <summary>
            /// Gets or sets the login provider.
            /// </summary>
            /// <value>The login provider.</value>
            public string LoginProvider { get; set; }
            /// <summary>
            /// Gets or sets the redirect URI.
            /// </summary>
            /// <value>The redirect URI.</value>
            public string RedirectUri { get; set; }
            /// <summary>
            /// Gets or sets the user identifier.
            /// </summary>
            /// <value>The user identifier.</value>
            public string UserId { get; set; }

            /// <summary>
            /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult" /> class.
            /// </summary>
            /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        /// <summary>
        /// Gets the state of the city by.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        public ActionResult GetCityByState(long id)
        {

            var model = new CitListViewModel();

            var list = new SBAS_DAL.Address().GetCitiesByState(id);

            var citylist = list.Select(x => new SelectListItem { Value = x.CityId.ToString(), Text = x.City })
                    .ToArray();
            model.SelectedCityID = 2;
            model.CityList = new SelectList(citylist, "Value", "Text", -1);

            return PartialView("_CityListPartial", model);
        }
    }
}