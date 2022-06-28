// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerChannelFilterCondition.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Class BuyerChannelFilterCondition.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class BuyerChannelFilterCondition : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the buyer channel identifier.
        /// </summary>
        /// <value>The buyer channel identifier.</value>
        public long BuyerChannelId { get; set; }

        /// <summary>
        /// Gets or Sets the First name
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the value2.
        /// </summary>
        /// <value>The value2.</value>
        public string Value2 { get; set; }

        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        /// <value>The condition.</value>
        public short Condition { get; set; }

        /// <summary>
        /// Gets or sets the condition operator.
        /// </summary>
        /// <value>The condition operator.</value>
        public short ConditionOperator { get; set; }

        /// <summary>
        /// Gets or sets the campaign template identifier.
        /// </summary>
        /// <value>The campaign template identifier.</value>
        public long CampaignTemplateId { get; set; }


        /// <summary>
        /// Gets or sets the parentid.
        /// </summary>
        /// <value>The parent id.</value>
        public long? ParentId { get; set; }

        #endregion Properties
    }
}