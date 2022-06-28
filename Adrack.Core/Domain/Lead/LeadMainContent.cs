// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="LeadMainContent.cs" company="Adrack.com">
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
    public partial class LeadMainContent : BaseEntity
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var lead = new LeadMainContent()
            {
            };

            return lead;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime? Created { get; set; }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        /// <value>The update date.</value>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the campaign identifier.
        /// </summary>
        /// <value>The campaign identifier.</value>
        public long CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the affiliate identifier.
        /// </summary>
        /// <value>The affiliate identifier.</value>
        public long AffiliateId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public short Status { get; set; }

        /// <summary>
        /// Gets or sets the affiliate channel identifier.
        /// </summary>
        /// <value>The affiliate channel identifier.</value>
        public long AffiliateChannelId { get; set; }

        /// <summary>
        /// Gets or sets the type of the campaign.
        /// </summary>
        /// <value>The type of the campaign.</value>
        public short CampaignType { get; set; }

        /// <summary>
        /// Gets or sets the lead number.
        /// </summary>
        /// <value>The lead number.</value>
        public long LeadNumber { get; set; }

        /// <summary>
        /// Gets or sets the warning.
        /// </summary>
        /// <value>The warning.</value>
        public short Warning { get; set; }

        /// <summary>
        /// Gets or sets the affiliate sub identifier.
        /// </summary>
        /// <value>The affiliate sub identifier.</value>
        public string AffiliateSubId { get; set; }

        /// <summary>
        /// Gets or sets the processing time.
        /// </summary>
        /// <value>The processing time.</value>
        public double? ProcessingTime { get; set; }

        /// <summary>
        /// Gets or sets the dublicate lead identifier.
        /// </summary>
        /// <value>The dublicate lead identifier.</value>
        public long DublicateLeadId { get; set; }

        /// <summary>
        /// Gets or sets the received data.
        /// </summary>
        /// <value>The received data.</value>
        public string ReceivedData { get; set; }

        /// <summary>
        /// Gets or sets the type of the error.
        /// </summary>
        /// <value>The type of the error.</value>
        public short? ErrorType { get; set; }

        // Lead Content fields
        /// <summary>
        /// Gets or sets the lead identifier.
        /// </summary>
        /// <value>The lead identifier.</value>
        public long LeadId { get; set; }

        /*
                public long AffiliateId { get; set; }
                public DateTime Created { get; set; }
        */

        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        /// <value>The ip.</value>
        public string Ip { get; set; }

        /// <summary>
        /// Gets or sets the
        /// </summary>
        public string Url { get; set; }

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
        /// Gets or sets the minprice string.
        /// </summary>
        /// <value>The minprice string.</value>
        public string MinpriceStr { get; set; }

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
        /// Gets or sets the buyer identifier.
        /// </summary>
        /// <value>The buyer identifier.</value>
        public long? BuyerId { get; set; }

        /// <summary>
        /// Gets or sets the buyer channel identifier.
        /// </summary>
        /// <value>The buyer channel identifier.</value>
        public long? BuyerChannelId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LeadMainContent"/> is clicked.
        /// </summary>
        /// <value><c>null</c> if [clicked] contains no value, <c>true</c> if [clicked]; otherwise, <c>false</c>.</value>
        public bool? Clicked { get; set; }

        /// <summary>
        /// Gets or sets the click ip.
        /// </summary>
        /// <value>The click ip.</value>
        public string ClickIp { get; set; }

        /// <summary>
        /// Gets or sets the affiliate price.
        /// </summary>
        /// <value>The affiliate price.</value>
        public decimal? AffiliatePrice { get; set; }

        /// <summary>
        /// Gets or sets the buyer price.
        /// </summary>
        /// <value>The buyer price.</value>
        public decimal? BuyerPrice { get; set; }

        /// <summary>
        /// Gets or sets the risk score.
        /// </summary>
        /// <value>The risk score.</value>
        public int? RiskScore { get; set; }

        #endregion Properties
    }
}