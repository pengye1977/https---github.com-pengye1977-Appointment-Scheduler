// ***********************************************************************
// Assembly         : SBAS_UnitTest
// Author           : Ye Peng
// Created          : 06-08-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="SBASUnitTest.cs" company="PENN STATE MASTERS PROGRAM">
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
    /// Class SBASUnitTest.
    /// </summary>
    [TestClass]
    public class SBASUnitTest
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

        // Use ClassInitialize to run code before running the first test in the class
        /// <summary>
        /// Mies the class initialize.
        /// </summary>
        /// <param name="testContext">The test context.</param>
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext) 
        {
            /*
            using (var db = new NPoco.Database(SBAS_DAL.Base.GetConnectionString, DatabaseType.SqlServer2012))
            {
                //Get Asp.net built-in ID
                var sqlbuilder = Sql.Builder;
                sqlbuilder.Append("SELECT Id FROM dbo.AspNetUsers WHERE Email = @Email", new { @Email = testEmail });
                AspNetUserID = db.Single<string>(sqlbuilder);
                
                //Test data
                sModle.Id = AspNetUserID;
                sModle.FirstName = "Ye";
                sModle.LastName = "Peng";
                sModle.UserId = 0;
                sModle.FaxNumber = "12345678";
                sModle.CreateUser = testEmail;
                sModle.CreateDateTime = DateTime.Now;
                sModle.UpdateUser = testEmail;
                sModle.UpdateDateTime = DateTime.Now;
            }
             */
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

        }

        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        /// <summary>
        /// Mies the class cleanup.
        /// </summary>
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
        }

        /// <summary>
        /// Tests the create sbas user by email.
        /// </summary>
        [TestMethod]
        public void TestCreateSBASUserByEmail()
        {
            
            try
            {
                // call tested method
                SBAS_DAL.SBASUser sDAL = new SBAS_DAL.SBASUser();
                sDAL.CreateSBASUserByEmail(TestCustomerEmail, sModle1);

                //Confirm data is created
                SBAS_Core.Model.SBASUser confirm;
                using (var db = new NPoco.Database(SBAS_DAL.Base.GetConnectionString, DatabaseType.SqlServer2012))
                {
                    var sqlbuilder = Sql.Builder;
                    sqlbuilder.Append(string.Format("SELECT SBASUser.* FROM SBASUser WHERE Id = '{0}' ", TestCustomerID));
                    confirm = db.Single<SBAS_Core.Model.SBASUser>(sqlbuilder);
                }

                Assert.AreEqual(sModle1, confirm, "(Confirm data not equal to input) Input:" + sModle1.ToString() + " OutPut=" + confirm.ToString());
            }
            finally
            {
                new SBAS_DAL.SBASUser().DeleteSBASUserByEmail(TestCustomerEmail, sModle1);
            }
        }


        /// <summary>
        /// Tests the get sbas user by email.
        /// </summary>
        [TestMethod]
        public void TestGetSBASUserByEmail()
        {
            try
            {
                // create test record
                SBAS_DAL.SBASUser sDAL = new SBAS_DAL.SBASUser();
                sDAL.CreateSBASUserByEmail(TestCustomerEmail, sModle1);

                // call tested method
                SBAS_Core.Model.SBASUser recv = sDAL.GetSBASUserByEmail(TestCustomerEmail);

                Assert.AreEqual(sModle1, recv, "(Receive data not equal to input) Input:" + sModle1.ToString() + " OutPut=" + recv.ToString());
            }
            finally
            {
                new SBAS_DAL.SBASUser().DeleteSBASUserByEmail(TestCustomerEmail, sModle1);
            }
        }

        /// <summary>
        /// Tests the get sbas user by identifier.
        /// </summary>
        [TestMethod]
        public void TestGetSBASUserByID()
        {
            try
            {
                // create test record
                SBAS_DAL.SBASUser sDAL = new SBAS_DAL.SBASUser();
                sDAL.CreateSBASUserByEmail(TestCustomerEmail, sModle1);
                SBAS_Core.Model.SBASUser confrim = sDAL.GetSBASUserByEmail(TestCustomerEmail);

                // call tested method
                SBAS_Core.Model.SBASUser recv = sDAL.GetSBASUserById(confrim.UserId);

                Assert.AreEqual(sModle1, recv, "(Receive data not equal to input) Input:" + sModle1.ToString() + " OutPut=" + recv.ToString());
            }
            finally
            {
                new SBAS_DAL.SBASUser().DeleteSBASUserByEmail(TestCustomerEmail, sModle1);
            }
        }

        /// <summary>
        /// Tests the update sbas user by email.
        /// </summary>
        [TestMethod]
        public void TestUpdateSBASUserByEmail()
        {
            try
            {
                // create test record
                SBAS_DAL.SBASUser sDAL = new SBAS_DAL.SBASUser();
                sDAL.CreateSBASUserByEmail(TestCustomerEmail, sModle1);

                //update test object
                sModle1.FaxNumber = "22345678";
                SBAS_Core.Model.SBASUser recv = sDAL.UpdateSBASUserByEmail(TestCustomerEmail, sModle1);

                Assert.AreEqual(sModle1, recv, "(Receive data not equal to update) Input:" + sModle1.ToString() + " OutPut=" + recv.ToString());
            }
            finally
            {
                new SBAS_DAL.SBASUser().DeleteSBASUserByEmail(TestCustomerEmail, sModle1);
                sModle1.FaxNumber = "12345678";
            }
        }

        /// <summary>
        /// Tests the delete sbas user by email.
        /// </summary>
        [TestMethod]
        public void TestDeleteSBASUserByEmail()
        {
            try
            {
                // create test record
                SBAS_DAL.SBASUser sDAL = new SBAS_DAL.SBASUser();
                sDAL.CreateSBASUserByEmail(TestCustomerEmail, sModle1);

                //Get original data
                SBAS_Core.Model.SBASUser confirm1;
                using (var db = new NPoco.Database(SBAS_DAL.Base.GetConnectionString, DatabaseType.SqlServer2012))
                {
                    var sqlbuilder = Sql.Builder;
                    sqlbuilder.Append(string.Format("SELECT SBASUser.* FROM SBASUser WHERE Id = '{0}' ", TestCustomerID));
                    confirm1 = db.Single<SBAS_Core.Model.SBASUser>(sqlbuilder);
                }

                bool rtn = sDAL.DeleteSBASUserByEmail(TestCustomerEmail, confirm1);

                //Confirm data is deleted
                Assert.AreNotEqual(true, "Test object is not deleted successfully.");

            }
            finally
            {
                new SBAS_DAL.SBASUser().DeleteSBASUserByEmail(TestCustomerEmail, sModle1);
            }
        }

        /// <summary>
        /// Tests the get all sbas user_ customer.
        /// </summary>
        [TestMethod]
        public void TestGetAllSBASUser_Customer()
        {
            SBAS_DAL.SBASUser sDAL = new SBAS_DAL.SBASUser();
            List<SBAS_Core.Model.SBASUser> allCustomer = sDAL.GetAllSBASUser_Customer();

            Assert.IsNotNull(allCustomer, "Get all Customer returned Null");
        }

        /// <summary>
        /// Tests the get all sbas user_ client.
        /// </summary>
        [TestMethod]
        public void TestGetAllSBASUser_Client()
        {
            SBAS_DAL.SBASUser sDAL = new SBAS_DAL.SBASUser();
            List<SBAS_Core.Model.SBASUser> allCustomer = sDAL.GetAllSBASUser_Client();

            Assert.IsNotNull(allCustomer, "Get all Client returned Null");
        }

        /// <summary>
        /// Tests the get all customer clients.
        /// </summary>
        [TestMethod]
        public void TestGetAllCustomerClients()
        {
            try
            {
                // create test record
                SBAS_DAL.SBASUser sDAL = new SBAS_DAL.SBASUser();
                sDAL.CreateSBASUserByEmail(TestCustomerEmail, sModle1);
                SBAS_Core.Model.SBASUser confrim = sDAL.GetSBASUserByEmail(TestCustomerEmail);

                // call tested method
                List<SBAS_Core.Model.SBASUser> recv = sDAL.GetAllCustomerClients(confrim.UserId);

                Assert.IsNotNull(recv, "Get all Client by customer ID returned Null");
            }
            finally
            {
                new SBAS_DAL.SBASUser().DeleteSBASUserByEmail(TestCustomerEmail, sModle1);
            }
        }

        /// <summary>
        /// Tests the name of the get user role.
        /// </summary>
        [TestMethod]
        public void TestGetUserRoleName()
        {
            try
            {
                // create test record
                SBAS_DAL.SBASUser.GetUserRoleName(TestCustomerID);

                Assert.AreNotEqual("Customer", "User role should be Customer.");
            }
            finally
            {
                new SBAS_DAL.SBASUser().DeleteSBASUserByEmail(TestCustomerEmail, sModle1);
            }
        }
    }
}
