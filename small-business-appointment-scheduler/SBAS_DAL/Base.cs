// ***********************************************************************
// Assembly         : SBAS_DAL
// Author           : Ye Peng
// Created          : 06-04-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-10-2014
// ***********************************************************************
// <copyright file="Base.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
//  This is the base class and contains common properties used by all of teh properties used by all of the DAL classes to 
// </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The SBAS_DAL namespace.
/// </summary>
namespace SBAS_DAL
{
    /// <summary>
    /// Class Base.
    /// </summary>
    public class Base
    {
        /// <summary>
        /// Gets the get connection string.
        /// </summary>
        /// <value>The get connection string.</value>
        public static string GetConnectionString
        {
            get
            {
                var connectionString = ConfigurationManager.ConnectionStrings["SBAS"].ConnectionString;
                return connectionString;
            }
        }
    }
}
