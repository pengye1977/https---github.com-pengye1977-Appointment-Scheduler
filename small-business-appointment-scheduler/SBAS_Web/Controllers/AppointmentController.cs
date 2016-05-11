// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Ye Peng
// Created          : 07-18-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-10-2014
// ***********************************************************************
// <copyright file="AppointmentController.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
//  This Controller is used to handle all request dealing with the appointment scheduler ,
//  appoinmented commpledted, and appointment line items. This contorller also calls the DAL for the 
//  algorrithm that does the ditance /time valdiation for appointment createation and updating, and process 
//  throughthe different stages.
//</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Microsoft.AspNet.Identity;
using SBAS_Core;
using SBAS_Core.Google;
using SBAS_Core.Model;
using SBAS_Web.Models;
using Kendo.Mvc.UI;
using SBASUser = SBAS_DAL.SBASUser;

/// <summary>
/// The Controllers namespace.
/// </summary>
namespace SBAS_Web.Controllers
{
    /// <summary>
    /// Class AppointmentController.
    /// </summary>
    public class AppointmentController : Controller
    {
        /// <summary>
        /// The appointment service
        /// </summary>
        private AppointmentService appointmentService;

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Clients the index.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Client")]
        public ActionResult ClientIndex()
        {
            return View("ClientAppointments");
        }

        /// <summary>
        /// Appointments_s the read.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>JsonResult.</returns>
        [Authorize(Roles = "Customer, Client")]  
        public virtual JsonResult Appointments_Read([DataSourceRequest] DataSourceRequest request)
        {
            this.appointmentService = new AppointmentService();
            appointmentService.UserName = User.Identity.GetUserName();

            var dal = new SBAS_DAL.SBASUser();
            var user = dal.GetSBASUserByEmail(appointmentService.UserName);
            appointmentService.RoleName = SBAS_DAL.SBASUser.GetUserRoleName(user.Id);

            return Json(appointmentService.GetAll().ToDataSourceResult(request));

        }

        /// <summary>
        /// Appointments_s the destroy.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="appointment">The appointment.</param>
        /// <returns>JsonResult.</returns>
        [Authorize(Roles = "Customer")]  
        public virtual JsonResult Appointments_Destroy([DataSourceRequest] DataSourceRequest request, AppointmentModel appointment)
        {
            if (ModelState.IsValid)
            {
                appointmentService = new AppointmentService();
                appointmentService.UserName = User.Identity.GetUserName();
                appointmentService.Delete(appointment, ModelState);
            }

            return Json(new[] { appointment }.ToDataSourceResult(request, ModelState));
        }

        /// <summary>
        /// Appointments_s the create.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="appointment">The appointment.</param>
        /// <returns>JsonResult.</returns>
        [Authorize(Roles = "Customer")]  
        public virtual JsonResult Appointments_Create([DataSourceRequest] DataSourceRequest request, AppointmentModel appointment)
        {
            
            if (ModelState.IsValid)
            {
                appointmentService = new AppointmentService();
                appointmentService.UserName = User.Identity.GetUserName();
                appointmentService.Insert(appointment, ModelState);
            }

            return Json(new[] { appointment }.ToDataSourceResult(request, ModelState));
        }

        /// <summary>
        /// Appointments_s the update.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="appointment">The appointment.</param>
        /// <returns>JsonResult.</returns>
        [Authorize(Roles = "Customer")]  
        public virtual JsonResult Appointments_Update([DataSourceRequest] DataSourceRequest request, AppointmentModel appointment)
        {
            if (ModelState.IsValid)
            {
                appointmentService = new AppointmentService();
                appointmentService.UserName = User.Identity.GetUserName();

                appointmentService.Update(appointment, ModelState);
            }

            return Json(new[] { appointment }.ToDataSourceResult(request, ModelState));
        }

