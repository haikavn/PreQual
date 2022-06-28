// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Filter.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Class Filter.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class Filter : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the First name
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the campaign identifier.
        /// </summary>
        /// <value>The campaign identifier.</value>
        public long CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the vertical identifier.
        /// </summary>
        /// <value>The vertical identifier.</value>
        public long? VerticalId { get; set; }

        #endregion Properties

        #region Fields

        #endregion

        #region Navigation properties

        #endregion
    }
}