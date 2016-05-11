// ***********************************************************************
// Assembly         : SBAS_UnitTest
// Author           : Ye Peng
// Created          : 06-01-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-10-2014
// ***********************************************************************
// <copyright file="AppointmentUnitTest.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
// This class files holds all the unit tests for Appointments, Appointments completed,
// Appointment line items and the algorithm for determining the distance and  time between
//</summary>
// ***********************************************************************

using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPoco;
using SBAS_Core.Model;
using SBAS_DAL;
using Address = SBAS_Core.Model.Address;
using Appointment = SBAS_Core.Model.Appointment;
using ClientList = SBAS_Core.Model.ClientList;
using SBASUser = SBAS_Core.Model.SBASUser;
using ServiceType = SBAS_Core.Model.ServiceType;
using Inventory = SBAS_Core.Model.Inventory;

/// <summary>
/// The SBAS_UnitTest namespace.
/// </summary>
namespace SBAS_UnitTest
{
    /// <summary>
    /// Class AppointmentUnitTest.
    /// </summary>
    [TestClass]
    public class AppointmentUnitTest
    {
        #region private static variables
        /// <summary>
        /// The customer address
        /// </summary>
        private static Address customerAddress;
        /// <summary>
        /// The customer
        /// </summary>
        private static SBASUser customer;
        /// <summary>
        /// The client address1
        /// </summary>
        private static Address clientAddress1;
        /// <summary>
        /// The client address2
        /// </summary>
        private static Address clientAddress2;
        /// <summary>
        /// The client address3
        /// </summary>
        private static Address clientAddress3;
        /// <summary>
        /// The client address4
        /// </summary>
        private static Address clientAddress4;
        /// <summary>
        /// The client address5
        /// </summary>
        private static Address clientAddress5;
        /// <summary>
        /// The client1
        /// </summary>
        private static SBASUser client1;
        /// <summary>
        /// The client2
        /// </summary>
        private static SBASUser client2;
        /// <summary>
        /// The client3
        /// </summary>
        private static SBASUser client3;
        /// <summary>
        /// The client4
        /// </summary>
        private static SBASUser client4;
        /// <summary>
        /// The client5
        /// </summary>
        private static SBASUser client5;
        /// <summary>
        /// The client list1
        /// </summary>
        private static ClientList clientList1;
        /// <summary>
        /// The client list2
        /// </summary>
        private static ClientList clientList2;
        /// <summary>
        /// The client list3
        /// </summary>
        private static ClientList clientList3;
        /// <summary>
        /// The client list4
        /// </summary>
        private static ClientList clientList4;
        /// <summary>
        /// The client list5
        /// </summary>
        private static ClientList clientList5;
        /// <summary>
        /// The service type1
        /// </summary>
        private static ServiceType serviceType1;
        /// <summary>
        /// The service type2
        /// </summary>
        private static ServiceType serviceType2;
        /// <summary>
        /// The service type3
        /// </summary>
        private static ServiceType serviceType3;
        /// <summary>
        /// The inventory
        /// </summary>
        private static Inventory inventory;
        /// <summary>
        /// The inventory item1
        /// </summary>
        private static InventoryItem inventoryItem1;
        /// <summary>
        /// The inventory item2
        /// </summary>
        private static InventoryItem inventoryItem2;
        /// <summary>
        /// The inventory item3
        /// </summary>
        private static InventoryItem inventoryItem3;

        /// <summary>
        /// The audit user name
        /// </summary>
        private static string auditUserName;
        /// <summary>
        /// The datetime
        /// </summary>
        private static DateTime datetime;
        /// <summary>
        /// The database
        /// </summary>
        private static Database db;
        #endregion

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        /// <summary>
        /// Appointments the unit test_ class initialize.
        /// </summary>
        /// <param name="testContext">The test context.</param>
        [ClassInitialize()]
        public static void AppointmentUnitTest_ClassInitialize(TestContext testContext)
        {
            db = new Database(Base.GetConnectionString, DatabaseType.SqlServer2012);
            using (db)
            {

                datetime = DateTime.Now;
                auditUserName = "RoyG";

                CreateCustomerAddresses();
                CreateClientAddesses();
                CreateCustomers();
                CreateClients();
                CreateClientList();

                CreateServiceTypes();
                CreateInventory();


            }
        }

        // Use ClassCleanup to run code after all tests in a class have run
        /// <summary>
        /// Appointments the unit test_ class cleanup.
        /// </summary>
        [ClassCleanup()]
        public static void AppointmentUnitTest_ClassCleanup()
        {
            using (db)
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append(string.Format("delete ClientList where ClientID = {0} and CustomerId = {1}",
                    clientList1.ClientId, clientList1.CustomerId));
                db.Execute(sqlbuilder);

                sqlbuilder.Append(string.Format("delete ClientList where ClientID = {0} and CustomerId = {1}",
                    clientList2.ClientId, clientList2.CustomerId));
                db.Execute(sqlbuilder);

                sqlbuilder.Append(string.Format("delete ClientList where ClientID = {0} and CustomerId = {1}",
                    clientList3.ClientId, clientList3.CustomerId));
                db.Execute(sqlbuilder);

                sqlbuilder.Append(string.Format("delete ClientList where ClientID = {0} and CustomerId = {1}",
                    clientList4.ClientId, clientList4.CustomerId));
                db.Execute(sqlbuilder);

                sqlbuilder.Append(string.Format("delete ClientList where ClientID = {0} and CustomerId = {1}",
                    clientList5.ClientId, clientList5.CustomerId));
                db.Execute(sqlbuilder);

                db.Delete("InventoryItem", "InventoryItemId", inventoryItem1, inventoryItem1.InventoryItemId);
                db.Delete("InventoryItem", "InventoryItemId", inventoryItem2, inventoryItem2.InventoryItemId);
                db.Delete("InventoryItem", "InventoryItemId", inventoryItem3, inventoryItem3.InventoryItemId);
                db.Delete("Inventory", "InventoryId", inventory, inventory.InventoryId);

                db.Delete("ServiceType", "ServiceTypeId", serviceType1, serviceType1.ServiceTypeId);
                db.Delete("ServiceType", "ServiceTypeId", serviceType2, serviceType2.ServiceTypeId);
                db.Delete("ServiceType", "ServiceTypeId", serviceType3, serviceType3.ServiceTypeId);


                db.Delete("SBASUser", "UserId", customer, customer.UserId);

                db.Delete("SBASUser", "UserId", client1, client1.UserId);
                db.Delete("SBASUser", "UserId", client2, client2.UserId);
                db.Delete("SBASUser", "UserId", client3, client3.UserId);
                db.Delete("SBASUser", "UserId", client4, client4.UserId);
                db.Delete("SBASUser", "UserId", client5, client5.UserId);


                db.Delete("Address", "AddressId", customerAddress, customerAddress.AddressId);

                db.Delete("Address", "AddressId", clientAddress1, clientAddress1.AddressId);
                db.Delete("Address", "AddressId", clientAddress2, clientAddress2.AddressId);
                db.Delete("Address", "AddressId", clientAddress3, clientAddress3.AddressId);
                db.Delete("Address", "AddressId", clientAddress4, clientAddress4.AddressId);
                db.Delete("Address", "AddressId", clientAddress5, clientAddress5.AddressId);
            }
        }

        #endregion