        /// <summary>
        /// Customers the clients as json.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult CustomerClientsAsJson()
        {
            var sbasuser = new SBAS_DAL.SBASUser();
            var list = sbasuser.GetAllCustomerClients(sbasuser.GetSBASUserByEmail(User.Identity.GetUserName()).UserId);
            var results = from x in list select new { Value = x.UserId, Text = x.FirstName + " " + x.LastName };

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Customers the service types as json.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult CustomerServiceTypesAsJson()
        {
            var sbasuser = new SBAS_DAL.SBASUser();
            var servicetypeDAL = new SBAS_DAL.ServiceType();

            var list = servicetypeDAL.GetListofUsersServiceTypesByUserId(sbasuser.GetSBASUserByEmail(User.Identity.GetUserName()).UserId);
            var results = from x in list select new { Value = x.ServiceTypeId, Text = x.NameOfService };

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Creates the appointment completed.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult CreateAppointmentCompleted(long id, DateTime startDate, DateTime endDate, long clientId, long customerId)
        {
            var appointmentDal = new SBAS_DAL.Appointment();
            var appointmentCompleted = new SBAS_Core.Model.AppointmentCompleted();
            appointmentCompleted.AppointmentId = id;
            appointmentCompleted.ClientId = clientId;
            appointmentCompleted.CustomerId = customerId;
            appointmentCompleted.CompletionDateTime = startDate;
            appointmentCompleted.IsCompleted = true;
            appointmentCompleted.IsReadyForInvoicing = false;
            appointmentCompleted.IsInvoiced = false;
            appointmentCompleted.CreateDateTime = DateTime.Now;
            appointmentCompleted.UpdateDateTime = DateTime.Now;
            appointmentCompleted.CreateUser = User.Identity.GetUserName();
            appointmentCompleted.UpdateUser = User.Identity.GetUserName();

            appointmentDal.Insert_AppointmentCompleted(appointmentCompleted);

            var result = new AppointmentCompletedResults
            {
                Id = appointmentCompleted.AppointmentCompletedId,
                IsCompleted = appointmentCompleted.IsCompleted,
                IsInvoiced = appointmentCompleted.IsInvoiced,
                IsReadyForInvoicing = appointmentCompleted.IsReadyForInvoicing
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Marks the appointment completed.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="appointmentCompletedId">The appointment completed identifier.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult MarkAppointmentCompleted(long id, long appointmentCompletedId)
        {
            var sbasUserDal = new SBASUser();
            var inventoryDal = new SBAS_DAL.Inventory();
            var appointmentDal = new SBAS_DAL.Appointment();

            var user = sbasUserDal.GetSBASUserByEmail(User.Identity.GetUserName());
            var tempInventoryItems = inventoryDal.GetUsersInventoryItems(user.UserId);
            var inventoryItems = tempInventoryItems.ToList().Select(m => new InventoryItemModel
            {
                InventoryId = m.InventoryId,
                InventoryItemId = m.InventoryItemId,
                ItemName = m.ItemName,
                ItemDescription = m.ItemDescription,
                ItemPrice = m.ItemPrice,
                QuantityOnHand = m.QuantityOnHand,
                HasPhysicalInventory = m.HasPhysicalInventory,
                ServiceTypeId = m.ServiceTypeId,
                CreateUser = m.CreateUser,
                CreateDateTime = m.CreateDateTime,
                UpdateDateTime = m.UpdateDateTime,
                UpdateUser = m.UpdateUser
            }).Where(m => m.ServiceTypeId == null).ToList();

            var appointmentCompleted = appointmentDal.GetAppointmentCompletedByCompleteId(appointmentCompletedId);

            ViewData["AppointmentId"] = id;
            ViewData["appointmentCompletedId"] = appointmentCompletedId;
            ViewData["IsReadyForInvoice"] = appointmentCompleted.IsReadyForInvoicing.ToString();


            return PartialView("AppointmentCompletion", inventoryItems);
        }

        /// <summary>
        /// Inventories the items_ read.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult InventoryItems_Read([DataSourceRequest]DataSourceRequest request)
        {
            var sbasUserDal = new SBASUser();
            var inventoryDal = new SBAS_DAL.Inventory();

            var user = sbasUserDal.GetSBASUserByEmail(User.Identity.GetUserName());
            var tempInventoryItems = inventoryDal.GetUsersInventoryItems(user.UserId);
            var inventoryItems = tempInventoryItems.ToList().Select(m => new InventoryItemModel
            {
                InventoryId = m.InventoryId,
                InventoryItemId = m.InventoryItemId,
                ItemName = m.ItemName,
                ItemDescription = m.ItemDescription,
                ItemPrice = m.ItemPrice,
                QuantityOnHand = m.QuantityOnHand,
                HasPhysicalInventory = m.HasPhysicalInventory,
                ServiceTypeId = m.ServiceTypeId,
                CreateUser = m.CreateUser,
                CreateDateTime = m.CreateDateTime,
                UpdateDateTime = m.UpdateDateTime,
                UpdateUser = m.UpdateUser
            }).Where(m => m.ServiceTypeId == null).ToList();

            return Json(inventoryItems.ToDataSourceResult(request));
        }

        /// <summary>
        /// Gets the inventory items.
        /// </summary>
        /// <returns>JsonResult.</returns>
        [Authorize(Roles = "Customer")]
        public JsonResult GetInventoryItems()
        {

            var sbasUserDal = new SBASUser();
            var inventoryDal = new SBAS_DAL.Inventory();

            var user = sbasUserDal.GetSBASUserByEmail(User.Identity.GetUserName());

            var tempInventoryItems = inventoryDal.GetUsersInventoryItems(user.UserId);
            var inventoryItems = tempInventoryItems.ToList().Select(m => new InventoryItem
            {
                InventoryId = m.InventoryId,
                InventoryItemId = m.InventoryItemId,
                ItemName = m.ItemName,
                ItemDescription = m.ItemDescription,
                ItemPrice = m.ItemPrice,
                QuantityOnHand = m.QuantityOnHand,
                HasPhysicalInventory = m.HasPhysicalInventory,
                ServiceTypeId = m.ServiceTypeId,
                CreateUser = m.CreateUser,
                CreateDateTime = m.CreateDateTime,
                UpdateDateTime = m.UpdateDateTime,
                UpdateUser = m.UpdateUser
            }).Where(m => m.ServiceTypeId == null).ToList();

            return Json(inventoryItems, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the appintment line items.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult GetAppintmentLineItems(long id)
        {
            var model = GetAppointmentLineItemsModel(id);
            return PartialView("AppointmentLineItems", model);
        }

        /// <summary>
        /// Adds the appintment line item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="inventoryId">The inventory identifier.</param>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="itemPrice">The item price.</param>
        /// <param name="quantityUsed">The quantity used.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult AddAppintmentLineItem(long id, long inventoryId, long itemId, decimal itemPrice, long quantityUsed)
        {

            var appointmentDal = new SBAS_DAL.Appointment();
            var inventoryDal = new SBAS_DAL.Inventory();

            var appointmentLineItem = new AppointmentLineItem
            {
                AppointmentCompletedId = id,
                InventoryId = inventoryId,
                InventoryItemId = itemId,
                ItemPrice = itemPrice,
                QuantityUsed = quantityUsed,
                CreateDateTime = DateTime.Now,
                UpdateDateTime = DateTime.Now,
                CreateUser = User.Identity.GetUserName(),
                UpdateUser = User.Identity.GetUserName()
            };

            appointmentDal.Insert_AppointmentLineItem(appointmentLineItem);
            inventoryDal.SubtractInventoryItemQuantity(itemId, (int)quantityUsed);


            var model = GetAppointmentLineItemsModel(id);
            return PartialView("AppointmentLineItems", model);
        }

        /// <summary>
        /// Deletes the appintment line item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [Authorize(Roles = "Customer")]
        public ActionResult DeleteAppintmentLineItem(long id)
        {
            var appointmentDal = new SBAS_DAL.Appointment();
            var inventoryDal = new SBAS_DAL.Inventory();

            var appointmentlineItem = appointmentDal.GetAppointmentLineItem(id);
            inventoryDal.AddInventoryItemQuantity(appointmentlineItem.InventoryItemId, (int)appointmentlineItem.QuantityUsed);
            var appointmentCompletedId = appointmentlineItem.AppointmentCompletedId;
            appointmentDal.Delete_AppointmentLineItem(appointmentlineItem);

            var model = GetAppointmentLineItemsModel(appointmentCompletedId);
            return PartialView("AppointmentLineItems", model);
        }

        /// <summary>
        /// Marks the appointment completed ready for invoicing.
        /// </summary>
        /// <param name="appointmentCompletedId">The appointment completed identifier.</param>
        /// <returns>JsonResult.</returns>
        [Authorize(Roles = "Customer")]
        public JsonResult MarkAppointmentCompletedReadyForInvoicing(long appointmentCompletedId)
        {
            var sbasUserDal = new SBASUser();
            var appointmentDal = new SBAS_DAL.Appointment();

            var appointmentCompleted = appointmentDal.GetAppointmentCompletedByCompleteId(appointmentCompletedId);
            var datetime = DateTime.Now;
            appointmentCompleted.UpdateDateTime = datetime;
            appointmentCompleted.UpdateUser = User.Identity.GetUserName();
            appointmentCompleted.IsReadyForInvoicing = true;

            var result = appointmentDal.Update_AppointmentCompleted(appointmentCompleted);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Determines whether [is ready for invoicing] [the specified appointment identifier].
        /// </summary>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <param name="start">The start.</param>
        [Authorize(Roles = "Customer")]
        public JsonResult IsReadyForInvoicing(long appointmentId, DateTime start)
        {
            var result = false;
            var appointmentDal = new SBAS_DAL.Appointment();

            var appointmentCompleted = appointmentDal.GetAppointmentCompletedAptId(appointmentId, start);

            if (appointmentCompleted != null)
            {
                result = appointmentCompleted.IsReadyForInvoicing;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// Determines whether [is appointment completed] [the specified appointment identifier].
        /// </summary>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <param name="start">The start.</param>
        [Authorize(Roles = "Customer")]
        public JsonResult IsAppointmentCompleted(long appointmentId, DateTime start)
        {
            var result = false;
            var appointmentDal = new SBAS_DAL.Appointment();

            var appointmentCompleted = appointmentDal.GetAppointmentCompletedAptId(appointmentId, start);

            if (appointmentCompleted != null)
            {
                result = appointmentCompleted.IsCompleted;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// Appointments the completed flags.
        /// </summary>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <param name="start">The start.</param>
        /// <returns>JsonResult.</returns>
        [Authorize(Roles = "Customer")]
        public JsonResult AppointmentCompletedFlags(long appointmentId, DateTime start)
        {
            var result = new AppointmentCompletedResults();
            var appointmentDal = new SBAS_DAL.Appointment();

            var appointmentCompleted = appointmentDal.GetAppointmentCompletedAptId(appointmentId, start);

            if (appointmentCompleted != null)
            {
                result = new AppointmentCompletedResults
               {
                   Id = appointmentCompleted.AppointmentCompletedId,
                   IsCompleted = appointmentCompleted.IsCompleted,
                   IsInvoiced = appointmentCompleted.IsInvoiced,
                   IsReadyForInvoicing = appointmentCompleted.IsReadyForInvoicing
               };
            }
            else
            {
                result = new AppointmentCompletedResults
                {
                    Id = 0,
                    IsCompleted = false,
                    IsInvoiced = false,
                    IsReadyForInvoicing = false
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Doeses the appointment meet time requirements.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="newAppointment">The new appointment.</param>
        /// <param name="operationType">Type of the operation.</param>
        /// <returns>JsonResult.</returns>
        [Authorize(Roles = "Customer")]
        public JsonResult DoesAppointmentMeetTimeRequirements(long customerId, Appointment newAppointment, string operationType)
        {
            var appointmentDal = new SBAS_DAL.Appointment();
            var prevAndNextAppointment = appointmentDal.GetPreviousAndNextAppointment(customerId, newAppointment, operationType);
            var result = appointmentDal.IsAppointmentCreatable(prevAndNextAppointment["PREV"], newAppointment, prevAndNextAppointment["NEXT"]);

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Gets the appointment line items model.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>List&lt;AppointmentLineItemModel&gt;.</returns>
        private static List<AppointmentLineItemModel> GetAppointmentLineItemsModel(long id)
        {
            var appointmentDal = new SBAS_DAL.Appointment();
            var inventoryitemDal = new SBAS_DAL.Inventory();

            var items = appointmentDal.GetAppointmentLineItemsByCompletedAppointmentId(id);
            var model = new List<AppointmentLineItemModel>();
            foreach (var appointmentLineItem in items)
            {
                var inventoryItem =
                    inventoryitemDal.GetUsersInventoryItemByInventoryItemId(appointmentLineItem.InventoryItemId);
                var appointmentLineItemModel = new AppointmentLineItemModel();
                appointmentLineItemModel.AppointmentCompletedId = appointmentLineItem.AppointmentCompletedId;
                appointmentLineItemModel.AppointmentLineItemId = appointmentLineItem.AppointmentLineItemId;
                appointmentLineItemModel.InventoryId = appointmentLineItem.InventoryId;
                appointmentLineItemModel.InventoryItemId = appointmentLineItem.InventoryItemId;
                appointmentLineItemModel.ItemPrice = appointmentLineItem.ItemPrice;
                appointmentLineItemModel.QuantityUsed = appointmentLineItem.QuantityUsed;
                appointmentLineItemModel.ItemName = inventoryItem.ItemName;
                appointmentLineItemModel.ItemDescription = inventoryItem.ItemDescription;


                model.Add(appointmentLineItemModel);
            }
            return model;
        }

        /// <summary>
        /// Gets the todays appointents.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="dateTime">The date time.</param>
        /// <returns>JsonResult.</returns>
        [Authorize(Roles="Customer")]
        public JsonResult GetTodaysAppointents(long customerId, string dateTime)
        {
            DateTime today = !String.IsNullOrEmpty(dateTime) ? Convert.ToDateTime(dateTime) : DateTime.Now;
            var startDateTime = string.Format("{0}/{1}/{2} {3}:{4}:{5}", today.Month, today.Day, today.Year, 0, 0, 01);
            var endDateTime = string.Format("{0}/{1}/{2} {3}:{4}:{5}",  today.Month, today.Day, today.Year, 23, 59, 59);

            var appointmentDal = new SBAS_DAL.Appointment();
            var mapInfo = appointmentDal.GetMapInfoByCustomerAndDate(customerId, Convert.ToDateTime(startDateTime), Convert.ToDateTime(endDateTime));

            return Json(mapInfo, JsonRequestBehavior.AllowGet);

        }

    }

    /// <summary>
    /// Class AppointmentCompletedResults.
    /// </summary>
    internal class AppointmentCompletedResults
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is completed.
        /// </summary>
        /// <value><c>true</c> if this instance is completed; otherwise, <c>false</c>.</value>
        public bool IsCompleted { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is ready for invoicing.
        /// </summary>
        /// <value><c>true</c> if this instance is ready for invoicing; otherwise, <c>false</c>.</value>
        public bool IsReadyForInvoicing { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is invoiced.
        /// </summary>
        /// <value><c>true</c> if this instance is invoiced; otherwise, <c>false</c>.</value>
        public bool IsInvoiced { get; set; }
    }
}

