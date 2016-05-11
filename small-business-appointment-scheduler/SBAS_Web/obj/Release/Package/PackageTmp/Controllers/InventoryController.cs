// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Adam Bak
// Created          : 06-02-2014
//
// Last Modified By : Adam Bak
// Last Modified On : 08-10-2014
// ***********************************************************************
// <copyright file="InventoryController.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>This class is the controller class that is responsible for handling all
// all the requests for Inventory Items and Business Services (Service Types)</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// The Controllers namespace.
/// </summary>
namespace SBAS_Web.Controllers
{
    /// <summary>
    /// Class InventoryController.
    /// </summary>
    public class InventoryController : Controller
    {
        /// <summary>
        /// This is the default entry point into the Inventory Maintenance Page.
        /// </summary>
        /// <param name="tabToSelect">The tab to select.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult Index(string tabToSelect)
        {
            // The User.Identity.Name currently is storing the email of the currently logged in user
            var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);
            var retrievedInventoryItems = new SBAS_DAL.Inventory().GetUsersInventoryItems(currentLoggedInUser.UserId);
            var model = new List<SBAS_Web.Models.InventoryItemModel>();

            foreach (SBAS_Core.Model.InventoryItem item in retrievedInventoryItems)
            {
                model.Add(new SBAS_Web.Models.InventoryItemModel()
                {
                    InventoryId = item.InventoryId,
                    InventoryItemId = item.InventoryItemId,
                    ItemName = item.ItemName,
                    ItemDescription = item.ItemDescription,
                    ItemPrice = item.ItemPrice,
                    QuantityOnHand = item.QuantityOnHand,
                    HasPhysicalInventory = item.HasPhysicalInventory,
                    ServiceTypeId = item.ServiceTypeId,
                    CreateDateTime = item.CreateDateTime,
                    CreateUser = item.CreateUser,
                    UpdateDateTime = item.UpdateDateTime,
                    UpdateUser = item.UpdateUser,
                });
            }

            if (tabToSelect == "InventoryItem")
            {
                ViewData["TabToSelect"] = "InventoryItem";
            }
            else if (tabToSelect == "BusinessService")
            {
                ViewData["TabToSelect"] = "BusinessService";
            }

            return View(model);
        }      


        /// <summary>
        /// This method displays the page used to create business services.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult DisplayBusinessServiceCreatePage()
        {
            var model = new SBAS_Web.Models.BusinessServiceModel();
            return View("AddBussinessService", model);
        }

        /// <summary>
        /// This method captures the POST data from the user and creates a business service
        /// </summary>
        /// <param name="businessServiceModel">The business service model.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public ActionResult CreateBusinessService(SBAS_Web.Models.BusinessServiceModel businessServiceModel)
        {
            // The User.Identity.Name is storing the email of the currently logged in user
            var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

            var serviceTypeToCreate = new SBAS_Core.Model.ServiceType()
            {
                CustomerId = currentLoggedInUser.UserId,
                NameOfService = businessServiceModel.ItemName,
                Description = businessServiceModel.ItemDescription,
                UpdateUser = "Update User",
                CreateUser = "Create User",
                //ServiceTypeId, UpdateDateTime, CreateDateTime, - Not needed, will be populated in create method
            };

            var createdServiceType = new SBAS_DAL.ServiceType().CreateServiceType(serviceTypeToCreate);
            var inventoryItem = new SBAS_Core.Model.InventoryItem()
            {
                ItemName = businessServiceModel.ItemName,
                ItemDescription = businessServiceModel.ItemDescription,
                ItemPrice = businessServiceModel.ItemPrice,
                QuantityOnHand = null, // A business service has no quantity
                HasPhysicalInventory = false, // A business service cannot actually be touched, it is not PHYSICAL
                ServiceTypeId = createdServiceType.ServiceTypeId,
                CreateUser = "Test Create User",
                UpdateUser = "Test Update User",
                // UpdateDateTime CreateDateTime InventoryItemId InventoryId are all set inside the CreateInventoryItemForUser() method
            };
              
            new SBAS_DAL.Inventory().CreateInventoryItemForUser(currentLoggedInUser.UserId, inventoryItem);
           
            return RedirectToAction("Index", new { tabToSelect = "BusinessService" });
          
        }

