// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 07-13-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 07-24-2014
// ***********************************************************************
// <copyright file="AppointmentCompleted.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
//     This class is used by the DAL and will hold an appointment complted record. The DAL pass this 
//     back to the controller that requests it.
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
    /// Class AppointmentCompleted.
    /// </summary>
    public class AppointmentCompleted : ModelBase
    {
        /// <summary>
        /// Gets or sets the appointment completed identifier.
        /// </summary>
        /// <value>The appointment completed identifier.</value>
        public long AppointmentCompletedId { get; set; }
        /// <summary>
        /// Gets or sets the appointment identifier.
        /// </summary>
        /// <value>The appointment identifier.</value>
        public long AppointmentId { get; set; }
        /// <summary>
        /// Gets or sets the completion date time.
        /// </summary>
        /// <value>The completion date time.</value>
        public DateTime CompletionDateTime { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is completed.
        /// </summary>
        /// <value><c>true</c> if this instance is completed; otherwise, <c>false</c>.</value>
        public bool IsCompleted { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is ready for invoicing.
        /// </summary>
        /// <value><c>true</c> if this instance is ready for invoicing; otherwise, <c>false</c>.</value>
        public bool IsReadyForInvoicing { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is invoiced.
        /// </summary>
        /// <value><c>true</c> if this instance is invoiced; otherwise, <c>false</c>.</value>
        public bool IsInvoiced { get; set; }
        /// <summary>
        /// Gets or sets the invoice identifier.
        /// </summary>
        /// <value>The invoice identifier.</value>
        public long InvoiceId { get; set; }
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


    }
}
