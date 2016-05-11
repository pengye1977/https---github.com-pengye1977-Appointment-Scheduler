// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Ye Peng
// Created          : 06-17-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-13-2014
// ***********************************************************************
// <copyright file="ClientController.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>Controls all actions related to the client side of the application.
// </summary>
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
using Owin;
using SBAS_Core;
using SBAS_Web.Models;
using SBAS_Core.Model;

/// <summary>
/// The SBAS_Web.Controllers namespace.
/// </summary>
namespace SBAS_Web.Controllers
{
    /// <summary>
    /// The ClientController class, which inherits from the Controller class, and controls the SBAS Client portion of the application.
    /// </summary>
    public class ClientController : Controller
    {
        /// <summary>
        /// Controls the actions for the Index.cshtml page for the SBAS Client portion of the application.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Client")]
        public ActionResult Index()
        {
            // Gets the client's outstanding invoices for display in the Index.cshtml page.
            ViewData["ClientOutstandingInvoices"] = GetOutstandingInvoices();
            return View();
        }

        /// <summary>
        /// Controls the actions for the ClientAppointments.cshtml page for the SBAS Client portion of the application.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Client")]
        public ActionResult Appointments()
        {
            return View("Appointment/ClientAppointments");
        }

        // All invoicing actions
        /// <summary>
        /// Controls the actions for the Invoicing Index.cshtml page for the SBAS Client portion of the application. Gets all invoices for the client and for display on the Invoicing Index.cshtml page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Client")]
        public ActionResult Invoicing()
        {
            // The User.Identity.Name currently is storing the email of the currently logged in user
            var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

            var retrievedInvoices = new SBAS_DAL.Invoice().GetAllInvoicesForClient(currentLoggedInUser.UserId);

            var invoiceModel = new List<SBAS_Web.Models.InvoiceModel>();

            foreach (SBAS_Core.Model.Invoice invoice in retrievedInvoices)
            {
                invoiceModel.Add(new SBAS_Web.Models.InvoiceModel()
                {
                    InvoiceID = invoice.InvoiceId,
                    InvoiceNumber = invoice.InvoiceNumber,
                    AmountDue = invoice.AmountDue,
                    DueDate = invoice.DueDate,
                    ClientID = invoice.ClientID,
                    Comments = invoice.Comments,
                    IsPaid = invoice.IsPaid,
                });
            }
            return View("Invoicing/Index", invoiceModel);
        }

        /// <summary>
        /// Gets the client's outstanding invoices.
        /// </summary>
        /// <returns>List<SBAS_Web.Models.InvoiceModel>.</returns>
        private List<SBAS_Web.Models.InvoiceModel> GetOutstandingInvoices()
        {
            var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

            var retrievedInvoices = new SBAS_DAL.Invoice().GetAllInvoicesForClient(currentLoggedInUser.UserId);

            var outstandingInvoices = new List<SBAS_Web.Models.InvoiceModel>();

            var invoices = new List<SBAS_Core.Model.Invoice>();

            foreach (SBAS_Core.Model.Invoice invoice in retrievedInvoices)
            {
                if (invoice.IsPaid == false)
                {
                    outstandingInvoices.Add(new SBAS_Web.Models.InvoiceModel()
                    {
                        InvoiceID = invoice.InvoiceId,
                        InvoiceNumber = invoice.InvoiceNumber,
                        AmountDue = invoice.AmountDue,
                        DueDate = invoice.DueDate
                    });
                }
            }

            return outstandingInvoices;
        }

