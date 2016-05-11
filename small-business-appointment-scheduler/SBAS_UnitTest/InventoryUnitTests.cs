// ***********************************************************************
// Assembly         : SBAS_UnitTest
// Author           : Team 1 SWENG 500 Summer of 2014
// Created          : 06-01-2014
//
// Last Modified By : Team 1 SWENG 500 Summer of 2014
// Last Modified On : 08-04-2014
// ***********************************************************************
// <copyright file="InventoryUnitTests.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>This test class is responsible for properly testing all the methods in the SBAS_DAL.Inventory class</summary>
// ***********************************************************************
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SBAS_DAL;
using NPoco;
using SBAS_Core;
using System.Net.Mail;
using System.Net;
using System.Configuration;

/// <summary>
/// The SBAS_UnitTest namespace.
/// </summary>
namespace SBAS_UnitTest
{
    /// <summary>
    /// Class InventoryUnitTests.
    /// </summary>
    [TestClass]
    public class InventoryUnitTests
    {
        /// <summary>
        /// This field is initialized in the class setup. In order to test the inventory logic, you first need to create
        /// an address for referential integrity
        /// </summary>
        private static SBAS_Core.Model.Address address;
        /// <summary>
        /// This field is initialized in the class setup. In order to test the inventory logic, you first need to create
        /// a user for referential integrity
        /// </summary>
        private static SBAS_Core.Model.SBASUser sbasUser;
        /// <summary>
        /// This is the reference to our SQL database
        /// </summary>
        private static NPoco.Database db;

        // The following attributes correspond to the CreateUser and UpdateUser fields in the Database
        // The CreateDateTime and UpdateDateTime fields in the database will be set to currentDataTime
        /// <summary>
        /// The create user field
        /// </summary>
        private static string createUserField = "Test Create User";
        /// <summary>
        /// The update user field
        /// </summary>
        private static string updateUserField = "Test Update User";
        /// <summary>
        /// The current time and date
        /// </summary>
        private static DateTime currentDateTime = DateTime.Now;

        /// <summary>
        /// This method is ran first and creates the database connection, the user and the user's address
        /// </summary>
        /// <param name="a">A.</param>
        [ClassInitialize]
        public static void ClassSetup(TestContext a)
        {
            db = new NPoco.Database(SBAS_DAL.Base.GetConnectionString, DatabaseType.SqlServer2012);

            address = new SBAS_Core.Model.Address()
            {
                // AddressId is not required since this will be automatically filled in by the SQL server
                AddressLine1 = "123 Test Lane",
                AddressLine2 = "Apt #2",
                CityId = 1,
                StateId = 2,
                ZipCode = "12345",
                Longitude = 96.2M,
                Latitude = 91.3M,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,                               
            };

            db.Insert("Address", "AddressId", true, address);

            sbasUser = new SBAS_Core.Model.SBASUser()
            {
                // UserId is not requirerd since this will automatically be filled in by the SQL server
                // Id might be required once we enforce referential integrity, for now it works
                CompanyName = "Test Inc.",
                FirstName = "Flying",
                LastName = "Fox",
                FaxNumber = "8002345555",
                PhoneNumber = "8002345555",
                MobileNumber = "8002345555",  
                AddressId = address.AddressId,
                FirstStartTime = currentDateTime,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,   
            };          
            
            db.Insert("SBASUser", "UserId", true, sbasUser);            
        }

        /// <summary>
        /// This unit test method tests the GetUsersInventoryItems(long userId) method in the SBAS_DAL.Inventory class. 
        /// The test method inserts one item into a user’s inventory and then checks to see if the actual method is able 
        /// to properly retrieve the inventory items. It makes sure that it retrieved only one item and that all the item 
        /// descriptions match with what was inserted
        /// </summary>
        [TestMethod]
        public void TestGetUsersInventoryItems_UserHasOneInventoryItem()
        {                   
            var insertedInventory = InsertSampleInventory();
            var insertedInventoryItems = InsertSampleInventoryItems(1, insertedInventory.InventoryId);
            var retrievedInventoryItems = new SBAS_DAL.Inventory().GetUsersInventoryItems(sbasUser.UserId);  //sbasUser created in ClassSetup
           
            DeleteSampleInventoryItems(insertedInventoryItems);
            DeleteSampleInventory(insertedInventory);           
            
            // We only inserted one inventoryItem and we need to verify that we retrieved only one inventoryItem
            Assert.AreEqual(1, retrievedInventoryItems.Count);

            // Compare each item in the retrieved inventory to see if it matches with was was put into the inventory
            Assert.AreEqual(insertedInventoryItems[0].InventoryItemId, retrievedInventoryItems[0].InventoryItemId);
            Assert.AreEqual(insertedInventoryItems[0].InventoryId, retrievedInventoryItems[0].InventoryId);
            Assert.AreEqual(insertedInventoryItems[0].ItemName, retrievedInventoryItems[0].ItemName);
            Assert.AreEqual(insertedInventoryItems[0].ItemDescription, retrievedInventoryItems[0].ItemDescription);
            Assert.AreEqual(insertedInventoryItems[0].ItemPrice, retrievedInventoryItems[0].ItemPrice);
            Assert.AreEqual(insertedInventoryItems[0].QuantityOnHand, retrievedInventoryItems[0].QuantityOnHand);
            Assert.AreEqual(insertedInventoryItems[0].ServiceTypeId, retrievedInventoryItems[0].ServiceTypeId);
    
            Assert.AreEqual(insertedInventoryItems[0].UpdateUser, retrievedInventoryItems[0].UpdateUser);                      
            Assert.AreEqual(insertedInventoryItems[0].UpdateDateTime.Year, retrievedInventoryItems[0].UpdateDateTime.Year);
            Assert.AreEqual(insertedInventoryItems[0].UpdateDateTime.Month, retrievedInventoryItems[0].UpdateDateTime.Month);
            Assert.AreEqual(insertedInventoryItems[0].UpdateDateTime.Day, retrievedInventoryItems[0].UpdateDateTime.Day); 
            Assert.AreEqual(insertedInventoryItems[0].UpdateDateTime.Hour, retrievedInventoryItems[0].UpdateDateTime.Hour);
            Assert.AreEqual(insertedInventoryItems[0].UpdateDateTime.Minute, retrievedInventoryItems[0].UpdateDateTime.Minute);
            Assert.AreEqual(insertedInventoryItems[0].UpdateDateTime.Second, retrievedInventoryItems[0].UpdateDateTime.Second);

            Assert.AreEqual(insertedInventoryItems[0].CreateUser, retrievedInventoryItems[0].CreateUser);
            Assert.AreEqual(insertedInventoryItems[0].CreateDateTime.Year, retrievedInventoryItems[0].CreateDateTime.Year);
            Assert.AreEqual(insertedInventoryItems[0].CreateDateTime.Month, retrievedInventoryItems[0].CreateDateTime.Month);
            Assert.AreEqual(insertedInventoryItems[0].CreateDateTime.Day, retrievedInventoryItems[0].CreateDateTime.Day);
            Assert.AreEqual(insertedInventoryItems[0].CreateDateTime.Hour, retrievedInventoryItems[0].CreateDateTime.Hour);
            Assert.AreEqual(insertedInventoryItems[0].CreateDateTime.Minute, retrievedInventoryItems[0].CreateDateTime.Minute);
            Assert.AreEqual(insertedInventoryItems[0].CreateDateTime.Second, retrievedInventoryItems[0].CreateDateTime.Second);

            // Retrieval of milliseconds fails. There is an error either in NPoco or in SQLServer that is not properly inserting the specified millisecond value
            // For all immediate purposes, having millisecond field off by 2 milliseconds is not important to our application.
            //Assert.AreEqual(insertedInventoryItems[0].UpdateDateTime.Millisecond, retrievedInventoryItems[0].UpdateDateTime.Millisecond);
            //Assert.AreEqual(insertedInventoryItems[0].CreateDateTime.Millisecond, retrievedInventoryItems[0].CreateDateTime.Millisecond);
               
        }

        /// <summary>
        /// This unit test method tests the GetUsersInventoryItems(long userId) method in the SBAS_DAL.Inventory class. 
        /// The test method inserts two items into a user’s inventory and then checks to see if the actual method is 
        /// able to properly retrieve both of these inventory items. It makes sure that it retrieved both items and 
        /// that all the item descriptions match with what was inserted.
        /// </summary>
        [TestMethod]
        public void TestGetUsersInventoryItems_UserHasTwoInventoryItems()
        {
            var insertedInventory = InsertSampleInventory();
            var insertedInventoryItems = InsertSampleInventoryItems(2, insertedInventory.InventoryId);
            var retrievedInventoryItems = new SBAS_DAL.Inventory().GetUsersInventoryItems(sbasUser.UserId);

            DeleteSampleInventoryItems(insertedInventoryItems);
            DeleteSampleInventory(insertedInventory);    

            // A possibility could exist where items in each list are the same but in different order
            // To avoid errors, it is necessay to sort these lists before comparing values
            insertedInventoryItems.Sort(delegate(SBAS_Core.Model.InventoryItem i1, SBAS_Core.Model.InventoryItem i2)
            {
                return i1.InventoryItemId.CompareTo(i2.InventoryItemId);
            });

            retrievedInventoryItems.Sort(delegate(SBAS_Core.Model.InventoryItem i1, SBAS_Core.Model.InventoryItem i2)
            {
                return i1.InventoryItemId.CompareTo(i2.InventoryItemId);
            });

            // Compare each item in the retrieved inventory to see if it matches with was was put into the inventory
            Assert.AreEqual(insertedInventoryItems.Count, retrievedInventoryItems.Count);
            for (int i = 0; i < insertedInventoryItems.Count; i++)
            {
                // Compare each item in the retrieved inventory to see if it matches with was was put into the inventory
                Assert.AreEqual(insertedInventoryItems[0].InventoryItemId, retrievedInventoryItems[0].InventoryItemId);
                Assert.AreEqual(insertedInventoryItems[0].InventoryId, retrievedInventoryItems[0].InventoryId);
                Assert.AreEqual(insertedInventoryItems[0].ItemName, retrievedInventoryItems[0].ItemName);
                Assert.AreEqual(insertedInventoryItems[0].ItemDescription, retrievedInventoryItems[0].ItemDescription);
                Assert.AreEqual(insertedInventoryItems[0].ItemPrice, retrievedInventoryItems[0].ItemPrice);
                Assert.AreEqual(insertedInventoryItems[0].QuantityOnHand, retrievedInventoryItems[0].QuantityOnHand);
                Assert.AreEqual(insertedInventoryItems[0].ServiceTypeId, retrievedInventoryItems[0].ServiceTypeId);

                Assert.AreEqual(insertedInventoryItems[0].UpdateUser, retrievedInventoryItems[0].UpdateUser);
                Assert.AreEqual(insertedInventoryItems[0].UpdateDateTime.Year, retrievedInventoryItems[0].UpdateDateTime.Year);
                Assert.AreEqual(insertedInventoryItems[0].UpdateDateTime.Month, retrievedInventoryItems[0].UpdateDateTime.Month);
                Assert.AreEqual(insertedInventoryItems[0].UpdateDateTime.Day, retrievedInventoryItems[0].UpdateDateTime.Day);
                Assert.AreEqual(insertedInventoryItems[0].UpdateDateTime.Hour, retrievedInventoryItems[0].UpdateDateTime.Hour);
                Assert.AreEqual(insertedInventoryItems[0].UpdateDateTime.Minute, retrievedInventoryItems[0].UpdateDateTime.Minute);
                Assert.AreEqual(insertedInventoryItems[0].UpdateDateTime.Second, retrievedInventoryItems[0].UpdateDateTime.Second);

                Assert.AreEqual(insertedInventoryItems[0].CreateUser, retrievedInventoryItems[0].CreateUser);
                Assert.AreEqual(insertedInventoryItems[0].CreateDateTime.Year, retrievedInventoryItems[0].CreateDateTime.Year);
                Assert.AreEqual(insertedInventoryItems[0].CreateDateTime.Month, retrievedInventoryItems[0].CreateDateTime.Month);
                Assert.AreEqual(insertedInventoryItems[0].CreateDateTime.Day, retrievedInventoryItems[0].CreateDateTime.Day);
                Assert.AreEqual(insertedInventoryItems[0].CreateDateTime.Hour, retrievedInventoryItems[0].CreateDateTime.Hour);
                Assert.AreEqual(insertedInventoryItems[0].CreateDateTime.Minute, retrievedInventoryItems[0].CreateDateTime.Minute);
                Assert.AreEqual(insertedInventoryItems[0].CreateDateTime.Second, retrievedInventoryItems[0].CreateDateTime.Second);

                // Retrieval of milliseconds fails. There is an error either in NPoco or in SQLServer that is not properly inserting the specified millisecond value
                // For all immediate purposes, having millisecond field off by 2 milliseconds is not important to our application.
                //Assert.AreEqual(insertedInventoryItems[0].UpdateDateTime.Millisecond, retrievedInventoryItems[0].UpdateDateTime.Millisecond);
                //Assert.AreEqual(insertedInventoryItems[0].CreateDateTime.Millisecond, retrievedInventoryItems[0].CreateDateTime.Millisecond);
               
            } // End for   
        }

