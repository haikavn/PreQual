// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Language.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Seo;
using System.Collections.Generic;

namespace Adrack.Core.Domain.Localization
{
    /// <summary>
    /// Represents a Language
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class Language : BaseEntity
    {
        #region Fields

        /// <summary>
        /// Localized String
        /// </summary>
        private ICollection<LocalizedString> _localizedStrings;

        /// <summary>
        /// Localized Property
        /// </summary>
        private ICollection<LocalizedProperty> _localizedProperties;

        /// <summary>
        /// Page Slug
        /// </summary>
        private ICollection<PageSlug> _pageSlugs;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or Sets the Name
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the Culture
        /// </summary>
        /// <value>The culture.</value>
        public string Culture { get; set; }

        /// <summary>
        /// Gets or Sets the Culture Identifier
        /// </summary>
        /// <value>The culture identifier.</value>
        public int CultureId { get; set; }

        /// <summary>
        /// Gets or Sets the Published
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        public bool Published { get; set; }

        /// <summary>
        /// Get or Sets the Rtl (Right To Left)
        /// </summary>
        /// <value><c>true</c> if RTL; otherwise, <c>false</c>.</value>
        public bool Rtl { get; set; }

        /// <summary>
        /// Gets or Sets the Display Order
        /// </summary>
        /// <value>The display order.</value>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or Sets the Description
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        #endregion Properties

        #region Navigation Properties

        /// <summary>
        /// Gets or Sets Localized String
        /// </summary>
        /// <value>The localized strings.</value>
        public virtual ICollection<LocalizedString> LocalizedStrings
        {
            get { return _localizedStrings ?? (_localizedStrings = new List<LocalizedString>()); }
            protected set { _localizedStrings = value; }
        }

        /// <summary>
        /// Gets or Sets Localized Property
        /// </summary>
        /// <value>The localized properties.</value>
        public virtual ICollection<LocalizedProperty> LocalizedProperties
        {
            get { return _localizedProperties ?? (_localizedProperties = new List<LocalizedProperty>()); }
            protected set { _localizedProperties = value; }
        }

        /// <summary>
        /// Gets or Sets the Page Slug
        /// </summary>
        /// <value>The page slugs.</value>
        public virtual ICollection<PageSlug> PageSlugs
        {
            get { return _pageSlugs ?? (_pageSlugs = new List<PageSlug>()); }
            protected set { _pageSlugs = value; }
        }

        #endregion Navigation Properties
    }
}