        /// <summary>
        /// Gets the client's unpaid invoices for display in the DisplayInvoicesForClient.cshtml page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Client")]
        public ActionResult ViewUnpaidInvoices()
        {
            try
            {
                // The User.Identity.Name currently is storing the email of the currently logged in user
                var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

                var retrievedInvoices = new SBAS_DAL.Invoice().GetAllInvoicesForClient(currentLoggedInUser.UserId);

                var invoiceModel = new List<SBAS_Web.Models.InvoiceModel>();

                foreach (SBAS_Core.Model.Invoice invoice in retrievedInvoices)
                {
                    if (invoice.IsPaid == false)
                    {
                        var client = new SBAS_DAL.SBASUser().GetSBASUserById(invoice.ClientID);

                        invoiceModel.Add(new SBAS_Web.Models.InvoiceModel()
                        {
                            InvoiceID = invoice.InvoiceId,
                            InvoiceNumber = invoice.InvoiceNumber,
                            AmountDue = invoice.AmountDue,
                            DueDate = invoice.DueDate,
                            ClientID = invoice.ClientID,
                            Comments = invoice.Comments,
                            IsPaid = invoice.IsPaid,
                        });
                    }
                }
                return View("Invoicing/DisplayInvoicesForClient", invoiceModel);
            }
            catch (Exception ex)
            {
                return Redirect("Invoicing");
            }
        }

        /// <summary>
        /// Creates the invoice model and populates the start and end dates for the SearchInvoicesForClient.cshtml page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Client")]
        public ActionResult FindInvoiceForClient()
        {
            var model = new SBAS_Web.Models.InvoiceModel();
            model.StartDate = DateTime.Now;
            model.EndDate = DateTime.Now;

            return View("Invoicing/SearchInvoicesForClient", model);
        }

        /// <summary>
        /// Searches the invoices for the client by either invoice number or date range for display on either the DisplayInvoiceDetails.cshtml or DisplayInvoicesForClient.cshtml page.
        /// </summary>
        /// <param name="invoiceModel">The invoice model.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Client")]
        public ActionResult SearchInvoicesForClient(SBAS_Web.Models.InvoiceModel invoiceModel)
        {
            try
            {
                // The User.Identity.Name currently is storing the email of the currently logged in user
                var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

                var retrievedInvoices = new SBAS_DAL.Invoice().GetAllInvoicesForClient(currentLoggedInUser.UserId);

                var modelDefault = new List<SBAS_Web.Models.InvoiceModel>();

                if (invoiceModel.InvoiceNumber != null)
                {
                    var modelSingleInvoice = new SBAS_Web.Models.InvoiceModel();

                    foreach (SBAS_Core.Model.Invoice invoice in retrievedInvoices)
                    {
                        if (invoiceModel.InvoiceNumber == invoice.InvoiceNumber)
                        {
                            var appointmentCompleted = new SBAS_DAL.Appointment().GetClientAppointmentCompletedByInvoiceID(invoice.InvoiceId);

                            var appointment = new SBAS_DAL.Appointment().GetAppointmentById(appointmentCompleted.AppointmentId);

                            var service = new SBAS_DAL.ServiceType().GetServiceTypeByServiceTypeId(appointment.ServiceTypeId);

                            modelSingleInvoice.InvoiceID = invoice.InvoiceId;
                            modelSingleInvoice.InvoiceNumber = invoice.InvoiceNumber;
                            modelSingleInvoice.NameofService = service.NameOfService;
                            modelSingleInvoice.AmountDue = invoice.AmountDue;
                            modelSingleInvoice.DueDate = invoice.DueDate;
                            modelSingleInvoice.IsPaid = invoice.IsPaid;

                            ViewData["LineItems"] = GetLineItems(modelSingleInvoice.InvoiceID);
                            return View("Invoicing/DisplayInvoiceDetails", modelSingleInvoice);
                        }
                    }

                    return View("Invoicing/DisplayInvoicesForClient", modelDefault);
                }
                else if ((invoiceModel.StartDate != null) && (invoiceModel.EndDate != null))
                {
                    var modelMultipleInvoices = new List<SBAS_Web.Models.InvoiceModel>();

                    foreach (SBAS_Core.Model.Invoice invoice2 in retrievedInvoices)
                    {
                        if ((invoice2.DueDate >= invoiceModel.StartDate) && (invoice2.DueDate <= invoiceModel.EndDate))
                        {
                            modelMultipleInvoices.Add(new SBAS_Web.Models.InvoiceModel()
                            {
                                InvoiceID = invoice2.InvoiceId,
                                InvoiceNumber = invoice2.InvoiceNumber,
                                AmountDue = invoice2.AmountDue,
                                DueDate = invoice2.DueDate,
                                Comments = invoice2.Comments,
                                SentToClient = invoice2.SentToClient,
                                IsPaid = invoice2.IsPaid,
                            });
                        }
                    }

                    return View("Invoicing/DisplayInvoicesForClient", modelMultipleInvoices);
                }
                else
                {
                    return View("Invoicing/DisplayInvoicesForClient", modelDefault);
                }
            }
            catch (Exception ex)
            {
                return Redirect("Invoicing");
            }
        }

