// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Team 1 SWENG 500 Summer of 2014
// Created          : 05-21-2014
//
// Last Modified By : Team 1 SWENG 500 Summer of 2014
// Last Modified On : 07-24-2014
// ***********************************************************************
// <copyright file="RouteConfig.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

/// <summary>
/// The SBAS_Web namespace.
/// </summary>
namespace SBAS_Web
{
    /// <summary>
    /// Class RouteConfig.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Error404",
                url: "Error404/{action}/{id}",
                defaults: new
                {
                    controller = "Error",
                    action = "Error404",
                    id = UrlParameter.Optional
                }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
