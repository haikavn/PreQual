// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerChannel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Attributes;
using System;
using System.Collections.Generic;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Class BuyerChannel.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    [Tracked]
    public partial class BuyerChannel : BaseEntity
    {
        #region Fields

        // private ICollection<User> _users;

        /// <summary>
        /// Attached Affiliate Channels
        /// </summary>
        private ICollection<AffiliateChannel> _attachedAffiliateChannels;

        private ICollection<BuyerChannelFilterCondition> _buyerChannelFilterConditions;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or Sets the State province Identifier
        /// </summary>
        /// <value>The campaign identifier.</value>
        public long CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the buyer identifier.
        /// </summary>
        /// <value>The buyer identifier.</value>
        public long BuyerId { get; set; }

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
        [Tracked]
        public BuyerChannelStatuses Status { get; set; }

        /// <summary>
        /// Gets or sets the XML template.
        /// </summary>
        /// <value>The XML template.</value>
        public string XmlTemplate { get; set; }

        /// <summary>
        /// Gets or sets the accepted field.
        /// </summary>
        /// <value>The accepted field.</value>
        [Tracked]
        public string AcceptedField { get; set; }

        /// <summary>
        /// Gets or sets the accepted value.
        /// </summary>
        /// <value>The accepted value.</value>
        [Tracked]
        public string AcceptedValue { get; set; }

        /// <summary>
        /// Gets or sets the accepted from.
        /// </summary>
        /// <value>The accepted from.</value>
        [Tracked]
        public short AcceptedFrom { get; set; }

        /// <summary>
        /// Gets or sets the error field.
        /// </summary>
        /// <value>The error field.</value>
        [Tracked]
        public string ErrorField { get; set; }

        /// <summary>
        /// Gets or sets the error value.
        /// </summary>
        /// <value>The error value.</value>
        [Tracked]
        public string ErrorValue { get; set; }

        /// <summary>
        /// Gets or sets the error from.
        /// </summary>
        /// <value>The error from.</value>
        [Tracked]
        public short ErrorFrom { get; set; }

        /// <summary>
        /// Gets or sets the rejected field.
        /// </summary>
        /// <value>The rejected field.</value>
        [Tracked]
        public string RejectedField { get; set; }

        /// <summary>
        /// Gets or sets the rejected value.
        /// </summary>
        /// <value>The rejected value.</value>
        [Tracked]
        public string RejectedValue { get; set; }

        /// <summary>
        /// Gets or sets the rejected from.
        /// </summary>
        /// <value>The rejected from.</value>
        [Tracked]
        public short RejectedFrom { get; set; }

        /// <summary>
        /// Gets or sets the test field.
        /// </summary>
        /// <value>The test field.</value>
        [Tracked]
        public string TestField { get; set; }

        /// <summary>
        /// Gets or sets the test value.
        /// </summary>
        /// <value>The test value.</value>
        [Tracked]
        public string TestValue { get; set; }

        /// <summary>
        /// Gets or sets the test from.
        /// </summary>
        /// <value>The test from.</value>
        [Tracked]
        public short TestFrom { get; set; }

        /// <summary>
        /// Gets or sets the redirect field.
        /// </summary>
        /// <value>The redirect field.</value>
        [Tracked]
        public string RedirectField { get; set; }

        /// <summary>
        /// Gets or sets the message field.
        /// </summary>
        /// <value>The message field.</value>
        [Tracked]
        public string MessageField { get; set; }

        /// <summary>
        /// Gets or sets the price field.
        /// </summary>
        /// <value>The price field.</value>
        [Tracked]
        public string PriceField { get; set; }

        public string Delimeter { get; set; }

        public string PriceRejectField { get; set; }

        public string PriceRejectValue { get; set; }

        /// <summary>
        /// Gets or sets the posting URL.
        /// </summary>
        /// <value>The posting URL.</value>
        [Tracked]
        public string PostingUrl { get; set; }

        /// <summary>
        /// Gets or sets the delivery method.
        /// </summary>
        /// <value>The delivery method.</value>
        [Tracked]
        public short DeliveryMethod { get; set; }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>The timeout.</value>
        [Tracked]
        public short Timeout { get; set; }

        /// <summary>
        /// Gets or sets the after timeout.
        /// </summary>
        /// <value>The after timeout.</value>
        [Tracked]
        public short AfterTimeout { get; set; }

        /// <summary>
        /// Gets or sets the notification email.
        /// </summary>
        /// <value>The notification email.</value>
        [Tracked]
        public string NotificationEmail { get; set; }

        /// <summary>
        /// Gets or sets the affiliate price.
        /// </summary>
        /// <value>The affiliate price.</value>
        [Tracked]
        public decimal AffiliatePrice { get; set; }

        /// <summary>
        /// Gets or sets the buyer price.
        /// </summary>
        /// <value>The buyer price.</value>
        [Tracked]
        public decimal BuyerPrice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [cap reached notification].
        /// </summary>
        /// <value><c>true</c> if [cap reached notification]; otherwise, <c>false</c>.</value>
        [Tracked]
        public bool CapReachedNotification { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [timeout notification].
        /// </summary>
        /// <value><c>true</c> if [timeout notification]; otherwise, <c>false</c>.</value>
        [Tracked]
        public bool TimeoutNotification { get; set; }

        /// <summary>
        /// Gets or sets the order number.
        /// </summary>
        /// <value>The order number.</value>
        public int OrderNum { get; set; }

        public int? GroupNum { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is fixed.
        /// </summary>
        /// <value><c>true</c> if this instance is fixed; otherwise, <c>false</c>.</value>
        public bool IsFixed { get; set; }

        /// <summary>
        /// Gets or sets the allowed affiliate channels.
        /// </summary>
        /// <value>The allowed affiliate channels.</value>
        public string AllowedAffiliateChannels { get; set; }

        /// <summary>
        /// Gets or sets the data format.
        /// </summary>
        /// <value>The data format.</value>
        public short DataFormat { get; set; }

        /// <summary>
        /// Gets or sets the posting headers.
        /// </summary>
        /// <value>The posting headers.</value>
        public string PostingHeaders { get; set; }

        /// <summary>
        /// Gets or sets the buyer price option.
        /// </summary>
        /// <value>The buyer price option.</value>
        public BuyerPriceOptions BuyerPriceOption { get; set; }

        /// <summary>
        /// Gets or sets the affiliate price option.
        /// </summary>
        /// <value>The affiliate price option.</value>
        public short AffiliatePriceOption { get; set; }

        /// <summary>
        /// Gets or sets the always sold option.
        /// </summary>
        /// <value>The always sold option.</value>
        public short AlwaysSoldOption { get; set; }

        /// <summary>
        /// Gets or sets the zip code targeting.
        /// </summary>
        /// <value>The zip code targeting.</value>
        public string ZipCodeTargeting { get; set; }

        /// <summary>
        /// Gets or sets the state targeting.
        /// </summary>
        /// <value>The state targeting.</value>
        public string StateTargeting { get; set; }

        /// <summary>
        /// Gets or sets the minimum age targeting.
        /// </summary>
        /// <value>The minimum age targeting.</value>
        public short MinAgeTargeting { get; set; }

        /// <summary>
        /// Gets or sets the maximum age targeting.
        /// </summary>
        /// <value>The maximum age targeting.</value>
        public short MaxAgeTargeting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable zip code targeting].
        /// </summary>
        /// <value><c>true</c> if [enable zip code targeting]; otherwise, <c>false</c>.</value>
        public bool EnableZipCodeTargeting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable state targeting].
        /// </summary>
        /// <value><c>true</c> if [enable state targeting]; otherwise, <c>false</c>.</value>
        public bool EnableStateTargeting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable age targeting].
        /// </summary>
        /// <value><c>true</c> if [enable age targeting]; otherwise, <c>false</c>.</value>
        public bool EnableAgeTargeting { get; set; }

        /// <summary>
        /// Gets or sets the zip code condition.
        /// </summary>
        /// <value>The zip code condition.</value>
        public short ZipCodeCondition { get; set; }

        /// <summary>
        /// Gets or sets the state condition.
        /// </summary>
        /// <value>The state condition.</value>
        public short StateCondition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BuyerChannel"/> is deleted.
        /// </summary>
        /// <value><c>null</c> if [deleted] contains no value, <c>true</c> if [deleted]; otherwise, <c>false</c>.</value>
        public bool? Deleted { get; set; }

        /// <summary>
        /// Gets or sets the holidays.
        /// </summary>
        /// <value>The holidays.</value>
        public string Holidays { get; set; }

        /// <summary>
        /// Gets or sets the redirect URL.
        /// </summary>
        /// <value>The redirect URL.</value>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Gets or sets the maximum duplicate days.
        /// </summary>
        /// <value>The maximum duplicate days.</value>
        public short? MaxDuplicateDays { get; set; }

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets or sets the time zone string.
        /// </summary>
        /// <value>The time zone string.</value>
        public string TimeZoneStr { get; set; }

        public double? LeadAcceptRate { get; set; }

        public bool? SubIdWhiteListEnabled { get; set; }

        public string AccountIdField { get; set; }

        public bool? EnableCustomPriceReject { get; set; }

        public string PriceRejectWinResponse { get; set; }

        public bool? FieldAppendEnabled { get; set; }

        public string WinResponseUrl { get; set; }

        public string WinResponsePostMethod { get; set; }

        public string LeadIdField { get; set; }

        public string ChildChannels { get; set; }

        public short? ResponseFormat { get; set; }

        public string ChannelMappingUniqueId { get; set; }

        public string StatusStr { get; set; }

        public DateTime? StatusExpireDate { get; set; }

        public bool? StatusAutoChange { get; set; }

        public short? StatusChangeMinutes { get; set; }

        public short? ChangeStatusAfterCount { get; set; }

        public short? CurrentStatusChangeNum { get; set; }

        public int? DailyCap { get; set; }

        public string Note { get; set; }

        public short? CapReachEmailCount { get; set;}

        public long? CountryId { get; set; }

        public int HolidayYear { get; set; }

        public bool HolidayAnnualAutoRenew { get; set; }

        public bool HolidayIgnore { get; set; }

        public long? ManagerId { get; set; }

        public bool? AlwaysBuyerPrice { get; set; }

        #endregion Properties

        #region Navigation Properties


        public virtual ICollection<BuyerChannelFilterCondition> BuyerChannelFilterConditions
        {
            get
            {
                if (_buyerChannelFilterConditions != null)
                    return _buyerChannelFilterConditions;
                _buyerChannelFilterConditions = new List<BuyerChannelFilterCondition>();
                return _buyerChannelFilterConditions;
            }
            set
            {
                _buyerChannelFilterConditions = value;
            }
        }

        #endregion Navigation Properties
    }
}