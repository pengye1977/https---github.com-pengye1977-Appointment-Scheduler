// ***********************************************************************
// Assembly         : SBAS_DAL
// Author           : Ye Peng
// Created          : 06-04-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-10-2014
// ***********************************************************************
// <copyright file="Inventory.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>This class is the Database access class that communicates with our database server.
// This class is responsible for managing the data for the Inventory and InventoryItems</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPoco;
using SBAS_Core;

/// <summary>
/// The SBAS_DAL namespace.
/// </summary>
namespace SBAS_DAL
{
    /// <summary>
    /// Class Inventory.
    /// </summary>
    public class Inventory : Base
    {
        /// <summary>
        /// Retrieves a list of all the InventoryItems associated with a Customer
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns>List&lt;SBAS_Core.Model.InventoryItem&gt;.</returns>
        public List<SBAS_Core.Model.InventoryItem> GetUsersInventoryItems(long userID)
        {         
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                string sqlQueryString = string.Format(@"SELECT item.InventoryItemID, item.InventoryId, item.ItemName, 
                item.ItemDescription, item.ItemPrice, item.QuantityOnHand, item.HasPhysicalInventory, item.ServiceTypeID,
                item.CreateUser, item.CreateDateTime, item.UpdateUser, item.UpdateDateTime
                FROM Inventory, InventoryItem item                
                WHERE item.InventoryId = Inventory.InventoryID
                AND Inventory.CustomerId = {0}", userID);

                var inventory = db.Fetch<SBAS_Core.Model.InventoryItem>(sqlQueryString);

                return inventory;
            }
        }

        /// <summary>
        /// Retrieves a single inventoryItem associated with a Customer
        /// </summary>
        /// <param name="inventoryItemId">The inventory item identifier.</param>
        /// <returns>SBAS_Core.Model.InventoryItem.</returns>
        public SBAS_Core.Model.InventoryItem GetUsersInventoryItemByInventoryItemId(long inventoryItemId)
        {            
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                string sqlQueryString = string.Format(@"SELECT * FROM InventoryItem WHERE InventoryItemId = {0}", inventoryItemId);
                var inventory = db.Single<SBAS_Core.Model.InventoryItem>(sqlQueryString);
                return inventory;
            }
        }

