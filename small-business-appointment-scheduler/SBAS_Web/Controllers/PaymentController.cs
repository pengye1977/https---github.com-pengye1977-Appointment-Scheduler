// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Ye Peng
// Created          : 06-02-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="PaymentController.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>Controls all actions related to payments on the customer side 
//     of the application.</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SBAS_Core;
using SBAS_Web.Models;
using SBAS_Core.Model;


/// <summary>
/// The SBAS_Web.Controllers namespace.
/// </summary>
namespace SBAS_Web.Controllers
{
    /// <summary>
    /// The PaymentController class, which inherits from the Controller class, and controls all Customer Payments activity.
    /// </summary>
    public class PaymentController : Controller
    {
        // All customer functionality

        /// <summary>
        /// Controls the actions for the Payments Index.cshtml page for the SBAS Customer portion of the application. Gets list of outstanding invoices for display on the Payments Index.cshtml page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult Index()
        {
            // The User.Identity.Name currently is storing the email of the currently logged in user
            var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

            var retrievedInvoices = new SBAS_DAL.Invoice().GetAllInvoicesForCustomer(currentLoggedInUser.UserId);

            var paymentModel = new List<SBAS_Web.Models.PaymentModel>();

            foreach (SBAS_Core.Model.Invoice invoice in retrievedInvoices)
            {
                if (invoice.IsPaid == false)
                {
                    var client = new SBAS_DAL.SBASUser().GetSBASUserById(invoice.ClientID);

                    paymentModel.Add(new SBAS_Web.Models.PaymentModel()
                    {
                        InvoiceId = invoice.InvoiceId,
                        InvoiceNumber = invoice.InvoiceNumber,
                        AmountDue = invoice.AmountDue,
                        DueDate = invoice.DueDate,
                        ClientId = invoice.ClientID,
                        ClientFirstName = client.FirstName,
                        ClientLastName = client.LastName,
                    });
                }
            }
            return View(paymentModel);
        }

        /// <summary>
        /// Gets all payments for the customer for display on the DisplayPayments.cshtml page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult ViewPayments()
        {
            try
            {
                // The User.Identity.Name currently is storing the email of the currently logged in user
                var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

                var retrievedPayments = new SBAS_DAL.Payment().GetPaymentsByCustomerId(currentLoggedInUser.UserId);

                var paymentModel = new List<SBAS_Web.Models.PaymentModel>();

                foreach (SBAS_Core.Model.Payment payment in retrievedPayments)
                {
                    var client = new SBAS_DAL.SBASUser().GetSBASUserById(payment.ClientId);

                    var invoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(payment.InvoiceId);

                    var paymentMethod = new SBAS_DAL.Payment().GetPaymentMethodByPaymentMethodID(payment.PaymentMethodId);

                    paymentModel.Add(new SBAS_Web.Models.PaymentModel()
                    {
                        InvoiceId = payment.InvoiceId,
                        InvoiceNumber = invoice.InvoiceNumber,
                        PaymentId = payment.PaymentId,
                        PaymentMethodId = payment.PaymentMethodId,
                        PaymentMethod = paymentMethod.PaymentMethodDescription,
                        PaymentAmount = payment.PaymentAmount,
                        PaymentDate = payment.PaymentDate,
                        ClientId = payment.ClientId,
                        ClientFirstName = client.FirstName,
                        ClientLastName = client.LastName,
                    });
                }

                return View("DisplayPayments", paymentModel);
            }
            catch (Exception ex)
            {
                return Redirect("/Payment");
            }
        }

