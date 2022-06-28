// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="RedirectUrl.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Represents a Affiliate
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class RedirectUrl : BaseEntity
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

        /// <summary>
        /// Gets or Sets the State province Identifier
        /// </summary>
        /// <value>The lead identifier.</value>
        public long LeadId { get; set; }

        /// <summary>
        /// Gets or Sets the First name
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RedirectUrl"/> is clicked.
        /// </summary>
        /// <value><c>true</c> if clicked; otherwise, <c>false</c>.</value>
        public bool Clicked { get; set; }

        /// <summary>
        /// Gets or sets the click date.
        /// </summary>
        /// <value>The click date.</value>
        public DateTime ClickDate { get; set; }

        /// <summary>
        /// Gets or sets the navigation key.
        /// </summary>
        /// <value>The navigation key.</value>
        public string NavigationKey { get; set; }

        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        /// <value>The ip.</value>
        public string Ip { get; set; }

        /// <summary>
        /// Gets or sets the device.
        /// </summary>
        /// <value>The device.</value>
        public string Device { get; set; }

        /// <summary>
        /// Gets or sets the browser.
        /// </summary>
        /// <value>The browser.</value>
        public string Browser { get; set; }

        /// <summary>
        /// Gets or sets the os.
        /// </summary>
        /// <value>The os.</value>
        public string OS { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>The zip code.</value>
        public string ZipCode { get; set; }


        public long? CampaignId { get; set; }

        public long? AffiliateId { get; set; }

        public long? AffiliateChannelId { get; set; }

        public long? BuyerId { get; set; }

        public long? BuyerChannelId { get; set; }

        public string State { get; set; }

        #endregion Properties
    }
}