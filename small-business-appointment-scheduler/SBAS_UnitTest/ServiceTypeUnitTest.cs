// ***********************************************************************
// Assembly         : SBAS_UnitTest
// Author           : Ye Peng
// Created          : 07-13-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 07-26-2014
// ***********************************************************************
// <copyright file="ServiceTypeUnitTest.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>This test class is responsible for properly testing all the methods in the SBAS_DAL.ServiceType class</summary>
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
    /// Class ServiceTypeUnitTest.
    /// </summary>
    [TestClass]
    public class ServiceTypeUnitTest
    {
        /// <summary>
        /// This is the reference to our SQL database
        /// </summary>
        private static NPoco.Database db;
        /// <summary>
        /// This field is initialized in the class setup. In order to test the service type logic, you first need to create
        /// an address for referential integrity
        /// </summary>
        private static SBAS_Core.Model.Address address;
        /// <summary>
        /// This field is initialized in the class setup. In order to test the service type logic, you first need to create
        /// a user for referential integrity
        /// </summary>
        private static SBAS_Core.Model.SBASUser sbasUser;

        // The following attributes correspond to the CreateUser and UpdateUser fields in the Database
        // The CreateDateTime and UpdateDateTime fields in the database will be set to currentDataTime
        /// <summary>
        /// The create user field
        /// </summary>
        private static string createUserField = "Test Create User";
        /// <summary>
        /// The update user field
        /// </summary>
        private static string updateUserField = "Test Update User";
        /// <summary>
        /// The current date time
        /// </summary>
        private static DateTime currentDateTime = DateTime.Now;

        /// <summary>
        /// This method is ran first and creates the database connection, the user and the user's address
        /// </summary>
        /// <param name="a">A.</param>
        [ClassInitialize]
        public static void ClassSetup(TestContext a)
        {
            db = new NPoco.Database(SBAS_DAL.Base.GetConnectionString, DatabaseType.SqlServer2012);

            address = new SBAS_Core.Model.Address()
            {
                // AddressId is not required since this will be automatically filled in by the SQL server
                AddressLine1 = "123 Test Lane",
                AddressLine2 = "Apt #2",
                CityId = 1,
                StateId = 2,
                ZipCode = "12345",
                Longitude = 96.2M,
                Latitude = 91.3M,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };

            db.Insert("Address", "AddressId", true, address);

            sbasUser = new SBAS_Core.Model.SBASUser()
            {
                // UserId is not requirerd since this will automatically be filled in by the SQL server
                // Id might be required once we enforce referential integrity, for now it works
                CompanyName = "Test Inc.",
                FirstName = "Flying",
                LastName = "Fox",
                FaxNumber = "8002345555",
                PhoneNumber = "8002345555",
                MobileNumber = "8002345555",
                AddressId = address.AddressId,
                FirstStartTime = currentDateTime,
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,
            };

            db.Insert("SBASUser", "UserId", true, sbasUser);
        }

        /// <summary>
        /// This unit test method tests the GetListofUsersServiceTypesByUserId(long userId) method in the SBAS_DAL.ServiceType class. 
        /// This method inserts 3 service types for a user, retrieves the service types for that user, and checks to see if indeed three 
        /// service types were retrieved. It also checks to see if the service types there were retrieved are the same as those service types that were inserted.
        /// </summary>
        [TestMethod]
        public void TestGetListofUsersServiceTypesByUserId()
        {
            const int AMOUNTOFINSERTEDSERVICETYPES = 3;

            using (db)
            {
                string sqlQueryString = String.Format("SELECT COUNT(*) FROM ServiceType WHERE CustomerId = {0}", sbasUser.UserId);

                int serviceTypeCountBeforeInsertion = db.Single<int>(sqlQueryString);
                Assert.AreEqual(0, serviceTypeCountBeforeInsertion);

                var insertedServiceTypes = InsertSampleServiceTypes(AMOUNTOFINSERTEDSERVICETYPES, sbasUser.UserId);
                int serviceTypeCountAfterInsertion = db.Single<int>(sqlQueryString);
                Assert.AreEqual(AMOUNTOFINSERTEDSERVICETYPES, serviceTypeCountAfterInsertion);

                var retrievedServiceTypes = new SBAS_DAL.ServiceType().GetListofUsersServiceTypesByUserId(sbasUser.UserId);

                DeleteSampleServiceTypes(insertedServiceTypes);

                // A possibility could exist where items in each list are the same but in different order
                // To avoid errors, it is necessay to sort these lists before comparing values
                insertedServiceTypes.Sort(delegate(SBAS_Core.Model.ServiceType i1, SBAS_Core.Model.ServiceType i2)
                {
                    return i1.ServiceTypeId.CompareTo(i2.ServiceTypeId);
                });

                retrievedServiceTypes.Sort(delegate(SBAS_Core.Model.ServiceType i1, SBAS_Core.Model.ServiceType i2)
                {
                    return i1.ServiceTypeId.CompareTo(i2.ServiceTypeId);
                });
    
                for (int i = 0; i < insertedServiceTypes.Count; i++)
                {                    
                    Assert.AreEqual(insertedServiceTypes[0].ServiceTypeId, retrievedServiceTypes[0].ServiceTypeId);
                    Assert.AreEqual(insertedServiceTypes[0].CustomerId, retrievedServiceTypes[0].CustomerId);
                    Assert.AreEqual(insertedServiceTypes[0].Description, retrievedServiceTypes[0].Description);
                    Assert.AreEqual(insertedServiceTypes[0].NameOfService, retrievedServiceTypes[0].NameOfService);

                    Assert.AreEqual(insertedServiceTypes[0].UpdateUser, retrievedServiceTypes[0].UpdateUser);
                    Assert.AreEqual(insertedServiceTypes[0].UpdateDateTime.Year, retrievedServiceTypes[0].UpdateDateTime.Year);
                    Assert.AreEqual(insertedServiceTypes[0].UpdateDateTime.Month, retrievedServiceTypes[0].UpdateDateTime.Month);
                    Assert.AreEqual(insertedServiceTypes[0].UpdateDateTime.Day, retrievedServiceTypes[0].UpdateDateTime.Day);
                    Assert.AreEqual(insertedServiceTypes[0].UpdateDateTime.Hour, retrievedServiceTypes[0].UpdateDateTime.Hour);
                    Assert.AreEqual(insertedServiceTypes[0].UpdateDateTime.Minute, retrievedServiceTypes[0].UpdateDateTime.Minute);
                    Assert.AreEqual(insertedServiceTypes[0].UpdateDateTime.Second, retrievedServiceTypes[0].UpdateDateTime.Second);

                    Assert.AreEqual(insertedServiceTypes[0].CreateUser, retrievedServiceTypes[0].CreateUser);
                    Assert.AreEqual(insertedServiceTypes[0].CreateDateTime.Year, retrievedServiceTypes[0].CreateDateTime.Year);
                    Assert.AreEqual(insertedServiceTypes[0].CreateDateTime.Month, retrievedServiceTypes[0].CreateDateTime.Month);
                    Assert.AreEqual(insertedServiceTypes[0].CreateDateTime.Day, retrievedServiceTypes[0].CreateDateTime.Day);
                    Assert.AreEqual(insertedServiceTypes[0].CreateDateTime.Hour, retrievedServiceTypes[0].CreateDateTime.Hour);
                    Assert.AreEqual(insertedServiceTypes[0].CreateDateTime.Minute, retrievedServiceTypes[0].CreateDateTime.Minute);
                    Assert.AreEqual(insertedServiceTypes[0].CreateDateTime.Second, retrievedServiceTypes[0].CreateDateTime.Second);
                } // End For                 
            } // End using
        }

        /// <summary>
        /// This unit test method tests the CreateServideType(SBAS_Core.Model.ServiceType serviceType) method in the SBAS_DAL.ServiceType class. 
        /// This method creates a service type, retrieves the newly created service type and inspects the created service type with the inserted 
        /// service type to make sure that they are equal.
        /// </summary>
        [TestMethod]
        public void TestCreateServiceType()
        {           
            var serviceTypeToInsert = new SBAS_Core.Model.ServiceType()
            {                 
                // ServiceTypeId will be populated by NPoco during insertion
                CustomerId = sbasUser.UserId,
                Description = "Test Description",
                NameOfService = "Test Service",
                CreateUser = createUserField,
                CreateDateTime = currentDateTime,
                UpdateUser = updateUserField,
                UpdateDateTime = currentDateTime,

            };

            var insertedServiceType = new SBAS_DAL.ServiceType().CreateServiceType(serviceTypeToInsert);
            var retrievedServiceType = GetSampleServiceTypeById(insertedServiceType.ServiceTypeId);

            DeleteSampleServiceType(insertedServiceType);

            Assert.AreEqual(insertedServiceType.ServiceTypeId, retrievedServiceType.ServiceTypeId);
            Assert.AreEqual(insertedServiceType.NameOfService, retrievedServiceType.NameOfService);
            Assert.AreEqual(insertedServiceType.Description, retrievedServiceType.Description);
            Assert.AreEqual(insertedServiceType.CustomerId, retrievedServiceType.CustomerId);
            Assert.AreEqual(insertedServiceType.CreateUser, retrievedServiceType.CreateUser);
            Assert.AreEqual(insertedServiceType.UpdateUser, retrievedServiceType.UpdateUser);

            // UpdateDateTime might not be different, since we do not know what the UpdateDateTime will be until it is inserted in the test method
            // Most likely the miliseconds will be off
            Assert.AreEqual(insertedServiceType.UpdateUser, retrievedServiceType.UpdateUser);
            Assert.AreEqual(insertedServiceType.UpdateDateTime.Year, retrievedServiceType.UpdateDateTime.Year);
            Assert.AreEqual(insertedServiceType.UpdateDateTime.Month, retrievedServiceType.UpdateDateTime.Month);
            Assert.AreEqual(insertedServiceType.UpdateDateTime.Day, retrievedServiceType.UpdateDateTime.Day);
            Assert.AreEqual(insertedServiceType.UpdateDateTime.Hour, retrievedServiceType.UpdateDateTime.Hour);
            Assert.AreEqual(insertedServiceType.UpdateDateTime.Minute, retrievedServiceType.UpdateDateTime.Minute);
            Assert.AreEqual(insertedServiceType.UpdateDateTime.Second, retrievedServiceType.UpdateDateTime.Second);

            // CreateDateTime might not be different, since we do not know what the CreateDateTime will be until it is inserted in the test method
            // Most likely the miliseconds will be off
            Assert.AreEqual(insertedServiceType.CreateUser, retrievedServiceType.CreateUser);
            Assert.AreEqual(insertedServiceType.CreateDateTime.Year, retrievedServiceType.CreateDateTime.Year);
            Assert.AreEqual(insertedServiceType.CreateDateTime.Month, retrievedServiceType.CreateDateTime.Month);
            Assert.AreEqual(insertedServiceType.CreateDateTime.Day, retrievedServiceType.CreateDateTime.Day);
            Assert.AreEqual(insertedServiceType.CreateDateTime.Hour, retrievedServiceType.CreateDateTime.Hour);
            Assert.AreEqual(insertedServiceType.CreateDateTime.Minute, retrievedServiceType.CreateDateTime.Minute);
            Assert.AreEqual(insertedServiceType.CreateDateTime.Second, retrievedServiceType.CreateDateTime.Second);
        }

        /// <summary>
        /// This unit test method tests the GetServiceTypeByServiceTypeId(long serviceTypeId) method in the SBAS_DAL.ServiceType class. This 
        /// method creates a service type, retrieves the service type by using the appropriate method, and compares the inserted service type 
        /// with the retrieved service type for equivalence.	
        /// </summary>
        [TestMethod]
        public void TestGetServiceTypeByServiceTypeId()
        {            
            var insertedServiceType = InsertSampleServiceTypes(1, sbasUser.UserId);
            var retrievedServiceType = new SBAS_DAL.ServiceType().GetServiceTypeByServiceTypeId(insertedServiceType[0].ServiceTypeId);
                        
            DeleteSampleServiceTypes(insertedServiceType);

            Assert.AreEqual(insertedServiceType[0].ServiceTypeId, retrievedServiceType.ServiceTypeId);
            Assert.AreEqual(insertedServiceType[0].CustomerId, retrievedServiceType.CustomerId);
            Assert.AreEqual(insertedServiceType[0].NameOfService, retrievedServiceType.NameOfService);
            Assert.AreEqual(insertedServiceType[0].Description, retrievedServiceType.Description);

            // UpdateDateTime might not be different, since we do not know what the UpdateDateTime will be until it is inserted in the test method
            // Most likely the miliseconds will be off
            Assert.AreEqual(insertedServiceType[0].UpdateUser, retrievedServiceType.UpdateUser);
            Assert.AreEqual(insertedServiceType[0].UpdateDateTime.Year, retrievedServiceType.UpdateDateTime.Year);
            Assert.AreEqual(insertedServiceType[0].UpdateDateTime.Month, retrievedServiceType.UpdateDateTime.Month);
            Assert.AreEqual(insertedServiceType[0].UpdateDateTime.Day, retrievedServiceType.UpdateDateTime.Day);
            Assert.AreEqual(insertedServiceType[0].UpdateDateTime.Hour, retrievedServiceType.UpdateDateTime.Hour);
            Assert.AreEqual(insertedServiceType[0].UpdateDateTime.Minute, retrievedServiceType.UpdateDateTime.Minute);
            Assert.AreEqual(insertedServiceType[0].UpdateDateTime.Second, retrievedServiceType.UpdateDateTime.Second);

            // CreateDateTime might not be different, since we do not know what the CreateDateTime will be until it is inserted in the test method
            // Most likely the miliseconds will be off
            Assert.AreEqual(insertedServiceType[0].CreateUser, retrievedServiceType.CreateUser);
            Assert.AreEqual(insertedServiceType[0].CreateDateTime.Year, retrievedServiceType.CreateDateTime.Year);
            Assert.AreEqual(insertedServiceType[0].CreateDateTime.Month, retrievedServiceType.CreateDateTime.Month);
            Assert.AreEqual(insertedServiceType[0].CreateDateTime.Day, retrievedServiceType.CreateDateTime.Day);
            Assert.AreEqual(insertedServiceType[0].CreateDateTime.Hour, retrievedServiceType.CreateDateTime.Hour);
            Assert.AreEqual(insertedServiceType[0].CreateDateTime.Minute, retrievedServiceType.CreateDateTime.Minute);
            Assert.AreEqual(insertedServiceType[0].CreateDateTime.Second, retrievedServiceType.CreateDateTime.Second);
        }

        /// <summary>
        /// This method tests the DeleteServiceTypeByServiceTypeId method
        /// We first insert one service type for a user, and then we use the test method to delete this service type
        /// We verify that only one service item was deleted by the method and that the correct item was deleted
        /// </summary>
        /// <exception cref="System.Exception">Error while deleting Service Type</exception>
        [TestMethod]        
        public void TestDeleteServiceTypeByServiceTypeId_ServiceTypeIdValid()
        {            
            using (db)
            {
                var insertedServiceType = InsertSampleServiceTypes(1, sbasUser.UserId);
                string sqlQueryString = String.Format("SELECT COUNT(*) FROM ServiceType WHERE CustomerId = {0}", sbasUser.UserId);
                int serviceTypeCountBeforeDeletion = db.Single<int>(sqlQueryString);

                try
                {
                    new SBAS_DAL.ServiceType().DeleteServiceTypeByServiceTypeId(insertedServiceType[0].ServiceTypeId);
                }
                catch (Exception ex)
                {
                    DeleteSampleServiceTypes(insertedServiceType);
                    throw new Exception("Error while deleting Service Type");
                }

                int serviceTypeCountAfterDeletion = db.Single<int>(sqlQueryString);

                // Make sure that the correct item was deleted
                sqlQueryString = String.Format("SELECT COUNT(*) FROM ServiceType WHERE ServiceTypeId = {0}", insertedServiceType[0].ServiceTypeId);
                int serviceTypeCount = db.Single<int>(sqlQueryString);

                Assert.AreEqual(1, serviceTypeCountBeforeDeletion - serviceTypeCountAfterDeletion);
                Assert.AreEqual(0, serviceTypeCount);
            } 
        }

        /// <summary>
        /// This method tests the DeleteServiceTypeByServiceTypeId method by passing in an invalid serviceTypeId
        /// In this method an invalid service type Id is an id that has no associated service type
        /// Why is a user trying to delete a ServiceType that does not exist? When this happens, an error should be thrown.
        /// We first have to find a random serviceTypeId and make sure that there are no service types that have this Id - this assures us that we have an invalid Id
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestDeleteServiceTypeByServiceTypeId_ServiceTypeIdNotValid()
        {                      
            bool foundGoodRandomServiceTypeId = false;
            while (!foundGoodRandomServiceTypeId)
            {
                // Create random Service Type Id
                var buffer = new byte[sizeof(Int32)];
                new Random().NextBytes(buffer);
                var randomServiceTypeId = BitConverter.ToUInt32(buffer, 0);

                // Find out if there is a ServiceType with this associated random serviceTypeId
                // If there is no associated ServiceType with this Id, we know that we found an invalidId, we can now try to call the test method with the invalid id.
                string sqlQueryString = String.Format("SELECT COUNT(*) FROM ServiceType WHERE ServiceTypeId = {0};", randomServiceTypeId);
                int serviceTypeCount = db.Single<int>(sqlQueryString);
                if(serviceTypeCount == 0)
                {
                    foundGoodRandomServiceTypeId = true;
                    new SBAS_DAL.ServiceType().DeleteServiceTypeByServiceTypeId((long)randomServiceTypeId);
                }
            }                                    
        }


        /// <summary>
        /// This method tests the DeleteServiceTypeByServiceTypeId method by passing in an invalid serviceTypeId
        /// In this method an invalid service type Id is an id that has a negative value
        /// Why is a user trying to delete a ServiceType that has a negative value? In our database, all ServiceTypeIds are postive values.
        /// When this happens, an error should be thrown.    
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestDeleteServiceTypeByServiceTypeId_ServiceTypeIdNegativeValue()
        {                       
            new SBAS_DAL.ServiceType().DeleteServiceTypeByServiceTypeId(-1);         
            
        }

        /// <summary>
        /// This method tests the DeleteServiceTypeByServiceTypeId method by passing in an invalid serviceTypeId
        /// In this method an invalid service type Id is an id that has a value of zero
        /// Why is a user trying to delete a ServiceType that has a value of zero? In our database, all ServiceTypeIds are postive values.
        /// When this happens, an error should be thrown. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestDeleteServiceTypeByServiceTypeId_ServiceTypeIdZeroValue()
        {                    
            new SBAS_DAL.ServiceType().DeleteServiceTypeByServiceTypeId(0);
        }

        /// <summary>
        /// Tests to make sure that the UpdateUsersServiceType method properly updates the service type
        /// This is accomplished by creating a service type for a user
        /// Updating the service type, retrieving the newly updated service type and checking to see that all the fields were properly updated           
        /// </summary>
        [TestMethod]
        public void TestUpdateUsersServiceType()
        {  
            var insertedServiceType = InsertSampleServiceTypes(1, sbasUser.UserId);

            insertedServiceType[0].Description = "New Test Description";
            insertedServiceType[0].NameOfService = "New Test Service Name";
            insertedServiceType[0].CreateUser = "New Test Create User";
            insertedServiceType[0].CreateDateTime = DateTime.Now;                 
            insertedServiceType[0].UpdateUser = "New Test Update User";
            insertedServiceType[0].UpdateDateTime = DateTime.Now;

            new SBAS_DAL.ServiceType().UpdateUsersServiceType(insertedServiceType[0]);

            SBAS_Core.Model.ServiceType retrievedServiceType; 
            using (db)
            {
                string sqlQueryString = string.Format("SELECT * FROM ServiceType WHERE CustomerID = {0}", sbasUser.UserId);
                retrievedServiceType = db.Single<SBAS_Core.Model.ServiceType>(sqlQueryString);
            }

            DeleteSampleServiceType(insertedServiceType[0]);

            Assert.AreEqual(insertedServiceType[0].ServiceTypeId, retrievedServiceType.ServiceTypeId);
            Assert.AreEqual(insertedServiceType[0].CreateUser, retrievedServiceType.CreateUser);
            Assert.AreEqual(insertedServiceType[0].UpdateUser, retrievedServiceType.UpdateUser);
            Assert.AreEqual(insertedServiceType[0].Description, retrievedServiceType.Description);
            Assert.AreEqual(insertedServiceType[0].NameOfService, retrievedServiceType.NameOfService);
            Assert.AreEqual(insertedServiceType[0].CustomerId, retrievedServiceType.CustomerId);

            // UpdateDateTime might not be different, since we do not know what the UpdateDateTime will be until it is inserted in the test method
            // Most likely the miliseconds will be off
            Assert.AreEqual(insertedServiceType[0].UpdateUser, retrievedServiceType.UpdateUser);
            Assert.AreEqual(insertedServiceType[0].UpdateDateTime.Year, retrievedServiceType.UpdateDateTime.Year);
            Assert.AreEqual(insertedServiceType[0].UpdateDateTime.Month, retrievedServiceType.UpdateDateTime.Month);
            Assert.AreEqual(insertedServiceType[0].UpdateDateTime.Day, retrievedServiceType.UpdateDateTime.Day);
            Assert.AreEqual(insertedServiceType[0].UpdateDateTime.Hour, retrievedServiceType.UpdateDateTime.Hour);
            Assert.AreEqual(insertedServiceType[0].UpdateDateTime.Minute, retrievedServiceType.UpdateDateTime.Minute);
            Assert.AreEqual(insertedServiceType[0].UpdateDateTime.Second, retrievedServiceType.UpdateDateTime.Second);

            // CreateDateTime might not be different, since we do not know what the CreateDateTime will be until it is inserted in the test method
            // Most likely the miliseconds will be off
            Assert.AreEqual(insertedServiceType[0].CreateUser, retrievedServiceType.CreateUser);
            Assert.AreEqual(insertedServiceType[0].CreateDateTime.Year, retrievedServiceType.CreateDateTime.Year);
            Assert.AreEqual(insertedServiceType[0].CreateDateTime.Month, retrievedServiceType.CreateDateTime.Month);
            Assert.AreEqual(insertedServiceType[0].CreateDateTime.Day, retrievedServiceType.CreateDateTime.Day);
            Assert.AreEqual(insertedServiceType[0].CreateDateTime.Hour, retrievedServiceType.CreateDateTime.Hour);
            Assert.AreEqual(insertedServiceType[0].CreateDateTime.Minute, retrievedServiceType.CreateDateTime.Minute);
            Assert.AreEqual(insertedServiceType[0].CreateDateTime.Second, retrievedServiceType.CreateDateTime.Second);
        }

        /// <summary>
        /// This method is a private helper method that is designed to insert a specified amount of service types to a certain user
        /// </summary>
        /// <param name="amountToInsert">The amount to insert.</param>
        /// <param name="associatedUserId">The associated user identifier.</param>
        /// <returns>List&lt;SBAS_Core.Model.ServiceType&gt;.</returns>
        private static List<SBAS_Core.Model.ServiceType> InsertSampleServiceTypes(int amountToInsert, long associatedUserId)
        {
            var serviceTypes = new List<SBAS_Core.Model.ServiceType>();

            for (int i = 0; i < amountToInsert; i++)
            {
                serviceTypes.Add(new SBAS_Core.Model.ServiceType()
                {
                    // ServiceTypeId will be populated by NPoco during insertion
                    CustomerId = associatedUserId,
                    Description = "TestDescription",
                    NameOfService = "TestService",
                    CreateUser = createUserField,
                    CreateDateTime = currentDateTime,
                    UpdateUser = updateUserField,
                    UpdateDateTime = currentDateTime,
                });
            }

            using (db)
            {
                for (int i = 0; i < serviceTypes.Count; i++)
                {
                    db.Insert("ServiceType", "ServiceTypeId", true, serviceTypes[i]);
                }
            }

            return serviceTypes;
        }

        /// <summary>
        /// This method is a private helper method that is designed to delete a list of service types
        /// </summary>
        /// <param name="insertedServiceTypes">The inserted service types.</param>
        private static void DeleteSampleServiceTypes(List<SBAS_Core.Model.ServiceType> insertedServiceTypes)
        {
            using (db)
            {
                foreach (SBAS_Core.Model.ServiceType serviceType in insertedServiceTypes)
                {
                    db.Delete("ServiceType", "ServiceTypeId", serviceType, serviceType.ServiceTypeId);
                }
            }
        }

        /// <summary>
        /// This method is a private helper method that is designed to delete a certain service type
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        private static void DeleteSampleServiceType(SBAS_Core.Model.ServiceType serviceType)
        {
            using (db)
            {
                db.Delete("ServiceType", "ServiceTypeId", serviceType, serviceType.ServiceTypeId);

            }
        }

        /// <summary>
        /// This method is a private helper method that is designed to retrieve a service type based on the service type ID
        /// </summary>
        /// <param name="serviceTypeId">The service type identifier.</param>
        /// <returns>SBAS_Core.Model.ServiceType.</returns>
        private static SBAS_Core.Model.ServiceType GetSampleServiceTypeById(long serviceTypeId)
        {
            using (db)
            {
                string sqlQueryString = string.Format(@"SELECT * FROM ServiceType WHERE ServiceTypeId = {0}", serviceTypeId);
                var serviceType = db.Single<SBAS_Core.Model.ServiceType>(sqlQueryString);
                return serviceType;
            }
        }


        /// <summary>
        /// This method is the last method that is run during testing. It is responsible for deleting the user and address
        /// that were created during the class setup.
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanUp()
        {
            // These are the only two classes that are shared amongst all the methods, and they need to be deleted
            // from the database when the class is finished running the methods
            using (db)
            {
                db.Delete("SBASUser", "UserId", sbasUser, sbasUser.UserId);
                db.Delete("Address", "AddressId", address, address.AddressId);
            }
        }


    }
}