        /// <summary>
        /// Gets the list of payment method types from the database.
        /// </summary>
        /// <returns>List<SelectListItem>.</returns>
        private List<SelectListItem> GetPaymentMethods()
        {
            var retrievedPaymentMethods = new SBAS_DAL.PaymentMethod().GetListofPaymentMethodTypes();

            var paymentMethods = new List<SBAS_Web.Models.PaymentMethodModel>();

            List<SelectListItem> paymentMethodsDropDown = new List<SelectListItem>();

            foreach (SBAS_Core.Model.PaymentMethod paymentMethod in retrievedPaymentMethods)
            {
                paymentMethodsDropDown.AddRange(new[]{
                    new SelectListItem() {Text = paymentMethod.PaymentMethodDescription, Value = paymentMethod.PaymentMethodType}
                });

                paymentMethods.Add(new SBAS_Web.Models.PaymentMethodModel()
                {
                    PaymentMethodId = paymentMethod.PaymentMethodId,
                    PaymentMethodType = paymentMethod.PaymentMethodType,
                    PaymentMethodDescription = paymentMethod.PaymentMethodDescription,
                    CreateUser = paymentMethod.CreateUser,
                    CreateDateTime = paymentMethod.CreateDateTime,
                    UpdateUser = paymentMethod.UpdateUser,
                    UpdateDateTime = paymentMethod.UpdateDateTime,
                });
            }

            return paymentMethodsDropDown;
        }

        /// <summary>
        /// Controls the AddPayment.cshtml page. Gets payment info for display on the AddPayment.cshtml page.
        /// </summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult AddPayment(int invoiceId)
        {
            try
            {
                // The User.Identity.Name currently is storing the email of the currently logged in user
                var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

                var paymentModel = new SBAS_Web.Models.PaymentModel();

                var retrievedInvoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(invoiceId);

                var client = new SBAS_DAL.SBASUser().GetSBASUserById(retrievedInvoice.ClientID);

                var newPayment = new SBAS_Web.Models.PaymentModel()
                {
                    InvoiceId = retrievedInvoice.InvoiceId,
                    InvoiceNumber = retrievedInvoice.InvoiceNumber,
                    ClientId = retrievedInvoice.ClientID,
                    AmountDue = retrievedInvoice.AmountDue,
                    ClientFirstName = client.FirstName,
                    ClientLastName = client.LastName,
                    PaymentAmount = retrievedInvoice.AmountDue,
                    CustomerId = currentLoggedInUser.UserId,
                    CreateUser = User.Identity.Name,
                    CreateDateTime = DateTime.Now,
                    UpdateUser = User.Identity.Name,
                    UpdateDateTime = DateTime.Now,
                    PaymentDate = DateTime.Now,
                };

                ViewData["PaymentMethods"] = GetPaymentMethods();
                return View("AddPayment", newPayment);
            }
            catch (Exception ex)
            {
                return Redirect("/Payment");
            }
        }

        /// <summary>
        /// Adds the payment to the database and updates the IsPaid field on the associated invoice.
        /// </summary>
        /// <param name="paymentModel">The payment model.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public ActionResult CreatePayment(SBAS_Web.Models.PaymentModel paymentModel)
        {
            try
            {
                // Need to add payment method field
                var paymentMethod = new SBAS_DAL.Payment().GetPaymentMethodByPaymentMethodType(paymentModel.PaymentMethodType);
                
                var newPayment = new SBAS_Core.Model.Payment()
                {
                    InvoiceId = paymentModel.InvoiceId,
                    PaymentDate = DateTime.Now,
                    ClientId = paymentModel.ClientId,
                    CustomerId = paymentModel.CustomerId,
                    PaymentAmount = paymentModel.AmountDue,
                    PaymentMethodId = paymentMethod.PaymentMethodId,
                    CreateUser = paymentModel.CreateUser,
                    CreateDateTime = DateTime.Now,
                    UpdateUser = paymentModel.UpdateUser,
                    UpdateDateTime = DateTime.Now,
                };

                var addedPayment = new SBAS_DAL.Payment().AddPayment(newPayment);

                // Need to update invoice to paid
                var invoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(newPayment.InvoiceId);
                invoice.IsPaid = true;
                var updatedInvoice = new SBAS_DAL.Invoice().UpdateInvoice(invoice);

                return Redirect("/Payment");
            }
            catch (Exception ex)
            {
                return Redirect("/Payment");
            }
        }

        /// <summary>
        /// Deletes the payment by payment identifier and updates the IsPaid field on the associated invoice.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult DeletePayment(long paymentId)
        {
            try
            {
                var payment = new SBAS_DAL.Payment().GetPaymentByPaymentId(paymentId);

                new SBAS_DAL.Payment().DeletePaymentByPaymentID(payment.PaymentId);

                // Need to mark invoice as unpaid
                var invoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(payment.InvoiceId);
                invoice.IsPaid = false;
                new SBAS_DAL.Invoice().UpdateInvoice(invoice);

                return Redirect("/Payment");
            }
            catch (Exception ex)
            {
                return Redirect("/Payment");
            }
        }

