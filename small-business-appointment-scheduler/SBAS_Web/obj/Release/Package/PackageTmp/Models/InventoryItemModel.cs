// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Adam Bak
// Created          : 07-08-2014
//
// Last Modified By : Adam Bak
// Last Modified On : 08-10-2014
// ***********************************************************************
// <copyright file="InventoryItemModel.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>This class represents the Model for the View used for Inventory Items</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using SBAS_Core.Common.Attributes;


/// <summary>
/// The Models namespace.
/// </summary>
namespace SBAS_Web.Models
{
    /// <summary>
    /// Class InventoryItemModel.
    /// </summary>
    public class InventoryItemModel
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
        [Display(Name = "Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]        
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the item description.
        /// </summary>
        /// <value>The item description.</value>
        [Required]
        [Display(Name = "Description")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string ItemDescription { get; set; }

        /// <summary>
        /// Gets or sets the item price.
        /// </summary>
        /// <value>The item price.</value>
        [Required]
        [Display(Name = "Price")]
        [Range(0.00, double.MaxValue,
        ErrorMessage = "Price can not be less than zero")]        
        public decimal ItemPrice { get; set; }


        /// <summary>
        /// Gets or sets the quantity on hand.
        /// </summary>
        /// <value>The quantity on hand.</value>
        [Required]
        [Display(Name = "Quantity On Hand")]
        [Range(0, long.MaxValue, ErrorMessage = "Value can not be less than zero")]        
        public long? QuantityOnHand { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has physical inventory.
        /// </summary>
        /// <value><c>true</c> if this instance has physical inventory; otherwise, <c>false</c>.</value>
        [Required]
        [Display(Name = "Physical Inventory?")]
        public bool HasPhysicalInventory { get; set; }
        /// <summary>
        /// Gets or sets the service type identifier.
        /// </summary>
        /// <value>The service type identifier.</value>
        public long? ServiceTypeId { get; set; }
        /// <summary>
        /// Gets or sets the create user.
        /// </summary>
        /// <value>The create user.</value>
        public string CreateUser { get; set; }
        /// <summary>
        /// Gets or sets the create date time.
        /// </summary>
        /// <value>The create date time.</value>
        public DateTime CreateDateTime { get; set; }
        /// <summary>
        /// Gets or sets the update user.
        /// </summary>
        /// <value>The update user.</value>
        public string UpdateUser { get; set; }
        /// <summary>
        /// Gets or sets the update date time.
        /// </summary>
        /// <value>The update date time.</value>
        public DateTime UpdateDateTime { get; set; }
    }
}