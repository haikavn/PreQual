// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ProfileSetting.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Configuration;

namespace Adrack.Core.Domain.Membership
{
    /// <summary>
    /// Represents a Profile Setting
    /// Implements the <see cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    public class ProfileSetting : ISetting
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the First Name Enabled
        /// </summary>
        /// <value><c>true</c> if [first name enabled]; otherwise, <c>false</c>.</value>
        public bool FirstNameEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the First Name Required
        /// </summary>
        /// <value><c>true</c> if [first name required]; otherwise, <c>false</c>.</value>
        public bool FirstNameRequired { get; set; }

        /// <summary>
        /// Gets or Sets the Middle Name Enabled
        /// </summary>
        /// <value><c>true</c> if [middle name enabled]; otherwise, <c>false</c>.</value>
        public bool MiddleNameEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Middle Name Required
        /// </summary>
        /// <value><c>true</c> if [middle name required]; otherwise, <c>false</c>.</value>
        public bool MiddleNameRequired { get; set; }

        /// <summary>
        /// Gets or Sets the Last Name Enabled
        /// </summary>
        /// <value><c>true</c> if [last name enabled]; otherwise, <c>false</c>.</value>
        public bool LastNameEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Last Name Required
        /// </summary>
        /// <value><c>true</c> if [last name required]; otherwise, <c>false</c>.</value>
        public bool LastNameRequired { get; set; }

        /// <summary>
        /// Gets or Sets the Alternate Name Enabled
        /// </summary>
        /// <value><c>true</c> if [alternate name enabled]; otherwise, <c>false</c>.</value>
        public bool AlternateNameEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Alternate Name Required
        /// </summary>
        /// <value><c>true</c> if [alternate name required]; otherwise, <c>false</c>.</value>
        public bool AlternateNameRequired { get; set; }

        /// <summary>
        /// Gets or Sets the Gender Enabled
        /// </summary>
        /// <value><c>true</c> if [gender enabled]; otherwise, <c>false</c>.</value>
        public bool GenderEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Gender Required
        /// </summary>
        /// <value><c>true</c> if [gender required]; otherwise, <c>false</c>.</value>
        public bool GenderRequired { get; set; }

        /// <summary>
        /// Gets or Sets the Date Of Birth Enabled
        /// </summary>
        /// <value><c>true</c> if [date of birth enabled]; otherwise, <c>false</c>.</value>
        public bool DateOfBirthEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Date Of Birth Required
        /// </summary>
        /// <value><c>true</c> if [date of birth required]; otherwise, <c>false</c>.</value>
        public bool DateOfBirthRequired { get; set; }

        /// <summary>
        /// Gets or Sets the Summary Enabled
        /// </summary>
        /// <value><c>true</c> if [summary enabled]; otherwise, <c>false</c>.</value>
        public bool SummaryEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Summary Required
        /// </summary>
        /// <value><c>true</c> if [summary required]; otherwise, <c>false</c>.</value>
        public bool SummaryRequired { get; set; }

        #endregion Properties
    }
}