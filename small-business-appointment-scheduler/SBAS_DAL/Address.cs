// ***********************************************************************
// Assembly         : SBAS_DAL
// Author           : Ye Peng
// Created          : 06-10-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="Address.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPoco;
using SBAS_Core.Model;
using SBAS_Core.CustomClasses;

/// <summary>
/// The SBAS_DAL namespace.
/// </summary>
namespace SBAS_DAL
{
    /// <summary>
    /// Class Address.
    /// </summary>
    public class Address : Base
    {
        /// <summary>
        /// Creates the address.
        /// </summary>
        /// <param name="aAddress">A address.</param>
        /// <returns>System.Int64.</returns>
        public long CreateAddress(SBAS_Core.Model.Address aAddress)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;

                db.Insert("Address", "AddressId", true, aAddress);

                return aAddress.AddressId;
            }
        }

        /// <summary>
        /// Gets the address by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>SBAS_Core.Model.Address.</returns>
        public SBAS_Core.Model.Address GetAddressById(long id)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append("SELECT * FROM Address WHERE (AddressId =  @Id) ", new { Id = id });

                var aAddress = db.FirstOrDefault<SBAS_Core.Model.Address>(sqlbuilder);

                return aAddress;
            }
        }

        /// <summary>
        /// Deletes the address by identifier.
        /// </summary>
        /// <param name="aAddress">A address.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool DeleteAddressByID(SBAS_Core.Model.Address aAddress)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                bool result;
                try
                {
                    db.Delete("Address", "AddressId", aAddress);
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
        /// Updates the address.
        /// </summary>
        /// <param name="aAddress">A address.</param>
        /// <returns>SBAS_Core.Model.Address.</returns>
        public SBAS_Core.Model.Address UpdateAddress(SBAS_Core.Model.Address aAddress)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                db.Update("Address", "AddressId", aAddress);

                return aAddress;
            }
        }

        /// <summary>
        /// Gets the listof states.
        /// </summary>
        /// <returns>List&lt;Lut_State&gt;.</returns>
        public List<Lut_State> GetListofStates()
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sql = Sql.Builder;
                sql.Append("SELECT * FROM Lut_State");
                var states = db.Fetch<Lut_State>(sql);
                return states;
            }
        }
        /// <summary>
        /// Gets the state of the cities by.
        /// </summary>
        /// <param name="stateID">The state identifier.</param>
        /// <returns>List&lt;Lut_City&gt;.</returns>
        public List<Lut_City> GetCitiesByState(long stateID)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sql = Sql.Builder;
                sql.Append(string.Format("SELECT * FROM Lut_City WHERE StateID = {0}", stateID));
                var cities = db.Fetch<Lut_City>(sql);
                return cities;
            }

        }

        /// <summary>
        /// Gets the city by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Lut_City.</returns>
        public Lut_City GetCityById(long id)
        {
            using (var db = new Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sql = Sql.Builder;
                sql.Append(string.Format("select * from lut_City Where CityId = {0}", id));
                var city = db.Single<Lut_City>(sql);
                return city;
            }
        }

        /// <summary>
        /// Gets the state by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Lut_State.</returns>
        public Lut_State GetStateById(long id)
        {
            using (var db = new Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sql = Sql.Builder;
                sql.Append(string.Format("select * from lut_State Where StateId = {0}", id));
                var state = db.Single<Lut_State>(sql);
                return state;
            }
        }

        /// <summary>
        /// Gets the city and state by names.
        /// </summary>
        /// <param name="city">The city.</param>
        /// <param name="stateAbbreviation">The state abbreviation.</param>
        /// <returns>CityAndState.</returns>
        public CityAndState GetCityAndStateByNames(string city, string stateAbbreviation)
        {
            using (var db = new Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sql = Sql.Builder;
                const string sqlstr = "  SELECT c.[CityId] ,c.[City] ,c.[StateId] ,S.[StateAbbreviation] ,S.[StateName] " +
                                      " FROM [lut_City] AS c JOIN [lut_State] AS s ON c.StateId = s.StateID " +
                                      " WHERE c.City = '{0}' and s.[StateAbbreviation] = '{1}'";
                sql.Append(string.Format(sqlstr,city,stateAbbreviation));

                var result = db.FirstOrDefault<CityAndState>(sql);
                return result;
            }
        }
    }
}
