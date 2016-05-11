// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Roy Glunt
// Created          : 06-17-2014
//
// Last Modified By : TRoy Glunt
// Last Modified On : 07-24-2014
// ***********************************************************************
// <copyright file="ErrorController.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
//  This controller is used by the ELMAG web server logging.
//</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// The Controllers namespace.
/// </summary>
namespace SBAS_Web.Controllers
{
    /// <summary>
    /// Class ErrorController.
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// Error404s this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        // The 404 action handler
        // Get: /Error404/
        public ActionResult Error404()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
    }
}