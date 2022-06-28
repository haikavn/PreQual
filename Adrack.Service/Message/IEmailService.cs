// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IEmailService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using Adrack.Core;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Message;
using System.Collections.Generic;

namespace Adrack.Service.Message
{
    /// <summary>
    /// Represents a Email Service
    /// </summary>
    public partial interface IEmailService
    {
        #region Methods

        #region Send Email

        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="smtpAccount">Smtp Account</param>
        /// <param name="sender">Sender</param>
        /// <param name="senderName">Sender Name</param>
        /// <param name="recipient">Recipient</param>
        /// <param name="recipientName">Recipient Name</param>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
        /// <param name="replyTo">Reply To</param>
        /// <param name="replyToName">Reply To Name</param>
        /// <param name="cc">Cc</param>
        /// <param name="bcc">Bcc</param>
        /// <param name="priority">Priority</param>
        /// <param name="attachmentId">Attachment Identifier</param>
        /// <param name="attachmentName">Attachment Name</param>
        /// <param name="attachmentPath">Attachment Path</param>
        /// <param name="emailQueueId">Email Queue Id</param>
        void SendEmail(SmtpAccount smtpAccount, string sender, string senderName, string recipient, string recipientName, string subject, string body, string replyTo = null, string replyToName = null, IEnumerable<string> cc = null, IEnumerable<string> bcc = null, int priority = 0, long attachmentId = 0, string attachmentName = null, string attachmentPath = null, EmailOperatorEnums sendingOperator = EmailOperatorEnums.LeadNative, string templateType = "", long emailQueueId = -1);

        /// <summary>
        /// Send Queue Emails.
        /// </summary>
        void SendQueueEmails();

        #endregion Send Email

        #region Membership

        /// <summary>
        /// Send User Welcome Message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Long Item</returns>
        void SendUserWelcomeMessage(User user, long languageId, EmailOperatorEnums emailOperator);


        /// <summary>
        /// Send User Registered Message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Long Item</returns>
        void SendUserRegisteredMessage(User user, long languageId, EmailOperatorEnums emailOperator);

        /// <summary>
        /// Send Network User Registered Message
        /// </summary>
        /// <param name="user"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        void SendNetworkUserRegisteredMessage(User user, long languageId);

        /// <summary>
        /// Sends the user welcome message with validation code.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="languageId"></param>
        void SendEmailUserWelcomeMessageWithValidationCode(string email, string name, string code, long languageId, EmailOperatorEnums emailOPerator);
        
        /// <summary>
        /// Send User Email Validation Message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Long Item</returns>
        void SendUserEmailValidationMessage(User user, long languageId, EmailOperatorEnums emailOperator);

        /// <summary>
        /// Sends the user manager approval message.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="languageId">The language identifier.</param>
        /// <returns>System.Int64.</returns>
        void SendUserManagerApprovalMessage(User user, long languageId);

        /// <summary>
        /// Sends the user manager reject message.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="languageId">The language identifier.</param>
        /// <returns>System.Int64.</returns>
        void SendUserManagerRejectMessage(User user, long languageId, EmailOperatorEnums emailOperator);

        /// <summary>
        /// Sends the user welcome message with validation code.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="name">The name.</param>
        /// <param name="code">The code.</param>
        /// <param name="languageId">The language identifier.</param>
        /// <returns>System.Int64.</returns>
        void SendUserWelcomeMessageWithValidationCode(string email, string name, string code, long languageId, EmailOperatorEnums emailOperator);

        /// <summary>
        /// Sends the user welcome message with username password.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="languageId">The language identifier.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>System.Int64.</returns>
        void SendUserWelcomeMessageWithUsernamePassword(User user, long languageId, EmailOperatorEnums emailOperator, string username = "", string password = "");

        /// <summary>
        /// Send User Forgot Password Message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Long Item</returns>
        void SendUserForgotPasswordMessage(User user, long languageId, EmailOperatorEnums emailOperator);

        /// <summary>
        /// Send User First Password Message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Long Item</returns>
        void SendUserFirstPasswordMessage(User user, long languageId, string password = "");


        /// <summary>
        /// Send User Invitation Message
        /// </summary>
        /// <param name="userEmail">User Email</param>
        /// <param name="invitedUserToken">Token</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Long Item</returns>
        /// <exception cref="ArgumentNullException">userEmail</exception>
        void SendUserInvitationMessage(string userEmail, string invitedUserToken, long languageId, string inviterType, string inviterName);


        /// <summary>
        /// Send Addon Change Status Message
        /// </summary>
        /// <param name="userEmail">User Email</param>
        /// <param name="languageId">Language Identifier</param>
        /// <param name="status">Addon Status</param>
        /// <returns>Long Item</returns>
        /// <exception cref="ArgumentNullException">userEmail</exception>
        void SendAddonChangeStatusMessage(string userEmail, long languageId, string status);

        /// <summary>
        /// Send User Password Change Message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Long Item</returns>
        void SendUserPasswordChangeMessage(User user, long languageId, EmailOperatorEnums emailOperator);
        


        #endregion Membership

        #region Message

        /// <summary>
        /// Send Email Subscription Activation Message
        /// </summary>
        /// <param name="emailSubscription">User</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Long Item</returns>
        void SendEmailSubscriptionActivationMessage(EmailSubscription emailSubscription, long languageId, EmailOperatorEnums emailOperator);

        /// <summary>
        /// Send Email Subscription Deactivation Message
        /// </summary>
        /// <param name="emailSubscription">User</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Long Item</returns>
        void SendEmailSubscriptionDeactivationMessage(EmailSubscription emailSubscription, long languageId, EmailOperatorEnums emailOperator);

        /// <summary>
        /// Send Email Support New Ticket
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>System.Int64.</returns>
        void SendUserNewTicket(User user, long languageId, EmailOperatorEnums emailOperator);

        /// <summary>
        /// Send Email Support New Ticket Message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>System.Int64.</returns>
        void SendUserNewTicketMessage(User user, long languageId, EmailOperatorEnums emailOperator);

        #endregion Message

        /// <summary>
        /// Sends the asteriks server error.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>System.Int64.</returns>
        void SendAsteriksServerError(string email);

        /// <summary>
        /// Sends the cap reach notification.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="email">The email.</param>
        /// <param name="languageId">The language identifier.</param>
        void SendCapReachNotification(string userName, string channelName, string buyerName, string email, EmailOperatorEnums emailOperator, long languageId = 1);

        /// <summary>
        /// Sends the timeout notification.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="email">The email.</param>
        /// <param name="languageId">The language identifier.</param>
        void SendTimeoutNotification(string userName, string channelName, string buyerName, string timeoutMessage, string email, EmailOperatorEnums emailOperator, long languageId = 1);

        void SendLeadEmail(long leadId, string email, string name, long languageId = 1);

        void SendNewTicket(string email, string name, EmailOperatorEnums emailOperator, long languageId = 1);
        void SendNewTicketMessage(string email, string name, EmailOperatorEnums emailOperator, long languageId = 1);
        void SendTicketStatusChange(string email, string name, long languageId = 1);

        #endregion Methods
    }
}