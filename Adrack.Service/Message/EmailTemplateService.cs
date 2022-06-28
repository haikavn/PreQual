// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="EmailTemplateService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Message;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Message
{
    /// <summary>
    /// Represents a Email Template Service
    /// Implements the <see cref="Adrack.Service.Message.IEmailTemplateService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Message.IEmailTemplateService" />
    public partial class EmailTemplateService : IEmailTemplateService
    {
        #region Constants

        /// <summary>
        /// Cache Email Template By Id Key
        /// </summary>
        private const string CACHE_EMAILTEMPLATE_BY_ID_KEY = "App.Cache.EmailTemplate.By.Id-{0}";

        /// <summary>
        /// Cache Email Template By Name Key
        /// </summary>
        private const string CACHE_EMAILTEMPLATE_BY_NAME_KEY = "App.Cache.EmailTemplate.By.Name-{0}";

        /// <summary>
        /// Cache Email Template All Key
        /// </summary>
        private const string CACHE_EMAILTEMPLATE_ALL_KEY = "App.Cache.EmailTemplate.All";

        /// <summary>
        /// Cache Email Template Pattern Key
        /// </summary>
        private const string CACHE_EMAILTEMPLATE_PATTERN_KEY = "App.Cache.EmailTemplate.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Email Template
        /// </summary>
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Email Template Service
        /// </summary>
        /// <param name="emailTemplateRepository">Email Template Repository</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public EmailTemplateService(IRepository<EmailTemplate> emailTemplateRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._emailTemplateRepository = emailTemplateRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Email Template By Id
        /// </summary>
        /// <param name="emailTemplateId">Email Template Identifier</param>
        /// <returns>Email Template Item</returns>
        public virtual EmailTemplate GetEmailTemplateById(long emailTemplateId)
        {
            if (emailTemplateId == 0)
                return null;

            return _emailTemplateRepository.GetById(emailTemplateId);
        }

        /// <summary>
        /// Get Email Template By Name
        /// </summary>
        /// <param name="emailTemplateName">Email Template Name</param>
        /// <returns>Email Template Item</returns>
        /// <exception cref="ArgumentException">emailTemplateName</exception>
        public virtual EmailTemplate GetEmailTemplateByName(string emailTemplateName)
        {
            if (String.IsNullOrWhiteSpace(emailTemplateName))
                throw new ArgumentException("emailTemplateName");

            //string key = string.Format(CACHE_EMAILTEMPLATE_BY_NAME_KEY, emailTemplateName);

            //return _cacheManager.Get(key, () =>
            //{
            var query = from x in _emailTemplateRepository.Table
                        where x.Name == emailTemplateName
                        orderby x.Id
                        select x;

            var emailTemplate = query.ToList();

            return emailTemplate.FirstOrDefault();
            //});
        }

        /// <summary>
        /// Get All Email Templates
        /// </summary>
        /// <returns>Email Template Collection Item</returns>
        public virtual IList<EmailTemplate> GetAllEmailTemplates()
        {
            string key = CACHE_EMAILTEMPLATE_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _emailTemplateRepository.Table
                            orderby x.Name, x.Id
                            select x;

                var emailTemplates = query.ToList();

                return emailTemplates;
            });
        }

        /// <summary>
        /// Insert Email Template
        /// </summary>
        /// <param name="emailTemplate">Email Template</param>
        /// <exception cref="ArgumentNullException">emailTemplate</exception>
        public virtual void InsertEmailTemplate(EmailTemplate emailTemplate)
        {
            if (emailTemplate == null)
                throw new ArgumentNullException("emailTemplate");

            _emailTemplateRepository.Insert(emailTemplate);

            _cacheManager.RemoveByPattern(CACHE_EMAILTEMPLATE_PATTERN_KEY);

            _appEventPublisher.EntityInserted(emailTemplate);
        }

        /// <summary>
        /// Update Email Template
        /// </summary>
        /// <param name="emailTemplate">Email Template</param>
        /// <exception cref="ArgumentNullException">emailTemplate</exception>
        public virtual void UpdateEmailTemplate(EmailTemplate emailTemplate)
        {
            if (emailTemplate == null)
                throw new ArgumentNullException("emailTemplate");

            _emailTemplateRepository.Update(emailTemplate);

            _cacheManager.RemoveByPattern(CACHE_EMAILTEMPLATE_PATTERN_KEY);

            _appEventPublisher.EntityUpdated(emailTemplate);
        }

        /// <summary>
        /// Delete Email Template
        /// </summary>
        /// <param name="emailTemplate">Email Template</param>
        /// <exception cref="ArgumentNullException">emailTemplate</exception>
        public virtual void DeleteEmailTemplate(EmailTemplate emailTemplate)
        {
            if (emailTemplate == null)
                throw new ArgumentNullException("emailTemplate");

            _emailTemplateRepository.Delete(emailTemplate);

            _cacheManager.RemoveByPattern(CACHE_EMAILTEMPLATE_PATTERN_KEY);

            _appEventPublisher.EntityDeleted(emailTemplate);
        }

        public EmailTemplate GetEmailTemplateBySendgridId(string sendgridId)
        {
                var query = from x in _emailTemplateRepository.Table
                            where x.SendgridId == sendgridId
                            orderby x.Name, x.Id
                            select x;

                return query.FirstOrDefault();
        }

        #endregion Methods
    }
}