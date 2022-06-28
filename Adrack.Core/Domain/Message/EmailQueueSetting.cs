// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="EmailQueueSetting.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Configuration;

namespace Adrack.Core.Domain.Message
{
    /// <summary>
    /// Represents a Email Queue Setting
    /// Implements the <see cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    public class EmailQueueSetting : ISetting
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Maximum Delivery Attempts
        /// </summary>
        /// <value>The maximum delivery attempts.</value>
        public int MaximumDeliveryAttempts { get; set; }

        #endregion Properties
    }
}