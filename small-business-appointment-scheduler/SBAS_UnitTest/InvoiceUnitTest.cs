// ***********************************************************************
// Assembly         : SBAS_UnitTest
// Author           : Ye Peng
// Created          : 06-05-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="InvoiceUnitTest.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>Contains all of the unit tests for the Invoice class.</summary>
// ***********************************************************************
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPoco;
using SBAS_Core.Model;
using SBASUser = SBAS_DAL.SBASUser;

/// <summary>
/// The SBAS_UnitTest namespace.
/// </summary>
namespace SBAS_UnitTest
{
    /// <summary>
    /// Contains all of the unit tests for the Invoice class.
    /// </summary>
    [TestClass]
    public class InvoiceUnitTest
    {

        /// <summary>
        /// The customer address
        /// </summary>
        private static Address customerAddress;
        
        /// <summary>
        /// The customer
        /// </summary>
        private static SBAS_Core.Model.SBASUser customer;
        
        /// <summary>
        /// The client address
        /// </summary>
        private static Address clientAddress;
        
        /// <summary>
        /// The client
        /// </summary>
        private static SBAS_Core.Model.SBASUser client;
        
        /// <summary>
        /// The database
        /// </summary>
        private static Database db;
        
        /// <summary>
        /// The test invoice
        /// </summary>
        private static Invoice testInvoice;
        
        /// <summary>
        /// The test invoice line item
        /// </summary>
        private static InvoiceLineItem testInvoiceLineItem;
        
        /// <summary>
        /// The test inventory
        /// </summary>
        private static Inventory testInventory;
        
        /// <summary>
        /// The test inventory item
        /// </summary>
        private static InventoryItem testInventoryItem;
        
        /// <summary>
        /// The test appointment
        /// </summary>
        private static Appointment testAppointment;
        
        /// <summary>
        /// The test appointment completed
        /// </summary>
        private static AppointmentCompleted testAppointmentCompleted;
        
        /// <summary>
        /// The test appointment line item
        /// </summary>
        private static AppointmentLineItem testAppointmentLineItem;
        
        /// <summary>
        /// The test service type
        /// </summary>
        private static ServiceType testServiceType;

        // The following attributes correspond to the CreateUser, UpdateUser 
        // CreateDateTime, and UpdateDateTime fields in the database
        
        /// <summary>
        /// The create user field
        /// </summary>
        private static string createUserField = "Bennington";
        
        /// <summary>
        /// The update user field
        /// </summary>
        private static string updateUserField = "Bennington";
        
        /// <summary>
        /// The current date time
        /// </summary>
        private static DateTime currentDateTime = DateTime.Now;

