// ***********************************************************************
// Assembly         : SBAS_DAL
// Author           : Ye Peng
// Created          : 07-10-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-11-2014
// ***********************************************************************
// <copyright file="PaymentMethod.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>Contains one method for the PaymentMethod class.</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPoco;
using SBAS_Core;

/// <summary>
/// The SBAS_DAL namespace.
/// </summary>
namespace SBAS_DAL
{
    /// <summary>
    /// Contains one method for the PaymentMethod class, which inherits from the Base class.
    /// </summary>
    public class PaymentMethod : Base
    {
        /// <summary>
        /// Gets the list of payment method types from the database.
        /// </summary>
        /// <returns>List<SBAS_Core.Model.PaymentMethod>.</returns>
        public List<SBAS_Core.Model.PaymentMethod> GetListofPaymentMethodTypes()
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                string sqlQueryString = "SELECT * FROM PaymentMethod";
                var paymentMethod = db.Fetch<SBAS_Core.Model.PaymentMethod>(sqlQueryString);
                return paymentMethod;
            }            
        }
    }
}
