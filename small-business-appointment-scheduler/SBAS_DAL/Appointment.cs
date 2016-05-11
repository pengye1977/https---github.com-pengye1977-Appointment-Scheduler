// ***********************************************************************
// Assembly         : SBAS_DAL
// Author           : Ye Peng
// Created          : 06-04-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-10-2014
// ***********************************************************************
// <copyright file="Appointment.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
//  This class has the database access methods used by the appointment scheduler. This class also has
//  This distance algorithm methods.
//</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NPoco;
using SBAS_Core.CustomClasses;
using SBAS_Core.Google;

/// <summary>
/// The SBAS_DAL namespace.
/// </summary>
namespace SBAS_DAL
{
    /// <summary>
    /// Class Appointment.
    /// </summary>
    public class Appointment : Base
    {


        /// <summary>
        /// Gets the appointment by appointment and client ids.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <returns>SBAS_Core.Model.Appointment.</returns>
        public SBAS_Core.Model.Appointment GetAppointmentByAppointmentAndClientIds(long clientId, long appointmentId)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlBuilder = Sql.Builder;
                sqlBuilder.Append(
                    "select * from [Appointment]  where [ClientId] = @ClientId and [AppointmentId] = @AppointmentId",
                    new { ClientId = clientId, AppointmentId = appointmentId });

                var customerAppointment = db.First<SBAS_Core.Model.Appointment>(sqlBuilder);