        /// <summary>
        /// Gets the invoice data by invoice identifier for display on the DisplayInvoiceDetails.cshtml page.
        /// </summary>
        /// <param name="invoiceID">The invoice identifier.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Client")]
        public ActionResult ViewInvoice(long invoiceID)
        {
            var retrievedInvoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(invoiceID);

            var appointmentCompleted = new SBAS_DAL.Appointment().GetClientAppointmentCompletedByInvoiceID(invoiceID);

            var appointment = new SBAS_DAL.Appointment().GetAppointmentById(appointmentCompleted.AppointmentId);

            var service = new SBAS_DAL.ServiceType().GetServiceTypeByServiceTypeId(appointment.ServiceTypeId);

            var invoiceModel = new SBAS_Web.Models.InvoiceModel()
            {
                InvoiceID = retrievedInvoice.InvoiceId,
                InvoiceNumber = retrievedInvoice.InvoiceNumber,
                NameofService = service.NameOfService,
                AmountDue = retrievedInvoice.AmountDue,
                DueDate = retrievedInvoice.DueDate,
                IsPaid = retrievedInvoice.IsPaid,
            };

            ViewData["LineItems"] = GetLineItems(retrievedInvoice.InvoiceId);
            return View("Invoicing/DisplayInvoiceDetails", invoiceModel);
        }

        /// <summary>
        /// Gets the invoice line items by invoice identifier.
        /// </summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>List<SBAS_Web.Models.InvoiceLineItemModel>.</returns>
        private List<SBAS_Web.Models.InvoiceLineItemModel> GetLineItems(long invoiceId)
        {
            var retrievedInvoiceLineItems = new SBAS_DAL.Invoice().GetInvoiceLineItemsByInvoiceId(invoiceId);

            var invoiceLineItems = new List<SBAS_Web.Models.InvoiceLineItemModel>();

            foreach (SBAS_Core.Model.InvoiceLineItem lineItem in retrievedInvoiceLineItems)
            {
                var inventoryItem = new SBAS_DAL.Inventory().GetUsersInventoryItemByInventoryItemId(lineItem.ItemId);

                invoiceLineItems.Add(new SBAS_Web.Models.InvoiceLineItemModel()
                {
                    InvoiceLineItemID = lineItem.InvoiceLineItemId,
                    ItemId = lineItem.ItemId,
                    ItemName = inventoryItem.ItemName,
                    ItemLineCostOverride = lineItem.ItemLineCostOverride,
                    InvoiceId = lineItem.InvoiceId,
                    CreateUser = lineItem.CreateUser,
                    CreateDateTime = lineItem.CreateDateTime,
                    UpdateUser = lineItem.UpdateUser,
                    UpdateDateTime = lineItem.UpdateDateTime,
                });
            }

            return invoiceLineItems;
        }

