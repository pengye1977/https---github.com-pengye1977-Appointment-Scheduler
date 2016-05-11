// ***********************************************************************
// Assembly         : SBAS_DAL
// Author           : Ye Peng
// Created          : 06-05-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="Invoice.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>Contains all of the methods for the Invoice class.</summary>
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
    /// Contains all of the methods for the Invoice class, which inherits from the Base class.
    /// </summary>
    public class Invoice : Base
    {
        // Methods for Invoices

        /// <summary>
        /// Gets the invoice by invoice identifier.
        /// </summary>
        /// <param name="Id">The invoice identifier.</param>
        /// <returns>SBAS_Core.Model.Invoice.</returns>
        public SBAS_Core.Model.Invoice GetInvoiceByInvoiceId(long Id)
       {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
           {
               try
               {
                   var sqlbuilder = Sql.Builder;
                   sqlbuilder.Append(string.Format("SELECT * FROM Invoice WHERE InvoiceId = {0} ", Id));
                   var invoiceinfo = db.Single<SBAS_Core.Model.Invoice>(sqlbuilder);

                   return invoiceinfo;
               }
               catch (Exception ex)
               {
                   throw;
               }
            }
        }

        /// <summary>
        /// Gets the invoice by invoice number.
        /// </summary>
        /// <param name="InvoiceNumber">The invoice number.</param>
        /// <returns>SBAS_Core.Model.Invoice.</returns>
        public SBAS_Core.Model.Invoice GetInvoiceByInvoiceNumber(string InvoiceNumber)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    var sqlbuilder = Sql.Builder;
                    sqlbuilder.Append("SELECT * FROM Invoice WHERE InvoiceNumber = @invoicenumber", new { invoicenumber = InvoiceNumber });

                    var invoiceinfo = db.Single<SBAS_Core.Model.Invoice>(sqlbuilder);

                    return invoiceinfo;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets all invoices for a customer by customer identifier.
        /// </summary>
        /// <param name="CustomerID">The customer identifier.</param>
        /// <returns>List<SBAS_Core.Model.Invoice>.</returns>
        public List<SBAS_Core.Model.Invoice> GetAllInvoicesForCustomer(long CustomerID)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    var sqlbuilder = Sql.Builder;
                    sqlbuilder.Append(string.Format("SELECT * FROM Invoice WHERE CustomerId = {0}", CustomerID));
                    var invoiceinfo = db.Fetch<SBAS_Core.Model.Invoice>(sqlbuilder).ToList();

                    return invoiceinfo;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets all invoices for a client by client identifier.
        /// </summary>
        /// <param name="ClientId">The client identifier.</param>
        /// <returns>List<SBAS_Core.Model.Invoice>.</returns>
        public List<SBAS_Core.Model.Invoice> GetAllInvoicesForClient(long ClientId)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    var sqlbuilder = Sql.Builder;
                    sqlbuilder.Append(string.Format("SELECT * FROM Invoice WHERE ClientId = {0}", ClientId));
                    var invoiceinfo = db.Fetch<SBAS_Core.Model.Invoice>(sqlbuilder).ToList();

                    return invoiceinfo;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets all invoices for a client by the client's e-mail address.
        /// </summary>
        /// <param name="Email">The client's e-mail address.</param>
        /// <returns>List<SBAS_Core.Model.Invoice>.</returns>
        public List<SBAS_Core.Model.Invoice> GetAllInvoicesForClientByEmail(string Email)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    var client = new SBAS_DAL.SBASUser();
                    var foundClient = client.GetSBASUserByEmail(Email);
                    long clientId = foundClient.UserId;

                    var sqlbuilder = Sql.Builder;
                    sqlbuilder.Append(string.Format("SELECT * FROM Invoice WHERE ClientId = {0}", clientId));
                    var invoiceinfo = db.Fetch<SBAS_Core.Model.Invoice>(sqlbuilder).ToList();

                    return invoiceinfo;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Adds the invoice to the database.
        /// </summary>
        /// <param name="invoice">The invoice to be added to the database.</param>
        /// <returns>SBAS_Core.Model.Invoice.</returns>
        public SBAS_Core.Model.Invoice AddInvoice(SBAS_Core.Model.Invoice invoice)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    db.Insert("Invoice", "InvoiceId", invoice);
                    
                    return invoice;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates the invoice in the database.
        /// </summary>
        /// <param name="invoice">The invoice to be updated in the database.</param>
        /// <returns>SBAS_Core.Model.Invoice.</returns>
        public SBAS_Core.Model.Invoice UpdateInvoice(SBAS_Core.Model.Invoice invoice)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    db.Update("Invoice", "InvoiceId", invoice);
                    
                    return invoice;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes the invoice from the database.
        /// </summary>
        /// <param name="invoice">The invoice to be deleted from the database.</param>
        /// <returns><c>true</c> if the delete operation is successful, <c>false</c> otherwise.</returns>
        public bool DeleteInvoice(SBAS_Core.Model.Invoice invoice)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                bool result;
                try
                {
                    db.Delete("Invoice", "InvoiceId", invoice);
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
        /// Deletes the invoice by invoice identifier.
        /// </summary>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns><c>true</c> if the delete operation is successful, <c>false</c> otherwise.</returns>
        public bool DeleteInvoiceByInvoiceID(long invoiceId)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                bool result;
                try
                {
                    db.Delete("Invoice", "InvoiceId", new SBAS_Core.Model.Invoice(), invoiceId);
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
        /// Gets all invoices for the customer by customer identifier and if paid.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="isPaid">if set to <c>true</c> invoice is paid.</param>
        /// <returns>List<SBAS_Core.Model.Invoice>.</returns>
        public List<SBAS_Core.Model.Invoice> GetAllInvoicesForCustomerByIfPaid(long customerId, bool isPaid)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    var sqlbuilder = Sql.Builder;
                    sqlbuilder.Append("SELECT * FROM [Invoice] WHERE [CustomerId] = @id AND [IsPaid] = @Paid", new { id = customerId, Paid = isPaid });
                    var invoiceinfo = db.Fetch<SBAS_Core.Model.Invoice>(sqlbuilder).ToList();

                    return invoiceinfo;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the next invoice number for a customer.
        /// </summary>
        /// <param name="CustomerID">The customer identifier.</param>
        /// <returns>System.String.</returns>
        public string GetNextInvoiceNumber(long CustomerID)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    string invoiceNumber;

                    var sqlbuilder = Sql.Builder;
                    sqlbuilder.Append(string.Format("SELECT * FROM Invoice WHERE CustomerId = {0}", CustomerID));
                    var invoiceinfo = db.Fetch<SBAS_Core.Model.Invoice>(sqlbuilder).ToList();

                    // The above SQL Statement will only return a row if there is at least one invoice in the database that the specified customer created.
                    // If this is the customer's first invoice, the above SQL statement will return an empty list, and our invoice number will start at "1".
                    if (invoiceinfo.Count == 0)
                        return "1";

                    SBAS_Core.Model.Invoice invoice = invoiceinfo.ElementAt(invoiceinfo.Count - 1);

                    int tempInvoiceNumber = Convert.ToInt32(invoice.InvoiceNumber);
                    tempInvoiceNumber = tempInvoiceNumber + 1;

                    invoiceNumber = tempInvoiceNumber.ToString();

                    return invoiceNumber;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        // Methods for InvoiceLineItems

        /// <summary>
        /// Adds the invoice line item to the database.
        /// </summary>
        /// <param name="invoiceLineItem">The invoice line item to be added to the database.</param>
        /// <returns>SBAS_Core.Model.InvoiceLineItem.</returns>
        public SBAS_Core.Model.InvoiceLineItem AddInvoiceLineItem(SBAS_Core.Model.InvoiceLineItem invoiceLineItem)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    db.Insert("InvoiceLineItem", "InvoiceLineItemId", invoiceLineItem);

                    return invoiceLineItem;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes the invoice line item from the database.
        /// </summary>
        /// <param name="invoiceLineItem">The invoice line item to be deleted from the database.</param>
        /// <returns><c>true</c> if the delete operation is successful, <c>false</c> otherwise.</returns>
        public bool DeleteInvoiceLineItem(SBAS_Core.Model.InvoiceLineItem invoiceLineItem)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                bool result;
                try
                {
                    db.Delete("InvoiceLineItem", "InvoiceLineItemId", invoiceLineItem);
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
        /// Updates the invoice line item in the database.
        /// </summary>
        /// <param name="invoiceLineItem">The invoice line item to be updated in the database.</param>
        /// <returns>SBAS_Core.Model.InvoiceLineItem.</returns>
        public SBAS_Core.Model.InvoiceLineItem UpdateInvoiceLineItem(SBAS_Core.Model.InvoiceLineItem invoiceLineItem)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    db.Update("InvoiceLineItem", "InvoiceLineItemId", invoiceLineItem);
                    
                    return invoiceLineItem;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the invoice line item(s) associated with an invoice by invoice identifier.
        /// </summary>
        /// <param name="Id">The invoice identifier.</param>
        /// <returns>List<SBAS_Core.Model.InvoiceLineItem>.</returns>
        public List<SBAS_Core.Model.InvoiceLineItem> GetInvoiceLineItemsByInvoiceId(long Id)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    var sqlbuilder = Sql.Builder;
                    sqlbuilder.Append(string.Format("SELECT * FROM InvoiceLineItem WHERE InvoiceId = {0} ", Id));
                    var invoiceitems = db.Fetch<SBAS_Core.Model.InvoiceLineItem>(sqlbuilder).ToList();

                    return invoiceitems;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the invoice line item by invoice line item identifier.
        /// </summary>
        /// <param name="Id">The invoice line item identifier.</param>
        /// <returns>SBAS_Core.Model.InvoiceLineItem.</returns>
        public SBAS_Core.Model.InvoiceLineItem GetInvoiceLineItemByInvoiceLineItemId(long Id)
        {
            using (var db = new NPoco.Database(GetConnectionString, DatabaseType.SqlServer2012))
            {
                try
                {
                    var sqlbuilder = Sql.Builder;
                    sqlbuilder.Append(string.Format("SELECT * FROM InvoiceLineItem WHERE InvoiceLineItemId = {0} ", Id));
                    var invoiceitems = db.Single<SBAS_Core.Model.InvoiceLineItem>(sqlbuilder);

                    return invoiceitems;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
