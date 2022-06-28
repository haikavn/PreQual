// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AffiliateChannel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Attributes;
using System.Collections.Generic;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Class AffiliateChannel.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    [Tracked]
    public partial class AffiliateChannel : BaseEntity
    {
        #region Fields

        // private ICollection<User> _users;
        /// <summary>
        /// Affiliate Channel Templates
        /// </summary>
        private ICollection<AffiliateChannelTemplate> _affiliateChannelFields;

        /// <summary>
        /// Attached Buyer Channels
        /// </summary>
        private ICollection<BuyerChannel> _attachedBuyerChannels;

        private ICollection<AffiliateChannelFilterCondition> _affiliateChannelFilterConditions;


        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or Sets the State province Identifier
        /// </summary>
        /// <value>The campaign identifier.</value>
        [Tracked(DisplayName = "Campaign", TableName = "Campaign")]

        public long? CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the affiliate identifier.
        /// </summary>
        /// <value>The affiliate identifier.</value>
        //[Tracked(DisplayName = "Affiliate", TableName = "Affiliate")]

        public long AffiliateId { get; set; }

        /// <summary>
        /// Gets or Sets the First name
        /// </summary>
        /// <value>The name.</value>
        [Tracked]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [Tracked(DisplayType = typeof(ActivityStatuses))]
        public ActivityStatuses Status { get; set; }

        /// <summary> 
        /// Gets or sets the affiliate channel key.
        /// </summary>
        /// <value>The affiliate channel key.</value>
        [Tracked]
        public string ChannelKey { get; set; }

        public string ChannelPassword { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AffiliateChannel"/> is deleted.
        /// </summary>
        /// <value><c>null</c> if [deleted] contains no value, <c>true</c> if [deleted]; otherwise, <c>false</c>.</value>
        public bool IsDeleted { get; set; }

        #endregion Properties

    }
}