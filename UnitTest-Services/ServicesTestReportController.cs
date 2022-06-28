using System;
using System.Web.Mvc;
using Adrack.Core.Infrastructure;
using Adrack.Service.Configuration;
using Adrack.Service.Directory;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Web.ContentManagement.Controllers;
using Adrack.Web.Framework.ViewEngines.Razor;

namespace UnitTest_Services
{
    public class ServicesTestReportController
    {
        private readonly ReportController bController;

        /// <summary>
        /// </summary>
        public ServicesTestReportController(UnitTestReportServices general)
        {
            General = general;

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

            bController = new ReportController(
                General.AppContext,
                AppEngineContext.Current.Resolve<IReportService>(),
                AppEngineContext.Current.Resolve<IBuyerService>(),
                AppEngineContext.Current.Resolve<ICampaignService>(),
                AppEngineContext.Current.Resolve<IAffiliateService>(),
                AppEngineContext.Current.Resolve<IBuyerChannelService>(),
                AppEngineContext.Current.Resolve<ISettingService>(),
                AppEngineContext.Current.Resolve<IBuyerChannelScheduleService>(),
                AppEngineContext.Current.Resolve<ILocalizedStringService>(),
                AppEngineContext.Current.Resolve<IStateProvinceService>(),
                AppEngineContext.Current.Resolve<INoteTitleService>(),
                AppEngineContext.Current.Resolve<IAffiliateChannelService>(),
                AppEngineContext.Current.Resolve<ICountryService>(),
                AppEngineContext.Current.Resolve<ILeadMainService>()
            );
        }

        private UnitTestReportServices General { get; }

        public void TestGetBuyerReportByBuyerChannel()
        {
            //  SqlConnection conn=connector.getSqlConnection();
            Console.WriteLine("Execution of GetBuyerReportByBuyerChannel");

            bController.SetFakeContext("st=1&startDate=2019-01-01&endDate=2019-01-10");

            var res = bController.GetBuyerReportByBuyerChannel();
            if (res == null) throw new Exception("Returned null");
            ///...........write code here

            Console.WriteLine("Success");

            // connector.CloseConnection(conn);
        }

        public void TestGetReportAffiliatesByAffiliateChannels()
        {
            //  SqlConnection conn=connector.getSqlConnection();
            Console.WriteLine("Execution of GetReportAffiliatesByAffiliateChannels");

            bController.SetFakeContext("st=1&startDate=2019-01-01&endDate=2019-01-10");

            var res = bController.GetReportAffiliatesByAffiliateChannels();
            if (res == null) throw new Exception("Returned null");
            ///...........write code here

            Console.WriteLine("Success");

            // connector.CloseConnection(conn);
        }

        public void TestGetReportAffiliatesByCampaigns()
        {
            //  SqlConnection conn=connector.getSqlConnection();
            Console.WriteLine("Execution of GetReportAffiliatesByCampaigns");

            bController.SetFakeContext("st=1&startDate=2019-01-01&endDate=2019-01-10");

            var res = bController.GetReportAffiliatesByCampaigns();
            if (res == null) throw new Exception("Returned null");
            ///...........write code here

            Console.WriteLine("Success");

            // connector.CloseConnection(conn);
        }

        public void TestGetReportAffiliatesByStates()
        {
            //  SqlConnection conn=connector.getSqlConnection();
            Console.WriteLine("Execution of GetReportAffiliatesByStates");

            bController.SetFakeContext("st=1&startDate=2019-01-01&endDate=2019-01-10");

            var res = bController.GetReportAffiliatesByStates();
            if (res == null) throw new Exception("Returned null");
            ///...........write code here

            Console.WriteLine("Success");

            // connector.CloseConnection(conn);
        }

        public void TestGetReportBuyersByAffiliateChannels()
        {
            //  SqlConnection conn=connector.getSqlConnection();
            Console.WriteLine("Execution of GetReportBuyersByAffiliateChannels");

            bController.SetFakeContext("st=1&startDate=2019-01-01&endDate=2019-01-10");

            var res = bController.GetReportBuyersByAffiliateChannels();
            if (res == null) throw new Exception("Returned null");
            ///...........write code here

            Console.WriteLine("Success");

            // connector.CloseConnection(conn);
        }

        public void TestGetReportBuyersByCampaigns()
        {
            //  SqlConnection conn=connector.getSqlConnection();
            Console.WriteLine("Execution of GetReportBuyersByCampaigns");

            bController.SetFakeContext("st=1&startDate=2019-01-01&endDate=2019-01-10");

            var res = bController.GetReportBuyersByCampaigns();
            if (res == null) throw new Exception("Returned null");
            ///...........write code here

            Console.WriteLine("Success");

            // connector.CloseConnection(conn);
        }

