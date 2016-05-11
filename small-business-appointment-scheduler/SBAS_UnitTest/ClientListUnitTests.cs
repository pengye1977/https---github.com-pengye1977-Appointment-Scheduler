// ***********************************************************************
// Assembly         : SBAS_UnitTest
// Author           : Ye Peng
// Created          : 07-29-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="ClientListUnitTests.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using NPoco;

/// <summary>
/// The SBAS_UnitTest namespace.
/// </summary>
namespace SBAS_UnitTest
{
    /// <summary>
    /// Class ClientListUnitTests.
    /// </summary>
    [TestClass]
    public class ClientListUnitTests
    {

        //private static string testEmail = "abc@def.com";
        //private static string AspNetUserID = string.Empty;
        /// <summary>
        /// The s modle1
        /// </summary>
        private static SBAS_Core.Model.SBASUser sModle1 = new SBAS_Core.Model.SBASUser();
        /// <summary>
        /// The customer address
        /// </summary>
        private static SBAS_Core.Model.Address customerAddress = new SBAS_Core.Model.Address();
        /// <summary>
        /// The address identifier
        /// </summary>
        private static long addressID = 0;
        /// <summary>
        /// The s modle2
        /// </summary>
        private static SBAS_Core.Model.SBASUser sModle2 = new SBAS_Core.Model.SBASUser();
        /// <summary>
        /// The customer address2
        /// </summary>
        private static SBAS_Core.Model.Address customerAddress2 = new SBAS_Core.Model.Address();
        /// <summary>
        /// The address i d2
        /// </summary>
        private static long addressID2 = 0;
        /// <summary>
        /// The test customer email
        /// </summary>
        private static string TestCustomerEmail = "customer@abc.com";
        /// <summary>
        /// The test client email
        /// </summary>
        private static string TestClientEmail = "client@abc.com";
        /// <summary>
        /// The test customer identifier
        /// </summary>
        private static string TestCustomerID = "e8cf26f8-d81c-4046-9616-61fdaaf108b1";
        /// <summary>
        /// The test client identifier
        /// </summary>
        private static string TestClientID = "13699b6d-0371-4fea-8514-5c9559946fb2";
        /// <summary>
        /// The user i d1
        /// </summary>
        private static long UserID1 = 0;
        /// <summary>
        /// The user i d2
        /// </summary>
        private static long UserID2 = 0;

        // Use ClassInitialize to run code before running the first test in the class
        /// <summary>
        /// Mies the class initialize.
        /// </summary>
        /// <param name="testContext">The test context.</param>
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            //Address
            //create customer address
            customerAddress.AddressLine1 = "ABCDEFG";
            customerAddress.AddressLine2 = "HIJKLMN";
            customerAddress.CityId = 6323;
            customerAddress.StateId = 40;
            customerAddress.ZipCode = "12345";
            customerAddress.Longitude = null;
            customerAddress.Latitude = null;
            customerAddress.CreateUser = TestCustomerEmail;
            customerAddress.UpdateUser = TestCustomerEmail;
            customerAddress.CreateDateTime = DateTime.Now;
            customerAddress.UpdateDateTime = DateTime.Now;
            SBAS_DAL.Address sDAL = new SBAS_DAL.Address();
            addressID = sDAL.CreateAddress(customerAddress);

            //Test data
            sModle1.Id = TestCustomerID;
            sModle1.FirstName = "Ye";
            sModle1.LastName = "Peng";
            sModle1.UserId = 0;
            sModle1.FaxNumber = "12345678";
            sModle1.MobileNumber = "12345678";
            sModle1.PhoneNumber = "12345678";
            sModle1.CreateUser = TestCustomerEmail;
            sModle1.CreateDateTime = DateTime.Now;
            sModle1.UpdateUser = TestCustomerEmail;
            sModle1.UpdateDateTime = DateTime.Now;
            sModle1.FirstStartTime = DateTime.Now;
            sModle1.AddressId = addressID;