        #region Additional test attributes
        /// <summary>
        /// Initializes the InvoiceUnitTest class. Adds test data to the database for invoice unit testing.
        /// </summary>
        /// <param name="testContext">The test context.</param>
        [ClassInitialize()]
        public static void InvoiceUnitTest_ClassInitialize(TestContext testContext)
        {
            db = new Database(SBAS_DAL.Base.GetConnectionString, DatabaseType.SqlServer2012);
            using (db)
            {
                // Create customer address
                customerAddress = new Address
                {
                    AddressLine1 = "123 Main St",
                    AddressLine2 = "Suite 101",
                    CityId = 6323,
                    StateId = 40,
                    ZipCode = "98765",
                    Longitude = null,
                    Latitude = null,
                    CreateUser = createUserField,
                    CreateDateTime = currentDateTime,
                    UpdateUser = updateUserField,
                    UpdateDateTime = currentDateTime, 
                };

                db.Insert("Address", "AddressId", true, customerAddress);

                // Create client address
                clientAddress = new Address
                {
                    AddressLine1 = "123 Park Avenue",
                    AddressLine2 = null,
                    CityId = 6323,
                    StateId = 40,
                    ZipCode = "98765",
                    Longitude = null,
                    Latitude = null,
                    CreateUser = createUserField,
                    CreateDateTime = currentDateTime,
                    UpdateUser = updateUserField,
                    UpdateDateTime = currentDateTime, 
                };

                db.Insert("Address", "AddressId", true, clientAddress);

                // Create customer
                customer = new SBAS_Core.Model.SBASUser
                {
                    FirstName = "Joe",
                    LastName = "Bob",
                    CompanyName = "Joe Bobs Pool Service",
                    FaxNumber = "724-555-1212",
                    AddressId = customerAddress.AddressId,
                    CreateUser = createUserField,
                    CreateDateTime = currentDateTime,
                    UpdateUser = updateUserField,
                    UpdateDateTime = currentDateTime, 
                };

                db.Insert("SBASUser", "UserId", true, customer);

                // Create client
                client = new SBAS_Core.Model.SBASUser
                {
                    FirstName = "Roy",
                    LastName = "Franks",
                    CompanyName = null,
                    FaxNumber = "724-327-4343",
                    AddressId = clientAddress.AddressId,
                    CreateUser = createUserField,
                    CreateDateTime = currentDateTime,
                    UpdateUser = updateUserField,
                    UpdateDateTime = currentDateTime, 
                };

                db.Insert("SBASUser", "UserId", true, client);

                // Create invoice
                testInvoice = new Invoice
                {
                    // Do not need InvoiceID - SQL Server will automatically populate
                    InvoiceNumber = "10010",
                    AmountDue = (decimal) 100.00,
                    DueDate = DateTime.Now.AddDays(20),
                    Comments = "This is a test.",
                    CustomerId = customer.UserId, 
                    ClientID = client.UserId,
                    SentToClient = false,
                    IsPaid = false,
                    CreateUser = createUserField,
                    CreateDateTime = currentDateTime,
                    UpdateUser = updateUserField,
                    UpdateDateTime = currentDateTime, 
                };

                db.Insert("Invoice", "InvoiceId", true, testInvoice);

                // Create inventory
                testInventory = new Inventory
                {
                    // Do not need InventoryID - SQL Server will automatically populate
                    CustomerId = customer.UserId,
                    LastRestockedDate = DateTime.Now.AddDays(-10),
                    LastInventoryInspection = DateTime.Now.AddDays(-5),
                    CreateUser = createUserField,
                    CreateDateTime = currentDateTime,
                    UpdateUser = updateUserField,
                    UpdateDateTime = currentDateTime, 
                };

                db.Insert("Inventory", "InventoryID", true, testInventory);

                // Create inventory item
                testInventoryItem = new InventoryItem
                {
                    // Do not need InventoryItemID - SQL Server will automatically populate
                    InventoryId = testInventory.InventoryId,
                    ItemName = "Landscape fabric",
                    ItemDescription = "Used to prevent weeds",
                    ItemPrice = (decimal)25.00,
                    QuantityOnHand = 6,
                    HasPhysicalInventory = true,
                    CreateUser = createUserField,
                    CreateDateTime = currentDateTime,
                    UpdateUser = updateUserField,
                    UpdateDateTime = currentDateTime, 
                };

                db.Insert("InventoryItem", "InventoryItemID", true, testInventoryItem);

                // Create invoice line item
                testInvoiceLineItem = new InvoiceLineItem
                {
                    // Do not need InvoiceLineItemID - SQL Server will automatically populate
                    InvoiceId = testInvoice.InvoiceId,
                    ItemId = testInventoryItem.InventoryItemId,
                    CreateUser = createUserField,
                    CreateDateTime = currentDateTime,
                    UpdateUser = updateUserField,
                    UpdateDateTime = currentDateTime, 
                };

                db.Insert("InvoiceLineItem", "InvoiceLineItemID", true, testInvoiceLineItem);

                // Create Service Type
                testServiceType = new ServiceType
                {
                    // Do not need ServiceTypeID - SQL Server will automatically populate
                    CustomerId = customer.UserId,
                    NameOfService = "Test Service",
                    Description = "Test Service",
                    CreateUser = createUserField,
                    CreateDateTime = currentDateTime,
                    UpdateUser = updateUserField,
                    UpdateDateTime = currentDateTime, 
                };

                db.Insert("ServiceType", "ServiceTypeID", true, testServiceType);

                // Create Appointment
                testAppointment = new Appointment
                {
                    // Do not need AppointmentId - SQL Server will automatically populate
                    Title = "Bennington Appt",
                    Description = "Test appointment",
                    Start = DateTime.Now.AddDays(-2),
                    End = DateTime.Now.AddDays(-2),
                    IsAllDay = false,
                    CustomerId = customer.UserId,
                    ClientId = client.UserId,
                    UseClientAddress = true,
                    AddressId = customerAddress.AddressId,
                    ServiceTypeId = testServiceType.ServiceTypeId,
                    CreateUser = createUserField,
                    CreateDateTime = currentDateTime,
                    UpdateUser = updateUserField,
                    UpdateDateTime = currentDateTime,
                };
                
                db.Insert("Appointment", "AppointmentId", true, testAppointment);

                // Create AppointmentCompleted
                testAppointmentCompleted = new AppointmentCompleted
                {
                    // Do not need AppointmentCompletedId - SQL Server will automatically populate
                    AppointmentId = testAppointment.AppointmentId,
                    CompletionDateTime = DateTime.Now.AddDays(-2),
                    IsCompleted = true,
                    IsReadyForInvoicing = true,
                    IsInvoiced = false,
                    CustomerId = customer.UserId,
                    ClientId = client.UserId,
                    CreateUser = createUserField,
                    CreateDateTime = currentDateTime,
                    UpdateUser = updateUserField,
                    UpdateDateTime = currentDateTime,
                };

                db.Insert("AppointmentCompleted", "AppointmentCompletedId", true, testAppointmentCompleted);

            } // End method ClassInitialize
        }