        /// <summary>
        /// Controls the action of the EditPayment.cshtml page. Retrieves the payment information for display on the EditPayment.cshtml page.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult UpdatePayment(int paymentId)
        {
            // The User.Identity.Name currently is storing the email of the currently logged in user
            var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

            var retrievedPayment = new SBAS_DAL.Payment().GetPaymentByPaymentId(paymentId);

            var paymentModel = new SBAS_Web.Models.PaymentModel();

            var retrievedInvoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(retrievedPayment.InvoiceId);

            var client = new SBAS_DAL.SBASUser().GetSBASUserById(retrievedInvoice.ClientID);

            var retrievedPaymentMethod = new SBAS_DAL.Payment().GetPaymentMethodByPaymentMethodID(retrievedPayment.PaymentMethodId);

            var payment = new SBAS_Web.Models.PaymentModel()
            {
                InvoiceId = retrievedInvoice.InvoiceId,
                InvoiceNumber = retrievedInvoice.InvoiceNumber,
                ClientFirstName = client.FirstName,
                ClientLastName = client.LastName,
                ClientId = retrievedInvoice.ClientID,
                PaymentId = paymentId,
                PaymentAmount = retrievedPayment.PaymentAmount,
                CustomerId = currentLoggedInUser.UserId,
                CreateUser = retrievedPayment.CreateUser,
                CreateDateTime = retrievedPayment.CreateDateTime,
                UpdateUser = retrievedPayment.UpdateUser,
                UpdateDateTime = retrievedPayment.UpdateDateTime,
                PaymentDate = retrievedPayment.PaymentDate,
                PaymentMethodType = retrievedPaymentMethod.PaymentMethodType,
                PaymentMethodId = retrievedPayment.PaymentMethodId,
            };

            ViewData["PaymentMethods"] = GetPaymentMethods();
            return View("EditPayment", payment);
        }

        /// <summary>
        /// Updates the payment in the database.
        /// </summary>
        /// <param name="paymentModel">The payment model.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public ActionResult EditPayment(SBAS_Web.Models.PaymentModel paymentModel)
        {
            try
            {
                // The User.Identity.Name currently is storing the email of the currently logged in user
                var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

                // Need to add payment method field
                var paymentMethod = new SBAS_DAL.Payment().GetPaymentMethodByPaymentMethodType(paymentModel.PaymentMethodType);

                var payment = new SBAS_Core.Model.Payment()
                {
                    PaymentId = paymentModel.PaymentId,
                    CustomerId = paymentModel.CustomerId,
                    ClientId = paymentModel.ClientId,
                    InvoiceId = paymentModel.InvoiceId,                    
                    PaymentAmount = paymentModel.PaymentAmount,
                    PaymentDate = paymentModel.PaymentDate,
                    PaymentMethodId = paymentMethod.PaymentMethodId,
                    CreateUser = paymentModel.CreateUser,
                    CreateDateTime = paymentModel.CreateDateTime,
                    UpdateUser = User.Identity.Name,
                    UpdateDateTime = DateTime.Now,
                };

                new SBAS_DAL.Payment().UpdatePayment(payment);

                return Redirect("/Payment/ViewPayments");
            }
            catch (Exception ex)
            {
                return Redirect("/Payment");
            }
        }

        /// <summary>
        /// Controls the action of the SearchPayments.cshtml page. Creates the payment model.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult FindPayment()
        {
            var model = new SBAS_Web.Models.PaymentModel();

            return View("SearchPayments", model);
        }

