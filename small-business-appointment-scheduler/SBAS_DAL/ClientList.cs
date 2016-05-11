// ***********************************************************************
// Assembly         : SBAS_DAL
// Author           : Ye Peng
// Created          : 07-11-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="ClientList.cs" company="PENN STATE MASTERS PROGRAM">
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
    /// Class ClientList.
    /// </summary>
    public class ClientList : Base
    {
        /// <summary>
        /// Creates the client list.
        /// </summary>
        /// <param name="aClientList">A client list.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool CreateClientList(SBAS_Core.Model.ClientList aClientList)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                db.Insert(aClientList);

                return true;
            }
        }

        /// <summary>
        /// Deletes the client list.
        /// </summary>
        /// <param name="aClientList">A client list.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool DeleteClientList(SBAS_Core.Model.ClientList aClientList)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                bool result;
                try
                {
                    //db.Delete(aClientList);

                    db.Delete("ClientList", "ClientId", new SBAS_Core.Model.ClientList(), aClientList.ClientId);

                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }
                return result;

            }
        }
    }
}
