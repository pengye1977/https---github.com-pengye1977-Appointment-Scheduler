// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 06-07-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-10-2014
// ***********************************************************************
// <copyright file="Appointment.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
// This class contains all the properties needed for an appointment that will be used 
// by that DAL and passed to the controller and that will then be translated into a 
// appointment vew model needed for the view
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
    /// Class Appointment.
    /// </summary>
    public class Appointment : ModelBase
    {
        /// <summary>
        /// Gets or sets the appointment identifier.
        /// </summary>
        /// <value>The appointment identifier.</value>
        public long AppointmentId { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>The start.</value>
        public DateTime Start { get; set; }
        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>The end.</value>
        public DateTime End { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is all day.
        /// </summary>
        /// <value><c>true</c> if this instance is all day; otherwise, <c>false</c>.</value>
        public bool IsAllDay { get; set; }
        /// <summary>
        /// Gets or sets the recurrence rule.
        /// </summary>
        /// <value>The recurrence rule.</value>
        public string RecurrenceRule { get; set; }
        /// <summary>
        /// Gets or sets the recurrence identifier.
        /// </summary>
        /// <value>The recurrence identifier.</value>
        public int? RecurrenceId { get; set; }
        /// <summary>
        /// Gets or sets the recurrence exception.
        /// </summary>
        /// <value>The recurrence exception.</value>
        public string RecurrenceException { get; set; }
        /// <summary>
        /// Gets or sets the start timezone.
        /// </summary>
        /// <value>The start timezone.</value>
        public string StartTimezone { get; set; }
        /// <summary>
        /// Gets or sets the end timezone.
        /// </summary>
        /// <value>The end timezone.</value>
        public string EndTimezone { get; set; }
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
        /// Gets or sets a value indicating whether [use client address].
        /// </summary>
        /// <value><c>true</c> if [use client address]; otherwise, <c>false</c>.</value>
        public bool UseClientAddress { get; set; }
        /// <summary>
        /// Gets or sets the address identifier.
        /// </summary>
        /// <value>The address identifier.</value>
        public long? AddressId { get; set; }
        /// <summary>
        /// Gets or sets the service type identifier.
        /// </summary>
        /// <value>The service type identifier.</value>
        public long ServiceTypeId { get; set; }

    }
}

