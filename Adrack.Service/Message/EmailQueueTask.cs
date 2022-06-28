// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="EmailQueueTask.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Message;
using Adrack.Service.Agent;
using Adrack.Service.Audit;
using System;

namespace Adrack.Service.Message
{
    /// <summary>
    /// Represents a Email Queue Task
    /// Implements the <see cref="Adrack.Service.Agent.ITask" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Agent.ITask" />
    public partial class EmailQueueTask : ITask
    {
        #region Fields

        /// <summary>
        /// Email Queue Setting
        /// </summary>
        private readonly EmailQueueSetting _emailQueueSetting;

        /// <summary>
        /// Log Service
        /// </summary>
        private readonly ILogService _logService;

        /// <summary>
        /// Email Service
        /// </summary>
        private readonly IEmailService _emailService;

        /// <summary>
        /// Email Queue Service
        /// </summary>
        private readonly IEmailQueueService _emailQueueService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Email Queue
        /// </summary>
        /// <param name="emailQueueSetting">Email Queue Setting</param>
        /// <param name="logService">Log Service</param>
        /// <param name="emailService">Email Service</param>
        /// <param name="emailQueueService">Email Queue Service</param>
        public EmailQueueTask(EmailQueueSetting emailQueueSetting, ILogService logService, IEmailService emailService, IEmailQueueService emailQueueService)
        {
            this._emailQueueSetting = emailQueueSetting;
            this._logService = logService;
            this._emailService = emailService;
            this._emailQueueService = emailQueueService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Execute
        /// </summary>
        public void Execute()
        {
            var maximumDeliveryAttempts = 8; //  _emailQueueSetting.MaximumDeliveryAttempts;

            var emailQueueService = _emailQueueService.SearchEmailQueues(null, null, null, null, true, false, maximumDeliveryAttempts);

            foreach (var emailQueue in emailQueueService)
            {
                var cc = String.IsNullOrWhiteSpace(emailQueue.Cc) ? null : emailQueue.Cc.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                var bcc = String.IsNullOrWhiteSpace(emailQueue.Bcc) ? null : emailQueue.Bcc.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                try
                {
                    _emailService.SendEmail(emailQueue.SmtpAccount, emailQueue.Sender, emailQueue.SenderName, emailQueue.Recipient, emailQueue.RecipientName, emailQueue.Subject, emailQueue.Body, emailQueue.ReplyTo, emailQueue.ReplyToName, cc, bcc, emailQueue.Priority, emailQueue.AttachmentId, emailQueue.AttachmentName, emailQueue.AttachmentPath);

                    emailQueue.SentOn = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    _logService.Error(string.Format("Error sending e-mail. {0}", ex.Message), ex);
                }
                finally
                {
                    emailQueue.DeliveryAttempts = emailQueue.DeliveryAttempts + 1;
                    _emailQueueService.UpdateEmailQueue(emailQueue);
                }
            }
        }

        #endregion Methods
    }
}