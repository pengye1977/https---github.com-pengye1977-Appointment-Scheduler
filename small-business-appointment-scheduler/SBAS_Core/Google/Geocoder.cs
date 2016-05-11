// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 07-23-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 07-24-2014
// ***********************************************************************
// <copyright file="Geocoder.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
//  This class takes an Address class and calls the Google Geo code API to get the lat and lng. Which later will be used when populateing the
//  addresses on the google maps.
// </summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using SBAS_Core.Model;

/// <summary>
/// The Google namespace.
/// </summary>
namespace SBAS_Core.Google
{
    /// <summary>
    /// Class Geocoder.
    /// </summary>
    public class Geocoder
    {

        /// <summary>
        /// Geocodes the address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>Address.</returns>
        public static Address GeocodeAddress(Address address)
        {

            string url = "https://maps.googleapis.com/maps/api/geocode/xml?address=" + DistanceMatrix.GetGoogleAddressString(address) +
              "&key=" + DistanceMatrix.GetAppSettingValue("GoogleKey");

            WebRequest request = WebRequest.Create(url);

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    XDocument document = XDocument.Load(new StreamReader(stream));

                    XElement longitudeElement = document.Descendants("lng").FirstOrDefault();
                    XElement latitudeElement = document.Descendants("lat").FirstOrDefault();

                    if (longitudeElement == null || latitudeElement == null) return address;
                    address.Latitude =  Decimal.Parse(latitudeElement.Value, CultureInfo.InvariantCulture);
                    address.Longitude = Decimal.Parse(longitudeElement.Value, CultureInfo.InvariantCulture);
                    return address;
                }
            }
        }
    }
}
