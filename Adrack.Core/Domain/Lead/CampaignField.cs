// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Hayk
// Last Modified On : 28-06-2022
// ***********************************************************************
// <copyright file="CampaignField.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Attributes;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Class CampaignField.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    [Tracked(EntityName = "Campaign", EntityIdField = "CampaignId")]

    public partial class CampaignField : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the campaign identifier.
        /// </summary>
        /// <value>The campaign identifier.</value>
        public long CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the template field.
        /// </summary>
        /// <value>The template field.</value>
        [Tracked]
        public string TemplateField { get; set; }

        /// <summary>
        /// Gets or sets the database field.
        /// </summary>
        /// <value>The database field.</value>
        [Tracked]
        public string DatabaseField { get; set; }

        /// <summary>
        /// Gets or sets the validator.
        /// </summary>
        /// <value>The validator.</value>
        public Validators Validator { get; set; }


        /// <summary>
        /// Gets or sets the validator value.
        /// </summary>
        /// <value>The validator value.</value>
        public string ValidatorSettings { get; set; }

        /// <summary>
        /// Gets or sets the name of the section.
        /// </summary>
        /// <value>The name of the section.</value>
        [Tracked]
        public string SectionName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CampaignField"/> is required.
        /// </summary>
        /// <value><c>true</c> if required; otherwise, <c>false</c>.</value>
        [Tracked]
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets the black list type identifier.
        /// </summary>
        /// <value>The black list type identifier.</value>
        public long? BlackListTypeId { get; set; }

        /// <summary>
        /// Gets or sets the white list type identifier.
        /// </summary>
        /// <value>The white list type identifier.</value>
        public long? WhiteListTypeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is hash.
        /// </summary>
        /// <value><c>null</c> if [is hash] contains no value, <c>true</c> if [is hash]; otherwise, <c>false</c>.</value>
        [Tracked]
        public bool IsHash { get; set; }

        #endregion Properties
    }
}