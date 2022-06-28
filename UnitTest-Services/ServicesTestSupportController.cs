using System;
using System.Web.Mvc;
using Adrack.Core.Infrastructure;
using Adrack.Service.Content;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Web.ContentManagement.Controllers;
using Adrack.Web.Framework.ViewEngines.Razor;

namespace UnitTest_Services
{
    public class ServicesTestSupportController
    {
        private readonly SupportController supportController;

        /// <summary>
        /// </summary>
        public ServicesTestSupportController(UnitTestAllServices general)
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

            supportController = new SupportController(
                AppEngineContext.Current.Resolve<ISupportTicketsService>(),
                AppEngineContext.Current.Resolve<ISupportTicketsMessagesService>(),
                general.AppContext,
                AppEngineContext.Current.Resolve<IUserService>(),
                AppEngineContext.Current.Resolve<IEmailService>(),
                AppEngineContext.Current.Resolve<IAffiliateService>(),
                AppEngineContext.Current.Resolve<IBuyerService>()
            );
        }

        public void TestSupportIndex()
        {
            Console.WriteLine("Execution of supportController");

            supportController.SetFakeContext("st=1");

            var res = supportController.Index();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestGetSupportTickets()
        {
            Console.WriteLine("Execution of GetSupportTickets");

            supportController.SetFakeContext("st=1");

            var res = supportController.GetSupportTickets();

            Console.WriteLine("Success");
        }

        public void TestGetSupportTicketsMessages()
        {
            Console.WriteLine("Execution of GetSupportTicketsMessages");

            supportController.SetFakeContext("ticketid=8");

            var res = supportController.GetSupportTicketsMessages();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestSupportItem()
        {
            Console.WriteLine("Execution of SupportItem");

            supportController.SetFakeContext("ticketid=8");

            var res = supportController.Item(8);

            Console.WriteLine("Success");
        }

        public void TestChangeTicketsStatus()
        {
            Console.WriteLine("Execution of ChangeTicketsStatus");

            supportController.SetFakeContext("ticketid=8&status=1");

            var res = supportController.ChangeTicketsStatus();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestAddTicketsMessages()
        {
            Console.WriteLine("Execution of AddTicketsMessages");

            supportController.SetFakeContext("ticketid=8&message=test1");

            var res = supportController.AddTicketsMessages();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }
    }
}