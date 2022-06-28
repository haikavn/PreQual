// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="EmailSubscriptionSetting.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Configuration;

namespace Adrack.Core.Domain.Message
{
    /// <summary>
    /// Represents a Email Subscription Setting
    /// Implements the <see cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    public class EmailSubscriptionSetting : ISetting
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Email Subscription Enabled
        /// </summary>
        /// <value><c>true</c> if [email subscription enabled]; otherwise, <c>false</c>.</value>
        public bool EmailSubscriptionEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Email Subscription Checked
        /// </summary>
        /// <value><c>true</c> if [email subscription checked]; otherwise, <c>false</c>.</value>
        public bool EmailSubscriptionChecked { get; set; }

        #endregion Properties
    }
}