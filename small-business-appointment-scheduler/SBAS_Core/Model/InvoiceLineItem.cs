// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 06-07-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="InvoiceLineItem.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>Definition of the InvoiceLineItem class.</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The SBAS_Core.Model namespace.
/// </summary>
namespace SBAS_Core.Model
{
    /// <summary>
    /// This is the definition of the InvoiceLineItem class, which inherits from the ModelBase class.
    /// </summary>
    public class InvoiceLineItem : ModelBase
    {
        /// <summary>
        /// Gets or sets the invoice line item identifier.
        /// </summary>
        /// <value>The invoice line item identifier.</value>
        public long InvoiceLineItemId { get; set; }
        
        /// <summary>
        /// Gets or sets the invoice identifier.
        /// </summary>
        /// <value>The invoice identifier.</value>
        public long InvoiceId { get; set; }
        
        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        /// <value>The item identifier.</value>
        public long ItemId { get; set; }
        
        /// <summary>
        /// Gets or sets the item line cost override in case the customer wishes to override the amount for the item as defined in the InventoryItem class.
        /// </summary>
        /// <value>The item line cost override.</value>
        public decimal ItemLineCostOverride { get; set; }
    }
}
