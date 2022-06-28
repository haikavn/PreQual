// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 05-05-2020
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 05-05-2020
// ***********************************************************************
// <copyright file="Navigation.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Common
{
    /// <summary>
    /// Represents a Navigation
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class AbaNumber : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the abanumber
        /// </summary>
        /// <value>The abanumber identifier.</value>
        public long abanumber { get; set; }

        /// <summary>
        /// Gets or Sets the bankname
        /// </summary>
        /// <value>The bankname identifier.</value>
        public string bankname { get; set; }

        /// <summary>
        /// Gets or Sets the state
        /// </summary>
        /// <value>The state identifier.</value>
        public string state { get; set; }

        /// <summary>
        /// Gets or Sets the city
        /// </summary>
        /// <value>The city identifier.</value>
        public string city { get; set; }

        #endregion Properties
    }
}