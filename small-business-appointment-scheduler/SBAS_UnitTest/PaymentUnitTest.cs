// ***********************************************************************
// Assembly         : SBAS_UnitTest
// Author           : Ye Peng
// Created          : 06-29-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="PaymentUnitTest.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>Contains all of the unit tests for the Payment class.</summary>
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
    /// Contains all of the unit tests for the Payment class.
    /// </summary>
    [TestClass]
    public class PaymentUnitTest
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
        /// The test payment
        /// </summary>
        private static Payment testPayment;
        
        /// <summary>
        /// The test payment method
        /// </summary>
        private static PaymentMethod testPaymentMethod;

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
        /// Initializes the PaymentUnitTest class. Adds test data to the database for payment unit testing.
        /// </summary>
        /// <param name="testContext">The test context.</param>
        [ClassInitialize()]
        public static void PaymentUnitTest_ClassInitialize(TestContext testContext)
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
                    AmountDue = (decimal)100.00,
                    DueDate = DateTime.Now.AddDays(20),
                    Comments = "This is a test.",
                    CustomerId = customer.UserId,
                    ClientID = client.UserId,
                    SentToClient = false,
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
                
                // Create payment method
                testPaymentMethod = new PaymentMethod
                {
                    // Do not need PaymentMethodID - SQL Server will automatically populate
                    PaymentMethodType = "5",
                    PaymentMethodDescription = "Miscellaneous Payment Method",
                    CreateUser = createUserField,
                    CreateDateTime = currentDateTime,
                    UpdateUser = updateUserField,
                    UpdateDateTime = currentDateTime,
                };

                db.Insert("PaymentMethod", "PaymentMethodID", true, testPaymentMethod);

                // Create payment
                testPayment = new Payment
                {
                    // Do not need PaymentID - SQL Server will automatically populate
                    CustomerId = customer.UserId,
                    ClientId = client.UserId,
                    InvoiceId = testInvoice.InvoiceId,
                    PaymentAmount = (decimal)50.00,
                    PaymentDate = DateTime.Now,
                    PaymentMethodId = testPaymentMethod.PaymentMethodId,
                    CreateUser = createUserField,
                    CreateDateTime = currentDateTime,
                    UpdateUser = updateUserField,
                    UpdateDateTime = currentDateTime,
                };

                db.Insert("Payment", "PaymentID", true, testPayment);

            } // End method ClassInitialize
        }

        /// <summary>
        /// Cleans up the database from the Payment unit testing.
        /// </summary>
        [ClassCleanup()]
        public static void PaymentUnitTest_ClassCleanup()
        {
            using (db)
            {
                db.Delete("Payment", "PaymentID", testPayment, testPayment.PaymentId);
                db.Delete("PaymentMethod", "PaymentMethodID", testPaymentMethod, testPaymentMethod.PaymentMethodId);
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
        /// Tests the GetPaymentByPaymentId method in the Payment class for an existing payment.
        /// </summary>
        [TestMethod]
        public void testGetExistingPaymentByPaymentId()
        {
            try
            {
                long paymentId = testPayment.PaymentId;
                Payment newPayment = new SBAS_DAL.Payment().GetPaymentByPaymentId(paymentId);
                Assert.AreNotEqual(0, newPayment.PaymentId, "Payment was not found!");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetPaymentsByClientId method in the Payment class for an existing client.
        /// </summary>
        [TestMethod]
        public void testGetAllPaymentsByClientId()
        {
            try
            {
                long clientId = client.UserId;
                SBAS_DAL.Payment payment = new SBAS_DAL.Payment();

                List<SBAS_Core.Model.Payment> allPayments = new SBAS_DAL.Payment().GetPaymentsByClientId(clientId);
                Assert.AreNotEqual(allPayments.Count, 0, "No payments found!");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetPaymentsByCustomerId method in the Payment class for an existing customer.
        /// </summary>
        [TestMethod]
        public void testGetAllPaymentsByCustomerId()
        {
            try
            {
                long customerId = customer.UserId;

                List<SBAS_Core.Model.Payment> allPayments = new SBAS_DAL.Payment().GetPaymentsByCustomerId(customerId);
                Assert.AreNotEqual(allPayments.Count, 0, "No payments found!");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetPaymentsByInvoiceId method in the payment class for an existing invoice.
        /// </summary>
        [TestMethod]
        public void testGetAllPaymentsByInvoiceId()
        {
            try
            {
                long invoiceId = testInvoice.InvoiceId;

                List<SBAS_Core.Model.Payment> allPayments = new SBAS_DAL.Payment().GetPaymentsByInvoiceId(invoiceId);
                Assert.AreNotEqual(allPayments.Count, 0, "No payments found!");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetPaymentByPaymentId method in the Payment class for a non-existent payment.
        /// </summary>
        [TestMethod]
        public void testGetPaymentByNonExistentPaymentId()
        {
            try
            {
                long paymentId = 999;
                Payment newPayment = new SBAS_DAL.Payment().GetPaymentByPaymentId(paymentId);
                Assert.AreNotEqual(0, newPayment.PaymentId, "Payment was not found!");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetPaymentsByClientId method in the Payment class for a non-existent client.
        /// </summary>
        [TestMethod]
        public void testGetAllPaymentsByNonExistentClientId()
        {
            try
            {
                long clientId = 999;
                List<SBAS_Core.Model.Payment> allPayments = new SBAS_DAL.Payment().GetPaymentsByClientId(clientId);
                Assert.AreNotEqual(allPayments.Count, 0, "No payments found!");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetPaymentsByCustomerId method in the Payment class for a non-existent customer.
        /// </summary>
        [TestMethod]
        public void testGetAllPaymentsByNonExistentCustomerId()
        {
            try
            {
                long customerId = 999;
                List<SBAS_Core.Model.Payment> allPayments = new SBAS_DAL.Payment().GetPaymentsByCustomerId(customerId);
                Assert.AreNotEqual(allPayments.Count, 0, "No payments found!");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetPaymentsByInvoiceId method in the Payment class for a non-existent invoice.
        /// </summary>
        [TestMethod]
        public void testGetAllPaymentsByNonExistentInvoiceId()
        {
            try
            {
                long invoiceId = 999;
                List<SBAS_Core.Model.Payment> allPayments = new SBAS_DAL.Payment().GetPaymentsByInvoiceId(invoiceId);
                Assert.AreNotEqual(allPayments.Count, 0, "No payments found!");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the AddPayment and DeletePayment methods in the Payment class.
        /// </summary>
        [TestMethod]
        public void AddAndDeletePayment()
        {
            try
            {
                Payment payment = new Payment();
                payment.CustomerId = customer.UserId;
                payment.ClientId = client.UserId;
                payment.InvoiceId = testInvoice.InvoiceId;
                payment.PaymentAmount = (decimal)75.00;
                payment.PaymentDate = DateTime.Now;
                payment.PaymentMethodId = testPaymentMethod.PaymentMethodId;
                payment.CreateUser = createUserField;
                payment.CreateDateTime = currentDateTime;
                payment.UpdateUser = updateUserField;
                payment.UpdateDateTime = currentDateTime;

                Payment newPayment = new SBAS_DAL.Payment().AddPayment(payment);
                Assert.AreNotEqual(0, newPayment.PaymentId, "Payment was not added!");

                if (newPayment != null && newPayment.PaymentId > 0)
                {
                    bool deleteResult = new SBAS_DAL.Payment().DeletePayment(newPayment);
                    Assert.AreEqual(true, deleteResult, "Could not delete Payment!");
                }
                else
                {
                    Assert.AreNotEqual(newPayment, null, "Payment does not exist!");
                }   
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the AddPaymentMethod and DeletePaymentMethod methods in the Payment class.
        /// </summary>
        [TestMethod]
        public void AddAndDeletePaymentMethod()
        {
            try
            {
                PaymentMethod paymentMethod = new PaymentMethod();
                paymentMethod.PaymentMethodType = "Check";
                paymentMethod.PaymentMethodDescription = "Personal Check";
                paymentMethod.CreateUser = createUserField;
                paymentMethod.CreateDateTime = currentDateTime;
                paymentMethod.UpdateUser = updateUserField;
                paymentMethod.UpdateDateTime = currentDateTime;

                PaymentMethod newPaymentMethod = new SBAS_DAL.Payment().AddPaymentMethod(paymentMethod);
                Assert.AreNotEqual(0, newPaymentMethod.PaymentMethodId, "Payment Method was not added!");

                Payment payment = new Payment();
                payment.CustomerId = customer.UserId;
                payment.ClientId = client.UserId;
                payment.InvoiceId = testInvoice.InvoiceId;
                payment.PaymentAmount = (decimal)75.00;
                payment.PaymentDate = DateTime.Now;
                payment.PaymentMethodId = paymentMethod.PaymentMethodId;
                payment.CreateUser = createUserField;
                payment.CreateDateTime = currentDateTime;
                payment.UpdateUser = updateUserField;
                payment.UpdateDateTime = currentDateTime;

                Payment newPayment = new SBAS_DAL.Payment().AddPayment(payment);
                Assert.AreNotEqual(0, newPayment.PaymentId, "Payment was not added!");

                if (newPayment != null && newPayment.PaymentId > 0)
                {
                    bool deleteResult = new SBAS_DAL.Payment().DeletePayment(newPayment);
                    Assert.AreEqual(true, deleteResult, "Could not delete Payment!");
                    
                    bool deleteResult2 = new SBAS_DAL.Payment().DeletePaymentMethod(newPaymentMethod);
                    Assert.AreEqual(true, deleteResult2, "Could not delete Payment Method!");
                }
                else
                {
                    Assert.AreNotEqual(newPayment, null, "Payment does not exist!");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the UpdatePayment method in the Payment class.
        /// </summary>
        [TestMethod]
        public void testUpdatePayment()
        {
            try
            {
                long paymentId = testPayment.PaymentId;
                Payment newPayment = new SBAS_DAL.Payment().GetPaymentByPaymentId(paymentId);
                Assert.AreNotEqual(0, newPayment.PaymentId, "Payment was not found!");

                newPayment.PaymentDate = DateTime.Now.AddDays(1);
                newPayment.UpdateUser = "Bennington Update";
                newPayment.UpdateDateTime = DateTime.Now;

                Payment newPaymentUpdate = new SBAS_DAL.Payment().UpdatePayment(newPayment);
                Assert.AreEqual("Bennington Update", newPaymentUpdate.UpdateUser, "Payment was not updated!");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the UpdatePaymentMethod method in the Payment class.
        /// </summary>
        [TestMethod]
        public void testUpdatePaymentMethod()
        {
            try
            {
                long paymentId = testPayment.PaymentId;
                Payment newPayment = new SBAS_DAL.Payment().GetPaymentByPaymentId(paymentId);
                Assert.AreNotEqual(0, newPayment.PaymentId, "Payment was not found!");

                PaymentMethod newPaymentMethod = testPaymentMethod;
                Assert.AreNotEqual(0, newPaymentMethod.PaymentMethodId, "Payment Method was not created!");

                newPaymentMethod.PaymentMethodType = "VISA";
                newPaymentMethod.UpdateUser = "Bennington Update";
                newPaymentMethod.UpdateDateTime = DateTime.Now;

                PaymentMethod newPaymentMethodUpdate = new SBAS_DAL.Payment().UpdatePaymentMethod(newPaymentMethod);
                Assert.AreEqual("Bennington Update", newPaymentMethodUpdate.UpdateUser, "Payment was not updated!");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the  GetPaymentMethodByPaymentMethodID method in the Payment class.
        /// </summary>
        [TestMethod]
        public void testGetPaymentMethodByPaymentMethodId()
        {
            try
            {
                long paymentMethodId = testPayment.PaymentMethodId;
                PaymentMethod newPaymentMethod = new SBAS_DAL.Payment().GetPaymentMethodByPaymentMethodID(paymentMethodId);
                Assert.AreNotEqual(0, newPaymentMethod.PaymentMethodId, "Payment method was not found!");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the DeletePaymentMethodByPaymentMethodID method in the Payment class.
        /// </summary>
        [TestMethod]
        public void testDeletePaymentMethodByPaymentMethodId()
        {
            try
            {
                PaymentMethod paymentMethod = new PaymentMethod();
                paymentMethod.PaymentMethodType = "Check";
                paymentMethod.PaymentMethodDescription = "Personal Check";
                paymentMethod.CreateUser = createUserField;
                paymentMethod.CreateDateTime = currentDateTime;
                paymentMethod.UpdateUser = updateUserField;
                paymentMethod.UpdateDateTime = currentDateTime;

                PaymentMethod newPaymentMethod = new SBAS_DAL.Payment().AddPaymentMethod(paymentMethod);

                bool deleteResult = new SBAS_DAL.Payment().DeletePaymentMethodByPaymentMethodID(newPaymentMethod.PaymentMethodId);
                Assert.AreEqual(true, deleteResult, "Could not delete Payment Method!");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the DeletePaymentByPaymentID method in the Payment class.
        /// </summary>
        [TestMethod]
        public void testDeletePaymentByPaymentId()
        {
            try
            {

                PaymentMethod paymentMethod = new PaymentMethod();
                paymentMethod.PaymentMethodType = "Check";
                paymentMethod.PaymentMethodDescription = "Personal Check";
                paymentMethod.CreateUser = createUserField;
                paymentMethod.CreateDateTime = currentDateTime;
                paymentMethod.UpdateUser = updateUserField;
                paymentMethod.UpdateDateTime = currentDateTime;

                PaymentMethod newPaymentMethod = new SBAS_DAL.Payment().AddPaymentMethod(paymentMethod);

                Payment payment = new Payment();
                payment.CustomerId = customer.UserId;
                payment.ClientId = client.UserId;
                payment.InvoiceId = testInvoice.InvoiceId;
                payment.PaymentAmount = (decimal)75.00;
                payment.PaymentDate = DateTime.Now;
                payment.PaymentMethodId = newPaymentMethod.PaymentMethodId;
                payment.CreateUser = createUserField;
                payment.CreateDateTime = currentDateTime;
                payment.UpdateUser = updateUserField;
                payment.UpdateDateTime = currentDateTime;

                Payment newPayment = new SBAS_DAL.Payment().AddPayment(payment);

                bool deleteResult2 = new SBAS_DAL.Payment().DeletePaymentByPaymentID(newPayment.PaymentId);
                Assert.AreEqual(true, deleteResult2, "Could not delete payment!");

                bool deleteResult = new SBAS_DAL.Payment().DeletePaymentMethodByPaymentMethodID(newPaymentMethod.PaymentMethodId);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Tests the GetPaymentMethodByPaymentMethodType method in the Payment class.
        /// </summary>
        [TestMethod]
        public void testGetPaymentMethodByPaymentMethodType()
        {
            try
            {
                string paymentMethodType = testPaymentMethod.PaymentMethodType;
                PaymentMethod newPaymentMethod = new SBAS_DAL.Payment().GetPaymentMethodByPaymentMethodType(paymentMethodType);
                Assert.AreNotEqual(0, newPaymentMethod.PaymentMethodType, "Payment method was not found!");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Generic Exception Handler: {0}", ex.ToString());
            }
        }
    }
}
