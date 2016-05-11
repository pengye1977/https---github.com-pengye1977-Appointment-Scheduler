// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Team 1 SWENG 500 Summer of 2014
// Created          : 05-21-2014
//
// Last Modified By : Team 1 SWENG 500 Summer of 2014
// Last Modified On : 07-24-2014
// ***********************************************************************
// <copyright file="Startup.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.Owin;
using Owin;

/// <summary>
/// The SBAS_Web namespace.
/// </summary>
[assembly: OwinStartupAttribute(typeof(SBAS_Web.Startup))]
namespace SBAS_Web
{
    /// <summary>
    /// Class Startup.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Configurations the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
