// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="TestingModules.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure;
using Adrack.Managers;
using Adrack.Service.Accounting;
using Adrack.Service.Configuration;
using Adrack.Service.Lead;
using Adrack.Web.Framework.ViewEngines.Razor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using RequestContext = Adrack.Managers.RequestContext;

namespace Adrack.Web.Managers
{
    /// <summary>
    ///     Class FakeHttpContext.
    /// </summary>
    public static class FakeHttpContext
    {
        /// <summary>
        ///     Gets the fake request.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>HttpRequestBase.</returns>
        public static HttpRequestBase GetFakeRequest()
        {
            HttpRequestBase request = new HttpRequestWrapper(new HttpRequest("test", "http://localhost", ""));
            //request.InputStream.Write(bytes, 0, bytes.Length);
            return request;
        }

        /// <summary>
        ///     Gets the fake response.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>HttpResponseBase.</returns>
        public static HttpResponseBase GetFakeResponse(string data = "")
        {
            var stream = new MemoryStream();
            var csvWriter = new StreamWriter(stream, Encoding.UTF8);
            csvWriter.Write(data);
            var response = new HttpResponse(csvWriter);
            return new HttpResponseWrapper(response);
        }

        /// <summary>
        ///     Sets the fake context.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="data">The data.</param>
        public static void SetFakeContext(this Controller controller, string queryString = "", string data = "")
        {
            var request = new HttpRequest("test", "http://localhost", queryString);
            var stream = new MemoryStream();
            var csvWriter = new StreamWriter(stream, Encoding.UTF8);
            csvWriter.Write(data);
            var responce = new HttpResponse(csvWriter);
            var context = new HttpContext(request, responce);
            HttpContext.Current = context;

            var wrapper = new HttpContextWrapper(HttpContext.Current);

            var contextController =
                new ControllerContext(
                    new System.Web.Routing.RequestContext(wrapper,
                        new RouteData()), controller);

            controller.ControllerContext = contextController;
        }
    }

    /// <summary>
    ///     Class TestingManager.
    /// </summary>
    public class TestingManager
    {
        /// <summary>
        ///     The validators
        /// </summary>
        private readonly List<string> validators = new List<string>();

        /// <summary>
        ///     Gets or sets the buyer channel.
        /// </summary>
        /// <value>The buyer channel.</value>
        private BuyerChannel buyerChannel { get; set; }

        /// <summary>
        ///     Gets or sets the affiliate channel.
        /// </summary>
        /// <value>The affiliate channel.</value>
        private AffiliateChannel affiliateChannel { get; set; }

