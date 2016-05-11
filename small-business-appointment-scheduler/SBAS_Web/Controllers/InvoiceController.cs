// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Ye Peng
// Created          : 06-02-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="InvoiceController.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>Controls all actions related to invoicing on the customer side 
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
    /// The InvoiceController class, which inherits from the Controller class, and controls all Customer Invoicing activity.
    /// </summary>
    public class InvoiceController : Controller
    {
        // All Customer functionality

        /// <summary>
        /// Controls the actions for the Invoicing Index.cshtml page for the SBAS Customer portion of the application. Gets list of completed appointments marked as ready for invoicing for display on the Invoicing Index.cshtml page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult Index()
        {
            // The User.Identity.Name currently is storing the email of the currently logged in user
            var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

            var retrievedAppointments = new SBAS_DAL.Appointment().GetCompletedAppointmentsByUser(currentLoggedInUser.UserId, true, false);

            var invoiceModel = new List<SBAS_Web.Models.InvoiceModel>();

            foreach(SBAS_Core.Model.AppointmentCompleted appointment in retrievedAppointments)
            {
                var client = new SBAS_DAL.SBASUser().GetSBASUserById(appointment.ClientId);

                var appointmentInfo = new SBAS_DAL.Appointment().GetAppointmentById(appointment.AppointmentId);

                var service = new SBAS_DAL.ServiceType().GetServiceTypeByServiceTypeId(appointmentInfo.ServiceTypeId);

                invoiceModel.Add(new SBAS_Web.Models.InvoiceModel()
                    {
                        ClientID = appointment.ClientId,
                        ClientFirstName = client.FirstName,
                        ClientLastName = client.LastName,
                        NameofService = service.NameOfService,
                        StartDate = appointmentInfo.Start,
                        EndDate = appointmentInfo.End,
                        AppointmentID = appointmentInfo.AppointmentId,
                    });
            }
            
            return View(invoiceModel);
        }

        /// <summary>
        /// Controls the action for the AddInvoice.cshtml page. Gets all invoice information for display on the AddInvoice.cshtml page.
        /// </summary>
        /// <param name="appointmentID">The appointment identifier.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult AddInvoice(int appointmentID)
        {
            try
            {
                var invoiceModel = new SBAS_Web.Models.InvoiceModel();
                 
                decimal totalInvoiceAmount = 0; 
                
                // The User.Identity.Name currently is storing the email of the currently logged in user
                var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

                // Get appointment info
                var appointment = new SBAS_DAL.Appointment().GetAppointmentById(appointmentID);

                // Get client info
                var client = new SBAS_DAL.SBASUser().GetSBASUserById(appointment.ClientId);

                // Get service info
                var inventoryItems = new SBAS_DAL.Inventory().GetUsersInventoryItems(currentLoggedInUser.UserId);

                foreach (SBAS_Core.Model.InventoryItem inventoryItem in inventoryItems)
                {
                    if (inventoryItem.ServiceTypeId == appointment.ServiceTypeId)
                    {
                        totalInvoiceAmount = totalInvoiceAmount + inventoryItem.ItemPrice;
                    }
                }

                var service = new SBAS_DAL.ServiceType().GetServiceTypeByServiceTypeId(appointment.ServiceTypeId);

                // Get appointment completed info
                var appointmentCompleted = new SBAS_DAL.Appointment().GetAppointmentCompletedByApptID(appointment.AppointmentId);

                // Add line items to invoice                
                var appointmentLineItems = new SBAS_DAL.Appointment().GetAppointmentLineItemsByCompletedAppointmentId(appointmentCompleted.AppointmentCompletedId);

                foreach (SBAS_Core.Model.AppointmentLineItem appointmentLineItem in appointmentLineItems)
                {
                    decimal totalItemAmount = appointmentLineItem.ItemPrice * appointmentLineItem.QuantityUsed;
                    
                    totalInvoiceAmount = totalInvoiceAmount + totalItemAmount;
                }

                SBAS_DAL.Invoice invoice = new SBAS_DAL.Invoice();
                
                var newInvoice = new SBAS_Web.Models.InvoiceModel()
                {
                    InvoiceNumber = invoice.GetNextInvoiceNumber(currentLoggedInUser.UserId),
                    AmountDue = totalInvoiceAmount,
                    DueDate = DateTime.Now,
                    ClientID = appointment.ClientId,
                    ClientFirstName = client.FirstName,
                    ClientLastName = client.LastName,
                    CustomerId = currentLoggedInUser.UserId,
                    Comments = invoiceModel.Comments,
                    SentToClient = invoiceModel.SentToClient,
                    IsPaid = invoiceModel.IsPaid,
                    CreateUser = User.Identity.Name,
                    CreateDateTime = DateTime.Now,
                    UpdateUser = User.Identity.Name,
                    UpdateDateTime = DateTime.Now,
                    AppointmentID = invoiceModel.AppointmentID,
                    NameofService = service.NameOfService,
                };

                ViewData["ApptItems"] = GetApptItems(appointmentCompleted.AppointmentCompletedId);
                return View("AddInvoice", newInvoice);
            }
            catch (Exception ex)
            {
                return Redirect("/Invoice");
            }
        }

        /// <summary>
        /// Gets the appointment line items by appointment completed identifier.
        /// </summary>
        /// <param name="appointmentCompletedID">The appointment completed identifier.</param>
        /// <returns>List<SBAS_Web.Models.InvoiceLineItemModel>.</returns>
        private List<SBAS_Web.Models.InvoiceLineItemModel> GetApptItems(long appointmentCompletedID)
        {
            var appointmentLineItems = new SBAS_DAL.Appointment().GetAppointmentLineItemsByCompletedAppointmentId(appointmentCompletedID);

            var invoiceLineItems = new List<SBAS_Web.Models.InvoiceLineItemModel>();

            foreach (SBAS_Core.Model.AppointmentLineItem lineItem in appointmentLineItems)
            {
                var inventoryItem = new SBAS_DAL.Inventory().GetUsersInventoryItemByInventoryItemId(lineItem.InventoryItemId);

                invoiceLineItems.Add(new SBAS_Web.Models.InvoiceLineItemModel()
                {
                    ItemId = lineItem.InventoryItemId,
                    ItemName = inventoryItem.ItemName,
                    ItemQuantity = lineItem.QuantityUsed,
                    ItemLineCostOverride = lineItem.ItemPrice,
                    ItemTotalCost = lineItem.QuantityUsed * lineItem.ItemPrice,
                });
            }

            return invoiceLineItems;
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

        /// <summary>
        /// Adds the invoice to the database and updates the appointmentCompleted IsInvoiced and InvoiceId fields in the database.
        /// </summary>
        /// <param name="invoiceModel">The invoice model.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public ActionResult CreateInvoice(SBAS_Web.Models.InvoiceModel invoiceModel)
        {
            try
            {
                var newInvoice = new SBAS_Core.Model.Invoice()
                {
                    InvoiceNumber = invoiceModel.InvoiceNumber,
                    AmountDue = invoiceModel.AmountDue,
                    DueDate = invoiceModel.DueDate,
                    ClientID = invoiceModel.ClientID,
                    CustomerId = invoiceModel.CustomerId,
                    Comments = invoiceModel.Comments,
                    SentToClient = invoiceModel.SentToClient,
                    IsPaid = invoiceModel.IsPaid,
                    CreateUser = invoiceModel.CreateUser,
                    CreateDateTime = DateTime.Now,
                    UpdateUser = invoiceModel.UpdateUser,
                    UpdateDateTime = DateTime.Now,
                };

                var addedInvoice = new SBAS_DAL.Invoice().AddInvoice(newInvoice);

                // Need to add invoice line items
                // Get appointment info
                var appointmentCompleted = new SBAS_DAL.Appointment().GetAppointmentCompletedByApptID(invoiceModel.AppointmentID);

                var appointmentLineItems = new SBAS_DAL.Appointment().GetAppointmentLineItemsByCompletedAppointmentId(appointmentCompleted.AppointmentCompletedId);

                var invoiceLineItem = new SBAS_Core.Model.InvoiceLineItem();
                
                foreach (SBAS_Core.Model.AppointmentLineItem appointmentLineItem in appointmentLineItems)
                {
                    invoiceLineItem.InvoiceId = addedInvoice.InvoiceId;
                    invoiceLineItem.ItemId = appointmentLineItem.InventoryItemId;
                    invoiceLineItem.ItemLineCostOverride = appointmentLineItem.ItemPrice * appointmentLineItem.QuantityUsed;
                    invoiceLineItem.CreateUser = invoiceModel.CreateUser;
                    invoiceLineItem.CreateDateTime = invoiceModel.CreateDateTime;
                    invoiceLineItem.UpdateUser = invoiceModel.UpdateUser;
                    invoiceLineItem.UpdateDateTime = invoiceModel.UpdateDateTime;

                    var invoiceLineItemAdded = new SBAS_DAL.Invoice().AddInvoiceLineItem(invoiceLineItem);
                };

                // Need to update IsInvoiced and InvoiceId
                appointmentCompleted.IsInvoiced = true;
                appointmentCompleted.InvoiceId = addedInvoice.InvoiceId;

                var appointmentComplete = new SBAS_DAL.Appointment().Update_AppointmentCompleted(appointmentCompleted);

                return Redirect("/Invoice");
            }
            catch (Exception ex)
            {
                return Redirect("/Invoice");
            }
        }

        /// <summary>
        /// If the invoice is not paid, deletes the invoice from the database by invoice identifier and updates the appointmentCompleted IsInvoiced and InvoiceId fields. If the invoice is paid, retrieves the invoice data for display on the InvoiceDetailsMessage.cshtml page, but does not delete it.
        /// </summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult DeleteInvoice(long invoiceId)
        {
            try
            {
                var retrievedInvoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(invoiceId);

                if (retrievedInvoice.IsPaid == false)
                {
                    var retrievedInvoiceLineItems = new SBAS_DAL.Invoice().GetInvoiceLineItemsByInvoiceId(invoiceId);

                    foreach (var invoiceLineItem in retrievedInvoiceLineItems)
                    {
                        new SBAS_DAL.Invoice().DeleteInvoiceLineItem(invoiceLineItem);
                    }

                    new SBAS_DAL.Invoice().DeleteInvoiceByInvoiceID(invoiceId);

                    // Need to update appointment completed record
                    var retrievedAppointmentCompleted = new SBAS_DAL.Appointment().GetClientAppointmentCompletedByInvoiceID(invoiceId);
                    retrievedAppointmentCompleted.IsInvoiced = false;
                    retrievedAppointmentCompleted.InvoiceId = 0;
                    new SBAS_DAL.Appointment().Update_AppointmentCompleted(retrievedAppointmentCompleted);

                    return Redirect("/Invoice/ViewInvoices");
                }
                else
                {
                    var client = new SBAS_DAL.SBASUser().GetSBASUserById(retrievedInvoice.ClientID);

                    var appointmentCompleted = new SBAS_DAL.Appointment().GetClientAppointmentCompletedByInvoiceID(invoiceId);

                    var appointmentInfo = new SBAS_DAL.Appointment().GetAppointmentById(appointmentCompleted.AppointmentId);

                    var service = new SBAS_DAL.ServiceType().GetServiceTypeByServiceTypeId(appointmentInfo.ServiceTypeId);

                    var invoiceModel = new SBAS_Web.Models.InvoiceModel()
                    {
                        InvoiceID = retrievedInvoice.InvoiceId,
                        InvoiceNumber = retrievedInvoice.InvoiceNumber,
                        AmountDue = retrievedInvoice.AmountDue,
                        DueDate = retrievedInvoice.DueDate,
                        ClientID = retrievedInvoice.ClientID,
                        ClientFirstName = client.FirstName,
                        ClientLastName = client.LastName,
                        NameofService = service.NameOfService,
                        CustomerId = retrievedInvoice.CustomerId,
                        Comments = retrievedInvoice.Comments,
                        SentToClient = retrievedInvoice.SentToClient,
                        IsPaid = retrievedInvoice.IsPaid,
                        CreateUser = retrievedInvoice.CreateUser,
                        CreateDateTime = retrievedInvoice.CreateDateTime,
                        UpdateUser = retrievedInvoice.UpdateUser,
                        UpdateDateTime = retrievedInvoice.UpdateDateTime,
                    };

                    ViewData["LineItems"] = GetLineItems(retrievedInvoice.InvoiceId);
                    return View("InvoiceDetailsMessage", invoiceModel);
                }
            }
            catch (Exception ex)
            {
                return Redirect("/Invoice/ViewInvoices");
            }
        }

        /// <summary>
        /// Controls the action for the EditInvoice.cshtml page. If the invoice is unpaid, gets all invoice information for display on the EditInvoice.cshtml page. If the invoice is paid, gets all invoice information for display on the InvoiceDetailsMessage.cshtml page.
        /// </summary>
        /// <param name="invoiceID">The invoice identifier.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult UpdateInvoice(int invoiceID)
        {
            try
            {
                var retrievedInvoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(invoiceID);

                var client = new SBAS_DAL.SBASUser().GetSBASUserById(retrievedInvoice.ClientID);

                var appointmentCompleted = new SBAS_DAL.Appointment().GetClientAppointmentCompletedByInvoiceID(invoiceID);

                var appointmentInfo = new SBAS_DAL.Appointment().GetAppointmentById(appointmentCompleted.AppointmentId);

                var service = new SBAS_DAL.ServiceType().GetServiceTypeByServiceTypeId(appointmentInfo.ServiceTypeId);

                var invoiceModel = new SBAS_Web.Models.InvoiceModel()
                {
                    InvoiceID = retrievedInvoice.InvoiceId,
                    InvoiceNumber = retrievedInvoice.InvoiceNumber,
                    AmountDue = retrievedInvoice.AmountDue,
                    DueDate = retrievedInvoice.DueDate,
                    ClientID = retrievedInvoice.ClientID,
                    ClientFirstName = client.FirstName,
                    ClientLastName = client.LastName,
                    NameofService = service.NameOfService,
                    CustomerId = retrievedInvoice.CustomerId,
                    Comments = retrievedInvoice.Comments,
                    SentToClient = retrievedInvoice.SentToClient,
                    IsPaid = retrievedInvoice.IsPaid,
                    CreateUser = retrievedInvoice.CreateUser,
                    CreateDateTime = retrievedInvoice.CreateDateTime,
                    UpdateUser = retrievedInvoice.UpdateUser,
                    UpdateDateTime = retrievedInvoice.UpdateDateTime,
                };

                ViewData["LineItems"] = GetLineItems(retrievedInvoice.InvoiceId);

                if (retrievedInvoice.IsPaid == true)
                {
                    return View("InvoiceDetailsMessage", invoiceModel);
                }
                else
                {
                    return View("EditInvoice", invoiceModel);
                }
            }
            catch (Exception ex)
            {
                return Redirect("/Invoice/ViewInvoices");
            }
        }

        /// <summary>
        /// Updates the invoice in the database.
        /// </summary>
        /// <param name="invoiceModel">The invoice model.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public ActionResult EditInvoice(SBAS_Web.Models.InvoiceModel invoiceModel)
        {
            try
            {
                var invoice = new SBAS_Core.Model.Invoice()
                {
                    InvoiceId = invoiceModel.InvoiceID,
                    InvoiceNumber = invoiceModel.InvoiceNumber,
                    AmountDue = invoiceModel.AmountDue,
                    DueDate = invoiceModel.DueDate,
                    ClientID = invoiceModel.ClientID,
                    CustomerId = invoiceModel.CustomerId,
                    Comments = invoiceModel.Comments,
                    SentToClient = invoiceModel.SentToClient,
                    IsPaid = invoiceModel.IsPaid,
                    CreateUser = invoiceModel.CreateUser,
                    CreateDateTime = invoiceModel.CreateDateTime,
                    UpdateUser = User.Identity.Name,
                    UpdateDateTime = DateTime.Now,
                };

                new SBAS_DAL.Invoice().UpdateInvoice(invoice);

                return Redirect("/Invoice/ViewInvoices");
            }
            catch (Exception ex)
            {
                return Redirect("/Invoice");
            }
        }

        /// <summary>
        /// Gets all invoice information for display on the DisplayInvoiceDetails.cshtml page.
        /// </summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult ViewInvoiceDetails(long invoiceId)
        {
            try
            {
                var invoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(invoiceId);

                var client = new SBAS_DAL.SBASUser().GetSBASUserById(invoice.ClientID);

                var appointmentCompleted = new SBAS_DAL.Appointment().GetClientAppointmentCompletedByInvoiceID(invoiceId);

                var appointmentInfo = new SBAS_DAL.Appointment().GetAppointmentById(appointmentCompleted.AppointmentId);

                var service = new SBAS_DAL.ServiceType().GetServiceTypeByServiceTypeId(appointmentInfo.ServiceTypeId);
                
                var invoiceModel = new SBAS_Web.Models.InvoiceModel()
                {
                    InvoiceID = invoice.InvoiceId,
                    InvoiceNumber = invoice.InvoiceNumber,
                    AmountDue = invoice.AmountDue,
                    DueDate = invoice.DueDate,
                    ClientID = invoice.ClientID,
                    ClientFirstName = client.FirstName, 
                    ClientLastName = client.LastName,
                    NameofService = service.NameOfService,
                    CustomerId = invoice.CustomerId,
                    Comments = invoice.Comments,
                    SentToClient = invoice.SentToClient,
                    IsPaid = invoice.IsPaid,
                    CreateUser = invoice.CreateUser,
                    CreateDateTime = invoice.CreateDateTime,
                    UpdateUser = invoice.UpdateUser,
                    UpdateDateTime = invoice.UpdateDateTime,
                };

                ViewData["LineItems"] = GetLineItems(invoice.InvoiceId);
                return View("DisplayInvoiceDetails", invoiceModel);
            }
            catch (Exception ex)
            {
                return Redirect("/Invoice");
            }
        }

        /// <summary>
        /// Controls the action for the SearchInvoices.cshtml page. Creates the invoice model for this page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult FindInvoice()
        {
            var model = new SBAS_Web.Models.InvoiceModel();

            return View("SearchInvoices", model);
        }

        /// <summary>
        /// Searches the invoices either by invoice number or client last name and retrieves the information for display on the DisplaySelectedInvoices.cshtml page.
        /// </summary>
        /// <param name="invoiceModel">The invoice model.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult SearchInvoices(SBAS_Web.Models.InvoiceModel invoiceModel)
        {
            try
            {
                // The User.Identity.Name currently is storing the email of the currently logged in user
                var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

                var modelDefault = new List<SBAS_Web.Models.InvoiceModel>();

                if (invoiceModel.InvoiceNumber != null)
                {
                    var retrievedInvoices = new SBAS_DAL.Invoice().GetAllInvoicesForCustomer(currentLoggedInUser.UserId);

                    foreach (SBAS_Core.Model.Invoice invoice in retrievedInvoices)
                    {
                        if (invoiceModel.InvoiceNumber == invoice.InvoiceNumber)
                        {
                            var retrievedInvoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(invoice.InvoiceId);

                            var client = new SBAS_DAL.SBASUser().GetSBASUserById(retrievedInvoice.ClientID);

                            var modelSingleInvoice = new List<SBAS_Web.Models.InvoiceModel>();

                            modelSingleInvoice.Add(new SBAS_Web.Models.InvoiceModel()
                            {
                                InvoiceID = retrievedInvoice.InvoiceId,
                                InvoiceNumber = retrievedInvoice.InvoiceNumber,
                                AmountDue = retrievedInvoice.AmountDue,
                                DueDate = retrievedInvoice.DueDate,
                                ClientID = retrievedInvoice.ClientID,
                                ClientFirstName = client.FirstName,
                                ClientLastName = client.LastName,
                                Comments = retrievedInvoice.Comments,
                                SentToClient = retrievedInvoice.SentToClient,
                                IsPaid = retrievedInvoice.IsPaid,
                            });

                            return View("DisplaySelectedInvoices", modelSingleInvoice);
                        }
                    }

                    return View("DisplaySelectedInvoices", modelDefault);
                }
                else if (invoiceModel.ClientLastName != null)
                {
                    var clientList = new SBAS_DAL.SBASUser().GetAllCustomerClients(currentLoggedInUser.UserId);

                    var modelMultipleInvoices = new List<SBAS_Web.Models.InvoiceModel>();

                    foreach (var client in clientList)
                    {
                        if (client.LastName == invoiceModel.ClientLastName)
                        {
                            var clientInvoices = new SBAS_DAL.Invoice().GetAllInvoicesForClient(client.UserId);

                            foreach (SBAS_Core.Model.Invoice clientInvoice in clientInvoices)
                            {
                                modelMultipleInvoices.Add(new SBAS_Web.Models.InvoiceModel()
                                {
                                    InvoiceID = clientInvoice.InvoiceId,
                                    InvoiceNumber = clientInvoice.InvoiceNumber,
                                    AmountDue = clientInvoice.AmountDue,
                                    DueDate = clientInvoice.DueDate,
                                    ClientID = clientInvoice.ClientID,
                                    ClientFirstName = client.FirstName,
                                    ClientLastName = client.LastName,
                                    Comments = clientInvoice.Comments,
                                    SentToClient = clientInvoice.SentToClient,
                                    IsPaid = clientInvoice.IsPaid,
                                });
                            }
                        }
                    }

                    return View("DisplaySelectedInvoices", modelMultipleInvoices);
                }
                else
                {
                    return View("DisplaySelectedInvoices", modelDefault);
                }
            }
            catch (Exception ex)
            {
                return Redirect("/Invoice/FindInvoice");
            }
        }

        /// <summary>
        /// Controls the action for the DisplayInvoices.cshtml page. Retrieves all of the invoices for the customer for display on the DisplayInvoices.cshtml page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult ViewInvoices()
        {
            try
            {
                // The User.Identity.Name currently is storing the email of the currently logged in user
                var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

                var retrievedInvoices = new SBAS_DAL.Invoice().GetAllInvoicesForCustomer(currentLoggedInUser.UserId);

                var invoiceModel = new List<SBAS_Web.Models.InvoiceModel>();

                foreach (SBAS_Core.Model.Invoice invoice in retrievedInvoices)
                {
                    var client = new SBAS_DAL.SBASUser().GetSBASUserById(invoice.ClientID);

                    invoiceModel.Add(new SBAS_Web.Models.InvoiceModel()
                    {
                        InvoiceID = invoice.InvoiceId,
                        InvoiceNumber = invoice.InvoiceNumber,
                        AmountDue = invoice.AmountDue,
                        DueDate = invoice.DueDate,
                        ClientID = invoice.ClientID,
                        ClientFirstName = client.FirstName,
                        ClientLastName = client.LastName,
                        Comments = invoice.Comments,
                        SentToClient = invoice.SentToClient,
                        IsPaid = invoice.IsPaid,
                    });
                }

                return View("DisplayInvoices", invoiceModel);
            }
            catch (Exception ex)
            {
                return Redirect("/Invoice");
            }
        }

        /// <summary>
        /// Retrieves all of the unpaid invoices for the customer for display on the DisplaySelectedInvoices.cshtml page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult ViewUnpaidInvoices()
        {
            try
            {
                // The User.Identity.Name currently is storing the email of the currently logged in user
                var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

                var retrievedInvoices = new SBAS_DAL.Invoice().GetAllInvoicesForCustomer(currentLoggedInUser.UserId);

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
                            ClientFirstName = client.FirstName,
                            ClientLastName = client.LastName,
                            Comments = invoice.Comments,
                            SentToClient = invoice.SentToClient,
                            IsPaid = invoice.IsPaid,
                        });
                    }
                }
                    return View("DisplaySelectedInvoices", invoiceModel);
            }
            catch (Exception ex)
            {
                return Redirect("/Invoice");
            }
        }

        /// <summary>
        /// Controls the action of the EditInvoiceLineItem.cshtml page. Retrieves the invoice line item data for display on the EditInvoiceLineItem.cshtml page.
        /// </summary>
        /// <param name="invoiceLineItemID">The invoice line item identifier.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult UpdateInvoiceLineItem(int invoiceLineItemID)
        {
            try
            {
                var retrievedInvoiceLineItem = new SBAS_DAL.Invoice().GetInvoiceLineItemByInvoiceLineItemId(invoiceLineItemID);

                var inventoryItem = new SBAS_DAL.Inventory().GetUsersInventoryItemByInventoryItemId(retrievedInvoiceLineItem.ItemId);

                var invoiceLineItemModel = new SBAS_Web.Models.InvoiceLineItemModel()
                {
                    InvoiceId = retrievedInvoiceLineItem.InvoiceId,
                    InvoiceLineItemID = retrievedInvoiceLineItem.InvoiceLineItemId,
                    ItemId = retrievedInvoiceLineItem.ItemId,
                    ItemName = inventoryItem.ItemName,
                    ItemLineCostOverride = retrievedInvoiceLineItem.ItemLineCostOverride,
                    CreateUser = retrievedInvoiceLineItem.CreateUser,
                    CreateDateTime = retrievedInvoiceLineItem.CreateDateTime,
                    UpdateUser = retrievedInvoiceLineItem.UpdateUser,
                    UpdateDateTime = retrievedInvoiceLineItem.UpdateDateTime,
                };

                return View("EditInvoiceLineItem", invoiceLineItemModel);
            }
            catch (Exception ex)
            {
                return Redirect("/Invoice/ViewInvoices");
            }
        }

        /// <summary>
        /// Updates the invoice line item in the database, as well as the amount on the invoice with which it is associated.
        /// </summary>
        /// <param name="invoiceLineItemModel">The invoice line item model.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public ActionResult EditInvoiceLineItem(SBAS_Web.Models.InvoiceLineItemModel invoiceLineItemModel)
        {
            try
            {
                var retrievedInvoiceLineItem = new SBAS_DAL.Invoice().GetInvoiceLineItemByInvoiceLineItemId(invoiceLineItemModel.InvoiceLineItemID);

                var invoiceLineItem = new SBAS_Core.Model.InvoiceLineItem()
                {
                    InvoiceId = invoiceLineItemModel.InvoiceId,
                    InvoiceLineItemId = invoiceLineItemModel.InvoiceLineItemID,
                    ItemId = invoiceLineItemModel.ItemId,
                    ItemLineCostOverride = invoiceLineItemModel.ItemLineCostOverride,
                    CreateUser = invoiceLineItemModel.CreateUser,
                    CreateDateTime = invoiceLineItemModel.CreateDateTime,
                    UpdateUser = User.Identity.Name,
                    UpdateDateTime = DateTime.Now,
                };

                if (invoiceLineItem.ItemLineCostOverride != retrievedInvoiceLineItem.ItemLineCostOverride)
                {
                    decimal difference = retrievedInvoiceLineItem.ItemLineCostOverride - invoiceLineItem.ItemLineCostOverride;

                    new SBAS_DAL.Invoice().UpdateInvoiceLineItem(invoiceLineItem);

                    // Need to update amount on invoice
                    var retrievedInvoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(invoiceLineItemModel.InvoiceId);
                    decimal updatedInvoiceAmount = retrievedInvoice.AmountDue - difference;

                    var invoice = new SBAS_Core.Model.Invoice()
                    {
                        InvoiceId = retrievedInvoice.InvoiceId,
                        InvoiceNumber = retrievedInvoice.InvoiceNumber,
                        AmountDue = updatedInvoiceAmount,
                        DueDate = retrievedInvoice.DueDate,
                        ClientID = retrievedInvoice.ClientID,
                        CustomerId = retrievedInvoice.CustomerId,
                        Comments = retrievedInvoice.Comments,
                        SentToClient = retrievedInvoice.SentToClient,
                        IsPaid = retrievedInvoice.IsPaid,
                        CreateUser = retrievedInvoice.CreateUser,
                        CreateDateTime = retrievedInvoice.CreateDateTime,
                        UpdateUser = User.Identity.Name,
                        UpdateDateTime = DateTime.Now,
                    };

                    new SBAS_DAL.Invoice().UpdateInvoice(invoice);
                }
                
                return RedirectToAction("UpdateInvoice", new { invoiceID = invoiceLineItemModel.InvoiceId });
            }
            catch (Exception ex)
            {
                return Redirect("/Invoice/ViewInvoices");
            }
        }

        /// <summary>
        /// Deletes the invoice line item from the database by invoice line item identifier and updates the amount on the invoice with which it is associated.
        /// </summary>
        /// <param name="invoiceLineItemId">The invoice line item identifier.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult DeleteInvoiceLineItem(long invoiceLineItemId)
        {
            try
            {
                var retrievedInvoiceLineItem = new SBAS_DAL.Invoice().GetInvoiceLineItemByInvoiceLineItemId(invoiceLineItemId);

                long invoiceId = retrievedInvoiceLineItem.InvoiceId;

                new SBAS_DAL.Invoice().DeleteInvoiceLineItem(retrievedInvoiceLineItem);

                // Need to update amount on invoice
                var retrievedInvoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(invoiceId);
                decimal updatedInvoiceAmount = retrievedInvoice.AmountDue - retrievedInvoiceLineItem.ItemLineCostOverride;

                var invoice = new SBAS_Core.Model.Invoice()
                {
                    InvoiceId = retrievedInvoice.InvoiceId,
                    InvoiceNumber = retrievedInvoice.InvoiceNumber,
                    AmountDue = updatedInvoiceAmount,
                    DueDate = retrievedInvoice.DueDate,
                    ClientID = retrievedInvoice.ClientID,
                    CustomerId = retrievedInvoice.CustomerId,
                    Comments = retrievedInvoice.Comments,
                    SentToClient = retrievedInvoice.SentToClient,
                    IsPaid = retrievedInvoice.IsPaid,
                    CreateUser = retrievedInvoice.CreateUser,
                    CreateDateTime = retrievedInvoice.CreateDateTime,
                    UpdateUser = User.Identity.Name,
                    UpdateDateTime = DateTime.Now,
                };

                new SBAS_DAL.Invoice().UpdateInvoice(invoice);

                return RedirectToAction("UpdateInvoice", new { invoiceID = invoiceId });
            }
            catch (Exception ex)
            {
                return Redirect("/Invoice/ViewInvoices");
            }
        }

    }
}