        // All payment actions
        /// <summary>
        /// Controls the actions for the Payment Index.cshtml page for the SBAS Client portion of the application. Gets all payments for the client for display on the Payment Index.cshtml page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Client")]
        public ActionResult Payments()
        {
            // The User.Identity.Name currently is storing the email of the currently logged in user
            var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

            var retrievedPayments = new SBAS_DAL.Payment().GetPaymentsByClientId(currentLoggedInUser.UserId);

            var paymentModel = new List<SBAS_Web.Models.PaymentModel>();

            foreach (SBAS_Core.Model.Payment payment in retrievedPayments)
            {
                var retrievedInvoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(payment.InvoiceId);
                
                var paymentMethod = new SBAS_DAL.Payment().GetPaymentMethodByPaymentMethodID(payment.PaymentMethodId);

                paymentModel.Add(new SBAS_Web.Models.PaymentModel()
                {
                    PaymentId = payment.PaymentId,
                    InvoiceNumber = retrievedInvoice.InvoiceNumber,
                    PaymentDate = payment.PaymentDate,
                    PaymentAmount = payment.PaymentAmount,
                    PaymentMethod = paymentMethod.PaymentMethodDescription,
                });
            }

            return View("Payments/Index", paymentModel);
        }

        /// <summary>
        /// Creates the payment model and populates the payment date for the SearchPaymentsForClient.cshtml page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Client")]
        public ActionResult FindPaymentForClient()
        {
            var model = new SBAS_Web.Models.PaymentModel();
            
            model.PaymentDate = DateTime.Now;

            return View("Payments/SearchPaymentsForClient", model);
        }