        /// <summary>
        ///     Generates the data.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        public void GenerateData(RequestContext requestContext)
        {
            validators.Clear();
            validators.Add("NONE");
            validators.Add("String");
            validators.Add("Number");
            validators.Add("EMail");
            validators.Add("UnsignedNumber");
            validators.Add("AccountNumber");
            validators.Add("SSN");
            validators.Add("ZIP");
            validators.Add("Phone");
            validators.Add("Password");
            validators.Add("DateTime");
            validators.Add("State");
            validators.Add("RoutingNumber");
            validators.Add("AffiliateRefferalField");
            validators.Add("DateOfBirth");
            validators.Add("RegularExpression");
            validators.Add("Decimal");

            var campaign = requestContext.CampaignService.GetCampaignByName("[test campaign]", 0);
            if (campaign == null)
            {
                campaign = new Campaign
                {
                    Name = "[test campaign]",
                    IsTemplate = false,
                    CreatedOn = DateTime.UtcNow,
                    CampaignType = 0,
                    Start = DateTime.UtcNow,
                    Finish = DateTime.UtcNow,
                    Status = Core.ActivityStatuses.Active,
                    VerticalId = 1
                };
                requestContext.CampaignService.InsertCampaign(campaign);
            }

            var buyer = requestContext.BuyerService.GetBuyerByName("[test buyer]", 0);
            if (buyer == null)
            {
                buyer = new Buyer
                {
                    Name = "[test buyer]",
                    CreatedOn = DateTime.UtcNow,
                    CountryId = 80,
                    StateProvinceId = 3,
                    Status = 1
                };
                requestContext.BuyerService.InsertBuyer(buyer);
            }

            var affiliate = requestContext.AffiliateService.GetAffiliateByName("[test affiliate]", 0);
            if (affiliate == null)
            {
                affiliate = new Affiliate
                {
                    Name = "[test affiliate]",
                    CreatedOn = DateTime.UtcNow,
                    CountryId = 80,
                    StateProvinceId = 3,
                    Status = 1
                };
                requestContext.AffiliateService.InsertAffiliate(affiliate);
            }

            buyerChannel = requestContext.BuyerChannelService.GetBuyerChannelByName("[test buyer channel]", 0);
            if (buyerChannel == null)
            {
                buyerChannel = new BuyerChannel
                {
                    Name = "[test buyer channel]",
                    BuyerId = buyer.Id,
                    CampaignId = campaign.Id,
                    Status = BuyerChannelStatuses.Active
                };
                requestContext.BuyerChannelService.InsertBuyerChannel(buyerChannel);
            }

            affiliateChannel =
                requestContext.AffiliateChannelService.GetAffiliateChannelByName("[test affiliate channel]", 0);
            if (affiliateChannel == null)
            {
                affiliateChannel = new AffiliateChannel
                {
                    Name = "[test affiliate channel]",
                    AffiliateId = affiliate.Id,
                    CampaignId = campaign.Id,
                    ChannelKey = GenerateString(7),
                    Status = 1
                };
                requestContext.AffiliateChannelService.InsertAffiliateChannel(affiliateChannel);
            }

            buyerChannel.AlwaysSoldOption = 0;
            buyerChannel.BuyerId = buyer.Id;
            buyerChannel.PostingUrl = "http://adracktest.azurewebsites.net/home/testresp";
            buyerChannel.AcceptedField = "status";
            buyerChannel.AcceptedValue = "sold";
            buyerChannel.AcceptedFrom = 0;
            buyerChannel.ErrorField = "status";
            buyerChannel.ErrorValue = "error";
            buyerChannel.ErrorFrom = 0;
            buyerChannel.RejectedField = "status";
            buyerChannel.RejectedValue = "reject";
            buyerChannel.RejectedFrom = 0;
            buyerChannel.TestField = "status";
            buyerChannel.TestValue = "sold";
            buyerChannel.TestFrom = 0;

            affiliateChannel.AffiliateId = affiliate.Id;

            requestContext.BuyerChannelService.UpdateBuyerChannel(buyerChannel);
            requestContext.AffiliateChannelService.UpdateAffiliateChannel(affiliateChannel);

            requestContext.BuyerChannelTemplateService.DeleteBuyerChannelTemplatesByBuyerChannelId(buyerChannel.Id);
            requestContext.AffiliateChannelTemplateService.DeleteAffiliateChannelTemplatesByAffiliateChannelId(
                affiliateChannel.Id);
            requestContext.CampaignService.DeleteCampaignTemplates(campaign.Id);

            var rnd = new Random();

            requestContext.Manager.Import.CustomData = "<REQUEST><CHANNELID>" + affiliateChannel.ChannelKey +
                                                       "</CHANNELID><String></String><Number></Number><EMail></EMail><UnsignedNumber></UnsignedNumber><AccountNumber></AccountNumber><SSN></SSN><ZIP></ZIP><Phone></Phone><Password></Password><DateTime></DateTime><State></State><RoutingNumber></RoutingNumber><channelid></channelid><DateOfBirth></DateOfBirth><RegularExpression></RegularExpression><Decimal></Decimal></REQUEST>";
            campaign.DataTemplate = requestContext.Manager.Import.CustomData;

            var xmldoc = new XmlDocument();
            xmldoc.LoadXml(requestContext.Manager.Import.CustomData);

            XmlNode root = xmldoc.DocumentElement;

            foreach (XmlNode node in root.ChildNodes)
                switch (node.Name.ToLower())
                {
                    case "string":
                        node.InnerText = GenerateString(8);
                        break;

                    case "number":
                        node.InnerText = rnd.Next(1, 100).ToString();
                        break;

                    case "email":
                        node.InnerText = GenerateString(4) + "@" + GenerateString(4) + ".com";
                        break;

                    case "unsignednumber":
                        node.InnerText = rnd.Next(1, 100).ToString();
                        break;

                    case "accountnumber":
                        node.InnerText = GenerateString(9);
                        break;

                    case "ssn":
                        node.InnerText = "123456789";
                        break;

                    case "zip":
                        node.InnerText = "12345";
                        break;

                    case "phone":
                        node.InnerText = GenerateString(9);
                        break;

                    case "datetime":
                        node.InnerText = DateTime.Now.ToString("MM/dd/yyyy");
                        break;

                    case "state":
                        node.InnerText = GenerateString(2);
                        break;

                    case "decimal":
                        node.InnerText = rnd.NextDouble().ToString();
                        break;
                }

            requestContext.Manager.Import.CustomData = xmldoc.OuterXml;

            for (var i = 1; i <= 16; i++)
            {
                var cfield = new CampaignField
                {
                    CampaignId = campaign.Id,
                    MinLength = 0,
                    MaxLength = 0,
                    Required = true,
                    TemplateField = validators[i],
                    Validator = (short)i,
                    SectionName = "REQUEST",
                    DatabaseField = "NONE"
                };

                switch (i)
                {
                    case 1:
                        cfield.DatabaseField = "Firstname";
                        break;
                }

                requestContext.CampaignTemplateService.InsertCampaignTemplate(cfield);

                var bfield = new BuyerChannelTemplate
                {
                    BuyerChannelId = buyerChannel.Id,
                    CampaignTemplateId = cfield.Id,
                    SectionName = "REQUEST",
                    TemplateField = validators[i]
                };
                requestContext.BuyerChannelTemplateService.InsertBuyerChannelTemplate(bfield);

                var afield = new AffiliateChannelTemplate
                {
                    AffiliateChannelId = affiliateChannel.Id,
                    CampaignTemplateId = cfield.Id,
                    SectionName = "REQUEST",
                    TemplateField = validators[i]
                };
                requestContext.AffiliateChannelTemplateService.InsertAffiliateChannelTemplate(afield);

                var condition = rnd.Next(1, 12);

                var bcondition = new BuyerChannelFilterCondition
                {
                    BuyerChannelId = buyerChannel.Id,
                    CampaignTemplateId = cfield.Id,
                    Condition = (short)condition
                };

                switch (condition)
                {
                    case 1:
                        bcondition.Value = "";
                        break;
                }
            }
        }

