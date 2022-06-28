// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="PermissionProvider.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Security;
using System.Collections.Generic;

namespace Adrack.Service.Security
{
    /// <summary>
    /// Represents a Permission Provider
    /// Implements the <see cref="Adrack.Service.Security.IPermissionProvider" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Security.IPermissionProvider" />
    public partial class PermissionProvider : IPermissionProvider
    {
        #region Contants

        #endregion

        #region Fields

        #region Access Application

        /// <summary>
        /// Access Content Management Page Application
        /// </summary>
        public static readonly Permission AccessContentManagementPageApplication =
            new Permission
            {
                Name = "Content Management Page: Access",
                Key = "AccessContentManagementPage-Application",
                EntityName = "Application",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AccessContentManagementPageApplicationKey = "AccessContentManagementPage-Application";

        /// <summary>
        /// The buyer reports access
        /// </summary>
        public static readonly Permission BuyerReportsAccess =
            new Permission
            {
                Name = "Buyer Reports: Access",
                Key = "BuyerReports-Access",
                EntityName = "Buyer Reports",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string BuyerReportsAccessKey = "BuyerReports-Access";

        /// <summary>
        /// The buyer reports by buyer channels
        /// </summary>
        public static readonly Permission BuyerReportsByBuyerChannels =
            new Permission
            {
                Name = "Buyer Reports: By Buyer Channels",
                Key = "BuyerReports-ByBuyerChannels",
                EntityName = "Buyer Reports",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string BuyerReportsByBuyerChannelsKey = "BuyerReports-ByBuyerChannels";

        /// <summary>
        /// The buyer reports by affiliate channels
        /// </summary>
        public static readonly Permission BuyerReportsByAffiliateChannels =
            new Permission
            {
                Name = "Buyer Reports: By Affiliate Channels",
                Key = "BuyerReports-ByAffiliateChannels",
                EntityName = "Buyer Reports",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string BuyerReportsByAffiliateChannelsKey = "BuyerReports-ByAffiliateChannels";

        /// <summary>
        /// The buyer reports by campaigns
        /// </summary>
        public static readonly Permission BuyerReportsByCampaigns =
            new Permission
            {
                Name = "Buyer Reports: By Campaigns",
                Key = "BuyerReports-ByCampaigns",
                EntityName = "Buyer Reports",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string BuyerReportsByCampaignsKey = "BuyerReports-ByCampaigns";

        /// <summary>
        /// The buyer reports by states
        /// </summary>
        public static readonly Permission BuyerReportsByStates =
            new Permission
            {
                Name = "Buyer Reports: By States",
                Key = "BuyerReports-ByStates",
                EntityName = "Buyer Reports",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string BuyerReportsByStatesKey = "BuyerReports-ByStates";

        /*********************** Affiliate reports *************************************/

        /// <summary>
        /// The affiliate reports access
        /// </summary>
        public static readonly Permission AffiliateReportsAccess =
            new Permission
            {
                Name = "Affiliate Reports: Access",
                Key = "AffiliateReports-Access",
                EntityName = "Affiliate Reports",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AffiliateReportsAccessKey = "AffiliateReports-Access";

        /// <summary>
        /// The affiliate reports by affiliate channels
        /// </summary>
        public static readonly Permission AffiliateReportsByAffiliateChannels =
             new Permission
             {
                 Name = "Affiliate Reports: By Affiliate Channels",
                 Key = "AffiliateReports-ByAffiliateChannels",
                 EntityName = "Affiliate Reports",
                 Active = true,
                 Deleted = false,
                 Description = null
             };
        public const string AffiliateReportsByAffiliateChannelsKey = "AffiliateReports-ByAffiliateChannels";

        /// <summary>
        /// The affiliate reports by campaigns
        /// </summary>
        public static readonly Permission AffiliateReportsByCampaigns =
            new Permission
            {
                Name = "Affiliate Reports: By Campaigns",
                Key = "AffiliateReports-ByCampaigns",
                EntityName = "Affiliate Reports",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AffiliateReportsByCampaignsKey = "AffiliateReports-ByCampaigns";

        /// <summary>
        /// The affiliate reports by states
        /// </summary>
        public static readonly Permission AffiliateReportsByStates =
            new Permission
            {
                Name = "Affiliate Reports: By States",
                Key = "AffiliateReports-ByStates",
                EntityName = "Affiliate Reports",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AffiliateReportsByStatesKey = "AffiliateReports-ByStates";

        /// <summary>
        /// The affiliate reports by epl
        /// </summary>
        public static readonly Permission AffiliateReportsByEPL =
            new Permission
            {
                Name = "Affiliate Reports: By EPL",
                Key = "AffiliateReports-ByEPL",
                EntityName = "Affiliate Reports",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AffiliateReportsByEPLKey = "AffiliateReports-ByEPL";

        // Campaigns
        /// <summary>
        /// The campaigns view
        /// </summary>
        public static readonly Permission CampaignsView =
            new Permission
            {
                Name = "Campaigns: View",
                Key = "Campaigns-View",
                EntityName = "Campaigns",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string CampaignsViewKey = "Campaigns-View";

        /// <summary>
        /// The campaigns modify
        /// </summary>
        public static readonly Permission CampaignsModify =
            new Permission
            {
                Name = "Campaigns: Modify",
                Key = "Campaigns-Modify",
                EntityName = "Campaigns",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string CampaignsModifyKey = "Campaigns-Modify";


        // Campaigns
        /// <summary>
        /// The campaigns filter
        /// </summary>
        public static readonly Permission CampaignsFilter =
            new Permission
            {
                Name = "Campaigns: Filter",
                Key = "Campaigns-Filter",
                EntityName = "Campaigns",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string CampaignsFilterKey = "Campaigns-Filter";

        /// <summary>
        /// The campaigns affiliate channels view
        /// </summary>
        public static readonly Permission CampaignsAffiliateChannelsView =
           new Permission
           {
               Name = "Affiliate Channels View",
               Key = "AffiliateChannels-View",
               EntityName = "Campaigns",
               Active = true,
               Deleted = false,
               Description = null
           };
        public const string CampaignsAffiliateChannelsViewKey = "AffiliateChannels-View";

        /// <summary>
        /// The campaigns affiliate channels modify
        /// </summary>
        public static readonly Permission CampaignsAffiliateChannelsModify =
           new Permission
           {
               Name = "Affiliate Channels Modify",
               Key = "AffiliateChannels-Modify",
               EntityName = "Campaigns",
               Active = true,
               Deleted = false,
               Description = null
           };
        public const string CampaignsAffiliateChannelsModifyKey = "AffiliateChannels-Modify";

        /// <summary>
        /// The campaigns buyer channels view
        /// </summary>
        public static readonly Permission CampaignsBuyerChannelsView =
           new Permission
           {
               Name = "Buyer Channels: View",
               Key = "BuyerChannels-View",
               EntityName = "Campaigns",
               Active = true,
               Deleted = false,
               Description = null
           };
        public const string CampaignsBuyerChannelsViewKey = "BuyerChannels-View";

        /// <summary>
        /// The campaigns buyer channels modify
        /// </summary>
        public static readonly Permission CampaignsBuyerChannelsModify =
           new Permission
           {
               Name = "Buyer Channels: Modify",
               Key = "BuyerChannels-Modify",
               EntityName = "Campaigns",
               Active = true,
               Deleted = false,
               Description = null
           };
        public const string CampaignsBuyerChannelsModifyKey = "BuyerChannels-Modify";

        /// <summary>
        /// The affiliates view
        /// </summary>
        public static readonly Permission AffiliatesView =
            new Permission
            {
                Name = "Affiliates: View",
                Key = "Affiliates-View",
                EntityName = "Affiliates",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AffiliatesViewKey = "Affiliates-View";

        /// <summary>
        /// The affiliates add
        /// </summary>
        public static readonly Permission AffiliatesAdd =
            new Permission
            {
                Name = "Add",
                Key = "Affiliates-Add",
                EntityName = "Affiliates",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AffiliatesAddKey = "Affiliates-Add";

        /// <summary>
        /// The affiliates view
        /// </summary>
        public static readonly Permission AffiliatesFilter =
            new Permission
            {
                Name = "Affiliates: Filter",
                Key = "Affiliates-Filter",
                EntityName = "Affiliates",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AffiliatesFilterKey = "Affiliates-Filter";

        /// <summary>
        /// The affiliates modify
        /// </summary>
        public static readonly Permission AffiliatesModify =
            new Permission
            {
                Name = "Modify",
                Key = "Affiliates-Modify",
                EntityName = "Affiliates",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AffiliatesModifyKey = "Affiliates-Modify";

        /// <summary>
        /// The affiliates bill frequency tab
        /// </summary>
        public static readonly Permission AffiliatesBillFrequencyTab =
            new Permission
            {
                Name = "Bill frequency tab",
                Key = "Affiliates-BillFrequencyTab",
                EntityName = "Affiliates",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AffiliatesBillFrequencyTabKey = "Affiliates-BillFrequencyTab";

        /// <summary>
        /// The affiliates show all
        /// </summary>
        public static readonly Permission AffiliatesShowAll =
            new Permission
            {
                Name = "Affiliates: Show all",
                Key = "Affiliates-ShowAll",
                EntityName = "Affiliates",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AffiliatesShowAllKey = "Affiliates-ShowAll";

        /// <summary>
        /// The affiliates documents access
        /// </summary>
        public static readonly Permission AffiliatesDocumentsAccess =
            new Permission
            {
                Name = "Affiliates: Documents access",
                Key = "Affiliates-DocumentsAccess",
                EntityName = "Affiliates",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AffiliatesDocumentsAccessKey = "Affiliates-DocumentsAccess";

        /// <summary>
        /// The buyers view
        /// </summary>
        public static readonly Permission BuyersView =
            new Permission
            {
                Name = "Buyers: View",
                Key = "Buyers-View",
                EntityName = "Buyers",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string BuyersViewKey = "Buyers-View";

        /// <summary>
        /// The buyers add
        /// </summary>
        public static readonly Permission BuyersAdd =
            new Permission
            {
                Name = "Add",
                Key = "Buyers-Add",
                EntityName = "Buyers",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string BuyersAddKey = "Buyers-Add";

        /// <summary>
        /// The buyers filter
        /// </summary>
        public static readonly Permission BuyersFilter =
            new Permission
            {
                Name = "Buyers: Filter",
                Key = "Buyers-Filter",
                EntityName = "Buyers",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string BuyersFilterKey = "Buyers-Filter";

        /// <summary>
        /// The buyers modify
        /// </summary>
        public static readonly Permission BuyersModify =
            new Permission
            {
                Name = "Modify",
                Key = "Buyers-Modify",
                EntityName = "Buyers",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string BuyersModifyKey = "Buyers-Modify";

        /// <summary>
        /// The buyers bill frequency tab
        /// </summary>
        public static readonly Permission BuyersBillFrequencyTab =
            new Permission
            {
                Name = "Payment options & Bill frequency",
                Key = "Buyers-BillFrequencyTab",
                EntityName = "Buyers",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string BuyersBillFrequencyTabKey = "Buyers-BillFrequencyTab";

        /// <summary>
        /// The buyers documents access
        /// </summary>
        public static readonly Permission BuyersDocumentsAccess =
            new Permission
            {
                Name = "Documents Access",
                Key = "Buyers-DocumentsAccess",
                EntityName = "Buyers",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string BuyersDocumentsAccessKey = "Buyers-DocumentsAccess";

        /// <summary>
        /// The buyers show all
        /// </summary>
        public static readonly Permission BuyersShowAll =
            new Permission
            {
                Name = "Buyers: Show all",
                Key = "Buyers-ShowAll",
                EntityName = "Buyers",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string BuyersShowAllKey = "Buyers-ShowAll";

        /// <summary>
        /// The affiliate channels modify
        /// </summary>
        public static readonly Permission AffiliateChannelsModify =
            new Permission
            {
                Name = "Affiliate Channels: Modify",
                Key = "AffiliateChannels-Modify",
                EntityName = "Afiliate Channels",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AffiliateChannelsModifyKey = "AffiliateChannels-Modify";

        /// <summary>
        /// The affiliate channels view
        /// </summary>
        public static readonly Permission AffiliateChannelsView =
            new Permission
            {
                Name = "Affiliate Channels: View",
                Key = "AffiliateChannels-View",
                EntityName = "Affiliate Channels",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AffiliateChannelsViewKey = "AffiliateChannels-View";

        /// <summary>
        /// The affiliate channels filter
        /// </summary>
        public static readonly Permission AffiliateChannelsFilter =
            new Permission
            {
                Name = "Affiliate Channels: Filter",
                Key = "AffiliateChannels-Filter",
                EntityName = "Affiliate Channels",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AffiliateChannelsFilterKey = "AffiliateChannels-Filter";

        /// <summary>
        /// The buyer channels modify
        /// </summary>
        public static readonly Permission BuyerChannelsModify =
            new Permission
            {
                Name = "Buyer Channels: Modify",
                Key = "BuyerChannels-Modify",
                EntityName = "Buyer Channels",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string BuyerChannelsModifyKey = "BuyerChannels-Modify";

        /// <summary>
        /// The buyer channels modify
        /// </summary>
        public static readonly Permission BuyerChannelsShowHistory =
            new Permission
            {
                Name = "Buyer Channels: ShowHistory",
                Key = "BuyerChannels-ShowHistory",
                EntityName = "Buyer Channels",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string BuyerChannelsShowHistoryKey = "BuyerChannels-ShowHistory";

        /// <summary>
        /// The buyer channels view
        /// </summary>
        public static readonly Permission BuyerChannelsView =
            new Permission
            {
                Name = "Buyer Channels: View",
                Key = "BuyerChannels-View",
                EntityName = "Buyer Channels",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string BuyerChannelsViewKey = "BuyerChannels-View";

        /// <summary>
        /// The buyer channels view
        /// </summary>
        public static readonly Permission BuyerChannelsFilter =
            new Permission
            {
                Name = "Buyer Channels: Filter",
                Key = "BuyerChannels-Filter",
                EntityName = "Buyer Channels",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string BuyerChannelsFilterKey = "BuyerChannels-Filter";

        /// <summary>
        /// The buyer invoices modify
        /// </summary>
        public static readonly Permission BuyerInvoicesModify =
            new Permission
            {
                Name = "Accounting: Buyer Invoices Modify",
                Key = "Accounting-BuyerInvoicesModify",
                EntityName = "Accounting",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string BuyerInvoicesModifyKey = "Accounting-BuyerInvoicesModify";

        /// <summary>
        /// The accounting approve buyer invoices
        /// </summary>
        public static readonly Permission AccountingApproveBuyerInvoices =
            new Permission
            {
                Name = "Accounting: Approve Buyer Invoices",
                Key = "Accounting-Approve-Buyer-Invoices",
                EntityName = "Accounting",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AccountingApproveBuyerInvoicesKey = "Accounting-Approve-Buyer-Invoices";

        /// <summary>
        /// The accounting disable buyer invoices
        /// </summary>
        public static readonly Permission AccountingDisableBuyerInvoices =
            new Permission
            {
                Name = "Accounting: Disable Buyer Invoices",
                Key = "Accounting-Disable-Buyer-Invoices",
                EntityName = "Accounting",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AccountingDisableBuyerInvoicesKey = "Accounting-Disable-Buyer-Invoices";

        /// <summary>
        /// The accounting pay buyer invoices
        /// </summary>
        public static readonly Permission AccountingPayBuyerInvoices =
            new Permission
            {
                Name = "Accounting: Pay Buyer Invoices",
                Key = "Accounting-Pay-Buyer-Invoices",
                EntityName = "Accounting",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AccountingPayBuyerInvoicesKey = "Accounting-Pay-Buyer-Invoices";

        /// <summary>
        /// The accounting download buyer invoices
        /// </summary>
        public static readonly Permission AccountingDownloadBuyerInvoices =
            new Permission
            {
                Name = "Accounting: Download Buyer Invoices",
                Key = "Accounting-Download-Buyer-Invoices",
                EntityName = "Accounting",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AccountingDownloadBuyerInvoicesKey = "Accounting-Download-Buyer-Invoices";

        // Buyer Payments
        /// <summary>
        /// The accounting add buyer payment
        /// </summary>
        public static readonly Permission AccountingAddBuyerPayment =
                new Permission
                {
                    Name = "Add Payment",
                    Key = "Accounting-Add-Buyer-Payment",
                    EntityName = "Accounting",
                    Active = true,
                    Deleted = false,
                    Description = null
                };
        public const string AccountingAddBuyerPaymentKey = "Accounting-Add-Buyer-Payment";

        /// <summary>
        /// The accounting edit buyer payment
        /// </summary>
        public static readonly Permission AccountingEditBuyerPayment =
        new Permission
        {
            Name = "Edit Payment",
            Key = "Accounting-Edit-Buyer-Payment",
            EntityName = "Accounting",
            Active = true,
            Deleted = false,
            Description = null
        };
        public const string AccountingEditBuyerPaymentKey = "Accounting-Edit-Buyer-Payment";

        /// <summary>
        /// The accounting delete buyer payment
        /// </summary>
        public static readonly Permission AccountingDeleteBuyerPayment =
        new Permission
        {
            Name = "Delete Payment",
            Key = "Accounting-Delete-Buyer-Payment",
            EntityName = "Accounting",
            Active = true,
            Deleted = false,
            Description = null
        };
        public const string AccountingDeleteBuyerPaymentKey = "Accounting-Delete-Buyer-Payment";
        //

        // Accounting Affiliate Payment Notice
        /// <summary>
        /// The accounting approve affiliate payment notice
        /// </summary>
        public static readonly Permission AccountingApproveAffiliatePaymentNotice =
                new Permission
                {
                    Name = "Approve",
                    Key = "Accounting-Approve-AffiliatePaymentNotice",
                    EntityName = "Accounting",
                    Active = true,
                    Deleted = false,
                    Description = null
                };
        public const string AccountingApproveAffiliatePaymentNoticeKey = "Accounting-Approve-AffiliatePaymentNotice";

        /// <summary>
        /// The accounting disable affiliate payment notice
        /// </summary>
        public static readonly Permission AccountingDisableAffiliatePaymentNotice =
            new Permission
            {
                Name = "Disable",
                Key = "Accounting-Disable-AffiliatePaymentNotice",
                EntityName = "Accounting",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AccountingDisableAffiliatePaymentNoticeKey = "Accounting-Disable-AffiliatePaymentNotice";

        /// <summary>
        /// The accounting pay affiliate payment notice
        /// </summary>
        public static readonly Permission AccountingPayAffiliatePaymentNotice =
            new Permission
            {
                Name = "Pay",
                Key = "Accounting-Pay-AffiliatePaymentNotice",
                EntityName = "Accounting",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AccountingPayAffiliatePaymentNoticeKey = "Accounting-Pay-AffiliatePaymentNotice";

        /// <summary>
        /// The accounting download affiliate payment notice
        /// </summary>
        public static readonly Permission AccountingDownloadAffiliatePaymentNotice =
            new Permission
            {
                Name = "Download",
                Key = "Accounting-Download-AffiliatePaymentNotice",
                EntityName = "Accounting",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AccountingDownloadAffiliatePaymentNoticeKey = "Accounting-Download-AffiliatePaymentNotice";

        // Refunded
        /// <summary>
        /// The accounting refunded change status
        /// </summary>
        public static readonly Permission AccountingRefundedChangeStatus =
                new Permission
                {
                    Name = "Refunded Change Status",
                    Key = "Accounting-Refunded-Change-Status",
                    EntityName = "Accounting",
                    Active = true,
                    Deleted = false,
                    Description = null
                };
        public const string AccountingRefundedChangeStatusKey = "Accounting-Refunded-Change-Status";

        // Generate custom Invoice
        /// <summary>
        /// The accounting generate custom invoices buyers
        /// </summary>
        public static readonly Permission AccountingGenerateCustomInvoicesBuyers =
                new Permission
                {
                    Name = "Invoice of Buyers",
                    Key = "Accounting-Generate-Custom-Invoices-Buyers",
                    EntityName = "Accounting",
                    Active = true,
                    Deleted = false,
                    Description = null
                };
        public const string AccountingGenerateCustomInvoicesBuyersKey = "Accounting-Generate-Custom-Invoices-Buyers";

        /// <summary>
        /// The accounting generate custom invoices affiliates
        /// </summary>
        public static readonly Permission AccountingGenerateCustomInvoicesAffiliates =
            new Permission
            {
                Name = "Payments notice of Affiliates",
                Key = "Accounting-Generate-Custom-Invoices-Affiliates",
                EntityName = "Accounting",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AccountingGenerateCustomInvoicesAffiliatesKey = "Accounting-Generate-Custom-Invoices-Affiliates";

        /// <summary>
        /// The affiliate payment notice modify
        /// </summary>
        public static readonly Permission AffiliatePaymentNoticeModify =
            new Permission
            {
                Name = "Accounting: Affiliate Payment Notice Modify",
                Key = "Accounting-AffiliatePaymentNoticeModify",
                EntityName = "Accounting",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string AffiliatePaymentNoticeModifyKey = "Accounting-AffiliatePaymentNoticeModify";

        /// <summary>
        /// The refunded leads modify
        /// </summary>
        public static readonly Permission RefundedLeadsModify =
            new Permission
            {
                Name = "Accounting: Refunded Leads Modify",
                Key = "Accounting-RefundedLeadsModify",
                EntityName = "Accounting",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string RefundedLeadsModifyKey = "Accounting-RefundedLeadsModify";

        // Support
        /// <summary>
        /// The support view
        /// </summary>
        public static readonly Permission SupportView =
                new Permission
                {
                    Name = "Support: View",
                    Key = "Support-View",
                    EntityName = "Support",
                    Active = true,
                    Deleted = false,
                    Description = null
                };
        public const string SupportViewKey = "Support-View";

        /// <summary>
        /// The support view tickets list
        /// </summary>
        public static readonly Permission SupportViewTicketsList =
               new Permission
               {
                   Name = "View Tickets List",
                   Key = "SupportViewTicketsList",
                   EntityName = "Support",
                   Active = true,
                   Deleted = false,
                   Description = null
               };
        public const string SupportViewTicketsListKey = "SupportViewTicketsList";

        /// <summary>
        /// The support add new ticket
        /// </summary>
        public static readonly Permission SupportAddNewTicket =
           new Permission
           {
               Name = "Add New Ticket",
               Key = "SupportAddNewTicket",
               EntityName = "Support",
               Active = true,
               Deleted = false,
               Description = null
           };
        public const string SupportAddNewTicketKey = "SupportAddNewTicket";

        /// <summary>
        /// The support change status
        /// </summary>
        public static readonly Permission SupportChangeStatus =
           new Permission
           {
               Name = "Change status",
               Key = "SupportChangeStatus",
               EntityName = "Support",
               Active = true,
               Deleted = false,
               Description = null
           };
        public const string SupportChangeStatusKey = "SupportChangeStatus";

        // Settings
        /// <summary>
        /// The settings company details
        /// </summary>
        public static readonly Permission SettingsCompanyDetails =
           new Permission
           {
               Name = "Company Details",
               Key = "SettingsCompanyDetails",
               EntityName = "Settings",
               Active = true,
               Deleted = false,
               Description = null
           };
        public const string SettingsCompanyDetailsKey = "SettingsCompanyDetails";

        /// <summary>
        /// The settings company details modify
        /// </summary>
        public static readonly Permission SettingsCompanyDetailsModify =
           new Permission
           {
               Name = "Modify",
               Key = "SettingsCompanyDetailsModify",
               EntityName = "Settings",
               Active = true,
               Deleted = false,
               Description = null
           };
        public const string SettingsCompanyDetailsModifyKey = "SettingsCompanyDetailsModify";

        /// <summary>
        /// The settings e mail templates modify
        /// </summary>
        public static readonly Permission SettingsEMailTemplatesModify =
           new Permission
           {
               Name = "Modify",
               Key = "SettingsEMailTemplatesModify",
               EntityName = "Settings",
               Active = true,
               Deleted = false,
               Description = null
           };
        public const string SettingsEMailTemplatesModifyKey = "SettingsEMailTemplatesModify";

        /// <summary>
        /// The settings SMTP modify
        /// </summary>
        public static readonly Permission SettingsSMTPModify =
          new Permission
          {
              Name = "Modify",
              Key = "SettingsSMTPModify",
              EntityName = "Settings",
              Active = true,
              Deleted = false,
              Description = null
          };
        public const string SettingsSMTPModifyKey = "SettingsSMTPModify";

        /// <summary>
        /// The settings time zone modify
        /// </summary>
        public static readonly Permission SettingsTimeZoneModify =
          new Permission
          {
              Name = "Modify",
              Key = "SettingsTimeZoneModify",
              EntityName = "Settings",
              Active = true,
              Deleted = false,
              Description = null
          };
        public const string SettingsTimeZoneModifyKey = "SettingsTimeZoneModify";

        /// <summary>
        /// The settings vertical add
        /// </summary>
        public static readonly Permission SettingsVerticalAdd =
            new Permission
            {
                Name = "Add",
                Key = "SettingsVerticalAdd",
                EntityName = "Settings",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string SettingsVerticalAddKey = "SettingsVerticalAdd";

        /// <summary>
        /// The settings vertical modify
        /// </summary>
        public static readonly Permission SettingsVerticalModify =
            new Permission
            {
                Name = "Modify",
                Key = "SettingsVerticalModify",
                EntityName = "Settings",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string SettingsVerticalModifyKey = "SettingsVerticalModify";

        /// <summary>
        /// The settings black lists add
        /// </summary>
        public static readonly Permission SettingsBlackListsAdd =
            new Permission
            {
                Name = "Add",
                Key = "SettingsBlackListsAdd",
                EntityName = "Settings",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string SettingsBlackListsAddKey = "SettingsBlackListsAdd";

        /// <summary>
        /// The settings black lists modify
        /// </summary>
        public static readonly Permission SettingsBlackListsModify =
            new Permission
            {
                Name = "Modify",
                Key = "SettingsBlackListsModify",
                EntityName = "Settings",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string SettingsBlackListsModifyKey = "SettingsBlackListsModify";

        /// <summary>
        /// The settings departments add
        /// </summary>
        public static readonly Permission SettingsDepartmentsAdd =
            new Permission
            {
                Name = "Add",
                Key = "SettingsDepartmentsAdd",
                EntityName = "Settings",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string SettingsDepartmentsAddKey = "SettingsDepartmentsAdd";

        /// <summary>
        /// The settings departments modify
        /// </summary>
        public static readonly Permission SettingsDepartmentsModify =
            new Permission
            {
                Name = "Modify",
                Key = "SettingsDepartmentsModify",
                EntityName = "Settings",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string SettingsDepartmentsModifyKey = "SettingsDepartmentsModify";

        /// <summary>
        /// The settings campaign templates add
        /// </summary>
        public static readonly Permission SettingsCampaignTemplatesAdd =
           new Permission
           {
               Name = "Add",
               Key = "SettingsCampaignTemplatesAdd",
               EntityName = "Settings",
               Active = true,
               Deleted = false,
               Description = null
           };
        public const string SettingsCampaignTemplatesAddKey = "SettingsCampaignTemplatesAdd";

        /// <summary>
        /// The settings campaign templates modify
        /// </summary>
        public static readonly Permission SettingsCampaignTemplatesModify =
            new Permission
            {
                Name = "Modify",
                Key = "SettingsCampaignTemplatesModify",
                EntityName = "Settings",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string SettingsCampaignTemplatesModifyKey = "SettingsCampaignTemplatesModify";

        /// <summary>
        /// The settings history
        /// </summary>
        public static readonly Permission SettingsHistory =
           new Permission
           {
               Name = "History",
               Key = "SettingsHistory",
               EntityName = "Settings",
               Active = true,
               Deleted = false,
               Description = null
           };
        public const string SettingsHistoryKey = "SettingsHistory";

        // Leads
        /// <summary>
        /// The leads view
        /// </summary>
        public static readonly Permission LeadsView =
            new Permission
            {
                Name = "LeadsView",
                Key = "Leads-View",
                EntityName = "Leads",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string LeadsViewKey = "Leads-View";


        /// <summary>
        /// The leads iinfo view
        /// </summary>
        public static readonly Permission LeadsIinfoView =
            new Permission
            {
                Name = "Lead info View",
                Key = "LeadsIinfoView",
                EntityName = "Leads",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string LeadsIinfoViewKey = "LeadsIinfoView";

        /// <summary>
        /// The leads refund request
        /// </summary>
        public static readonly Permission LeadsRefundRequest =
            new Permission
            {
                Name = "Refund Request",
                Key = "LeadsRefundRequest",
                EntityName = "Leads",
                Active = true,
                Deleted = false,
                Description = null
            };
        public const string LeadsRefundRequestKey = "LeadsRefundRequest";

        // User
        /// <summary>
        /// The user
        /// </summary>
        public static readonly Permission User =
                new Permission
                {
                    Name = "User",
                    Key = "UserView",
                    EntityName = "User",
                    Active = true,
                    Deleted = false,
                    Description = null
                };
        public const string UserViewKey = "UserView";

        /// <summary>
        /// The user roles view
        /// </summary>
        public static readonly Permission UserRolesView =
        new Permission
        {
            Name = "Roles",
            Key = "UserRolesView",
            EntityName = "User",
            Active = true,
            Deleted = false,
            Description = null
        };
        public const string UserRolesViewKey = "UserRolesView";

        /// <summary>
        /// The user roles network users view
        /// </summary>
        public static readonly Permission UserRolesNetworkUsersView =
        new Permission
        {
            Name = "Network users",
            Key = "UserRolesNetworkUsersView",
            EntityName = "User",
            Active = true,
            Deleted = false,
            Description = null
        };
        public const string UserRolesNetworkUsersViewKey = "UserRolesNetworkUsersView";

        /// <summary>
        /// The user roles network users modify
        /// </summary>
        public static readonly Permission UserRolesNetworkUsersModify =
        new Permission
        {
            Name = "Add/Modify",
            Key = "UserRolesNetworkUsersModify",
            EntityName = "User",
            Active = true,
            Deleted = false,
            Description = null
        };
        public const string UserRolesNetworkUsersModifyKey = "UserRolesNetworkUsersModify";

        /// <summary>
        /// The user roles affiliate user view
        /// </summary>
        public static readonly Permission UserRolesAffiliateUserView =
        new Permission
        {
            Name = "Affiliate users",
            Key = "UserRolesAffiliateUserView",
            EntityName = "User",
            Active = true,
            Deleted = false,
            Description = null
        };
        public const string UserRolesAffiliateUserViewKey = "UserRolesAffiliateUserView";

        /// <summary>
        /// The user roles affiliate user modify
        /// </summary>
        public static readonly Permission UserRolesAffiliateUserModify =
        new Permission
        {
            Name = "Add/Modify",
            Key = "UserRolesAffiliateUserModify",
            EntityName = "User",
            Active = true,
            Deleted = false,
            Description = null
        };
        public const string UserRolesAffiliateUserModifyKey = "UserRolesAffiliateUserModify";

        /// <summary>
        /// The user roles buyer user view
        /// </summary>
        public static readonly Permission UserRolesBuyerUserView =
        new Permission
        {
            Name = "Buyer users",
            Key = "UserRolesBuyerUserView",
            EntityName = "User",
            Active = true,
            Deleted = false,
            Description = null
        };
        public const string UserRolesBuyerUserViewKey = "UserRolesBuyerUserView";

        /// <summary>
        /// The user roles buyer user modify
        /// </summary>
        public static readonly Permission UserRolesBuyerUserModify =
        new Permission
        {
            Name = "Add/Modify",
            Key = "UserRolesBuyerUserModify",
            EntityName = "User",
            Active = true,
            Deleted = false,
            Description = null
        };
        public const string UserRolesBuyerUserModifyKey = "UserRolesBuyerUserModify";

        /******************************************************************************/

        /// <summary>
        /// The accounting access
        /// </summary>
        public static readonly Permission AccountingAccess =
        new Permission
        {
            Name = "Accounting: Access",
            Key = "Accounting-Access",
            EntityName = "Accounting",
            Active = true,
            Deleted = false,
            Description = null
        };
        public const string AccountingAccessKey = "Accounting-Access";

        #endregion Access Application

        #endregion Fields

        #region Methods

        /// <summary>
        /// Get Permissions
        /// </summary>
        /// <returns>Permission Collection Item</returns>
        public virtual IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                AccessContentManagementPageApplication,
            };
        }

        #endregion Methods
    }
}