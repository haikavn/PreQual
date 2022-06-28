// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AffiliateChannelFilterCondition.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Class AffiliateChannelFilterCondition.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class AffiliateChannelFilterCondition : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the affiliate channel identifier.
        /// </summary>
        /// <value>The affiliate channel identifier.</value>
        public long AffiliateChannelId { get; set; }

        /// <summary>
        /// Gets or Sets the First name
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /*public List<string> Values
        {
            get
            {
                var list = new List<string>();
                if (string.IsNullOrEmpty(Value)) return list;

                var a = Value.Split(new string[1] { "," }, System.StringSplitOptions.None);
                list.AddRange()
            }
        }*/

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