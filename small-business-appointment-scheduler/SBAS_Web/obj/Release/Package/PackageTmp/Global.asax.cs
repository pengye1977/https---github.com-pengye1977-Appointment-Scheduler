// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Team 1 SWENG 500 Summer of 2014
// Created          : 05-21-2014
//
// Last Modified By : Team 1 SWENG 500 Summer of 2014
// Last Modified On : 07-24-2014
// ***********************************************************************
// <copyright file="Global.asax.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

/// <summary>
/// The SBAS_Web namespace.
/// </summary>
namespace SBAS_Web
{
    /// <summary>
    /// Class MvcApplication.
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Application_s the start.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;
            // Log this exception with your logger
        }

    }
}
