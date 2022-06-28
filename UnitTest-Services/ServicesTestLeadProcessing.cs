using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Xml;
using Adrack;
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure;
using Adrack.Managers;
using Adrack.Service.Configuration;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Web.Framework.ViewEngines.Razor;

namespace UnitTest_Services
{
    public class ServicesTestLeadProcessing
    {
        private readonly ExportManager exportManager;
        private readonly ImportManager importManager;
        private readonly List<string> validators = new List<string>();
        private TestSettings Settings { get; }
        public RequestContext requestContext = null;

        /// <summary>
        /// </summary>
        public ServicesTestLeadProcessing(UnitTestLeadProcessing parent)
        {
            Parent = parent;

            // Disable IIS Information Request
            MvcHandler.DisableMvcResponseHeader = true;

            // Initialize Engine Context
            AppEngineContext.Initialize(false);

            // Remove All View Engines
            ViewEngines.Engines.Clear();

            // Add Web Application Razor View Engine
            ViewEngines.Engines.Add(new WebAppRazorViewEngine());

            // Add Functionality On Top Of The Default Model Metadata Provider

            // Registering Rebular MVC
            //AreaRegistration.RegisterAllAreas();

            // Fluent Validation
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            importManager = new ImportManager();
            exportManager = new ExportManager();

            Settings = new TestSettings();
            Settings.Load("ServicesTestLeadProcessing");

            requestContext = new RequestContext
            {
                AffiliateChannelFilterConditionService =
                    AppEngineContext.Current.Resolve<IAffiliateChannelFilterConditionService>(),
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
                DoNotPresentService = AppEngineContext.Current.Resolve<IDoNotPresentService>(),

                Manager = new RequestManager()
            };
        }

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

        protected UnitTestLeadProcessing Parent { get; set; }

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
                    Status = Adrack.Core.ActivityStatuses.Active,
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
                    Validator = (short) i,
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
                    Condition = (short) condition
                };