        #region Appointment Unit Tests Methods
        /// <summary>
        /// Creates the appointments.
        /// </summary>
        [TestMethod]
        public void CreateAppointments()
        {
            using (db)
            {
                var appointments = new List<Appointment>();
                try
                {
                    var appointmentDal = new SBAS_DAL.Appointment();

                    var appointment = new Appointment();
                    appointment.AddressId = client1.AddressId;
                    appointment.ClientId = client1.UserId;
                    appointment.CustomerId = customer.UserId;
                    appointment.Title = "Test Appointment";
                    appointment.Description = "Test Appointment Description";
                    appointment.IsAllDay = false;
                    appointment.Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 0, 0, 0, DateTimeKind.Local);
                    appointment.End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 0, 0, 0, DateTimeKind.Local);
                    appointment.RecurrenceRule = null;
                    appointment.RecurrenceException = null;
                    appointment.RecurrenceId = null;
                    appointment.ServiceTypeId = serviceType1.ServiceTypeId;
                    appointment.CreateUser = auditUserName;
                    appointment.UpdateUser = auditUserName;
                    appointment.CreateDateTime = datetime;
                    appointment.UpdateDateTime = datetime;

                    var newaAppointment = appointmentDal.AddAppointment(appointment);

                    Assert.AreNotEqual(0, newaAppointment.AppointmentId, "The Appointment was not created!");
                    appointments.Add(newaAppointment);

                    appointment = new Appointment();
                    appointment.AddressId = client1.AddressId;
                    appointment.ClientId = client1.UserId;
                    appointment.CustomerId = customer.UserId;
                    appointment.Title = "Test Appointment 2";
                    appointment.Description = "Test Appointment Description 2";
                    appointment.IsAllDay = false;
                    appointment.Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 0, 0, 0, DateTimeKind.Local);
                    appointment.End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 0, 0, 0, DateTimeKind.Local);
                    appointment.RecurrenceRule = null;
                    appointment.RecurrenceException = null;
                    appointment.RecurrenceId = null;
                    appointment.ServiceTypeId = serviceType1.ServiceTypeId;
                    appointment.CreateUser = auditUserName;
                    appointment.UpdateUser = auditUserName;
                    appointment.CreateDateTime = datetime;
                    appointment.UpdateDateTime = datetime;

                    newaAppointment = appointmentDal.AddAppointment(appointment);

                    Assert.AreNotEqual(0, newaAppointment.AppointmentId, "The Appointment was not created!");
                    appointments.Add(newaAppointment);
                }
                finally
                {
                    if (appointments.Any())
                    {
                        var appointment = new SBAS_DAL.Appointment();

                        foreach (var appt in appointments)
                        {
                            appointment.DeleteAppointment(appt);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Lists the customer appointments by date range.
        /// </summary>
        [TestMethod] 
        public void ListCustomerAppointmentsByDateRange()
        {
            using (db)
            {
                var appointments = new List<Appointment>();
                var appointmentResults = new List<Appointment>();
                var appointmentDal = new SBAS_DAL.Appointment();

                try
                {
                    var appointment = new Appointment();
                    appointment.AddressId = client1.AddressId;
                    appointment.ClientId = client1.UserId;
                    appointment.CustomerId = customer.UserId;
                    appointment.Title = "Test Appointment 3";
                    appointment.Description = "Test Appointment Description 3";
                    appointment.IsAllDay = false;
                    appointment.Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 0, 0, 0, DateTimeKind.Local);
                    appointment.End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 0, 0, 0, DateTimeKind.Local);
                    appointment.RecurrenceRule = null;
                    appointment.RecurrenceException = null;
                    appointment.RecurrenceId = null;
                    appointment.ServiceTypeId = serviceType1.ServiceTypeId;
                    appointment.CreateUser = auditUserName;
                    appointment.UpdateUser = auditUserName;
                    appointment.CreateDateTime = datetime;
                    appointment.UpdateDateTime = datetime;

                    var newAppointment = appointmentDal.AddAppointment(appointment);

                    appointments.Add(newAppointment);

                    appointment = new Appointment();
                    appointment.AddressId = client1.AddressId;
                    appointment.ClientId = client1.UserId;
                    appointment.CustomerId = customer.UserId;
                    appointment.Title = "Test Appointment 4";
                    appointment.Description = "Test Appointment Description 4";
                    appointment.IsAllDay = false;
                    appointment.Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 0, 0, 0, DateTimeKind.Local);
                    appointment.End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 0, 0, 0, DateTimeKind.Local);
                    appointment.RecurrenceRule = null;
                    appointment.RecurrenceException = null;
                    appointment.RecurrenceId = null;
                    appointment.ServiceTypeId = serviceType1.ServiceTypeId;
                    appointment.CreateUser = auditUserName;
                    appointment.UpdateUser = auditUserName;
                    appointment.CreateDateTime = datetime;
                    appointment.UpdateDateTime = datetime;

                    newAppointment = appointmentDal.AddAppointment(appointment);
                    appointments.Add(newAppointment);

                    appointmentResults = appointmentDal.GetAppointmentsByCustomerId(customer.UserId, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));
                    CollectionAssert.AreNotEqual(appointments, appointmentResults, "Added appointments do not match selected appointments results");

                }
                finally
                {
                    if (appointments.Any())
                    {
                        foreach (var appt in appointments)
                        {
                            new SBAS_DAL.Appointment().DeleteAppointment(appt);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Lists the client appointments by date range.
        /// </summary>
        [TestMethod] 
        public void ListClientAppointmentsByDateRange()
        {
            using (db)
            {
                var appointments = new List<Appointment>();
                var appointmentDal = new SBAS_DAL.Appointment();
                try
                {
                    var appointment = new Appointment();
                    appointment.AddressId = client1.AddressId;
                    appointment.ClientId = client1.UserId;
                    appointment.CustomerId = customer.UserId;
                    appointment.Title = "Test Appointment 5";
                    appointment.Description = "Test Appointment Description 5";
                    appointment.IsAllDay = false;
                    appointment.Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0, 0, DateTimeKind.Local);
                    appointment.End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0, 0, DateTimeKind.Local);
                    appointment.RecurrenceRule = null;
                    appointment.RecurrenceException = null;
                    appointment.RecurrenceId = null;
                    appointment.ServiceTypeId = serviceType1.ServiceTypeId;
                    appointment.CreateUser = auditUserName;
                    appointment.UpdateUser = auditUserName;
                    appointment.CreateDateTime = datetime;
                    appointment.UpdateDateTime = datetime;

                    Appointment newAppointment = appointmentDal.AddAppointment(appointment);

                    appointments.Add(newAppointment);


                    List<Appointment> appointmentResults = appointmentDal.GetAppointmentsByClientId(client1.UserId, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));
                    CollectionAssert.AreNotEqual(appointments, appointmentResults, "Added appointments do not match selected appointments results");

                }
                finally
                {
                    if (appointments.Any())
                    {
                        foreach (var appt in appointments)
                        {
                            bool result = appointmentDal.DeleteAppointment(appt);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the appointments.
        /// </summary>
        [TestMethod] 
        public void UpdateAppointments()
        {
            using (db)
            {
                var newAddress = new Address();
                var newAppointment = new Appointment();
                var appointmentDal = new SBAS_DAL.Appointment();
                try
                {
                    var appointment = new Appointment();
                    appointment.AddressId = client1.AddressId;
                    appointment.ClientId = client1.UserId;
                    appointment.CustomerId = customer.UserId;
                    appointment.Title = "Test Appointment 7";
                    appointment.Description = "Test Appointment Description 7";
                    appointment.IsAllDay = false;
                    appointment.Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 11, 0, 0, 0, DateTimeKind.Local);
                    appointment.End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0, 0, DateTimeKind.Local);
                    appointment.RecurrenceRule = null;
                    appointment.RecurrenceException = null;
                    appointment.RecurrenceId = null;
                    appointment.ServiceTypeId = serviceType1.ServiceTypeId;
                    appointment.CreateUser = auditUserName;
                    appointment.UpdateUser = auditUserName;
                    appointment.CreateDateTime = datetime;
                    appointment.UpdateDateTime = datetime;

                    newAppointment = appointmentDal.AddAppointment(appointment);

                    newAddress = new Address();
                    newAddress.AddressLine1 = "143 Park Drive";
                    newAddress.AddressLine2 = "Suite 101";
                    newAddress.CityId = 6323;
                    newAddress.StateId = 40;
                    newAddress.ZipCode = "16055";
                    newAddress.Longitude = null;
                    newAddress.Latitude = null;
                    newAddress.CreateUser = auditUserName;
                    newAddress.UpdateUser = auditUserName;
                    newAddress.CreateDateTime = datetime;
                    newAddress.UpdateDateTime = datetime;

                    db.Insert("Address", "AddressId", true, newAddress);

                    newAppointment.AddressId = newAddress.AddressId;
                    var updatedAppointment = appointmentDal.UpdateAppointment(newAppointment);

                    var reslectedAppointment =
                        appointmentDal.GetAppointmentByAppointmentAndCustomerIds(customer.UserId,
                            updatedAppointment.AppointmentId);

                    Assert.AreEqual(updatedAppointment.AddressId, reslectedAppointment.AddressId, "The Address was not updated!");

                }
                finally
                {
                    db.Delete("Appointment", "AppointmentId", newAppointment, newAppointment.AppointmentId);
                    db.Delete("Address", "AddressId", newAddress, newAddress.AddressId);
                }
            }
        }

        /// <summary>
        /// Deletes the appointments.
        /// </summary>
        [TestMethod] 
        public void DeleteAppointments()
        {
            using (db)
            {
                var appointmentDal = new SBAS_DAL.Appointment();
                try
                {
                    var appointment = new Appointment();
                    appointment.AddressId = client1.AddressId;
                    appointment.ClientId = client1.UserId;
                    appointment.CustomerId = customer.UserId;
                    appointment.Title = "Test Appointment 8";
                    appointment.Description = "Test Appointment Description 8";
                    appointment.IsAllDay = false;
                    appointment.Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 0, 0, 0, DateTimeKind.Local);
                    appointment.End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 11, 0, 0, 0, DateTimeKind.Local);
                    appointment.RecurrenceRule = null;
                    appointment.RecurrenceException = null;
                    appointment.RecurrenceId = null;
                    appointment.ServiceTypeId = serviceType1.ServiceTypeId;
                    appointment.CreateUser = auditUserName;
                    appointment.UpdateUser = auditUserName;
                    appointment.CreateDateTime = datetime;
                    appointment.UpdateDateTime = datetime;

                    var newAppointment = appointmentDal.AddAppointment(appointment);

                    bool result = new SBAS_DAL.Appointment().DeleteAppointment(newAppointment);
                    Assert.AreNotEqual(true, result, "Appointment deleted!");

                }
                catch (Exception ex)
                {

                }

            }
        }

        /// <summary>
        /// Determines whether [is appointment creatable].
        /// </summary>
        [TestMethod] 
        public void IsAppointmentCreatable()
        {
            using (db)
            {
                var appointmentDal = new SBAS_DAL.Appointment();
                var dateTime = DateTime.Now.AddDays(10);
                var appointment = BuildAppointment(client1.UserId, customer.UserId, client1.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 11, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 12, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");

                var appointment2 = BuildAppointment(client2.UserId, customer.UserId, client2.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 13, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 14, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");

                var appointment3 = BuildAppointment(client3.UserId, customer.UserId, client3.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");
                var result = appointmentDal.IsAppointmentCreatable(appointment, appointment2, appointment3);
                Assert.AreEqual(true, result, "Appointment Is not creatable.");
            }
        }

        /// <summary>
        /// Gets the previous and next appointment.
        /// </summary>
        [TestMethod] 
        public void GetPreviousAndNextAppointment()
        {
            using (db)
            {
                var appointmentDal = new SBAS_DAL.Appointment();
                var dateTime = DateTime.Now.AddDays(11);
                var appointment = BuildAppointment(client1.UserId, customer.UserId, client1.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 11, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 12, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");

                var appointment2 = BuildAppointment(client2.UserId, customer.UserId, client2.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 13, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 14, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");

                var appointment3 = BuildAppointment(client3.UserId, customer.UserId, client3.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");
                try
                {
                    appointmentDal.AddAppointment(appointment);
                    appointmentDal.AddAppointment(appointment3);
                    var prenextdic = appointmentDal.GetPreviousAndNextAppointment(customer.UserId, appointment2, "Insert");

                    var prev = prenextdic["PREV"];
                    var next = prenextdic["NEXT"];
                    var result = prev != null && next != null ? true : false;
                    Assert.AreEqual(true, result, "Appointment Is not creatable.");
                }
                finally
                {
                    appointmentDal.DeleteAppointment(appointment);
                    appointmentDal.DeleteAppointment(appointment3);

                }
            }
        }

        /// <summary>
        /// Validates the appointment time range.
        /// </summary>
        [TestMethod] 
        public void ValidateAppointmentTimeRange()
        {
            using (db)
            {
                var appointmentDal = new SBAS_DAL.Appointment();
                var dateTime = DateTime.Now.AddDays(15);
                var appointment = BuildAppointment(client1.UserId, customer.UserId, client1.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 11, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 12, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");

                var appointment2 = BuildAppointment(client2.UserId, customer.UserId, client2.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 13, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 14, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");

                var appointment3 = BuildAppointment(client3.UserId, customer.UserId, client3.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");
                try
                {
                    appointmentDal.AddAppointment(appointment);
                    appointmentDal.AddAppointment(appointment3);

                    var result = appointmentDal.ValidateAppointmentTimeRange(appointment2, "Insert");

                    
                    Assert.AreEqual(true, result, "Appointment Is not creatable.");
                }
                finally
                {
                    appointmentDal.DeleteAppointment(appointment);
                    appointmentDal.DeleteAppointment(appointment3);

                }
            }
        }
        
        #endregion

        #region Appointment Completed Unit Tests

        /// <summary>
        /// Creates the and delete appointment completed.
        /// </summary>
        [TestMethod] 
        public void CreateAndDeleteAppointmentCompleted()
        {
            using (db)
            {
                var appointmentDal = new SBAS_DAL.Appointment();
                var dateTime = DateTime.Now;
                var appointment = BuildAppointment(client1.UserId, customer.UserId, client1.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");
                var appointmentCompleted = new AppointmentCompleted();
                try
                {
                    appointmentDal.AddAppointment(appointment);
                    appointmentCompleted = BuildAppointmentCompleted(appointment, true);
                    appointmentDal.Insert_AppointmentCompleted(appointmentCompleted);
                    Assert.AreNotEqual(0,appointmentCompleted.AppointmentCompletedId,"Appointment Completed record not created.");
                    var result = appointmentDal.Delete_AppointmentCompleted(appointmentCompleted);
                    Assert.AreEqual(true, result, "Appointment Completed record was not deleted.");
                }
                finally
                {
                   
                    appointmentDal.DeleteAppointment(appointment);
                }
            }
        }

        /// <summary>
        /// Updates the appointment compelted.
        /// </summary>
        [TestMethod] 
        public void UpdateAppointmentCompelted()
        {
            using (db)
            {
                var appointmentDal = new SBAS_DAL.Appointment();
                var dateTime = DateTime.Now;
                var appointment = BuildAppointment(client1.UserId, customer.UserId, client1.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");
                var appointmentCompleted = new AppointmentCompleted();
                try
                {
                    appointmentDal.AddAppointment(appointment);
                    appointmentCompleted = BuildAppointmentCompleted(appointment, true);
                    appointmentDal.Insert_AppointmentCompleted(appointmentCompleted);
                    appointmentCompleted.IsReadyForInvoicing = true;
                    appointmentDal.Update_AppointmentCompleted(appointmentCompleted);
                    Assert.AreEqual(true, appointmentCompleted.IsReadyForInvoicing, "Appointment Completed record was not set ready for invoicing.");
                    
                }
                finally
                {
                    appointmentDal.Delete_AppointmentCompleted(appointmentCompleted);
                    appointmentDal.DeleteAppointment(appointment);
                }
            }
        }

        /// <summary>
        /// Gets the appointment completed by identifier.
        /// </summary>
        [TestMethod] 
        public void GetAppointmentCompletedById()
        {
            using (db)
            {
                var appointmentDal = new SBAS_DAL.Appointment();
                var dateTime = DateTime.Now;
                var appointment = BuildAppointment(client1.UserId, customer.UserId, client1.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");
                var appointmentCompleted = new AppointmentCompleted();
                try
                {
                    appointmentDal.AddAppointment(appointment);
                    appointmentCompleted = BuildAppointmentCompleted(appointment, true);
                    appointmentDal.Insert_AppointmentCompleted(appointmentCompleted);
                    var selctedAppointmentCompleted =
                        appointmentDal.GetAppointmentCompletedAptId(appointmentCompleted.AppointmentId,
                            appointmentCompleted.CompletionDateTime);
                    Assert.AreEqual(selctedAppointmentCompleted.AppointmentCompletedId,
                        appointmentCompleted.AppointmentCompletedId,
                        "Appointment Completed record was not return in selection.");

                }
                finally
                {
                    appointmentDal.Delete_AppointmentCompleted(appointmentCompleted);
                    appointmentDal.DeleteAppointment(appointment);
                }
            }
        }

        /// <summary>
        /// Checks if appointment record exists.
        /// </summary>
        [TestMethod] 
        public void CheckIfAppointmentRecordExists()
        {
            using (db)
            {
                var appointmentDal = new SBAS_DAL.Appointment();
                var dateTime = DateTime.Now;
                var appointment = BuildAppointment(client1.UserId, customer.UserId, client1.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");
                var appointmentCompleted = new AppointmentCompleted();
                try
                {
                    appointmentDal.AddAppointment(appointment);
                    appointmentCompleted = BuildAppointmentCompleted(appointment, true);
                    appointmentDal.Insert_AppointmentCompleted(appointmentCompleted);
                    var result = appointmentDal.AppointmentCompletedRecordExists(
                        appointmentCompleted.CompletionDateTime, appointmentCompleted.AppointmentId);
                    Assert.AreEqual(true,result,"Appointment Completed record does not exists.");
                }
                finally
                {
                    appointmentDal.Delete_AppointmentCompleted(appointmentCompleted);
                    appointmentDal.DeleteAppointment(appointment);
                }
            }
        }

        /// <summary>
        /// Checks if appoinment is completed.
        /// </summary>
        [TestMethod] 
        private void CheckIfAppoinmentIsCompleted()
        {
            using (db)
            {
                var appointmentDal = new SBAS_DAL.Appointment();
                var dateTime = DateTime.Now;
                var appointment = BuildAppointment(client1.UserId, customer.UserId, client1.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");
                var appointmentCompleted = new AppointmentCompleted();
                try
                {
                    appointmentDal.AddAppointment(appointment);
                    appointmentCompleted = BuildAppointmentCompleted(appointment, true);
                    appointmentDal.Insert_AppointmentCompleted(appointmentCompleted);
                    var result = appointmentDal.IsAppointmentCompleted(appointmentCompleted.CompletionDateTime, false,
                        appointmentCompleted.AppointmentId);
                    Assert.AreEqual(true, result, "Appointment Completed record is completed.");
                }
                finally
                {
                    appointmentDal.Delete_AppointmentCompleted(appointmentCompleted);
                    appointmentDal.DeleteAppointment(appointment);
                }
            }
        }

        /// <summary>
        /// Gets all appointment completed by date and invoiced.
        /// </summary>
        [TestMethod] 
        public void GetAllAppointmentCompletedByDateAndInvoiced()
        {
            using (db)
            {
                var appointmentDal = new SBAS_DAL.Appointment();
                var dateTime = DateTime.Now;
                dateTime.AddDays(2);
                var appointment = BuildAppointment(client1.UserId, customer.UserId, client1.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");
                var appointmentCompleted = new AppointmentCompleted();
                try
                {
                    appointmentDal.AddAppointment(appointment);
                    appointmentCompleted = BuildAppointmentCompleted(appointment, true);
                    appointmentDal.Insert_AppointmentCompleted(appointmentCompleted);



                    var selctedAppointmentCompleted =
                        appointmentDal.GetAllAppointmentCompletedByDateAndInvoiced(customer.UserId, appointment.Start,
                            appointment.End, false);


                    Assert.AreEqual(selctedAppointmentCompleted.Count, 1,
                        "Appointment Completed record was not return in selection.");

                }
                finally
                {
                    appointmentDal.Delete_AppointmentCompleted(appointmentCompleted);
                    appointmentDal.DeleteAppointment(appointment);
                }
            }
        }

        /// <summary>
        /// Gets the completed appointments by user.
        /// </summary>
        [TestMethod] 
        public void GetCompletedAppointmentsByUser()
        {
            using (db)
            {
                var appointmentDal = new SBAS_DAL.Appointment();
                var dateTime = DateTime.Now;
                dateTime.AddDays(3);
                var appointment = BuildAppointment(client1.UserId, customer.UserId, client1.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");
                var appointmentCompleted = new AppointmentCompleted();
                try
                {
                    appointmentDal.AddAppointment(appointment);
                    appointmentCompleted = BuildAppointmentCompleted(appointment, true);
                    appointmentDal.Insert_AppointmentCompleted(appointmentCompleted);



                    var selctedAppointmentCompleted = appointmentDal.GetCompletedAppointmentsByUser(customer.UserId,
                        false, false);
                        //appointmentDal.GetAllAppointmentCompletedByDateAndInvoiced(customer.UserId, appointment.Start,
                        //    appointment.End, false);


                    Assert.AreEqual(selctedAppointmentCompleted.Count, 1,
                        "Appointment Completed record was not return in selection.");

                }
                finally
                {
                    appointmentDal.Delete_AppointmentCompleted(appointmentCompleted);
                    appointmentDal.DeleteAppointment(appointment);
                }
            }
        }

        /// <summary>
        /// Gets the appointment completed by complete identifier.
        /// </summary>
        [TestMethod] 
        public void GetAppointmentCompletedByCompleteId()
        {
            using (db)
            {
                var appointmentDal = new SBAS_DAL.Appointment();
                var dateTime = DateTime.Now;
                dateTime.AddDays(3);
                var appointment = BuildAppointment(client1.UserId, customer.UserId, client1.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");
                var appointmentCompleted = new AppointmentCompleted();
                try
                {
                    appointmentDal.AddAppointment(appointment);
                    appointmentCompleted = BuildAppointmentCompleted(appointment, true);
                    appointmentDal.Insert_AppointmentCompleted(appointmentCompleted);



                    var selctedAppointmentCompleted =
                        appointmentDal.GetAppointmentCompletedByCompleteId(appointmentCompleted.AppointmentCompletedId);


                    Assert.AreEqual(selctedAppointmentCompleted.AppointmentCompletedId, appointmentCompleted.AppointmentCompletedId,
                        "Appointment Completed record did not match.");

                }
                finally
                {
                    appointmentDal.Delete_AppointmentCompleted(appointmentCompleted);
                    appointmentDal.DeleteAppointment(appointment);
                }
            }
        }

        /// <summary>
        /// Gets the appointment completed apt identifier.
        /// </summary>
        [TestMethod] 
        public void GetAppointmentCompletedAptId()
        {
            using (db)
            {
                var appointmentDal = new SBAS_DAL.Appointment();
                var dateTime = DateTime.Now;
                dateTime.AddDays(3);
                var appointment = BuildAppointment(client1.UserId, customer.UserId, client1.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");
                var appointmentCompleted = new AppointmentCompleted();
                try
                {
                    appointmentDal.AddAppointment(appointment);
                    appointmentCompleted = BuildAppointmentCompleted(appointment, true);
                    appointmentDal.Insert_AppointmentCompleted(appointmentCompleted);



                    var selctedAppointmentCompleted =
                        appointmentDal.GetAppointmentCompletedAptId(appointment.AppointmentId,
                            appointmentCompleted.CompletionDateTime);


                    Assert.AreEqual(selctedAppointmentCompleted.AppointmentCompletedId, appointmentCompleted.AppointmentCompletedId,
                        "Appointment Completed record did not match.");

                }
                finally
                {
                    appointmentDal.Delete_AppointmentCompleted(appointmentCompleted);
                    appointmentDal.DeleteAppointment(appointment);
                }
            }
        }

        /// <summary>
        /// Gets the customer appointment completed by date.
        /// </summary>
        [TestMethod] 
        public void GetCustomerAppointmentCompletedByDate()
        {
            using (db)
            {
                var appointmentDal = new SBAS_DAL.Appointment();
                var dateTime = DateTime.Now;
                dateTime.AddDays(3);
                var appointment = BuildAppointment(client1.UserId, customer.UserId, client1.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");
                var appointmentCompleted = new AppointmentCompleted();
                try
                {
                    appointmentDal.AddAppointment(appointment);
                    appointmentCompleted = BuildAppointmentCompleted(appointment, true);
                    appointmentDal.Insert_AppointmentCompleted(appointmentCompleted);



                    var selctedAppointmentCompleted =
                        appointmentDal.GetCustomerAppointmentCompletedByDate(customer.UserId, appointment.Start,
                            appointment.End, appointment.AppointmentId);


                    Assert.AreEqual(selctedAppointmentCompleted.AppointmentCompletedId, appointmentCompleted.AppointmentCompletedId,
                        "Appointment Completed record did not match.");

                }
                finally
                {
                    appointmentDal.Delete_AppointmentCompleted(appointmentCompleted);
                    appointmentDal.DeleteAppointment(appointment);
                }
            }
        }

        /// <summary>
        /// Gets the client appointment completed by date.
        /// </summary>
        [TestMethod] 
        public void GetClientAppointmentCompletedByDate()
        {
            using (db)
            {
                var appointmentDal = new SBAS_DAL.Appointment();
                var dateTime = DateTime.Now;
                dateTime.AddDays(3);
                var appointment = BuildAppointment(client1.UserId, customer.UserId, client1.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");
                var appointmentCompleted = new AppointmentCompleted();
                try
                {
                    appointmentDal.AddAppointment(appointment);
                    appointmentCompleted = BuildAppointmentCompleted(appointment, true);
                    appointmentDal.Insert_AppointmentCompleted(appointmentCompleted);



                    var selctedAppointmentCompleted =
                        appointmentDal.GetClientAppointmentCompletedByDate(client1.UserId, appointment.Start,
                            appointment.End, appointment.AppointmentId);

                    Assert.AreEqual(selctedAppointmentCompleted.AppointmentCompletedId, appointmentCompleted.AppointmentCompletedId,
                        "Appointment Completed record did not match.");

                }
                finally
                {
                    appointmentDal.Delete_AppointmentCompleted(appointmentCompleted);
                    appointmentDal.DeleteAppointment(appointment);
                }
            }
        }

        #endregion

        #region Appointment Line Item Unit Tests

        /// <summary>
        /// Adds the appointment line item.
        /// </summary>
        [TestMethod] 
        public void AddAppointmentLineItem()
        {
            using (db)
            {
                var appointmentDal = new SBAS_DAL.Appointment();
                var dateTime = DateTime.Now;
                dateTime.AddDays(3);
                var appointment = BuildAppointment(client1.UserId, customer.UserId, client1.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");
                var appointmentCompleted = new AppointmentCompleted();
                var appointmentLineItem = new AppointmentLineItem();
                try
                {
                    appointmentDal.AddAppointment(appointment);
                    appointmentCompleted = BuildAppointmentCompleted(appointment, true);
                    appointmentDal.Insert_AppointmentCompleted(appointmentCompleted);



                    appointmentLineItem = new AppointmentLineItem();
                    appointmentLineItem.AppointmentCompletedId = appointmentCompleted.AppointmentCompletedId;
                    appointmentLineItem.InventoryId = inventory.InventoryId;
                    appointmentLineItem.InventoryItemId = inventoryItem1.InventoryItemId;
                    appointmentLineItem.ItemPrice = inventoryItem1.ItemPrice;
                    appointmentLineItem.QuantityUsed = 5;
                    appointmentLineItem.UpdateDateTime = datetime;
                    appointmentLineItem.CreateDateTime = datetime;
                    appointmentLineItem.UpdateUser = auditUserName;
                    appointmentLineItem.CreateUser = auditUserName;

                    appointmentDal.Insert_AppointmentLineItem(appointmentLineItem);


                    Assert.AreNotEqual(appointmentLineItem.AppointmentLineItemId, 0,
                        "Appointment Line Item  did not create.");

                }
                finally
                {
                    appointmentDal.Delete_AppointmentLineItem(appointmentLineItem);
                    appointmentDal.Delete_AppointmentCompleted(appointmentCompleted);
                    appointmentDal.DeleteAppointment(appointment);
                }
            }
        }

        /// <summary>
        /// Deletes the appointment line item.
        /// </summary>
        [TestMethod] 
        public void DeleteAppointmentLineItem()
        {
            using (db)
            {
                var appointmentDal = new SBAS_DAL.Appointment();
                var dateTime = DateTime.Now;
                dateTime.AddDays(3);
                var appointment = BuildAppointment(client1.UserId, customer.UserId, client1.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");
                var appointmentCompleted = new AppointmentCompleted();
                var appointmentLineItem = new AppointmentLineItem();
                try
                {
                    appointmentDal.AddAppointment(appointment);
                    appointmentCompleted = BuildAppointmentCompleted(appointment, true);
                    appointmentDal.Insert_AppointmentCompleted(appointmentCompleted);



                    appointmentLineItem = new AppointmentLineItem();
                    appointmentLineItem.AppointmentCompletedId = appointmentCompleted.AppointmentCompletedId;
                    appointmentLineItem.InventoryId = inventory.InventoryId;
                    appointmentLineItem.InventoryItemId = inventoryItem1.InventoryItemId;
                    appointmentLineItem.ItemPrice = inventoryItem1.ItemPrice;
                    appointmentLineItem.QuantityUsed = 5;
                    appointmentLineItem.UpdateDateTime = datetime;
                    appointmentLineItem.CreateDateTime = datetime;
                    appointmentLineItem.UpdateUser = auditUserName;
                    appointmentLineItem.CreateUser = auditUserName;

                    appointmentDal.Insert_AppointmentLineItem(appointmentLineItem);
                    var result = appointmentDal.Delete_AppointmentLineItem(appointmentLineItem);

                    Assert.AreEqual(result, true, "Appointment Line Item did not delete.");

                }
                finally
                {
                   
                    appointmentDal.Delete_AppointmentCompleted(appointmentCompleted);
                    appointmentDal.DeleteAppointment(appointment);
                }
            }
        }

        /// <summary>
        /// Updates the appointment line item.
        /// </summary>
        [TestMethod] 
        public void UpdateAppointmentLineItem()
        {
            using (db)
            {
                var appointmentDal = new SBAS_DAL.Appointment();
                var dateTime = DateTime.Now;
                dateTime.AddDays(7);
                var appointment = BuildAppointment(client1.UserId, customer.UserId, client1.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");
                var appointmentCompleted = new AppointmentCompleted();
                var appointmentLineItem = new AppointmentLineItem();
                try
                {
                    appointmentDal.AddAppointment(appointment);
                    appointmentCompleted = BuildAppointmentCompleted(appointment, true);
                    appointmentDal.Insert_AppointmentCompleted(appointmentCompleted);



                    appointmentLineItem = new AppointmentLineItem();
                    appointmentLineItem.AppointmentCompletedId = appointmentCompleted.AppointmentCompletedId;
                    appointmentLineItem.InventoryId = inventory.InventoryId;
                    appointmentLineItem.InventoryItemId = inventoryItem1.InventoryItemId;
                    appointmentLineItem.ItemPrice = inventoryItem1.ItemPrice;
                    appointmentLineItem.QuantityUsed = 5;
                    appointmentLineItem.UpdateDateTime = datetime;
                    appointmentLineItem.CreateDateTime = datetime;
                    appointmentLineItem.UpdateUser = auditUserName;
                    appointmentLineItem.CreateUser = auditUserName;

                    appointmentDal.Insert_AppointmentLineItem(appointmentLineItem);

                    var appointmentLineItem2 = new AppointmentLineItem();
                    appointmentLineItem2.ItemPrice = appointmentLineItem.ItemPrice;
                    appointmentLineItem2.InventoryItemId = appointmentLineItem.InventoryItemId;
                    appointmentLineItem2.InventoryId = appointmentLineItem.InventoryId;
                    appointmentLineItem2.QuantityUsed = 8;
                    appointmentLineItem2.UpdateDateTime = datetime;
                    appointmentLineItem2.CreateDateTime = datetime;
                    appointmentLineItem2.UpdateUser = auditUserName;
                    appointmentLineItem2.CreateUser = auditUserName;


                    appointmentDal.Update_AppointmentLineItem(appointmentLineItem2);

                    Assert.AreNotEqual(appointmentLineItem.QuantityUsed, appointmentLineItem2.QuantityUsed,
                        "Appointment Line Item  did not create.");

                }
                finally
                {
                    appointmentDal.Delete_AppointmentLineItem(appointmentLineItem);
                    appointmentDal.Delete_AppointmentCompleted(appointmentCompleted);
                    appointmentDal.DeleteAppointment(appointment);
                }
            }
        }

        /// <summary>
        /// Gets all appointment line items.
        /// </summary>
        [TestMethod] 
        public void GetAllAppointmentLineItems()
        {
            using (db)
            {
                var appointmentDal = new SBAS_DAL.Appointment();
                var dateTime = DateTime.Now;
                dateTime.AddDays(8);
                var appointment = BuildAppointment(client1.UserId, customer.UserId, client1.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");
                var appointmentCompleted = new AppointmentCompleted();
                var appointmentLineItem = new AppointmentLineItem();
                var appointmentLineItem2 = new AppointmentLineItem();
                try
                {
                    appointmentDal.AddAppointment(appointment);
                    appointmentCompleted = BuildAppointmentCompleted(appointment, true);
                    appointmentDal.Insert_AppointmentCompleted(appointmentCompleted);



                    appointmentLineItem = new AppointmentLineItem();
                    appointmentLineItem.AppointmentCompletedId = appointmentCompleted.AppointmentCompletedId;
                    appointmentLineItem.InventoryId = inventory.InventoryId;
                    appointmentLineItem.InventoryItemId = inventoryItem1.InventoryItemId;
                    appointmentLineItem.ItemPrice = inventoryItem1.ItemPrice;
                    appointmentLineItem.QuantityUsed = 5;
                    appointmentLineItem.UpdateDateTime = datetime;
                    appointmentLineItem.CreateDateTime = datetime;
                    appointmentLineItem.UpdateUser = auditUserName;
                    appointmentLineItem.CreateUser = auditUserName;

                    appointmentDal.Insert_AppointmentLineItem(appointmentLineItem);

                    appointmentLineItem2 = new AppointmentLineItem();
                    appointmentLineItem2.AppointmentCompletedId = appointmentCompleted.AppointmentCompletedId;
                    appointmentLineItem2.InventoryId = inventory.InventoryId;
                    appointmentLineItem2.InventoryItemId = inventoryItem1.InventoryItemId;
                    appointmentLineItem2.ItemPrice = inventoryItem1.ItemPrice;
                    appointmentLineItem2.QuantityUsed = 5;
                    appointmentLineItem2.UpdateDateTime = datetime;
                    appointmentLineItem2.CreateDateTime = datetime;
                    appointmentLineItem2.UpdateUser = auditUserName;
                    appointmentLineItem2.CreateUser = auditUserName;

                    appointmentDal.Insert_AppointmentLineItem(appointmentLineItem2);

                    var appointmentLineItems =
                        appointmentDal.GetAppointmentLineItemsByCompletedAppointmentId(
                            appointmentCompleted.AppointmentCompletedId);


                    Assert.AreEqual(appointmentLineItems.Count, 2,
                        "Appointment Line Items  did not match the count to be expected returned.");

                }
                finally
                {
                    appointmentDal.Delete_AppointmentLineItem(appointmentLineItem2);
                    appointmentDal.Delete_AppointmentLineItem(appointmentLineItem);
                    appointmentDal.Delete_AppointmentCompleted(appointmentCompleted);
                    appointmentDal.DeleteAppointment(appointment);
                }
            }
        }

        /// <summary>
        /// Gets the single appointment line item.
        /// </summary>
        [TestMethod]
        public void GetSingleAppointmentLineItem()
        {
            using (db)
            {
                var appointmentDal = new SBAS_DAL.Appointment();
                var dateTime = DateTime.Now;
                dateTime.AddDays(8);
                var appointment = BuildAppointment(client1.UserId, customer.UserId, client1.AddressId,
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 0, 0, 0, DateTimeKind.Local),
                    "Test Appointment", "Test Appointment Description");
                var appointmentCompleted = new AppointmentCompleted();
                var appointmentLineItem = new AppointmentLineItem();
                try
                {
                    appointmentDal.AddAppointment(appointment);
                    appointmentCompleted = BuildAppointmentCompleted(appointment, true);
                    appointmentDal.Insert_AppointmentCompleted(appointmentCompleted);



                    appointmentLineItem = new AppointmentLineItem();
                    appointmentLineItem.AppointmentCompletedId = appointmentCompleted.AppointmentCompletedId;
                    appointmentLineItem.InventoryId = inventory.InventoryId;
                    appointmentLineItem.InventoryItemId = inventoryItem1.InventoryItemId;
                    appointmentLineItem.ItemPrice = inventoryItem1.ItemPrice;
                    appointmentLineItem.QuantityUsed = 5;
                    appointmentLineItem.UpdateDateTime = datetime;
                    appointmentLineItem.CreateDateTime = datetime;
                    appointmentLineItem.UpdateUser = auditUserName;
                    appointmentLineItem.CreateUser = auditUserName;

                    appointmentDal.Insert_AppointmentLineItem(appointmentLineItem);

                    var appointmentLineItem2 =
                        appointmentDal.GetAppointmentLineItem(appointmentLineItem.AppointmentLineItemId);


                    Assert.AreEqual(appointmentLineItem2.AppointmentLineItemId, appointmentLineItem.AppointmentLineItemId,
                        "Appointment Line Items  did not match each other.");

                }
                finally
                {
                    appointmentDal.Delete_AppointmentLineItem(appointmentLineItem);
                    appointmentDal.Delete_AppointmentCompleted(appointmentCompleted);
                    appointmentDal.DeleteAppointment(appointment);
                }
            }
        }

        #endregion

        #region Setup Methods

        /// <summary>
        /// Creates the customer addresses.
        /// </summary>
        private static void CreateCustomerAddresses()
        {
            //create customer Prince Charming Pool Cleaning Address
            customerAddress = new Address();
            customerAddress.AddressLine1 = "6163 Mohican Drive";
            customerAddress.AddressLine2 = null;
            customerAddress.CityId = 544;
            customerAddress.StateId = 6;
            customerAddress.ZipCode = "95336";
            customerAddress.Longitude = (decimal?)-121.209733;
            customerAddress.Latitude = (decimal?)37.831395;
            customerAddress.CreateUser = auditUserName;
            customerAddress.UpdateUser = auditUserName;
            customerAddress.CreateDateTime = datetime;
            customerAddress.UpdateDateTime = datetime;
            db.Insert("Address", "AddressId", true, customerAddress);
        }

        /// <summary>
        /// Creates the client addesses.
        /// </summary>
        private static void CreateClientAddesses()
        {
            //create client address's
            // Donald Duck Address
            clientAddress1 = new Address();
            clientAddress1.AddressLine1 = "1376 Keith Way";
            clientAddress1.AddressLine2 = null;
            clientAddress1.CityId = 544;
            clientAddress1.StateId = 6;
            clientAddress1.ZipCode = "95336";
            clientAddress1.Longitude = (decimal?)-121.239953;
            clientAddress1.Latitude = (decimal?)37.825167;
            clientAddress1.CreateUser = auditUserName;
            clientAddress1.UpdateUser = auditUserName;
            clientAddress1.CreateDateTime = datetime;
            clientAddress1.UpdateDateTime = datetime;
            db.Insert("Address", "AddressId", true, clientAddress1);

            // Mickey Mouse Address
            clientAddress2 = new Address();
            clientAddress2.AddressLine1 = "1911 Crom St";
            clientAddress2.AddressLine2 = null;
            clientAddress2.CityId = 544;
            clientAddress2.StateId = 6;
            clientAddress2.ZipCode = "95337";
            clientAddress2.Longitude = (decimal?)-121.248716;
            clientAddress2.Latitude = (decimal?)37.804856;
            clientAddress2.CreateUser = auditUserName;
            clientAddress2.UpdateUser = auditUserName;
            clientAddress2.CreateDateTime = datetime;
            clientAddress2.UpdateDateTime = datetime;
            db.Insert("Address", "AddressId", true, clientAddress2);

            // Scrooge McDuck Address
            clientAddress3 = new Address();
            clientAddress3.AddressLine1 = "2298 Vicki Way";
            clientAddress3.AddressLine2 = null;
            clientAddress3.CityId = 544;
            clientAddress3.StateId = 6;
            clientAddress3.ZipCode = "95337";
            clientAddress3.Longitude = (decimal?)-121.207783;
            clientAddress3.Latitude = (decimal?)37.768659;
            clientAddress3.CreateUser = auditUserName;
            clientAddress3.UpdateUser = auditUserName;
            clientAddress3.CreateDateTime = datetime;
            clientAddress3.UpdateDateTime = datetime;
            db.Insert("Address", "AddressId", true, clientAddress3);

            // Snow White Address
            clientAddress4 = new Address();
            clientAddress4.AddressLine1 = "1876 Davidson Ct";
            clientAddress4.AddressLine2 = null;
            clientAddress4.CityId = 642;
            clientAddress4.StateId = 6;
            clientAddress4.ZipCode = "95366";
            clientAddress4.Longitude = (decimal?)-121.149421;
            clientAddress4.Latitude = (decimal?)37.723743;
            clientAddress4.CreateUser = auditUserName;
            clientAddress4.UpdateUser = auditUserName;
            clientAddress4.CreateDateTime = datetime;
            clientAddress4.UpdateDateTime = datetime;
            db.Insert("Address", "AddressId", true, clientAddress4);


            // Jack Sparrow Address
            clientAddress5 = new Address();
            clientAddress5.AddressLine1 = "1636 Brett Ln";
            clientAddress5.AddressLine2 = null;
            clientAddress5.CityId = 561;
            clientAddress5.StateId = 6;
            clientAddress5.ZipCode = "95358";
            clientAddress5.Longitude = (decimal?)-121.032938;
            clientAddress5.Latitude = (decimal?)37.651699;
            clientAddress5.CreateUser = auditUserName;
            clientAddress5.UpdateUser = auditUserName;
            clientAddress5.CreateDateTime = datetime;
            clientAddress5.UpdateDateTime = datetime;

            db.Insert("Address", "AddressId", true, clientAddress5);
        }

        /// <summary>
        /// Creates the customers.
        /// </summary>
        private static void CreateCustomers()
        {
            customer = new SBASUser();
            customer.FirstName = "Prince";
            customer.LastName = "Charming";
            customer.CompanyName = "Prince Charming Pool Cleaning";
            customer.FaxNumber = "7245551212";
            customer.PhoneNumber = "7245551213";
            customer.MobileNumber = "7245551214";
            customer.AddressId = customerAddress.AddressId;
            customer.CreateUser = auditUserName;
            customer.UpdateUser = auditUserName;
            customer.CreateDateTime = datetime;
            customer.UpdateDateTime = datetime;
            db.Insert("SBASUser", "UserId", true, customer);
        }

        /// <summary>
        /// Creates the clients.
        /// </summary>
        private static void CreateClients()
        {
            // create client
            client1 = new SBASUser();
            client1.FirstName = "Donald";
            client1.LastName = "Duck";
            client1.CompanyName = null;
            client1.FaxNumber = "7245551212";
            client1.PhoneNumber = "7245551213";
            client1.MobileNumber = "7245551214";
            client1.AddressId = clientAddress1.AddressId;
            client1.CreateUser = auditUserName;
            client1.UpdateUser = auditUserName;
            client1.CreateDateTime = datetime;
            client1.UpdateDateTime = datetime;
            db.Insert("SBASUser", "UserId", true, client1);

            // create client
            client2 = new SBASUser();
            client2.FirstName = "Mickey";
            client2.LastName = "Mouse";
            client2.CompanyName = null;
            client2.FaxNumber = "7245551212";
            client2.PhoneNumber = "7245551213";
            client2.MobileNumber = "7245551214";
            client2.AddressId = clientAddress2.AddressId;
            client2.CreateUser = auditUserName;
            client2.UpdateUser = auditUserName;
            client2.CreateDateTime = datetime;
            client2.UpdateDateTime = datetime;
            db.Insert("SBASUser", "UserId", true, client2);

            // create client
            client3 = new SBASUser();
            client3.FirstName = "Scrooge";
            client3.LastName = "McDuck";
            client3.CompanyName = null;
            client3.FaxNumber = "7245551212";
            client3.PhoneNumber = "7245551213";
            client3.MobileNumber = "7245551214";
            client3.AddressId = clientAddress3.AddressId;
            client3.CreateUser = auditUserName;
            client3.UpdateUser = auditUserName;
            client3.CreateDateTime = datetime;
            client3.UpdateDateTime = datetime;
            db.Insert("SBASUser", "UserId", true, client3);

            // create client
            client4 = new SBASUser();
            client4.FirstName = "Snow";
            client4.LastName = "White";
            client4.CompanyName = null;
            client4.FaxNumber = "7245551212";
            client4.PhoneNumber = "7245551213";
            client4.MobileNumber = "7245551214";
            client4.AddressId = clientAddress4.AddressId;
            client4.CreateUser = auditUserName;
            client4.UpdateUser = auditUserName;
            client4.CreateDateTime = datetime;
            client4.UpdateDateTime = datetime;
            db.Insert("SBASUser", "UserId", true, client4);

            // create client
            client5 = new SBASUser();
            client5.FirstName = "Jack";
            client5.LastName = "Sparrow";
            client5.CompanyName = null;
            client5.FaxNumber = "7245551212";
            client5.PhoneNumber = "7245551213";
            client5.MobileNumber = "7245551214";
            client5.AddressId = clientAddress5.AddressId;
            client5.CreateUser = auditUserName;
            client5.UpdateUser = auditUserName;
            client5.CreateDateTime = datetime;
            client5.UpdateDateTime = datetime;
            db.Insert("SBASUser", "UserId", true, client5);

        }

        /// <summary>
        /// Creates the client list.
        /// </summary>
        private static void CreateClientList()
        {
            clientList1 = new ClientList();
            clientList1.ClientId = client1.UserId;
            clientList1.CustomerId = customer.UserId;
            db.Insert("ClientList", "ClientID,CustomerID", true, clientList1);

            clientList2 = new ClientList();
            clientList2.ClientId = client2.UserId;
            clientList2.CustomerId = customer.UserId;
            db.Insert("ClientList", "ClientID,CustomerID", true, clientList2);

            clientList3 = new ClientList();
            clientList3.ClientId = client3.UserId;
            clientList3.CustomerId = customer.UserId;
            db.Insert("ClientList", "ClientID,CustomerID", true, clientList3);

            clientList4 = new ClientList();
            clientList4.ClientId = client4.UserId;
            clientList4.CustomerId = customer.UserId;
            db.Insert("ClientList", "ClientID,CustomerID", true, clientList4);

            clientList5 = new ClientList();
            clientList5.ClientId = client5.UserId;
            clientList5.CustomerId = customer.UserId;
            db.Insert("ClientList", "ClientID,CustomerID", true, clientList5);



        }

        /// <summary>
        /// Creates the service types.
        /// </summary>
        private static void CreateServiceTypes()
        {
            serviceType1 = new ServiceType();
            serviceType1.CustomerId = customer.UserId;
            serviceType1.Description = "Weekly Pool Service";
            serviceType1.NameOfService = "Weekly Pool Service";
            serviceType1.UpdateDateTime = datetime;
            serviceType1.CreateDateTime = datetime;
            serviceType1.UpdateUser = auditUserName;
            serviceType1.CreateUser = auditUserName;

            db.Insert("ServiceType", "ServiceTypeId", true, serviceType1);

            serviceType2 = new ServiceType();
            serviceType2.CustomerId = customer.UserId;
            serviceType2.Description = "Weekly Pool Service";
            serviceType2.NameOfService = "Weekly Pool Service";
            serviceType2.UpdateDateTime = datetime;
            serviceType2.CreateDateTime = datetime;
            serviceType2.UpdateUser = auditUserName;
            serviceType2.CreateUser = auditUserName;

            db.Insert("ServiceType", "ServiceTypeId", true, serviceType2);

            serviceType3 = new ServiceType();
            serviceType3.CustomerId = customer.UserId;
            serviceType3.Description = "Weekly Pool Service";
            serviceType3.NameOfService = "Weekly Pool Service";
            serviceType3.UpdateDateTime = datetime;
            serviceType3.CreateDateTime = datetime;
            serviceType3.UpdateUser = auditUserName;
            serviceType3.CreateUser = auditUserName;

            db.Insert("ServiceType", "ServiceTypeId", true, serviceType3);

        }

        /// <summary>
        /// Creates the inventory.
        /// </summary>
        private static void CreateInventory()
        {
            inventory = new Inventory();
            inventory.CustomerId = customer.UserId;
            inventory.UpdateDateTime = datetime;
            inventory.CreateDateTime = datetime;
            inventory.UpdateUser = auditUserName;
            inventory.CreateUser = auditUserName;
            
            db.Insert("Inventory", "InventoryId", true, inventory);

            inventoryItem1 = new InventoryItem();
            inventoryItem1.HasPhysicalInventory = true;
            inventoryItem1.InventoryId = inventory.InventoryId;
            inventoryItem1.ItemDescription = "Inventory Item 1";
            inventoryItem1.ItemName = "";
            inventoryItem1.ItemPrice = (decimal) 50.00;
            inventoryItem1.QuantityOnHand = 50;
            inventoryItem1.UpdateDateTime = datetime;
            inventoryItem1.CreateDateTime = datetime;
            inventoryItem1.UpdateUser = auditUserName;
            inventoryItem1.CreateUser = auditUserName;

            db.Insert("InventoryItem", "InventoryItemId", true, inventoryItem1);

            inventoryItem2 = new InventoryItem();
            inventoryItem2.HasPhysicalInventory = true;
            inventoryItem2.InventoryId = inventory.InventoryId;
            inventoryItem2.ItemDescription = "Inventory Item 2";
            inventoryItem2.ItemName = "";
            inventoryItem2.ItemPrice = (decimal)50.00;
            inventoryItem2.QuantityOnHand = 50;
            inventoryItem2.UpdateDateTime = datetime;
            inventoryItem2.CreateDateTime = datetime;
            inventoryItem2.UpdateUser = auditUserName;
            inventoryItem2.CreateUser = auditUserName;

            db.Insert("InventoryItem", "InventoryItemId", true, inventoryItem2);

            inventoryItem3 = new InventoryItem();
            inventoryItem3.HasPhysicalInventory = true;
            inventoryItem3.InventoryId = inventory.InventoryId;
            inventoryItem3.ItemDescription = "Inventory Item 3";
            inventoryItem3.ItemName = "";
            inventoryItem3.ItemPrice = (decimal)50.00;
            inventoryItem3.QuantityOnHand = 50;
            inventoryItem3.UpdateDateTime = datetime;
            inventoryItem3.CreateDateTime = datetime;
            inventoryItem3.UpdateUser = auditUserName;
            inventoryItem3.CreateUser = auditUserName;

            db.Insert("InventoryItem", "InventoryItemId", true, inventoryItem3);
        }



        /// <summary>
        /// Builds the appointment.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="addressId">The address identifier.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <returns>Appointment.</returns>
        private static Appointment BuildAppointment(long clientId, long customerId, long addressId, DateTime start, DateTime end, string title, string description)
        {
            var appointment = new Appointment();
            appointment.AddressId = addressId;
            appointment.ClientId = clientId;
            appointment.CustomerId = customerId;
            appointment.Title = title;
            appointment.Description = description;
            appointment.IsAllDay = false;
            appointment.Start = start;
            appointment.End = end;
            appointment.RecurrenceRule = null;
            appointment.RecurrenceException = null;
            appointment.RecurrenceId = null;
            appointment.ServiceTypeId = serviceType1.ServiceTypeId;
            appointment.CreateUser = auditUserName;
            appointment.UpdateUser = auditUserName;
            appointment.CreateDateTime = datetime;
            appointment.UpdateDateTime = datetime;
            
            return appointment;
        }

        /// <summary>
        /// Builds the appointment completed.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <param name="isCompleted">if set to <c>true</c> [is completed].</param>
        /// <returns>AppointmentCompleted.</returns>
        private static AppointmentCompleted BuildAppointmentCompleted(Appointment appointment, bool isCompleted)
        {
            var appointmentCompleted = new AppointmentCompleted();
            appointmentCompleted.AppointmentId = appointment.AppointmentId;
            appointmentCompleted.ClientId = appointment.ClientId;
            appointmentCompleted.CustomerId = appointment.CustomerId;
            appointmentCompleted.CompletionDateTime = appointment.End;
            appointmentCompleted.IsCompleted = isCompleted;
            appointmentCompleted.IsReadyForInvoicing = false;
            appointmentCompleted.IsInvoiced = false;
            appointmentCompleted.CreateDateTime = datetime;
            appointmentCompleted.UpdateDateTime = datetime;
            appointmentCompleted.CreateUser = auditUserName;
            appointmentCompleted.UpdateUser = auditUserName;

            return appointmentCompleted;
        }
        #endregion

    }
}

