// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 06-07-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="Payment.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>Definition of the Payment class.</summary>
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
    /// This is the definition of the Payment class, which inherits from the ModelBase class.
    /// </summary>
    public class Payment : ModelBase
    {
        /// <summary>
        /// Gets or sets the payment identifier.
        /// </summary>
        /// <value>The payment identifier.</value>
        public long PaymentId { get; set; }
        
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>The customer identifier.</value>
        public long CustomerId { get; set; }
        
        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>The client identifier.</value>
        public long ClientId { get; set; }
        
        /// <summary>
        /// Gets or sets the invoice identifier.
        /// </summary>
        /// <value>The invoice identifier.</value>
        public long InvoiceId { get; set; }
        
        /// <summary>
        /// Gets or sets the payment amount.
        /// </summary>
        /// <value>The payment amount.</value>
        public decimal PaymentAmount { get; set; }
        
        /// <summary>
        /// Gets or sets the payment date.
        /// </summary>
        /// <value>The payment date.</value>
        public DateTime PaymentDate { get; set; }
        
        /// <summary>
        /// Gets or sets the payment method identifier.
        /// </summary>
        /// <value>The payment method identifier.</value>
        public long PaymentMethodId { get; set; }
    }
}
