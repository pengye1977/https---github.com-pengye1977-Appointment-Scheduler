// ***********************************************************************
// Assembly         : SBAS_UnitTest
// Author           : Team 1 SWENG 500 Summer of 2014
// Created          : 08-04-2014
//
// Last Modified By : Team 1 SWENG 500 Summer of 2014
// Last Modified On : 08-04-2014
// ***********************************************************************
// <copyright file="EmailUnitTest.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// The SBAS_UnitTest namespace.
/// </summary>
namespace SBAS_UnitTest
{
    /// <summary>
    /// Class EmailUnitTest.
    /// </summary>
    [TestClass]
    public class EmailUnitTest
    {
        /// <summary>
        /// Tests the send.
        /// </summary>
        [TestMethod]
        public void TestSend()
        {
            var mailMessage = new SBAS_Core.Mail.MailMessage();
            mailMessage.To = "SmallBusinessAppointmentScheduler@hotmail.com";
            mailMessage.Subject = "Test Subject";
            mailMessage.Body = "Test Body";
            Assert.AreEqual(true, mailMessage.Send());
        }

        /// <summary>
        /// Tests the send_ without to field.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void TestSend_WithoutToField()
        {
            // It should not be possible to send an email without specifying a "To" field

            var mailMessage = new SBAS_Core.Mail.MailMessage();            
            mailMessage.Subject = "Test Subject";
            mailMessage.Body = "Test Body";
            mailMessage.Send();
        }
    }
}
