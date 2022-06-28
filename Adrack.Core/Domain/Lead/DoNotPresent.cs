// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="DoNotPresent.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Class Filter.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class DoNotPresent : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the email
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the ssn.
        /// </summary>
        /// <value>The ssn.</value>
        public string Ssn { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>The phone.</value>
        public string Phone { get; set; }


        /// <summary>
        /// Gets or sets the expirtation date.
        /// </summary>
        /// <value>The expiration date.</value>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the buyer id.
        /// </summary>
        /// <value>The expiration date.</value>
        public long? BuyerId { get; set; }

        #endregion Properties
    }
}