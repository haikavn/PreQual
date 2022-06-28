// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="SmtpAccountService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Message;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Configuration;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Message
{
    /// <summary>
    /// Represents a Simple Mail Transfer Protocol Account Service
    /// Implements the <see cref="Adrack.Service.Message.ISmtpAccountService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Message.ISmtpAccountService" />
    public partial class SmtpAccountService : ISmtpAccountService
    {
        #region Constants

        /// <summary>
        /// Cache Smtp Account By Id Key
        /// </summary>
        private const string CACHE_SMTPACCOUNT_BY_ID_KEY = "App.Cache.SmtpAccount.By.Id-{0}";

        /// <summary>
        /// Cache Smtp Account All Key
        /// </summary>
        private const string CACHE_SMTPACCOUNT_ALL_KEY = "App.Cache.SmtpAccount.All";

        /// <summary>
        /// Cache Smtp Account Pattern Key
        /// </summary>
        private const string CACHE_SMTPACCOUNT_PATTERN_KEY = "App.Cache.SmtpAccount.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Smtp Account
        /// </summary>
        private readonly IRepository<SmtpAccount> _smtpAccountRepository;

        /// <summary>
        /// Smtp Account
        /// </summary>
        private readonly ISettingService _settingService;

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
        /// Smtp Account Service
        /// </summary>
        /// <param name="smtpAccountRepository">Smtp Account Repository</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public SmtpAccountService(IRepository<SmtpAccount> smtpAccountRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, ISettingService settingService)
        {
            this._smtpAccountRepository = smtpAccountRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._settingService = settingService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Smtp Account By Id
        /// </summary>
        /// <param name="smtpAccountId">Smtp Account Identifier</param>
        /// <returns>SmtpAccount Item</returns>
        public virtual SmtpAccount GetSmtpAccountById(long smtpAccountId)
        {
            if (smtpAccountId == 0)
                return null;

            return _smtpAccountRepository.GetById(smtpAccountId);
        }

        /// <summary>
        /// Get All Smtp Accounts
        /// </summary>
        /// <returns>Smtp Account Collection Item</returns>
        public virtual IList<SmtpAccount> GetAllSmtpAccounts()
        {
            string key = CACHE_SMTPACCOUNT_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _smtpAccountRepository.Table
                            orderby x.DisplayName, x.Id
                            select x;

                var smtpAccounts = query.ToList();

                return smtpAccounts;
            });
        }

        /// <summary>
        /// Insert Smtp Account
        /// </summary>
        /// <param name="smtpAccount">Smtp Account</param>
        /// <exception cref="ArgumentNullException">smtpAccount</exception>
        public virtual void InsertSmtpAccount(SmtpAccount smtpAccount)
        {
            if (smtpAccount == null)
                throw new ArgumentNullException("smtpAccount");

            _smtpAccountRepository.Insert(smtpAccount);

            _cacheManager.RemoveByPattern(CACHE_SMTPACCOUNT_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(smtpAccount);
        }

        /// <summary>
        /// Update Smtp Account
        /// </summary>
        /// <param name="smtpAccount">Smtp Account</param>
        /// <exception cref="ArgumentNullException">smtpAccount</exception>
        public virtual void UpdateSmtpAccount(SmtpAccount smtpAccount)
        {
            if (smtpAccount == null)
                throw new ArgumentNullException("smtpAccount");
            InsertOrUpdateSetting("Settings.SMTP.DisplayName", smtpAccount.DisplayName);
            InsertOrUpdateSetting("Settings.SMTP.Email", smtpAccount.Email);
            InsertOrUpdateSetting("Settings.SMTP.EnableSsl", smtpAccount.EnableSsl.ToString());
            InsertOrUpdateSetting("Settings.SMTP.Host", smtpAccount.Host.ToString());
            InsertOrUpdateSetting("Settings.SMTP.Password", smtpAccount.Password);
            InsertOrUpdateSetting("Settings.SMTP.Port", smtpAccount.Port.ToString());
            InsertOrUpdateSetting("Settings.SMTP.Username", smtpAccount.Username);
            InsertOrUpdateSetting("Settings.SMTP.UseDefaultCredentials", smtpAccount.UseDefaultCredentials.ToString());
        }

        public virtual SmtpAccount GetSmtpAccount()
        {
            var smtpAccount = new SmtpAccount();

            smtpAccount.DisplayName = _settingService.GetSetting("Settings.SMTP.DisplayName")?.Value ?? string.Empty;
            smtpAccount.Email = _settingService.GetSetting("Settings.SMTP.Email")?.Value ?? string.Empty;

            var enableSsl = _settingService.GetSetting("Settings.SMTP.EnableSsl");
            smtpAccount.EnableSsl = enableSsl == null || Convert.ToBoolean((enableSsl.Value));

            smtpAccount.Host=  _settingService.GetSetting("Settings.SMTP.Host")?.Value?? string.Empty;
            
            smtpAccount.Password =  _settingService.GetSetting("Settings.SMTP.Password")?.Value?? string.Empty;
            var port = _settingService.GetSetting("Settings.SMTP.Port");
            smtpAccount.Port = port == null ? 0 : Convert.ToInt32(port.Value);

            smtpAccount.Username = _settingService.GetSetting("Settings.SMTP.Username")?.Value ?? string.Empty;

            var useDefaultCredentials = _settingService.GetSetting("Settings.SMTP.UseDefaultCredentials");
            smtpAccount.UseDefaultCredentials = useDefaultCredentials == null || Convert.ToBoolean((useDefaultCredentials.Value));

            return smtpAccount;
        }

        /// <summary>
        /// Delete Smtp Account
        /// </summary>
        /// <param name="smtpAccount">Smtp Account</param>
        /// <exception cref="ArgumentNullException">smtpAccount</exception>
        public virtual void DeleteSmtpAccount(SmtpAccount smtpAccount)
        {
            if (smtpAccount == null)
                throw new ArgumentNullException("smtpAccount");

            _smtpAccountRepository.Delete(smtpAccount);

            _cacheManager.RemoveByPattern(CACHE_SMTPACCOUNT_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(smtpAccount);
        }

        private void InsertOrUpdateSetting(string key, string value)
        {
            if (value != null)
            {
                var setting = _settingService.GetSetting(key);
                if (setting != null)
                {
                    setting.Value = value;
                    _settingService.UpdateSetting(setting);
                }
                else
                {
                    setting = new Setting
                    {
                        Key = key,
                        Value = value
                    };
                    _settingService.InsertSetting(setting);
                }
            }
        }

        #endregion Methods
    }
}