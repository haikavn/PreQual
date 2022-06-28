using System;
using System.Web.Mvc;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Web.ContentManagement.Controllers;
using Adrack.Web.Framework.ViewEngines.Razor;

namespace UnitTest_Services
{
    public class ServicesTestBlacklistController
    {
        private readonly BlackListController aController;

        /// <summary>
        /// </summary>
        public ServicesTestBlacklistController(UnitTestAllServices general)
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

            aController = new BlackListController(
                AppEngineContext.Current.Resolve<IBlackListService>(),
                AppEngineContext.Current.Resolve<ILocalizedStringService>()
            );
        }

        public void TestGetBlackListTypes()
        {
            Console.WriteLine("Execution of GetBlackListTypes");

            aController.SetFakeContext("st=1");

            var res = aController.GetBlackListTypes();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestGetBlackListValues()
        {
            Console.WriteLine("Execution of GetBlackListValues");

            aController.SetFakeContext("params=1");

            var res = aController.GetBlackListValues();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestGetCustomBlackListValues()
        {
            Console.WriteLine("Execution of GetCustomBlackListValues");

            aController.SetFakeContext("id=1&type=1&field=a");

            var res = aController.GetCustomBlackListValues();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestRemoveBlackListValue()
        {
            Console.WriteLine("Execution of RemoveBlackListValue");

            aController.SetFakeContext("id=1&status=1");

            var res = aController.RemoveBlackListValue();

            Console.WriteLine("Success");
        }

        public void TestRemoveCustomBlackListValue()
        {
            Console.WriteLine("Execution of RemoveCustomBlackListValue");

            aController.SetFakeContext("id=1");

            var res = aController.RemoveCustomBlackListValue();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestInsertDelete()
        {
            var blackListService = AppEngineContext.Current.Resolve<IBlackListService>();

            var blackListTypeId = blackListService.InsertBlackListType(new BlackListType
            {
                Name = "",
                BlackType = 0,
                ParentId = 1
            });

            var blackListType = blackListService.GetBlackListTypeById(blackListTypeId);

            var blackListTypeValueId = blackListService.InsertBlackListValue(new BlackListValue
            {
                BlackListTypeId = blackListTypeId,
                Value = ""
            });

            var blackListTypeValue = blackListService.GetBlackListValueById(blackListTypeValueId);

            blackListService.DeleteBlackListValue(blackListTypeValue);

            blackListService.DeleteBlackListType(blackListType);
        }
    }
}