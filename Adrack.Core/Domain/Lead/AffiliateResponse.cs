// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AffiliateResponse.cs" company="Adrack.com">
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
    public partial class AffiliateResponse : BaseEntity
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
        /// Gets or sets the lead identifier.
        /// </summary>
        /// <value>The lead identifier.</value>
        public long? LeadId { get; set; }

        /// <summary>
        /// Gets or sets the affiliate identifier.
        /// </summary>
        /// <value>The affiliate identifier.</value>
        public long AffiliateId { get; set; }

        /// <summary>
        /// Gets or sets the affiliate channel identifier.
        /// </summary>
        /// <value>The affiliate channel identifier.</value>
        public long AffiliateChannelId { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the response.
        /// </summary>
        /// <value>The response.</value>
        public string Response { get; set; }

        /// <summary>
        /// Gets or sets the minimum price.
        /// </summary>
        /// <value>The minimum price.</value>
        public decimal MinPrice { get; set; }

        /// <summary>
        /// Gets or sets the process started at.
        /// </summary>
        /// <value>The process started at.</value>
        public DateTime ProcessStartedAt { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public short? Status { get; set; }

        /// <summary>
        /// Gets or sets the type of the error.
        /// </summary>
        /// <value>The type of the error.</value>
        public short? ErrorType { get; set; }

        /// <summary>
        /// Gets or sets the validator.
        /// </summary>
        /// <value>The validator.</value>
        public short? Validator { get; set; }


        /// <summary>
        /// Gets or sets the received data.
        /// </summary>
        /// <value>The received data.</value>
        public string ReceivedData { get; set; }

        public string State { get; set; }

        #endregion Properties
    }
}