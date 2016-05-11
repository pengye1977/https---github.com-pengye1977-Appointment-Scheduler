// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Ye Peng
// Created          : 07-15-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-10-2014
// ***********************************************************************
// <copyright file="AppointmentCompleteModel.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
//   This model is used by the controller to send to the views for model completion.
//</summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBAS_Core.Model;

/// <summary>
/// The Models namespace.
/// </summary>
namespace SBAS_Web.Models
{
    /// <summary>
    /// Class AppointmentCompleteModel.
    /// </summary>
    public class AppointmentCompleteModel
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