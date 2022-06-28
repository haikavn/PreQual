// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="UserSubscription.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Class UserSubscribtion.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class UserSubscribtion : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the First name
        /// </summary>
        /// <value>The user identifier.</value>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the subscriber identifier.
        /// </summary>
        /// <value>The subscriber identifier.</value>
        public long SubscriberId { get; set; }

        #endregion Properties
    }
}