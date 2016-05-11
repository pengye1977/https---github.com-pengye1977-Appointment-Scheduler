// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 06-05-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 07-26-2014
// ***********************************************************************
// <copyright file="DistanceMatrix.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
//       This is classes that used to call the Google Distance Matrix to get distance time between two appointments. This is 
//       used by the algorithm that determines time and distance between appointments in the DAL layer.
//</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using SBAS_Core.CustomClasses;
using SBAS_Core.Model;

/// <summary>
/// The Google namespace.
/// </summary>
namespace SBAS_Core.Google
{
    /// <summary>
    /// Class DistanceMatrix.
    /// </summary>
    public class DistanceMatrix
    {
        /// <summary>
        /// Gets the application setting value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        public static string GetAppSettingValue(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Gets the google address string.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>System.String.</returns>
        public static string GetGoogleAddressString(Address address)
        {
            try
            {
                return string.Format("{0}+{1}+{2}+{3}+{4}", address.AddressLine1, address.AddressLine2, 1, 2, //address.City, address.State
                      address.ZipCode);
            }
            catch (Exception ex)
            {
                return String.Empty;

            }
        }

        /// <summary>
        /// Spaces the separated pairs.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
        public static Dictionary<string, string> SpaceSeparatedPairs(string input)
        {

            string[] result = input.Split(' ');
            result = result.Where((s, i) => i % 2 == 0)
                           .Zip(result.Where((s, i) => i % 2 == 1), (a, b) => a + " " + b)
                           .ToArray();

            var dictionary = result.Select(s => s.Split(' ')).ToDictionary(values => values[1], values => values[0]);
            return dictionary;
        }

        /// <summary>
        /// Gets the driving distance in miles.
        /// </summary>
        /// <param name="originAddress">The origin address.</param>
        /// <param name="destinationAddress">The destination address.</param>
        /// <returns>DistanceMatrixResponse.</returns>
        public static DistanceMatrixResponse GetDrivingDistanceInMiles(Address originAddress, Address destinationAddress)
        {
            if (originAddress != null && destinationAddress != null)
            {
                var url =
                    string.Format(
                        "{0}?origins={1}&destinations={2}&mode=driving&sensor=false&language=en-EN&units=imperial",
                        GetAppSettingValue("DistanceMatrixURL"), GetGoogleAddressString(originAddress),
                        GetGoogleAddressString(destinationAddress));

                var request = (HttpWebRequest)WebRequest.Create(url);
                var response = request.GetResponse();
                var dataStream = response.GetResponseStream();
                if (dataStream == null) return null;
                var sreader = new StreamReader(dataStream);
                var responsereader = sreader.ReadToEnd();
                response.Close();

                var xmldoc = new XmlDocument();
                xmldoc.LoadXml(responsereader);


                if (xmldoc.GetElementsByTagName("status")[0].ChildNodes[0].InnerText == "OK")
                {
                    XmlNodeList distance = xmldoc.GetElementsByTagName("distance");
                    XmlNodeList duration = xmldoc.GetElementsByTagName("duration");
                    var result = new DistanceMatrixResponse();

                    var dictionary = SpaceSeparatedPairs(distance[0].ChildNodes[1].InnerText + " " + duration[0].ChildNodes[1].InnerText);

                    result.Miles = dictionary.ContainsKey("mi") ? Convert.ToDouble(dictionary["mi"]) : 0;

                    if (dictionary.ContainsKey("min"))
                    { result.Minutes = Convert.ToInt64(dictionary["min"]); }
                    else if (dictionary.ContainsKey("mins"))
                    { result.Minutes = Convert.ToInt64(dictionary["mins"]); }

                    if (dictionary.ContainsKey("hours"))
                    { result.Days = Convert.ToInt64(dictionary["hours"]); }
                    else if (dictionary.ContainsKey("hour"))
                    { result.Days = Convert.ToInt64(dictionary["hour"]); }


                    if (dictionary.ContainsKey("day"))
                    { result.Days = Convert.ToInt64(dictionary["day"]); }
                    else if (dictionary.ContainsKey("days"))
                    { result.Days = Convert.ToInt64(dictionary["days"]); }

                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the total minutes.
        /// </summary>
        /// <param name="distanceMatrixResponse">The distance matrix response.</param>
        /// <returns>System.Double.</returns>
        public static double GetTotalMinutes(DistanceMatrixResponse distanceMatrixResponse)
        {
            return (distanceMatrixResponse.Days*1400) + (distanceMatrixResponse.Hours*60) +
                   distanceMatrixResponse.Minutes;
        }
    }

    /// <summary>
    /// Class AppointmentListTime. This is a class that can be used with appointments stat and end times.
    /// </summary>
    public class AppointmentListTime
    {
        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>The start.</value>
        public DateTime? Start { get; set; }
        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>The end.</value>
        public DateTime? End { get; set; }
    }
}
