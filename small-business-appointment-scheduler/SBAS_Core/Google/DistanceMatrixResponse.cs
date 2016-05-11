// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 06-05-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 07-24-2014
// ***********************************************************************
// <copyright file="DistanceMatrixResponse.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
// This is the class that holds the response from distance matrix that is 
// used in the DAL and returned to the controller 
// </summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The Google namespace.
/// </summary>
namespace SBAS_Core.Google
{
    /// <summary>
    /// Class DistanceMatrixResponse.
    /// </summary>
    public class DistanceMatrixResponse
    {
        /// <summary>
        /// Gets or sets the miles.
        /// </summary>
        /// <value>The miles.</value>
        public double Miles { get; set; }
        /// <summary>
        /// Gets or sets the days.
        /// </summary>
        /// <value>The days.</value>
        public long Days { get; set; }
        /// <summary>
        /// Gets or sets the hours.
        /// </summary>
        /// <value>The hours.</value>
        public long Hours { get; set; }
        /// <summary>
        /// Gets or sets the minutes.
        /// </summary>
        /// <value>The minutes.</value>
        public long Minutes { get; set; }
    }
}
