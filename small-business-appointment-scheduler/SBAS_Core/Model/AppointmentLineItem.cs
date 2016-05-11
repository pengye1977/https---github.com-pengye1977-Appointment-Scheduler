// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 07-13-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 07-24-2014
// ***********************************************************************
// <copyright file="AppointmentLineItem.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
//   This class holds a single record of the Appointment line items of a completed appointment 
//   , this is used by the DAL to pass the record to the requesting controller. 
//   
//</summary>
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
    /// Class AppointmentLineItem.
    /// </summary>
    public class AppointmentLineItem : ModelBase
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
