// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Roy Glunt
// Created          : 07-09-2014
//
// Last Modified By : Roy Glunt
// Last Modified On : 08-10-2014
// ***********************************************************************
// <copyright file="AppointmentService.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
// This model is used by the kendo scheudler to procces requests as a MVVM to the controller.
//</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using SBAS_Core.Google;
using SBAS_Core.Model;
using Appointment = SBAS_DAL.Appointment;
using SBASUser = SBAS_DAL.SBASUser;

/// <summary>
/// The Models namespace.
/// </summary>
namespace SBAS_Web.Models
{
    /// <summary>
    /// Class AppointmentService.
    /// </summary>
    public class AppointmentService : ISchedulerEventService<AppointmentModel>
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>The name of the role.</value>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>IQueryable&lt;AppointmentModel&gt;.</returns>
        public IQueryable<AppointmentModel> GetAll()
        {
            var sbasUser = new SBASUser();
            var user = sbasUser.GetSBASUserByEmail(UserName);
            var appointments = new List<SBAS_Core.CustomClasses.AppointmentAndAddress>();
            var appointmentCompletedDal = new SBAS_DAL.Appointment();

            var appointmentDal = new SBAS_DAL.Appointment();
            appointments = RoleName == "Client"
                ? appointmentDal.GetAllClientAppointments(user.UserId)
                : appointmentDal.GetAllCustomerAppointments(user.UserId);
            var temp = appointments.ToList().Select(m => new AppointmentModel
            {
                AppointmentId = m.AppointmentId,
                Title = m.Title,
                Start = DateTime.SpecifyKind(m.Start, DateTimeKind.Utc),
                End = DateTime.SpecifyKind(m.End, DateTimeKind.Utc),
                StartTimezone = m.StartTimezone,
                EndTimezone = m.EndTimezone,
                Description = m.Description,
                IsAllDay = m.IsAllDay,
                RecurrenceID = m.RecurrenceId,
                RecurrenceException = m.RecurrenceException,
                RecurrenceRule = m.RecurrenceRule,
                CustomerId = m.CustomerId,
                ClientId = m.ClientId,
                ServiceTypeId = m.ServiceTypeId,
                UseClientAddress = m.UseClientAddress,
                AddressId = m.AddressId//,
                //AppointmentCompleted = m.AppointmentCompleted
            }).ToList();

            foreach (var appointment in temp)
            {
                AppointmentCompleted result = RoleName == "Client"
                    ? appointmentDal.GetClientAppointmentCompletedByDate(appointment.ClientId,
                        appointment.Start, appointment.End, appointment.AppointmentId)
                    : appointmentDal.GetCustomerAppointmentCompletedByDate(appointment.CustomerId,
                        appointment.Start, appointment.End, appointment.AppointmentId);

                appointment.AppointmentCompleted = result != null;
                appointment.AppointmentCompletedId = result != null ? result.AppointmentCompletedId : (long?)null;

                appointment.AppointmentState = "Default";
                if (result != null && result.IsCompleted)
                    appointment.AppointmentState = "Completed";
                if (result != null && result.IsReadyForInvoicing)
                    appointment.AppointmentState = "ReadyForInvoicing";
                if (result != null && result.IsInvoiced)
                    appointment.AppointmentState = "Invoiced";
            }

            var t = temp.ToList().Select(m => m).AsQueryable();

            return t;
        }

        /// <summary>
        /// Inserts the specified appointment.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <param name="modelState">State of the model.</param>
        public void Insert(AppointmentModel appointment, ModelStateDictionary modelState)
        {
            if (ValidateModel(appointment, modelState))
            {
                var sbasUser = new SBASUser();
                var user = sbasUser.GetSBASUserByEmail(UserName);
                var appointmentDal = new Appointment();
                var client = sbasUser.GetSBASUserById(appointment.ClientId);

                var newAppointment = new SBAS_Core.Model.Appointment();
                newAppointment.CreateDateTime = DateTime.Now;
                newAppointment.CreateUser = user.LastName + "," + user.FirstName;
                newAppointment.UpdateDateTime = DateTime.Now;
                newAppointment.UpdateUser = user.LastName + "," + user.FirstName;
                newAppointment.ClientId = appointment.ClientId;
                newAppointment.CustomerId = user.UserId;
                newAppointment.Description = appointment.Description;
                newAppointment.End = appointment.End;
                newAppointment.Start = appointment.Start;
                newAppointment.EndTimezone = appointment.EndTimezone;
                newAppointment.IsAllDay = appointment.IsAllDay;
                newAppointment.RecurrenceException = appointment.RecurrenceException;
                newAppointment.RecurrenceId = appointment.RecurrenceID;
                newAppointment.RecurrenceRule = appointment.RecurrenceRule;
                newAppointment.StartTimezone = appointment.StartTimezone;
                newAppointment.Title = appointment.Title;
                newAppointment.AddressId = client.AddressId;
                if (appointment.ServiceTypeId != null) newAppointment.ServiceTypeId = appointment.ServiceTypeId.Value;
                newAppointment.UseClientAddress = appointment.UseClientAddress;

                if (appointmentDal.ValidateAppointmentTimeRange(newAppointment,"Insert"))
                {
                    var appointmentResult = appointmentDal.AddAppointment(newAppointment);
                    appointment.AppointmentId = appointmentResult.AppointmentId;
                    appointment.AppointmentState = "Default";
                    appointment.CustomerId = user.UserId;
                }
                else
                {
                    modelState.AddModelError("errors", "There is not enough between these appointments to create this appointment. Either adjust time of appointment, or try different timeslot or day.");
                }
            }
        }

