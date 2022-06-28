// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="BuyerChannelModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Models.Lead
{
    /// <summary>
    /// Class BuyerChannelModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class BuyerChannelModel : BaseAppModel
    {
        /// <summary>
        /// Class TreeItem.
        /// </summary>
        public class TreeItem
        {
            /// <summary>
            /// Gets or sets the title.
            /// </summary>
            /// <value>The title.</value>
            public string title { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="TreeItem"/> is folder.
            /// </summary>
            /// <value><c>true</c> if folder; otherwise, <c>false</c>.</value>
            public bool folder { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="TreeItem"/> is expanded.
            /// </summary>
            /// <value><c>true</c> if expanded; otherwise, <c>false</c>.</value>
            public bool expanded { get; set; }

            /// <summary>
            /// Gets or sets the buyer channel template identifier.
            /// </summary>
            /// <value>The buyer channel template identifier.</value>
            public long BuyerChannelTemplateId { get; set; }

            /// <summary>
            /// Gets or sets the template field.
            /// </summary>
            /// <value>The template field.</value>
            public string TemplateField { get; set; }

            /// <summary>
            /// Gets or sets the campaign template identifier.
            /// </summary>
            /// <value>The campaign template identifier.</value>
            public long CampaignTemplateId { get; set; }

            /// <summary>
            /// Gets or sets the default value.
            /// </summary>
            /// <value>The default value.</value>
            public string DefaultValue { get; set; }

            /// <summary>
            /// Gets or sets the matchings.
            /// </summary>
            /// <value>The matchings.</value>
            public string Matchings { get; set; }

            /// <summary>
            /// Gets or sets the children.
            /// </summary>
            /// <value>The children.</value>
            public List<TreeItem> children { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="TreeItem"/> class.
            /// </summary>
            public TreeItem()
            {
                children = new List<TreeItem>();
                Matchings = "";
                folder = false;
                expanded = false;
            }
        }



        #region Constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public BuyerChannelModel()
        {
            this.ListCampaign = new List<SelectListItem>();
            this.ListBuyer = new List<SelectListItem>();
            this.ListStatus = new List<SelectListItem>();
            this.ListCampaignField = new List<SelectListItem>();
            this.ListFromFieldType = new List<SelectListItem>();
            this.ListDeliveryMethod = new List<SelectListItem>();
            this.ScheduleItems = new List<Controllers.BuyerChannelController.ScheduleItem>();
            this.ListDataFormat = new List<SelectListItem>();
            ListBuyerPriceOption = new List<SelectListItem>();
            ListAffiliatePriceOption = new List<SelectListItem>();
            ListAlwaysSoldOption = new List<SelectListItem>();
            TimeZones = new List<SelectListItem>();
            this.Filters = new List<Core.Domain.Lead.Filter>();
            this.CampaignTemplate = new List<Core.Domain.Lead.CampaignField>();
            BuyerChannelTemplateMatchings = new List<BuyerChannelTemplateMatching>();
            ListAffiliateChannels = new List<SelectListItem>();
            ListWinResponsePostMethod = new List<SelectListItem>();
            ListChildChannels = new List<SelectListItem>();
            this.ListBuyerChannels = new List<SelectListItem>();
            ListResponseFormats = new List<SelectListItem>();
            AllowedAffiliateChannels = "";
            CampaignId = 0;
            MaxDuplicateDays = 0;
            Timeout = 0;
            AfterTimeout = 0;
            StatusChangeMinutes = 30;
        }

        #endregion Constructor



        #region Properties

        /// <summary>
        /// Gets or sets the buyer channel identifier.
        /// </summary>
        /// <value>The buyer channel identifier.</value>
        public long BuyerChannelId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public BuyerChannelStatuses Status { get; set; }

        /// <summary>
        /// Gets or sets the XML template.
        /// </summary>
        /// <value>The XML template.</value>
        public string XmlTemplate { get; set; }

        /// <summary>
        /// Gets or sets the campaign identifier.
        /// </summary>
        /// <value>The campaign identifier.</value>
        public long CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the type of the campaign.
        /// </summary>
        /// <value>The type of the campaign.</value>
        public CampaignTypes CampaignType { get; set; }

        /// <summary>
        /// Gets or sets the buyer identifier.
        /// </summary>
        /// <value>The buyer identifier.</value>
        public long BuyerId { get; set; }

        public bool ReturnToLocalList { get; set; }

        /// <summary>
        /// Gets or sets the list campaign.
        /// </summary>
        /// <value>The list campaign.</value>
        public IList<SelectListItem> ListCampaign { get; set; }

        /// <summary>
        /// Gets or sets the list buyer.
        /// </summary>
        /// <value>The list buyer.</value>
        public IList<SelectListItem> ListBuyer { get; set; }

        /// <summary>
        /// Gets or sets the list status.
        /// </summary>
        /// <value>The list status.</value>
        public IList<SelectListItem> ListStatus { get; set; }

        /// <summary>
        /// Gets or sets the list campaign field.
        /// </summary>
        /// <value>The list campaign field.</value>
        public IList<SelectListItem> ListCampaignField { get; set; }

        /// <summary>
        /// Gets or sets the type of the list from field.
        /// </summary>
        /// <value>The type of the list from field.</value>
        public IList<SelectListItem> ListFromFieldType { get; set; }

        /// <summary>
        /// Gets or sets the list delivery method.
        /// </summary>
        /// <value>The list delivery method.</value>
        public IList<SelectListItem> ListDeliveryMethod { get; set; }

        /// <summary>
        /// Gets or sets the list data format.
        /// </summary>
        /// <value>The list data format.</value>
        public IList<SelectListItem> ListDataFormat { get; set; }

        /// <summary>
        /// Gets or sets the list buyer price option.
        /// </summary>
        /// <value>The list buyer price option.</value>
        public IList<SelectListItem> ListBuyerPriceOption { get; set; }

        /// <summary>
        /// Gets or sets the list affiliate price option.
        /// </summary>
        /// <value>The list affiliate price option.</value>
        public IList<SelectListItem> ListAffiliatePriceOption { get; set; }

        /// <summary>
        /// Gets or sets the list always sold option.
        /// </summary>
        /// <value>The list always sold option.</value>
        public IList<SelectListItem> ListAlwaysSoldOption { get; set; }

        /// <summary>
        /// Gets or sets the list affiliate channels.
        /// </summary>
        /// <value>The list affiliate channels.</value>
        public IList<SelectListItem> ListAffiliateChannels { get; set; }

        /// <summary>
        /// Gets or sets the buyer channel template matchings.
        /// </summary>
        /// <value>The buyer channel template matchings.</value>
        public IList<BuyerChannelTemplateMatching> BuyerChannelTemplateMatchings { get; set; }

        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>The filters.</value>
        public List<Adrack.Core.Domain.Lead.Filter> Filters { get; set; }

        /// <summary>
        /// Gets or sets the filter conditions.
        /// </summary>
        /// <value>The filter conditions.</value>
        public List<Adrack.Core.Domain.Lead.BuyerChannelFilterCondition> FilterConditions { get; set; }

        /// <summary>
        /// Gets or sets the campaign template.
        /// </summary>
        /// <value>The campaign template.</value>
        public List<CampaignField> CampaignTemplate { get; set; }

        /// <summary>
        /// Gets or sets the schedule items.
        /// </summary>
        /// <value>The schedule items.</value>
        public List<Adrack.Web.ContentManagement.Controllers.BuyerChannelController.ScheduleItem> ScheduleItems { get; set; }

        /// <summary>
        /// Gets or sets the accepted field.
        /// </summary>
        /// <value>The accepted field.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.AcceptedField")]
        public string AcceptedField { get; set; }

        /// <summary>
        /// Gets or sets the accepted value.
        /// </summary>
        /// <value>The accepted value.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.AcceptedValue")]
        public string AcceptedValue { get; set; }

        /// <summary>
        /// Gets or sets the accepted from field.
        /// </summary>
        /// <value>The accepted from field.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.AcceptedFromField")]
        public short AcceptedFromField { get; set; }

        /// <summary>
        /// Gets or sets the error field.
        /// </summary>
        /// <value>The error field.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.ErrorField")]
        public string ErrorField { get; set; }

        /// <summary>
        /// Gets or sets the error value.
        /// </summary>
        /// <value>The error value.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.ErrorValue")]
        public string ErrorValue { get; set; }

        /// <summary>
        /// Gets or sets the error from field.
        /// </summary>
        /// <value>The error from field.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.ErrorFromField")]
        public short ErrorFromField { get; set; }

        /// <summary>
        /// Gets or sets the rejected field.
        /// </summary>
        /// <value>The rejected field.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.RejectedField")]
        public string RejectedField { get; set; }

        /// <summary>
        /// Gets or sets the rejected value.
        /// </summary>
        /// <value>The rejected value.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.RejectedValue")]
        public string RejectedValue { get; set; }

        /// <summary>
        /// Gets or sets the rejected from field.
        /// </summary>
        /// <value>The rejected from field.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.RejectedFromField")]
        public short RejectedFromField { get; set; }

        /// <summary>
        /// Gets or sets the test field.
        /// </summary>
        /// <value>The test field.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.TestField")]
        public string TestField { get; set; }

        /// <summary>
        /// Gets or sets the test value.
        /// </summary>
        /// <value>The test value.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.TestValue")]
        public string TestValue { get; set; }

        /// <summary>
        /// Gets or sets the test from field.
        /// </summary>
        /// <value>The test from field.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.TestFromField")]
        public short TestFromField { get; set; }

        /// <summary>
        /// Gets or sets the redirect field.
        /// </summary>
        /// <value>The redirect field.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.RedirectField")]
        public string RedirectField { get; set; }

        /// <summary>
        /// Gets or sets the message field.
        /// </summary>
        /// <value>The message field.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.MessageField")]
        public string MessageField { get; set; }

        /// <summary>
        /// Gets or sets the price field.
        /// </summary>
        /// <value>The price field.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.PriceField")]
        public string PriceField { get; set; }

        /// <summary>
        /// Gets or sets the account id field.
        /// </summary>
        /// <value>The price field.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.AccountIdField")]
        public string AccountIdField { get; set; }

        public string Delimeter { get; set; }

        public string PriceRejectField { get; set; }

        public string PriceRejectValue { get; set; }


        /// <summary>
        /// Gets or sets the posting URL.
        /// </summary>
        /// <value>The posting URL.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.PostingUrl")]
        public string PostingUrl { get; set; }

        /// <summary>
        /// Gets or sets the delivery method.
        /// </summary>
        /// <value>The delivery method.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.DeliveryMethod")]
        public short DeliveryMethod { get; set; }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>The timeout.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.Timeout")]
        public short Timeout { get; set; }

        /// <summary>
        /// Gets or sets the after timeout.
        /// </summary>
        /// <value>The after timeout.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.AfterTimeout")]
        public short AfterTimeout { get; set; }

        /// <summary>
        /// Gets or sets the notification email.
        /// </summary>
        /// <value>The notification email.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.NotificationEmail")]
        public string NotificationEmail { get; set; }

        /// <summary>
        /// Gets or sets the affiliate price.
        /// </summary>
        /// <value>The affiliate price.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.AffiliatePrice")]
        public decimal AffiliatePrice { get; set; }

        /// <summary>
        /// Gets or sets the buyer price.
        /// </summary>
        /// <value>The buyer price.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.BuyerPrice")]
        public decimal BuyerPrice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [cup reached notification].
        /// </summary>
        /// <value><c>true</c> if [cap reached notification]; otherwise, <c>false</c>.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.CapReachedNotification")]
        public bool CapReachedNotification { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [timeout notification].
        /// </summary>
        /// <value><c>true</c> if [timeout notification]; otherwise, <c>false</c>.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.TimeoutNotification")]
        public bool TimeoutNotification { get; set; }

        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        /// <value>The base URL.</value>
        public string BaseUrl { get; set; }

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
        [AppLocalizedStringDisplayName("BuyerChannel.Field.PostingHeaders")]
        public string PostingHeaders { get; set; }

        /// <summary>
        /// Gets or sets the buyer price option.
        /// </summary>
        /// <value>The buyer price option.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.BuyerPriceOption")]
        public short BuyerPriceOption { get; set; }

        /// <summary>
        /// Gets or sets the affiliate price option.
        /// </summary>
        /// <value>The affiliate price option.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.AffiliatePriceOption")]
        public short AffiliatePriceOption { get; set; }

        /// <summary>
        /// Gets or sets the always sold option.
        /// </summary>
        /// <value>The always sold option.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.AlwaysSoldOption")]
        public short AlwaysSoldOption { get; set; }

        /// <summary>
        /// Gets or sets the type of the price.
        /// </summary>
        /// <value>The type of the price.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.PriceType")]
        public short PriceType { get; set; }

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
        /// Gets or sets the redirect URL.
        /// </summary>
        /// <value>The redirect URL.</value>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Gets or sets the maximum duplicate days.
        /// </summary>
        /// <value>The maximum duplicate days.</value>
        [AppLocalizedStringDisplayName("BuyerChannel.Field.MaxDuplicateDays")]
        public short? MaxDuplicateDays { get; set; }

        /// <summary>
        /// Gets or sets the holidays.
        /// </summary>
        /// <value>The holidays.</value>
        public List<SelectListItem> Holidays { get; set; }

        /// <summary>
        /// Gets or sets the time zones.
        /// </summary>
        /// <value>The time zones.</value>
        public List<SelectListItem> TimeZones { get; set; }


        /// <summary>
        /// Gets or sets the selected time zones.
        /// </summary>
        /// <value>The selected time zones.</value>
        public string SelectedTimeZone { get; set; }

        public bool SubIdWhiteListEnabled { get; set; }

        public bool EnableCustomPriceReject { get; set; }

        public string PriceRejectWinResponse { get; set; }

        public bool FieldAppendEnabled { get; set; }

        public string WinResponseUrl { get; set; }

        public string WinResponsePostMethod { get; set; }

        public string LeadIdField { get; set; }

        public List<SelectListItem> ListWinResponsePostMethod { get; set; }

        public string ChildChannels { get; set; }

        public List<SelectListItem> ListChildChannels { get; set; }

        public IList<SelectListItem> ListBuyerChannels { get; set; }

        public short ResponseFormat { get; set; }

        public IList<SelectListItem> ListResponseFormats { get; set; }

        public string ChannelMappingUniqueId { get; set; }

        public bool StatusAutoChange { get; set; }

        public short StatusChangeMinutes { get; set; }

        public short ChangeStatusAfterCount { get; set; }

        public int DailyCap { get; set; }

        public string Note { get; set; }

        #endregion Properties
    }
}