// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 06-07-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-10-2014
// ***********************************************************************
// <copyright file="Inventory.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>This class is used in the Object-Relational mapping of data from our database server. 
// This class serves as the model for the data found in the Inventory table in our database</summary>
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
    /// Class Inventory.
    /// </summary>
    public class Inventory : ModelBase
    {
        /// <summary>
        /// Gets or sets the inventory identifier.
        /// </summary>
        /// <value>The inventory identifier.</value>
        public long InventoryId { get; set; }
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>The customer identifier.</value>
        public long CustomerId { get; set; }
        /// <summary>
        /// Gets or sets the last restocked date.
        /// </summary>
        /// <value>The last restocked date.</value>
        public DateTime? LastRestockedDate { get; set; }
        /// <summary>
        /// Gets or sets the last inventory inspection.
        /// </summary>
        /// <value>The last inventory inspection.</value>
        public DateTime? LastInventoryInspection { get; set; }
    }
}
