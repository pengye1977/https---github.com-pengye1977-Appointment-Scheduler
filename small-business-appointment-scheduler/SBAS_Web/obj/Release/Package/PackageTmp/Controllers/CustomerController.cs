// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Sydney Bennington
// Created          : 06-17-2014
//
// Last Modified By : Sydney Bennington
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="CustomerController.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SBAS_DAL;
using SBAS_Web.Models;

/// <summary>
/// The SBAS_Web.Controllers namespace.
/// </summary>
namespace SBAS_Web.Controllers
{
    /// <summary>
    /// CThe CustomerController class, which inherits from the Controller class, and controls the SBAS Customer Index.cshtml page.
    /// </summary>
    public class CustomerController : Controller
    {
        /// <summary>
        /// Controls the actions for the Index.cshtml page for the SBAS Customer portion of the application.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult Index()
        {
            var sDal = new SBAS_DAL.SBASUser();
            var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);
            var sAddress = new SBAS_DAL.Address();
            var custAddress = sAddress.GetAddressById(currentLoggedInUser.AddressId);

            ViewData["customerid"] = currentLoggedInUser.UserId;
            ViewData["custLat"] = custAddress.Latitude;
            ViewData["custLng"] = custAddress.Longitude;
            ViewData["LowInventoryItems"] = GetLowInventoryItems();
            ViewData["OutstandingInvoices"] = GetOutstandingInvoices();
            return View();
        }


        /// <summary>
        /// Clients the list.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult ClientList()
        {
            //SBAS_DAL.SBASUser sDAL = new SBAS_DAL.SBASUser();
            //SBAS_Core.Model.SBASUser recv = sDAL.GetSBASUserByEmail(testEmail);
            var model = new List<ClientListModel>();

            var sDal = new SBAS_DAL.SBASUser();
            var sAddress = new SBAS_DAL.Address();

            // The User.Identity.Name is storing the email of the currently logged in user
            var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

            var clientlist = sDal.GetAllCustomerClients(currentLoggedInUser.UserId);
            //var clientlist = sDal.GetAllSBASUser_Client();
            foreach (var client in clientlist)
            {
                var clientListModel = new ClientListModel();
                clientListModel.FullName = string.Format("{0} {1}", client.FirstName, client.LastName);
                var address = sAddress.GetAddressById(client.AddressId);
                var city = sAddress.GetCityById(address.CityId);
                var state = sAddress.GetStateById(address.StateId);
                clientListModel.Address = string.Format("{0} {1} ,{2} , {3} {4}", address.AddressLine1, address.AddressLine2,city.City, state.StateAbbreviation, address.ZipCode);
                clientListModel.ClientId = client.UserId;
                clientListModel.AddressId = address.AddressId;
                model.Add(clientListModel);
            }
            return View(model);
        }

        /// <summary>
        /// Gets the low inventory items.
        /// </summary>
        /// <returns>List&lt;SBAS_Web.Models.InventoryItemModel&gt;.</returns>
        private List<SBAS_Web.Models.InventoryItemModel> GetLowInventoryItems()
        {           
            var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);
            var inventoryItems = new SBAS_DAL.Inventory().GetUsersLowInventoryItems(currentLoggedInUser.UserId, 3);

            var lowInventoryItems = new List<SBAS_Web.Models.InventoryItemModel>();
            foreach (SBAS_Core.Model.InventoryItem item in inventoryItems)
            {
                lowInventoryItems.Add(new SBAS_Web.Models.InventoryItemModel()
                {
                    ItemName = item.ItemName,
                    ItemDescription = item.ItemDescription,
                    QuantityOnHand = item.QuantityOnHand,
                    ItemPrice = item.ItemPrice,
                });
            }

            return lowInventoryItems;
        }

        /// <summary>
        /// Gets the outstanding invoices for the customer.
        /// </summary>
        /// <returns>List<SBAS_Web.Models.InvoiceModel>.</returns>
        private List<SBAS_Web.Models.InvoiceModel> GetOutstandingInvoices()
        {
            var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

            var invoices = new SBAS_DAL.Invoice().GetAllInvoicesForCustomer(currentLoggedInUser.UserId);

            var outstandingInvoices = new List<SBAS_Web.Models.InvoiceModel>();
            var tenInvoices = new List<SBAS_Core.Model.Invoice>();

            if (invoices.Count >= 10)
            {
                tenInvoices = invoices.GetRange(invoices.Count - 11, 10);
            }
            else
            {
                tenInvoices = invoices.GetRange(0, invoices.Count);
            }

            foreach (SBAS_Core.Model.Invoice invoice in tenInvoices)
            {
                if (invoice.IsPaid == false)
                {
                    outstandingInvoices.Add(new SBAS_Web.Models.InvoiceModel()
                    {
                        InvoiceNumber = invoice.InvoiceNumber,
                        AmountDue = invoice.AmountDue,
                        DueDate = invoice.DueDate
                    });
                }
            }
            
            return outstandingInvoices;
        }
    }
}