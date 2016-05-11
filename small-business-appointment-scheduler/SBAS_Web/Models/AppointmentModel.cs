// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Ye Peng
// Created          : 07-05-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-10-2014
// ***********************************************************************
// <copyright file="AppointmentModel.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
// This model is used by the Appointment controller for passing appointments back to the view for displaying 
//</summary>
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
    /// Class AppointmentModel.
    /// </summary>
    public class AppointmentModel : ISchedulerEvent
    {
        /// <summary>
        /// The _start
        /// </summary>
        private DateTime _start;
        /// <summary>
        /// The _end
        /// </summary>
        private DateTime _end;
        /// <summary>
        /// The _use client address
        /// </summary>
        private bool _useClientAddress = true;

        /// <summary>
        /// Gets or sets the appointment identifier.
        /// </summary>
        /// <value>The appointment identifier.</value>
        public long AppointmentId { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [Display(Name = "Description")]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>The start.</value>
        [Required]

        [Display(Name = "Start")]
        public DateTime Start
        {
            get
            {
                return _start;
            }
            set
            {
                _start = value.ToUniversalTime();
            }
        }
        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>The end.</value>
        [Required]
        [DateGreaterThan(OtherField = "Start")]
        [Display(Name = "End")]
        public DateTime End
        {
            get
            {
                return _end;
            }
            set
            {
                _end = value.ToUniversalTime();
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is all day.
        /// </summary>
        /// <value><c>true</c> if this instance is all day; otherwise, <c>false</c>.</value>
        [Display(Name = "Is Allday")]
        public bool IsAllDay { get; set; }
        /// <summary>
        /// Gets or sets the recurrence rule.
        /// </summary>
        /// <value>The recurrence rule.</value>
        [Display(Name = "Recurrence")]
        public string RecurrenceRule { get; set; }
        /// <summary>
        /// Gets or sets the recurrence identifier.
        /// </summary>
        /// <value>The recurrence identifier.</value>
        public int? RecurrenceID { get; set; }
        /// <summary>
        /// Gets or sets the recurrence exception.
        /// </summary>
        /// <value>The recurrence exception.</value>
        public string RecurrenceException { get; set; }
        /// <summary>
        /// Gets or sets the start timezone.
        /// </summary>
        /// <value>The start timezone.</value>
        [Display(Name = "Start Timezone")]
        public string StartTimezone { get; set; }
        /// <summary>
        /// Gets or sets the end timezone.
        /// </summary>
        /// <value>The end timezone.</value>
        [Display(Name = "End Timezone")]
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
        [Display(Name = "Client")]
        public long ClientId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use client address].
        /// </summary>
        /// <value><c>true</c> if [use client address]; otherwise, <c>false</c>.</value>
        [DefaultValue(true)]
        [Display(Name = "Use Client Address")]
        public bool UseClientAddress
        {
            get { return _useClientAddress; }
            set { _useClientAddress = value; }
        }
        /// <summary>
        /// Gets or sets the service type identifier.
        /// </summary>
        /// <value>The service type identifier.</value>
        [Display(Name = "Service Type")]
        [Required]
        public long? ServiceTypeId { get; set; }
        /// <summary>
        /// Gets or sets the address identifier.
        /// </summary>
        /// <value>The address identifier.</value>
        public long? AddressId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [appointment completed].
        /// </summary>
        /// <value><c>true</c> if [appointment completed]; otherwise, <c>false</c>.</value>
        [Display(Name = "Appointment Completed")]
        public bool AppointmentCompleted { get; set; }
        /// <summary>
        /// Gets or sets the appointment completed identifier.
        /// </summary>
        /// <value>The appointment completed identifier.</value>
        public long? AppointmentCompletedId { get; set; }
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
        /// Gets or sets the state of the appointment.
        /// </summary>
        /// <value>The state of the appointment.</value>
        public string AppointmentState { get; set; }
        /// <summary>
        /// To the appointment.
        /// </summary>
        /// <returns>SBAS_Core.Model.Appointment.</returns>
        public SBAS_Core.Model.Appointment ToAppointment()
        {
            var appointment = new SBAS_Core.Model.Appointment
            {
                AppointmentId = AppointmentId,
                Title = Title,
                Description = Description,
                Start = Start,
                End = End,
                IsAllDay = IsAllDay,
                RecurrenceRule = RecurrenceRule,
                RecurrenceId = RecurrenceID,
                RecurrenceException = RecurrenceException,
                StartTimezone = StartTimezone,
                EndTimezone = EndTimezone,
                CustomerId = CustomerId,
                ClientId = ClientId,
                UseClientAddress = UseClientAddress,
                ServiceTypeId = ServiceTypeId.Value,
                AddressId = AddressId//,

            };
            return appointment;
        }
    }
}