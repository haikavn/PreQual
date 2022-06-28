using System;
using System.Web;
using System.Web.Mvc;
using Adrack.Core.Infrastructure;
using Adrack.Service.Accounting;
using Adrack.Service.Configuration;
using Adrack.Service.Directory;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Adrack.Web.ContentManagement.Controllers;
using Adrack.Web.Framework.ViewEngines.Razor;

namespace UnitTest_Services
{
    public class ServicesTestLeadController
    {
        private readonly LeadController lController;

        /// <summary>
        /// </summary>
        public ServicesTestLeadController(UnitTestAllServices general)
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

            lController = new LeadController(
                AppEngineContext.Current.Resolve<ILeadMainService>(),
                AppEngineContext.Current.Resolve<IAccountingService>(),
                AppEngineContext.Current.Resolve<ILeadMainResponseService>(),
                AppEngineContext.Current.Resolve<IAffiliateResponseService>(),
                AppEngineContext.Current.Resolve<ISettingService>(),
                AppEngineContext.Current.Resolve<ICampaignService>(),
                AppEngineContext.Current.Resolve<IAffiliateService>(),
                AppEngineContext.Current.Resolve<IAffiliateChannelService>(),
                AppEngineContext.Current.Resolve<IBuyerService>(),
                AppEngineContext.Current.Resolve<IBuyerChannelService>(),
                AppEngineContext.Current.Resolve<ILeadContentDublicateService>(),
                general.AppContext,
                AppEngineContext.Current.Resolve<IRedirectUrlService>(),
                AppEngineContext.Current.Resolve<INoteTitleService>(),
                AppEngineContext.Current.Resolve<IUserService>(),
                AppEngineContext.Current.Resolve<IEncryptionService>(),
                AppEngineContext.Current.Resolve<ICampaignTemplateService>(),
                AppEngineContext.Current.Resolve<ILeadSensitiveDataService>(),
                AppEngineContext.Current.Resolve<IBlackListService>(),
                AppEngineContext.Current.Resolve <IStateProvinceService>(),
                AppEngineContext.Current.Resolve<HttpContextBase>()
            );
        }

        public void TestLeadIndex()
        {
            Console.WriteLine("Execution of TestLeadIndex");

            lController.SetFakeContext("st=1");

            var res = lController.Index();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestLeadsAjax()
        {
            Console.WriteLine("Execution of TestLeadsAjax");
            lController.SetFakeContext("dates=11/01/2017:12/01/2017&page=1&pagesize=50&status=-1");
            var res = lController.GetLeadsAjax();
            Console.WriteLine("Success");
        }

        public void TestBadIPClicksReport()
        {
            Console.WriteLine("Execution of BadIPClicksReport");
            lController.SetFakeContext("");
            var res = lController.BadIPClicksReport();
            if (res == null) throw new Exception("Returned null");
            Console.WriteLine("Success");
        }

        public void TestLeadByIdAjax()
        {
            Console.WriteLine("Execution of LeadByIdAjax");
            lController.SetFakeContext("leadid=33100");
            var res = lController.GetLeadByIdAjax();
            if (res == null) throw new Exception("Returned null");
            Console.WriteLine("Success");
        }

        public void TestGetLeadNotesAjax()
        {
            Console.WriteLine("Execution of GetLeadNotesAjax");
            lController.SetFakeContext("leadid=33100");
            var res = lController.GetLeadNotesAjax();
            if (res == null) throw new Exception("Returned null");
            Console.WriteLine("Success");
        }

        public void TestLeadItem()
        {
            Console.WriteLine("Execution of LeadItem");
            lController.SetFakeContext("leadid=33100");
            var res = lController.Item(33100);
            if (res == null) throw new Exception("Returned null");
            Console.WriteLine("Success");
        }

        public void TestErrorLeadsReportBuyer()
        {
            Console.WriteLine("Execution of ErrorLeadsReportBuyer");
            lController.SetFakeContext("dates=01/01/2016:01/01/2017&error=0");
            var res = lController.ErrorLeadsReportBuyer();
            Console.WriteLine("Success");
        }

        public void TestErrorLeadsReportAffiliate()
        {
            Console.WriteLine("Execution of ErrorLeadsReportAffiliate");
            lController.SetFakeContext("dates=01/01/2016:01/01/2017&error=0");
            var res = lController.ErrorLeadsReportAffiliate();
            if (res == null) throw new Exception("Returned null");
            Console.WriteLine("Success");
        }

        public void TestGetErrorLeadsReportAjax()
        {
            Console.WriteLine("Execution of GetErrorLeadsReportAjax");
            lController.SetFakeContext("dates=01/01/2016:01/01/2017&error=0");
            var res = lController.GetErrorLeadsReportAjax();
            if (res == null) throw new Exception("Returned null");
            Console.WriteLine("Success");
        }

        public void TestGetBadIPClicksReportAjax()
        {
            Console.WriteLine("Execution of GetErrorLeadsReportAjax");
            lController.SetFakeContext("dates=01/01/2016:01/01/2017");
            var res = lController.GetBadIPClicksReportAjax();
            if (res == null) throw new Exception("Returned null");
            Console.WriteLine("Success");
        }
    }
}