// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="EmailService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Message;
using Adrack.Service.Infrastructure.ApplicationEvent;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Adrack.Service.Configuration;
using Adrack.Core;
using SendGrid.Helpers.Mail;
using SendGrid;
using Adrack.Core.Domain.SendGrid;

namespace Adrack.Service.Message
{
    public class SendEmailTemlate
    {
        public int Type;

        public string Subject;

        public string Message;

        public SendEmailTemlate(EmailTemplate emailTemplate)
        {
            //this.Type = (int)emailTemplate.type;
            this.Subject = emailTemplate.Subject;
            this.Message = emailTemplate.Body;
        }
    }
    /// <summary>
    /// Represents a Email Service
    /// Implements the <see cref="Adrack.Service.Message.IEmailService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Message.IEmailService" />
    public partial class EmailService : IEmailService
    {
        #region Fields

        /// <summary>
        /// Language Service
        /// </summary>
        private readonly ILanguageService _languageService;

        /// <summary>
        /// Email Tokenizer
        /// </summary>
        private readonly IEmailTokenizer _emailTokenizer;

        /// <summary>
        /// Email Token Dictionary
        /// </summary>
        private readonly IEmailTokenDictionary _emailTokenDictionary;

        /// <summary>
        /// Email Queue Service
        /// </summary>
        private readonly IEmailQueueService _emailQueueService;

        /// <summary>
        /// Smtp Account Service
        /// </summary>
        private readonly ISmtpAccountService _smtpAccountService;

        /// <summary>
        /// Email Template Service
        /// </summary>
        private readonly IEmailTemplateService _emailTemplateService;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly ISettingService _settingService;

        private readonly EmailOperatorEnums _emailProvider;

        public bool CanThrowException { get; set; } = false;

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="emailTemplate">Email Template</param>
        /// <param name="smtpAccount">Smtp Account</param>
        /// <param name="emailToken">Email Token</param>
        /// <param name="languageId">Language Identifier</param>
        /// <param name="recipient">Recipient</param>
        /// <param name="recipientName">Recipient Name</param>
        /// <param name="replyTo">Reply To</param>
        /// <param name="replyToName">Reply To Name</param>
        /// <param name="attachmentName">Attachment Name</param>
        /// <param name="attachmentPath">Attachment Path</param>
        /// <returns>Long Item</returns>
        protected virtual long BuildEmailQueue(EmailTemplate emailTemplate, SmtpAccount smtpAccount, IEnumerable<EmailToken> emailToken, long languageId, string recipient, string recipientName, string replyTo = null, string replyToName = null, string attachmentName = null, string attachmentPath = null)
        {
            // Get Localized Template
            var bcc = emailTemplate.GetLocalized(x => x.Bcc, languageId);
            var subject = emailTemplate.GetLocalized(x => x.Subject, languageId);
            var body = emailTemplate.GetLocalized(x => x.Body, languageId);

            var subjectReplaced = _emailTokenizer.Replace(subject, emailToken, false);
            var bodyReplaced = _emailTokenizer.Replace(body, emailToken, true);

            var emailQueue = new EmailQueue
            {
                SmtpAccountId = smtpAccount.Id,
                AttachmentId = emailTemplate.AttachmentId,
                Sender = smtpAccount.Email,
                SenderName = smtpAccount.DisplayName,
                Recipient = recipient,
                RecipientName = recipientName,
                ReplyTo = replyTo,
                ReplyToName = replyToName,
                Cc = string.Empty,
                Bcc = bcc,
                Subject = subjectReplaced,
                Body = bodyReplaced,
                AttachmentName = attachmentName,
                AttachmentPath = attachmentPath,
                Priority = 5,
                DeliveryAttempts = 0,
                SentOn = null,
                CreatedOn = DateTime.UtcNow
            };

            _emailQueueService.InsertEmailQueue(emailQueue);

            return emailQueue.Id;
        }

        /// <summary>
        /// Get Active Email Template
        /// </summary>
        /// <param name="emailTemplateName">Email Template Name</param>
        /// <returns>Email Template Item</returns>
        protected virtual EmailTemplate GetActiveEmailTemplate(string emailTemplateName)
        {
            var emailTemplateService = _emailTemplateService.GetEmailTemplateByName(emailTemplateName);

            if (emailTemplateService == null)
                return null;

            var active = emailTemplateService.Active;

            if (!active)
                return null;

            return emailTemplateService;
        }

        /// <summary>
        /// Get Email Template Smtp Account
        /// </summary>
        /// <param name="emailTemplate">Email Template</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Smtp Account Item</returns>
        protected virtual SmtpAccount GetEmailTemplateSmtpAccount(EmailTemplate emailTemplate, long languageId)
        {
            var smtpAccountId = emailTemplate.GetLocalized(x => x.SmtpAccountId, languageId);

            var smtpAccount = _smtpAccountService.GetSmtpAccountById(smtpAccountId);

            if (smtpAccount == null)
                smtpAccount = _smtpAccountService.GetAllSmtpAccounts().FirstOrDefault();

            return smtpAccount;
        }

        /// <summary>
        /// Validate Language
        /// </summary>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Language Item</returns>
        /// <exception cref="Exception">No active language could be loaded!</exception>
        protected virtual long ValidateLanguage(long languageId)
        {
            var language = _languageService.GetLanguageById(languageId);

            if (language == null || !language.Published)
            {
                language = _languageService.GetAllLanguages().FirstOrDefault();
            }

            if (language == null)
                throw new Exception("No active language could be loaded!");

            return language.Id;
        }

        #endregion Utilities

        #region Constructor

