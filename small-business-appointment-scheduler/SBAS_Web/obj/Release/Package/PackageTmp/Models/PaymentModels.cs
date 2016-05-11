// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Sydney Bennington
// Created          : 07-10-2014
//
// Last Modified By : Sydney Bennington
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="PaymentModels.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>Defines the payment models (PaymentModel and PaymentMethodModel)
//    and controls how information is displayed on the website.</summary>
// ***********************************************************************
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

/// <summary>
/// The SBAS_Web.Models namespace.
/// </summary>
namespace SBAS_Web.Models
{
    /// <summary>
    /// The PaymentModel class
    /// </summary>
    public class PaymentModel
    {
        /// <summary>
        /// Gets or sets the payment identifier.
        /// </summary>
        /// <value>The payment identifier.</value>
        public long PaymentId { get; set; }
        
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>The customer identifier.</value>
        public long CustomerId { get; set; }
        
        /// <summary>
        /// Gets or sets the invoice identifier.
        /// </summary>
        /// <value>The invoice identifier.</value>
        public long InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>The client identifier.</value>
        [Required]
        [Display(Name = "Client ID")]
        public long ClientId { get; set; }

        /// <summary>
        /// Gets or sets the payment amount.
        /// </summary>
        /// <value>The payment amount.</value>
        [Required]
        [Display(Name = "Payment Amount")]
        [Range(0.00, double.MaxValue, ErrorMessage = "Amount cannot be less than zero")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PaymentAmount { get; set; }

        /// <summary>
        /// Gets or sets the payment date.
        /// </summary>
        /// <value>The payment date.</value>
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Gets or sets the invoice number.
        /// </summary>
        /// <value>The invoice number.</value>
        [Display(Name = "Invoice Number")]
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets the first name of the client.
        /// </summary>
        /// <value>The first name of the client.</value>
        [Display(Name = "First Name")]
        public string ClientFirstName {get; set;}

        /// <summary>
        /// Gets or sets the last name of the client.
        /// </summary>
        /// <value>The last name of the client.</value>
        [Display(Name = "Last Name")]
        public string ClientLastName {get; set; }

        /// <summary>
        /// Gets or sets the amount due.
        /// </summary>
        /// <value>The amount due.</value>
        [Display(Name = "Amount Due")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal AmountDue { get; set; }

        /// <summary>
        /// Gets or sets the due date.
        /// </summary>
        /// <value>The due date.</value>
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Gets or sets the payment method.
        /// </summary>
        /// <value>The payment method.</value>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the payment method identifier.
        /// </summary>
        /// <value>The payment method identifier.</value>
        public long PaymentMethodId { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment method.
        /// </summary>
        /// <value>The type of the payment method.</value>
        [Display(Name = "Payment Method")]
        public string PaymentMethodType { get; set; }

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

    /// <summary>
    /// The PaymentMethodModel class
    /// </summary>
    public class PaymentMethodModel
    {
        /// <summary>
        /// Gets or sets the payment method identifier.
        /// </summary>
        /// <value>The payment method identifier.</value>
        public long PaymentMethodId { get; set; }
       
        /// <summary>
        /// Gets or sets the type of the payment method.
        /// </summary>
        /// <value>The type of the payment method.</value>
        public string PaymentMethodType { get; set; }

        /// <summary>
        /// Gets or sets the payment method description.
        /// </summary>
        /// <value>The payment method description.</value>
        [Display(Name = "Payment Method")]
        public string PaymentMethodDescription { get; set; }

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