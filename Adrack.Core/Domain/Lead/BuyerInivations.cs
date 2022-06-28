// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Affiliate.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Directory;
using System;

namespace Adrack.Core.Domain.Lead
{
    public partial class BuyerInvitation : BaseEntity
    {
        #region Fields

        // private ICollection<User> _users;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var affiliate = new Affiliate()
            {
            };

            return affiliate;
        }

        #endregion Methods

        #region Properties
        public long BuyerId { get; set; }
        public string RecipientEmail { get; set; }
        public DateTime InvitationDate { get; set; }
        public BuyerInvitationStatuses Status { get; set; }
        public int Role { get; set; }

        #endregion Properties

        #region Navigation Properties
        public virtual Buyer Buyer { get; set; }
        #endregion Navigation Properties
    }
}