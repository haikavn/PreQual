// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="EmailSubscriptionMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Message;

namespace Adrack.Data.Domain.Message
{
    /// <summary>
    /// Represents a Email Subscription Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Message.EmailSubscription}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Message.EmailSubscription}" />
    public partial class EmailSubscriptionMap : AppEntityTypeConfiguration<EmailSubscription>
    {
        #region Constructor

        /// <summary>
        /// Email Subscription Map
        /// </summary>
        public EmailSubscriptionMap()
        {
            this.ToTable("EmailSubscription");

            this.HasKey(x => x.Id);

            this.Property(x => x.Email).IsRequired().HasMaxLength(100);
        }

        #endregion Constructor
    }
}