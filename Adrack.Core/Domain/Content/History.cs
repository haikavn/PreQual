// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="History.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Core.Domain.Content
{
    /// <summary>
    /// Class History.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// Implements the <see cref="System.ICloneable" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    /// <seealso cref="System.ICloneable" />
    public partial class History : BaseEntity, ICloneable
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var aHistory = new History()
            {
            };

            return aHistory;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the module.
        /// </summary>
        /// <value>The module.</value>
        public string Module { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public int Action { get; set; }

        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        /// <value>The entity.</value>
        public string Entity { get; set; }

        /// <summary>
        /// Gets or sets the entity identifier.
        /// </summary>
        /// <value>The entity identifier.</value>
        public long EntityID { get; set; }

        /// <summary>
        /// Gets or sets the data1.
        /// </summary>
        /// <value>The data1.</value>
        public string Data1 { get; set; }

        /// <summary>
        /// Gets or sets the data2.
        /// </summary>
        /// <value>The data2.</value>
        public string Data2 { get; set; }

        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        /// <value>The note.</value>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public long UserID { get; set; }

        #endregion Properties
    }
}