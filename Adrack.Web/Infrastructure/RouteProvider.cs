// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="RouteProvider.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Web.Framework.Localization;
using Adrack.Web.Framework.Mvc.Route;
using System.Web.Mvc;
using System.Web.Routing;

namespace Adrack.Web.Infrastructure
{
    /// <summary>
    ///     Represents a Route Provider
    ///     Implements the <see cref="IRouteProvider" />
    /// </summary>
    /// <seealso cref="IRouteProvider" />
    public class RouteProvider : IRouteProvider
    {
        #region Methods

        /// <summary>
        ///     Register Routes
        /// </summary>
        /// <param name="routeCollection">Route Collection</param>
        public void RegisterRoutes(RouteCollection routeCollection)
        {
            #region Home

            // Home Page
            routeCollection.MapLocalizedRoute("HomePage", "", new { controller = "Home", action = "Index" },
                new[] { "Adrack.Web.Controllers" });

            routeCollection.MapLocalizedRoute("TestImport", "testimport/{id}",
                new { controller = "Home", action = "TestImport", id = UrlParameter.Optional },
                new[] { "Adrack.Web.Controllers" });

            routeCollection.MapLocalizedRoute("Import", "import/{id}",
                new { controller = "Home", action = "Import", id = UrlParameter.Optional },
                new[] { "Adrack.Web.Controllers" });
            routeCollection.MapLocalizedRoute("ImportC", "importc/{id}",
                new { controller = "Home", action = "ImportC", id = UrlParameter.Optional },
                new[] { "Adrack.Web.Controllers" });
            routeCollection.MapLocalizedRoute("SubscribePushUser", "subscribepushuser/{id}",
                new { controller = "Home", action = "SubscribePushUser" }, new[] { "Adrack.Web.Controllers" });

            routeCollection.MapLocalizedRoute("HomeTest", "test/{id}",
                new { controller = "Home", action = "Test", id = UrlParameter.Optional },
                new[] { "Adrack.Web.Controllers" });

            routeCollection.MapLocalizedRoute("Navigate", "navigate/{id}",
                new { controller = "Home", action = "Navigate" }, new[] { "Adrack.Web.Controllers" });

            routeCollection.MapLocalizedRoute("TestResp", "testresp", new { controller = "Home", action = "TestResp" },
                new[] { "Adrack.Web.Controllers" });

            routeCollection.MapLocalizedRoute("TestJsonResp", "testjsonresp",
                new { controller = "Home", action = "TestJsonResp" }, new[] { "Adrack.Web.Controllers" });

            #endregion Home

            #region Membership

            routeCollection.MapLocalizedRoute("Validate", "validate",
                new { controller = "Membership", action = "Validate" }, new[] { "Adrack.Web.Controllers" });

            routeCollection.MapLocalizedRoute("RegistrationRequest", "registrationrequest",
                new { controller = "Membership", action = "RegistrationRequest" }, new[] { "Adrack.Web.Controllers" });

            // Login
            routeCollection.MapLocalizedRoute("Login", "login/", new { controller = "Membership", action = "Login" },
                new[] { "Adrack.Web.Controllers" });

            routeCollection.MapLocalizedRoute("RemoteLogin", "login/execute",
                new { controller = "Membership", action = "RemoteLogin" }, new[] { "Adrack.Web.Controllers" });
            routeCollection.MapLocalizedRoute("RemoteCheck", "login/check",
                new { controller = "Membership", action = "RemoteCheck" }, new[] { "Adrack.Web.Controllers" });

            // Logout
            routeCollection.MapLocalizedRoute("Logout", "logout/", new { controller = "Membership", action = "Logout" },
                new[] { "Adrack.Web.Controllers" });

            // Register
            routeCollection.MapLocalizedRoute("Register", "register/",
                new { controller = "Membership", action = "Register" }, new[] { "Adrack.Web.Controllers" });

            // Register
            routeCollection.MapLocalizedRoute("RegisterAffiliate", "registeraffiliate",
                new { controller = "Membership", action = "RegisterAffiliate" }, new[] { "Adrack.Web.Controllers" });

            routeCollection.MapLocalizedRoute("RegisterBuyer", "registerbuyer",
                new { controller = "Membership", action = "RegisterBuyer" }, new[] { "Adrack.Web.Controllers" });

            // Forgot Password
            routeCollection.MapLocalizedRoute("ForgotPassword", "forgot-password",
                new { controller = "Membership", action = "ForgotPassword" }, new[] { "Adrack.Web.Controllers" });

            // Forgot Password Confirmation
            routeCollection.MapLocalizedRoute("ForgotPasswordConfirmation", "forgotpassword/confirmation",
                new { controller = "Membership", action = "ForgotPasswordConfirmation" },
                new[] { "Adrack.Web.Controllers" });

            routeCollection.MapLocalizedRoute("ChangePassword", "changepassword/{id}",
                new { controller = "Membership", action = "ChangePassword", id = UrlParameter.Optional },
                new[] { "Adrack.Web.Controllers" });

            // Register Result
            routeCollection.MapLocalizedRoute("RegisterResult", "registerresult/{resultId}",
                new { controller = "Membership", action = "RegisterResult" }, new { resultId = @"\d+" },
                new[] { "Adrack.Web.Controllers" });

            // Check Username Availability
            routeCollection.MapLocalizedRoute("CheckUsernameAvailability", "user/checkusernameavailability",
                new { controller = "Membership", action = "CheckUsernameAvailability" },
                new[] { "Adrack.Web.Controllers" });

            // Membership Activation
            routeCollection.MapLocalizedRoute("Activation", "user/activation",
                new { controller = "Membership", action = "Activation" }, new[] { "Adrack.Web.Controllers" });

            routeCollection.MapLocalizedRoute("GetUsersByAffiliate", "getusersbyaffiliate/{affiliateId}",
                new { controller = "Membership", action = "GetUsersByAffiliate" }, new[] { "Adrack.Web.Controllers" });
            routeCollection.MapLocalizedRoute("GetUsersByBuyer", "getusersbybuyer/{buyerId}",
                new { controller = "Membership", action = "GetUsersByBuyer", buyerId = UrlParameter.Optional },
                new[] { "Adrack.Web.Controllers" });

            routeCollection.MapLocalizedRoute("GetUsers", "getusers",
                new { controller = "Membership", action = "GetUsers" }, new[] { "Adrack.Web.Controllers" });
            routeCollection.MapLocalizedRoute("GetAffiliateUsers", "getaffiliateusers",
                new { controller = "Membership", action = "GetAffiliateUsers" }, new[] { "Adrack.Web.Controllers" });
            routeCollection.MapLocalizedRoute("GetBuyerUsers", "getbuyerusers",
                new { controller = "Membership", action = "GetBuyerUsers" }, new[] { "Adrack.Web.Controllers" });

            routeCollection.MapLocalizedRoute("DeleteUser", "deleteuser",
                new { controller = "Membership", action = "DeleteUser" }, new[] { "Adrack.Web.Controllers" });

            #endregion Membership

            #region Common

            // Page Error
            routeCollection.MapLocalizedRoute("PageError", "page-error/{statusCode}",
                new { controller = "Common", action = "PageError" }, new { statusCode = @"\d+" },
                new[] { "Adrack.Web.Controllers" });

            // Page Unauthorized
            routeCollection.MapLocalizedRoute("PageUnauthorized", "page-unauthorized",
                new { controller = "Common", action = "PageUnauthorized" }, new[] { "Adrack.Web.Controllers" });

            // Page Not Found
            routeCollection.MapLocalizedRoute("PageNotFound", "page-not-found",
                new { controller = "Common", action = "PageNotFound" }, new[] { "Adrack.Web.Controllers" });

            // Site Maintenance
            routeCollection.MapLocalizedRoute("SiteMaintenance", "site-maintenance",
                new { controller = "Common", action = "SiteMaintenance" }, new[] { "Adrack.Web.Controllers" });

            // Content Search
            routeCollection.MapLocalizedRoute("CommonSearch", "search/", new { controller = "Common", action = "Search" },
                new[] { "Adrack.Web.Controllers" });

            // About Us
            routeCollection.MapLocalizedRoute("AboutUs", "about-us", new { controller = "Common", action = "AboutUs" },
                new[] { "Adrack.Web.Controllers" });

            // About Us
            routeCollection.MapLocalizedRoute("ContactUs", "contact-us",
                new { controller = "Common", action = "ContactUs" }, new[] { "Adrack.Web.Controllers" });

            #endregion Common

            #region Content

            routeCollection.MapLocalizedRoute("AffiliateInvoice", "affiliateinvoice",
                new { controller = "AccountingController", action = "AffiliateInvoices" },
                new[] { "Adrack.Web.Controllers" });
            routeCollection.MapLocalizedRoute("BuyerPayment", "buyerpayment",
                new { controller = "AccountingController", action = "BuyerPayments" }, new[] { "Adrack.Web.Controllers" });

            routeCollection.MapLocalizedRoute("BuyerInvoice", "buyerinvoice",
                new { controller = "AccountingController", action = "BuyerInvoices" }, new[] { "Adrack.Web.Controllers" });
            routeCollection.MapLocalizedRoute("BuyerInvoicesPartial", "BuyerInvoicesPartial{id}",
                new { controller = "AccountingController", action = "BuyerInvoicesPartial" },
                new[] { "Adrack.Web.Controllers" });
            routeCollection.MapLocalizedRoute("Invoices", "Invoices",
                new { controller = "AccountingController", action = "Invoices" }, new[] { "Adrack.Web.Controllers" });
            routeCollection.MapLocalizedRoute("AffiliateInvoicesPartial", "AffiliateInvoicesPartial{id}",
                new { controller = "AccountingController", action = "AffiliateInvoicesPartial" },
                new[] { "Adrack.Web.Controllers" });
            routeCollection.MapLocalizedRoute("RefundedLead", "refundedlead",
                new { controller = "AccountingController", action = "RefundedLeads" }, new[] { "Adrack.Web.Controllers" });

            routeCollection.MapLocalizedRoute("BuyerInvoiceItem", "BuyerInvoiceItem{id}",
                new { controller = "AccountingController", action = "BuyerInvoiceItem" },
                new[] { "Adrack.Web.Controllers" });

            routeCollection.MapLocalizedRoute("SupportTickets", "Management/Support/Tickets",
                new { controller = "SupportController", action = "Ticket" }, new[] { "Adrack.Web.Controllers" });
            routeCollection.MapLocalizedRoute("SupportTicketsMessages", "Management/Support/Item",
                new { controller = "SupportController", action = "Item" }, new[] { "Adrack.Web.Controllers" });

            routeCollection.MapRoute("Profile", "profile/{Id}", new { controller = "UserController", action = "Item" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapRoute("GetBuyerFirstUser", "getbuyerfirstuser",
                new { controller = "UserController", action = "GetBuyerFirstUser" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapRoute("AffiliateUser", "affiliate",
                new { controller = "UserController", action = "Affiliate" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapRoute("BuyerUser", "buyer", new { controller = "UserController", action = "Buyer" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapRoute("BuyerView", "BuyerView", new { controller = "Home", action = "BuyerView" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            #endregion Content

            #region Directory

            // Get State Province By Country Id (Ajax)
            routeCollection.MapRoute("GetStateProvinceByCountryId", "Directory/GetStateProvinceByCountryId/",
                new { controller = "Directory", action = "GetStateProvinceByCountryId" },
                new[] { "Adrack.Web.Controllers" });

            #endregion Directory

            #region Message

            routeCollection.MapLocalizedRoute("EmailSubscription", "email-subscription/activation/{token}/{active}",
                new { controller = "Message", action = "Activation" }, new { token = new GuidRouteConstraint(false) },
                new[] { "Adrack.Web.Controllers" });

            #endregion Message

            #region Lead

            routeCollection.MapLocalizedRoute("Affiliates", "affiliate/list",
                new { controller = "Affiliate", action = "List" }, new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("Affiliate", "affiliate/item/{id}",
                new { controller = "Affiliate", action = "Item", id = UrlParameter.Optional },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetAffiliates", "getaffiliates",
                new { controller = "Affiliate", action = "GetAffiliates" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("SetAffiliateStatus", "SetAffiliateStatus",
                new { controller = "Affiliate", action = "SetAffiliateStatus" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("AffiliateHistory", "affiliate/history/{id}",
                new { controller = "Affiliate", action = "History", id = UrlParameter.Optional },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("DeleteAffiliate", "deleteaffiliate",
                new { controller = "Affiliate", action = "DeleteAffiliate" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("SingleAffiliateDashboard", "affiliate/dashboard/{id}",
                new { controller = "Affiliate", action = "Dashboard", id = UrlParameter.Optional },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("SingleAffiliateChannels", "affiliate/channels/{id}",
                new { controller = "Affiliate", action = "Channels", id = UrlParameter.Optional },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("SingleAffiliateUsers", "affiliate/users/{id}",
                new { controller = "Affiliate", action = "Users", id = UrlParameter.Optional },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("SingleAffiliateReports", "affiliate/reports/{id}",
                new { controller = "Affiliate", action = "Reports", id = UrlParameter.Optional },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("SingleAffiliateBanners", "affiliate/banners/{id}",
                new { controller = "Affiliate", action = "Banners" }, new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("Buyers", "buyer/list", new { controller = "Buyer", action = "List" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("Buyer", "buyer/item/{id}",
                new { controller = "Buyer", action = "Item", id = UrlParameter.Optional },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetBuyers", "getbuyers",
                new { controller = "Buyer", action = "GetBuyers" }, new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("BuyerHistory", "buyer/history/{id}",
                new { controller = "Buyer", action = "History", id = UrlParameter.Optional },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("DeleteBuyer", "deletebuyer",
                new { controller = "Buyer", action = "DeleteBuyer" }, new[] { "Adrack.Web.ContentManagement.Controllers" });

            //routeCollection.MapLocalizedRoute("SingleBuyer", "buyer/item/{id}", new { controller = "Buyer", action = "Item", id = UrlParameter.Optional }, new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("SingleBuyerDashboard", "buyer/dashboard/{id}",
                new { controller = "Buyer", action = "Dashboard", id = UrlParameter.Optional },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("SingleBuyerChannels", "buyer/channels/{id}",
                new { controller = "Buyer", action = "Channels", id = UrlParameter.Optional },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("SingleBuyerUsers", "buyer/users/{id}",
                new { controller = "Buyer", action = "Users", id = UrlParameter.Optional },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("SingleBuyerReports", "buyer/reports/{id}",
                new { controller = "Buyer", action = "Reports", id = UrlParameter.Optional },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("PaymentMethods", "paymentmethod/list",
                new { controller = "PaymentMethod", action = "List" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("PaymentMethod", "paymentmethod/item/{id}",
                new { controller = "PaymentMethod", action = "Item" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetPaymentMethods", "getpaymentmethods/{id}",
                new { controller = "PaymentMethod", action = "GetPaymentMethods" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("MemberAffiliate", "Affiliate/Item/{id}",
                new { controller = "MemberAffiliate", action = "Item" }, new[] { "Adrack.Web.Member.Controllers" });
            routeCollection.MapLocalizedRoute("MemberBuyer", "Buyer/Item/{id}",
                new { controller = "MemberBuyer", action = "Item" }, new[] { "Adrack.Web.Member.Controllers" });

            routeCollection.MapLocalizedRoute("Verticals", "vertical/list",
                new { controller = "Vertical", action = "List" }, new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("Vertical", "Verticals/item/{id}",
                new { controller = "Vertical", action = "Item" }, new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetVerticals", "getverticals",
                new { controller = "Vertical", action = "GetVerticals" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("Campaigns", "campaign/list",
                new { controller = "Campaign", action = "List" }, new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("Campaign", "campaign/item/{id}",
                new { controller = "Campaign", action = "Item" }, new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetCampaigns", "getcampaigns",
                new { controller = "Campaign", action = "GetCampaigns" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("CampaignWizard", "campaign/create",
                new { controller = "Campaign", action = "Create" }, new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("DeleteCampaign", "deletecampaign",
                new { controller = "Campaign", action = "DeleteCampaign" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("CampaignTemplates", "campaign/templatelist",
                new { controller = "Campaign", action = "TemplateList" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("CampaignTemplate", "campaign/templateitem/{id}",
                new { controller = "Campaign", action = "TemplateItem" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetCampaignTemplates", "getcampaigntemplates",
                new { controller = "Campaign", action = "GetCampaignTemplates" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetCampaignFields", "getcampaignfields",
                new { controller = "Campaign", action = "GetCampaignFields" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetCampaignInfo", "getcampaigninfo",
                new { controller = "Campaign", action = "GetCampaignInfo" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetCampaignPossibleValues", "getcampaignpossiblevalues",
                new { controller = "Campaign", action = "GetCampaignPossibleValues" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("LoadFromXml", "loadfromxml",
                new { controller = "Campaign", action = "LoadFromXml" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("LoadCampaignTemplate", "loadcampaigntemplate",
                new { controller = "Campaign", action = "LoadCampaignTemplate" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("LoadCampaignTemplateList", "loadcampaigntemplateList",
                new { controller = "Campaign", action = "LoadCampaignTemplateList" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapRoute("GetCampaignsByVerticalId", "GetCampaignsByVerticalId",
                new { controller = "Campaign", action = "GetCampaignsByVerticalId" }, new[] { "Adrack.Web.Controllers" });
            routeCollection.MapRoute("GetCampaignTemplatesByVerticalId", "GetCampaignTemplatesByVerticalId",
                new { controller = "Campaign", action = "GetCampaignTemplatesByVerticalId" },
                new[] { "Adrack.Web.Controllers" });

            routeCollection.MapLocalizedRoute("AffiliateChannels", "affiliatechannel/list",
                new { controller = "AffiliateChannel", action = "List" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("AffiliateChannel", "affiliatechannel/item/{id}",
                new { controller = "AffiliateChannel", action = "Item" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetAffiliateChannels", "getaffiliatechannels",
                new { controller = "AffiliateChannel", action = "GetAffiliateChannels" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetAffiliateResponses", "getaffiliateresponses",
                new { controller = "AffiliateChannel", action = "GetAffiliateResponses" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("LoadFromAffiliateChannelXml", "loadfromachannelxml",
                new { controller = "AffiliateChannel", action = "LoadFromXml" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("LoadFromCampaignXml", "loadfromcampaignxml",
                new { controller = "AffiliateChannel", action = "LoadFromCampaignXml" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("LoadAffiliateChannelTemplate", "loadaffiliatechanneltemplate",
                new { controller = "AffiliateChannel", action = "LoadAffiliateChannelTemplate" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetAffiliateChannelXml", "GetAffiliateChannelXml",
                new { controller = "AffiliateChannel", action = "GetAffiliateChannelXml" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetMemberAffiliateChannels", "getmemberaffiliatechannels",
                new { controller = "MemberAffiliateChannel", action = "GetMemberAffiliateChannels" },
                new[] { "Adrack.Web.Member.Controllers" });
            routeCollection.MapLocalizedRoute("AffiliateChannelWizard", "affiliatechannel/create",
                new { controller = "AffiliateChannel", action = "Create" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("DeleteAffiliateChannel", "deleteaffiliatechannel",
                new { controller = "AffiliateChannel", action = "DeleteAffiliateChannel" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("BuyerChannels", "buyerchannel/list",
                new { controller = "BuyerChannel", action = "List" }, new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("BuyerChannel", "buyerchannel/item/{id}",
                new { controller = "BuyerChannel", action = "Item" }, new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetBuyerChannels", "getbuyerchannels",
                new { controller = "BuyerChannel", action = "GetBuyerChannels" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetPostedData", "getposteddata",
                new { controller = "BuyerChannel", action = "GetPostedData" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetAllowedFrom", "getallowedfrom",
                new { controller = "BuyerChannel", action = "GetAllowedFrom" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetBuyerChannelsbyCampaign", "getbuyerchannelsbycampaign",
                new { controller = "BuyerChannel", action = "GetBuyerChannelsByCampaign" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("UpdateAllowed", "updateallowed",
                new { controller = "BuyerChannel", action = "UpdateAllowed" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetBuyerChannelTemplateMatchings", "getbuyerchanneltemplatematchings",
                new { controller = "BuyerChannel", action = "GetBuyerChannelTemplateMatchings" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("AddBuyerChannelTemplateMatching", "addbuyerchanneltemplatematching",
                new { controller = "BuyerChannel", action = "AddBuyerChannelTemplateMatching" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("UpdateBuyerChannelTemplateMatching",
                "updatebuyerchanneltemplatematching",
                new { controller = "BuyerChannel", action = "UpdateBuyerChannelTemplateMatching" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("DeleteBuyerChannelTemplateMatching",
                "deletebuyerchanneltemplatematching",
                new { controller = "BuyerChannel", action = "DeleteBuyerChannelTemplateMatching" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetBuyerChannelInfo", "getbuyerchannelinfo",
                new { controller = "BuyerChannel", action = "GetBuyerChannelInfo" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("CloneBuyerChannel", "clone",
                new { controller = "BuyerChannel", action = "Clone" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("DeleteBuyerChannel", "deletebuyerchannel",
                new { controller = "BuyerChannel", action = "DeleteBuyerChannel" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("Offers", "offer/list", new { controller = "Offer", action = "Offers" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("Offer", "offer/item/{id}", new { controller = "Offer", action = "Item" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("OfferWizard", "offer/create",
                new { controller = "Offer", action = "Create" }, new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("Banner", "buyerchannel/banner",
                new { controller = "BuyerChannel", action = "Banner" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("LoadBuyerChannelTemplate", "loadbuyerchanneltemplate",
                new { controller = "BuyerChannel", action = "LoadBuyerChannelTemplate" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("LoadFromBuyerChannelXml", "loadfrombchannelxml",
                new { controller = "BuyerChannel", action = "LoadFromXml" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("LoadFromCampaignXml2", "loadfromcampaignxml2",
                new { controller = "BuyerChannel", action = "LoadFromCampaignXml" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("SetBuyerChannelOrder", "setbuyerchannelorder",
                new { controller = "BuyerChannel", action = "SetBuyerChannelOrder" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetMemberBuyerChannels", "getmemberbuyerchannels",
                new { controller = "MemberBuyerChannel", action = "GetMemberBuyerChannels" },
                new[] { "Adrack.Web.Member.Controllers" });

            routeCollection.MapLocalizedRoute("DepartmentItem", "Department/Item/{id}",
                new { controller = "Department", action = "Item" }, new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("DepartmentList", "Department/List",
                new { controller = "Department", action = "List" }, new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetDepartments", "getdepartments",
                new { controller = "Department", action = "GetDepartments" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("SupportItem", "Support/Item/{id}",
                new { controller = "Support", action = "Item" }, new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("SettingEMailTemplates", "Management/Setting/EMailTemplates",
                new { controller = "SupportController", action = "EMailTemplates" }, new[] { "Adrack.Web.Controllers" });
            routeCollection.MapLocalizedRoute("SettingMailTemplateItem", "Management/Setting/EMailTemplateItem/{Id}",
                new { controller = "SettingController", action = "EMailTemplateItem" }, new[] { "Adrack.Web.Controllers" });

            routeCollection.MapLocalizedRoute("TimeZone", "Management/Setting/TimeZone",
                new { controller = "SettingController", action = "TimeZone" }, new[] { "Adrack.Web.Controllers" });

            routeCollection.MapLocalizedRoute("ReportBuyers", "report/ReportBuyers",
                new { controller = "ReportController", action = "ReportBuyers" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("ReportBuyersComparison", "report/ReportBuyersComparison",
                new { controller = "ReportController", action = "ReportBuyersComparison" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("ReportAffiliates", "report/ReportAffiliates",
                new { controller = "ReportController", action = "ReportAffiliates" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("BuyerReportByBuyerChannel", "report/BuyerReportByBuyerChannel",
                new { controller = "ReportController", action = "BuyerReportByBuyerChannel" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetBuyerReportByBuyerChannel", "report/GetBuyerReportByBuyerChannel",
                new { controller = "ReportController", action = "GetBuyerReportByBuyerChannel" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("ReportBuyersByCampaigns", "report/ReportBuyersByCampaigns",
                new { controller = "ReportController", action = "ReportBuyersByCampaigns" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetReportBuyersByCampaigns", "report/GetReportBuyersByCampaigns",
                new { controller = "ReportController", action = "GetReportBuyersByCampaigns" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("ReportBuyersByAffiliateChannels",
                "report/ReportBuyersByAffiliateChannels",
                new { controller = "ReportController", action = "ReportBuyersByCampaigns" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetReportBuyersByAffiliateChannels",
                "report/GetReportBuyersByAffiliateChannels",
                new { controller = "ReportController", action = "GetReportBuyersByCampaigns" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("ReportBuyersByStates", "report/ReportBuyersByStates",
                new { controller = "ReportController", action = "ReportBuyersByStates" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetReportBuyersByStates", "report/GetReportBuyersByStates",
                new { controller = "ReportController", action = "GetReportBuyersByStates" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("ReportBuyersByDates", "report/ReportBuyersByDates",
                new { controller = "ReportController", action = "ReportBuyersByDates" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetReportBuyersByDates", "report/GetReportBuyersByDates",
                new { controller = "ReportController", action = "GetReportBuyersByDates" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("ReportAffiliatesByCampaigns", "report/ReportAffiliatesByCampaigns",
                new { controller = "ReportController", action = "ReportAffiliatesByCampaigns" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetReportAffiliatesByCampaigns", "report/GetReportAffiliatesByCampaigns",
                new { controller = "ReportController", action = "GetReportAffiliatesByCampaigns" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("ReportAfiliatesByAffiliateChannels",
                "report/ReportAfiliatesByAffiliateChannels",
                new { controller = "ReportController", action = "ReportAfiliatesByAffiliateChannels" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetReportAfiliatesByAffiliateChannels",
                "report/GetReportAfiliatesByAffiliateChannels",
                new { controller = "ReportController", action = "GetReportAfiliatesByAffiliateChannels" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("ReportAffiliatesByStates", "report/ReportAffiliatesByStates",
                new { controller = "ReportController", action = "ReportAffiliatesByStates" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetReportAffiliatesByStates", "report/GetReportAffiliatesByStates",
                new { controller = "ReportController", action = "GetReportAffiliatesByStates" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("ReportByDays", "report/ReportByDays",
                new { controller = "ReportController", action = "ReportByDays" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetReportByDays", "report/GetReportByDays",
                new { controller = "ReportController", action = "GetReportByDays" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetReportByMinutes", "report/GetReportByMinutes",
                new { controller = "ReportController", action = "GetReportByMinutes" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("ReportTotals", "report/ReportTotals",
                new { controller = "ReportController", action = "ReportTotals" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetReportTotals", "report/GetReportTotals",
                new { controller = "ReportController", action = "GetReportTotals" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetReportTotalsBuyer", "report/GetReportTotalsBuyer",
                new { controller = "ReportController", action = "GetReportTotalsBuyer" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("GetReportLeadCalls", "report/GetReportTotalsBuyer",
                new { controller = "ReportController", action = "GetReportTotalsBuyer" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("Lead", "Lead/Item/{id}",
                new { controller = "LeadController", action = "Item" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("GenerateExcelFileAjax", "Lead/GenerateExcelFileAjax ",
                new { controller = "LeadController", action = "GenerateExcelFileAjax " },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("Filters", "filter/list", new { controller = "Filter", action = "List" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("Filter", "filter/item/{id}",
                new { controller = "Filter", action = "Item" }, new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetFilter", "getfilters",
                new { controller = "Filter", action = "GetFilters" }, new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetFilterConditions", "GetFilterConditions",
                new { controller = "Filter", action = "GetFilterConditions" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetFiltersByCampaignId", "GetFiltersByCampaignId",
                new { controller = "Filter", action = "GetFiltersByCampaignId" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("DeleteFilterSet", "deletefilterset",
                new { controller = "Filter", action = "DeleteFilterSet" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("Blacklists", "blacklist/list",
                new { controller = "BlackList", action = "List" }, new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("Blacklist", "blacklist/item/{id}",
                new { controller = "BlackList", action = "Item" }, new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetBlackListTypes", "GetBlackListTypes",
                new { controller = "BlackList", action = "GetBlackListTypes" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetBlackListValues", "GetBlackListValues",
                new { controller = "BlackList", action = "GetBlackListValues" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetCustomBlackListValues", "GetCustomBlackListValues",
                new { controller = "BlackList", action = "GetCustomBlackListValues" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("addblacklistvalue", "addblacklistvalue",
                new { controller = "BlackList", action = "addblacklistvalue" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("removeblacklistvalue", "removeblacklistvalue",
                new { controller = "BlackList", action = "removeblacklistvalue" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("addcustomblacklistvalue", "addcustomblacklistvalue",
                new { controller = "BlackList", action = "addcustomblacklistvalue" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("removecustomblacklistvalue", "removecustomblacklistvalue",
                new { controller = "BlackList", action = "removecustomblacklistvalue" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("ZipCodeRedirects", "zipcoderedirect/list/{buyerChannelId}",
                new { controller = "ZipCodeRedirect", action = "List" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("ZipCodeRedirect", "zipcoderedirect/item/{id}/{buyerChannelId}",
                new { controller = "ZipCodeRedirect", action = "Item", buyerChannelId = UrlParameter.Optional },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetZipCodeRedirects", "getzipcoderedirects",
                new { controller = "ZipCodeRedirect", action = "GetZipCodeRedirects" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("removezipcoderedirect", "removezipcoderedirect",
                new { controller = "ZipCodeRedirect", action = "RemoveZipCodeRedirect" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            routeCollection.MapLocalizedRoute("GetValidatorType", "GetValidatorType/{type}",
                new { controller = "ValidatorType", action = "GetValidatorType" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            #endregion Lead

            #region Security

            routeCollection.MapLocalizedRoute("RoleItem", "Role/Item/{id}", new { controller = "Role", action = "Item" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("RoleList", "Role/List", new { controller = "Role", action = "List" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });
            routeCollection.MapLocalizedRoute("GetRoles", "getroles", new { controller = "Role", action = "GetRoles" },
                new[] { "Adrack.Web.ContentManagement.Controllers" });

            #endregion Security
        }

        #endregion Methods

        #region Properties

        /// <summary>
        ///     Priority
        /// </summary>
        /// <value>The priority.</value>
        public int Priority => 0;

        #endregion Properties
    }
}