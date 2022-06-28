// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IEmailTemplateService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Message;
using System.Collections.Generic;

namespace Adrack.Service.Message
{
    /// <summary>
    /// Represents a Email Template Service
    /// </summary>
    public partial interface IEmailTemplateService
    {
        #region Methods

        /// <summary>
        /// Get Email Template By Id
        /// </summary>
        /// <param name="emailTemplateId">Email Template Identifier</param>
        /// <returns>Email Template Item</returns>
        EmailTemplate GetEmailTemplateById(long emailTemplateId);

        /// <summary>
        /// Get Email Template By Name
        /// </summary>
        /// <param name="emailTemplateName">Email Template Name</param>
        /// <returns>Email Template Item</returns>
        EmailTemplate GetEmailTemplateByName(string emailTemplateName);

        /// <summary>
        /// Get All Email Templates
        /// </summary>
        /// <returns>Email Template Collection Item</returns>
        IList<EmailTemplate> GetAllEmailTemplates();

        /// <summary>
        /// Insert Email Template
        /// </summary>
        /// <param name="emailTemplate">Email Template</param>
        void InsertEmailTemplate(EmailTemplate emailTemplate);

        /// <summary>
        /// Update Email Template
        /// </summary>
        /// <param name="emailTemplate">Email Template</param>
        void UpdateEmailTemplate(EmailTemplate emailTemplate);

        /// <summary>
        /// Delete Email Template
        /// </summary>
        /// <param name="emailTemplate">Email Template</param>
        void DeleteEmailTemplate(EmailTemplate emailTemplate);

        EmailTemplate GetEmailTemplateBySendgridId(string sendgridId);

        #endregion Methods
    }
}