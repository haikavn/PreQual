// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="LeadFieldsContent.cs" company="Adrack.com">
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
    public partial class LeadFieldsContent : BaseEntity
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
        /// Gets or sets the string value.
        /// </summary>
        /// <value>The string value.</value>
        public string StringValue { get; set; }

        /// <summary>
        /// Gets or sets the int value.
        /// </summary>
        /// <value>The int value.</value>
        public int? IntValue { get; set; }

        /// <summary>
        /// Gets or sets the decimal value.
        /// </summary>
        /// <value>The decimal value.</value>
        public decimal? DecimalValue { get; set; }

        /// <summary>
        /// Gets or sets the date time value.
        /// </summary>
        /// <value>The date time value.</value>
        public DateTime? DateTimeValue { get; set; }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        public string FieldName { get; set; }

        #endregion Properties
    }
}