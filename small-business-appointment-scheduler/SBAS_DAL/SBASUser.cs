// ***********************************************************************
// Assembly         : SBAS_DAL
// Author           : Ye Peng
// Created          : 06-07-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="SBASUser.cs" company="PENN STATE MASTERS PROGRAM">
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

/// <summary>
/// The SBAS_DAL namespace.
/// </summary>
namespace SBAS_DAL
{
    /// <summary>
    /// Class SBASUser.
    /// </summary>
    public class SBASUser : Base
    {
        /// <summary>
        /// Creates the sbas user by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="aSBABUserInfo">A sbab user information.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool CreateSBASUserByEmail(string email, SBAS_Core.Model.SBASUser aSBABUserInfo)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append("SELECT Id FROM dbo.AspNetUsers WHERE Email = @Email ", 
                    new {Email = email} );

                string s = db.Single<string>(sqlbuilder);

                aSBABUserInfo.Id = s;

                db.Insert("SBASUser", "UserId", true, aSBABUserInfo);

                return true;
            }
        }

        /// <summary>
        /// Gets the sbas user by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>SBAS_Core.Model.SBASUser.</returns>
        public SBAS_Core.Model.SBASUser GetSBASUserByEmail(string email)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append("SELECT SBASUser.*, dbo.AspNetUsers.Email FROM " +
                    "dbo.AspNetUsers INNER JOIN SBASUser ON dbo.AspNetUsers.Id = SBASUser.Id " +
                    "WHERE dbo.AspNetUsers.Email = @Email ", new { Email = email } );

                var aSBABUserInfo = db.Single<SBAS_Core.Model.SBASUser>(sqlbuilder);

                return aSBABUserInfo;
            }
        }

        /// <summary>
        /// Gets the sbas user by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>SBAS_Core.Model.SBASUser.</returns>
        public SBAS_Core.Model.SBASUser GetSBASUserById(long Id)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append("SELECT SBASUser.*, dbo.AspNetUsers.Email FROM " +
                    "dbo.AspNetUsers INNER JOIN SBASUser ON dbo.AspNetUsers.Id = SBASUser.Id " +
                    "WHERE SBASUser.UserId = @ID ", new { ID = Id });

                var aSBABUserInfo = db.Single<SBAS_Core.Model.SBASUser>(sqlbuilder);

                return aSBABUserInfo;
            }
        }


        /// <summary>
        /// Updates the sbas user by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="aSBABUserInfo">A sbab user information.</param>
        /// <returns>SBAS_Core.Model.SBASUser.</returns>
        public SBAS_Core.Model.SBASUser UpdateSBASUserByEmail(string email, SBAS_Core.Model.SBASUser aSBABUserInfo)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                db.Update("SBASUser", "UserId", aSBABUserInfo);

                return aSBABUserInfo;
            }
        }

        /// <summary>
        /// Deletes the sbas user by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="aSBABUserInfo">A sbab user information.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool DeleteSBASUserByEmail(string email, SBAS_Core.Model.SBASUser aSBABUserInfo)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                bool result;
                try
                {
                    db.Delete("SBASUser", "UserId", aSBABUserInfo);
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
        /// Gets all sbas user_ customer.
        /// </summary>
        /// <returns>List&lt;SBAS_Core.Model.SBASUser&gt;.</returns>
        public List<SBAS_Core.Model.SBASUser> GetAllSBASUser_Customer()
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append("SELECT     SBASUser.* " +
                                "FROM         SBASUser INNER JOIN " +
                                "dbo.AspNetRoles ON SBASUser.Id = dbo.AspNetRoles.Id " +
                                "WHERE     (dbo.AspNetRoles.Name = 'Customer')");

                var SBABUserInfo = db.Fetch<SBAS_Core.Model.SBASUser>(sqlbuilder);

                return SBABUserInfo;
            }
        }

        /// <summary>
        /// Gets all sbas user_ client.
        /// </summary>
        /// <returns>List&lt;SBAS_Core.Model.SBASUser&gt;.</returns>
        public List<SBAS_Core.Model.SBASUser> GetAllSBASUser_Client()
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append("SELECT s.* " +
                                  " FROM [SWENG500_User].[SBASUser] s JOIN AspNetUsers u ON S.ID = u.ID " +
                                  " JOIN AspNetUserRoles ur ON u.ID = ur.UserId  " +
                                  " JOIN AspNetRoles r ON ur.RoleId = r.Id and r.Name = 'Client' " +
                                  " where s.ID is not null");

                var SBABUserInfo = db.Fetch<SBAS_Core.Model.SBASUser>(sqlbuilder);

                return SBABUserInfo;
            }
        }

        /// <summary>
        /// Gets all customer clients.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>List&lt;SBAS_Core.Model.SBASUser&gt;.</returns>
        public List<SBAS_Core.Model.SBASUser> GetAllCustomerClients(long id)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append("select usr.* from ClientList AS cl JOIN SBASUser usr ON cl.ClientId = usr.UserID where cl.CustomerId = @Id", new {Id = id});

                var customerCLients = db.Fetch<SBAS_Core.Model.SBASUser>(sqlbuilder);

                return customerCLients;
            }
        }

        /// <summary>
        /// Gets the name of the user role.
        /// </summary>
        /// <param name="aspNetUserId">The ASP net user identifier.</param>
        /// <returns>System.String.</returns>
        public static string GetUserRoleName(string aspNetUserId)
        {

            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {

               string result;


                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append("SELECT anr.name as rolename FROM [AspNetUserRoles] AS anur JOIN [AspNetRoles] as anr ON anur.roleId = anr.id where userid = @Id", new { Id = aspNetUserId });

                result = db.ExecuteScalar<string>(sqlbuilder);

                return result;
            }
        }
    }
}
