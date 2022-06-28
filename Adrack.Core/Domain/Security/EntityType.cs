using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.Security
{
    /// <summary>
    /// Entity type
    /// </summary>
    public enum EntityType : byte
    {
        /// <summary>
        /// Campaign Entity
        /// </summary>
        Campaign = 0,
        /// <summary>
        /// Buyer Entity
        /// </summary>
        Buyer = 1,
        /// <summary>
        /// Buyer Channel Entity
        /// </summary>
        BuyerChannel = 2,
        /// <summary>
        /// Affiliate Entity
        /// </summary>
        Affiliate = 3,
        /// <summary>
        /// Affiliate Channel Entity
        /// </summary>
        AffiliateChannel = 4,
        /// <summary>
        /// Affiliate Invitation Entity
        /// </summary>
        AffiliateInvitation = 5,
        /// <summary>
        ///  Buyer Invitation Entity
        /// </summary>
        BuyerInvitation = 6,
        /// <summary>
        /// Lead Entity
        /// </summary>
        Lead = 7
    }
}
