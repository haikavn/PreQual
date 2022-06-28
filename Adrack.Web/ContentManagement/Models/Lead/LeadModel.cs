// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="LeadModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Models.Lead
{
    /// <summary>
    /// Class LeadModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class LeadModel : BaseAppModel
    {
        #region Constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public LeadModel()
        {
            this.ListStates = new List<SelectListItem>();
        }

        #endregion Constructor



        #region Properties

        // Lead Main Properties
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the created UTC.
        /// </summary>
        /// <value>The created UTC.</value>
        public DateTime CreatedUtc { get; set; }

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
        /// Gets or sets the buyer identifier string.
        /// </summary>
        /// <value>The buyer identifier string.</value>
        public string BuyerIdStr { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public short Status { get; set; }

        /// <summary>
        /// Gets or sets a channel identifier.
        /// </summary>
        /// <value>a channel identifier.</value>
        public long AChannelId { get; set; }

        /// <summary>
        /// Gets or sets the b channel ids.
        /// </summary>
        /// <value>The b channel ids.</value>
        public string BChannelIds { get; set; }

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
        /// Gets or sets the t time.
        /// </summary>
        /// <value>The t time.</value>
        public double TTime { get; set; }

        /// <summary>
        /// Gets or sets the r time string.
        /// </summary>
        /// <value>The r time string.</value>
        public string RTimeStr { get; set; }

        /// <summary>
        /// Gets or sets the aff sub identifier.
        /// </summary>
        /// <value>The aff sub identifier.</value>
        public string AffSubId { get; set; }

        /// <summary>
        /// Gets or sets the minpricestr.
        /// </summary>
        /// <value>The minpricestr.</value>
        public string Minpricestr { get; set; }

        // Lead Content Properties

        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        /// <value>The ip.</value>
        public string Ip { get; set; }

        /// <summary>
        /// Gets or sets the minprice.
        /// </summary>
        /// <value>The minprice.</value>
        public decimal Minprice { get; set; }

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
        public decimal IncomeType { get; set; }

        /// <summary>
        /// Gets or sets the net monthly income.
        /// </summary>
        /// <value>The net monthly income.</value>
        public decimal NetMonthlyIncome { get; set; }

        /// <summary>
        /// Gets or sets the emptime.
        /// </summary>
        /// <value>The emptime.</value>
        public short Emptime { get; set; }

        /// <summary>
        /// Gets or sets the address month.
        /// </summary>
        /// <value>The address month.</value>
        public short AddressMonth { get; set; }

        /// <summary>
        /// Gets or sets the dob.
        /// </summary>
        /// <value>The dob.</value>
        public DateTime Dob { get; set; }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>The age.</value>
        public short Age { get; set; }

        /// <summary>
        /// Gets or sets the requested amount.
        /// </summary>
        /// <value>The requested amount.</value>
        public decimal RequestedAmount { get; set; }

        /// <summary>
        /// Gets or sets the SSN.
        /// </summary>
        /// <value>The SSN.</value>
        public string Ssn { get; set; }

        /// <summary>
        /// Gets or sets the total rows count.
        /// </summary>
        /// <value>The total rows count.</value>
        public int TotalRowsCount { get; set; }

        /// <summary>
        /// Gets or sets the page count.
        /// </summary>
        /// <value>The page count.</value>
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the rows per page.
        /// </summary>
        /// <value>The rows per page.</value>
        public int RowsPerPage { get; set; }

        /// <summary>
        /// Gets or sets the time zone now.
        /// </summary>
        /// <value>The time zone now.</value>
        public DateTime TimeZoneNow { get; set; }

        /// <summary>
        /// Gets or sets the time zone now string.
        /// </summary>
        /// <value>The time zone now string.</value>
        public string TimeZoneNowStr { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show notes].
        /// </summary>
        /// <value><c>true</c> if [show notes]; otherwise, <c>false</c>.</value>
        public bool ShowNotes { get; set; }


        /// <summary>
        /// Gets or sets the list states.
        /// </summary>
        /// <value>The list states.</value>
        public IList<SelectListItem> ListStates { get; set; }

        #endregion Properties
    }
}