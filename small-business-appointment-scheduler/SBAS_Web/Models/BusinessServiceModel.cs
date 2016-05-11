// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Ye Peng
// Created          : 07-10-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-10-2014
// ***********************************************************************
// <copyright file="BusinessServiceModel.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>This class represent the Model for the View used for Business Services</summary>
// ***********************************************************************
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

/// <summary>
/// The Models namespace.
/// </summary>
namespace SBAS_Web.Models
{
    /// <summary>
    /// Class BusinessServiceModel.
    /// </summary>
    public class BusinessServiceModel
    {
        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>The name of the item.</value>
        [Required]
        [Display(Name = "Service Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the item description.
        /// </summary>
        /// <value>The item description.</value>
        [Required]
        [Display(Name = "Service Description")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string ItemDescription { get; set; }

        /// <summary>
        /// Gets or sets the item price.
        /// </summary>
        /// <value>The item price.</value>
        [Required]
        [Display(Name = "Service Price")]
        [Range(0.00, double.MaxValue,
        ErrorMessage = "Price can not be less than zero")]
        public decimal ItemPrice { get; set; }
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