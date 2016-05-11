// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Team 1 SWENG 500 Summer of 2014
// Created          : 05-21-2014
//
// Last Modified By : Team 1 SWENG 500 Summer of 2014
// Last Modified On : 07-24-2014
// ***********************************************************************
// <copyright file="FilterConfig.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Web;
using System.Web.Mvc;

/// <summary>
/// The SBAS_Web namespace.
/// </summary>
namespace SBAS_Web
{
    /// <summary>
    /// Class FilterConfig.
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Registers the global filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleAndLogErrorAttribute());
        }


        /// <summary>
        /// Class HandleAndLogErrorAttribute.
        /// </summary>
        public class HandleAndLogErrorAttribute : HandleErrorAttribute
        {
            /// <summary>
            /// Called when an exception occurs.
            /// </summary>
            /// <param name="filterContext">The action-filter context.</param>
            public override void OnException(ExceptionContext filterContext)
            {
                // Log the filterContext.Exception details
                base.OnException(filterContext);
            }
        }
    }
}
