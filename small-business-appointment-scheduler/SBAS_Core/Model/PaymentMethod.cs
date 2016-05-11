// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 06-07-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="PaymentMethod.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>Definition of the PaymentMethod class.</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The SBAS_Core.Model namespace.
/// </summary>
namespace SBAS_Core.Model
{
    /// <summary>
    /// This is the definition of the PaymentMethod class, which inherits from the ModelBase class.
    /// </summary>
    public class PaymentMethod : ModelBase
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
      public string PaymentMethodDescription { get; set; }
    }
}
