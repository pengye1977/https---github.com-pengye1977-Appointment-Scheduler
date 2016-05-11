// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 06-07-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 07-24-2014
// ***********************************************************************
// <copyright file="Lut_State.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
//  This class represents a single State record. This is used by the DAL 
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
    /// Class Lut_State.
    /// </summary>
    public class Lut_State : ModelBase
    {
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