        /// <summary>
        ///     Generates the string.
        /// </summary>
        /// <param name="len">The length.</param>
        /// <returns>System.String.</returns>
        public string GenerateString(int len)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[len];
            var random = new Random();

            for (var i = 0; i < stringChars.Length; i++) stringChars[i] = chars[random.Next(chars.Length)];

            var finalString = new string(stringChars);
            return finalString;
        }

        /// <summary>
        ///     Runs this instance.
        /// </summary>
        public void Run()
        {
            MvcHandler.DisableMvcResponseHeader = true;

            // Initialize Engine Context
            //AppEngineContext.Initialize(false);

            // Remove All View Engines
            ViewEngines.Engines.Clear();

            // Add Web Application Razor View Engine
            ViewEngines.Engines.Add(new WebAppRazorViewEngine());

            // Add Functionality On Top Of The Default Model Metadata Provider

            // Registering Rebular MVC
            //AreaRegistration.RegisterAllAreas();

            // Fluent Validation
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            var requestContext = new RequestContext
            {
                AffiliateChannelFilterConditionService =
                    AppEngineContext.Current.Resolve<IAffiliateChannelFilterConditionService>(),
                AffiliateService = AppEngineContext.Current.Resolve<IAffiliateService>(),
                AffiliateChannelService = AppEngineContext.Current.Resolve<IAffiliateChannelService>(),
                AffiliateChannelTemplateService = AppEngineContext.Current.Resolve<IAffiliateChannelTemplateService>(),
                AffiliateResponseService = AppEngineContext.Current.Resolve<IAffiliateResponseService>(),
                BlackListService = AppEngineContext.Current.Resolve<IBlackListService>(),
                BuyerChannelFilterConditionService =
                    AppEngineContext.Current.Resolve<IBuyerChannelFilterConditionService>(),
                BuyerChannelService = AppEngineContext.Current.Resolve<IBuyerChannelService>(),
                BuyerChannelTemplateService = AppEngineContext.Current.Resolve<IBuyerChannelTemplateService>(),
                BuyerResponseService = AppEngineContext.Current.Resolve<IBuyerResponseService>(),
                BuyerService = AppEngineContext.Current.Resolve<IBuyerService>(),
                CampaignService = AppEngineContext.Current.Resolve<ICampaignService>(),
                CampaignTemplateService = AppEngineContext.Current.Resolve<ICampaignTemplateService>(),
                LeadContentDublicateService = AppEngineContext.Current.Resolve<ILeadContentDublicateService>(),
                LeadContentService = AppEngineContext.Current.Resolve<ILeadContentService>(),
                LeadMainResponseService = AppEngineContext.Current.Resolve<ILeadMainResponseService>(),
                LeadMainService = AppEngineContext.Current.Resolve<ILeadMainService>(),
                BuyerChannelScheduleService = AppEngineContext.Current.Resolve<IBuyerChannelScheduleService>(),
                ProcessingLogService = AppEngineContext.Current.Resolve<IProcessingLogService>(),
                RedirectUrlService = AppEngineContext.Current.Resolve<IRedirectUrlService>(),
                SettingService = AppEngineContext.Current.Resolve<ISettingService>(),
                ZipCodeRedirectService = AppEngineContext.Current.Resolve<IZipCodeRedirectService>(),
                AccountingService = AppEngineContext.Current.Resolve<IAccountingService>(),
                BuyerChannelTemplateMatchingService =
                    AppEngineContext.Current.Resolve<IBuyerChannelTemplateMatchingService>(),

                Manager = new RequestManager(),

                HttpRequest = FakeHttpContext.GetFakeRequest(),
                HttpResponse = FakeHttpContext.GetFakeResponse()
            };

            GenerateData(requestContext);

            try
            {
                requestContext.Manager.ProcessData(requestContext.HttpRequest, requestContext.HttpResponse,
                    requestContext, true);
            }
            catch (Exception ex)
            {
                var frames = new StackTrace(ex, true).GetFrames();

                var i = 0;
                var LineNumber = 0;
                string FileName = null;
                while (i < frames.Length && string.IsNullOrEmpty(FileName))
                {
                    LineNumber = frames[i].GetFileLineNumber();
                    FileName = frames[i].GetFileName();
                    i++;
                }
            }
        }
    }
}