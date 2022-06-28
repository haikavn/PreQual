// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ISmtpAccountService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Message;
using System.Collections.Generic;

namespace Adrack.Service.Message
{
    /// <summary>
    /// Represents a Simple Mail Transfer Protocol Account Service
    /// </summary>
    public partial interface ISmtpAccountService
    {
        #region Methods

        /// <summary>
        /// Get Smtp Account By Id
        /// </summary>
        /// <param name="smtpAccountId">Smtp Account Identifier</param>
        /// <returns>Smtp Account Item</returns>
        SmtpAccount GetSmtpAccountById(long smtpAccountId);

        SmtpAccount GetSmtpAccount();

        /// <summary>
        /// Get All Smtp Accounts
        /// </summary>
        /// <returns>Smtp Account Collection Item</returns>
        IList<SmtpAccount> GetAllSmtpAccounts();

        /// <summary>
        /// Insert Smtp Account
        /// </summary>
        /// <param name="smtpAccount">Smtp Account</param>
        void InsertSmtpAccount(SmtpAccount smtpAccount);

        /// <summary>
        /// Update Smtp Account
        /// </summary>
        /// <param name="smtpAccount">Smtp Account</param>
        void UpdateSmtpAccount(SmtpAccount smtpAccount);

        /// <summary>
        /// Delete Smtp Account
        /// </summary>
        /// <param name="smtpAccount">Smtp Account</param>
        void DeleteSmtpAccount(SmtpAccount smtpAccount);

        #endregion Methods
    }
}