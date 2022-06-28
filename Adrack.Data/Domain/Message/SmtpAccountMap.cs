// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="SmtpAccountMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Message;

namespace Adrack.Data.Domain.Message
{
    /// <summary>
    /// Represents a Smtp Account Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Message.SmtpAccount}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Message.SmtpAccount}" />
    public partial class SmtpAccountMap : AppEntityTypeConfiguration<SmtpAccount>
    {
        #region Constructor

        /// <summary>
        /// Smtp Account Map
        /// </summary>
        public SmtpAccountMap()
        {
            this.ToTable("SmtpAccount");

            this.HasKey(x => x.Id);

            this.Property(x => x.Email).IsRequired().HasMaxLength(100);
            this.Property(x => x.DisplayName).IsRequired().HasMaxLength(50);
            this.Property(x => x.Host).IsRequired().HasMaxLength(50);
            this.Property(x => x.Port).IsRequired();
            this.Property(x => x.Username).IsRequired().HasMaxLength(50);
            this.Property(x => x.Password).IsRequired().HasMaxLength(50);

            this.Ignore(x => x.FriendlyName);
        }

        #endregion Constructor
    }
}