        /// <summary>
        /// Searches the customer's payments by invoice number or client last name and retrieves the payment information for display on the DisplayPayments.cshtml page.
        /// </summary>
        /// <param name="paymentModel">The payment model.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        //[HttpGet]
        public ActionResult SearchPayments(SBAS_Web.Models.PaymentModel paymentModel)
        {
            try
            {
                // The User.Identity.Name currently is storing the email of the currently logged in user
                var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

                var modelDefault = new List<SBAS_Web.Models.PaymentModel>();

                if (paymentModel.InvoiceNumber != null)
                {
                    var retrievedInvoices = new SBAS_DAL.Invoice().GetAllInvoicesForCustomer(currentLoggedInUser.UserId);

                    foreach (SBAS_Core.Model.Invoice invoice in retrievedInvoices)
                    {
                        if (paymentModel.InvoiceNumber == invoice.InvoiceNumber)
                        {
                            var retrievedInvoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(invoice.InvoiceId);

                            var retrievedPayments = new SBAS_DAL.Payment().GetPaymentsByInvoiceId(retrievedInvoice.InvoiceId);

                            var modelMultiplePayments = new List<SBAS_Web.Models.PaymentModel>();

                            foreach (SBAS_Core.Model.Payment payment in retrievedPayments)
                            {
                                var client = new SBAS_DAL.SBASUser().GetSBASUserById(retrievedInvoice.ClientID);

                                var paymentMethod = new SBAS_DAL.Payment().GetPaymentMethodByPaymentMethodID(payment.PaymentMethodId);

                                modelMultiplePayments.Add(new SBAS_Web.Models.PaymentModel()
                                {
                                    PaymentId = payment.PaymentId,
                                    CustomerId = payment.CustomerId,
                                    ClientId = payment.ClientId,
                                    ClientFirstName = client.FirstName,
                                    ClientLastName = client.LastName,
                                    InvoiceId = payment.InvoiceId,
                                    InvoiceNumber = retrievedInvoice.InvoiceNumber,
                                    PaymentAmount = payment.PaymentAmount,
                                    PaymentDate = payment.PaymentDate,
                                    PaymentMethodId = payment.PaymentMethodId,
                                    PaymentMethod = paymentMethod.PaymentMethodDescription,
                                });
                            }

                            return View("DisplayPayments", modelMultiplePayments);
                        }
                    }
                    return View("DisplayPayments", modelDefault);
                }
                else if (paymentModel.ClientLastName != null)
                {
                    var clientList = new SBAS_DAL.SBASUser().GetAllCustomerClients(currentLoggedInUser.UserId);

                    var modelMultiplePayments2 = new List<SBAS_Web.Models.PaymentModel>();

                    foreach (var client in clientList)
                    {
                        if (client.LastName == paymentModel.ClientLastName)
                        {
                            var clientPayments = new SBAS_DAL.Payment().GetPaymentsByClientId(client.UserId);

                            foreach (SBAS_Core.Model.Payment clientPayment in clientPayments)
                            {
                                var retrievedInvoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(clientPayment.InvoiceId);

                                var paymentMethod = new SBAS_DAL.Payment().GetPaymentMethodByPaymentMethodID(clientPayment.PaymentMethodId);

                                modelMultiplePayments2.Add(new SBAS_Web.Models.PaymentModel()
                                {
                                    PaymentId = clientPayment.PaymentId,
                                    CustomerId = clientPayment.CustomerId,
                                    ClientId = clientPayment.ClientId,
                                    ClientFirstName = client.FirstName,
                                    ClientLastName = client.LastName,
                                    InvoiceNumber = retrievedInvoice.InvoiceNumber,
                                    InvoiceId = clientPayment.InvoiceId,
                                    PaymentAmount = clientPayment.PaymentAmount,
                                    PaymentDate = clientPayment.PaymentDate,
                                    PaymentMethodId = clientPayment.PaymentMethodId,
                                    PaymentMethod = paymentMethod.PaymentMethodDescription,
                                });
                            }
                        }
                    }

                    var retrievedPayments = new SBAS_DAL.Payment().GetPaymentsByInvoiceId(paymentModel.InvoiceId);

                    var modelMultiplePayments = new List<SBAS_Web.Models.PaymentModel>();

                    return View("DisplayPayments", modelMultiplePayments2);
                }
                else
                {
                    return View("DisplayPayments", modelDefault);
                }
            }
            catch (Exception ex)
            {
                return Redirect("/Payment/FindPayment");
            }
        }
    }
}