        /// <summary>
        /// Updates the specified appointment.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <param name="modelState">State of the model.</param>
        public void Update(AppointmentModel appointment, ModelStateDictionary modelState)
        {

            var sbasUser = new SBASUser();
            var user = sbasUser.GetSBASUserByEmail(UserName);
            var client = sbasUser.GetSBASUserById(appointment.ClientId);
            var appointmentDal = new Appointment();

            var updateAppointment = new SBAS_Core.Model.Appointment();
            updateAppointment.AppointmentId = appointment.AppointmentId;
            updateAppointment.CreateDateTime = DateTime.Now;
            updateAppointment.CreateUser = user.LastName + "," + user.FirstName;
            updateAppointment.UpdateDateTime = DateTime.Now;
            updateAppointment.UpdateUser = user.LastName + "," + user.FirstName;
            updateAppointment.ClientId = appointment.ClientId;
            updateAppointment.CustomerId = user.UserId;
            updateAppointment.Description = appointment.Description;
            updateAppointment.End = appointment.End;
            updateAppointment.Start = appointment.Start;
            updateAppointment.EndTimezone = appointment.EndTimezone;
            updateAppointment.IsAllDay = appointment.IsAllDay;
            updateAppointment.RecurrenceException = appointment.RecurrenceException;
            updateAppointment.RecurrenceId = appointment.RecurrenceID;
            updateAppointment.RecurrenceRule = appointment.RecurrenceRule;
            updateAppointment.StartTimezone = appointment.StartTimezone;
            updateAppointment.Title = appointment.Title;
            updateAppointment.AddressId = client.AddressId;
            if (appointment.ServiceTypeId != null) updateAppointment.ServiceTypeId = appointment.ServiceTypeId.Value;
            updateAppointment.UseClientAddress = appointment.UseClientAddress;

            AppointmentCompleted result = appointmentDal.GetCustomerAppointmentCompletedByDate(appointment.CustomerId,
                        appointment.Start, appointment.End, appointment.AppointmentId);
            appointment.AppointmentCompleted = result != null;
            appointment.AppointmentCompletedId = result != null ? result.AppointmentCompletedId : (long?)null;

            appointment.AppointmentState = "Default";
            if (result != null && result.IsCompleted)
                appointment.AppointmentState = "Completed";
            if (result != null && result.IsReadyForInvoicing)
                appointment.AppointmentState = "ReadyForInvoicing";
            if (result != null && result.IsInvoiced)
                appointment.AppointmentState = "Invoiced";

            if (appointment.AppointmentState != "Default")
            {
                string message = String.Empty;

                if (result != null && result.IsCompleted)
                    message = "This appointment can't be changed it has already been marked completed";
                if (result != null && result.IsReadyForInvoicing)
                    message = "This appointment can't be changed it has already been marked ready for invoicing";
                if (result != null && result.IsInvoiced)
                    message = "This appointment can't be changed it has already been Invoiced";
                
                modelState.AddModelError("errors", message);
            }
            else
            {
                if (appointmentDal.ValidateAppointmentTimeRange(updateAppointment,"Update"))
                {
                    appointmentDal.UpdateAppointment(updateAppointment);
                }
                else
                {
                    modelState.AddModelError("errors", "There is not enough between these appointments to move this appointment.");
                }
            }

        }

        /// <summary>
        /// Deletes the specified appointment.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <param name="modelState">State of the model.</param>
        public void Delete(AppointmentModel appointment, ModelStateDictionary modelState)
        {
            var sbasUser = new SBASUser();
            var user = sbasUser.GetSBASUserByEmail(UserName);


            if (appointment.ServiceTypeId != null)
            {
                var delAppointment = new SBAS_Core.Model.Appointment
                {
                    AppointmentId = appointment.AppointmentId,
                    CreateDateTime = DateTime.Now,
                    CreateUser = user.LastName + "," + user.FirstName,
                    UpdateDateTime = DateTime.Now,
                    UpdateUser = user.LastName + "," + user.FirstName,
                    ClientId = appointment.ClientId,
                    CustomerId = user.UserId,
                    Description = appointment.Description,
                    End = appointment.End,
                    Start = appointment.Start,
                    EndTimezone = appointment.EndTimezone,
                    IsAllDay = appointment.IsAllDay,
                    RecurrenceException = appointment.RecurrenceException,
                    RecurrenceId = appointment.RecurrenceID,
                    RecurrenceRule = appointment.RecurrenceRule,
                    StartTimezone = appointment.StartTimezone,
                    Title = appointment.Title,
                    AddressId = user.AddressId,
                    ServiceTypeId = appointment.ServiceTypeId.Value,
                    UseClientAddress = appointment.UseClientAddress
                };
                var appointmentDal = new Appointment();
                appointmentDal.DeleteAppointment(delAppointment);
            }
        }

        /// <summary>
        /// Validates the model.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <param name="modelState">State of the model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ValidateModel(AppointmentModel appointment, ModelStateDictionary modelState)
        {
            if (appointment.Start > appointment.End)
            {
                modelState.AddModelError("errors", "End date must be greater or equal to Start date.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {

        }
    }
}