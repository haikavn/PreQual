// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="EmailTemplateMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Message;

namespace Adrack.Data.Domain.Message
{
    /// <summary>
    /// Represents a Email Template Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Message.EmailTemplate}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Message.EmailTemplate}" />
    public partial class EmailTemplateMap : AppEntityTypeConfiguration<EmailTemplate>
    {
        #region Constructor

        /// <summary>
        /// Email Template Map
        /// </summary>
        public EmailTemplateMap()
        {
            this.ToTable("EmailTemplate");

            this.HasKey(x => x.Id);

            this.Property(x => x.SmtpAccountId).IsRequired();
            this.Property(x => x.Name).IsRequired().HasMaxLength(200);
            this.Property(x => x.Bcc).HasMaxLength(200);
            this.Property(x => x.Subject).HasMaxLength(1000);
            //this.Property(x => x.Body).HasMaxLength(4000);
        }

        #endregion Constructor
    }
}