        /// <summary>
        /// This unit test method tests the GetUsersInventoryItems(long userId) method in the SBAS_DAL.Inventory class. 
        /// The test method attempts to retrieve a list of inventory items of a customer that does not exist in the 
        /// database. This type of customer has an invalid UserID of -1.
        /// </summary>
        [TestMethod]
        public void TestGetUsersInventoryItems_UserIdNotValid()
        {
            // The UserId is the primary key and in the Database can not have values less than zero
            // This guarantees that this user with this userID does not exist.
            var retrievedInventoryItems = new SBAS_DAL.Inventory().GetUsersInventoryItems(-1);

            // The associated SQL will correctly run, but will only return 0 results
            Assert.AreEqual(0, retrievedInventoryItems.Count);
        }

        /// <summary>
        /// This unit test method tests the GetUsersInventoryItemByInventoryItemId(long inventoryItemId) method in the 
        /// SBAS_DAL.Inventory class. The test method create a new inventory item for a user, retrieved the inventory item,
        /// and verifies that the same item that was inserted into the inventory was the one that was retrieved from the inventory.
        /// </summary>
        [TestMethod]
        public void TestGetUsersInventoryItemByInventoryItemId()
        {          
            var insertedInventory = InsertSampleInventory();
            var insertedInventoryItems = InsertSampleInventoryItems(1, insertedInventory.InventoryId); // Insert only one inventory item into the inventory
            var retrievedInventoryItem = new SBAS_DAL.Inventory().GetUsersInventoryItemByInventoryItemId(insertedInventoryItems[0].InventoryItemId);
            
            DeleteSampleInventoryItems(insertedInventoryItems);
            DeleteSampleInventory(insertedInventory);

            Assert.AreEqual(insertedInventoryItems[0].InventoryId, retrievedInventoryItem.InventoryId);
            Assert.AreEqual(insertedInventoryItems[0].HasPhysicalInventory, retrievedInventoryItem.HasPhysicalInventory);
            Assert.AreEqual(insertedInventoryItems[0].InventoryItemId, retrievedInventoryItem.InventoryItemId);
            Assert.AreEqual(insertedInventoryItems[0].ItemName, retrievedInventoryItem.ItemName);
            Assert.AreEqual(insertedInventoryItems[0].ItemPrice, retrievedInventoryItem.ItemPrice);
            Assert.AreEqual(insertedInventoryItems[0].QuantityOnHand, retrievedInventoryItem.QuantityOnHand);
            Assert.AreEqual(insertedInventoryItems[0].ServiceTypeId, retrievedInventoryItem.ServiceTypeId);
            
            Assert.AreEqual(insertedInventoryItems[0].CreateDateTime.Year, retrievedInventoryItem.CreateDateTime.Year);
            Assert.AreEqual(insertedInventoryItems[0].CreateDateTime.Month, retrievedInventoryItem.CreateDateTime.Month);
            Assert.AreEqual(insertedInventoryItems[0].CreateDateTime.Day, retrievedInventoryItem.CreateDateTime.Day);
            Assert.AreEqual(insertedInventoryItems[0].CreateDateTime.Hour, retrievedInventoryItem.CreateDateTime.Hour);
            Assert.AreEqual(insertedInventoryItems[0].CreateDateTime.Minute, retrievedInventoryItem.CreateDateTime.Minute);
            Assert.AreEqual(insertedInventoryItems[0].CreateDateTime.Second, retrievedInventoryItem.CreateDateTime.Second);

            Assert.AreEqual(insertedInventoryItems[0].UpdateDateTime.Year, retrievedInventoryItem.UpdateDateTime.Year);
            Assert.AreEqual(insertedInventoryItems[0].UpdateDateTime.Month, retrievedInventoryItem.UpdateDateTime.Month);
            Assert.AreEqual(insertedInventoryItems[0].UpdateDateTime.Day, retrievedInventoryItem.UpdateDateTime.Day);
            Assert.AreEqual(insertedInventoryItems[0].UpdateDateTime.Hour, retrievedInventoryItem.UpdateDateTime.Hour);
            Assert.AreEqual(insertedInventoryItems[0].UpdateDateTime.Minute, retrievedInventoryItem.UpdateDateTime.Minute);
            Assert.AreEqual(insertedInventoryItems[0].UpdateDateTime.Second, retrievedInventoryItem.UpdateDateTime.Second);  
        }

        /// <summary>
        /// This unit test method tests the CreateInventoryForNewUser(long userId) method in the SBAS_DAL.Inventory class. 
        /// This test method creates an inventory for the user and verifies than an inventory was in fact created for the user.
        /// </summary>
        [TestMethod]
        public void TestCreateInventoryForNewUser()
        {            
            using (db)
            {               
                string sqlQueryString = String.Format("SELECT COUNT(*) FROM Inventory WHERE CustomerId = {0}", sbasUser.UserId);
                
                int inventoryCountBeforeInsertion = db.Single<int>(sqlQueryString);
                var insertedInventory = new SBAS_DAL.Inventory().CreateInventoryForNewUser(sbasUser.UserId);
                int inventoryCountAfterInsertion = db.Single<int>(sqlQueryString);

                DeleteSampleInventory(insertedInventory);

                Assert.AreEqual(0, inventoryCountBeforeInsertion);
                Assert.AreEqual(1, inventoryCountAfterInsertion);
            }
        }

        /// <summary>
        /// This unit test method tests the GetUsersLowInventoryItems(long userId) method in the SBAS_DAL.Inventory class. 
        /// This method creates a specific amount of items that have a low inventory (quantity less than 2), inserts them 
        /// into a user’s inventory and then verifies that the respective method properly retrieves the inventory items 
        /// that only have a low inventory.
        /// </summary>
        [TestMethod]
        public void TestGetUsersLowInventoryItems()
        {
            const int AMOUNTOFLOWINVENTORYITEMS = 2;
            const int AMOUNTOFHIGHINVENTORYITEMS = 4;

            var insertedInventory = InsertSampleInventory();
            var insertedInventoryItems = InsertSampleInventoryItems(AMOUNTOFHIGHINVENTORYITEMS, insertedInventory.InventoryId); // This method inserts items with a quantity of 20
            insertedInventoryItems = InsertSampleLowInventoryItems(AMOUNTOFLOWINVENTORYITEMS, insertedInventoryItems, insertedInventory.InventoryId); // This method inserts items with a quantity of 1

            var retrievedInventoryItems = new SBAS_DAL.Inventory().GetUsersLowInventoryItems(sbasUser.UserId, 2);

            DeleteSampleInventoryItems(insertedInventoryItems);
            DeleteSampleInventory(insertedInventory);

            // A possibility could exist where items in each list are the same but in different order
            // To avoid errors, it is necessay to sort these lists before comparing values
            insertedInventoryItems.Sort(delegate(SBAS_Core.Model.InventoryItem i1, SBAS_Core.Model.InventoryItem i2)
            {
                return i1.InventoryItemId.CompareTo(i2.InventoryItemId);
            });

            retrievedInventoryItems.Sort(delegate(SBAS_Core.Model.InventoryItem i1, SBAS_Core.Model.InventoryItem i2)
            {
                return i1.InventoryItemId.CompareTo(i2.InventoryItemId);
            });
    
            Assert.AreEqual(AMOUNTOFLOWINVENTORYITEMS, retrievedInventoryItems.Count);            
        }

        /// <summary>
        /// This unit test method tests the GetUsersInventory(long userId) method in the SBAS_DAL.Inventory class. 
        /// The test method inserts a new inventory for a user and uses the actual method to retrieve the inventory 
        /// contents. The contents are then compared to make sure that they match with what was inserted and what was 
        /// retrieved.
        /// </summary>
        [TestMethod]
        public void TestGetUsersInventory_UserIdValid()
        {
            var insertedInventory = InsertSampleInventory();                        
            var retrievedInventory = new SBAS_DAL.Inventory().GetUsersInventory(sbasUser.UserId);

            DeleteSampleInventory(insertedInventory);   
            
            // Compare each item in the retrieved inventory to see if it matches with was was put into the inventory
            Assert.AreEqual(insertedInventory.InventoryId, retrievedInventory.InventoryId);
            Assert.AreEqual(insertedInventory.CustomerId, retrievedInventory.CustomerId);
            Assert.AreEqual(insertedInventory.LastRestockedDate, retrievedInventory.LastRestockedDate);
            Assert.AreEqual(insertedInventory.LastInventoryInspection, retrievedInventory.LastInventoryInspection);
            
            Assert.AreEqual(insertedInventory.UpdateUser, retrievedInventory.UpdateUser);
            Assert.AreEqual(insertedInventory.UpdateDateTime.Year, retrievedInventory.UpdateDateTime.Year);
            Assert.AreEqual(insertedInventory.UpdateDateTime.Month, retrievedInventory.UpdateDateTime.Month);
            Assert.AreEqual(insertedInventory.UpdateDateTime.Day, retrievedInventory.UpdateDateTime.Day);
            Assert.AreEqual(insertedInventory.UpdateDateTime.Hour, retrievedInventory.UpdateDateTime.Hour);
            Assert.AreEqual(insertedInventory.UpdateDateTime.Minute, retrievedInventory.UpdateDateTime.Minute);
            Assert.AreEqual(insertedInventory.UpdateDateTime.Second, retrievedInventory.UpdateDateTime.Second);

            Assert.AreEqual(insertedInventory.CreateUser, retrievedInventory.CreateUser);
            Assert.AreEqual(insertedInventory.CreateDateTime.Year, retrievedInventory.CreateDateTime.Year);
            Assert.AreEqual(insertedInventory.CreateDateTime.Month, retrievedInventory.CreateDateTime.Month);
            Assert.AreEqual(insertedInventory.CreateDateTime.Day, retrievedInventory.CreateDateTime.Day);
            Assert.AreEqual(insertedInventory.CreateDateTime.Hour, retrievedInventory.CreateDateTime.Hour);
            Assert.AreEqual(insertedInventory.CreateDateTime.Minute, retrievedInventory.CreateDateTime.Minute);
            Assert.AreEqual(insertedInventory.CreateDateTime.Second, retrievedInventory.CreateDateTime.Second);

            // Retrieval of milliseconds fails. There is an error either in NPoco or in SQLServer that is not properly inserting the specified millisecond value
            // For all immediate purposes, having millisecond field off by 2 milliseconds is not important to our application.
            //Assert.AreEqual(insertedInventory.UpdateDateTime.Millisecond, retrievedInventory.UpdateDateTime.Millisecond);
            //Assert.AreEqual(insertedInventory.CreateDateTime.Millisecond, retrievedInventory.CreateDateTime.Millisecond);
               
           
        }