        /// <summary>
        /// Email Service
        /// </summary>
        /// <param name="languageService">Language Service</param>
        /// <param name="emailTokenizer">Email Tokenizer</param>
        /// <param name="emailTokenDictionary">Email Token Dictionary</param>
        /// <param name="emailQueueService">Email Queue Service</param>
        /// <param name="smtpAccountService">Smtp Account Service</param>
        /// <param name="emailTemplateService">Email Template Service</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public EmailService(ILanguageService languageService, IEmailTokenizer emailTokenizer, IEmailTokenDictionary emailTokenDictionary, IEmailQueueService emailQueueService, ISmtpAccountService smtpAccountService, IEmailTemplateService emailTemplateService, IAppEventPublisher appEventPublisher, ISettingService settingService)
        {
            this._languageService = languageService;
            this._emailTokenizer = emailTokenizer;
            this._emailTokenDictionary = emailTokenDictionary;
            this._emailQueueService = emailQueueService;
            this._smtpAccountService = smtpAccountService;
            this._emailTemplateService = emailTemplateService;
            this._appEventPublisher = appEventPublisher;
            this._settingService = settingService;
            //var emailProviderSetting = _settingService.GetSetting("System.EmailProvider");
            _emailProvider = EmailOperatorEnums.LeadNative;//emailProviderSetting != null ? (EmailOperatorEnums)Convert.ToInt16(emailProviderSetting.Value) : EmailOperatorEnums.LeadNative;
        }

        #endregion Constructor

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
        public virtual void SendEmail(SmtpAccount smtpAccount, string sender, string senderName, string recipient, string recipientName, string subject, string body, string replyTo = null, string replyToName = null, IEnumerable<string> cc = null, IEnumerable<string> bcc = null, int priority = 0, long attachmentId = 0, string attachmentName = null, string attachmentPath = null, EmailOperatorEnums sendingOperator = EmailOperatorEnums.LeadNative, string templateType = "", long emailQueueId = -1)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(sender, senderName)
            };
            mailMessage.To.Add(new MailAddress(recipient, recipientName));

            if (!String.IsNullOrEmpty(replyTo))
            {
                mailMessage.ReplyToList.Add(new MailAddress(replyTo, replyToName));
            }

            if (cc != null)
            {
                foreach (var item in cc.Where(x => !String.IsNullOrWhiteSpace(x)))
                {
                    mailMessage.CC.Add(item.Trim());
                }
            }

            if (bcc != null)
            {
                foreach (var item in bcc.Where(x => !String.IsNullOrWhiteSpace(x)))
                {
                    mailMessage.Bcc.Add(item.Trim());
                }
            }

            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            if (!String.IsNullOrEmpty(attachmentPath) && File.Exists(attachmentPath))
            {
                var attachment = new System.Net.Mail.Attachment(attachmentPath);

                attachment.ContentDisposition.CreationDate = File.GetCreationTime(attachmentPath);
                attachment.ContentDisposition.ModificationDate = File.GetLastWriteTime(attachmentPath);
                attachment.ContentDisposition.ReadDate = File.GetLastAccessTime(attachmentPath);

                if (!String.IsNullOrEmpty(attachmentName))
                {
                    attachment.Name = attachmentName;
                }

                mailMessage.Attachments.Add(attachment);
            }

            if (attachmentId > 0)
            {
                // Write Logic
            }

