// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Campaign.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Xml.Serialization;
using Adrack.Core.Attributes;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Class Campaign.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    [Tracked]
    public partial class Campaign : BaseEntity
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var campaign = new Campaign()
            {
            };

            return campaign;
        }

        #endregion Methods

        #region Fields

        /// <summary>
        /// Campaign Fields
        /// </summary>
        private ICollection<CampaignField> _campaignFields;

        #endregion Fields

        #region Properties


        /// <summary>
        /// Gets or Sets the First name
        /// </summary>
        /// <value>The name.</value>
        //[Tracked]
        [Tracked]
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [Tracked]
        public ActivityStatuses Status { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        //[Tracked(DisplayName = "Full Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the XML template.
        /// </summary>
        /// <value>The XML template.</value>
        public string DataTemplate { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Campaign"/> is deleted.
        /// </summary>
        /// <value><c>null</c> if [deleted] contains no value, <c>true</c> if [deleted]; otherwise, <c>false</c>.</value>
        public bool IsDeleted { get; set; }

        #endregion Properties

    }


}