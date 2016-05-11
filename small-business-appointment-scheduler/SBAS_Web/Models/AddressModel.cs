// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Ye Peng
// Created          : 07-10-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="AddressModel.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// The Models namespace.
/// </summary>
namespace SBAS_Web.Models
{
    /// <summary>
    /// Class AddressModel.
    /// </summary>
    public class AddressModel
    {
        /// <summary>
        /// Gets or sets the address identifier.
        /// </summary>
        /// <value>The address identifier.</value>
        public long? AddressId { get; set; }
        /// <summary>
        /// Gets or sets the address line1.
        /// </summary>
        /// <value>The address line1.</value>
        [Required]
        [Display(Name = "Address Line 1")]
        [StringLength(150, ErrorMessage = "The {0} must be no greater than {1} characters long.")]
        public string AddressLine1 { get; set; }
        /// <summary>
        /// Gets or sets the address line2.
        /// </summary>
        /// <value>The address line2.</value>
        [StringLength(100, ErrorMessage = "The {0} must be no greater than {1} characters long.")]
        public string AddressLine2 { get; set; }
        /// <summary>
        /// Gets or sets the selected city identifier.
        /// </summary>
        /// <value>The selected city identifier.</value>
        public long SelectedCityID { get; set; }
        /// <summary>
        /// Gets or sets the selected state identifier.
        /// </summary>
        /// <value>The selected state identifier.</value>
        public long SelectedStateID { get; set; }
        /// <summary>
        /// Gets or sets the city list.
        /// </summary>
        /// <value>The city list.</value>
        [Display(Name = "City")]
        public SelectList CityList { get; set; }
        /// <summary>
        /// Gets or sets the state list.
        /// </summary>
        /// <value>The state list.</value>
        [Display(Name = "State")]
        public SelectList StateList { get; set; }
        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>The zip code.</value>
        [Required]
        [Display(Name = "Zip Code")]
        [StringLength(10, ErrorMessage = "The {0} must be no greater than {1} characters long.")]
        public string ZipCode { get; set; }

    }
}
