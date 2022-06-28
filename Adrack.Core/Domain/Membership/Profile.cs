// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Profile.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;

namespace Adrack.Core.Domain.Membership
{
    /// <summary>
    /// Represents a Profile
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class Profile : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the User Identifier
        /// </summary>
        /// <value>The user identifier.</value>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or Sets the First Name
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or Sets the Middle Name
        /// </summary>
        /// <value>The name of the middle.</value>
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or Sets the Last Name
        /// </summary>
        /// <value>The last name.</value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or Sets the Phone
        /// </summary>
        /// <value>The phone.</value>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or Sets the Cell Phone
        /// </summary>
        /// <value>The cell phone.</value>
        public string CellPhone { get; set; }

        /// <summary>
        /// Gets or Sets the Summary
        /// </summary>
        /// <value>The summary.</value>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the JobTitle value.
        /// </summary>
        /// <value>The JobTitle value.</value>
        [MaxLength(50)]
        public string JobTitle { get; set; }

        public string CompanyName { get; set; }

        public string CompanyWebSite { get; set; }

        public long? VerticalId { get; set; }

        #endregion Properties

        #region Navigation Properties

        /// <summary>
        /// Gets or Sets the User
        /// </summary>
        /// <value>The user.</value>
        public virtual User User { get; set; }

        #endregion Navigation Properties
    }
}