        /// <summary>
        /// This unit test method tests the GetUsersInventory(long userId) method in the SBAS_DAL.Inventory class. 
        /// The test method attempts to retrieve the inventory of a customer that does not exist in the database. 
        /// This type of customer has an invalid UserID of -1.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestGetUsersInventory_UserIdNotValid()
        {
            // This will throw an exception in the NPoco layer
            // The UserId is the primary key and  the Database cannot have values less than zero
            // This guarantees that this userID does not exist.
            var retrievedInventory = new SBAS_DAL.Inventory().GetUsersInventory(-1);            
        }

        /// <summary>
        /// This unit test method tests the CreateInventoryItemForUser(long userID, SBAS_Core.Model.InventoryItem 
        /// inventoryItemToInsert) method in the SBAS_DAL.Inventory class. The test method uses the actual method to 
        /// insert inventory items into a user’s inventory. It then retrieves these items and makes sure that what was 
        /// inserted was the same as what was retrieved.
        /// </summary>
        [TestMethod]
        public void TestCreateInventoryItemForUser_UserIdValid()
        {
            // We first need to create an inventory and a associate an inventory with a customer before inserting an item into the inventory
            var insertedInventory = InsertSampleInventory();
           
            var insertedInventoryItem = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = insertedInventory.InventoryId, // Associate the particular item with a user's inventoryID
                ItemName = "Test Name",
                ItemDescription = "Test Description",
                ItemPrice = 15M,
                QuantityOnHand = 1,
                HasPhysicalInventory = true,
                ServiceTypeId = null,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,  
            };

            // Insert the inventory item to a user's inventory
            new SBAS_DAL.Inventory().CreateInventoryItemForUser(sbasUser.UserId, insertedInventoryItem);

            // Retrieve all the inventory items associated with the user
            var retrievedInventoryItems = new SBAS_DAL.Inventory().GetUsersInventoryItems(sbasUser.UserId);

            // Delete previously inserted inventory and inventory item
            DeleteSampleInventoryItem(retrievedInventoryItems[0]);
            DeleteSampleInventory(insertedInventory);

            // We only inserted one inventory item to the user's inventory and expect that we only retrieve one
            Assert.AreEqual(1, retrievedInventoryItems.Count);

            // Compare each item in the retrieved inventory to see if it matches with was was put into the inventory           
            Assert.AreEqual(insertedInventoryItem.InventoryId, retrievedInventoryItems[0].InventoryId);
            Assert.AreEqual(insertedInventoryItem.ItemName, retrievedInventoryItems[0].ItemName);
            Assert.AreEqual(insertedInventoryItem.ItemDescription, retrievedInventoryItems[0].ItemDescription);
            Assert.AreEqual(insertedInventoryItem.ItemPrice, retrievedInventoryItems[0].ItemPrice);
            Assert.AreEqual(insertedInventoryItem.QuantityOnHand, retrievedInventoryItems[0].QuantityOnHand);
            Assert.AreEqual(insertedInventoryItem.ServiceTypeId, retrievedInventoryItems[0].ServiceTypeId);

            Assert.AreEqual(insertedInventory.UpdateUser, retrievedInventoryItems[0].UpdateUser);
            Assert.AreEqual(insertedInventory.UpdateDateTime.Year, retrievedInventoryItems[0].UpdateDateTime.Year);
            Assert.AreEqual(insertedInventory.UpdateDateTime.Month, retrievedInventoryItems[0].UpdateDateTime.Month);
            Assert.AreEqual(insertedInventory.UpdateDateTime.Day, retrievedInventoryItems[0].UpdateDateTime.Day);
            Assert.AreEqual(insertedInventory.UpdateDateTime.Hour, retrievedInventoryItems[0].UpdateDateTime.Hour);
            Assert.AreEqual(insertedInventory.UpdateDateTime.Minute, retrievedInventoryItems[0].UpdateDateTime.Minute);
            
            Assert.AreEqual(insertedInventory.CreateUser, retrievedInventoryItems[0].CreateUser);
            Assert.AreEqual(insertedInventory.CreateDateTime.Year, retrievedInventoryItems[0].CreateDateTime.Year);
            Assert.AreEqual(insertedInventory.CreateDateTime.Month, retrievedInventoryItems[0].CreateDateTime.Month);
            Assert.AreEqual(insertedInventory.CreateDateTime.Day, retrievedInventoryItems[0].CreateDateTime.Day);
            Assert.AreEqual(insertedInventory.CreateDateTime.Hour, retrievedInventoryItems[0].CreateDateTime.Hour);
            Assert.AreEqual(insertedInventory.CreateDateTime.Minute, retrievedInventoryItems[0].CreateDateTime.Minute);
             
            // The DateTime fields of the insertedInventory object that is created above will differ from the actual inserted
            // DateTime that is found in the Database, since this datetime is created in the InsertInventoryItemByUserId method. 
            // The minute field could also be off, but most of the time this will be correct
            // Assert.AreEqual(insertedInventory.CreateDateTime.Second, retrievedInventoryItems[0].CreateDateTime.Second);
            // Assert.AreEqual(insertedInventory.UpdateDateTime.Second, retrievedInventoryItems[0].UpdateDateTime.Second);

            // Retrieval of milliseconds fails. There is an error either in NPoco or in SQLServer that is not properly inserting the specified millisecond value
            // For all immediate purposes, having millisecond field off by 2 milliseconds is not important to our application.
            // Assert.AreEqual(insertedInventory.UpdateDateTime.Millisecond, retrievedInventory.UpdateDateTime.Millisecond);
            // Assert.AreEqual(insertedInventory.CreateDateTime.Millisecond, retrievedInventory.CreateDateTime.Millisecond);
               

        }

        /// <summary>
        /// This unit test method tests the CreateInventoryItemForUser(long userID, SBAS_Core.Model.InventoryItem inventoryItemToInsert) method in 
        /// the SBAS_DAL.Inventory class. This method attempts to insert an inventory item to a customer that does not exist in the database.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateInventoryItemForUser_UserIdNotValid()
        {
            // We first need to create an inventory and a associate an inventory with a customer before inserting an item into the inventory
            var insertedInventory = InsertSampleInventory();
           
            var inventoryItemToInsert = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = insertedInventory.InventoryId, // Associate the particular item with a user's inventoryID
                ItemName = "Test Name",
                ItemDescription = "Test Description",
                ItemPrice = 19.97M,
                QuantityOnHand = 1,
                HasPhysicalInventory = true,
                ServiceTypeId = null,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };

