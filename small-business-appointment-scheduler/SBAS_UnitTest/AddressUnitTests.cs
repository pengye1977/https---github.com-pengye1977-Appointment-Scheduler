// ***********************************************************************
// Assembly         : SBAS_UnitTest
// Author           : Ye Peng
// Created          : 06-29-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-12-2014
// ***********************************************************************
// <copyright file="AddressUnitTests.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SBAS_Core.Model;

/// <summary>
/// The SBAS_UnitTest namespace.
/// </summary>
namespace SBAS_UnitTest
{
    /// <summary>
    /// Class AddressUnitTests.
    /// </summary>
    [TestClass]
    public class AddressUnitTests
    {
        /// <summary>
        /// Tests the create address.
        /// </summary>
        [TestMethod]
        public void TestCreateAddress()
        {
            SBAS_Core.Model.Address a = new Address()
            {
                AddressLine1 = "a",
                AddressLine2 = "b",
                CityId = 1,
                StateId = 2,
                ZipCode = "12345",
                Longitude = (decimal)0.0,
                Latitude = (decimal)0.0,
                CreateUser = "f020f3ba-0ca5-4ff0-8e47-25734f4ac41b",
                CreateDateTime = DateTime.Now,
                UpdateUser = "f020f3ba-0ca5-4ff0-8e47-25734f4ac41b",
                UpdateDateTime = DateTime.Now,
            };

            SBAS_DAL.Address aDAL = new SBAS_DAL.Address();
            long id = aDAL.CreateAddress(a);

            //Confirm
            a.AddressId = id;
            SBAS_Core.Model.Address confirm = aDAL.GetAddressById(id);

            Assert.AreEqual(a.AddressLine1, confirm.AddressLine1, "Input:" + a.ToString() + " Confirm:" + confirm.ToString());

            //Delete test data
            aDAL.DeleteAddressByID(a);
        }

        /// <summary>
        /// Tests the get address by identifier.
        /// </summary>
        [TestMethod]
        public void TestGetAddressById()
        {
            SBAS_Core.Model.Address a = new Address()
            {
                AddressLine1 = "a",
                AddressLine2 = "b",
                CityId = 1,
                StateId = 2,
                ZipCode = "12345",
                Longitude = (decimal)0.0,
                Latitude = (decimal)0.0,
                CreateUser = "f020f3ba-0ca5-4ff0-8e47-25734f4ac41b",
                CreateDateTime = DateTime.Now,
                UpdateUser = "f020f3ba-0ca5-4ff0-8e47-25734f4ac41b",
                UpdateDateTime = DateTime.Now,
            };

            SBAS_DAL.Address aDAL = new SBAS_DAL.Address();
            long id = aDAL.CreateAddress(a);

            //COnfirm
            a.AddressId = id;
            SBAS_Core.Model.Address confirm = aDAL.GetAddressById(id);

            Assert.AreEqual(a.AddressLine1, confirm.AddressLine1, "Input:" + a.ToString() + " Confirm:" + confirm.ToString());

            //Delete test data
            aDAL.DeleteAddressByID(a);
        }

        /// <summary>
        /// Tests the delete address by identifier.
        /// </summary>
        [TestMethod]
        public void TestDeleteAddressByID()
        {
            SBAS_Core.Model.Address a = new Address()
            {
                AddressLine1 = "a",
                AddressLine2 = "b",
                CityId = 1,
                StateId = 2,
                ZipCode = "12345",
                Longitude = (decimal)0.0,
                Latitude = (decimal)0.0,
                CreateUser = "f020f3ba-0ca5-4ff0-8e47-25734f4ac41b",
                CreateDateTime = DateTime.Now,
                UpdateUser = "f020f3ba-0ca5-4ff0-8e47-25734f4ac41b",
                UpdateDateTime = DateTime.Now,
            };

            SBAS_DAL.Address aDAL = new SBAS_DAL.Address();
            long id = aDAL.CreateAddress(a);
            a.AddressId = id;

            //Delete test data
            bool rtn = aDAL.DeleteAddressByID(a);

            //confirm
            Assert.AreEqual(rtn, true, "Delete method returned error");

        }

        /// <summary>
        /// Tests the update address.
        /// </summary>
        [TestMethod]
        public void TestUpdateAddress()
        {
            SBAS_Core.Model.Address a = new Address()
            {
                AddressLine1 = "a",
                AddressLine2 = "b",
                CityId = 1,
                StateId = 2,
                ZipCode = "12345",
                Longitude = (decimal)0.0,
                Latitude = (decimal)0.0,
                CreateUser = "f020f3ba-0ca5-4ff0-8e47-25734f4ac41b",
                CreateDateTime = DateTime.Now,
                UpdateUser = "f020f3ba-0ca5-4ff0-8e47-25734f4ac41b",
                UpdateDateTime = DateTime.Now,
            };

            SBAS_DAL.Address aDAL = new SBAS_DAL.Address();
            long id = aDAL.CreateAddress(a);

            a.AddressLine1 = "c";
            SBAS_Core.Model.Address confirm = aDAL.UpdateAddress(a);

            Assert.AreEqual(a.AddressLine1, confirm.AddressLine1, "Input:" + a.ToString() + " Confirm:" + confirm.ToString());

            //Delete test data
            aDAL.DeleteAddressByID(a);
        }

    }
}
