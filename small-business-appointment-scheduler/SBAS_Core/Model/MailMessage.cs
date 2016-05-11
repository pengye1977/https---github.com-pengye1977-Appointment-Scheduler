// ***********************************************************************
// Assembly         : SBAS_Core
// Author           : Ye Peng
// Created          : 07-30-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-04-2014
// ***********************************************************************
// <copyright file="MailMessage.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary>This class is responsible for delivering email messages using the Microsoft Hotmail server. 
//          The connection details are stored in the App.config file
//</summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Configuration;
using System.Net;

/// <summary>
/// The Mail namespace.
/// </summary>
namespace SBAS_Core.Mail
{
    /// <summary>
    /// Class MailMessage.
    /// </summary>
    public class MailMessage
    {
        /// <summary>
        /// This field contains the newworkCredentials used for connecting to the server.
        /// </summary>
        private static System.Net.NetworkCredential networkCredentials;
        /// <summary>
        /// This field specifies the actual SMTP server used to connect to
        /// </summary>
        private static System.Net.Mail.SmtpClient smtpClient;
        /// <summary>
        /// This field is the actual mail message that is sent for delivery.
        /// </summary>
        private System.Net.Mail.MailMessage message;

        /// <summary>
        /// This is the "To" field in the email message. It signifies to whom the email is being sent.
        /// </summary>
        /// <value>To.</value>
        public string To { get; set; }
        /// <summary>
        /// This is the "Subject" field of the email message.
        /// </summary>
        /// <value>The subject.</value>
        public string Subject { get; set; }
        /// <summary>
        /// This is the "Body" of the email message.
        /// </summary>
        /// <value>The body.</value>
        public string Body { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailMessage"/> class.
        /// </summary>
        public MailMessage()
        {          
            setupDefaultValues();
            setupNetworkCredentials();
            setupSmtpClient();
        }

        /// <summary>
        /// This is the only public interface to this class. A client will first create the message by 
        /// setting the "To" field, the  "Subject" field and the "Body" and then using this method to send the email
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Send()
        {
            createMessage();
            createMessageViews();
            
            try
            {                
                smtpClient.Send(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }                      
        }

        /// <summary>
        /// These are the default Subject and Body values in case the user of this class forgets to set these values
        /// </summary>
        private void setupDefaultValues()
        {            
            Subject = "Message from SBAS";
            Body = "Hello, You have received a message from SBAS";
        }

        /// <summary>
        /// This method sets up the NetworkCredentials that enable connection to the SMTP client.
        /// </summary>
        private void setupNetworkCredentials()
        {
            // If the networkCredentials is null, it is the first one and has not yet been created.
            if (networkCredentials == null)
            {
                networkCredentials = new System.Net.NetworkCredential();
                networkCredentials.UserName = ConfigurationManager.AppSettings["SMTPUserName"].ToString();
                networkCredentials.Password = ConfigurationManager.AppSettings["SMTPPassword"].ToString(); 
            }
        }

        /// <summary>
        /// This method setups the properties of the SMTP client that will be used to send email to.
        /// </summary>
        private void setupSmtpClient()
        {
            // If the SMTPClient is null, it is the first one and has not yet been created.
            if (smtpClient == null)
            {
                smtpClient = new System.Net.Mail.SmtpClient();
                smtpClient.Host = ConfigurationManager.AppSettings["SMTPHost"].ToString();
                smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["SMTPPort"].ToString());
                smtpClient.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["SMTPEnableSsl"].ToString());
                smtpClient.UseDefaultCredentials = bool.Parse(ConfigurationManager.AppSettings["SMTPDefaultCredentials"].ToString());
                smtpClient.Credentials = networkCredentials;
            }
        }

        /// <summary>
        /// This method actually creates the Mail message and makes it ready for delivery.
        /// </summary>
        /// <exception cref="System.Exception">Cannot create message: The 'To' field cannot be empty</exception>
        private void createMessage()
        {
            if (string.IsNullOrEmpty(this.To))
                throw new Exception("Cannot create message: The 'To' field cannot be empty");

            message = new System.Net.Mail.MailMessage();
            message.From = new System.Net.Mail.MailAddress("SmallBusinessAppointmentScheduler@hotmail.com");
            message.To.Add(this.To);
            message.Subject = this.Subject;
            message.IsBodyHtml = true;
            // The actual body of the message is specified in each separate view that is created for the message
        }

        /// <summary>
        /// This methods add the different message views.
        /// The two popular views that an email client can have is to view an HTML encoded message or a plain text message.
        /// </summary>
        private void createMessageViews()
        {
            var textView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(formatBodyToPlainText(), new System.Net.Mime.ContentType("text/plain"));
            var htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(formatBodyToHTML(), new System.Net.Mime.ContentType("text/html"));
            message.AlternateViews.Add(textView);
            message.AlternateViews.Add(htmlView);
        }

        /// <summary>
        /// This method formats the body of the message into Plain Text
        /// </summary>
        /// <returns>System.String.</returns>
        private string formatBodyToPlainText()
        {
            StringBuilder messageBody = new StringBuilder();
            messageBody.Append(this.Body);
            messageBody.Append(Environment.NewLine);
            messageBody.Append(Environment.NewLine);
            messageBody.Append(Environment.NewLine);
            messageBody.Append(Environment.NewLine);
            messageBody.Append(Environment.NewLine);
            messageBody.Append(Environment.NewLine);
            messageBody.Append("**************************************************************************************");
            messageBody.Append(Environment.NewLine);
            messageBody.Append(Environment.NewLine);
            messageBody.Append("Please do not reply to this email. This is a System generated email.");
            messageBody.Append(Environment.NewLine);
            messageBody.Append(Environment.NewLine);
            messageBody.Append("**************************************************************************************");
            return messageBody.ToString();
        }

        /// <summary>
        /// This method formats the body of the message to HTML
        /// </summary>
        /// <returns>System.String.</returns>
        private string formatBodyToHTML()
        {
            StringBuilder messageBody = new StringBuilder();
            messageBody.Append("<p>");
            messageBody.Append(this.Body);
            messageBody.Append("</p>");
            messageBody.Append("<br />");
            messageBody.Append("<br />"); 
            messageBody.Append("<br />");
            messageBody.Append("<br />");
            messageBody.Append("<br />");
            messageBody.Append("<h5>");
            messageBody.Append("**************************************************************************************");
            messageBody.Append("<br />");
            messageBody.Append("<br />");            
            messageBody.Append("Please do not reply to this email. This is a System generated email.");
            messageBody.Append("<br />");
            messageBody.Append("<br />");
            messageBody.Append("**************************************************************************************");
            messageBody.Append("</h5>");
            return messageBody.ToString();
        }

    } // End class
} // End namespace