        /// <summary>
        /// Searches the payments for the client by either invoice number or payment date for display on the DisplayPaymentsForClient.cshtml page.
        /// </summary>
        /// <param name="paymentModel">The payment model.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Client")]
        public ActionResult SearchPaymentsForClient(SBAS_Web.Models.PaymentModel paymentModel)
        {
            try
            {
                // The User.Identity.Name currently is storing the email of the currently logged in user
                var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

                var retrievedPayments = new SBAS_DAL.Payment().GetPaymentsByClientId(currentLoggedInUser.UserId);

                if (paymentModel.InvoiceNumber != null)
                {
                    var model = new List<SBAS_Web.Models.PaymentModel>();

                    foreach (SBAS_Core.Model.Payment payment in retrievedPayments)
                    {
                        var invoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(payment.InvoiceId);

                        var paymentMethod = new SBAS_DAL.Payment().GetPaymentMethodByPaymentMethodID(payment.PaymentMethodId);

                        if (invoice.InvoiceNumber == paymentModel.InvoiceNumber)
                        {
                            model.Add(new SBAS_Web.Models.PaymentModel()
                            {
                                InvoiceNumber = invoice.InvoiceNumber,
                                PaymentDate = payment.PaymentDate,
                                PaymentAmount = payment.PaymentAmount,
                                PaymentMethod = paymentMethod.PaymentMethodDescription,
                            });
                        }
                    }

                    return View("Payments/DisplayPaymentsForClient", model);
                }
                else if (paymentModel.PaymentDate != null)
                {
                    var model2 = new List<SBAS_Web.Models.PaymentModel>();

                    foreach (SBAS_Core.Model.Payment payment2 in retrievedPayments)
                    {
                        DateTime date1 = payment2.PaymentDate;
                        DateTime date2 = paymentModel.PaymentDate;

                        DateTime date1DateOnly = date1.Date;
                        DateTime date2DateOnly = date2.Date;

                        if (date1DateOnly.Equals(date2DateOnly))
                        {
                            var invoice2 = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(payment2.InvoiceId);

                            var paymentMethod2 = new SBAS_DAL.Payment().GetPaymentMethodByPaymentMethodID(payment2.PaymentMethodId);

                            model2.Add(new SBAS_Web.Models.PaymentModel()
                            {
                                InvoiceNumber = invoice2.InvoiceNumber,
                                PaymentDate = payment2.PaymentDate,
                                PaymentAmount = payment2.PaymentAmount,
                                PaymentMethod = paymentMethod2.PaymentMethodDescription,

                            });
                        }
                    }

                    return View("Payments/DisplayPaymentsForClient", model2);
                }
                else
                {
                    return Redirect("Payments");
                }
            }
            catch (Exception ex)
            {
                return Redirect("Payments");
            }
        }

        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>JsonResult.</returns>
        [Authorize(Roles = "Client, Customer")]
        public JsonResult GetUserInfo(long userId)
        {
            var sbasDal = new SBAS_DAL.SBASUser();
            var userInfo = sbasDal.GetSBASUserById(userId);
            var addressDal = new SBAS_DAL.Address();
            
            var address = addressDal.GetAddressById(userInfo.AddressId);
            if (address.Latitude == null || address.Longitude == null)
            {
                SBAS_Core.Google.Geocoder.GeocodeAddress(address);
                addressDal.UpdateAddress(address);
            }
            var userInfoModel = new MapInfoModel();
            {
                userInfoModel.FullName =  string.Format("{0} {1}", userInfo.FirstName, userInfo.LastName);
                //userInfoModel.Latitude = userInfo.LastN
                userInfoModel.MobileNumber = userInfo.MobileNumber;
                userInfoModel.PhoneNumber = userInfo.PhoneNumber;
                userInfoModel.FaxNumber = userInfo.FaxNumber;
                userInfoModel.Latitude = address.Latitude;
                userInfoModel.Longitude = address.Longitude;
                userInfoModel.AddressLine1 = string.Format("{0} {1}", address.AddressLine1, address.AddressLine2);
                var lutcity = addressDal.GetCityById(address.CityId);
                var lutstate = addressDal.GetStateById(address.StateId);
                userInfoModel.AddressLine2 = string.Format("{0}, {1} {2}", lutcity.City, lutstate.StateAbbreviation, address.ZipCode);

            }
            return Json(userInfoModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Displays the personal information page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Client")]
        public ActionResult DisplayPersonalInfoPage()
        {
            var model = new ManageUserInfoViewModel();
            var userInfo = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);
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
            model.StateList = new SelectList(stateList, "Value", "Text", -1);
            model.SelectedStateID = address.StateId;
            model.CityList = new SelectList(citylist, "Value", "Text", -1);
            model.SelectedCityID = address.CityId;
            model.ZipCode = address.ZipCode;
            model.PhoneNumber = userInfo.PhoneNumber;
            model.MobileNumber = userInfo.MobileNumber;
            model.FaxNumber = userInfo.FaxNumber;
            model.Email = User.Identity.Name;
            model.Latitude = address.Latitude;
            model.Longitude = address.Longitude;

            return View("Admin/UpdatePersonalInfo", model);
        }

        /// <summary>
        /// Updates the personal information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePersonalInfo(ManageUserInfoViewModel model)
        {
            UpdateCurrentUserInfo(model);

            var userInfo = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);
            var address = new SBAS_DAL.Address().GetAddressById(userInfo.AddressId);
            var alist = new SBAS_DAL.Address().GetListofStates();
            var stateList = new SBAS_DAL.Address().GetListofStates().Select(x => new SelectListItem { Value = x.StateId.ToString(), Text = x.StateAbbreviation }).ToArray();
            var alist2 = new SBAS_DAL.Address().GetCitiesByState(address.StateId);
            var citylist = alist2.Select(x => new SelectListItem { Value = x.CityId.ToString(), Text = x.City })
                    .ToArray();
           
            model.StateList = new SelectList(stateList, "Value", "Text", model.SelectedStateID);
            model.CityList = new SelectList(citylist, "Value", "Text", model.SelectedCityID);
             
            return PartialView("Admin/UpdatePersonalInfo", model);
        }

        /// <summary>
        /// Updates the current user information.
        /// </summary>
        /// <param name="model">The model.</param>
        private void UpdateCurrentUserInfo(ManageUserInfoViewModel model)
        {            
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
    }    
}