        /// <summary>
        /// Retrieves a list of all the InventoryItems associated with a Customer
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="lowThreshold">The low threshold.</param>
        /// <returns>List&lt;SBAS_Core.Model.InventoryItem&gt;.</returns>
        public List<SBAS_Core.Model.InventoryItem> GetUsersLowInventoryItems(long userID, int lowThreshold)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                string sqlQueryString = string.Format(@"SELECT item.InventoryItemID, item.InventoryId, item.ItemName, 
                item.ItemDescription, item.ItemPrice, item.QuantityOnHand, item.HasPhysicalInventory, item.ServiceTypeID,
                item.CreateUser, item.CreateDateTime, item.UpdateUser, item.UpdateDateTime
                FROM Inventory, InventoryItem item                
                WHERE item.InventoryId = Inventory.InventoryID
                AND Inventory.CustomerId = {0}
                AND item.QuantityOnHand < {1}", userID, lowThreshold);

                var lowInventoryItems = db.Fetch<SBAS_Core.Model.InventoryItem>(sqlQueryString);

                return lowInventoryItems;
            }
        }

        /// <summary>
        /// Retrieves the details stored in the Inventory Table associated with a Customer
        /// An assumption is made that any Customer can have only one Inventory
        /// If a customer can have more than one inventory, for example,
        /// if they have more than one business and have an inventory for each business
        /// then this logic needs to be changed since it only returns a single Inventory
        /// object and not a list of inventory objects
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns>SBAS_Core.Model.Inventory.</returns>
        public SBAS_Core.Model.Inventory GetUsersInventory(long userID)
        {            
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                string sqlQueryString = string.Format("SELECT * FROM Inventory WHERE Inventory.CustomerID = {0}", userID);

                var inventory = db.Single<SBAS_Core.Model.Inventory>(sqlQueryString);

                return inventory;
            } 
        }

        /// <summary>
        /// This method inserts an InventoryItem for a Customer based on userID, and not based on inventoryID  
        /// There needs to be an existing inventory already associated with the user for this method to work
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="inventoryItemToInsert">The inventory item to insert.</param>
        /// <exception cref="System.ArgumentException">
        /// Cannot insert: Item cannot have a physical inventory and have a quantity less than zero
        /// or
        /// User has no associated inventory. Please create inventory for user
        /// </exception>
        public void CreateInventoryItemForUser(long userID, SBAS_Core.Model.InventoryItem inventoryItemToInsert)
        {          
            if(inventoryItemToInsert.HasPhysicalInventory == true && inventoryItemToInsert.QuantityOnHand < 0)
            {
                throw new ArgumentException("Cannot insert: Item cannot have a physical inventory and have a quantity less than zero");
            }
         
            // Since we are creating a new inventory item in the database, it is advisable to make sure that the create time and the update time are the same
            DateTime currentDateTime = DateTime.Now;
            inventoryItemToInsert.CreateDateTime = currentDateTime;
            inventoryItemToInsert.UpdateDateTime = currentDateTime;

            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                // Check to see if there is an inventory associated with the user
                string sqlQueryString = string.Format("SELECT COUNT(*) FROM Inventory WHERE Inventory.CustomerId  = {0}", userID);
                if (db.Single<int>(sqlQueryString) == 0)
                {
                    throw new ArgumentException("User has no associated inventory. Please create inventory for user");
                }
                else
                {
                    // We need to find the proper inventoryID associated with our user, each user has a unique associated inventory
                    sqlQueryString = string.Format("SELECT * FROM Inventory WHERE Inventory.CustomerId  = {0}", userID);
                    var inventory = db.Single<SBAS_Core.Model.Inventory>(sqlQueryString);

                    // After we find the user's associated inventoryID, we associate the particular inventoryItem to this inventoryID
                    // In affect, we are adding an item into a user's inventory
                    inventoryItemToInsert.InventoryId = inventory.InventoryId;
                    db.Insert("InventoryItem", "InventoryItemId", true, inventoryItemToInsert);
                } // End else                
            } // End using                       
        }

        /// <summary>
        /// This method creates an inventory for a user. This method should only be called once after a new user is added to the database.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="inventoryToCreate">The inventory to create.</param>
        /// <returns>SBAS_Core.Model.Inventory.</returns>
        public SBAS_Core.Model.Inventory CreateInventoryForUser(long userID, SBAS_Core.Model.Inventory inventoryToCreate)
        {
            // Should this method not create another inventory for a user if there is already one inventory created
            // Restrict each customer to have only one inventory?      

            // Associate an inventory with a particular user
            inventoryToCreate.CustomerId = userID;

            DateTime currentDateTime = DateTime.Now;
            inventoryToCreate.CreateDateTime = currentDateTime;
            inventoryToCreate.UpdateDateTime = currentDateTime;

            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                db.Insert("Inventory", "InventoryId", true, inventoryToCreate);
            }

            return inventoryToCreate;
        }

        /// <summary>
        /// This method creates an inventory for a user. This method should only be called once after a new user is added to the database.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns>SBAS_Core.Model.Inventory.</returns>
        public SBAS_Core.Model.Inventory CreateInventoryForNewUser(long userID)
        {
            // Should this method not create another inventory for a user if there is already one inventory created
            // Restrict each customer to have only one inventory?    

            DateTime currentDateTime = DateTime.Now;
            var inventoryToCreate = new SBAS_Core.Model.Inventory()
            {
                // InventoryId will be populated by NPoco during the insertion of this field
                CustomerId = userID, // Inventory Created for new User with passed in UserID
                CreateDateTime = currentDateTime,
                UpdateDateTime = currentDateTime,
                CreateUser = "Create User",
                UpdateUser = "Update User",
                // LastInventoryInspection not required, can be null
                // LastRestockedDate not required, can be null
            };

            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                db.Insert("Inventory", "InventoryId", true, inventoryToCreate);
            }

            return inventoryToCreate;
        }

        /// <summary>
        /// This method deletes a user's inventory by inputting a user's ID
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns><c>true</c> if the deletion was successful, <c>false</c> otherwise.</returns>
        public bool DeleteUsersInventoryByUserId(long userID)
        {
            bool result;
            string sqlQueryString = string.Format("DELETE FROM Inventory WHERE CustomerId={0}", userID);

            try
            {
                using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
                {
                    db.Execute(sqlQueryString);
                }

                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// This method deletes a user's inventory by inputting a specific inventory ID
        /// </summary>
        /// <param name="inventoryIdToDelete">The inventory identifier to delete.</param>
        /// <returns><c>true</c> if the deletion was successful, <c>false</c> otherwise.</returns>
        public bool DeleteUsersInventoryByInventoryId(long inventoryIdToDelete)
        {
            bool result;

            try
            {
                using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
                {
                    db.Delete("Inventory", "InventoryId", new SBAS_Core.Model.Inventory(), inventoryIdToDelete);
                }

                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Detetes the users inventory item by inventory item identifier.
        /// </summary>
        /// <param name="inventoryItemIdToDelete">The inventory item identifier to delete.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool DeteteUsersInventoryItemByInventoryItemId(long inventoryItemIdToDelete)
        {
            bool result;

            try
            {
                using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
                {
                    db.Delete("InventoryItem", "InventoryItemId", new SBAS_Core.Model.InventoryItem(), inventoryItemIdToDelete);
                }

                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// This method deletes all of user's inventory items by userId.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool DeleteAllOfUsersInventoryItemsByUserId(long userID)
        {
            bool result;

            try
            {
                using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
                {
                    // Retrieve the associated inventoryID of the user
                    string sqlQueryString = string.Format("SELECT InventoryId FROM Inventory WHERE CustomerId = {0}", userID);
                    var userinventoryId = db.Single<long>(sqlQueryString);

                    // Delete all the inventory Items associated with the inventoryID, in effect deleting all of a user's inventory items
                    sqlQueryString = string.Format("DELETE FROM InventoryItem WHERE InventoryId={0}", userinventoryId);
                    db.Execute(sqlQueryString);
                }

                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// This method updates a user's inventory
        /// </summary>
        /// <param name="inventory">The inventory.</param>
        /// <returns>SBAS_Core.Model.Inventory.</returns>
        public SBAS_Core.Model.Inventory UpdateUsersInventory(SBAS_Core.Model.Inventory inventory)
        {
            // We do not want to change the object coming into this method in fear that other parts of the program might have a reference to it
            // Also, NPoco will change this object, and it is a good idea to pass it a new reference

            var inventoryToUpdate = new SBAS_Core.Model.Inventory()
            {
                InventoryId = inventory.InventoryId,
                CustomerId = inventory.CustomerId,
                LastRestockedDate = inventory.LastRestockedDate,
                LastInventoryInspection = inventory.LastInventoryInspection,
                CreateUser = inventory.CreateUser,
                CreateDateTime = inventory.CreateDateTime,
                UpdateUser = inventory.UpdateUser,
                UpdateDateTime = DateTime.Now, // Since inventory is being updated, we need to update this field.
            };

            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                db.Update("Inventory", "InventoryId", inventoryToUpdate);
            }

            return inventoryToUpdate;
        }

        /// <summary>
        /// This method updates a user's inventory item
        /// </summary>
        /// <param name="inventoryItem">The inventory item.</param>
        /// <returns>SBAS_Core.Model.InventoryItem.</returns>
        public SBAS_Core.Model.InventoryItem UpdateUsersInventoryItem(SBAS_Core.Model.InventoryItem inventoryItem)
        {
            // We do not want to change the object coming into this method in fear that other parts of the program might have a reference to it
            // Also, NPoco will change this object, and it is a good idea to pass it a new reference

            var inventoryItemToUpdate = new SBAS_Core.Model.InventoryItem()
            {
                InventoryItemId = inventoryItem.InventoryItemId,
                InventoryId = inventoryItem.InventoryId,
                ItemName = inventoryItem.ItemName,
                ItemDescription = inventoryItem.ItemDescription,
                ItemPrice = inventoryItem.ItemPrice,
                QuantityOnHand = inventoryItem.QuantityOnHand,
                HasPhysicalInventory = inventoryItem.HasPhysicalInventory,
                ServiceTypeId = inventoryItem.ServiceTypeId,
                CreateUser = inventoryItem.CreateUser,
                CreateDateTime = inventoryItem.CreateDateTime,
                UpdateUser = inventoryItem.UpdateUser,
                UpdateDateTime = DateTime.Now, // Since inventoryItem is being updated, we need to update this field.            
            };

            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                db.Update("InventoryItem", "InventoryItemId", inventoryItemToUpdate);
            }

            return inventoryItemToUpdate;
        }

        /// <summary>
        /// This method lowers an inventory item's quantity by a given amount.
        /// </summary>
        /// <param name="inventoryItemId">The inventory item identifier.</param>
        /// <param name="quantityToSubtract">The quantity to subtract.</param>
        /// <returns>SBAS_Core.Model.InventoryItem.</returns>
        /// <exception cref="System.ArgumentException">
        /// Cannot subtract: Quantity to subtract cannot be less than zero
        /// or
        /// Cannot subtract: Item is not a physical inventory item
        /// or
        /// </exception>
        public SBAS_Core.Model.InventoryItem SubtractInventoryItemQuantity(long inventoryItemId, int quantityToSubtract)
        {
            var inventoryItem = GetUsersInventoryItemByInventoryItemId(inventoryItemId);

            if (quantityToSubtract < 0)
            {
                throw new ArgumentException("Cannot subtract: Quantity to subtract cannot be less than zero");
            }

            if (inventoryItem.ServiceTypeId != null || inventoryItem.HasPhysicalInventory == false)
            {
                throw new ArgumentException("Cannot subtract: Item is not a physical inventory item");
            }

            if(inventoryItem.QuantityOnHand < quantityToSubtract)
            {
                throw new ArgumentException(String.Format("Cannot subtract: Attempting to subtract: {0}, Current Item Quantity {1}", quantityToSubtract, inventoryItem.QuantityOnHand));
            }

            inventoryItem.QuantityOnHand -= quantityToSubtract;
            var updatedInventoryItem = UpdateUsersInventoryItem(inventoryItem);
            return updatedInventoryItem;
        }

        /// <summary>
        /// This method increases an inventory item's quantity by a given amount.
        /// </summary>
        /// <param name="inventoryItemId">The inventory item identifier.</param>
        /// <param name="quantityToAdd">The quantity to add.</param>
        /// <returns>SBAS_Core.Model.InventoryItem.</returns>
        /// <exception cref="System.ArgumentException">
        /// Cannot add: Quantity to add cannot be less than zero
        /// or
        /// Cannot add: Item is not a physical inventory item
        /// </exception>
        public SBAS_Core.Model.InventoryItem AddInventoryItemQuantity(long inventoryItemId, int quantityToAdd)
        {
            var inventoryItem = GetUsersInventoryItemByInventoryItemId(inventoryItemId);

            if (quantityToAdd < 0)
            {
                throw new ArgumentException("Cannot add: Quantity to add cannot be less than zero");
            }
            
            if (inventoryItem.ServiceTypeId != null || inventoryItem.HasPhysicalInventory == false)
            {
                throw new ArgumentException("Cannot add: Item is not a physical inventory item");
            }

            inventoryItem.QuantityOnHand += quantityToAdd;
            var updatedInventoryItem = UpdateUsersInventoryItem(inventoryItem);
            return updatedInventoryItem;
        }

        /// <summary>
        /// This method retrieves the current quantity of a specified inventory item
        /// </summary>
        /// <param name="inventoryItemId">The inventory item identifier.</param>
        /// <returns>System.Nullable&lt;System.Int64&gt;.</returns>
        public long? GetInventoryItemQuantityOnHand(long inventoryItemId)
        {
            // Inventory Item can have a null quantity, this is a service type item

            var inventoryItem = GetUsersInventoryItemByInventoryItemId(inventoryItemId);
            return inventoryItem.QuantityOnHand;
        }

        /// <summary>
        /// This method retrieves all the physical inventory items that a customer has
        /// A physical inventory item is any inventory item that is not a business service
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>List&lt;SBAS_Core.Model.InventoryItem&gt;.</returns>
        public List<SBAS_Core.Model.InventoryItem> GetPhysicalInventoryItemsByCustomerId(long customerId)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                string sqlQueryString = string.Format(@"SELECT item.InventoryItemID, item.InventoryId, item.ItemName, 
                item.ItemDescription, item.ItemPrice, item.QuantityOnHand, item.HasPhysicalInventory, item.ServiceTypeID,
                item.CreateUser, item.CreateDateTime, item.UpdateUser, item.UpdateDateTime
                FROM Inventory, InventoryItem item                
                WHERE item.InventoryId = Inventory.InventoryID
                AND item.HasPhysicalInventory = 'true'
                AND Inventory.CustomerId = {0}", customerId);

                var inventory = db.Fetch<SBAS_Core.Model.InventoryItem>(sqlQueryString);

                return inventory;
            }
        }


        /// <summary>
        /// This method determines whether a particular inventory item is associated with an appointment.
        /// </summary>
        /// <param name="inventoryItemID">The inventory item identifier.</param>
        /// <returns><c>true</c> if the inventory item is assoicated with an appointment; otherwise, <c>false</c>.</returns>
        public bool IsInventoryItemAssoicatedWithAppointment(int inventoryItemID)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                // Check to see if there is an inventory associated with an appointment
                string sqlQueryString = string.Format("SELECT COUNT(*) FROM AppointmentLineItem WHERE InventoryItemId  = {0}", inventoryItemID);
                if (db.Single<int>(sqlQueryString) > 0)
                {
                    return true;
                }

                return false;
            }
        }
        
    } // End class
} // End namespace