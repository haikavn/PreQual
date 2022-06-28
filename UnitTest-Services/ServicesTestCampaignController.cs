using System;
using System.Web.Mvc;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure;
using Adrack.Service.Content;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Web.ContentManagement.Controllers;
using Adrack.Web.Framework.ViewEngines.Razor;

namespace UnitTest_Services
{
    public class ServicesTestCampaignController
    {
        private readonly CampaignController bController;

        /// <summary>
        /// </summary>
        public ServicesTestCampaignController(UnitTestAllServices general)
        {
            // connector = new DBConnector();
            //string connectionString = "Data Source=ARMANZ; Initial Catalog=helloadrack; Integrated Security=True; Persist Security Info=False; Connect Timeout=10";
            //  connector.InitConnection(connectionString);

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

            bController = new CampaignController(
                AppEngineContext.Current.Resolve<ICampaignService>(),
                AppEngineContext.Current.Resolve<IBuyerChannelService>(),
                AppEngineContext.Current.Resolve<ICampaignTemplateService>(),
                AppEngineContext.Current.Resolve<IVerticalService>(),
                AppEngineContext.Current.Resolve<ILocalizedStringService>(),
                AppEngineContext.Current.Resolve<IHistoryService>(),
                general.AppContext,
                AppEngineContext.Current.Resolve<IReportService>(),
                AppEngineContext.Current.Resolve<IBuyerService>(),
                AppEngineContext.Current.Resolve<IUserService>(),
                AppEngineContext.Current.Resolve<IAffiliateChannelService>()
            );
        }

        public void TestGetCampaignFields()
        {
            //  SqlConnection conn=connector.getSqlConnection();
            Console.WriteLine("Execution of GetCampaignFields");

            bController.SetFakeContext("campaignid=1");

            var res = bController.GetCampaignFields();
            if (res == null) throw new Exception("Returned null");
            ///...........write code here

            Console.WriteLine("Success");

            // connector.CloseConnection(conn);
        }

        public void TestGetCampaigns()
        {
            Console.WriteLine("Execution of GetCampaigns");

            var res = bController.GetCampaigns();
            if (res == null) throw new Exception("TestGetCampaigns Returned null");

            Console.WriteLine("Success");
        }

        public void TestGetCampaignsByVerticalId()
        {
            Console.WriteLine("Execution of GetCampaignsByVerticalId");

            bController.SetFakeContext("verticalid=1");

            var res = bController.GetCampaignsByVerticalId();
            if (res == null) throw new Exception("TestGetCampaignsByVerticalId Returned null");

            Console.WriteLine("Success");
        }

        public void TestGetCampaignTemplates()
        {
            Console.WriteLine("Execution of GetCampaignTemplates");

            bController.SetFakeContext("verticalid=1");

            var res = bController.GetCampaignTemplates();
            if (res == null) throw new Exception("TestGetCampaignTemplates Returned null");

            Console.WriteLine("Success");
        }

        public void TestGetCampaignTemplatesByVerticalId()
        {
            Console.WriteLine("Execution of GetCampaignTemplatesByVerticalId");

            var res = bController.GetCampaignTemplatesByVerticalId();
            if (res == null) throw new Exception("TestGetCampaignTemplatesByVerticalId Returned null");

            Console.WriteLine("Success");
        }

        public void TestLoadCampaignTemplate()
        {
            Console.WriteLine("Execution of LoadCampaignTemplate");

            /*ActionResult res = bController.LoadCampaignTemplate();
            if (res == null)
            {
                throw new Exception("TestLoadCampaignTemplate Returned null");
            }*/

            Console.WriteLine("Success");
        }

        public void TestInsertDelete()
        {
            var campaignService = AppEngineContext.Current.Resolve<ICampaignService>();

            var campaignId = campaignService.InsertCampaign(new Campaign
            {
                CampaignKey = "",
                CampaignType = 0,
                Description = "",
                Finish = DateTime.Now,
                IsTemplate = false,
                Name = "",
                PriceFormat = 0,
                Start = DateTime.Now,
                Status = 0,
                VerticalId = 1,
                Visibility = 0,
                DataTemplate = "",
                CreatedOn = DateTime.Now
            });

            var campaign = campaignService.GetCampaignById(campaignId);

            campaignService.DeleteCampaign(campaign);
        }
    }
}