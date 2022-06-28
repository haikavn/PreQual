// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerChannelTemplateMatching.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Class BuyerChannelTemplateMatching.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class BuyerChannelTemplateMatching : BaseEntity
    {
        #region Fields

        // private ICollection<User> _users;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or Sets the State province Identifier
        /// </summary>
        /// <value>The buyer channel template identifier.</value>
        public long BuyerChannelTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the input value.
        /// </summary>
        /// <value>The input value.</value>
        public string InputValue { get; set; }

        /// <summary>
        /// Gets or sets the output value.
        /// </summary>
        /// <value>The output value.</value>
        public string OutputValue { get; set; }

        #endregion Properties
    }
}