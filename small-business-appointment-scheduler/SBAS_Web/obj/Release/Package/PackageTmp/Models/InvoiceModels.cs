// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Sydney Bennington
// Created          : 07-03-2014
//
// Last Modified By : Sydney Bennington
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="InvoiceModels.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>Defines the invoice models (InvoiceModel and InvoiceLineItemModel)
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
    /// Defines the InvoiceModel class.
    /// </summary>
    public class InvoiceModel
    {
        /// <summary>
        /// Gets or sets the invoice identifier.
        /// </summary>
        /// <value>The invoice identifier.</value>
        public long InvoiceID { get; set; }

        /// <summary>
        /// Gets or sets the invoice number.
        /// </summary>
        /// <value>The invoice number.</value>
        [Display(Name = "Invoice Number")]
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets the amount due.
        /// </summary>
        /// <value>The amount due.</value>
        [Display(Name = "Amount Due")]
        [Range(0.00, double.MaxValue, ErrorMessage = "Amount cannot be less than zero")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal AmountDue { get; set; }

        /// <summary>
        /// Gets or sets the due date.
        /// </summary>
        /// <value>The due date.</value>
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>The client identifier.</value>
        [Display(Name = "Client ID")]
        public long ClientID { get; set; }

        /// <summary>
        /// Gets or sets the first name of the client.
        /// </summary>
        /// <value>The first name of the client.</value>
        [Display(Name = "First Name")]
        public string ClientFirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the client.
        /// </summary>
        /// <value>The last name of the client.</value>
        [Display(Name = "Last Name")]
        public string ClientLastName { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        [Display(Name = "Comments")]
        [StringLength(100, ErrorMessage = "The {0} must be no greater than {1} characters long.")]
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the invoice has been sent to the client.
        /// </summary>
        /// <value><c>true</c> if invoice has been sent to client; otherwise, <c>false</c>.</value>
        [Display(Name = "Sent to Client")]
        public bool SentToClient { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the invoice is paid.
        /// </summary>
        /// <value><c>true</c> if the invoice is paid; otherwise, <c>false</c>.</value>
        [Display(Name = "Paid")]
        public bool IsPaid { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>The end date.</value>
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the appointment identifier.
        /// </summary>
        /// <value>The appointment identifier.</value>
        public long AppointmentID { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>The customer identifier.</value>
        public long CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the name of service.
        /// </summary>
        /// <value>The name of service.</value>
        [Display(Name = "Service Performed")]
        public string NameofService { get; set; }

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
    /// The InvoiceLineItemModel class.
    /// </summary>
    public class InvoiceLineItemModel
    {
        /// <summary>
        /// Gets or sets the invoice line item identifier.
        /// </summary>
        /// <value>The invoice line item identifier.</value>
        public long InvoiceLineItemID { get; set; }
        
        /// <summary>
        /// Gets or sets the invoice identifier.
        /// </summary>
        /// <value>The invoice identifier.</value>
        public long InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        /// <value>The item identifier.</value>
        [Display(Name = "Item ID")]
        public long ItemId { get; set; }

        /// <summary>
        /// Gets or sets the item line cost override.
        /// </summary>
        /// <value>The item line cost override.</value>
        [Display(Name = "Cost Override Amount")]
        [Range(0.00, double.MaxValue, ErrorMessage = "Amount cannot be less than zero")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal ItemLineCostOverride { get; set; }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>The name of the item.</value>
        [Display(Name = "Item Name")]
        public string ItemName {get; set;}

        /// <summary>
        /// Gets or sets the item quantity.
        /// </summary>
        /// <value>The item quantity.</value>
        public decimal ItemQuantity { get; set; }

        /// <summary>
        /// Gets or sets the item total cost.
        /// </summary>
        /// <value>The item total cost.</value>
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal ItemTotalCost { get; set; }

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