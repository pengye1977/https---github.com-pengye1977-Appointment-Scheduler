// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 06-07-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="Invoice.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>Definition of the Invoice class.</summary>
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
    /// This is the definition of the Invoice class, which inherits from the ModelBase class.
    /// </summary>
    public class Invoice : ModelBase
    {
        /// <summary>
        /// Gets or sets the invoice identifier.
        /// </summary>
        /// <value>The invoice identifier.</value>
        public long InvoiceId { get; set; }
        
        /// <summary>
        /// Gets or sets the invoice number.
        /// </summary>
        /// <value>The invoice number.</value>
        public string InvoiceNumber { get; set; }
        
        /// <summary>
        /// Gets or sets the amount due.
        /// </summary>
        /// <value>The amount due.</value>
        public decimal AmountDue { get; set; }
        
        /// <summary>
        /// Gets or sets the due date.
        /// </summary>
        /// <value>The due date.</value>
        public DateTime DueDate { get; set; }
        
        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        public string Comments { get; set; }
        
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>The customer identifier.</value>
        public long CustomerId { get; set; }
        
        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>The client identifier.</value>
        public long ClientID { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the invoice has been sent to the client.
        /// </summary>
        /// <value><c>true</c> if the invoice has been sent to client; otherwise, <c>false</c>.</value>
        public bool SentToClient { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether this instance is paid.
        /// </summary>
        /// <value><c>true</c> if the invoice is paid; otherwise, <c>false</c>.</value>
        public bool IsPaid { get; set; }
    }
}
