// ***********************************************************************
// Assembly         : SBAS_DAL
// Author           : Ye Peng
// Created          : 07-09-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="Payment.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>Contains all of the methods for the Payment class.</summary>
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
    /// Contains all of the methods for the Payment class, which inherits from the Base class.
    /// </summary>
    public class Payment : Base
    {
        // Methods for Payments

        /// <summary>
        /// Gets a payment by the payment identifier.
        /// </summary>
        /// <param name="Id">The payment identifier.</param>
        /// <returns>SBAS_Core.Model.Payment.</returns>
        public SBAS_Core.Model.Payment GetPaymentByPaymentId(long Id)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    var sqlbuilder = Sql.Builder;
                    sqlbuilder.Append(string.Format("SELECT * FROM Payment WHERE PaymentId = {0} ", Id));
                    var paymentinfo = db.Single<SBAS_Core.Model.Payment>(sqlbuilder);

                    return paymentinfo;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the customer's payments by the customer identifier.
        /// </summary>
        /// <param name="Id">The customer identifier.</param>
        /// <returns>List<SBAS_Core.Model.Payment>.</returns>
        public List<SBAS_Core.Model.Payment> GetPaymentsByCustomerId(long Id)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    var sqlbuilder = Sql.Builder;
                    sqlbuilder.Append(string.Format("SELECT * FROM Payment WHERE CustomerId = {0} ", Id));
                    var paymentinfo = db.Fetch<SBAS_Core.Model.Payment>(sqlbuilder);

                    return paymentinfo;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the client's payments by the client identifier.
        /// </summary>
        /// <param name="Id">The client dentifier.</param>
        /// <returns>List<SBAS_Core.Model.Payment>.</returns>
        public List<SBAS_Core.Model.Payment> GetPaymentsByClientId(long Id)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    var sqlbuilder = Sql.Builder;
                    sqlbuilder.Append(string.Format("SELECT * FROM Payment WHERE ClientId = {0} ", Id));
                    var paymentinfo = db.Fetch<SBAS_Core.Model.Payment>(sqlbuilder);

                    return paymentinfo;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the payments by the invoice identifier.
        /// </summary>
        /// <param name="Id">The invoice identifier.</param>
        /// <returns>List<SBAS_Core.Model.Payment>.</returns>
        public List<SBAS_Core.Model.Payment> GetPaymentsByInvoiceId(long Id)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    var sqlbuilder = Sql.Builder;
                    sqlbuilder.Append(string.Format("SELECT * FROM Payment WHERE InvoiceId = {0} ", Id));
                    var paymentinfo = db.Fetch<SBAS_Core.Model.Payment>(sqlbuilder);

                    return paymentinfo;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Adds the payment to the database.
        /// </summary>
        /// <param name="payment">The payment to be added to the database.</param>
        /// <returns>SBAS_Core.Model.Payment.</returns>
        public SBAS_Core.Model.Payment AddPayment(SBAS_Core.Model.Payment payment)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    db.Insert("Payment", "PaymentId", payment);

                    return payment;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates the payment in the database.
        /// </summary>
        /// <param name="payment">The payment to be updated in the database.</param>
        /// <returns>SBAS_Core.Model.Payment.</returns>
        public SBAS_Core.Model.Payment UpdatePayment(SBAS_Core.Model.Payment payment)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    db.Update("Payment", "PaymentId", payment);

                    return payment;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes the payment from the database.
        /// </summary>
        /// <param name="payment">The payment to be deleted from the database.</param>
        /// <returns><c>true</c> if the delete operation is successful, <c>false</c> otherwise.</returns>
        public bool DeletePayment(SBAS_Core.Model.Payment payment)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                bool result;
                try
                {
                    db.Delete("Payment", "PaymentId", payment);
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }
                return result;
            }
        }

        /// <summary>
        /// Deletes the payment by the payment identifier.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns><c>true</c> if the delete operation is successful, <c>false</c> otherwise.</returns>
        public bool DeletePaymentByPaymentID(long paymentId)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                bool result;
                try
                {
                    db.Delete("Payment", "PaymentId", new SBAS_Core.Model.Payment(), paymentId);
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }
                return result;
            }
        }

        // Methods for payment methods

        /// <summary>
        /// Adds the payment method to the database.
        /// </summary>
        /// <param name="paymentMethod">The payment method to be added to the database.</param>
        /// <returns>SBAS_Core.Model.PaymentMethod.</returns>
        public SBAS_Core.Model.PaymentMethod AddPaymentMethod(SBAS_Core.Model.PaymentMethod paymentMethod)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    db.Insert("PaymentMethod", "PaymentMethodID", paymentMethod);

                    return paymentMethod;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates the payment method in the database.
        /// </summary>
        /// <param name="paymentMethod">The payment method to be updated in the database.</param>
        /// <returns>SBAS_Core.Model.PaymentMethod.</returns>
        public SBAS_Core.Model.PaymentMethod UpdatePaymentMethod(SBAS_Core.Model.PaymentMethod paymentMethod)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    db.Update("PaymentMethod", "PaymentMethodID", paymentMethod);

                    return paymentMethod;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes the payment method from the database.
        /// </summary>
        /// <param name="paymentMethod">The payment method to be deleted from the database.</param>
        /// <returns><c>true</c> if the delete operation is successful, <c>false</c> otherwise.</returns>
        public bool DeletePaymentMethod(SBAS_Core.Model.PaymentMethod paymentMethod)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                bool result;
                try
                {
                    db.Delete("PaymentMethod", "PaymentMethodID", paymentMethod);
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }
                return result;
            }
        }

        /// <summary>
        /// Deletes the payment method from the database by the payment method identifier.
        /// </summary>
        /// <param name="paymentMethodId">The payment method identifier.</param>
        /// <returns><c>true</c> if the delete operation is successful, <c>false</c> otherwise.</returns>
        public bool DeletePaymentMethodByPaymentMethodID(long paymentMethodId)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                bool result;
                try
                {
                    db.Delete("PaymentMethod", "PaymentMethodID", new SBAS_Core.Model.PaymentMethod(), paymentMethodId);
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the payment method by the payment method identifier.
        /// </summary>
        /// <param name="Id">The payment method identifier.</param>
        /// <returns>SBAS_Core.Model.PaymentMethod.</returns>
        public SBAS_Core.Model.PaymentMethod GetPaymentMethodByPaymentMethodID(long Id)
                    {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    var sqlbuilder = Sql.Builder;
                    sqlbuilder.Append(string.Format("SELECT * FROM PaymentMethod WHERE PaymentMethodId = {0} ", Id));
                    var paymentinfo = db.Single<SBAS_Core.Model.PaymentMethod>(sqlbuilder);

                    return paymentinfo;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the payment method by the payment method type.
        /// </summary>
        /// <param name="type">The payment method type.</param>
        /// <returns>SBAS_Core.Model.PaymentMethod.</returns>
        public SBAS_Core.Model.PaymentMethod GetPaymentMethodByPaymentMethodType(string type)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    var sqlbuilder = Sql.Builder;
                    sqlbuilder.Append(string.Format("SELECT * FROM PaymentMethod WHERE PaymentMethodType = {0} ", type));
                    var paymentinfo = db.Single<SBAS_Core.Model.PaymentMethod>(sqlbuilder);

                    return paymentinfo;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