        public void TestGetReportBuyersByDates()
        {
            //  SqlConnection conn=connector.getSqlConnection();
            Console.WriteLine("Execution of GetReportBuyersByDates");

            bController.SetFakeContext("st=1&startDate=2019-01-01&endDate=2019-01-10");

            var res = bController.GetReportBuyersByDates();
            if (res == null) throw new Exception("Returned null");
            ///...........write code here

            Console.WriteLine("Success");

            // connector.CloseConnection(conn);
        }

        public void TestGetReportBuyersByLeadNotes()
        {
            //  SqlConnection conn=connector.getSqlConnection();
            Console.WriteLine("Execution of GetReportBuyersByLeadNotes");

            bController.SetFakeContext("st=1&startDate=2019-01-01&endDate=2019-01-10");

            var res = bController.GetReportBuyersByLeadNotes();
            if (res == null) throw new Exception("Returned null");
            ///...........write code here

            Console.WriteLine("Success");

            // connector.CloseConnection(conn);
        }

        public void TestGetReportBuyersByReactionTime()
        {
            //  SqlConnection conn=connector.getSqlConnection();
            Console.WriteLine("Execution of GetReportBuyersByReactionTime");

            bController.SetFakeContext("st=1&startDate=2019-01-01&endDate=2019-01-10");

            var res = bController.GetReportBuyersByReactionTime();
            if (res == null) throw new Exception("Returned null");
            ///...........write code here

            Console.WriteLine("Success");

            // connector.CloseConnection(conn);
        }

        public void TestGetReportBuyersByStates()
        {
            //  SqlConnection conn=connector.getSqlConnection();
            Console.WriteLine("Execution of GetReportBuyersByStates");

            bController.SetFakeContext("st=1&startDate=2019-01-01&endDate=2019-01-10");

            var res = bController.GetReportBuyersByStates();
            if (res == null) throw new Exception("Returned null");
            ///...........write code here

            Console.WriteLine("Success");

            // connector.CloseConnection(conn);
        }

        public void TestGetReportByDays()
        {
            //  SqlConnection conn=connector.getSqlConnection();
            Console.WriteLine("Execution of GetReportByDays");

            bController.SetFakeContext("st=1&startDate=2019-01-01&endDate=2019-01-10");

            var res = bController.GetReportByDays();
            if (res == null) throw new Exception("Returned null");
            ///...........write code here

            Console.WriteLine("Success");

            // connector.CloseConnection(conn);
        }

        public void TestGetReportByHour()
        {
            //  SqlConnection conn=connector.getSqlConnection();
            Console.WriteLine("Execution of GetReportByHour");

            bController.SetFakeContext("st=1&startDate=2019-01-01&endDate=2019-01-10");

            var res = bController.GetReportByHour();
            if (res == null) throw new Exception("Returned null");
            ///...........write code here

            Console.WriteLine("Success");

            // connector.CloseConnection(conn);
        }

        public void TestGetReportByMinutes()
        {
            //  SqlConnection conn=connector.getSqlConnection();
            Console.WriteLine("Execution of GetReportByMinutes");

            bController.SetFakeContext("st=1&startDate=2019-01-01&endDate=2019-01-10");

            var res = bController.GetReportByMinutes();
            if (res == null) throw new Exception("Returned null");
            ///...........write code here

            Console.WriteLine("Success");

            // connector.CloseConnection(conn);
        }

        public void TestGetReportByYear()
        {
            //  SqlConnection conn=connector.getSqlConnection();
            Console.WriteLine("Execution of GetReportByYear");

            bController.SetFakeContext("st=1&startDate=2019-01-01&endDate=2019-01-10");

            var res = bController.GetReportByYear();
            if (res == null) throw new Exception("Returned null");
            ///...........write code here

            Console.WriteLine("Success");

            // connector.CloseConnection(conn);
        }

        public void TestGetReportTotals()
        {
            //  SqlConnection conn=connector.getSqlConnection();
            Console.WriteLine("Execution of GetReportTotals");

            bController.SetFakeContext("st=1&startDate=2019-01-01&endDate=2019-01-10");

            var res = bController.GetReportTotals();
            if (res == null) throw new Exception("Returned null");
            ///...........write code here

            Console.WriteLine("Success");

            // connector.CloseConnection(conn);
        }

        public void TestGetReportTotalsBuyer()
        {
            //  SqlConnection conn=connector.getSqlConnection();
            Console.WriteLine("Execution of GetReportTotalsBuyer");

            bController.SetFakeContext("st=1&startDate=2019-01-01&endDate=2019-01-10");

            var res = bController.GetReportTotalsBuyer();
            if (res == null) throw new Exception("Returned null");
            ///...........write code here

            Console.WriteLine("Success");

            // connector.CloseConnection(conn);
        }
    }
}