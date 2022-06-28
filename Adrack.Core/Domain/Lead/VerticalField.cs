// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : 
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="VerticalField.cs" company="Adrack.com">
//     Copyright © 2020
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Class Vertical.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class VerticalField : BaseEntity
    {
        #region Properties
        /// <summary>
        /// Gets or Sets Vertical Id
        /// </summary>
        public long VerticalId { get; set; }
        /// <summary>
        /// Gets or Sets Vertical Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or Sets Vertical DataType
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or Sets Vertical Required
        /// </summary>
        public bool IsRequired { get; set; }
        #endregion Properties
    }
}
