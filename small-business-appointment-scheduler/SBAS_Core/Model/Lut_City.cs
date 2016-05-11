// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 06-07-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 07-24-2014
// ***********************************************************************
// <copyright file="Lut_City.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
//    This class is used by the DAL to hold a single City record requested by the DAL.
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
    /// Class Lut_City.
    /// </summary>
    public class Lut_City : ModelBase
    {
        /// <summary>
        /// Gets or sets the city identifier.
        /// </summary>
        /// <value>The city identifier.</value>
        public long CityId { get; set; }
        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public string City { get; set; }
        /// <summary>
        /// Gets or sets the state identifier.
        /// </summary>
        /// <value>The state identifier.</value>
        public long StateId { get; set; }
    }
}
