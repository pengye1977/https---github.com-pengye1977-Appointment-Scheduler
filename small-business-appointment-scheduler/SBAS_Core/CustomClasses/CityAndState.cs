// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 07-26-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 07-26-2014
// ***********************************************************************
// <copyright file="CityAndState.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
//  This is a custom class that is used for the complex city and state queries in the DAL layer that has multiple table joins. This is used by the 
//  views that has city and state drop down list boxes.
//</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The CustomClasses namespace.
/// </summary>
namespace SBAS_Core.CustomClasses
{
    /// <summary>
    /// Class CityAndState.
    /// </summary>
    public class CityAndState
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
        /// <summary>
        /// Gets or sets the state abbreviation.
        /// </summary>
        /// <value>The state abbreviation.</value>
        public string StateAbbreviation { get; set; }
        /// <summary>
        /// Gets or sets the name of the state.
        /// </summary>
        /// <value>The name of the state.</value>
        public string StateName { get; set; }
    }
}
