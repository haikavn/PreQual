using System;
using System.Web.Mvc;
using Adrack.Core.Infrastructure;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Security;
using Adrack.Web.ContentManagement.Controllers;
using Adrack.Web.Framework.ViewEngines.Razor;

namespace UnitTest_Services
{
    public class ServicesTestAffiliateChannelController
    {
        private readonly AffiliateChannelController aController;

        /// <summary>
        /// </summary>
        public ServicesTestAffiliateChannelController(UnitTestAllServices general)
        {
            Settings = new TestSettings();
            Settings.Load("ServicesTestAffiliateChannelController");

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

            aController = new AffiliateChannelController(
                AppEngineContext.Current.Resolve<ICampaignService>(),
                AppEngineContext.Current.Resolve<IAffiliateService>(),
                AppEngineContext.Current.Resolve<IFilterService>(),
                AppEngineContext.Current.Resolve<IBlackListService>(),
                AppEngineContext.Current.Resolve<IAffiliateChannelFilterConditionService>(),
                AppEngineContext.Current.Resolve<ICampaignTemplateService>(),
                AppEngineContext.Current.Resolve<IAffiliateChannelService>(),
                AppEngineContext.Current.Resolve<IAffiliateResponseService>(),
                AppEngineContext.Current.Resolve<IAffiliateChannelTemplateService>(),
                AppEngineContext.Current.Resolve<ILocalizedStringService>(),
                AppEngineContext.Current.Resolve<IHistoryService>(),
                AppEngineContext.Current.Resolve<ISettingService>(),
                general.AppContext,
                AppEngineContext.Current.Resolve<IBuyerChannelService>(),
                AppEngineContext.Current.Resolve<IPermissionService>()
            );
        }

        private TestSettings Settings { get; }

        public void TestGetAffiliateChannels()
        {
            Console.WriteLine("Execution of GetAffiliateChannels");

            aController.SetFakeContext("օ=1&params=1&aid=1");

            var res = aController.GetAffiliateChannels();

            Console.WriteLine("Success");
        }

        public void TestGetAffiliateChannelXml()
        {
            Console.WriteLine("Execution of GetAffiliateChannelXml");

            aController.SetFakeContext("st=1");

            var res = aController.GetAffiliateChannelXml();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestGetAffiliateResponses()
        {
            Console.WriteLine("Execution of GetAffiliateResponses");

            aController.SetFakeContext("aid=1&xml=&json=");

            var res = aController.GetAffiliateResponses();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestGetCampaignTemp()
        {
            Console.WriteLine("Execution of GetCampaignTemp");

            aController.SetFakeContext("st=1");

            var res = aController.GetCampaignTemp();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }
    }
}