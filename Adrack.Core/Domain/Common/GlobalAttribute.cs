// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="GlobalAttribute.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Common
{
    /// <summary>
    /// Represents a Global Attribute
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class GlobalAttribute : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Entity Identifier
        /// </summary>
        /// <value>The entity identifier.</value>
        public long EntityId { get; set; }

        /// <summary>
        /// Gets or Sets the  Key Group
        /// </summary>
        /// <value>The key group.</value>
        public string KeyGroup { get; set; }

        /// <summary>
        /// Gets or Sets the Key
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or Sets the Value
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }


        /// <summary>
        /// Gets or Sets the ExtraData
        /// </summary>
        /// <value>The value.</value>
        public string ExtraData { get; set; }


        #endregion Properties
    }
}