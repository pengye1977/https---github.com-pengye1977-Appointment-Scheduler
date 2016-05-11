// ***********************************************************************
// Assembly         : SBAS_UnitTest
// Author           : Team 1 SWENG 500 Summer of 2014
// Created          : 06-06-2014
//
// Last Modified By : Team 1 SWENG 500 Summer of 2014
// Last Modified On : 07-27-2014
// ***********************************************************************
// <copyright file="GoogleUnitTests.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Diagnostics.Eventing.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPoco;
using System.Collections.Generic;
using SBAS_Core.CustomClasses;
using SBAS_Core.Google;
using SBAS_Core.Model;
using SBAS_DAL;
using Address = SBAS_DAL.Address;
using ClientList = SBAS_Core.Model.ClientList;
using SBASUser = SBAS_Core.Model.SBASUser;
using ServiceType = SBAS_Core.Model.ServiceType;

/// <summary>
/// The SBAS_UnitTest namespace.
/// </summary>
namespace SBAS_UnitTest
{
    /// <summary>
    /// Class GoogleUnitTests.
    /// </summary>
    [TestClass]
    public class GoogleUnitTests
    {
        #region static variables
        // DB and DAL
        /// <summary>
        /// The database
        /// </summary>
        private static Database db = new Database(Base.GetConnectionString, DatabaseType.SqlServer2012);
        /// <summary>
        /// The address dal
        /// </summary>
        private static Address addressDal = new Address();

        // User Objects
        /// <summary>
        /// The customer
        /// </summary>
        private static SBASUser customer;
        /// <summary>
        /// The client
        /// </summary>
        private static SBASUser client;
        /// <summary>
        /// The client2
        /// </summary>
        private static SBASUser client2;
        /// <summary>
        /// The client3
        /// </summary>
        private static SBASUser client3;

        // Address Objects
        /// <summary>
        /// The customer address
        /// </summary>
        private static SBAS_Core.Model.Address customerAddress;
        /// <summary>
        /// The client address
        /// </summary>
        private static SBAS_Core.Model.Address clientAddress;
        /// <summary>
        /// The client address2
        /// </summary>
        private static SBAS_Core.Model.Address clientAddress2;
        /// <summary>
        /// The client address3
        /// </summary>
        private static SBAS_Core.Model.Address clientAddress3;

        // City and State
        /// <summary>
        /// The manteca city and state
        /// </summary>
        private static CityAndState mantecaCityAndState;
        /// <summary>
        /// The modesto city and state
        /// </summary>
        private static CityAndState modestoCityAndState;
        /// <summary>
        /// The stockton city and state
        /// </summary>
        private static CityAndState stocktonCityAndState;
        /// <summary>
        /// The tracy city and state
        /// </summary>
        private static CityAndState tracyCityAndState;

        // ClientList
        /// <summary>
        /// The listofclient list
        /// </summary>
        private static List<ClientList> listofclientList = new List<ClientList>();

        //Service Types
        /// <summary>
        /// The service type
        /// </summary>
        private static ServiceType serviceType;

        // Other classes 
        /// <summary>
        /// The audit user name
        /// </summary>
        private static string auditUserName = "RoyG";
        /// <summary>
        /// The datetime
        /// </summary>
        private static DateTime datetime = DateTime.Now;

        #endregion

        #region Initialize & Cleanup
        /// <summary>
        /// Googles the unit tests_ class initialize.
        /// </summary>
        /// <param name="testContext">The test context.</param>
        [ClassInitialize()]
        public static void GoogleUnitTests_ClassInitialize(TestContext testContext)
        {

            BuildNeededStateAndCity();

            BuildCustomerAddresses();

            BuildClientAddresses();

            BuildCustomers();

            BuildClients();

            BuildClientList();

            BuildServiceTypes();

        }

        // Use ClassCleanup to run code after all tests in a class have run
        /// <summary>
        /// Googles the unit tests_ class cleanup.
        /// </summary>
        [ClassCleanup()]
        public static void GoogleUnitTests_ClassCleanup()
        {
            using (db)
            {
                foreach (var clientlist in listofclientList)
                {
                    var sqlbuilder = Sql.Builder;
                    sqlbuilder.Append(string.Format("delete ClientList where ClientID = {0} and CustomerId = {1}",
                        clientlist.ClientId, clientlist.CustomerId));
                    db.Execute(sqlbuilder);
                }

                listofclientList.Clear();


                db.Delete("ServiceType", "ServiceTypeId", serviceType, serviceType.ServiceTypeId);

                db.Delete("SBASUser", "UserId", customer, customer.UserId);
                db.Delete("SBASUser", "UserId", client, client.UserId);
                db.Delete("SBASUser", "UserId", client2, client2.UserId);
                db.Delete("SBASUser", "UserId", client3, client3.UserId);
                db.Delete("Address", "AddressId", customerAddress, customerAddress.AddressId);
                db.Delete("Address", "AddressId", clientAddress, clientAddress.AddressId);
                db.Delete("Address", "AddressId", clientAddress2, clientAddress2.AddressId);
                db.Delete("Address", "AddressId", clientAddress3, clientAddress3.AddressId);
            }
        }

