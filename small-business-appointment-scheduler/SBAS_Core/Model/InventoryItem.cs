// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 06-07-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 07-24-2014
// ***********************************************************************
// <copyright file="InventoryItem.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>This class is used in the Object-Relational mapping of data from our database server. 
// This class serves as the model for the data found in the InventoryItem table in our database</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The Model namespace.
/// </summary>
namespace SBAS_Core.Model
{
    /// <summary>
    /// Class InventoryItem.
    /// </summary>
    public class InventoryItem : ModelBase
    {
        /// <summary>
        /// Gets or sets the inventory item identifier.
        /// </summary>
        /// <value>The inventory item identifier.</value>
        public long InventoryItemId { get; set; }
        /// <summary>
        /// Gets or sets the inventory identifier.
        /// </summary>
        /// <value>The inventory identifier.</value>
        public long InventoryId { get; set; }
        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>The name of the item.</value>
        public string ItemName { get; set; }
        /// <summary>
        /// Gets or sets the item description.
        /// </summary>
        /// <value>The item description.</value>
        public string ItemDescription { get; set; }
        /// <summary>
        /// Gets or sets the item price.
        /// </summary>
        /// <value>The item price.</value>
        public decimal ItemPrice { get; set; }
        /// <summary>
        /// Gets or sets the quantity on hand.
        /// </summary>
        /// <value>The quantity on hand.</value>
        public long? QuantityOnHand { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance has physical inventory.
        /// </summary>
        /// <value><c>true</c> if this instance has physical inventory; otherwise, <c>false</c>.</value>
        public bool HasPhysicalInventory { get; set; }
        /// <summary>
        /// Gets or sets the service type identifier.
        /// </summary>
        /// <value>The service type identifier.</value>
        public long? ServiceTypeId { get; set; }

    }
}