        /// <summary>
        /// Cleans up the database from the Invoice unit testing.
        /// </summary>
        [ClassCleanup()]
        public static void InvoiceUnitTest_ClassCleanup()
        {
            using (db)
            {
                db.Delete("AppointmentCompleted", "AppointmentCompletedId", testAppointmentCompleted, testAppointmentCompleted.AppointmentCompletedId);
                db.Delete("Appointment", "AppointmentID", testAppointment, testAppointment.AppointmentId);
                db.Delete("ServiceType", "ServiceTypeID", testServiceType, testServiceType.ServiceTypeId);
                db.Delete("InvoiceLineItem", "InvoiceLineItemID", testInvoiceLineItem, testInvoiceLineItem.InvoiceLineItemId);
                db.Delete("InventoryItem", "InventoryItemID", testInventoryItem, testInventoryItem.InventoryItemId);
                db.Delete("Inventory", "InventoryID", testInventory, testInventory.InventoryId);
                db.Delete("Invoice", "InvoiceId", testInvoice, testInvoice.InvoiceId);
                db.Delete("SBASUser", "UserId", customer, customer.UserId);
                db.Delete("SBASUser", "UserId", client, client.UserId);
                db.Delete("Address", "AddressId", customerAddress, customerAddress.AddressId);
                db.Delete("Address", "AddressId", clientAddress, clientAddress.AddressId);
            }
        }

        #endregion