            //create customer account
            SBAS_DAL.SBASUser sDAL2 = new SBAS_DAL.SBASUser();
            sDAL2.CreateSBASUserByEmail(TestCustomerEmail, sModle1);

            //Address
            //create Client address
            customerAddress2.AddressLine1 = "ABCDEFG";
            customerAddress2.AddressLine2 = "HIJKLMN";
            customerAddress2.CityId = 6323;
            customerAddress2.StateId = 40;
            customerAddress2.ZipCode = "12345";
            customerAddress2.Longitude = null;
            customerAddress2.Latitude = null;
            customerAddress2.CreateUser = TestClientEmail;
            customerAddress2.UpdateUser = TestClientEmail;
            customerAddress2.CreateDateTime = DateTime.Now;
            customerAddress2.UpdateDateTime = DateTime.Now;
            addressID2 = sDAL.CreateAddress(customerAddress2);

            //Test data
            sModle2.Id = TestClientID;
            sModle2.FirstName = "Ye";
            sModle2.LastName = "Peng";
            sModle2.UserId = 0;
            sModle2.FaxNumber = "12345678";
            sModle2.MobileNumber = "12345678";
            sModle2.PhoneNumber = "12345678";
            sModle2.CreateUser = TestClientEmail;
            sModle2.CreateDateTime = DateTime.Now;
            sModle2.UpdateUser = TestClientEmail;
            sModle2.UpdateDateTime = DateTime.Now;
            sModle2.FirstStartTime = DateTime.Now;
            sModle2.AddressId = addressID2;

            //create client account
            sDAL2.CreateSBASUserByEmail(TestClientEmail, sModle2);

            UserID1 = sDAL2.GetSBASUserByEmail(TestCustomerEmail).UserId;
            UserID2 = sDAL2.GetSBASUserByEmail(TestClientEmail).UserId;

        }

        // Use ClassCleanup to run code after all tests in a class have run
        /// <summary>
        /// Mies the class cleanup.
        /// </summary>
        [ClassCleanup]
        public static void MyClassCleanup()
        {
            /*
            using (var db = new NPoco.Database(SBAS_DAL.Base.GetConnectionString, DatabaseType.SqlServer2012))
            {
                db.Delete("SBASUser", "UserId", sModle);
            }
             */
            SBAS_DAL.Address sDAL = new SBAS_DAL.Address();
            sDAL.DeleteAddressByID(customerAddress);
            sDAL.DeleteAddressByID(customerAddress2);

            SBAS_DAL.SBASUser sDAL2 = new SBAS_DAL.SBASUser();
            sDAL2.DeleteSBASUserByEmail(TestCustomerEmail, sModle1);
            sDAL2.DeleteSBASUserByEmail(TestClientEmail, sModle2);
        }

        /// <summary>
        /// Tests the create client list.
        /// </summary>
        [TestMethod]
        public void TestCreateClientList()
        {
            SBAS_Core.Model.ClientList c = new SBAS_Core.Model.ClientList() { CustomerId = UserID1, ClientId = UserID2 };
            try
            {
                // call test method
                bool rtn = new SBAS_DAL.ClientList().CreateClientList(c);

                Assert.IsTrue(rtn, "Create Client list failed.");
            }
            finally
            {
                new SBAS_DAL.ClientList().DeleteClientList(c);
            }
        }

        /// <summary>
        /// Tests the delete client list.
        /// </summary>
        [TestMethod]
        public void TestDeleteClientList()
        {
            SBAS_Core.Model.ClientList c = new SBAS_Core.Model.ClientList() { CustomerId = UserID1, ClientId = UserID2 };
            try
            {
                // call test method
                new SBAS_DAL.ClientList().CreateClientList(c);
                bool rtn = new SBAS_DAL.ClientList().DeleteClientList(c);

                Assert.IsTrue(rtn, "Delete Client list failed.");
            }
            finally
            {
                new SBAS_DAL.ClientList().DeleteClientList(c);
            }
        }
    }
}