        /// <summary>
        /// This method captures the POST data from the user and creates an inventory item
        /// </summary>
        /// <param name="inventoryItemModel">The inventory item model.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public ActionResult CreateInventoryItem(SBAS_Web.Models.InventoryItemModel inventoryItemModel)
        {
            var inventoryItem = new SBAS_Core.Model.InventoryItem()
            {
                ItemName = inventoryItemModel.ItemName,
                ItemDescription = inventoryItemModel.ItemDescription,
                ItemPrice = inventoryItemModel.ItemPrice,
                QuantityOnHand = inventoryItemModel.QuantityOnHand,
                HasPhysicalInventory = true, // A physical inventory item is PHYSICAL, it can be touched
                ServiceTypeId = null, // A physical inventory item does not have an associated service type
                CreateUser = "Test Create User",
                UpdateUser = "Test Update User",
                // UpdateDateTime CreateDateTime InventoryItemId InventoryId are all set inside the CreateInventoryItemForUser() method
            };

            // The User.Identity.Name currently is storing the email of the currently logged in user
            var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);
            new SBAS_DAL.Inventory().CreateInventoryItemForUser(currentLoggedInUser.UserId, inventoryItem);
            return Redirect("/Inventory");
        }


        /// <summary>
        /// This method displays the page used to create inventory items.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult DisplayInventoryItemCreatePage()
        {
            var model = new SBAS_Web.Models.InventoryItemModel();
            return View("AddInventoryItem", model);        
        }

        /// <summary>
        /// This method deletes an inventory item based on the inventory item ID
        /// </summary>
        /// <param name="inventoryItemID">The inventory item identifier.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult DeleteInventoryItem(int inventoryItemID)
        {
            new SBAS_DAL.Inventory().DeteteUsersInventoryItemByInventoryItemId(inventoryItemID);
            return Redirect("/Inventory");
        }

        /// <summary>
        /// This method deletes a business service based on the service's inventory item ID
        /// A business service is a type of inventoryItem without any physical inventory
        /// </summary>
        /// <param name="inventoryItemID">The inventory item identifier.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult DeleteBusinessService(int inventoryItemID)
        {         
            // First delete the inventory item and then delete the associated service type, 
            // Delete in this order due to referential integrity.

            var serviceTypeIdToDelete = new SBAS_DAL.Inventory().GetUsersInventoryItemByInventoryItemId(inventoryItemID).ServiceTypeId;

            if (new SBAS_DAL.Inventory().IsInventoryItemAssoicatedWithAppointment(inventoryItemID) || new SBAS_DAL.ServiceType().IsBusinessServiceAssoicatedWithAppointment((long)serviceTypeIdToDelete))
            {
                // Do not delete any business services if they are part of an appointment, this will cause referential integrity errors on the database side
                return RedirectToAction("Index", new { tabToSelect = "BusinessService" });
            }               
                        
            new SBAS_DAL.Inventory().DeteteUsersInventoryItemByInventoryItemId(inventoryItemID);            
            new SBAS_DAL.ServiceType().DeleteServiceTypeByServiceTypeId((long)serviceTypeIdToDelete);

            return RedirectToAction("Index", new { tabToSelect = "BusinessService" });
            
        }

        /// <summary>
        /// This method displays the page used to edit business services
        /// </summary>
        /// <param name="inventoryItemID">The inventory item identifier.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult  DisplayBusinessServiceEditPage(int inventoryItemID)
        {
            var retrievedInventoryItem = new SBAS_DAL.Inventory().GetUsersInventoryItemByInventoryItemId(inventoryItemID);

            var model = new SBAS_Web.Models.BusinessServiceModel()
            {
                InventoryId = retrievedInventoryItem.InventoryId,
                InventoryItemId = retrievedInventoryItem.InventoryItemId,
                ItemName = retrievedInventoryItem.ItemName,
                ItemDescription = retrievedInventoryItem.ItemDescription,
                ItemPrice = retrievedInventoryItem.ItemPrice,
                QuantityOnHand = retrievedInventoryItem.QuantityOnHand,
                HasPhysicalInventory = retrievedInventoryItem.HasPhysicalInventory,
                ServiceTypeId = retrievedInventoryItem.ServiceTypeId,
                CreateDateTime = retrievedInventoryItem.CreateDateTime,
                CreateUser = retrievedInventoryItem.CreateUser,
                UpdateDateTime = retrievedInventoryItem.UpdateDateTime,
                UpdateUser = retrievedInventoryItem.UpdateUser,
            };

            return View("EditBusinessService", model);
        }

        /// <summary>
        /// This method displays the page used to edit inventory items
        /// </summary>
        /// <param name="inventoryItemID">The inventory item identifier.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult DisplayInventoryItemEditPage(int inventoryItemID)
        {
            var retrievedInventoryItem = new SBAS_DAL.Inventory().GetUsersInventoryItemByInventoryItemId(inventoryItemID);

            var model = new SBAS_Web.Models.InventoryItemModel()
            {
                InventoryId = retrievedInventoryItem.InventoryId,
                InventoryItemId = retrievedInventoryItem.InventoryItemId,
                ItemName = retrievedInventoryItem.ItemName,
                ItemDescription = retrievedInventoryItem.ItemDescription,
                ItemPrice = retrievedInventoryItem.ItemPrice,
                QuantityOnHand = retrievedInventoryItem.QuantityOnHand,
                HasPhysicalInventory = retrievedInventoryItem.HasPhysicalInventory,
                ServiceTypeId = retrievedInventoryItem.ServiceTypeId,
                CreateDateTime = retrievedInventoryItem.CreateDateTime,
                CreateUser = retrievedInventoryItem.CreateUser,
                UpdateDateTime = retrievedInventoryItem.UpdateDateTime,
                UpdateUser = retrievedInventoryItem.UpdateUser,
            };

            return View("EditInventoryItem", model);
        }


        /// <summary>
        /// This page captures the POST data from the user and uses that data to edit a business service
        /// </summary>
        /// <param name="businessServiceModel">The business service model.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public ActionResult EditBusinessService(SBAS_Web.Models.BusinessServiceModel businessServiceModel)
        {
            var inventoryItem = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = businessServiceModel.InventoryId,
                InventoryItemId = businessServiceModel.InventoryItemId,
                ItemName = businessServiceModel.ItemName,
                ItemDescription = businessServiceModel.ItemDescription,
                ItemPrice = businessServiceModel.ItemPrice,
                QuantityOnHand = businessServiceModel.QuantityOnHand,
                HasPhysicalInventory = businessServiceModel.HasPhysicalInventory,
                ServiceTypeId = businessServiceModel.ServiceTypeId,
                CreateDateTime = businessServiceModel.CreateDateTime,
                CreateUser = businessServiceModel.CreateUser,
                UpdateDateTime = businessServiceModel.UpdateDateTime,
                UpdateUser = businessServiceModel.UpdateUser,
            };

            new SBAS_DAL.Inventory().UpdateUsersInventoryItem(inventoryItem);
            var currentLoggedInUser = new SBAS_DAL.SBASUser().GetSBASUserByEmail(User.Identity.Name);

            var serviceType = new SBAS_Core.Model.ServiceType()
            {
                ServiceTypeId = (long)businessServiceModel.ServiceTypeId,
                CustomerId = currentLoggedInUser.UserId,
                NameOfService = businessServiceModel.ItemName,
                Description = businessServiceModel.ItemDescription,                           
                CreateDateTime = businessServiceModel.CreateDateTime,
                CreateUser = businessServiceModel.CreateUser,
                UpdateDateTime = businessServiceModel.UpdateDateTime,
                UpdateUser = businessServiceModel.UpdateUser,
            };

            new SBAS_DAL.ServiceType().UpdateUsersServiceType(serviceType);

            return RedirectToAction("Index", new { tabToSelect = "BusinessService" });
        }

        /// <summary>
        /// This page captures the POST data from the user and uses that data to edit an inventory item
        /// </summary>
        /// <param name="inventoryItemModel">The inventory item model.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public ActionResult EditInventoryItem(SBAS_Web.Models.InventoryItemModel inventoryItemModel)
        {
            var inventoryItem = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = inventoryItemModel.InventoryId,
                InventoryItemId = inventoryItemModel.InventoryItemId,
                ItemName = inventoryItemModel.ItemName,
                ItemDescription = inventoryItemModel.ItemDescription,
                ItemPrice = inventoryItemModel.ItemPrice,
                QuantityOnHand = inventoryItemModel.QuantityOnHand,
                HasPhysicalInventory = inventoryItemModel.HasPhysicalInventory,
                ServiceTypeId = inventoryItemModel.ServiceTypeId,
                CreateDateTime = inventoryItemModel.CreateDateTime,
                CreateUser = inventoryItemModel.CreateUser,
                UpdateDateTime = inventoryItemModel.UpdateDateTime,
                UpdateUser = inventoryItemModel.UpdateUser,
            };

            new SBAS_DAL.Inventory().UpdateUsersInventoryItem(inventoryItem);
            return Redirect("/Inventory");
        }     
    } // End class
} // End namespace