        #endregion

        /// <summary>
        /// Tests the geocoding address.
        /// </summary>
        [TestMethod]
        public void TestGeocodingAddress()
        {

            //1922 Redwood Ave, Manteca, CA 95336
            decimal lat = (decimal)37.82433;
            decimal lng = (decimal)-121.224189;
            var datetime = DateTime.Now;
            var address = new SBAS_Core.Model.Address();
            address.AddressLine1 = "1922 Redwood Ave";
            //address.AddressLine2 = "Suite 101";
            address.CityId = mantecaCityAndState.CityId;
            address.StateId = mantecaCityAndState.StateId;
            address.ZipCode = "95336";
            address.Longitude = null;
            address.Latitude = null;
            address.CreateUser = "RoyG";
            address.UpdateUser = "RoyG";
            address.CreateDateTime = datetime;
            address.UpdateDateTime = datetime;
            Geocoder.GeocodeAddress(address);
            Assert.AreEqual(lat, address.Latitude, "Latitudes do not match");
            Assert.AreEqual(lng, address.Longitude, "Longitudes do not match");
        }

        /// <summary>
        /// Tests the can appointment be created.
        /// </summary>
        [TestMethod]
        public void TestCanAppointmentBeCreated()
        {

        }

 
        #region Private Methods
        /// <summary>
        /// Builds the needed state and city.
        /// </summary>
        private static void BuildNeededStateAndCity()
        {
            using (db)
            {
                mantecaCityAndState = addressDal.GetCityAndStateByNames("Manteca", "CA");
                modestoCityAndState = addressDal.GetCityAndStateByNames("Modesto", "CA");
                stocktonCityAndState = addressDal.GetCityAndStateByNames("Stockton", "CA");
                tracyCityAndState = addressDal.GetCityAndStateByNames("Tracy", "CA");
            }
        }

        /// <summary>
        /// Builds the customer addresses.
        /// </summary>
        private static void BuildCustomerAddresses()
        {
            using (db)
            {
                //create customer address
                customerAddress = new SBAS_Core.Model.Address();
                customerAddress.AddressLine1 = "6000 Lathrop Rd";
                //customerAddress.AddressLine2 = "Suite 101";
                customerAddress.CityId = mantecaCityAndState.CityId;
                customerAddress.StateId = mantecaCityAndState.StateId;
                customerAddress.ZipCode = "95336";
                customerAddress.Longitude = null;
                customerAddress.Latitude = null;
                customerAddress.CreateUser = auditUserName;
                customerAddress.UpdateUser = auditUserName;
                customerAddress.CreateDateTime = datetime;
                customerAddress.UpdateDateTime = datetime;

                db.Insert("Address", "AddressId", true, customerAddress);
            }
        }

        /// <summary>
        /// Builds the client addresses.
        /// </summary>
        private static void BuildClientAddresses()
        {
            using (db)
            {

                //Client Addrees (Valid Address)
                clientAddress = new SBAS_Core.Model.Address();
                clientAddress.AddressLine1 = "1315 Primavera Ave";
                clientAddress.AddressLine2 = null;
                clientAddress.CityId = mantecaCityAndState.CityId;
                clientAddress.StateId = mantecaCityAndState.StateId;
                clientAddress.ZipCode = "95336";
                clientAddress.Longitude = null;
                clientAddress.Latitude = null;
                clientAddress.CreateUser = auditUserName;
                clientAddress.UpdateUser = auditUserName;
                clientAddress.CreateDateTime = datetime;
                clientAddress.UpdateDateTime = datetime;

                //Client Addrees (Valid Address)
                clientAddress2 = new SBAS_Core.Model.Address();
                clientAddress2.AddressLine1 = "322 Laurelwood Cir";
                clientAddress2.AddressLine2 = null;
                clientAddress2.CityId = mantecaCityAndState.CityId;
                clientAddress2.StateId = mantecaCityAndState.StateId;
                clientAddress2.ZipCode = "95336";
                clientAddress2.Longitude = null;
                clientAddress2.Latitude = null;
                clientAddress2.CreateUser = auditUserName;
                clientAddress2.UpdateUser = auditUserName;
                clientAddress2.CreateDateTime = datetime;
                clientAddress2.UpdateDateTime = datetime;

                //Client Addrees (Valid Address)
                clientAddress3 = new SBAS_Core.Model.Address();
                clientAddress3.AddressLine1 = "2209 Eicher Ave";
                clientAddress3.AddressLine2 = null;
                clientAddress3.CityId = modestoCityAndState.CityId;
                clientAddress3.StateId = modestoCityAndState.StateId;
                clientAddress3.ZipCode = "95350";
                clientAddress3.Longitude = null;
                clientAddress3.Latitude = null;
                clientAddress3.CreateUser = auditUserName;
                clientAddress3.UpdateUser = auditUserName;
                clientAddress3.CreateDateTime = datetime;
                clientAddress3.UpdateDateTime = datetime;

                db.Insert("Address", "AddressId", true, clientAddress);
                db.Insert("Address", "AddressId", true, clientAddress2);
                db.Insert("Address", "AddressId", true, clientAddress3);

            }

        }

