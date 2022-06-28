// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="LeadContent.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Represents a LeadContent
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class LeadContent : BaseEntity
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var leadContent = new LeadContent()
            {
            };

            return leadContent;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or Sets the Lead Content
        /// </summary>
        /// <value>The lead identifier.</value>
        public long LeadId { get; set; }

        /// <summary>
        /// Gets or sets the affiliate identifier.
        /// </summary>
        /// <value>The affiliate identifier.</value>
        public long AffiliateId { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        /// <value>The ip.</value>
        public string Ip { get; set; }

        /// <summary>
        /// Gets or sets the minprice.
        /// </summary>
        /// <value>The minprice.</value>
        public decimal? Minprice { get; set; }

        /// <summary>
        /// Gets or sets the firstname.
        /// </summary>
        /// <value>The firstname.</value>
        public string Firstname { get; set; }

        /// <summary>
        /// Gets or sets the lastname.
        /// </summary>
        /// <value>The lastname.</value>
        public string Lastname { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the zip.
        /// </summary>
        /// <value>The zip.</value>
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets the home phone.
        /// </summary>
        /// <value>The home phone.</value>
        public string HomePhone { get; set; }

        /// <summary>
        /// Gets or sets the cell phone.
        /// </summary>
        /// <value>The cell phone.</value>
        public string CellPhone { get; set; }

        /// <summary>
        /// Gets or sets the bank phone.
        /// </summary>
        /// <value>The bank phone.</value>
        public string BankPhone { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the pay frequency.
        /// </summary>
        /// <value>The pay frequency.</value>
        public string PayFrequency { get; set; }

        /// <summary>
        /// Gets or sets the directdeposit.
        /// </summary>
        /// <value>The directdeposit.</value>
        public string Directdeposit { get; set; }

        /// <summary>
        /// Gets or sets the type of the account.
        /// </summary>
        /// <value>The type of the account.</value>
        public string AccountType { get; set; }

        /// <summary>
        /// Gets or sets the type of the income.
        /// </summary>
        /// <value>The type of the income.</value>
        public string IncomeType { get; set; }

        /// <summary>
        /// Gets or sets the net monthly income.
        /// </summary>
        /// <value>The net monthly income.</value>
        public decimal? NetMonthlyIncome { get; set; }

        /// <summary>
        /// Gets or sets the emptime.
        /// </summary>
        /// <value>The emptime.</value>
        public short? Emptime { get; set; }

        /// <summary>
        /// Gets or sets the address month.
        /// </summary>
        /// <value>The address month.</value>
        public short? AddressMonth { get; set; }

        /// <summary>
        /// Gets or sets the dob.
        /// </summary>
        /// <value>The dob.</value>
        public DateTime? Dob { get; set; }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>The age.</value>
        public short? Age { get; set; }

        /// <summary>
        /// Gets or sets the requested amount.
        /// </summary>
        /// <value>The requested amount.</value>
        public decimal? RequestedAmount { get; set; }

        /// <summary>
        /// Gets or sets the SSN.
        /// </summary>
        /// <value>The SSN.</value>
        public string Ssn { get; set; }

        /// <summary>
        /// Gets or sets the string1.
        /// </summary>
        /// <value>The string1.</value>
        public string String1 { get; set; }

        /// <summary>
        /// Gets or sets the string2.
        /// </summary>
        /// <value>The string2.</value>
        public string String2 { get; set; }

        /// <summary>
        /// Gets or sets the string3.
        /// </summary>
        /// <value>The string3.</value>
        public string String3 { get; set; }

        /// <summary>
        /// Gets or sets the string4.
        /// </summary>
        /// <value>The string4.</value>
        public string String4 { get; set; }

        /// <summary>
        /// Gets or sets the string5.
        /// </summary>
        /// <value>The string5.</value>
        public string String5 { get; set; }

        /// <summary>
        /// Gets or sets the type of the campaign.
        /// </summary>
        /// <value>The type of the campaign.</value>
        public CampaignTypes? CampaignType { get; set; }

        /// <summary>
        /// Gets or sets the minprice string.
        /// </summary>
        /// <value>The minprice string.</value>
        public string MinpriceStr { get; set; }

        /// <summary>
        /// Gets or sets the affiliate sub identifier.
        /// </summary>
        /// <value>The affiliate sub identifier.</value>
        public string AffiliateSubId { get; set; }

        /// <summary>
        /// Gets or sets the affiliate sub id2.
        /// </summary>
        /// <value>The affiliate sub id2.</value>
        public string AffiliateSubId2 { get; set; }

        #endregion Properties
    }
}