// ***********************************************************************
// Assembly         : SBAS_DAL
// Author           : Ye Peng, Ye Peng
// Created          : 07-08-2014
//
// Last Modified By : Ye Peng, Ye Peng
// Last Modified On : 07-24-2014
// ***********************************************************************
// <copyright file="ServiceType.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>This class is the Database access class that communicates with our database server.
// This class is responsible for managing the data for the Service Type A service type is the table 
// that is stored in the Database, in this application a service type is a business service.</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPoco;
using SBAS_Core;

/// <summary>
/// The SBAS_DAL namespace.
/// </summary>
namespace SBAS_DAL
{
    /// <summary>
    /// Class ServiceType.
    /// </summary>
    public class ServiceType : Base
    {
        /// <summary>
        /// This method retrieves all of a user's business service types
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>List&lt;SBAS_Core.Model.ServiceType&gt;.</returns>
        /// <exception cref="System.ArgumentException">Cannot retrieve: userId cannot be less than or equal to zero</exception>
        public List<SBAS_Core.Model.ServiceType> GetListofUsersServiceTypesByUserId(long userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Cannot retrieve: userId cannot be less than or equal to zero");
            }

            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
               string sqlQueryString = "SELECT * FROM [ServiceType] WHERE CustomerId = @Id";

               var sqlbuilder = Sql.Builder;

                sqlbuilder.Append(sqlQueryString, new {Id = userId});

                var serviceType = db.Fetch<SBAS_Core.Model.ServiceType>(sqlbuilder);
                return serviceType;
            }
        }

        /// <summary>
        /// This method inserts a service type into the Database
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>SBAS_Core.Model.ServiceType.</returns>
        public SBAS_Core.Model.ServiceType CreateServiceType(SBAS_Core.Model.ServiceType serviceType)
        {
            DateTime currentDateTime = DateTime.Now;
            var serviceTypeToCreate = new SBAS_Core.Model.ServiceType()
            {
                CustomerId = serviceType.CustomerId,
                NameOfService = serviceType.NameOfService,
                Description = serviceType.Description,
                CreateUser = serviceType.CreateUser,
                CreateDateTime = currentDateTime,
                UpdateUser = serviceType.UpdateUser,
                UpdateDateTime = currentDateTime,
            };

            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                db.Insert("ServiceType", "ServiceTypeId", true, serviceTypeToCreate);
            }

            return serviceTypeToCreate;
        }

        /// <summary>
        /// This method retrieves a service type given a service type Id
        /// </summary>
        /// <param name="serviceTypeId">The service type identifier.</param>
        /// <returns>SBAS_Core.Model.ServiceType.</returns>
        /// <exception cref="System.ArgumentException">Cannot retrieve: ServiceTypeId cannot be less than or equal to zero</exception>
        public SBAS_Core.Model.ServiceType GetServiceTypeByServiceTypeId(long serviceTypeId)
        {
            if (serviceTypeId <= 0)
            {
                throw new ArgumentException("Cannot retrieve: ServiceTypeId cannot be less than or equal to zero");
            }

            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                string sqlQueryString = string.Format(@"SELECT * FROM ServiceType WHERE ServiceTypeId = {0}", serviceTypeId);
                var serviceType = db.Single<SBAS_Core.Model.ServiceType>(sqlQueryString);
                return serviceType;
            }
        }

        /// <summary>
        /// This method deletes a service type given a service type Id
        /// </summary>
        /// <param name="serviceTypeId">The service type identifier.</param>
        /// <exception cref="System.ArgumentException">
        /// Cannot delete: ServiceTypeId cannot be less than or equal to zero
        /// or
        /// </exception>
        public void DeleteServiceTypeByServiceTypeId(long serviceTypeId)
        {
            if (serviceTypeId <= 0)
            {
                throw new ArgumentException("Cannot delete: ServiceTypeId cannot be less than or equal to zero");
            }

            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                string sqlQueryString = String.Format("SELECT COUNT(*) FROM ServiceType WHERE ServiceTypeId = {0};", serviceTypeId);
                int serviceTypeCount = db.Single<int>(sqlQueryString);

                if(serviceTypeCount == 0)
                {
                    throw new ArgumentException(String.Format("Cannot delete: There is no service type with the Id of {0}", serviceTypeId));
                }

                db.Delete("ServiceType", "ServiceTypeId", new SBAS_Core.Model.ServiceType(), serviceTypeId);
            }                  
        }

        /// <summary>
        /// This method updates a service type
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>SBAS_Core.Model.ServiceType.</returns>
        public SBAS_Core.Model.ServiceType UpdateUsersServiceType(SBAS_Core.Model.ServiceType serviceType)
        {         
            var serviceTypeToUpdate = new SBAS_Core.Model.ServiceType()
            {
                ServiceTypeId = serviceType.ServiceTypeId,
                CustomerId = serviceType.CustomerId,
                NameOfService = serviceType.NameOfService,
                Description = serviceType.Description,
                CreateDateTime = serviceType.CreateDateTime,
                CreateUser = serviceType.CreateUser,
                UpdateDateTime = DateTime.Now,
                UpdateUser = serviceType.UpdateUser,
            };

            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                db.Update("ServiceType", "ServiceTypeId", serviceTypeToUpdate);
            }

            return serviceTypeToUpdate;
        }

        /// <summary>
        /// This method determines whether a particular business service is associated with an appointment.
        /// </summary>
        /// <param name="serviceTypeId">The service type identifier.</param>
        /// <returns><c>true</c> if the business service is assoicated with an appointment; otherwise, <c>false</c>.</returns>
        public bool IsBusinessServiceAssoicatedWithAppointment(long serviceTypeId)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                // Check to see if there is a business service associated with an appointment
                string sqlQueryString = string.Format("SELECT COUNT(*) FROM Appointment WHERE ServiceTypeId  = {0}", serviceTypeId);
                if (db.Single<int>(sqlQueryString) > 0)
                {
                    return true;
                }

                return false;
            }
        }

    } // End class
} // End namespace
