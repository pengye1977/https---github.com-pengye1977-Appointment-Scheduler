// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 07-13-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-13-2014
// ***********************************************************************
// <copyright file="ModelBase.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>
//   This is the base class that has common properites that all the model classes have in commin.
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
    /// Class ModelBase.
    /// </summary>
    public class ModelBase
    {
        /// <summary>
        /// Gets or sets the create user.
        /// </summary>
        /// <value>The create user.</value>
        public string CreateUser { get; set; }
        /// <summary>
        /// Gets or sets the create date time.
        /// </summary>
        /// <value>The create date time.</value>
        public DateTime CreateDateTime { get; set; }
        /// <summary>
        /// Gets or sets the update user.
        /// </summary>
        /// <value>The update user.</value>
        public string UpdateUser { get; set; }
        /// <summary>
        /// Gets or sets the update date time.
        /// </summary>
        /// <value>The update date time.</value>
        public DateTime UpdateDateTime { get; set; }
    }
}