                switch (condition)
                {
                    case 1:
                        bcondition.Value = "";
                        break;
                }
            }
        }

        public void ProcessData()
        {
            MvcHandler.DisableMvcResponseHeader = true;

            // Initialize Engine Context
            AppEngineContext.Initialize(false);

            // Remove All View Engines
            ViewEngines.Engines.Clear();

            // Add Web Application Razor View Engine
            ViewEngines.Engines.Add(new WebAppRazorViewEngine());

            // Add Functionality On Top Of The Default Model Metadata Provider

            // Registering Rebular MVC
            //AreaRegistration.RegisterAllAreas();

            // Fluent Validation
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            requestContext.Manager.Import.CustomData =
                Parent.TestSettings.SettingsXmlDocument
                    .OuterXml; //"<REQUEST><REFERRAL><CHANNELID>9e366e9</CHANNELID><PASSWORD>6bdfde95</PASSWORD><AFFSUBID>797</AFFSUBID><AFFSUBID2>980890</AFFSUBID2><REFERRINGURL>www.leadz.aa</REFERRINGURL><MINPRICE>0</MINPRICE></REFERRAL><CUSTOMER><PERSONAL><IPADDRESS>123.123.123.123</IPADDRESS><REQUESTEDAMOUNT>100</REQUESTEDAMOUNT><SSN>9B979436A911EE800D828AE2686F17742DE438C8B358BAD4B0DE5384F16C06D9</SSN><DOB>07-07-1966</DOB><FIRSTNAME>Pogos</FIRSTNAME><LASTNAME>Pogosyn</LASTNAME><ADDRESS>4F5C786731F124E59EF9CDCE787D2FC28D9FE42DB069C8CBE5DDC5AC31F0C701</ADDRESS><CITY>Yerevan</CITY><STATE>CA</STATE><ZIP>64850</ZIP><HOMEPHONE>2484564561</HOMEPHONE><CELLPHONE>2484564564</CELLPHONE><DLSTATE>CA</DLSTATE><DLNUMBER>3E708083D55D959D3A1A2DA2D97D225ABC71115C7BFA255AAFCB2C5F74897D36</DLNUMBER><ARMEDFORCES>no</ARMEDFORCES><CONTACTTIME>anytime</CONTACTTIME><RENTOROWN>rent</RENTOROWN><EMAIL>a@gmail.com</EMAIL><ADDRESSMONTH>24</ADDRESSMONTH><CITIZENSHIP>yes</CITIZENSHIP></PERSONAL><EMPLOYMENT><INCOMETYPE>job income</INCOMETYPE><EMPTIME>45</EMPTIME><EMPNAME>Google inc.</EMPNAME><EMPPHONE>2484441111</EMPPHONE><JOBTITLE>CEO</JOBTITLE><PAYFREQUENCY>weekly</PAYFREQUENCY><NEXTPAYDATE>11-11-2016</NEXTPAYDATE><SECONDPAYDATE>10-12-2018</SECONDPAYDATE></EMPLOYMENT><BANK><BANKNAME>Bank of America</BANKNAME><BANKPHONE>2486796849</BANKPHONE><ACCOUNTTYPE>Checking Account </ACCOUNTTYPE><ROUTINGNUMBER>104000016</ROUTINGNUMBER><ACCOUNTNUMBER>D2DC85FED2104598A780A20F44DFF7573EE5A67C3BB1D38FD1FFA2C8B66F4AC4</ACCOUNTNUMBER><BANKMONTHS>78</BANKMONTHS><NETMONTHLYINCOME>10000</NETMONTHLYINCOME><DIRECTDEPOSIT>yes</DIRECTDEPOSIT></BANK></CUSTOMER></REQUEST>";

            requestContext.HttpRequest = FakeHttpContext.GetFakeRequest();
            requestContext.HttpResponse = FakeHttpContext.GetFakeResponse();

            requestContext.Manager.ProcessData(requestContext.HttpRequest, requestContext.HttpResponse, requestContext, false);
        }

        public void ExportProcessData()
        {
            MvcHandler.DisableMvcResponseHeader = true;

            // Initialize Engine Context
            AppEngineContext.Initialize(false);

            // Remove All View Engines
            ViewEngines.Engines.Clear();

            // Add Web Application Razor View Engine
            ViewEngines.Engines.Add(new WebAppRazorViewEngine());

            // Add Functionality On Top Of The Default Model Metadata Provider

            // Registering Rebular MVC
            //AreaRegistration.RegisterAllAreas();

            // Fluent Validation
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            importManager.CustomData = "<request></request>";

            requestContext.HttpRequest = FakeHttpContext.GetFakeRequest();
            requestContext.HttpResponse = FakeHttpContext.GetFakeResponse();

            var leadContent = new LeadContent
            {
                Created = DateTime.Now
            };

            var leadMain = new LeadMain
            {
                Created = DateTime.Now
            };

            requestContext.Extra["leadContent"] = leadContent;

            exportManager.Process(requestContext, leadMain);
        }

        public void ValidateBuyerChannelSchedule()
        {
            BuyerChannel buyerChannel = requestContext.BuyerChannelService.GetBuyerChannelById(long.Parse(Settings.GetValue("ValidateBuyerChannelSchedule", "BuyerChannelId")));

            DateTime created = DateTime.ParseExact(Settings.GetValue("ValidateBuyerChannelSchedule", "created"), Settings.GetValue("ValidateBuyerChannelSchedule", "dateTimeFormat"), CultureInfo.InvariantCulture);

            created = DateTime.UtcNow;
            
            if (exportManager.ValidateSchedule(requestContext, requestContext.CampaignService.GetCampaignById(buyerChannel.CampaignId), new LeadMain() { Created = created }, buyerChannel, requestContext.BuyerService.GetBuyerById(buyerChannel.BuyerId)) == -1)
                throw new Exception("ValidateSchedule failed");
        }
        public void XmlToQueryString()
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml("<REQUEST><REFERRAL><CHANNELID>a43a7d2</CHANNELID><PASSWORD>248066a1</PASSWORD><AFFSUBID>1</AFFSUBID><AFFSUBID2>2</AFFSUBID2><REFERRINGURL>http://www.911paydayadvance.com</REFERRINGURL><MINPRICE>2</MINPRICE></REFERRAL><CUSTOMER><PERSONAL><IPADDRESS>107.135.93.244</IPADDRESS><REQUESTEDAMOUNT>500</REQUESTEDAMOUNT><SSN>3CF3FC36548DABEF867A3CF2954402D3B2A999B4FDB6A7B5DB140EC8658A1EF6</SSN><DOB>03-25-1975</DOB><FIRSTNAME>l</FIRSTNAME><LASTNAME>A</LASTNAME><ADDRESS>1307 S 4th Str</ADDRESS><CITY>Bonham</CITY><STATE>NY</STATE><ZIP>75418</ZIP><HOMEPHONE>9035837473</HOMEPHONE><CELLPHONE></CELLPHONE><DLSTATE>TX</DLSTATE><DLNUMBER>3FA3FEEA979931D1011B21F5EA13BDC55FD2F2D38896FCE97F539ECD38524E3C</DLNUMBER><ARMEDFORCES>No</ARMEDFORCES><CONTACTTIME>Anytime</CONTACTTIME><RENTOROWN>Own</RENTOROWN><EMAIL>haikavn@yahoo.com</EMAIL><ADDRESSMONTH>67</ADDRESSMONTH><CITIZENSHIP>US</CITIZENSHIP></PERSONAL><EMPLOYMENT><INCOMETYPE>Job Income</INCOMETYPE><EMPTIME>77</EMPTIME><EMPNAME>Outreach Home Health</EMPNAME><EMPPHONE>9035837473</EMPPHONE><JOBTITLE>Sales</JOBTITLE><PAYFREQUENCY>Every 2 weeks</PAYFREQUENCY><NEXTPAYDATE>03-15-2019</NEXTPAYDATE><SECONDPAYDATE>03-29-2019</SECONDPAYDATE></EMPLOYMENT><BANK><BANKNAME>American Bank Of Texas</BANKNAME><BANKPHONE>5559995555</BANKPHONE><ACCOUNTTYPE>Checking Account</ACCOUNTTYPE><ROUTINGNUMBER>111901645</ROUTINGNUMBER><ACCOUNTNUMBER>1234567</ACCOUNTNUMBER><BANKMONTHS>11</BANKMONTHS><NETMONTHLYINCOME>1000</NETMONTHLYINCOME><DIRECTDEPOSIT>Yes</DIRECTDEPOSIT></BANK></CUSTOMER></REQUEST>");
            string query = Helpers.XmlToQueryString(document as XmlNode);
            if (query != "CHANNELID=a43a7d2&PASSWORD=248066a1&AFFSUBID=1&AFFSUBID2=2&REFERRINGURL=http%3a%2f%2fwww.911paydayadvance.com&MINPRICE=2&IPADDRESS=107.135.93.244&REQUESTEDAMOUNT=500&SSN=3CF3FC36548DABEF867A3CF2954402D3B2A999B4FDB6A7B5DB140EC8658A1EF6&DOB=03-25-1975&FIRSTNAME=l&LASTNAME=A&ADDRESS=1307+S+4th+Str&CITY=Bonham&STATE=NY&ZIP=75418&HOMEPHONE=9035837473&DLSTATE=TX&DLNUMBER=3FA3FEEA979931D1011B21F5EA13BDC55FD2F2D38896FCE97F539ECD38524E3C&ARMEDFORCES=No&CONTACTTIME=Anytime&RENTOROWN=Own&EMAIL=haikavn%40yahoo.com&ADDRESSMONTH=67&CITIZENSHIP=US&INCOMETYPE=Job+Income&EMPTIME=77&EMPNAME=Outreach+Home+Health&EMPPHONE=9035837473&JOBTITLE=Sales&PAYFREQUENCY=Every+2+weeks&NEXTPAYDATE=03-15-2019&SECONDPAYDATE=03-29-2019&BANKNAME=American+Bank+Of+Texas&BANKPHONE=5559995555&ACCOUNTTYPE=Checking+Account&ROUTINGNUMBER=111901645&ACCOUNTNUMBER=1234567&BANKMONTHS=11&NETMONTHLYINCOME=1000&DIRECTDEPOSIT=Yes")
                throw new Exception("Invalid Query String Result");
        }

        public void PingTreeLeadCount()
        {
            Campaign campaign = requestContext.CampaignService.GetCampaignById(long.Parse(Settings.GetValue("PingTreeLeadCount", "campaignid")));

            List<BuyerChannel> buyerChannels = (List<BuyerChannel>)requestContext.BuyerChannelService.GetAllBuyerChannelsByCampaignId(campaign.Id);

            SharedData.ResetBuyerChannelLeadsCount(campaign.Id, new List<PingTreeItem>(), 5);

            while (buyerChannels.Count > 0)
            {
                var buyerChannel = buyerChannels[0];

                if (!SharedData.CheckBuyerChannelLeadsCount(campaign.Id, new PingTreeItem()))
                {
                    if (buyerChannels.Count > 0)
                        buyerChannels.RemoveAt(0);
                    continue;
                }

                SharedData.DecrementBuyerChannelLeadsCount(campaign.Id, new PingTreeItem());
            }
        }

        public void ValidateDoNotPresent()
        {
            string message = "";
            if (!exportManager.ValidateDoNotPresent(null, requestContext, requestContext.BuyerService.GetBuyerById(long.Parse(Settings.GetValue("ValidateDoNotPresent", "buyerid"))), out message))
                throw new Exception("ValidateSchedule failed");
        }

        public void CheckSubIdWhiteList()
        {
            var _subIdWhiteListService = AppEngineContext.Current.Resolve<ISubIdWhiteListService>();
            _subIdWhiteListService.CheckSubIdWhiteList(Settings.GetValue("CheckSubIdWhiteList", "subid"), long.Parse(Settings.GetValue("CheckSubIdWhiteList", "BuyerChannelId")));
        }
    }
}