                return customerAppointment;
            }
        }

        /// <summary>
        /// Gets the appointments by customer identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="startDateTime">The start date time.</param>
        /// <param name="endDateTime">The end date time.</param>
        /// <returns>List&lt;SBAS_Core.Model.Appointment&gt;.</returns>
        public List<SBAS_Core.Model.Appointment> GetAppointmentsByCustomerId(long id, DateTime startDateTime, DateTime endDateTime)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append(
                    "SELECT * FROM [Appointment]  where [CustomerId] = @Id and [Start] >= @StartDate  and  [End] <=  @EndDate",
                    new
                    {
                        Id = id,
                        StartDate = startDateTime,
                        EndDate = endDateTime
                    });

                var appointments = db.Fetch<SBAS_Core.Model.Appointment>(sqlbuilder).ToList();


                return appointments;
            }

        }

        /// <summary>
        /// Gets the appointments by client identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="startDateTime">The start date time.</param>
        /// <param name="endDateTime">The end date time.</param>
        /// <returns>List&lt;SBAS_Core.Model.Appointment&gt;.</returns>
        public List<SBAS_Core.Model.Appointment> GetAppointmentsByClientId(long id, DateTime startDateTime, DateTime endDateTime)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append(
                    "SELECT * FROM [Appointment]  where [ClientId] = @Id and [Start] = @StartDate  and  [End] =  @EndDate",
                    new
                    {
                        Id = id,
                        StartDate = startDateTime,
                        EndDate = endDateTime
                    });

                var appointments = db.Fetch<SBAS_Core.Model.Appointment>(sqlbuilder);


                return appointments;
            }

        }

        /// <summary>
        /// Gets the appointment by appointment and customer ids.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <returns>SBAS_Core.Model.Appointment.</returns>
        public SBAS_Core.Model.Appointment GetAppointmentByAppointmentAndCustomerIds(long customerId, long appointmentId)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlBuilder = Sql.Builder;
                sqlBuilder.Append(
                    "select * from [Appointment]  where [CustomerId] = @CustomerId and [AppointmentId] = @AppointmentId",
                    new { CustomerId = customerId, AppointmentId = appointmentId });

                var customerAppointment = db.First<SBAS_Core.Model.Appointment>(sqlBuilder);

                return customerAppointment;
            }
        }
        /// <summary>
        /// Gets the appointment by identifier.
        /// </summary>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <returns>SBAS_Core.Model.Appointment.</returns>
        public SBAS_Core.Model.Appointment GetAppointmentById(long appointmentId)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlBuilder = Sql.Builder;
                sqlBuilder.Append(
                    "select * from [Appointment]  where [AppointmentId] = @AppointmentId",
                    new { AppointmentId = appointmentId });

                var customerAppointment = db.First<SBAS_Core.Model.Appointment>(sqlBuilder);

                return customerAppointment;
            }
        }
        /// <summary>
        /// Gets all customer appointments.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>List&lt;AppointmentAndAddress&gt;.</returns>
        public List<AppointmentAndAddress> GetAllCustomerAppointments(long id)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;

                var sql =
                    string.Format(
                        " SELECT APT.[AppointmentId] ,APT.[Title] ,APT.[Description] ,APT.[Start] ,APT.[End] {0}" +
                        "      ,APT.[IsAllDay] ,APT.[RecurrenceRule] ,APT.[RecurrenceId] ,APT.[RecurrenceException] {0}" +
                        "      ,APT.[StartTimezone] ,APT.[EndTimezone] ,APT.[CustomerId] ,APT.[ClientId] ,APT.[UseClientAddress] {0}" +
                        "      ,APT.[AddressId] ,APT.[ServiceTypeId] ,ADR.[AddressLine1] ,ADR.[AddressLine2] ,ADR.[CityId] {0}" +
                        "	  ,ADR.[StateId] ,ADR.[Latitude] ,ADR.[Longitude] {0}" +
                        "  FROM [Appointment] AS APT LEFT JOIN Address AS ADR ON APT.AddressId = ADR.AddressId {0}" +
                        "  WHERE APT.[CustomerId] = @Id {0}", Environment.NewLine);

                sqlbuilder.Append(sql, new { Id = id });

                var appointmentAndAddresses = db.Fetch<AppointmentAndAddress>(sqlbuilder).ToList();

                return appointmentAndAddresses;
            }

        }
        /// <summary>
        /// Gets all client appointments.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>List&lt;AppointmentAndAddress&gt;.</returns>
        public List<AppointmentAndAddress> GetAllClientAppointments(long id)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;

                var sql =
                    string.Format(
                        " SELECT APT.[AppointmentId] ,APT.[Title] ,APT.[Description] ,APT.[Start] ,APT.[End] {0}" +
                        "      ,APT.[IsAllDay] ,APT.[RecurrenceRule] ,APT.[RecurrenceId] ,APT.[RecurrenceException] {0}" +
                        "      ,APT.[StartTimezone] ,APT.[EndTimezone] ,APT.[CustomerId] ,APT.[ClientId] ,APT.[UseClientAddress] {0}" +
                        "      ,APT.[AddressId] ,APT.[ServiceTypeId] ,ADR.[AddressLine1] ,ADR.[AddressLine2] ,ADR.[CityId] {0}" +
                        "	  ,ADR.[StateId] ,ADR.[Latitude] ,ADR.[Longitude] {0}" +
                        "  FROM [Appointment] AS APT LEFT JOIN Address AS ADR ON APT.AddressId = ADR.AddressId {0}" +
                        "  WHERE APT.[ClientId] = @Id {0}", Environment.NewLine);

                sqlbuilder.Append(sql, new { Id = id });

                var appointmentAndAddresses = db.Fetch<AppointmentAndAddress>(sqlbuilder).ToList();

                return appointmentAndAddresses;
            }

        }

        /// <summary>
        /// Adds the appointment.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns>SBAS_Core.Model.Appointment.</returns>
        public SBAS_Core.Model.Appointment AddAppointment(SBAS_Core.Model.Appointment appointment)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    db.Insert("Appointment", "AppointmentId", appointment);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }
            return appointment;
        }

        /// <summary>
        /// Updates the appointment.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns>SBAS_Core.Model.Appointment.</returns>
        public SBAS_Core.Model.Appointment UpdateAppointment(SBAS_Core.Model.Appointment appointment)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                db.Update("Appointment", "AppointmentId", appointment);

            }
            return appointment;
        }

        /// <summary>
        /// Deletes the appointment.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool DeleteAppointment(SBAS_Core.Model.Appointment appointment)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                bool result;
                try
                {
                    db.Delete("Appointment", "AppointmentId", appointment);
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the appointments by date range.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="startDateTime">The start date time.</param>
        /// <param name="endDateTime">The end date time.</param>
        /// <returns>List&lt;SBAS_Core.Model.Appointment&gt;.</returns>
        public List<SBAS_Core.Model.Appointment> GetAppointmentsByDateRange(long customerId, DateTime startDateTime, DateTime endDateTime)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;

                const string sqlstr = " SELECT * FROM [SWENG500_User].[Appointment] " +
                                      " WHERE CUSTOMERID = @CustomerId AND " +
                                      " ([START] >= @StartDate  OR [START] <= @StartDate ) " +
                                      " AND [START] <= @EndDate " +
                                      " AND [END] >= @StartDate  AND " +
                                      " ([END] >= @EndDate OR [END] <= @EndDate) " +
                                      " OR [RecurrenceRule] IS NOT NULL " +
                                      " ORDER BY [END] DESC ";

                sqlbuilder.Append(sqlstr,
                    new
                    {
                        CustomerId = customerId,
                        StartDate = startDateTime,
                        EndDate = endDateTime
                    });

                var result = db.Fetch<SBAS_Core.Model.Appointment>(sqlbuilder).ToList();


                return result;
            }
        }

        /// <summary>
        /// Gets the map information by customer and date.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="startDateTime">The start date time.</param>
        /// <param name="endDateTime">The end date time.</param>
        /// <returns>List&lt;MapInfo&gt;.</returns>
        public List<MapInfo> GetMapInfoByCustomerAndDate(long customerId, DateTime startDateTime, DateTime endDateTime)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;

                const string sqlstr = " SELECT  A.AppointmentId,A.Start, A.[End], " +
                                      " U.CompanyName, U.FirstName + ' ' + U.LastName As FullName, U.[PhoneNumber],U.[MobileNumber], U.[FaxNumber], " +
                                      " ADR.[AddressLine1],adr.[AddressLine2], C.City, S.[StateAbbreviation], ADR.[Latitude], ADR.[Longitude] " +
                                      " FROM [Appointment] as a LEFT JOIN [Address] ADR ON A.AddressId = ADR.AddressId " +
                                      " LEFT JOIN Lut_City AS C ON ADR.CityId = C.CityId " +
                                      " LEFT JOIN Lut_State AS S ON ADR.StateId = S.StateId " +
                                      " LEFT JOIN SBASUser AS U ON A.ClientId = U.UserId " +
                                      " WHERE a.CUSTOMERID = @CustomerId AND  " +
                                      " (a.[START] >= @StartDate  OR a.[START] <= @StartDate )  " +
                                      " AND a.[START] <= @EndDate " +
                                      " AND a.[END] >= @StartDate   " +
                                      " AND ([END] >= @EndDate OR a.[END] <= @EndDate)  " +
                                      " OR a.[RecurrenceRule] IS NOT NULL   " +
                                      " ORDER BY [END] DESC ";

                sqlbuilder.Append(sqlstr,
                    new
                    {
                        CustomerId = customerId,
                        StartDate = startDateTime,
                        EndDate = endDateTime
                    });

                var result = db.Fetch<MapInfo>(sqlbuilder).ToList();


                return result;
            }
        }

        /// <summary>
        /// Insert_s the appointment completed.
        /// </summary>
        /// <param name="appointmentCompleted">The appointment completed.</param>
        /// <returns>SBAS_Core.Model.AppointmentCompleted.</returns>
        public SBAS_Core.Model.AppointmentCompleted Insert_AppointmentCompleted(SBAS_Core.Model.AppointmentCompleted appointmentCompleted)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    db.Insert("AppointmentCompleted", "AppointmentCompletedId", appointmentCompleted);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
                return appointmentCompleted;
            }
        }

        /// <summary>
        /// Delete_s the appointment completed.
        /// </summary>
        /// <param name="appointmentCompleted">The appointment completed.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Delete_AppointmentCompleted(SBAS_Core.Model.AppointmentCompleted appointmentCompleted)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                bool result;
                try
                {
                    db.Delete("AppointmentCompleted", "AppointmentCompletedId", appointmentCompleted);
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }
                return result;
            }
        }

        /// <summary>
        /// Update_s the appointment completed.
        /// </summary>
        /// <param name="appointmentCompleted">The appointment completed.</param>
        /// <returns>SBAS_Core.Model.AppointmentCompleted.</returns>
        public SBAS_Core.Model.AppointmentCompleted Update_AppointmentCompleted(
            SBAS_Core.Model.AppointmentCompleted appointmentCompleted)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                db.Update("AppointmentCompleted", "AppointmentCompletedId", appointmentCompleted);

            }
            return appointmentCompleted;
        }

        /// <summary>
        /// Gets the appointment completed by complete identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>SBAS_Core.Model.AppointmentCompleted.</returns>
        public SBAS_Core.Model.AppointmentCompleted GetAppointmentCompletedByCompleteId(long id)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append(
                    "SELECT * FROM [AppointmentCompleted]  where [AppointmentCompletedId] = @Id",
                    new
                    {
                        Id = id
                    });

                var appointmentCompleted = db.Single<SBAS_Core.Model.AppointmentCompleted>(sqlbuilder);


                return appointmentCompleted;
            }
        }

        /// <summary>
        /// Gets the appointment completed apt identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="completedDateTime">The completed date time.</param>
        /// <returns>SBAS_Core.Model.AppointmentCompleted.</returns>
        public SBAS_Core.Model.AppointmentCompleted GetAppointmentCompletedAptId(long id, DateTime completedDateTime)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append(
                    "SELECT * FROM [AppointmentCompleted]  where [AppointmentId] = @Id and CompletionDateTime = @CompletionDateTime ",
                    new
                    {
                        Id = id,
                        CompletionDateTime = completedDateTime
                    });

                var appointmentCompleted = db.FirstOrDefault<SBAS_Core.Model.AppointmentCompleted>(sqlbuilder);


                return appointmentCompleted;
            }
        }

        /// <summary>
        /// Gets the appointment completed by appt identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>SBAS_Core.Model.AppointmentCompleted.</returns>
        public SBAS_Core.Model.AppointmentCompleted GetAppointmentCompletedByApptID(long id)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append(
                    "SELECT * FROM [AppointmentCompleted]  where [AppointmentId] = @Id",
                    new
                    {
                        Id = id
                    });

                var appointmentCompleted = db.Single<SBAS_Core.Model.AppointmentCompleted>(sqlbuilder);


                return appointmentCompleted;
            }
        }

        /// <summary>
        /// Appointments the completed record exists.
        /// </summary>
        /// <param name="completedDate">The completed date.</param>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool AppointmentCompletedRecordExists(DateTime completedDate, long appointmentId)
        {

            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append(
                    "SELECT count(*) HasCount FROM [AppointmentCompleted]  where AppointmentId = @AppointmentId and [CompletionDateTime]  = @CompletedDate ",
                    new
                    {
                        CompletedDate = completedDate,
                        AppointmentId = appointmentId
                    });

                var hasCount = db.ExecuteScalar<int>(sqlbuilder);


                return hasCount != 0;
            }
        }

        /// <summary>
        /// Determines whether [is appointment completed] [the specified completed date].
        /// </summary>
        /// <param name="completedDate">The completed date.</param>
        /// <param name="invoiced">if set to <c>true</c> [invoiced].</param>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <returns><c>true</c> if [is appointment completed] [the specified completed date]; otherwise, <c>false</c>.</returns>
        public bool IsAppointmentCompleted(DateTime completedDate, bool invoiced, long appointmentId)
        {

            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append(
                    "SELECT count(*) HasCount FROM [AppointmentCompleted]  where [IsInvoiced] = @Invoiced and AppointmentId = @AppointmentId and [CompletionDateTime]  = @CompletedDate ",
                    new
                    {
                        Invoiced = invoiced,
                        CompletedDate = completedDate,
                        AppointmentId = appointmentId
                    });

                var hasCount = db.ExecuteScalar<int>(sqlbuilder);


                return hasCount != 0;
            }
        }

        /// <summary>
        /// Gets the customer appointment completed by date.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <returns>SBAS_Core.Model.AppointmentCompleted.</returns>
        public SBAS_Core.Model.AppointmentCompleted GetCustomerAppointmentCompletedByDate(long customerId, DateTime startDate, DateTime endDate, long appointmentId)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append(
                    "SELECT * FROM [AppointmentCompleted]  where CustomerId = @CustomerId and AppointmentId = @AppointmentId and ([CompletionDateTime] between @StartDate and @EndDate) ",
                    new
                    {
                        CustomerId = customerId,
                        StartDate = startDate,
                        EndDate = endDate,
                        AppointmentId = appointmentId
                    });
                var appointmentCompleted = db.FirstOrDefault<SBAS_Core.Model.AppointmentCompleted>(sqlbuilder);


                return appointmentCompleted;
            }
        }
        /// <summary>
        /// Gets the client appointment completed by date.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <returns>SBAS_Core.Model.AppointmentCompleted.</returns>
        public SBAS_Core.Model.AppointmentCompleted GetClientAppointmentCompletedByDate(long clientId, DateTime startDate, DateTime endDate,  long appointmentId)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append(
                    "SELECT * FROM [AppointmentCompleted]  where ClientId = @ClientId and AppointmentId = @AppointmentId and ([CompletionDateTime] between @StartDate and @EndDate) ",
                    new
                    {
                        ClientId = clientId,
                        StartDate = startDate,
                        EndDate = endDate,
                        AppointmentId = appointmentId
                    });

                var appointmentCompleted = db.FirstOrDefault<SBAS_Core.Model.AppointmentCompleted>(sqlbuilder);


                return appointmentCompleted;
            }
        }

        /// <summary>
        /// Gets the client appointment completed by invoice identifier.
        /// </summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>SBAS_Core.Model.AppointmentCompleted.</returns>
        public SBAS_Core.Model.AppointmentCompleted GetClientAppointmentCompletedByInvoiceID(long invoiceId)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append(string.Format("SELECT * FROM AppointmentCompleted WHERE InvoiceId = {0} ", invoiceId));

                var appointmentCompleted = db.FirstOrDefault<SBAS_Core.Model.AppointmentCompleted>(sqlbuilder);


                return appointmentCompleted;
            }
        }

        /// <summary>
        /// Gets all appointment completed by date and invoiced.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="invoiced">if set to <c>true</c> [invoiced].</param>
        /// <returns>List&lt;SBAS_Core.Model.AppointmentCompleted&gt;.</returns>
        public List<SBAS_Core.Model.AppointmentCompleted> GetAllAppointmentCompletedByDateAndInvoiced(long customerId, DateTime startDate, DateTime endDate, bool invoiced)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append(
                    "SELECT * FROM [AppointmentCompleted]  where [IsInvoiced] = @Invoiced and CustomerId = @CustomerId and ([CompletionDateTime] between @StartDate and @EndDate) ",
                    new
                    {
                        Invoiced = invoiced,
                        CustomerId = customerId,
                        StartDate = startDate,
                        EndDate = endDate
                    });

                var appointmentCompleted = db.Fetch<SBAS_Core.Model.AppointmentCompleted>(sqlbuilder);


                return appointmentCompleted;
            }
        }

        /// <summary>
        /// Gets the completed appointments by user.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="isReadyForInvoicing">if set to <c>true</c> [is ready for invoicing].</param>
        /// <param name="isInvoiced">if set to <c>true</c> [is invoiced].</param>
        /// <returns>List&lt;SBAS_Core.Model.AppointmentCompleted&gt;.</returns>
        public List<SBAS_Core.Model.AppointmentCompleted> GetCompletedAppointmentsByUser(long customerId, bool isReadyForInvoicing, bool isInvoiced)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append(
                    "SELECT * FROM [AppointmentCompleted] WHERE [CustomerID] = @CustomerId and [IsReadyForInvoicing] = @IsReadyForInvoicing and [IsInvoiced] = @IsInvoiced",
                    new
                    {
                        CustomerId = customerId,
                        IsReadyForInvoicing = isReadyForInvoicing,
                        IsInvoiced = isInvoiced
                    });

                var appointmentsCompleted = db.Fetch<SBAS_Core.Model.AppointmentCompleted>(sqlbuilder);


                return appointmentsCompleted;
            }
        }


        /// <summary>
        /// Insert_s the appointment line item.
        /// </summary>
        /// <param name="appointmentLineItem">The appointment line item.</param>
        /// <returns>SBAS_Core.Model.AppointmentLineItem.</returns>
        public SBAS_Core.Model.AppointmentLineItem Insert_AppointmentLineItem(SBAS_Core.Model.AppointmentLineItem appointmentLineItem)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    db.Insert("AppointmentLineItem", "AppointmentLineItemId", appointmentLineItem);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
                return appointmentLineItem;
            }
        }

        /// <summary>
        /// Delete_s the appointment line item.
        /// </summary>
        /// <param name="appointmentLineItem">The appointment line item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Delete_AppointmentLineItem(SBAS_Core.Model.AppointmentLineItem appointmentLineItem)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                bool result;
                try
                {
                    db.Delete("AppointmentLineItem", "AppointmentLineItemId", appointmentLineItem);
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }
                return result;
            }
        }

        /// <summary>
        /// Update_s the appointment line item.
        /// </summary>
        /// <param name="appointmentLineItem">The appointment line item.</param>
        /// <returns>SBAS_Core.Model.AppointmentLineItem.</returns>
        public SBAS_Core.Model.AppointmentLineItem Update_AppointmentLineItem(
            SBAS_Core.Model.AppointmentLineItem appointmentLineItem)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                db.Update("AppointmentLineItem", "AppointmentLineItemId", appointmentLineItem);

            }
            return appointmentLineItem;
        }

        /// <summary>
        /// Gets the appointment line items by completed appointment identifier.
        /// </summary>
        /// <param name="completedAppointmentId">The completed appointment identifier.</param>
        /// <returns>List&lt;SBAS_Core.Model.AppointmentLineItem&gt;.</returns>
        public List<SBAS_Core.Model.AppointmentLineItem> GetAppointmentLineItemsByCompletedAppointmentId(long completedAppointmentId)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append(
                    "SELECT * FROM [AppointmentLineItem]  where [AppointmentCompletedId] = @Id",
                    new
                    {
                        Id = completedAppointmentId
                    });

                var appointmentLineItems = db.Fetch<SBAS_Core.Model.AppointmentLineItem>(sqlbuilder);


                return appointmentLineItems;
            }
        }

        /// <summary>
        /// Gets the appointment line item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>SBAS_Core.Model.AppointmentLineItem.</returns>
        public SBAS_Core.Model.AppointmentLineItem GetAppointmentLineItem(long id)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append(
                    "SELECT * FROM [AppointmentLineItem]  where [AppointmentLineItemId] = @Id",
                    new
                    {
                        Id = id
                    });

                var appointmentLineItem = db.Single<SBAS_Core.Model.AppointmentLineItem>(sqlbuilder);


                return appointmentLineItem;
            }
        }


        /// <summary>
        /// Validates the appointment time range.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <param name="operationType">Type of the operation.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ValidateAppointmentTimeRange(SBAS_Core.Model.Appointment appointment,string operationType)
        {
            var prevAndNextAppointment = GetPreviousAndNextAppointment(appointment.CustomerId, appointment, operationType);

            return IsAppointmentCreatable(prevAndNextAppointment["PREV"], appointment, prevAndNextAppointment["NEXT"]);
        }

        /// <summary>
        /// Determines whether [is appointment creatable] [the specified appointment a].
        /// </summary>
        /// <param name="appointmentA">The appointment a.</param>
        /// <param name="appointmentB">The appointment b.</param>
        /// <param name="appointmentC">The appointment c.</param>
        /// <returns><c>true</c> if [is appointment creatable] [the specified appointment a]; otherwise, <c>false</c>.</returns>
        public bool IsAppointmentCreatable(SBAS_Core.Model.Appointment appointmentA, SBAS_Core.Model.Appointment appointmentB, SBAS_Core.Model.Appointment appointmentC)
        {
            // These Steps below are a seperate method called IsAppointmentCreatable
            // Step 1 Get TimeSpan from A Endtime to C StartTime
            // Step 2 Get DistanceMatrixResponse from A to B 
            // Step 3 Get DistanceMatrixResponse from B to C
            // Step 4 Add Step 5 TimeSpan with step 6 TimeSpan and New apointment Start/End TimeSpan
            // Step 5 return true if step 7 is less then or equal to step 4 else false

            var addressDal = new SBAS_DAL.Address();
            TimeSpan AtoC;
            TimeSpan BtoC;
            TimeSpan AtoB;
            double AtoCTotalMinutes;
            double AtoBTotalMinutes;
            double BtoCTotalMinutes;
            double TotalMinutesNeeded;
            SBAS_Core.Model.Address addressA;
            SBAS_Core.Model.Address addressB;
            SBAS_Core.Model.Address addressC;
            SBAS_Core.Google.DistanceMatrixResponse dmAtoB;
            DistanceMatrixResponse dmBtoC;


            TimeSpan appointmentBTimeSpan = appointmentB.End - appointmentB.Start;

            if ((appointmentA == null) && (appointmentC == null))
            {
                return true;
            }
            if ((appointmentA != null) && (appointmentC != null))
            {

                addressA = addressDal.GetAddressById(appointmentA.AddressId.Value);
                addressB = addressDal.GetAddressById(appointmentB.AddressId.Value);
                addressC = addressDal.GetAddressById(appointmentC.AddressId.Value);

                TimeSpan timeSpanAB = appointmentB.Start - appointmentA.End;
                TimeSpan timeSpanBC = appointmentC.Start - appointmentB.End;
                TimeSpan timeSpanB = appointmentB.End - appointmentB.Start;
                double timespantotal = timeSpanAB.TotalMinutes + timeSpanBC.TotalMinutes + timeSpanB.TotalMinutes;

                dmAtoB = DistanceMatrix.GetDrivingDistanceInMiles(addressA, addressB);
                dmBtoC = DistanceMatrix.GetDrivingDistanceInMiles(addressB, addressC);

                double dmtotalminutes = DistanceMatrix.GetTotalMinutes(dmAtoB) + DistanceMatrix.GetTotalMinutes(dmBtoC) +
                                        timeSpanB.TotalMinutes;

                if (
                    (timeSpanAB.TotalMinutes > DistanceMatrix.GetTotalMinutes(dmAtoB)) &&
                    (timeSpanBC.TotalMinutes > DistanceMatrix.GetTotalMinutes(dmBtoC)) &&
                    (timespantotal > dmtotalminutes)
                    )
                {
                    return true; }

                return false;

            }
            if (appointmentA == null)
            {
                BtoC = appointmentC.Start - appointmentB.End;

                addressB = addressDal.GetAddressById(appointmentB.AddressId.Value);
                addressC = addressDal.GetAddressById(appointmentC.AddressId.Value);
                dmBtoC = DistanceMatrix.GetDrivingDistanceInMiles(addressB, addressC);
                BtoCTotalMinutes = DistanceMatrix.GetTotalMinutes(dmBtoC);

                return BtoCTotalMinutes <= BtoC.TotalMinutes;
            }

            AtoB = appointmentB.Start - appointmentA.End;
            addressA = addressDal.GetAddressById(appointmentA.AddressId.Value);
            addressB = addressDal.GetAddressById(appointmentB.AddressId.Value);

            dmAtoB = DistanceMatrix.GetDrivingDistanceInMiles(addressA, addressB);
            AtoBTotalMinutes = DistanceMatrix.GetTotalMinutes(dmAtoB);

            return AtoBTotalMinutes <= AtoB.TotalMinutes;

        }

        /// <summary>
        /// Gets the previous and next appointment.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="appointment">The appointment.</param>
        /// <param name="operationType">Type of the operation.</param>
        /// <returns>Dictionary&lt;System.String, SBAS_Core.Model.Appointment&gt;.</returns>
        public Dictionary<string, SBAS_Core.Model.Appointment> GetPreviousAndNextAppointment(long customerId, SBAS_Core.Model.Appointment appointment, string operationType)
        {
            var result = new Dictionary<string, SBAS_Core.Model.Appointment>();
            // Get the en-US culture.
            CultureInfo ci = new CultureInfo("en-US");
            // Get the DateTimeFormatInfo for the en-US culture.
            DateTimeFormatInfo dtfi = ci.DateTimeFormat;
            // 1.) Call and get list of appointments for the whole day
            var startDateTime = string.Format("{0}/{1}/{2} {3}:{4}:{5}", appointment.Start.Month, appointment.Start.Day, appointment.Start.Year, 0, 0, 01);
            var endDateTime = string.Format("{0}/{1}/{2} {3}:{4}:{5}", appointment.End.Month, appointment.End.Day, appointment.End.Year, 23, 59, 59);

            var appointmentDal = new SBAS_DAL.Appointment();
            var appointments = appointmentDal.GetAppointmentsByDateRange(customerId, Convert.ToDateTime(startDateTime), Convert.ToDateTime(endDateTime));
            if (operationType == "Update")
            {
                //var originalAppointment = appointmentDal.GetAppointmentById(appointment.AppointmentId);
                var itemToRemove = appointments.SingleOrDefault(r => r.AppointmentId == appointment.AppointmentId);
                if (itemToRemove != null)
                    appointments.Remove(itemToRemove);
            }
            // 2.) create list non recurring appointments
            var nonRecurringAppointments = appointments.Select(m => m).Where(m => m.RecurrenceRule == null).ToList();
            // 3.) create list recurring appointments
            var recurringAppointments = appointments.Select(m => m).Where(m => m.RecurrenceRule != null).ToList();
            // 4.) check each recurring and add to non recurring list if it is on the same day
            var newStartDateTimeShortestDayName = dtfi.GetShortestDayName(appointment.Start.DayOfWeek);
            foreach (var recurringAppointment in recurringAppointments)
            {
                var pair = recurringAppointment.RecurrenceRule.Split(';');
                Dictionary<string, string> recurrDictionary =
                      pair.ToDictionary(s => s.Split('=')[0], s => s.Split('=')[1]);
                if (recurrDictionary.ContainsKey("FREQ"))
                {
                    DateTime updatedStartDateTime;
                    DateTime updatedEndtDateTime;

                    switch (recurrDictionary["FREQ"])
                    {
                        case "DAILY":
                            // add to the recurringApoointments 
                            // after changing date to be the same date but leave the time portion alone
                            updatedStartDateTime = new DateTime(appointment.Start.Year, appointment.Start.Month,
                               appointment.Start.Day, recurringAppointment.Start.Hour,
                               recurringAppointment.Start.Minute, recurringAppointment.Start.Second,
                               recurringAppointment.Start.Millisecond);
                            updatedEndtDateTime = new DateTime(appointment.End.Year, appointment.End.Month,
                               appointment.End.Day, recurringAppointment.End.Hour,
                               recurringAppointment.End.Minute, recurringAppointment.End.Second,
                               recurringAppointment.End.Millisecond);

                            recurringAppointment.Start = updatedStartDateTime;
                            recurringAppointment.End = updatedEndtDateTime;
                            nonRecurringAppointments.Add(recurringAppointment);
                            break;
                        case "WEEKLY":
                            // check that day of week is equal to appointments day of week if so add to the recurringApoointments 
                            // after changing date to be the same date but leave the time portion alone
                            if (newStartDateTimeShortestDayName.ToUpper() == recurrDictionary["BYDAY"])
                            {
                                updatedStartDateTime = new DateTime(appointment.Start.Year, appointment.Start.Month,
                                appointment.Start.Day, recurringAppointment.Start.Hour,
                                recurringAppointment.Start.Minute, recurringAppointment.Start.Second,
                                recurringAppointment.Start.Millisecond);
                                updatedEndtDateTime = new DateTime(appointment.End.Year, appointment.End.Month,
                                    appointment.End.Day, recurringAppointment.End.Hour,
                                    recurringAppointment.End.Minute, recurringAppointment.End.Second,
                                    recurringAppointment.End.Millisecond);

                                recurringAppointment.Start = updatedStartDateTime;
                                recurringAppointment.End = updatedEndtDateTime;
                                nonRecurringAppointments.Add(recurringAppointment);
                            }
                            break;
                        case "MONTHLY":
                            // check to see of the number of the day equals the number of the appointment day if so at to recurringApoointments
                            // after changing date to be the same date but leave the time portion alone
                            if (appointment.Start.Day == recurringAppointment.Start.Day)
                            {
                                updatedStartDateTime = new DateTime(appointment.Start.Year, appointment.Start.Month,
                                    appointment.Start.Day, recurringAppointment.Start.Hour,
                                    recurringAppointment.Start.Minute, recurringAppointment.Start.Second,
                                    recurringAppointment.Start.Millisecond);
                                updatedEndtDateTime = new DateTime(appointment.End.Year, appointment.End.Month,
                                    appointment.End.Day, recurringAppointment.End.Hour, recurringAppointment.End.Minute,
                                    recurringAppointment.End.Second, recurringAppointment.End.Millisecond);

                                recurringAppointment.Start = updatedStartDateTime;
                                recurringAppointment.End = updatedEndtDateTime;
                                nonRecurringAppointments.Add(recurringAppointment);
                            }
                            break;
                        case "YEARLY":
                            if ((appointment.Start.Month == recurringAppointment.Start.Month) &&
                               (appointment.Start.Day == recurringAppointment.Start.Day))
                            {
                                updatedStartDateTime = new DateTime(appointment.Start.Year, appointment.Start.Month,
                                    appointment.Start.Day, recurringAppointment.Start.Hour,
                                    recurringAppointment.Start.Minute, recurringAppointment.Start.Second,
                                    recurringAppointment.Start.Millisecond);
                                updatedEndtDateTime = new DateTime(appointment.End.Year, appointment.End.Month,
                                    appointment.End.Day, recurringAppointment.End.Hour, recurringAppointment.End.Minute,
                                    recurringAppointment.End.Second, recurringAppointment.End.Millisecond);

                                recurringAppointment.Start = updatedStartDateTime;
                                recurringAppointment.End = updatedEndtDateTime;
                                nonRecurringAppointments.Add(recurringAppointment);
                            }
                            break;
                    }
                }
            }
            var finalDateRangeAppointments = nonRecurringAppointments.Select(m => m).OrderBy(m => m.Start).ToList();

            var start = Convert.ToDateTime(startDateTime);
            var end = Convert.ToDateTime(endDateTime);

            var previousAppointment = finalDateRangeAppointments.OrderByDescending(m => m.Start).FirstOrDefault(t => (t.Start >= start || t.Start <= start) && t.Start <= appointment.Start
                                                                                     && t.End >= start && (t.End >= appointment.Start || t.End <= appointment.Start));
            var nextAppointment =
                finalDateRangeAppointments.Where(
                    t => (t.Start >= appointment.End || t.Start <= appointment.End) && t.Start <= end
                         && t.End >= appointment.End && (t.End >= end || t.End <= end))
                    .OrderBy(m => m.Start)
                    .FirstOrDefault();
            result.Add("PREV", previousAppointment);
            result.Add("NEXT", nextAppointment);
            // 5.) create
            /*
             
             
                         return GetAll().Where(t => (t.Start >= start || t.Start <= start) && t.Start <= end
                && t.End >= start && (t.End >= end || t.End <= end) || t.RecurrenceRule != null);  
             */



            return result;
        }

    }
}


