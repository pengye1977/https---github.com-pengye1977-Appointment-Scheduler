// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 07-23-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 07-24-2014
// ***********************************************************************
// <copyright file="GeocoderLocation.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
//   This class is intended to hold a lat and lng and has a custom string format
//</summary>
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
    /// Class GeocoderLocation.
    /// </summary>
    public class GeocoderLocation
    {
        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude { get; set; }
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude { get; set; }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return String.Format("{0}, {1}", Latitude, Longitude);
        }
    }
}