        /// <summary>
        /// Builds the customers.
        /// </summary>
        private static void BuildCustomers()
        {
            using (db)
            {
                // create customer
                customer = new SBASUser();
                customer.FirstName = "Joe";
                customer.LastName = "Bob";
                customer.CompanyName = "Joe Bobs Pool Service";
                customer.FaxNumber = "724-555-1212";
                customer.AddressId = customerAddress.AddressId;
                customer.CreateUser = auditUserName;
                customer.UpdateUser = auditUserName;
                customer.CreateDateTime = datetime;
                customer.UpdateDateTime = datetime;
                db.Insert("SBASUser", "UserId", true, customer);
            }
        }

        /// <summary>
        /// Builds the clients.
        /// </summary>
        private static void BuildClients()
        {
            using (db)
            {
                // create client
                client = new SBASUser();
                client.FirstName = "Robert";
                client.LastName = "Diedrick";
                client.CompanyName = null;
                client.FaxNumber = "209-111-1111";
                client.AddressId = clientAddress.AddressId;
                client.CreateUser = auditUserName;
                client.UpdateUser = auditUserName;
                client.CreateDateTime = datetime;
                client.UpdateDateTime = datetime;
                db.Insert("SBASUser", "UserId", true, client);

                // create client
                client2 = new SBASUser();
                client2.FirstName = "Sandy";
                client2.LastName = "Diedrick";
                client2.CompanyName = null;
                client2.FaxNumber = "209-222-2222";
                client2.AddressId = clientAddress2.AddressId;
                client2.CreateUser = auditUserName;
                client2.UpdateUser = auditUserName;
                client2.CreateDateTime = datetime;
                client2.UpdateDateTime = datetime;
                db.Insert("SBASUser", "UserId", true, client2);

                // create client
                client3 = new SBASUser();
                client3.FirstName = "Raymond";
                client3.LastName = "Diedrick";
                client3.CompanyName = null;
                client3.FaxNumber = "209-333-3333";
                client3.AddressId = clientAddress3.AddressId;
                client3.CreateUser = auditUserName;
                client3.UpdateUser = auditUserName;
                client3.CreateDateTime = datetime;
                client3.UpdateDateTime = datetime;
                db.Insert("SBASUser", "UserId", true, client3);
            }
        }

        /// <summary>
        /// Builds the client list.
        /// </summary>
        private static void BuildClientList()
        {
            ClientList clientlist;
            using (db)
            {

                clientlist = new ClientList();
                clientlist.ClientId = client.UserId;
                clientlist.CustomerId = customer.UserId;

                db.Insert("ClientList", "ClientID,CustomerID", true, clientlist);

                listofclientList.Add(clientlist);

                clientlist = new ClientList();
                clientlist.ClientId = client2.UserId;
                clientlist.CustomerId = customer.UserId;

                db.Insert("ClientList", "ClientID,CustomerID", true, clientlist);

                listofclientList.Add(clientlist);

                clientlist = new ClientList();
                clientlist.ClientId = client3.UserId;
                clientlist.CustomerId = customer.UserId;

                db.Insert("ClientList", "ClientID,CustomerID", true, clientlist);

                listofclientList.Add(clientlist);
            }
        }

        /// <summary>
        /// Builds the service types.
        /// </summary>
        private static void BuildServiceTypes()
        {
            using (db)
            {
                serviceType = new ServiceType();
                serviceType.CustomerId = customer.UserId;
                serviceType.Description = "Weekly Pool Cleaning and Chemical adjustment";
                serviceType.NameOfService = "Weekly Service Pack 1";
                serviceType.UpdateDateTime = datetime;
                serviceType.CreateDateTime = datetime;
                serviceType.UpdateUser = auditUserName;
                serviceType.CreateUser = auditUserName;

                db.Insert("ServiceType", "ServiceTypeId", true, serviceType);
            }
        }

        #endregion
    }
}

