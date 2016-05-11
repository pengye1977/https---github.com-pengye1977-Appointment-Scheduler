// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Ye Peng
// Created          : 07-17-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 07-24-2014
// ***********************************************************************
// <copyright file="AppointmentLineItemModel.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
// This class containts all the properties that are passed from the controller to the view
//</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

/// <summary>
/// The Models namespace.
/// </summary>
namespace SBAS_Web.Models
{
    /// <summary>
    /// Class AppointmentLineItemModel.
    /// </summary>
    public class AppointmentLineItemModel
    {
        /// <summary>
        /// Gets or sets the appointment line item identifier.
        /// </summary>
        /// <value>The appointment line item identifier.</value>
        public long AppointmentLineItemId { get; set; }
        /// <summary>
        /// Gets or sets the appointment completed identifier.
        /// </summary>
        /// <value>The appointment completed identifier.</value>
        public long AppointmentCompletedId { get; set; }
        /// <summary>
        /// Gets or sets the inventory identifier.
        /// </summary>
        /// <value>The inventory identifier.</value>
        public long InventoryId { get; set; }
        /// <summary>
        /// Gets or sets the inventory item identifier.
        /// </summary>
        /// <value>The inventory item identifier.</value>
        public long InventoryItemId { get; set; }
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
        /// Gets or sets the quantity used.
        /// </summary>
        /// <value>The quantity used.</value>
        public long QuantityUsed { get; set; }

    }
}