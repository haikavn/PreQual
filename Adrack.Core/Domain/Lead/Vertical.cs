// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Vertical.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Class Vertical.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class Vertical : BaseEntity
    {
        #region Fields

        /// <summary>
        /// Vertical Fields
        /// </summary>
        private ICollection<VerticalField> _verticalFields;

        /// <summary>
        /// Campaigns
        /// </summary>
        private ICollection<Campaign> _campaigns;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or Sets the First name
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the Group name
        /// </summary>
        /// <value>The group.</value>
        public string Group { get; set; }
        /// <summary>
        /// Gets or Sets the Icon name
        /// </summary>
        /// <value>The icona name.</value>
        public string IconName { get; set; }

        #endregion Properties

        #region Navigation Properties

        /// <summary>
        /// Gets or Sets the Vertical Fields
        /// </summary>
        /// <value>The Vertical Fields.</value>
        public virtual ICollection<VerticalField> VerticalFields
        {
            get
            {
                if (_verticalFields != null)
                    return _verticalFields;
                _verticalFields = new List<VerticalField>();
                return _verticalFields;
            }
            set { _verticalFields = value; }
        }

        /// <summary>
        /// Gets or Sets campaigns
        /// </summary>
        /// <value>The Vertical Fields.</value>
        /*public virtual ICollection<Campaign> Campaigns
        {
            get
            {
                if (_campaigns != null)
                    return _campaigns;
                _campaigns = new List<Campaign>();
                return _campaigns;
            }
            set { _campaigns = value; }
        }*/

        #endregion Navigation Properties
    }
}