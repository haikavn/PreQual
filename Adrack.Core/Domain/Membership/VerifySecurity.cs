// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="VerifySecurity.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Core.Domain.Membership
{
    /// <summary>
    /// Represents a Verify Security
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class VerifySecurity : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the User Identifier
        /// </summary>
        /// <value>The user identifier.</value>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or Sets the Question
        /// </summary>
        /// <value>The question.</value>
        public string Question { get; set; }

        /// <summary>
        /// Gets or Sets the Answer
        /// </summary>
        /// <value>The answer.</value>
        public string Answer { get; set; }

        /// <summary>
        /// Gets or Sets the Created On
        /// </summary>
        /// <value>The created on.</value>
        public DateTime CreatedOn { get; set; }

        #endregion Properties
    }
}