        /// <summary>
        /// Tests the AddInvoice and DeleteInvoice methods in the Invoice class.
        /// </summary>
        [TestMethod]
        public void CreateandDeleteInvoice()
        {
            try
            {
                Invoice  invoice = new Invoice();
                invoice.AmountDue = (decimal) 100.00;
                invoice.ClientID = client.UserId;
                invoice.CustomerId = customer.UserId;
                invoice.Comments = "This had no extra parts";
                invoice.InvoiceNumber = "DF455464";
                invoice.SentToClient = false;
                invoice.DueDate = DateTime.Now.AddDays(20);
                invoice.CreateUser = createUserField;
                invoice.CreateDateTime = currentDateTime;
                invoice.UpdateUser = updateUserField;
                invoice.UpdateDateTime = currentDateTime; 

                Invoice newInvoice = new SBAS_DAL.Invoice().AddInvoice(invoice);
                Assert.AreNotEqual(0,invoice.InvoiceId,"Invoice was not created!");
                
                if (newInvoice != null && newInvoice.InvoiceId > 0)
                {
                    bool deleteResult = new SBAS_DAL.Invoice().DeleteInvoice(newInvoice);
                    Assert.AreEqual(true, deleteResult, "Could not delete Invoice!");
                }
                else
                {
                    Assert.AreNotEqual(newInvoice, null, "Invoice does not exist!");
                }                
            }           
        catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the AddInvoice, AddInvoiceLineItem, Delete Invoice, and DeleteInvoiceLineItem methods in the Invoice class. The InvoiceLineItem added in this test does not have a price override.
        /// </summary>
        [TestMethod]
        public void CreateInvoiceAndAddOneLineItemNoOverride()
        {
            try
            {
                Invoice invoice = new Invoice();
                invoice.AmountDue = (decimal)100.00;
                invoice.ClientID = client.UserId;
                invoice.CustomerId = customer.UserId;
                invoice.Comments = "There is one additional line item on this invoice - no price override.";
                invoice.InvoiceNumber = "DF455465";
                invoice.SentToClient = false;
                invoice.DueDate = DateTime.Now.AddDays(20);
                invoice.CreateUser = createUserField;
                invoice.CreateDateTime = currentDateTime;
                invoice.UpdateUser = updateUserField;
                invoice.UpdateDateTime = currentDateTime; 

                Invoice newInvoice = new SBAS_DAL.Invoice().AddInvoice(invoice);
                Assert.AreNotEqual(0,invoice.InvoiceId,"Invoice was not created!");
               
                InvoiceLineItem invoiceLineItem = new InvoiceLineItem();
                invoiceLineItem.InvoiceId = newInvoice.InvoiceId;
                invoiceLineItem.ItemId = testInventoryItem.InventoryItemId;
                invoiceLineItem.CreateUser = createUserField;
                invoiceLineItem.CreateDateTime = currentDateTime;
                invoiceLineItem.UpdateUser = updateUserField;
                invoiceLineItem.UpdateDateTime = currentDateTime;

                InvoiceLineItem newLineItem = new SBAS_DAL.Invoice().AddInvoiceLineItem(invoiceLineItem);
                Assert.AreNotEqual(0,newLineItem.InvoiceLineItemId,"Invoice Line Item was not created!");
                
                if (newInvoice != null && newInvoice.InvoiceId > 0)
                {
                    bool deleteItem = new SBAS_DAL.Invoice().DeleteInvoiceLineItem(newLineItem);
                    Assert.AreEqual(true, deleteItem, "Could not delete Invoice Line Item!");
                  
                    bool deleteResult = new SBAS_DAL.Invoice().DeleteInvoice(newInvoice);
                    Assert.AreEqual(true, deleteResult, "Could not delete Invoice!");
                }
                else
                {
                    Assert.AreNotEqual(newInvoice, null, "Invoice does not exist!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the AddInvoice, AddInvoiceLineItem, Delete Invoice, and DeleteInvoiceLineItem methods in the Invoice class. The InvoiceLineItem added in this test has a price override.
        /// </summary>
        [TestMethod]
        public void CreateInvoiceAndAddOneLineItemWithOverride()
        {
            try
            {
                Invoice invoice = new Invoice();
                invoice.AmountDue = (decimal)100.00;
                invoice.ClientID = client.UserId;
                invoice.CustomerId = customer.UserId;
                invoice.Comments = "There is one additional line item on this invoice with price override.";
                invoice.InvoiceNumber = "DF455466";
                invoice.SentToClient = false;
                invoice.DueDate = DateTime.Now.AddDays(20);
                invoice.CreateUser = createUserField;
                invoice.CreateDateTime = currentDateTime;
                invoice.UpdateUser = updateUserField;
                invoice.UpdateDateTime = currentDateTime; 

                Invoice newInvoice = new SBAS_DAL.Invoice().AddInvoice(invoice);
                Assert.AreNotEqual(0, invoice.InvoiceId, "Invoice was not created!");

                InvoiceLineItem invoiceLineItem = new InvoiceLineItem();
                invoiceLineItem.InvoiceId = newInvoice.InvoiceId;
                invoiceLineItem.ItemId = testInventoryItem.InventoryItemId;
                invoiceLineItem.ItemLineCostOverride = (decimal)97.50;
                invoiceLineItem.CreateUser = createUserField;
                invoiceLineItem.CreateDateTime = currentDateTime;
                invoiceLineItem.UpdateUser = updateUserField;
                invoiceLineItem.UpdateDateTime = currentDateTime;

                InvoiceLineItem newLineItem = new SBAS_DAL.Invoice().AddInvoiceLineItem(invoiceLineItem);
                Assert.AreNotEqual(0, newLineItem.InvoiceLineItemId, "Invoice Line Item was not created!");

                if (newInvoice != null && newInvoice.InvoiceId > 0)
                {
                    bool deleteItem = new SBAS_DAL.Invoice().DeleteInvoiceLineItem(newLineItem);
                    Assert.AreEqual(true, deleteItem, "Could not delete Invoice Line Item!");

                    bool deleteResult = new SBAS_DAL.Invoice().DeleteInvoice(newInvoice);
                    Assert.AreEqual(true, deleteResult, "Could not delete Invoice!");
                }
                else
                {
                    Assert.AreNotEqual(newInvoice, null, "Invoice does not exist!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the AddInvoice, AddInvoiceLineItem, Delete Invoice, and DeleteInvoiceLineItem methods in the Invoice class. Two InvoiceLineItems are added in this test.
        /// </summary>
        [TestMethod]
        public void CreateInvoiceAddTwoItems()
        {
            try
            {
                Invoice invoice = new Invoice();
                invoice.AmountDue = (decimal)100.00;
                invoice.ClientID = client.UserId;
                invoice.CustomerId = customer.UserId;
                invoice.Comments = "There are two additional line items on this invoice.";
                invoice.InvoiceNumber = "DF455467";
                invoice.SentToClient = false;
                invoice.DueDate = DateTime.Now.AddDays(20);
                invoice.CreateUser = createUserField;
                invoice.CreateDateTime = currentDateTime;
                invoice.UpdateUser = updateUserField;
                invoice.UpdateDateTime = currentDateTime; 

                Invoice newInvoice = new SBAS_DAL.Invoice().AddInvoice(invoice);
                Assert.AreNotEqual(0, invoice.InvoiceId, "Invoice was not created!");

                InvoiceLineItem invoiceLineItem = new InvoiceLineItem();
                invoiceLineItem.InvoiceId = newInvoice.InvoiceId;
                invoiceLineItem.ItemId = testInventoryItem.InventoryItemId;
                invoiceLineItem.CreateUser = createUserField;
                invoiceLineItem.CreateDateTime = currentDateTime;
                invoiceLineItem.UpdateUser = updateUserField;
                invoiceLineItem.UpdateDateTime = currentDateTime;

                InvoiceLineItem newLineItem = new SBAS_DAL.Invoice().AddInvoiceLineItem(invoiceLineItem);
                Assert.AreNotEqual(0, newLineItem.InvoiceLineItemId, "Invoice Line Item #1 was not created!");
               
                InvoiceLineItem invoiceLineItem2 = new InvoiceLineItem();
                invoiceLineItem2.InvoiceId = newInvoice.InvoiceId;
                invoiceLineItem2.ItemId = testInventoryItem.InventoryItemId;
                invoiceLineItem2.ItemLineCostOverride = (decimal)54.60;
                invoiceLineItem2.CreateUser = createUserField;
                invoiceLineItem2.CreateDateTime = currentDateTime;
                invoiceLineItem2.UpdateUser = updateUserField;
                invoiceLineItem2.UpdateDateTime = currentDateTime;
                
                InvoiceLineItem newLineItem2 = new SBAS_DAL.Invoice().AddInvoiceLineItem(invoiceLineItem2);
                Assert.AreNotEqual(0, newLineItem2.InvoiceLineItemId, "Invoice Line Item #2 was not created!");

                if (newInvoice != null && newInvoice.InvoiceId > 0)
                {
                    bool deleteItem = new SBAS_DAL.Invoice().DeleteInvoiceLineItem(newLineItem2);
                    Assert.AreEqual(true, deleteItem, "Could not delete Invoice Line Item #2!");
                    
                    bool deleteItem2 = new SBAS_DAL.Invoice().DeleteInvoiceLineItem(newLineItem);
                    Assert.AreEqual(true, deleteItem, "Could not delete Invoice Line Item #1!");
                    
                    bool deleteResult = new SBAS_DAL.Invoice().DeleteInvoice(newInvoice);
                    Assert.AreEqual(true, deleteItem, "Could not delete Invoice!");
                }
                else
                {
                    Assert.AreNotEqual(newInvoice, null, "Invoice does not exist!");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the AddInvoice, AddInvoiceLineItem, Delete Invoice, and DeleteInvoiceLineItem methods in the Invoice class. An override of the invoicelineitemId for one InvoiceLineItem forces an error to test error handling in case of a non-existent line item.
        /// </summary>
        [TestMethod]
        public void CreateInvoiceAddTwoItemsOneNonExistentItem()
        {
            try
            {
                Invoice invoice = new Invoice();
                invoice.AmountDue = (decimal)100.00;
                invoice.ClientID = client.UserId;
                invoice.CustomerId = customer.UserId;
                invoice.Comments = "There are two additional line items on this invoice.";
                invoice.InvoiceNumber = "DF455468";
                invoice.SentToClient = false;
                invoice.DueDate = DateTime.Now.AddDays(20);
                invoice.CreateUser = createUserField;
                invoice.CreateDateTime = currentDateTime;
                invoice.UpdateUser = updateUserField;
                invoice.UpdateDateTime = currentDateTime;

                Invoice newInvoice = new SBAS_DAL.Invoice().AddInvoice(invoice);
                Assert.AreNotEqual(0, invoice.InvoiceId, "Invoice was not created!");
               
                InvoiceLineItem invoiceLineItem = new InvoiceLineItem();
                invoiceLineItem.InvoiceId = newInvoice.InvoiceId;
                invoiceLineItem.ItemId = testInventoryItem.InventoryItemId;
                invoiceLineItem.CreateUser = createUserField;
                invoiceLineItem.CreateDateTime = currentDateTime;
                invoiceLineItem.UpdateUser = updateUserField;
                invoiceLineItem.UpdateDateTime = currentDateTime;

                InvoiceLineItem newLineItem = new SBAS_DAL.Invoice().AddInvoiceLineItem(invoiceLineItem);
                Assert.AreNotEqual(0, newLineItem.InvoiceLineItemId, "Invoice Line Item #1 was not created!");
               
                InvoiceLineItem invoiceLineItem2 = new InvoiceLineItem();
                invoiceLineItem2.InvoiceId = newInvoice.InvoiceId;
                invoiceLineItem2.ItemId = testInventoryItem.InventoryItemId;
                invoiceLineItem2.ItemLineCostOverride = (decimal)54.60;
                invoiceLineItem2.CreateUser = createUserField;
                invoiceLineItem2.CreateDateTime = currentDateTime;
                invoiceLineItem2.UpdateUser = updateUserField;
                invoiceLineItem2.UpdateDateTime = currentDateTime;

                InvoiceLineItem newLineItem2 = new SBAS_DAL.Invoice().AddInvoiceLineItem(invoiceLineItem2);
                Assert.AreNotEqual(0, newLineItem2.InvoiceLineItemId, "Invoice Line Item #2 was not created!");
                
                // Move original Invoice Line Item Id to hold variable.
                long holdInvoiceLineItemID = invoiceLineItem2.InvoiceLineItemId;

                // Override Invoice Line Item Id to force error.
                newLineItem2.InvoiceLineItemId = 0;
                Assert.AreEqual(0, newLineItem2.InvoiceLineItemId, "Forced error did not work!");

                if (newInvoice != null && newInvoice.InvoiceId > 0)
                {
                    // Reinstate original Invoice Line Item Id to allow for database cleanup.
                    newLineItem2.InvoiceLineItemId = holdInvoiceLineItemID;
                    bool deleteItem = new SBAS_DAL.Invoice().DeleteInvoiceLineItem(newLineItem2);
                    Assert.AreEqual(true, deleteItem, "Could not delete Invoice Line Item #2!");

                    bool deleteItem2 = new SBAS_DAL.Invoice().DeleteInvoiceLineItem(newLineItem);
                    Assert.AreEqual(true, deleteItem, "Could not delete Invoice Line Item #1!");

                    bool deleteResult = new SBAS_DAL.Invoice().DeleteInvoice(newInvoice);
                    Assert.AreEqual(true, deleteItem, "Could not delete Invoice!");
                }
                else
                {
                    Assert.AreNotEqual(newInvoice, null, "Invoice does not exist!");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetInvoiceByInvoiceNumber method in the Invoice class for an existing invoice.
        /// </summary>
        [TestMethod]
        public void FindInvoiceThatExistsByInvoiceNumber()
        {
            string invoiceNumber = "10010";

            try
            {
                Invoice newInvoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceNumber(invoiceNumber);
                Assert.AreNotEqual(0, newInvoice.InvoiceId, "Invoice was not found!");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetInvoiceByInvoiceNumber method in the Invoice class for a non-existent invoice.
        /// </summary>
        [TestMethod]
        public void HandleNonExistentInvoiceByInvoiceNumber()
        {
            string invoiceNumber = "10011";

            try
            {
                Invoice newInvoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceNumber(invoiceNumber);
                Assert.AreEqual(0, newInvoice.InvoiceId, "Invoice was found!");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetInvoiceByInvoiceId method in the Invoice class for an existing invoice.
        /// </summary>
        [TestMethod]
        public void FindInvoiceThatExistsByInvoiceID()
        {
            long invoiceId = testInvoice.InvoiceId;

            try
            {
                Invoice newInvoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(invoiceId);
                Assert.AreNotEqual(0, newInvoice.InvoiceId, "Invoice was not found!");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetInvoiceByInvoiceId method in the Invoice class for a non-existent invoice.
        /// </summary>
        [TestMethod]
        public void HandleNonExistentInvoiceByInvoiceId()
        {
            long invoiceId = 999;

            try
            {
                Invoice newInvoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(invoiceId);
                Assert.AreEqual(0, newInvoice.InvoiceId, "Invoice was found!");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetAllInvoicesForCustomer method in the Invoice class for an existing customer.
        /// </summary>
        [TestMethod]
        public void testGetAllInvoicesForExistingCustomer()
        {
            try
            {
                SBAS_DAL.Invoice invoice = new SBAS_DAL.Invoice();

                List<SBAS_Core.Model.Invoice> allinvoices = invoice.GetAllInvoicesForCustomer(customer.UserId);
                Assert.AreNotEqual(allinvoices.Count, 0, "No invoices found!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetAllInvoicesForCustomer method in the Invoice class for a non-existent customer.
        /// </summary>
        [TestMethod]
        public void testGetAllInvoicesForNonExistentCustomer()
        {
            long customerID = 999;

            try
            {                
                SBAS_DAL.Invoice invoice = new SBAS_DAL.Invoice();

                List<SBAS_Core.Model.Invoice> allinvoices = invoice.GetAllInvoicesForCustomer(customerID);
                Assert.AreEqual(allinvoices.Count, 0, "Invoices found!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetAllInvoicesForClient method in the Invoice class for an existing client.
        /// </summary>
        [TestMethod]
        public void testGetAllInvoicesForExistingClient()
        {
            try
            {
                SBAS_DAL.Invoice invoice = new SBAS_DAL.Invoice();

                List<SBAS_Core.Model.Invoice> allinvoices = invoice.GetAllInvoicesForClient(client.UserId);
                Assert.AreNotEqual(allinvoices.Count, 0, "No invoices found!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetAllInvoicesForClient method in the Invoice class for a non-existent client.
        /// </summary>
        [TestMethod]
        public void testGetAllInvoicesForNonExistentClient()
        {
            long customerID = 999;
            try
            {
                SBAS_DAL.Invoice invoice = new SBAS_DAL.Invoice();

                List<SBAS_Core.Model.Invoice> allinvoices = invoice.GetAllInvoicesForClient(customerID);
                Assert.AreEqual(allinvoices.Count, 0, "Invoices found!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the UpdateInvoice method in the Invoice class.
        /// </summary>
        [TestMethod]
        public void UpdateInvoice()
        {
            long invoiceId = testInvoice.InvoiceId;

            try
            {
                Invoice newInvoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(invoiceId);
                Assert.AreNotEqual(0, newInvoice.InvoiceId, "Invoice was not created!");

                newInvoice.SentToClient = true;
                newInvoice.UpdateUser = "Bennington Update";
                newInvoice.UpdateDateTime = DateTime.Now;

                Invoice newInvoiceUpdate = new SBAS_DAL.Invoice().UpdateInvoice(newInvoice);
                Assert.AreEqual(newInvoiceUpdate.SentToClient, true, "Invoice not updated!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the UpdateInvoiceLineItem method in the Invoice class.
        /// </summary>
        [TestMethod]
        public void UpdateInvoiceLineItem()
        {
            long invoiceId = testInvoice.InvoiceId;

            try
            {
                Invoice newInvoice = new SBAS_DAL.Invoice().GetInvoiceByInvoiceId(invoiceId);
                Assert.AreNotEqual(0, newInvoice.InvoiceId, "Invoice was not created!");

                InvoiceLineItem newLineItem = testInvoiceLineItem;
                Assert.AreNotEqual(0, newLineItem.InvoiceLineItemId, "Invoice Line Item was not created!");

                newLineItem.ItemLineCostOverride = (decimal)81.40;
                newLineItem.UpdateUser = "Bennington Update";
                newLineItem.UpdateDateTime = DateTime.Now;

                InvoiceLineItem newLineItemUpdate = new SBAS_DAL.Invoice().UpdateInvoiceLineItem(newLineItem);
                Assert.AreEqual(newLineItemUpdate.ItemLineCostOverride, (decimal)81.40, "Invoice Line Item was not updated!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetAllInvoicesForClientByEmail method in the Invoice class.
        /// </summary>
        [TestMethod]
        public void testGetAllInvoicesForClientByEmail()
        {
            try
            {
                string testEmail = "abc@abclandscaping.com";

                SBAS_DAL.Invoice invoice = new SBAS_DAL.Invoice();

                List<SBAS_Core.Model.Invoice> allinvoices = invoice.GetAllInvoicesForClientByEmail(testEmail);
                Assert.AreNotEqual(allinvoices.Count, 0, "No invoices found!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetAllInvoicesForCustomerByIfPaid method in the Invoice class for unpaid invoices.
        /// </summary>
        [TestMethod]
        public void GetAllUnpaidInvoicesforCustomer()
        {
            try
            {
                SBAS_DAL.Invoice invoice = new SBAS_DAL.Invoice();

                List<SBAS_Core.Model.Invoice> allinvoices = invoice.GetAllInvoicesForCustomerByIfPaid(customer.UserId, false);
                Assert.AreNotEqual(allinvoices.Count, 0, "No unpaid invoices found!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetAllInvoicesForCustomerByIfPaid method in the Invoice class for paid invoices.
        /// </summary>
        [TestMethod]
        public void GetAllPaidInvoicesforCustomer()
        {
            try
            {
                SBAS_DAL.Invoice invoice = new SBAS_DAL.Invoice();

                List<SBAS_Core.Model.Invoice> allinvoices = invoice.GetAllInvoicesForCustomerByIfPaid(customer.UserId, true);
                Assert.AreNotEqual(allinvoices.Count, 0, "No paid invoices found!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetNextInvoiceNumber method in the Invoice class.
        /// </summary>
        [TestMethod]
        public void testGetNextInvoiceNumber()
        {
            try
            {
                SBAS_DAL.Invoice invoice = new SBAS_DAL.Invoice();
                long customerId = 392;

                string invoiceNumber = invoice.GetNextInvoiceNumber(customerId);

                Assert.AreNotEqual(invoiceNumber, null, "Invoice number not returned!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetInvoiceLineItemsByInvoiceId method in the Invoice class.
        /// </summary>
        [TestMethod]
        public void testGetInvoiceLineItemsByInvoiceId()
        {
            try
            {
                long invoiceID = testInvoice.InvoiceId;

                SBAS_DAL.Invoice invoice = new SBAS_DAL.Invoice();

                List<SBAS_Core.Model.InvoiceLineItem> allInvoiceLineItems = invoice.GetInvoiceLineItemsByInvoiceId(invoiceID);
                Assert.AreNotEqual(allInvoiceLineItems.Count, 0, "Invoice line items not found!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetInvoiceLineItemByInvoiceLineItemId method in the Invoice class.
        /// </summary>
        [TestMethod]
        public void testGetInvoiceLineItemByInvoiceLineItemId()
        {
            try
            {
                long invoiceLineItemID = testInvoiceLineItem.InvoiceLineItemId;

                InvoiceLineItem invoiceLineItem = new SBAS_DAL.Invoice().GetInvoiceLineItemByInvoiceLineItemId(invoiceLineItemID);
               Assert.AreNotEqual(invoiceLineItemID, 0, "Invoice line item not found!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
       }

        /// <summary>
        /// Tests the DeleteInvoiceByInvoiceID method in the Invoice class.
        /// </summary>
        [TestMethod]
        public void testDeleteInvoiceByInvoiceId()
        {
            try
            {
                long invoiceId = testInvoice.InvoiceId;

                var retrievedInvoiceLineItems = new SBAS_DAL.Invoice().GetInvoiceLineItemsByInvoiceId(invoiceId);

                foreach (var invoiceLineItem in retrievedInvoiceLineItems)
                {
                    new SBAS_DAL.Invoice().DeleteInvoiceLineItem(invoiceLineItem);
                }

                bool deleteResult = new SBAS_DAL.Invoice().DeleteInvoiceByInvoiceID(invoiceId);
                Assert.AreEqual(true, deleteResult, "Could not delete Invoice!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }
    }
}
