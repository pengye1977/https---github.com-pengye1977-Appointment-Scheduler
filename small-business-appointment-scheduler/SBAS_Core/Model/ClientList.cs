// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 07-03-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="ClientList.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
//    This is holds a single ClientList record and is used by the DAL 
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
    /// Class ClientList.
    /// </summary>
    public class ClientList
    {
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
    }
}
