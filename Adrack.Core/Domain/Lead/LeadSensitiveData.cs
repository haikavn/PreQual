// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="LeadSensitiveData.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Represents a LeadContent
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class LeadSensitiveData : BaseEntity
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var leadContent = new LeadContent()
            {
            };

            return leadContent;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Lead Content
        /// </summary>
        /// <value>The lead identifier.</value>
        public long LeadId { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

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
        /// Gets or sets the data3.
        /// </summary>
        /// <value>The data3.</value>
        public string Data3 { get; set; }

        /// <summary>
        /// Gets or sets the data4.
        /// </summary>
        /// <value>The data4.</value>
        public string Data4 { get; set; }

        /// <summary>
        /// Gets or sets the data5.
        /// </summary>
        /// <value>The data5.</value>
        public string Data5 { get; set; }

        /// <summary>
        /// Gets or sets the data6.
        /// </summary>
        /// <value>The data6.</value>
        public string Data6 { get; set; }

        /// <summary>
        /// Gets or sets the data7.
        /// </summary>
        /// <value>The data7.</value>
        public string Data7 { get; set; }

        /// <summary>
        /// Gets or sets the data8.
        /// </summary>
        /// <value>The data8.</value>
        public string Data8 { get; set; }

        /// <summary>
        /// Gets or sets the data9.
        /// </summary>
        /// <value>The data9.</value>
        public string Data9 { get; set; }

        /// <summary>
        /// Gets or sets the data10.
        /// </summary>
        /// <value>The data10.</value>
        public string Data10 { get; set; }

        #endregion Properties
    }
}