            using (var smtpClient = new SmtpClient())
            {
                try
                {
                    smtpClient.UseDefaultCredentials = smtpAccount.UseDefaultCredentials;
                    smtpClient.Host = smtpAccount.Host;
                    smtpClient.Port = smtpAccount.Port;
                    smtpClient.EnableSsl = smtpAccount.EnableSsl;
                    smtpClient.Credentials = smtpAccount.UseDefaultCredentials
                        ? CredentialCache.DefaultNetworkCredentials
                        : new NetworkCredential(smtpAccount.Username, smtpAccount.Password);
                    smtpClient.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    if (emailQueueId != -1)
                    {
                         var emailQueue = _emailQueueService.GetEmailQueueById(emailQueueId);
                         if (emailQueue.DeliveryAttempts > 2)
                         {
                             _emailQueueService.DeleteEmailQueue(emailQueue);
                         }
                         else
                         {
                             emailQueue.DeliveryAttempts++;
                             _emailQueueService.UpdateEmailQueue(emailQueue);
                         }
                    }
                    else
                    {
                        var emailQueue = new EmailQueue
                        {
                            SmtpAccountId = smtpAccount.Id,
                            Sender = smtpAccount.Email,
                            SenderName = smtpAccount.DisplayName,
                            Recipient = recipient,
                            RecipientName = recipientName,
                            ReplyTo = replyTo,
                            ReplyToName = replyToName,
                            Cc = (cc != null) ? string.Join(",", ((string[])cc)) : string.Empty,
                            Bcc = (bcc != null) ? string.Join(",", ((string[])bcc)) : string.Empty,
                            Subject = subject,
                            Body = body,
                            AttachmentId = attachmentId,
                            AttachmentName = attachmentName,
                            AttachmentPath = attachmentPath,
                            Priority = 5,
                            DeliveryAttempts = 0,
                            SentOn = null,
                            CreatedOn = DateTime.UtcNow
                        };

                        _emailQueueService.InsertEmailQueue(emailQueue);
                    }

                    if (CanThrowException)
                    {
                        throw ex;
                    }
                }

                if (emailQueueId != -1)
                {
                    var emailQueue = _emailQueueService.GetEmailQueueById(emailQueueId);
                    _emailQueueService.DeleteEmailQueue(emailQueue);
                }
            }
        }



        /// <summary>
        /// Send Queue Emails.
        /// </summary>
        public virtual void SendQueueEmails()
        {
            var emailQueues = _emailQueueService.GetAllEmailQueues();
            foreach(var item in emailQueues)
            {
                var smtpAccount = _smtpAccountService.GetSmtpAccountById(item.SmtpAccountId);

                SendEmail(smtpAccount,
                    item.Sender,
                    item.SenderName,
                    item.Recipient,
                    item.RecipientName,
                    item.Subject,
                    item.Body,
                    item.ReplyTo,
                    item.ReplyToName,
                    item.Cc?.Split(','),
                    item.Bcc?.Split(','),
                    item.Priority,
                    item.AttachmentId,
                    item.AttachmentName,
                    item.AttachmentPath,
                    EmailOperatorEnums.LeadNative,
                    string.Empty,
                    item.Id
                );

            }
        }


        public virtual void ProcessSendgridEmail(string sender, string senderName, string templateType, object templateDataModel)
        {
            System.Threading.Tasks.Task.Run(async () =>
            {
                SendGridMessage msg;
                var apiKey = "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";
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("no-reply@adrack.com", "Adrack lead");
                //var subject = this.Subject.Trim();

                var to = new EmailAddress(sender, senderName);
                //var htmlContent = //this.Message.Trim();
                //if (toAdmin)
                //{
                //    var recepients = to.Email.Split(',').Select(item => new EmailAddress(item, string.Empty)).ToList();
                //    var messageDataObjects = new List<object>();
                //    recepients.ForEach(x =>
                //    {
                //        messageDataObjects.Add(templateDataModel);
                //    });
                //    msg = MailHelper.CreateMultipleTemplateEmailsToMultipleRecipients(from, recepients, templateID, messageDataObjects);
                //}
                //else
                var template = _emailTemplateService.GetEmailTemplateByName(templateType.ToString());

                {
                    msg = MailHelper.CreateSingleTemplateEmail(from, to, template.SendgridId, templateDataModel);
                }
                var response = await client.SendEmailAsync(msg);
            });
        }

        private void SendEmailWithUserToken(User user, EmailTemplate emailTemplate, long languageId, EmailOperatorEnums emailOperator, string password = "")
        {
            var smtpAccount = _smtpAccountService.GetSmtpAccount();
            var emailToken = new List<EmailToken>();

            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);
            _emailTokenDictionary.AddUserToken(emailToken, user);
            if (!string.IsNullOrEmpty(password))
                _emailTokenDictionary.AddCustomToken(emailToken, "User.Password", password);

            var recipient = user.Email;
            var recipientName = user.GetFullName();
            var subject = emailTemplate.GetLocalized(x => x.Subject, languageId);
            var body = emailTemplate.GetLocalized(x => x.Body, languageId);

            var subjectReplaced = _emailTokenizer.Replace(subject, emailToken, false);
            var bodyReplaced = _emailTokenizer.Replace(body, emailToken, true);

            if (emailOperator == EmailOperatorEnums.LeadNative)
                SendEmail(smtpAccount, smtpAccount?.Email, smtpAccount?.DisplayName, recipient, recipientName, subjectReplaced, bodyReplaced);
            else
            {
                var appName = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.name");
                var appUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.url");
                var appEmail = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.email");
                var userEmail = emailToken.FirstOrDefault(x => x.Key.ToLower() == "user.email");
                var userPassword = emailToken.FirstOrDefault(x => x.Key.ToLower() == "user.password");
                var activationUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "user.activationurl");
                ProcessSendgridEmail(recipient, user.GetFullName(), emailTemplate.Name, new SendgridBaseDataModel()
                {
                    CustomerName = "sdfsdf",
                    Email = appEmail == null ? string.Empty : appEmail.Value,
                    ApplicationName = appName == null ? string.Empty : appName.Value,
                    Link = appUrl == null ? string.Empty : appUrl.Value,
                    UserPassword = userPassword == null ? string.Empty : userPassword.Value,
                    UserEmail = userEmail == null ? string.Empty : userEmail.Value,
                    UserActivationUrl = activationUrl == null ? string.Empty : activationUrl.Value,
                });
            }
        }

        #endregion Send Email

        #region Membership

        /// <summary>
        /// Send User Welcome Message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Long Item</returns>
        /// <exception cref="ArgumentNullException">user</exception>
        public virtual void SendUserWelcomeMessage(User user, long languageId, EmailOperatorEnums emailOperator)
        {
            var smtpAccount = _smtpAccountService.GetSmtpAccount();
            if (user == null)
                throw new ArgumentNullException("user");
            var emailToken = new List<EmailToken>();
            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);
            _emailTokenDictionary.AddUserToken(emailToken, user);
            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("User.WelcomeMessage");

            if (emailTemplate == null)
                return;

            if (emailOperator == EmailOperatorEnums.LeadNative)
                SendEmailWithUserToken(user, emailTemplate, languageId, emailOperator: emailOperator);
            else
            {
                var appName = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.name");
                var appUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.url");
                var appEmail = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.email");
                ProcessSendgridEmail(user.Email, user.GetFullName(), emailTemplate.Name, new SendgridBaseDataModel()
                {
                    Email = appEmail == null ? string.Empty : appEmail.Value,
                    ApplicationName = appName == null ? string.Empty : appName.Value,
                    Link = appUrl == null ? string.Empty : appUrl.Value,
                });
            }
        }


        /// <summary>
        /// Send User Registered Message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Long Item</returns>
        /// <exception cref="ArgumentNullException">user</exception>
        public virtual void SendUserRegisteredMessage(User user, long languageId, EmailOperatorEnums emailOperator)
        {
            var smtpAccount = _smtpAccountService.GetSmtpAccount();
            if (user == null)
                throw new ArgumentNullException("user");
            var emailToken = new List<EmailToken>();
            languageId = ValidateLanguage(languageId);
            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);
            _emailTokenDictionary.AddUserToken(emailToken, user);

            var emailTemplate = GetActiveEmailTemplate("User.RegisteredMessage");

            if (emailTemplate == null)
                return;
            if (emailOperator == EmailOperatorEnums.LeadNative)
                SendEmailWithUserToken(user, emailTemplate, languageId, EmailOperatorEnums.LeadNative);
            else
            {
                var appName = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.name");
                var appUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.url");
                var appEmail = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.email");
                ProcessSendgridEmail(user.Email, user.GetFullName(), emailTemplate.Name, new SendgridBaseDataModel()
                {
                    Email = appEmail == null ? string.Empty : appEmail.Value,
                    ApplicationName = appName == null ? string.Empty : appName.Value,
                    Link = appUrl == null ? string.Empty : appUrl.Value,
                    FullName = user.GetFullName()
                });
            }
        }

        /// <summary>
        /// Send Network User Registered Message
        /// </summary>
        /// <param name="user"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public virtual void SendNetworkUserRegisteredMessage(User user, long languageId)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("User.NewNetworkUserActivationMessage");

            if (emailTemplate == null)
                return;

            var smtpAccount = _smtpAccountService.GetSmtpAccount();

            var emailToken = new List<EmailToken>();

            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);
            _emailTokenDictionary.AddUserToken(emailToken, user);

            var recipientName = _settingService.GetSetting("Administrator.Name", false);
            var recipient = _settingService.GetSetting("Administrator.Email", false);
            var subject = emailTemplate.GetLocalized(x => x.Subject, languageId);
            var body = emailTemplate.GetLocalized(x => x.Body, languageId);

            var subjectReplaced = _emailTokenizer.Replace(subject, emailToken, false);
            var bodyReplaced = _emailTokenizer.Replace(body, emailToken, true);
            SendEmail(smtpAccount, smtpAccount?.Email, smtpAccount?.DisplayName, recipient?.Value, recipientName?.Value, subjectReplaced, bodyReplaced);
        }

        /// <summary>
        /// Sends email user welcome message with validation code.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="languageId"></param>
        public virtual void SendEmailUserWelcomeMessageWithValidationCode(string email, string name, string code, long languageId, EmailOperatorEnums emailOperator)
        {
            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("User.WelcomeMessageWithValidationCode");

            if (emailTemplate == null)
                return;

            var smtpAccount = _smtpAccountService.GetSmtpAccount();

            var emailToken = new List<EmailToken>();

            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);
            _emailTokenDictionary.AddCustomToken(emailToken, "User.ActivationCode", code);

            var recipient = email;
            var recipientName = name;
            var subject = emailTemplate.GetLocalized(x => x.Subject, languageId);
            var body = emailTemplate.GetLocalized(x => x.Body, languageId);

            var subjectReplaced = _emailTokenizer.Replace(subject, emailToken, false);
            var bodyReplaced = _emailTokenizer.Replace(body, emailToken, true);

            if(emailOperator == EmailOperatorEnums.LeadNative)
            SendEmail(smtpAccount, smtpAccount?.Email, smtpAccount?.DisplayName, recipient, recipientName, subjectReplaced, bodyReplaced);
            else
            {
                var appName = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.name");
                var appUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.url");
                var appEmail = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.email");
                var activationCode = emailToken.FirstOrDefault(x => x.Key.ToLower() == "user.activationcode");
                ProcessSendgridEmail(recipient, recipientName, emailTemplate.Name, new SendgridBaseDataModel()
                {
                    Email = appEmail == null ? string.Empty : appEmail.Value,
                    ApplicationName = appName == null ? string.Empty : appName.Value,
                    Link = appUrl == null ? string.Empty : appUrl.Value,
                    FullName = recipientName,
                    ActivationCode = activationCode == null ? string.Empty : activationCode.Value,
                });
            }
        }

        /// <summary>
        /// Send User Email Validation Message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Long Item</returns>
        /// <exception cref="ArgumentNullException">user</exception>
        public virtual void SendUserEmailValidationMessage(User user, long languageId, EmailOperatorEnums emailOperator)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            var smtpAccount = _smtpAccountService.GetSmtpAccount();
            if (user == null)
                throw new ArgumentNullException("user");
            var emailToken = new List<EmailToken>();
            languageId = ValidateLanguage(languageId);
            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);
            _emailTokenDictionary.AddUserToken(emailToken, user);


            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("User.EmailValidationMessage");

            if (emailTemplate == null)
                return;
            if (emailOperator == EmailOperatorEnums.LeadNative)
                SendEmailWithUserToken(user, emailTemplate, languageId, EmailOperatorEnums.LeadNative);
            else
            {
                var appName = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.name");
                var appUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.url");
                var appEmail = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.email");
                ProcessSendgridEmail(user.Email, user.GetFullName(), emailTemplate.Name, new SendgridBaseDataModel()
                {
                    Email = appEmail == null ? string.Empty : appEmail.Value,
                    ApplicationName = appName == null ? string.Empty : appName.Value,
                    Link = appUrl == null ? string.Empty : appUrl.Value,
                    FullName = user.GetFullName()
                });
            }
        }

        /// <summary>
        /// Sends the user welcome message with validation code.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="name">The name.</param>
        /// <param name="code">The code.</param>
        /// <param name="languageId">The language identifier.</param>
        /// <returns>System.Int64.</returns>
        public virtual void SendUserWelcomeMessageWithValidationCode(string email, string name, string code, long languageId, EmailOperatorEnums emailOperator)
        {
            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("User.WelcomeMessageWithValidationCode");

            if (emailTemplate == null)
                return;

            var smtpAccount = _smtpAccountService.GetSmtpAccount();
            var emailToken = new List<EmailToken>();

            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);
            _emailTokenDictionary.AddCustomToken(emailToken, "User.ActivationCode", code);

            var recipient = email;
            var recipientName = name;
            var subject = emailTemplate.GetLocalized(x => x.Subject, languageId);
            var body = emailTemplate.GetLocalized(x => x.Body, languageId);

            var subjectReplaced = _emailTokenizer.Replace(subject, emailToken, false);
            var bodyReplaced = _emailTokenizer.Replace(body, emailToken, true);
            if(emailOperator == EmailOperatorEnums.LeadNative)
            SendEmail(smtpAccount, smtpAccount?.Email, smtpAccount?.DisplayName, recipient, recipientName, subjectReplaced, bodyReplaced);
            else
            {
                var appName = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.name");
                var appUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.url");
                var appEmail = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.email");
                var activationCode = emailToken.FirstOrDefault(x => x.Key.ToLower() == "user.activationcode");
                ProcessSendgridEmail(recipient, recipientName, emailTemplate.Name, new SendgridBaseDataModel()
                {
                    Email = appEmail == null ? string.Empty : appEmail.Value,
                    ApplicationName = appName == null ? string.Empty : appName.Value,
                    Link = appUrl == null ? string.Empty : appUrl.Value,
                    FullName = recipientName,
                    ActivationCode = activationCode == null ? string.Empty : activationCode.Value,
                });

            }
        }

        /// <summary>
        /// Sends the user welcome message with username password.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="languageId">The language identifier.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>System.Int64.</returns>
        public virtual void SendUserWelcomeMessageWithUsernamePassword(User user, long languageId, EmailOperatorEnums emailOperator,  string username = "", string password = "")
        {
            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("User.WelcomeMessageWithUsernamePassword");

            if (emailTemplate == null)
                return;

            SendEmailWithUserToken(user, emailTemplate, languageId, emailOperator, password);
        }

        /// <summary>
        /// Sends the user manager approval message.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="languageId">The language identifier.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">user</exception>
        public virtual void SendUserManagerApprovalMessage(User user, long languageId)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("User.ManagerApprovalMessage");

            if (emailTemplate == null)
                return;

            SendEmailWithUserToken(user, emailTemplate, languageId, _emailProvider);
        }

        /// <summary>
        /// Sends the user manager reject message.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="languageId">The language identifier.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">user</exception>
        public virtual void SendUserManagerRejectMessage(User user, long languageId, EmailOperatorEnums emailOperator)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("User.ManagerRejectMessage");

            if (emailTemplate == null)
                return;

            SendEmailWithUserToken(user, emailTemplate, languageId, emailOperator);
        }

        /// <summary>
        /// Send User Forgot Password Message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Long Item</returns>
        /// <exception cref="ArgumentNullException">user</exception>
        public virtual void SendUserForgotPasswordMessage(User user, long languageId, EmailOperatorEnums emailOperator)
        {
            var smtpAccount = _smtpAccountService.GetSmtpAccount();
            if (user == null)
                throw new ArgumentNullException("user");

            var emailToken = new List<EmailToken>();
            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);
            _emailTokenDictionary.AddUserToken(emailToken, user);
            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("User.ForgotPasswordMessage");

            if (emailTemplate == null)
                return;
            if (emailOperator == EmailOperatorEnums.LeadNative)
                SendEmailWithUserToken(user, emailTemplate, languageId, EmailOperatorEnums.LeadNative);
            else
            {
                var appName = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.name");
                var appUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.url");
                var appEmail = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.email");
                var forgotPasswordUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "user.forgotpasswordurl");
                ProcessSendgridEmail(user.Email, user.GetFullName(), emailTemplate.Name, new SendgridBaseDataModel()
                {
                    Email = appEmail == null ? string.Empty : appEmail.Value,
                    ApplicationName = appName == null ? string.Empty : appName.Value,
                    Link = appUrl == null ? string.Empty : appUrl.Value,
                    FullName = user.GetFullName(),
                    ForgotPasswordUrl = forgotPasswordUrl == null ? string.Empty : forgotPasswordUrl.Value,
                });
            }
        }



        /// <summary>
        /// Send User First Password Message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Long Item</returns>
        /// <exception cref="ArgumentNullException">user</exception>
        public virtual void SendUserFirstPasswordMessage(User user, long languageId, string password = "")
        {
            if (user == null)
                throw new ArgumentNullException("user");

            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("User.ChangeFirstPassword");

            if (emailTemplate == null)
                return;

            SendEmailWithUserToken(user, emailTemplate, languageId,EmailOperatorEnums.LeadNative, password);
        }


        /// <summary>
        /// Send User Invitation Message
        /// </summary>
        /// <param name="userEmail">User Email</param>
        /// <param name="invitedUserToken">Token</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Long Item</returns>
        /// <exception cref="ArgumentNullException">userEmail</exception>
        public virtual void SendUserInvitationMessage(string userEmail, string invitedUserToken, long languageId, string inviterType, string inviterName)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
                throw new ArgumentNullException("userEmail");

            if (string.IsNullOrWhiteSpace(inviterType))
                throw new ArgumentNullException("inviterType");


            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("User.UserInvitation"); 
            if (emailTemplate == null)
                return;

            var smtpAccount = _smtpAccountService.GetSmtpAccount();

            var emailToken = new List<EmailToken>();
            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);
            _emailTokenDictionary.AddUserInvitationToken(emailToken, userEmail, invitedUserToken, inviterType, inviterName);

            var subject = emailTemplate.GetLocalized(x => x.Subject, languageId);
            var body = emailTemplate.GetLocalized(x => x.Body, languageId);

            var subjectReplaced = _emailTokenizer.Replace(subject, emailToken, false);
            var bodyReplaced = _emailTokenizer.Replace(body, emailToken, true);

            SendEmail(smtpAccount, smtpAccount?.Email, smtpAccount?.DisplayName, userEmail, "", subjectReplaced, bodyReplaced);
        }


        /// <summary>
        /// Send Addon Change Status Message
        /// </summary>
        /// <param name="userEmail">User Email</param>
        /// <param name="languageId">Language Identifier</param>
        /// <param name="status">Addon Status</param>
        /// <returns>Long Item</returns>
        /// <exception cref="ArgumentNullException">userEmail</exception>
        public virtual void SendAddonChangeStatusMessage(string userEmail, long languageId, string status)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
                throw new ArgumentNullException("userEmail");

            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("Addon.ChangeStatus");
            if (emailTemplate == null)
                return;

            var smtpAccount = _smtpAccountService.GetSmtpAccount();

            var emailToken = new List<EmailToken>();
            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);
            _emailTokenDictionary.AddAddonStatus(emailToken, status);

            var subject = emailTemplate.GetLocalized(x => x.Subject, languageId);
            var body = emailTemplate.GetLocalized(x => x.Body, languageId);

            var subjectReplaced = _emailTokenizer.Replace(subject, emailToken, false);
            var bodyReplaced = _emailTokenizer.Replace(body, emailToken, true);

            SendEmail(smtpAccount, smtpAccount?.Email, smtpAccount?.DisplayName, userEmail, "", subjectReplaced, bodyReplaced);
        }



        /// <summary> 
        /// Send User Password Change Message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Long Item</returns>
        /// <exception cref="ArgumentNullException">user</exception>
        public virtual void SendUserPasswordChangeMessage(User user, long languageId, EmailOperatorEnums emailOperator)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("User.PasswordChangeMessage");

            if (emailTemplate == null)
                return;

            SendEmailWithUserToken(user, emailTemplate, languageId, emailOperator);
        }




        /// <summary>
        /// Sends the asteriks server error.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>System.Int64.</returns>
        public virtual void SendAsteriksServerError(string email)
        {
            long languageId = 1;

            var emailTemplate = GetActiveEmailTemplate("Calls.AsteriskServerCheck");

            if (emailTemplate == null)
                return;

            var smtpAccount = _smtpAccountService.GetSmtpAccount();

            var emailToken = new List<EmailToken>();

            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);

            var subject = emailTemplate.GetLocalized(x => x.Subject, languageId);
            var body = emailTemplate.GetLocalized(x => x.Body, languageId);

            var subjectReplaced = _emailTokenizer.Replace(subject, emailToken, false);
            var bodyReplaced = _emailTokenizer.Replace(body, emailToken, true);
            SendEmail(smtpAccount, smtpAccount?.Email, smtpAccount?.DisplayName, email, "", subjectReplaced, bodyReplaced);

        }

        /// <summary>
        /// Sends the cap reach notification.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="channelName">The chanel name.</param>
        /// <param name="buyerName">The buyer name.</param>
        /// <param name="email">The email.</param>
        /// <param name="languageId">The language identifier.</param>
        public virtual void SendCapReachNotification(string userName, string channelName, string buyerName, string email, EmailOperatorEnums emailOperator, long languageId = 1)
        {
            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("User.CapReachNotification");

            if (emailTemplate == null)
                return;

            var smtpAccount = GetEmailTemplateSmtpAccount(emailTemplate, languageId);

            var emailToken = new List<EmailToken>();

            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);

            try
            {
                if (emailOperator == EmailOperatorEnums.LeadNative)
                {
                    SendEmail(smtpAccount, smtpAccount.Email, smtpAccount.DisplayName, email, buyerName, emailTemplate.Subject, emailTemplate.Body
                            .Replace("%User.FullName%", userName)
                            .Replace("%BuyerChannel.Name%", channelName)
                            .Replace("%Buyer.Name%", buyerName));
                }
                else
                {
                    ProcessSendgridEmail(email, userName, emailTemplate.Name, new SendgridBaseDataModel()
                    {
                        FullName = userName,
                        BuyerChannelName = channelName,
                        BuyerName = buyerName
                    });
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Sends the timeout notification.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="channelName">The chanel name.</param>
        /// <param name="buyerName">The buyer name.</param>
        /// <param name="timeoutMessage">The timeout message.</param>
        /// <param name="email">The email.</param>
        /// <param name="languageId">The language identifier.</param>
        public virtual void SendTimeoutNotification(string userName, string channelName, string buyerName, string timeoutMessage, string email, EmailOperatorEnums emailOperator, long languageId = 1)
        {
            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("User.TimeoutNotification");

            if (emailTemplate == null)
                return;

            var smtpAccount = GetEmailTemplateSmtpAccount(emailTemplate, languageId);

            var emailToken = new List<EmailToken>();

            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);

            try
            {
                if (emailOperator == EmailOperatorEnums.LeadNative)
                {
                    SendEmail(smtpAccount, smtpAccount.Email, smtpAccount.DisplayName, email, buyerName, emailTemplate.Subject, emailTemplate.Body
                        .Replace("%User.FullName%", userName)
                        .Replace("%BuyerChannel.Name%", channelName)
                        .Replace("%Buyer.Name%", buyerName)
                        .Replace("%Timeout.Message%", timeoutMessage));
                }
                else
                {
                    ProcessSendgridEmail(email, userName, emailTemplate.Name, new SendgridBaseDataModel()
                    {
                        BuyerChannelName = channelName,
                        BuyerName = buyerName,
                        FullName = userName
                    });
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Sends Lead email.
        /// </summary>
        /// <param name="leadId"></param>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <param name="languageId"></param>
        public virtual void SendLeadEmail(long leadId, string email, string name, long languageId = 1)
        {
            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("User.NewLeadNotification");

            if (emailTemplate == null)
                return;

            var smtpAccount = GetEmailTemplateSmtpAccount(emailTemplate, languageId);

            var emailToken = new List<EmailToken>();

            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);

            try
            {
                SendEmail(smtpAccount, smtpAccount.Email, smtpAccount.DisplayName, email, name, "You have new lead", emailTemplate.Body);
            }
            catch (Exception)
            {
            }
        }

        #region Support Tickets

        /// <summary>
        /// Send New Ticket.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <param name="languageId"></param>
        public virtual void SendNewTicket(string email, string name, EmailOperatorEnums emailOperator, long languageId = 1)
        {
            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("Support.NewTicket");

            if (emailTemplate == null)
                return;

            var smtpAccount = GetEmailTemplateSmtpAccount(emailTemplate, languageId);

            var emailToken = new List<EmailToken>();

            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);

            try
            {
                if (emailOperator == EmailOperatorEnums.LeadNative)
                    SendEmail(smtpAccount, smtpAccount.Email, smtpAccount.DisplayName, email, name, "You have new Support Ticket", emailTemplate.Body);
                else
                {
                    var appName = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.name");
                    var appUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.url");
                    var appEmail = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.email");
                    var forgotPasswordUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "user.forgotpasswordurl");
                    var emailSubscribtionUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "emailsubscription.activationurl");
                    ProcessSendgridEmail(email, name, emailTemplate.Name, new SendgridBaseDataModel()
                    {
                        Email = appEmail == null ? string.Empty : appEmail.Value,
                        ApplicationName = appName == null ? string.Empty : appName.Value,
                        Link = appUrl == null ? string.Empty : appUrl.Value,
                        ActivationUrl = emailSubscribtionUrl == null ? string.Empty : emailSubscribtionUrl.Value,
                        FullName = name,

                    });
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Send New Ticket Message.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <param name="languageId"></param>
        public virtual void SendNewTicketMessage(string email, string name, EmailOperatorEnums emailOperator, long languageId = 1)
        {
            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("Support.NewTicketMessage");

            if (emailTemplate == null)
                return;

            var smtpAccount = GetEmailTemplateSmtpAccount(emailTemplate, languageId);

            var emailToken = new List<EmailToken>();

            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);

            try
            {
                if (emailOperator == EmailOperatorEnums.LeadNative)
                    SendEmail(smtpAccount, smtpAccount.Email, smtpAccount.DisplayName, email, name, "You have new Support Ticket Message", emailTemplate.Body);
                else
                {
                    var appName = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.name");
                    var appUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.url");
                    var appEmail = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.email");
                    var forgotPasswordUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "user.forgotpasswordurl");
                    var emailSubscribtionUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "emailsubscription.activationurl");
                    ProcessSendgridEmail(email, name, emailTemplate.Name, new SendgridBaseDataModel()
                    {
                        Email = appEmail == null ? string.Empty : appEmail.Value,
                        ApplicationName = appName == null ? string.Empty : appName.Value,
                        Link = appUrl == null ? string.Empty : appUrl.Value,
                        ActivationUrl = emailSubscribtionUrl == null ? string.Empty : emailSubscribtionUrl.Value,
                        FullName = name,
                        
                    });
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Send Ticket Status Change.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <param name="languageId"></param>
        public virtual void SendTicketStatusChange(string email, string name, long languageId = 1)
        {
            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("Support.TicketStatusChanged");

            if (emailTemplate == null)
                return;

            var smtpAccount = GetEmailTemplateSmtpAccount(emailTemplate, languageId);

            var emailToken = new List<EmailToken>();

            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);

            try
            {
                SendEmail(smtpAccount, smtpAccount.Email, smtpAccount.DisplayName, email, name, "You Support Ticket Status changed to ", emailTemplate.Body);
            }
            catch (Exception)
            {
            }
        }
        #endregion




        #endregion Membership

        #region Message

        /// <summary>
        /// Send Email Subscription Activation Message
        /// </summary>
        /// <param name="emailSubscription">User</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Long Item</returns>
        /// <exception cref="ArgumentNullException">emailSubscription</exception>
        public virtual void SendEmailSubscriptionActivationMessage(EmailSubscription emailSubscription, long languageId, EmailOperatorEnums emailOperator)
        {
            if (emailSubscription == null)
                throw new ArgumentNullException("emailSubscription");

            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("EmailSubscription.ActivationMessage");

            if (emailTemplate == null)
                return;

            var smtpAccount = _smtpAccountService.GetSmtpAccount();
            var emailToken = new List<EmailToken>();

            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);
            _emailTokenDictionary.AddEmailSubscriptionToken(emailToken, emailSubscription);

            var recipient = emailSubscription.Email;
            var recipientName = smtpAccount.DisplayName;
            var subject = emailTemplate.GetLocalized(x => x.Subject, languageId);
            var body = emailTemplate.GetLocalized(x => x.Body, languageId);

            var subjectReplaced = _emailTokenizer.Replace(subject, emailToken, false);
            var bodyReplaced = _emailTokenizer.Replace(body, emailToken, true);
            if (emailOperator == EmailOperatorEnums.LeadNative)
                SendEmail(smtpAccount, smtpAccount?.Email, smtpAccount?.DisplayName, recipient, recipientName, subjectReplaced, bodyReplaced);
            else
            {
                var appName = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.name");
                var appUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.url");
                var appEmail = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.email");
                var forgotPasswordUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "user.forgotpasswordurl");
                var emailSubscribtionUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "emailsubscription.activationurl");
                ProcessSendgridEmail(recipient, recipientName, emailTemplate.Name, new SendgridBaseDataModel()
                {
                    Email = appEmail == null ? string.Empty : appEmail.Value,
                    ApplicationName = appName == null ? string.Empty : appName.Value,
                    Link = appUrl == null ? string.Empty : appUrl.Value,
                    ActivationUrl = emailSubscribtionUrl == null ? string.Empty : emailSubscribtionUrl.Value,

                });
            }
        }

        /// <summary>
        /// Send Email Subscription Deactivation Message
        /// </summary>
        /// <param name="emailSubscription">User</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Long Item</returns>
        /// <exception cref="ArgumentNullException">emailSubscription</exception>
        public virtual void SendEmailSubscriptionDeactivationMessage(EmailSubscription emailSubscription, long languageId, EmailOperatorEnums emailOperator)
        {
            if (emailSubscription == null)
                throw new ArgumentNullException("emailSubscription");

            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("EmailSubscription.DeactivationMessage");

            if (emailTemplate == null)
                return;

            var smtpAccount = _smtpAccountService.GetSmtpAccount();
            var emailToken = new List<EmailToken>();

            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);
            _emailTokenDictionary.AddEmailSubscriptionToken(emailToken, emailSubscription);

            var recipient = emailSubscription.Email;
            var recipientName = smtpAccount.DisplayName;
            var subject = emailTemplate.GetLocalized(x => x.Subject, languageId);
            var body = emailTemplate.GetLocalized(x => x.Body, languageId);

            var subjectReplaced = _emailTokenizer.Replace(subject, emailToken, false);
            var bodyReplaced = _emailTokenizer.Replace(body, emailToken, true);

            if (emailOperator == EmailOperatorEnums.LeadNative)
                SendEmail(smtpAccount, smtpAccount?.Email, smtpAccount?.DisplayName, recipient, recipientName, subjectReplaced, bodyReplaced);

            else
            {
                var appName = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.name");
                var appUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.url");
                var appEmail = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.email");
                var forgotPasswordUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "user.forgotpasswordurl");
                var emailSubscribtionUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "emailsubscription.deactivationurl");
                ProcessSendgridEmail(recipient, recipientName, emailTemplate.Name, new SendgridBaseDataModel()
                {
                    Email = appEmail == null ? string.Empty : appEmail.Value,
                    ApplicationName = appName == null ? string.Empty : appName.Value,
                    Link = appUrl == null ? string.Empty : appUrl.Value,
                    DeActivationUrl = emailSubscribtionUrl == null ? string.Empty : emailSubscribtionUrl.Value,

                });
            }
        }

        /// <summary>
        /// Send Email Subscription Deactivation Message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Long Item</returns>
        /// <exception cref="ArgumentNullException">user</exception>
        public virtual void SendUserNewTicket(User user, long languageId, EmailOperatorEnums emailOperator)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            languageId = ValidateLanguage(languageId);

            var emailTemplate = GetActiveEmailTemplate("Support.NewTicket");

            if (emailTemplate == null)
                return;


            SendEmailWithUserToken(user, emailTemplate, languageId, emailOperator:emailOperator);
        }

        /// <summary>
        /// Send Email Subscription Deactivation Message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Language Identifier</param>
        /// <returns>Long Item</returns>
        /// <exception cref="ArgumentNullException">user</exception>
        public virtual void SendUserNewTicketMessage(User user, long languageId, EmailOperatorEnums emailOperator)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            var emailTemplate = GetActiveEmailTemplate("Support.NewTicketMessage");
            var smtpAccount = GetEmailTemplateSmtpAccount(emailTemplate, languageId);

            languageId = ValidateLanguage(languageId);
            var emailToken = new List<EmailToken>();

            _emailTokenDictionary.AddApplicationToken(emailToken, smtpAccount);
           

            if (emailTemplate == null)
                return;

            if(emailOperator==EmailOperatorEnums.LeadNative)
                SendEmailWithUserToken(user, emailTemplate, languageId, EmailOperatorEnums.LeadNative);

            else
                {
                var appName = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.name");
                var appUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.url");
                var appEmail = emailToken.FirstOrDefault(x => x.Key.ToLower() == "application.email");
                var forgotPasswordUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "user.forgotpasswordurl");
                var emailSubscribtionUrl = emailToken.FirstOrDefault(x => x.Key.ToLower() == "emailsubscription.activationurl");
                ProcessSendgridEmail(user.Email, user.GetFullName(), emailTemplate.Name, new SendgridBaseDataModel()
                {
                    Email = appEmail == null ? string.Empty : appEmail.Value,
                    ApplicationName = appName == null ? string.Empty : appName.Value,
                    Link = appUrl == null ? string.Empty : appUrl.Value,
                    ActivationUrl = emailSubscribtionUrl == null ? string.Empty : emailSubscribtionUrl.Value,
                    FullName = user.GetFullName(),

                });
            }
        }

        #endregion Message

        #endregion Methods
    }
}