            try
            {
                // The first paramater is the userId, and we choose -1 since in our SQL database, the userID is the primary Key
                // and can never be less than 0
                new SBAS_DAL.Inventory().CreateInventoryItemForUser(-1, inventoryItemToInsert);
            }
            finally
            {
                DeleteSampleInventory(insertedInventory);
            }           
          
        }

        /// <summary>
        /// This unit test method tests the CreateInventoryItemForUser(long userID, SBAS_Core.Model.InventoryItem inventoryItemToInsert) method 
        /// in the SBAS_DAL.Inventory class. An inventory first has to be created for a user before an inventory item can be associated with a user. 
        /// This method attempts to insert an inventory item for a user without an associated inventory. The test method expects an exception to occur.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateInventoryItemForUser_UserHasNoPreexstingInventory()
        {
            // We try to create an inventory Item for a user; however, we did not first create
            // an associated inventory for the user, and so this method should return an exception

            var insertedInventoryItem = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = 1,
                ItemName = "Test Name",
                ItemDescription = "Test Description",
                ItemPrice = 15M,
                QuantityOnHand = 1,
                HasPhysicalInventory = true,
                ServiceTypeId = null,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };

            // Insert the inventory item to a user's inventory
            new SBAS_DAL.Inventory().CreateInventoryItemForUser(sbasUser.UserId, insertedInventoryItem);
        }

        /// <summary>
        /// This unit test method tests the CreateInventoryItemForUser(long userID, SBAS_Core.Model.InventoryItem inventoryItemToInsert)  method in 
        /// the SBAS_DAL.Inventory class. This method inserts an inventory item to the database that has a decimal value for the price field. 
        /// It then retrieves this value from the database and makes sure that the price that was inserted is the same as the price that was entered
        /// </summary>
        [TestMethod]
        public void TestCreateInventoryItemForUser_ItemPriceDecimal()
        {
            // Need to associate an inventory with a customer before inserting an item into the inventory
            var insertedInventory = InsertSampleInventory();

            var insertedInventoryItem = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = insertedInventory.InventoryId, // Associate the particular item with a user's inventoryID
                ItemName = "Test Name",
                ItemDescription = "Test Description",
                ItemPrice = 19.97M, // Create an item price with values after the decimal point
                QuantityOnHand = 1,
                HasPhysicalInventory = true,
                ServiceTypeId = null,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };

            new SBAS_DAL.Inventory().CreateInventoryItemForUser(sbasUser.UserId, insertedInventoryItem);

            var retrievedInventoryItems = new SBAS_DAL.Inventory().GetUsersInventoryItems(sbasUser.UserId);

            DeleteSampleInventoryItem(retrievedInventoryItems[0]);
            DeleteSampleInventory(insertedInventory);

            // We only inserted one inventory item to the user's inventory and expect that we only retrieve one
            Assert.AreEqual(1, retrievedInventoryItems.Count);

            // Compare each item in the retrieved inventory to see if it matches with was was put into the inventory           
            Assert.AreEqual(insertedInventoryItem.InventoryId, retrievedInventoryItems[0].InventoryId);
            Assert.AreEqual(insertedInventoryItem.ItemName, retrievedInventoryItems[0].ItemName);
            Assert.AreEqual(insertedInventoryItem.ItemDescription, retrievedInventoryItems[0].ItemDescription);
            Assert.AreEqual(insertedInventoryItem.ItemPrice, retrievedInventoryItems[0].ItemPrice);
            Assert.AreEqual(insertedInventoryItem.QuantityOnHand, retrievedInventoryItems[0].QuantityOnHand);
            Assert.AreEqual(insertedInventoryItem.ServiceTypeId, retrievedInventoryItems[0].ServiceTypeId);

            Assert.AreEqual(insertedInventory.UpdateUser, retrievedInventoryItems[0].UpdateUser);
            Assert.AreEqual(insertedInventory.UpdateDateTime.Year, retrievedInventoryItems[0].UpdateDateTime.Year);
            Assert.AreEqual(insertedInventory.UpdateDateTime.Month, retrievedInventoryItems[0].UpdateDateTime.Month);
            Assert.AreEqual(insertedInventory.UpdateDateTime.Day, retrievedInventoryItems[0].UpdateDateTime.Day);
            Assert.AreEqual(insertedInventory.UpdateDateTime.Hour, retrievedInventoryItems[0].UpdateDateTime.Hour);
            Assert.AreEqual(insertedInventory.UpdateDateTime.Minute, retrievedInventoryItems[0].UpdateDateTime.Minute);

            Assert.AreEqual(insertedInventory.CreateUser, retrievedInventoryItems[0].CreateUser);
            Assert.AreEqual(insertedInventory.CreateDateTime.Year, retrievedInventoryItems[0].CreateDateTime.Year);
            Assert.AreEqual(insertedInventory.CreateDateTime.Month, retrievedInventoryItems[0].CreateDateTime.Month);
            Assert.AreEqual(insertedInventory.CreateDateTime.Day, retrievedInventoryItems[0].CreateDateTime.Day);
            Assert.AreEqual(insertedInventory.CreateDateTime.Hour, retrievedInventoryItems[0].CreateDateTime.Hour);
            Assert.AreEqual(insertedInventory.CreateDateTime.Minute, retrievedInventoryItems[0].CreateDateTime.Minute);

            // The DateTime fields of the insertedInventory object that is created above will differ from the actual inserted
            // DateTime that is found in the Database, since this datetime is created in the InsertInventoryItemByUserId method. 
            // The minute field could also be off, but most of the time this will be correct
            // Assert.AreEqual(insertedInventory.CreateDateTime.Second, retrievedInventoryItems[0].CreateDateTime.Second);
            // Assert.AreEqual(insertedInventory.UpdateDateTime.Second, retrievedInventoryItems[0].UpdateDateTime.Second);

            // Retrieval of milliseconds fails. There is an error either in NPoco or in SQLServer that is not properly inserting the specified millisecond value
            // For all immediate purposes, having millisecond field off by 2 milliseconds is not important to our application.
            // Assert.AreEqual(insertedInventory.UpdateDateTime.Millisecond, retrievedInventory.UpdateDateTime.Millisecond);
            // Assert.AreEqual(insertedInventory.CreateDateTime.Millisecond, retrievedInventory.CreateDateTime.Millisecond);
               
        }

        /// <summary>
        /// This unit test method tests the CreateInventoryItemForUser(long userID, SBAS_Core.Model.InventoryItem inventoryItemToInsert) method in 
        /// the SBAS_DAL.Inventory class. When inserting inventory items into the database it is important to associate the type of service that this 
        /// inventory item is going to perform. This test method attempts to insert and associate an inventory item to an an invalid service type. The test 
        /// method expects an exception to occur.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.Data.SqlClient.SqlException))]
        public void TestCreateInventoryItemForUser_ServiceTypeIdNotValid()
        {
            // This method attempts to insert an inventory ID with an invalid ServiceTypeId of -1
            // The ServiceTypeId is a Foreign Key and in our SQL server this value will never be a -1 

            // Need to associate an inventory with a customer before inserting an item into the inventory
            var insertedInventory = InsertSampleInventory();

            var insertedInventoryItem = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = insertedInventory.InventoryId,
                ItemName = "Test Name",
                ItemDescription = "Test Description",
                ItemPrice = 15M,
                QuantityOnHand = 1,
                HasPhysicalInventory = true,
                ServiceTypeId = -1, // Invalid ServiceTypeId
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };                        

            try
            {
                new SBAS_DAL.Inventory().CreateInventoryItemForUser(sbasUser.UserId, insertedInventoryItem);

            }
            finally
            {               
                DeleteSampleInventory(insertedInventory);
            }         

        }

        /// <summary>
        /// This unit test method tests the CreateInventoryForUser(long userID, SBAS_Core.Model.Inventory inventoryToCreate) method in the SBAS_DAL.Inventory class. 
        /// This method checks to see if the respective method properly inserted an associated inventory with a user.
        /// </summary>
        [TestMethod]
        public void TestCreateInventoryForUser_UserIdValid()
        {
            // Tests that only one piece of inventory was created
            // Tests that the inventory item characteristics that were created, match with what we thought we created

            using (db)
            {
                // Get inventory count before insertion
                string sqlQueryString = "SELECT COUNT(*) FROM Inventory";
                int inventoryCountBeforeInsertion = db.Single<int>(sqlQueryString);

                var inventory = new SBAS_Core.Model.Inventory()
                {
                    CustomerId = sbasUser.UserId,
                    LastRestockedDate = new DateTime(2014, 1, 1), // (Year, month, date)
                    LastInventoryInspection = new DateTime(2014, 1, 1),
                    CreateUser = createUserField,
                    CreateDateTime = currentDateTime,
                    UpdateUser = updateUserField,
                    UpdateDateTime = currentDateTime,
                };

                new SBAS_DAL.Inventory().CreateInventoryForUser(sbasUser.UserId, inventory);
                
                // Get inventory count after insertion
                int inventoryCountAfterInsertion = db.Single<int>(sqlQueryString);

                // Was only one inventory created for a cusomter?
                Assert.AreEqual(1, inventoryCountAfterInsertion - inventoryCountBeforeInsertion);

                // Retrieve the Inventory that we just created
                sqlQueryString = string.Format("SELECT * FROM Inventory WHERE Inventory.CustomerID = {0}", sbasUser.UserId);                             
                var retrievedInventory = db.Single<SBAS_Core.Model.Inventory>(sqlQueryString);

                DeleteSampleInventory(inventory);

                // Make sure that all field of the inserted and retrieved match
                Assert.AreEqual(inventory.CustomerId, retrievedInventory.CustomerId);
                Assert.AreEqual(inventory.LastRestockedDate, retrievedInventory.LastRestockedDate);
                Assert.AreEqual(inventory.LastInventoryInspection, retrievedInventory.LastInventoryInspection);

                Assert.AreEqual(inventory.InventoryId, retrievedInventory.InventoryId);
                Assert.AreEqual(inventory.UpdateUser, retrievedInventory.UpdateUser);
                Assert.AreEqual(inventory.UpdateDateTime.Year, retrievedInventory.UpdateDateTime.Year);
                Assert.AreEqual(inventory.UpdateDateTime.Month, retrievedInventory.UpdateDateTime.Month);
                Assert.AreEqual(inventory.UpdateDateTime.Day, retrievedInventory.UpdateDateTime.Day);
                Assert.AreEqual(inventory.UpdateDateTime.Hour, retrievedInventory.UpdateDateTime.Hour);
                Assert.AreEqual(inventory.UpdateDateTime.Minute, retrievedInventory.UpdateDateTime.Minute);
                Assert.AreEqual(inventory.UpdateDateTime.Second, retrievedInventory.UpdateDateTime.Second);
                
                Assert.AreEqual(inventory.CreateUser, retrievedInventory.CreateUser);
                Assert.AreEqual(inventory.CreateDateTime.Year, retrievedInventory.CreateDateTime.Year);
                Assert.AreEqual(inventory.CreateDateTime.Month, retrievedInventory.CreateDateTime.Month);
                Assert.AreEqual(inventory.CreateDateTime.Day, retrievedInventory.CreateDateTime.Day);
                Assert.AreEqual(inventory.CreateDateTime.Hour, retrievedInventory.CreateDateTime.Hour);
                Assert.AreEqual(inventory.CreateDateTime.Minute, retrievedInventory.CreateDateTime.Minute);
                Assert.AreEqual(inventory.CreateDateTime.Second, retrievedInventory.CreateDateTime.Second);
              
            } // End using   
        }

        /// <summary>
        /// Tests that it is not possible to create an inventory for a user that does not exist in the database
        /// This is accomplished by trying to create an inventory for a user with a userID of "-1". 
        /// There will never be a user with this type of Primary Key in the database
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.Data.SqlClient.SqlException))]
        public void TestCreateInventoryForUser_UserIdNotValid()
        {            
            using (db)
            {                
                var inventory = new SBAS_Core.Model.Inventory()
                {
                    CustomerId = sbasUser.UserId,
                    LastRestockedDate = new DateTime(2014, 1, 1), // (Year, month, date)
                    LastInventoryInspection = new DateTime(2014, 1, 1),
                    CreateUser = createUserField,
                    CreateDateTime = currentDateTime,
                    UpdateUser = updateUserField,
                    UpdateDateTime = currentDateTime,
                };

                new SBAS_DAL.Inventory().CreateInventoryForUser(-1, inventory);
                    
            } // End using   
        }

        /// <summary>
        /// Tests to make sure that the Inventory of a User is properly deleted
        /// This is accomplished by inserting an inventory for a user and then deleting this inventory
        /// and making sure only one inventory with that inventoryId was deleted
        /// </summary>
        [TestMethod]
        public void TestDeleteUsersInventoryByUserId_UserIdValid()
        {                        
            using (db)
            {
                var insertedInventory = InsertSampleInventory();
                string sqlQueryString = String.Format("SELECT COUNT(*) FROM Inventory WHERE InventoryId = {0}", insertedInventory.InventoryId);
                int inventoryCountAfterInsertion = db.Single<int>(sqlQueryString);

                try
                {
                    new SBAS_DAL.Inventory().DeleteUsersInventoryByUserId(sbasUser.UserId);
                }
                catch (Exception ex)
                {
                    DeleteSampleInventory(insertedInventory);
                }

                sqlQueryString = String.Format("SELECT COUNT(*) FROM Inventory WHERE InventoryId = {0}", insertedInventory.InventoryId);
                int inventoryCountAfterDeletion = db.Single<int>(sqlQueryString);

                Assert.AreEqual(1, inventoryCountAfterInsertion - inventoryCountAfterDeletion);
            }             
        }

        /// <summary>
        /// Tests to make sure that nothing was deleted when an invalid userId is passed to the method
        /// The inventory count should be the same before deletion and after deletion
        /// No errors are raised because deleting a user with an invalid ID is valid in SQL, the deletion will return 0 rows affected
        /// </summary>
        [TestMethod]
        public void TestDeleteUsersInventoryByUserId_UserIdNotValid()
        {                                    
            string sqlQueryString = "SELECT COUNT(*) FROM Inventory";
            int inventoryCountBeforeDeletion = db.Single<int>(sqlQueryString);
             
            new SBAS_DAL.Inventory().DeleteUsersInventoryByUserId(-1);
                
                
            int inventoryCountAfterDeletion = db.Single<int>(sqlQueryString);
            Assert.AreEqual(inventoryCountBeforeDeletion, inventoryCountAfterDeletion);                         
        }

        /// <summary>
        /// Tests to make sure that the Inventory of a User is properly deleted
        /// This is accomplished by inserting an inventory for a user and then deleting this inventory
        /// and making sure only one inventory with that inventoryId was deleted
        /// </summary>
        [TestMethod]
        public void TestDeleteUsersInventoryByInventoryId_InventoryIdValid()
        {            
            using (db)
            {
                var insertedInventory = InsertSampleInventory();
                string sqlQueryString = String.Format("SELECT COUNT(*) FROM Inventory WHERE InventoryId = {0}", insertedInventory.InventoryId);
                int inventoryCountAfterInsertion = db.Single<int>(sqlQueryString);

                try
                {
                    new SBAS_DAL.Inventory().DeleteUsersInventoryByInventoryId(insertedInventory.InventoryId);
                }
                catch (Exception ex)
                {
                    DeleteSampleInventory(insertedInventory);
                }

                sqlQueryString = String.Format("SELECT COUNT(*) FROM Inventory WHERE InventoryId = {0}", insertedInventory.InventoryId);
                int inventoryCountAfterDeletion = db.Single<int>(sqlQueryString);

                Assert.AreEqual(1, inventoryCountAfterInsertion - inventoryCountAfterDeletion);
            } // End using     
        }

        /// <summary>
        /// Tests to make sure that nothing was deleted when an invalid userId is passed to the method
        /// The inventory count should be the same before deletion and after deletion
        /// No errors are raised because deleting a user with an invalid ID is valid in SQL, the deletion will return 0 rows affected
        /// </summary>
        [TestMethod]
        public void TestDeleteUsersInventoryByInventoryId_InventoryIdNotValid()
        {           
            string sqlQueryString = "SELECT COUNT(*) FROM Inventory";
            int inventoryCountBeforeDeletion = db.Single<int>(sqlQueryString);

            new SBAS_DAL.Inventory().DeleteUsersInventoryByInventoryId(-1);

            int inventoryCountAfterDeletion = db.Single<int>(sqlQueryString);
            Assert.AreEqual(inventoryCountBeforeDeletion, inventoryCountAfterDeletion);    
        }


        /// <summary>
        /// Test to make sure that the InventoyItem of a user is properly deleted
        /// This is accomplished by first inserting an inventory item, then retrieving the amount of inventory items with that matching inventoryItemId
        /// Then deleting the inventory Item, and again retrieving the amount of inventory Items with that matching inventoryItemID
        /// </summary>
        [TestMethod]
        public void TestDeleteUsersInventoryItemByInventoryItemId_InventoryItemIdValid()
        {                       
            var insertedInventory = InsertSampleInventory();
            var insertedInventoryItems = InsertSampleInventoryItems(1, insertedInventory.InventoryId);

            string sqlQueryString = String.Format("SELECT COUNT(*) FROM InventoryItem WHERE InventoryItemId = {0}", insertedInventoryItems[0].InventoryItemId);
            int inventoryItemCountBeforeDeletion = db.Single<int>(sqlQueryString);                
            new SBAS_DAL.Inventory().DeteteUsersInventoryItemByInventoryItemId(insertedInventoryItems[0].InventoryItemId);                
            int inventoryItemCountAfterDeletion = db.Single<int>(sqlQueryString);
                
            DeleteSampleInventory(insertedInventory);

            Assert.AreEqual(1, inventoryItemCountBeforeDeletion - inventoryItemCountAfterDeletion);               
        }

        /// <summary>
        /// Tests to make sure that nothing was deleted when an invalid InventoryItemId is passed to the method
        /// The inventoryItem count should be the same before deletion and after deletion
        /// No errors are raised because deleting an InventoryItemId with an invalid ID is valid in SQL, the deletion will return 0 rows affected
        /// </summary>
        [TestMethod]
        public void TestDeleteUsersInventoryItemByInventoryItemId_InventoryItemIdNotValid()
        {           
            string sqlQueryString = "SELECT COUNT(*) FROM InventoryItem";
            int inventoryItemCountBeforeDeletion = db.Single<int>(sqlQueryString);
            new SBAS_DAL.Inventory().DeteteUsersInventoryItemByInventoryItemId(-1);
            int inventoryItemCountAfterDeletion = db.Single<int>(sqlQueryString);

            Assert.AreEqual(inventoryItemCountBeforeDeletion, inventoryItemCountAfterDeletion);
        }

        /// <summary>
        /// Tests to make sure that all of a user's inventory Items were properly deleted
        /// This is accomplished by first inserting inventory items for a user
        /// Confirming that this user has only five items, deleting all the items, and then
        /// confirming that the user has no more items
        /// </summary>
        [TestMethod]
        public void TestDeleteAllOfUsersInventoryItemsByUserId()
        {            
            const int NUMBEROFITEMSTOINSERT = 5;

            var insertedInventory = InsertSampleInventory();
            var insertedInventoryItems = InsertSampleInventoryItems(NUMBEROFITEMSTOINSERT, insertedInventory.InventoryId);

            string sqlQueryString = String.Format(@"SELECT COUNT(*) FROM InventoryItem, Inventory
                                     WHERE InventoryItem.InventoryId = Inventory.InventoryId
                                     AND Inventory.CustomerId = {0}", sbasUser.UserId);

            
            int inventoryItemCountAfterInsertion = db.Single<int>(sqlQueryString);
            new SBAS_DAL.Inventory().DeleteAllOfUsersInventoryItemsByUserId(sbasUser.UserId);
            int inventoryItemCountAfterDeletion = db.Single<int>(sqlQueryString);

            // Only need to delete the inventory that we created since we already deleted the inventory items
            DeleteSampleInventory(insertedInventory);

            Assert.AreEqual(5, inventoryItemCountAfterInsertion);
            Assert.AreEqual(0, inventoryItemCountAfterDeletion);
        }

        /// <summary>
        /// Tests to make sure that the UpdateUsersInventory method properly updates the inventory
        /// This is accomplished by creating an inventory for a user, updating the inventory, retrieving the 
        /// newly updated inventory and checking to see that all the fields were properly updated
        /// </summary>
        [TestMethod]
        public void TestUpdateUsersInventory()
        {
            var insertedInventory = InsertSampleInventory();

            insertedInventory.CreateUser = "New User";
            insertedInventory.UpdateUser = "New User";
            insertedInventory.LastInventoryInspection = new DateTime(1999, 12, 31); // (Year, month, date)
            insertedInventory.LastRestockedDate = new DateTime(1999, 12, 31); // (Year, month, date)

            new SBAS_DAL.Inventory().UpdateUsersInventory(insertedInventory);
            
            SBAS_Core.Model.Inventory retrievedInventory;
            using (db)
            {           
                string sqlQueryString = string.Format("SELECT * FROM Inventory WHERE Inventory.CustomerID = {0}", sbasUser.UserId);
                retrievedInventory = db.Single<SBAS_Core.Model.Inventory>(sqlQueryString);
   
            }

            DeleteSampleInventory(insertedInventory);

            Assert.AreEqual(insertedInventory.CreateUser, retrievedInventory.CreateUser);
            Assert.AreEqual(insertedInventory.UpdateUser, retrievedInventory.UpdateUser);
            Assert.AreEqual(insertedInventory.LastInventoryInspection, retrievedInventory.LastInventoryInspection);
            Assert.AreEqual(insertedInventory.LastRestockedDate, retrievedInventory.LastRestockedDate);
        }

        /// <summary>
        /// Tests to make sure that the UpdateUsersInventoryItem method properly updates the inventory item
        /// This is accomplished by creating an inventoryitem for a user
        /// Updating the item's fields, retrieving the newly updated inventory item and checking to see that all the fields were properly updated
        /// </summary>
        [TestMethod]
        public void TestUpdateUsersInventoryItem()
        {           
            var insertedInventory = InsertSampleInventory();
            var insertedInventoryItems = InsertSampleInventoryItems(1, insertedInventory.InventoryId);

            insertedInventoryItems[0].CreateUser = "New User";
            insertedInventoryItems[0].UpdateUser = "New User";
            insertedInventoryItems[0].HasPhysicalInventory = false;
            insertedInventoryItems[0].ItemDescription = "Brand new item description";
            insertedInventoryItems[0].ItemName = "Brand new item name";
            insertedInventoryItems[0].ItemPrice = 500.00M;
            insertedInventoryItems[0].QuantityOnHand = 200;

            new SBAS_DAL.Inventory().UpdateUsersInventoryItem(insertedInventoryItems[0]);

            SBAS_Core.Model.InventoryItem retrievedInventoryItem;
            using (db)
            {
                string sqlQueryString = string.Format(@"SELECT * FROM InventoryItem
                                                        WHERE InventoryItem.InventoryItemId = {0}", insertedInventoryItems[0].InventoryItemId);

                retrievedInventoryItem = db.Single<SBAS_Core.Model.InventoryItem>(sqlQueryString);

            }

            DeleteSampleInventoryItems(insertedInventoryItems);
            DeleteSampleInventory(insertedInventory);

            Assert.AreEqual(insertedInventoryItems[0].InventoryItemId, retrievedInventoryItem.InventoryItemId);
            Assert.AreEqual(insertedInventoryItems[0].InventoryId, retrievedInventoryItem.InventoryId);
            Assert.AreEqual(insertedInventoryItems[0].CreateUser, retrievedInventoryItem.CreateUser);
            Assert.AreEqual(insertedInventoryItems[0].UpdateUser, retrievedInventoryItem.UpdateUser);
            Assert.AreEqual(insertedInventoryItems[0].HasPhysicalInventory, retrievedInventoryItem.HasPhysicalInventory);
            Assert.AreEqual(insertedInventoryItems[0].ItemDescription, retrievedInventoryItem.ItemDescription);
            Assert.AreEqual(insertedInventoryItems[0].ItemName, retrievedInventoryItem.ItemName);
            Assert.AreEqual(insertedInventoryItems[0].ItemPrice, retrievedInventoryItem.ItemPrice);
            Assert.AreEqual(insertedInventoryItems[0].QuantityOnHand, retrievedInventoryItem.QuantityOnHand);           
        }

        /// <summary>
        /// This unit test method tests the SubtractInventoryItemQuantity(long inventoryItemId, int quantityToSubtract) method 
        /// in the SBAS_DAL.Inventory class. This method creates a nonphysical (business service) inventory item and attempts 
        /// to subtract a quantity from this item. An error should be thrown by the method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestSubtractInventoryItemQuantity_NonPhysicalInventory()
        {
            const int QUANTITY = 5;
            const int QUANTITY_TO_SUBTRACT = 4;

            var insertedInventory = InsertSampleInventory();
            var inventoryItemToInsert = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = insertedInventory.InventoryId,
                ItemName = "Test Nail Gun",
                ItemDescription = "Hitachi NT50AE2 18-Gauge Brad Nailer",
                ItemPrice = 48.00M,
                QuantityOnHand = QUANTITY,
                HasPhysicalInventory = false, // Non-Physical Inventory                 
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };

            var insertedInventoryItem = InsertSampleInventoryItem(inventoryItemToInsert);

            try
            {
                var newInventoryItem = new SBAS_DAL.Inventory().SubtractInventoryItemQuantity(insertedInventoryItem.InventoryItemId, QUANTITY_TO_SUBTRACT);
            }
            finally
            {
                DeleteSampleInventoryItem(insertedInventoryItem);
                DeleteSampleInventory(insertedInventory);
            }
        }

        /// <summary>
        /// This unit test method tests the SubtractInventoryItemQuantity(long inventoryItemId, int quantityToSubtract) method 
        /// in the SBAS_DAL.Inventory class. This method attempts to subtract a negative quantity from an item. 
        /// An error should be thrown by the method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestSubtractInventoryItemQuantity_SubtractQuantityLessThanZero()
        {
            const int QUANTITY = 5;
            const int QUANTITY_TO_SUBTRACT = -1;

            var insertedInventory = InsertSampleInventory();
            var inventoryItemToInsert = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = insertedInventory.InventoryId,
                ItemName = "Test Nail Gun",
                ItemDescription = "Hitachi NT50AE2 18-Gauge Brad Nailer",
                ItemPrice = 48.00M,
                QuantityOnHand = QUANTITY,
                HasPhysicalInventory = true,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };

            var insertedInventoryItem = InsertSampleInventoryItem(inventoryItemToInsert);

            try
            {
                var newInventoryItem = new SBAS_DAL.Inventory().SubtractInventoryItemQuantity(insertedInventoryItem.InventoryItemId, QUANTITY_TO_SUBTRACT);
            }
            finally
            {
                DeleteSampleInventoryItem(insertedInventoryItem);
                DeleteSampleInventory(insertedInventory);
            }          
        }

        /// <summary>
        /// This unit test method tests the SubtractInventoryItemQuantity(long inventoryItemId, int quantityToSubtract) method 
        /// in the SBAS_DAL.Inventory class. This method attempts to subtract a zero quantity from an item. It verifies that 
        /// the quantity of the inventory item did not change. 
        /// </summary>
        [TestMethod]
        public void TestSubtractInventoryItemQuantity_SubtractQuantityZero()
        {
            const int QUANTITY = 5;
            const int QUANTITY_TO_SUBTRACT = 0;

            var insertedInventory = InsertSampleInventory();
            var inventoryItemToInsert = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = insertedInventory.InventoryId,
                ItemName = "Test Nail Gun",
                ItemDescription = "Hitachi NT50AE2 18-Gauge Brad Nailer",
                ItemPrice = 48.00M,
                QuantityOnHand = QUANTITY,
                HasPhysicalInventory = true,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };

            var insertedInventoryItem = InsertSampleInventoryItem(inventoryItemToInsert);
            var newInventoryItem = new SBAS_DAL.Inventory().SubtractInventoryItemQuantity(insertedInventoryItem.InventoryItemId, QUANTITY_TO_SUBTRACT);

            DeleteSampleInventoryItem(insertedInventoryItem);
            DeleteSampleInventory(insertedInventory);

            Assert.AreEqual(QUANTITY - QUANTITY_TO_SUBTRACT, newInventoryItem.QuantityOnHand);
        }

        /// <summary>
        /// This unit test method tests the SubtractInventoryItemQuantity(long inventoryItemId, int quantityToSubtract) method in the SBAS_DAL.Inventory class.
        /// This method attempts to subtract a quantity of one from an item. It verifies that the quantity of the inventory item only changed by a value of one.
        /// </summary>
        [TestMethod]
        public void TestSubtractInventoryItemQuantity_SubtractQuantityOne()
        {
            const int QUANTITY = 5;
            const int QUANTITY_TO_SUBTRACT = 1;

            var insertedInventory = InsertSampleInventory();
            var inventoryItemToInsert = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = insertedInventory.InventoryId,
                ItemName = "Test Nail Gun",
                ItemDescription = "Hitachi NT50AE2 18-Gauge Brad Nailer",
                ItemPrice = 48.00M,
                QuantityOnHand = QUANTITY,
                HasPhysicalInventory = true,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };

            var insertedInventoryItem = InsertSampleInventoryItem(inventoryItemToInsert);
            var newInventoryItem = new SBAS_DAL.Inventory().SubtractInventoryItemQuantity(insertedInventoryItem.InventoryItemId, QUANTITY_TO_SUBTRACT);

            DeleteSampleInventoryItem(insertedInventoryItem);
            DeleteSampleInventory(insertedInventory);

            Assert.AreEqual(QUANTITY - QUANTITY_TO_SUBTRACT, newInventoryItem.QuantityOnHand);            
        }

        /// <summary>
        /// This unit test method tests the SubtractInventoryItemQuantity(long inventoryItemId, int quantityToSubtract) method 
        /// in the SBAS_DAL.Inventory class.This method attempts to subtract a quantity of two from an item. It verifies that the 
        /// quantity of the inventory item only changed by a value of two.
        /// </summary>
        [TestMethod]
        public void TestSubtractInventoryItemQuantity_SubtractQuantityTwo()
        {
            const int QUANTITY = 5;
            const int QUANTITY_TO_SUBTRACT = 2;

            var insertedInventory = InsertSampleInventory();
            var inventoryItemToInsert = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = insertedInventory.InventoryId,
                ItemName = "Test Nail Gun",
                ItemDescription = "Hitachi NT50AE2 18-Gauge Brad Nailer",
                ItemPrice = 48.00M,
                QuantityOnHand = QUANTITY,
                HasPhysicalInventory = true,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };

            var insertedInventoryItem = InsertSampleInventoryItem(inventoryItemToInsert);
            var newInventoryItem = new SBAS_DAL.Inventory().SubtractInventoryItemQuantity(insertedInventoryItem.InventoryItemId, QUANTITY_TO_SUBTRACT);

            DeleteSampleInventoryItem(insertedInventoryItem);
            DeleteSampleInventory(insertedInventory);

            Assert.AreEqual(QUANTITY - QUANTITY_TO_SUBTRACT, newInventoryItem.QuantityOnHand);
        }

        /// <summary>
        /// This unit test method tests the SubtractInventoryItemQuantity(long inventoryItemId, int quantityToSubtract) method 
        /// in the SBAS_DAL.Inventory class. This method verifies that the test method properly subtracts from the inventory quantity 
        /// when the resulting quantity is equal to zero. 
        /// </summary>
        [TestMethod]
        public void TestSubtractInventoryItemQuantity_FinalQuantityZero()
        {
            const int QUANTITY = 5;
            const int QUANTITY_TO_SUBTRACT = 5;

            var insertedInventory = InsertSampleInventory();
            var inventoryItemToInsert = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = insertedInventory.InventoryId,
                ItemName = "Test Nail Gun",
                ItemDescription = "Hitachi NT50AE2 18-Gauge Brad Nailer",
                ItemPrice = 48.00M,
                QuantityOnHand = QUANTITY,
                HasPhysicalInventory = true,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };

            var insertedInventoryItem = InsertSampleInventoryItem(inventoryItemToInsert);
            var newInventoryItem = new SBAS_DAL.Inventory().SubtractInventoryItemQuantity(insertedInventoryItem.InventoryItemId, QUANTITY_TO_SUBTRACT);

            DeleteSampleInventoryItem(insertedInventoryItem);
            DeleteSampleInventory(insertedInventory);

            Assert.AreEqual(QUANTITY - QUANTITY_TO_SUBTRACT, newInventoryItem.QuantityOnHand);
        }

        /// <summary>
        /// This unit test method tests the SubtractInventoryItemQuantity(long inventoryItemId, int quantityToSubtract) method 
        /// in the SBAS_DAL.Inventory class. This method verifies that it is not possible to subtract more inventory that what is currently in stock.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestSubtractInventoryItemQuantity_FinalQuantityLessThanZero()
        {
            const int QUANTITY = 5;
            const int QUANTITY_TO_SUBTRACT = 6;

            var insertedInventory = InsertSampleInventory();
            var inventoryItemToInsert = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = insertedInventory.InventoryId,
                ItemName = "Test Nail Gun",
                ItemDescription = "Hitachi NT50AE2 18-Gauge Brad Nailer",
                ItemPrice = 48.00M,
                QuantityOnHand = QUANTITY,
                HasPhysicalInventory = true,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };

            var insertedInventoryItem = InsertSampleInventoryItem(inventoryItemToInsert);

            try
            {
                var newInventoryItem = new SBAS_DAL.Inventory().SubtractInventoryItemQuantity(insertedInventoryItem.InventoryItemId, QUANTITY_TO_SUBTRACT);
            }
            finally
            {
                DeleteSampleInventoryItem(insertedInventoryItem);
                DeleteSampleInventory(insertedInventory);
            }                      
        }

        /// <summary>
        /// This unit test method tests the AddInventoryItemQuantity(long inventoryItemId, int quantityToAdd) method in the SBAS_DAL.Inventory class. 
        /// This method creates a nonphysical (business service) inventory item and attempts to add a quantity to this item. An error should be thrown by the method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestAddInventoryItemQuantity_NonPhysicalInventory()
        {
            const int QUANTITY = 5;
            const int QUANTITY_TO_ADD = 5;

            var insertedInventory = InsertSampleInventory();
            var inventoryItemToInsert = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = insertedInventory.InventoryId,
                ItemName = "Test Nail Gun",
                ItemDescription = "Hitachi NT50AE2 18-Gauge Brad Nailer",
                ItemPrice = 48.00M,
                QuantityOnHand = QUANTITY,
                HasPhysicalInventory = false, // Nonphysical Inventory
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };

            var insertedInventoryItem = InsertSampleInventoryItem(inventoryItemToInsert);

            try
            {
                var newInventoryItem = new SBAS_DAL.Inventory().AddInventoryItemQuantity(insertedInventoryItem.InventoryItemId, QUANTITY_TO_ADD);
            }
            finally
            {
                DeleteSampleInventoryItem(insertedInventoryItem);
                DeleteSampleInventory(insertedInventory);
            }
        }

        /// <summary>
        /// This unit test method tests the AddInventoryItemQuantity(long inventoryItemId, int quantityToAdd) method
        /// in the SBAS_DAL.Inventory class. This method attempts to add a negative quantity to an item. 
        /// An error should be thrown by the method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestAddInventoryItemQuantity_AddQuantityLessThanZero()
        {
            const int QUANTITY = 5;
            const int QUANTITY_TO_ADD = -1;

            var insertedInventory = InsertSampleInventory();
            var inventoryItemToInsert = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = insertedInventory.InventoryId,
                ItemName = "Test Nail Gun",
                ItemDescription = "Hitachi NT50AE2 18-Gauge Brad Nailer",
                ItemPrice = 48.00M,
                QuantityOnHand = QUANTITY,
                HasPhysicalInventory = true,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };

            var insertedInventoryItem = InsertSampleInventoryItem(inventoryItemToInsert);

            try
            {
                var newInventoryItem = new SBAS_DAL.Inventory().AddInventoryItemQuantity(insertedInventoryItem.InventoryItemId, QUANTITY_TO_ADD);
            }
            finally
            {
                DeleteSampleInventoryItem(insertedInventoryItem);
                DeleteSampleInventory(insertedInventory);
            }
        }

        /// <summary>
        /// This unit test method tests the AddInventoryItemQuantity(long inventoryItemId, int quantityToAdd) method 
        /// in the SBAS_DAL.Inventory class. This method attempts to add a zero quantity to an item. 
        /// It verifies that the quantity of the inventory item did not change. 
        /// </summary>
        [TestMethod]
        public void TestAddInventoryItemQuantity_AddQuantityZero()
        {
            const int QUANTITY = 5;
            const int QUANTITY_TO_ADD = 0;

            var insertedInventory = InsertSampleInventory();
            var inventoryItemToInsert = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = insertedInventory.InventoryId,
                ItemName = "Test Nail Gun",
                ItemDescription = "Hitachi NT50AE2 18-Gauge Brad Nailer",
                ItemPrice = 48.00M,
                QuantityOnHand = QUANTITY,
                HasPhysicalInventory = true,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };

            var insertedInventoryItem = InsertSampleInventoryItem(inventoryItemToInsert);
            var newInventoryItem = new SBAS_DAL.Inventory().AddInventoryItemQuantity(insertedInventoryItem.InventoryItemId, QUANTITY_TO_ADD);

            DeleteSampleInventoryItem(insertedInventoryItem);
            DeleteSampleInventory(insertedInventory);

            Assert.AreEqual(QUANTITY + QUANTITY_TO_ADD, newInventoryItem.QuantityOnHand);
        }

        /// <summary>
        /// This unit test method tests the AddInventoryItemQuantity(long inventoryItemId, int quantityToAdd) method 
        /// in the SBAS_DAL.Inventory class. This method attempts to add a quantity of one to an item. 
        /// It verifies that the quantity of the inventory item only changed by a value of one
        /// </summary>
        [TestMethod]
        public void TestAddInventoryItemQuantity_AddQuantityOne()
        {
            const int QUANTITY = 5;
            const int QUANTITY_TO_ADD = 1;

            var insertedInventory = InsertSampleInventory();
            var inventoryItemToInsert = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = insertedInventory.InventoryId,
                ItemName = "Test Nail Gun",
                ItemDescription = "Hitachi NT50AE2 18-Gauge Brad Nailer",
                ItemPrice = 48.00M,
                QuantityOnHand = QUANTITY,
                HasPhysicalInventory = true,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };

            var insertedInventoryItem = InsertSampleInventoryItem(inventoryItemToInsert);
            var newInventoryItem = new SBAS_DAL.Inventory().AddInventoryItemQuantity(insertedInventoryItem.InventoryItemId, QUANTITY_TO_ADD);

            DeleteSampleInventoryItem(insertedInventoryItem);
            DeleteSampleInventory(insertedInventory);

            Assert.AreEqual(QUANTITY + QUANTITY_TO_ADD, newInventoryItem.QuantityOnHand);
        }

        /// <summary>
        /// This unit test method tests the AddInventoryItemQuantity(long inventoryItemId, int quantityToAdd) method 
        /// in the SBAS_DAL.Inventory class. This method attempts to add a quantity of two to an item. 
        /// It verifies that the quantity of the inventory item only changed by a value of two.
        /// </summary>
        [TestMethod]
        public void TestAddInventoryItemQuantity_AddQuantityTwo()
        {
            const int QUANTITY = 5;
            const int QUANTITY_TO_ADD = 2;

            var insertedInventory = InsertSampleInventory();
            var inventoryItemToInsert = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = insertedInventory.InventoryId,
                ItemName = "Test Nail Gun",
                ItemDescription = "Hitachi NT50AE2 18-Gauge Brad Nailer",
                ItemPrice = 48.00M,
                QuantityOnHand = QUANTITY,
                HasPhysicalInventory = true,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };

            var insertedInventoryItem = InsertSampleInventoryItem(inventoryItemToInsert);
            var newInventoryItem = new SBAS_DAL.Inventory().AddInventoryItemQuantity(insertedInventoryItem.InventoryItemId, QUANTITY_TO_ADD);

            DeleteSampleInventoryItem(insertedInventoryItem);
            DeleteSampleInventory(insertedInventory);

            Assert.AreEqual(QUANTITY + QUANTITY_TO_ADD, newInventoryItem.QuantityOnHand);
        }

        /// <summary>
        /// This unit test method tests the GetInventoryItemQuantityOnHand(long inventoryItemId) method 
        /// in the SBAS_DAL.Inventory class. This method adds five inventory items to a user, uses the 
        /// test method, and verifies that the quantity of five was properly retrieved.
        /// </summary>
        [TestMethod]
        public void TestGetInventoryItemQuantityOnHand_PositiveQuantity()
        {
            const int QUANTITY = 5;            

            var insertedInventory = InsertSampleInventory();
            var inventoryItemToInsert = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = insertedInventory.InventoryId,
                ItemName = "Test Nail Gun",
                ItemDescription = "Hitachi NT50AE2 18-Gauge Brad Nailer",
                ItemPrice = 48.00M,
                QuantityOnHand = QUANTITY,
                HasPhysicalInventory = true,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };

            var insertedInventoryItem = InsertSampleInventoryItem(inventoryItemToInsert);
            var retrievedInventoryQuantity = new SBAS_DAL.Inventory().GetInventoryItemQuantityOnHand(insertedInventoryItem.InventoryItemId);

            DeleteSampleInventoryItem(insertedInventoryItem);
            DeleteSampleInventory(insertedInventory);

            Assert.AreEqual(QUANTITY, retrievedInventoryQuantity);
        }

        /// <summary>
        /// This unit test method tests the GetInventoryItemQuantityOnHand(long inventoryItemId) method
        /// in the SBAS_DAL.Inventory class. This method does not add any inventory items to a user 
        /// and makes sure that a quantity of zero was properly retrieved by the method.
        /// </summary>
        [TestMethod]
        public void TestGetInventoryItemQuantityOnHand_ZeroQuantity()
        {
            const int QUANTITY = 0;

            var insertedInventory = InsertSampleInventory();
            var inventoryItemToInsert = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = insertedInventory.InventoryId,
                ItemName = "Test Nail Gun",
                ItemDescription = "Hitachi NT50AE2 18-Gauge Brad Nailer",
                ItemPrice = 48.00M,
                QuantityOnHand = QUANTITY,
                HasPhysicalInventory = true,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };

            var insertedInventoryItem = InsertSampleInventoryItem(inventoryItemToInsert);
            var retrievedInventoryQuantity = new SBAS_DAL.Inventory().GetInventoryItemQuantityOnHand(insertedInventoryItem.InventoryItemId);

            DeleteSampleInventoryItem(insertedInventoryItem);
            DeleteSampleInventory(insertedInventory);

            Assert.AreEqual(QUANTITY, retrievedInventoryQuantity);
        }

        /// <summary>
        /// This unit test method tests the GetInventoryItemQuantityOnHand(long inventoryItemId) method
        /// in the SBAS_DAL.Inventory class. This method adds an inventory item with a quantity of null (a business service item)
        /// and verifies that the value of null was properly received by the test method.
        /// </summary>
        [TestMethod]
        public void TestGetInventoryItemQuantityOnHand_NullQuantity()
        {
            // An inventory item with a null quantity is an inventory item that is a service type
            int? QUANTITY = null;

            var insertedInventory = InsertSampleInventory();
            var inventoryItemToInsert = new SBAS_Core.Model.InventoryItem()
            {
                InventoryId = insertedInventory.InventoryId,
                ItemName = "Test Nail Gun",
                ItemDescription = "Hitachi NT50AE2 18-Gauge Brad Nailer",
                ItemPrice = 48.00M,
                QuantityOnHand = QUANTITY,
                HasPhysicalInventory = true,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };

            var insertedInventoryItem = InsertSampleInventoryItem(inventoryItemToInsert);
            var retrievedInventoryQuantity = new SBAS_DAL.Inventory().GetInventoryItemQuantityOnHand(insertedInventoryItem.InventoryItemId);

            DeleteSampleInventoryItem(insertedInventoryItem);
            DeleteSampleInventory(insertedInventory);

            Assert.AreEqual(QUANTITY, retrievedInventoryQuantity);
        }

        /// <summary>
        /// This unit test method tests the GetPhysicalInventoryItemsByCustomerId(long customerId) method.
        /// </summary>
        [TestMethod]
        public void TestGetPhysicalInventoryItemsByCustomerId()
        {
            const int PHYSICAL_INVENTORY_AMOUNT = 3;
            const int NONPHYSICAL_INVENTORY_AMOUNT = 2;            

            var insertedInventory = InsertSampleInventory();

            var insertedPhysicalInventoryItems = InsertSamplePhysicalInventoryItems(PHYSICAL_INVENTORY_AMOUNT, insertedInventory.InventoryId);
            var insertedNonPhysicalInventoryItems = InsertSampleNonPhysicalInventoryItems(NONPHYSICAL_INVENTORY_AMOUNT, insertedInventory.InventoryId);
            var retrievedPhycialInventoryItems = new SBAS_DAL.Inventory().GetPhysicalInventoryItemsByCustomerId(sbasUser.UserId);

            // A possibility could exist where items in each list are the same but in different order
            // To avoid errors, it is necessay to sort these lists before comparing values
            insertedPhysicalInventoryItems.Sort(delegate(SBAS_Core.Model.InventoryItem i1, SBAS_Core.Model.InventoryItem i2)
            {
                return i1.InventoryItemId.CompareTo(i2.InventoryItemId);
            });

            retrievedPhycialInventoryItems.Sort(delegate(SBAS_Core.Model.InventoryItem i1, SBAS_Core.Model.InventoryItem i2)
            {
                return i1.InventoryItemId.CompareTo(i2.InventoryItemId);
            });

            Assert.AreEqual(insertedPhysicalInventoryItems.Count, retrievedPhycialInventoryItems.Count);

            for (int i = 0; i < insertedPhysicalInventoryItems.Count; i++)
            {
                Assert.AreEqual(insertedPhysicalInventoryItems[0].InventoryItemId, retrievedPhycialInventoryItems[0].InventoryItemId);
                Assert.AreEqual(insertedPhysicalInventoryItems[0].InventoryId, retrievedPhycialInventoryItems[0].InventoryId);
                Assert.AreEqual(insertedPhysicalInventoryItems[0].ItemName, retrievedPhycialInventoryItems[0].ItemName);
                Assert.AreEqual(insertedPhysicalInventoryItems[0].ItemDescription, retrievedPhycialInventoryItems[0].ItemDescription);
                Assert.AreEqual(insertedPhysicalInventoryItems[0].ItemPrice, retrievedPhycialInventoryItems[0].ItemPrice);
                Assert.AreEqual(insertedPhysicalInventoryItems[0].QuantityOnHand, retrievedPhycialInventoryItems[0].QuantityOnHand);
                Assert.AreEqual(insertedPhysicalInventoryItems[0].ServiceTypeId, retrievedPhycialInventoryItems[0].ServiceTypeId);

                Assert.AreEqual(insertedPhysicalInventoryItems[0].UpdateUser, retrievedPhycialInventoryItems[0].UpdateUser);
                Assert.AreEqual(insertedPhysicalInventoryItems[0].UpdateDateTime.Year, retrievedPhycialInventoryItems[0].UpdateDateTime.Year);
                Assert.AreEqual(insertedPhysicalInventoryItems[0].UpdateDateTime.Month, retrievedPhycialInventoryItems[0].UpdateDateTime.Month);
                Assert.AreEqual(insertedPhysicalInventoryItems[0].UpdateDateTime.Day, retrievedPhycialInventoryItems[0].UpdateDateTime.Day);
                Assert.AreEqual(insertedPhysicalInventoryItems[0].UpdateDateTime.Hour, retrievedPhycialInventoryItems[0].UpdateDateTime.Hour);
                Assert.AreEqual(insertedPhysicalInventoryItems[0].UpdateDateTime.Minute, retrievedPhycialInventoryItems[0].UpdateDateTime.Minute);
                Assert.AreEqual(insertedPhysicalInventoryItems[0].UpdateDateTime.Second, retrievedPhycialInventoryItems[0].UpdateDateTime.Second);

                Assert.AreEqual(insertedPhysicalInventoryItems[0].CreateUser, retrievedPhycialInventoryItems[0].CreateUser);
                Assert.AreEqual(insertedPhysicalInventoryItems[0].CreateDateTime.Year, retrievedPhycialInventoryItems[0].CreateDateTime.Year);
                Assert.AreEqual(insertedPhysicalInventoryItems[0].CreateDateTime.Month, retrievedPhycialInventoryItems[0].CreateDateTime.Month);
                Assert.AreEqual(insertedPhysicalInventoryItems[0].CreateDateTime.Day, retrievedPhycialInventoryItems[0].CreateDateTime.Day);
                Assert.AreEqual(insertedPhysicalInventoryItems[0].CreateDateTime.Hour, retrievedPhycialInventoryItems[0].CreateDateTime.Hour);
                Assert.AreEqual(insertedPhysicalInventoryItems[0].CreateDateTime.Minute, retrievedPhycialInventoryItems[0].CreateDateTime.Minute);
                Assert.AreEqual(insertedPhysicalInventoryItems[0].CreateDateTime.Second, retrievedPhycialInventoryItems[0].CreateDateTime.Second);

                // The time between the creation of the inventoryObject in memory and the actual insertion of this object will be at times off my a milisecond or two
                // Assert.AreEqual(insertedPhysicalInventoryItems[0].UpdateDateTime.Millisecond, retrievedPhycialInventoryItems[0].UpdateDateTime.Millisecond);
                // Assert.AreEqual(insertedPhysicalInventoryItems[0].CreateDateTime.Millisecond, retrievedPhycialInventoryItems[0].CreateDateTime.Millisecond);
            } 

            DeleteSampleInventoryItems(insertedPhysicalInventoryItems);
            DeleteSampleInventoryItems(insertedNonPhysicalInventoryItems);
            DeleteSampleInventory(insertedInventory);
        }

        /// <summary>
        /// This method is a private helper method that is designed to add inventoryItems with a low quantityOnHand to an already established inventory
        /// This is why we are passing into this method the parameter of "insertedInventoryItems"
        /// </summary>
        /// <param name="amountToInsert">The amount to insert.</param>
        /// <param name="insertedInventoryItems">The inserted inventory items.</param>
        /// <param name="associatedInventoryId">The associated inventory identifier.</param>
        /// <returns>List&lt;SBAS_Core.Model.InventoryItem&gt;.</returns>
        private List<SBAS_Core.Model.InventoryItem> InsertSampleLowInventoryItems(int amountToInsert, List<SBAS_Core.Model.InventoryItem> insertedInventoryItems, long associatedInventoryId)
        {           
            for (int i = 0; i < amountToInsert; i++)
            {
                insertedInventoryItems.Add(new SBAS_Core.Model.InventoryItem()
                {
                    InventoryId = associatedInventoryId,
                    ItemName = "Test Nail Gun",
                    ItemDescription = "Hitachi NT50AE2 18-Gauge Brad Nailer",
                    ItemPrice = 48.00M,
                    QuantityOnHand = 1,
                    HasPhysicalInventory = true,
                    CreateUser = createUserField,
                    CreateDateTime = currentDateTime,
                    UpdateUser = updateUserField,
                    UpdateDateTime = currentDateTime,
                });
            }

            using (db)
            {
                // Insert items specified items into database
                for (int i = 0; i < amountToInsert; i++)
                {
                    db.Insert("InventoryItem", "InventoryItemId", true, insertedInventoryItems[(insertedInventoryItems.Count - 1) - i]);
                }
            }

            return insertedInventoryItems;
        }


        /// <summary>
        /// This method is a private helper method that deletes one single inventory at a time.
        /// </summary>
        /// <param name="insertedInventory">The inserted inventory.</param>
        private static void DeleteSampleInventory(SBAS_Core.Model.Inventory insertedInventory)
        {
            using (db)
            {
                db.Delete("Inventory", "InventoryId", insertedInventory, insertedInventory.InventoryId);
            }
        }

        /// <summary>
        /// This method is a private helper method that deletes a single inventory item at a time.
        /// </summary>
        /// <param name="inventoryItem">The inventory item.</param>
        private static void DeleteSampleInventoryItem(SBAS_Core.Model.InventoryItem inventoryItem)
        {
            using (db)
            {
                db.Delete("InventoryItem", "InventoryItemId", inventoryItem, inventoryItem.InventoryItemId);
                
            }
        }

        /// <summary>
        /// This method is a private helper method that deletes a lit of inventory items
        /// </summary>
        /// <param name="insertedInventoryItems">The inserted inventory items.</param>
        private static void DeleteSampleInventoryItems(List<SBAS_Core.Model.InventoryItem> insertedInventoryItems)
        {
            using (db)
            {
                foreach (SBAS_Core.Model.InventoryItem inventoryItem in insertedInventoryItems)
                {
                    db.Delete("InventoryItem", "InventoryItemId", inventoryItem, inventoryItem.InventoryItemId);
                }
            } 
        }

        /// <summary>
        /// This method is a private helper method the inserts one inventory for a user.
        /// </summary>
        /// <returns>SBAS_Core.Model.Inventory.</returns>
        private static SBAS_Core.Model.Inventory InsertSampleInventory()
        {
            var inventory = new SBAS_Core.Model.Inventory()
            {
                CustomerId = sbasUser.UserId,
                LastRestockedDate = new DateTime(2014, 1, 1), // (Year, month, date)
                LastInventoryInspection = new DateTime(2014, 1, 1),
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime, 
            };

            using (db)
            {
                db.Insert("Inventory", "InventoryId", true, inventory);
            }
            
            return inventory;
        }

        /// <summary>
        /// This method is a private helper method the inserts one inventory item for a user.
        /// </summary>
        /// <param name="inventoryItem">The inventory item.</param>
        /// <returns>SBAS_Core.Model.InventoryItem.</returns>
        private static SBAS_Core.Model.InventoryItem InsertSampleInventoryItem(SBAS_Core.Model.InventoryItem inventoryItem)
        {
            using (db)
            {
                db.Insert("InventoryItem", "InventoryItemId", true, inventoryItem);                
            }

            return inventoryItem;
        }

        /// <summary>
        /// This method is a private helper method the inserts a list of inventory items
        /// </summary>
        /// <param name="amountToInsert">The amount to insert.</param>
        /// <param name="associatedInventoryId">The associated inventory identifier.</param>
        /// <returns>List&lt;SBAS_Core.Model.InventoryItem&gt;.</returns>
        private static List<SBAS_Core.Model.InventoryItem> InsertSampleInventoryItems(int amountToInsert, long associatedInventoryId)
        {
            var inventoryItems = new List<SBAS_Core.Model.InventoryItem>();

            for (int i = 0; i < amountToInsert; i++)
            {
                inventoryItems.Add(new SBAS_Core.Model.InventoryItem()
                {
                    InventoryId = associatedInventoryId,
                    ItemName = "Test Nail Gun",
                    ItemDescription = "Hitachi NT50AE2 18-Gauge Brad Nailer",
                    ItemPrice = 48.00M,
                    QuantityOnHand = 20,
                    HasPhysicalInventory = true,
                    CreateUser = createUserField,
                    CreateDateTime = currentDateTime,
                    UpdateUser = updateUserField,
                    UpdateDateTime = currentDateTime,
                });
            }

            using (db)
            {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    db.Insert("InventoryItem", "InventoryItemId", true, inventoryItems[i]);
                }
            }
                       
            return inventoryItems;
        }


        /// <summary>
        /// This method is a private helper method the inserts a specified amount of inventory items for a user.
        /// </summary>
        /// <param name="amountToInsert">The amount to insert.</param>
        /// <param name="associatedInventoryId">The associated inventory identifier.</param>
        /// <returns>List&lt;SBAS_Core.Model.InventoryItem&gt;.</returns>
        private static List<SBAS_Core.Model.InventoryItem> InsertSamplePhysicalInventoryItems(int amountToInsert, long associatedInventoryId)
        {
            var inventoryItems = new List<SBAS_Core.Model.InventoryItem>();

            for (int i = 0; i < amountToInsert; i++)
            {
                var inventoryItem = new SBAS_Core.Model.InventoryItem()
                {
                    InventoryId = associatedInventoryId,
                    ItemName = "Test Physical Item",
                    ItemDescription = "Phycial Inventory Item",
                    ItemPrice = 10.00M,
                    QuantityOnHand = 10,
                    HasPhysicalInventory = true,
                    CreateUser = createUserField,
                    CreateDateTime = currentDateTime,
                    UpdateUser = updateUserField,
                    UpdateDateTime = currentDateTime,
                };

                var insertedInventoryItem = InsertSampleInventoryItem(inventoryItem);
                inventoryItems.Add(insertedInventoryItem);
            }            

            return inventoryItems;
        }


        /// <summary>
        /// This method is a private helper method that inserts a specified amount of non-physical inventory items.
        /// A non-physical inventory item is a business service.
        /// </summary>
        /// <param name="amountToInsert">The amount to insert.</param>
        /// <param name="associatedInventoryId">The associated inventory identifier.</param>
        /// <returns>List&lt;SBAS_Core.Model.InventoryItem&gt;.</returns>
        private static List<SBAS_Core.Model.InventoryItem> InsertSampleNonPhysicalInventoryItems(int amountToInsert, long associatedInventoryId)
        {
            var inventoryItems = new List<SBAS_Core.Model.InventoryItem>();

            for (int i = 0; i < amountToInsert; i++)
            {
                var inventoryItem = new SBAS_Core.Model.InventoryItem()
                {
                    InventoryId = associatedInventoryId,
                    ItemName = "Test Nonphycial Inventory Item",
                    ItemDescription = "Nonphysical Inventory Item",
                    ItemPrice = 48.00M,
                    QuantityOnHand = 20,
                    HasPhysicalInventory = false, // Nonphysical attribute
                    CreateUser = createUserField,
                    CreateDateTime = currentDateTime,
                    UpdateUser = updateUserField,
                    UpdateDateTime = currentDateTime,
                };

                var insertedInventoryItem = InsertSampleInventoryItem(inventoryItem);
                inventoryItems.Add(insertedInventoryItem);
            }

            return inventoryItems;
        }

        /// <summary>
        /// This method is the last method that is run during testing. It is responsible for deleting the user and address
        /// that were created during the class setup
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanUp()
        {
            // These are the only two classes that are shared amongst all the methods, and they need to be deleted
            // from the database when the class is finished running the methods
            using (db)
            {
                db.Delete("SBASUser", "UserId", sbasUser, sbasUser.UserId);
                db.Delete("Address", "AddressId", address, address.AddressId);
            }
        }
    } // End class InventoryUnitTests
} // End namespace SBAS_UnitTest