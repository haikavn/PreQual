// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="LeadNote.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Represents a Lead
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class Coupon : BaseEntity
    {
        #region Properties
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string CouponExpression { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime ValidToUtc { get; set; }
        public DateTime? AppliedByUserUtc { get; set; }//When did usr apply the coupon.
        public DateTime? UsedBySystemUtc { get; set; }//When did system userd applied coupon 

        
        public decimal DiscountAmount { get; set; }//If the coupon generated as a discount by percentage 


        public bool IsPercent { get; set; }
        #endregion Properties
    }
}