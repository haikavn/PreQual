// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ScheduleTaskMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Agent;

namespace Adrack.Data.Domain.Notification
{
    /// <summary>
    /// Represents a Schedule Task Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Notification.UserNotification}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Notification.UserNotification}" />
    public partial class UserNotificationMap : AppEntityTypeConfiguration<Adrack.Core.Domain.Notification.UserNotification>
    {
        #region Constructor

        /// <summary>
        /// Schedule Task Map
        /// </summary>
        public UserNotificationMap()
        {
            this.ToTable("UserNotification");
            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}