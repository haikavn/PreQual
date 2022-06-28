// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="EmailQueueMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Message;

namespace Adrack.Data.Domain.Message
{
    /// <summary>
    /// Represents a Email Queue Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Message.EmailQueue}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Message.EmailQueue}" />
    public partial class EmailQueueMap : AppEntityTypeConfiguration<EmailQueue>
    {
        #region Constructor

        /// <summary>
        /// Email Queue Map
        /// </summary>
        public EmailQueueMap()
        {
            this.ToTable("EmailQueue");

            this.HasKey(x => x.Id);

            this.Property(x => x.Sender).IsRequired().HasMaxLength(200);
            this.Property(x => x.SenderName).HasMaxLength(100);
            this.Property(x => x.Recipient).IsRequired().HasMaxLength(200);
            this.Property(x => x.RecipientName).HasMaxLength(100);
            this.Property(x => x.ReplyTo).HasMaxLength(200);
            this.Property(x => x.ReplyToName).HasMaxLength(100);
            this.Property(x => x.Cc).HasMaxLength(200);
            this.Property(x => x.Bcc).HasMaxLength(200);
            this.Property(x => x.Subject).HasMaxLength(1000);
            //this.Property(x => x.Body).HasMaxLength(4000);
            this.Property(x => x.Priority).IsRequired();

            //this.HasRequired(x => x.SmtpAccount)
            //    .WithMany()
            //    .HasForeignKey(x => x.SmtpAccountId)
            //    .WillCascadeOnDelete(true);
        }

        #endregion Constructor
    }
}