using System;
using System.IO;
using System.Web.Mvc;
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure;
using Adrack.Service.Common;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Adrack.Web.ContentManagement.Controllers;
using Adrack.Web.Framework.ViewEngines.Razor;

namespace UnitTest_Services
{
    public class ServicesTestBuyerChannelController
    {
        private readonly BuyerChannelController aController;
        private TestSettings Settings { get; }

        /// <summary>
        /// </summary>
        public ServicesTestBuyerChannelController(UnitTestAllServices general)
        {
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

            aController = new BuyerChannelController(
                AppEngineContext.Current.Resolve<ICampaignService>(),
                AppEngineContext.Current.Resolve<IBuyerService>(),
                AppEngineContext.Current.Resolve<IFilterService>(),
                AppEngineContext.Current.Resolve<IBuyerChannelFilterConditionService>(),
                AppEngineContext.Current.Resolve<ICampaignTemplateService>(),
                AppEngineContext.Current.Resolve<IBuyerChannelService>(),
                AppEngineContext.Current.Resolve<IBuyerChannelTemplateService>(),
                AppEngineContext.Current.Resolve<ILocalizedStringService>(),
                AppEngineContext.Current.Resolve<IBuyerChannelScheduleService>(),
                AppEngineContext.Current.Resolve<IAffiliateChannelService>(),
                AppEngineContext.Current.Resolve<IPostedDataService>(),
                AppEngineContext.Current.Resolve<IHistoryService>(),
                AppEngineContext.Current.Resolve<IBuyerChannelTemplateMatchingService>(),
                general.AppContext,
                AppEngineContext.Current.Resolve<IAffiliateChannelTemplateService>(),
                AppEngineContext.Current.Resolve<IUserService>(),
                AppEngineContext.Current.Resolve<IPermissionService>(),
                AppEngineContext.Current.Resolve<IDateTimeHelper>(),
                AppEngineContext.Current.Resolve<ISettingService>(),
                AppEngineContext.Current.Resolve<ISubIdWhiteListService>()
            );

            Settings = new TestSettings();
            Settings.Load("ServicesTestBuyerChannel");

        }

        public void TestGetAllowedFrom()
        {
            Console.WriteLine("Execution of GetAllowedFrom");

            aController.SetFakeContext("id=1");

            var res = aController.GetAllowedFrom();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestGetBuyerChannels()
        {
            Console.WriteLine("Execution of GetBuyerChannels");

            aController.SetFakeContext("օ=1");

            var res = aController.GetBuyerChannels();

            Console.WriteLine("Success");
        }

        public void TestGetCampaignTemp()
        {
            Console.WriteLine("Execution of GetCampaignTemp");

            aController.SetFakeContext("st=1");

            var res = aController.GetCampaignTemp();

            Console.WriteLine("Success");
        }

        public void TestGetPostedData()
        {
            Console.WriteLine("Execution of GetPostedData");

            aController.SetFakeContext("bid=1");

            var res = aController.GetPostedData();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestInsertDelete()
        {
            var buyerChannelService = AppEngineContext.Current.Resolve<IBuyerChannelService>();

            var buyerChannelId = buyerChannelService.InsertBuyerChannel(new BuyerChannel
            {
                Name = "",
                AcceptedField = "",
                AcceptedFrom = 0,
                AcceptedValue = "",
                AffiliatePrice = 0,
                AfterTimeout = 0,
                BuyerId = 1,
                BuyerPrice = 0,
                CampaignId = 1,
                CapReachedNotification = false,
                DeliveryMethod = 0,
                ErrorField = "",
                ErrorFrom = 0,
                ErrorValue = "",
                IsFixed = false,
                MessageField = "",
                NotificationEmail = "",
                OrderNum = 0,
                PostingUrl = "",
                PriceField = "",
                RedirectField = "",
                RejectedField = "",
                RejectedFrom = 0,
                RejectedValue = "",
                Status = BuyerChannelStatuses.Active,
                TestField = "",
                TestFrom = 0,
                TestValue = "",
                Timeout = 0,
                TimeoutNotification = false,
                XmlTemplate = ""
            });

            var buyerChannel = buyerChannelService.GetBuyerChannelById(buyerChannelId);

            buyerChannelService.DeleteBuyerChannel(buyerChannel);
        }

        public void TestFillSubIdWhiteList()
        {
            string path = Settings.GetRootPath("~/App_Data/") + "SAMPLE_INCLUDE.csv";

            var _subIdWhiteListService = AppEngineContext.Current.Resolve<ISubIdWhiteListService>();

            long buyerChannelId = long.Parse(Settings.GetValue("FillSubIdWhiteList", "BuyerChannelId"));
            _subIdWhiteListService.DeleteAllSubIdWhiteList(buyerChannelId);

            StreamReader sr = new StreamReader(path);
            string line = "";
            int lineNumber = 0;
            int index = -1;

            while ((line = sr.ReadLine()) != null)
            {
                string[] row = line.Split(',');

                if (lineNumber == 0)
                {
                    index = Array.IndexOf(row, "MAX_SUBID");
                }
                else if (index >= 0 && index < row.Length)
                {
                    _subIdWhiteListService.InsertSubIdWhiteList(new SubIdWhiteList() { SubId = row[index], BuyerChannelId = buyerChannelId });
                }

                lineNumber++;
            }
            sr.Close();

        }
    }
}