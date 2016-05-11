// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 07-06-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 07-24-2014
// ***********************************************************************
// <copyright file="AppointmentAndAddress.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>This was created to provide a custom view for the appointment scheduler complex custom queries used in the DAL that has more then one table 
//          joins that gives the address to the appointment. This way the appointment information can be passed to the controllers that then will be passed
//          to the views.
// </summary>
// ***********************************************************************
using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// The CustomClasses namespace.
/// </summary>
namespace SBAS_Core.CustomClasses
{
    /// <summary>
    /// Class AppointmentAndAddress.
    /// </summary>
    public class AppointmentAndAddress
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
        //public bool AppointmentCompleted { get; set; }
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
        /// <summary>
        /// Gets or sets the address line1.
        /// </summary>
        /// <value>The address line1.</value>
        public string AddressLine1 { get; set; }
        /// <summary>
        /// Gets or sets the address line2.
        /// </summary>
        /// <value>The address line2.</value>
        public string AddressLine2 { get; set; }
        /// <summary>
        /// Gets or sets the selected city identifier.
        /// </summary>
        /// <value>The selected city identifier.</value>
        public long SelectedCityId { get; set; }
        /// <summary>
        /// Gets or sets the selected state identifier.
        /// </summary>
        /// <value>The selected state identifier.</value>
        public long SelectedStateId { get; set; }
        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>The zip code.</value>
        public string ZipCode { get; set; }
        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public decimal? Longitude { get; set; }
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public decimal? Latitude { get; set; }
    }
}
