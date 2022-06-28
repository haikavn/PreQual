// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="LogMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Audit;

namespace Adrack.Data.Domain.Audit
{
    /// <summary>
    /// Represents a Log Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Audit.Log}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Audit.Log}" />
    public partial class LogMap : AppEntityTypeConfiguration<Log>
    {
        #region Constructor

        /// <summary>
        /// Log Map
        /// </summary>
        public LogMap()
        {
            this.ToTable("Log");

            this.HasKey(x => x.Id);

            this.Property(x => x.Message).IsRequired().HasMaxLength(1000);
            this.Property(x => x.IpAddress).HasMaxLength(15);

            this.Ignore(x => x.LogLevel);

            this.HasOptional(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(true);
        }

        #endregion Constructor
    }
}