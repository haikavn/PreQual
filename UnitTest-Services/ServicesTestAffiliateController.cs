using System;
using System.Web.Mvc;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Directory;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Security;
using Adrack.Web.ContentManagement.Controllers;
using Adrack.Web.Framework.ViewEngines.Razor;

namespace UnitTest_Services
{
    public class ServicesTestAffiliateController
    {
        private readonly AffiliateController aController;

        /// <summary>
        /// </summary>
        public ServicesTestAffiliateController(UnitTestAllServices general)
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

            aController = new AffiliateController(
                AppEngineContext.Current.Resolve<IAffiliateService>(),
                AppEngineContext.Current.Resolve<IAffiliateNoteService>(),
                AppEngineContext.Current.Resolve<IAffiliateChannelService>(),
                AppEngineContext.Current.Resolve<ISettingService>(),
                AppEngineContext.Current.Resolve<IAffiliateChannelFilterConditionService>(),
                AppEngineContext.Current.Resolve<ILocalizedStringService>(),
                AppEngineContext.Current.Resolve<ICountryService>(),
                AppEngineContext.Current.Resolve<IStateProvinceService>(),
                AppEngineContext.Current.Resolve<IUserService>(),
                AppEngineContext.Current.Resolve<IHistoryService>(),
                AppEngineContext.Current.Resolve<IPermissionService>(),
                AppEngineContext.Current.Resolve<IRoleService>(),
                AppEngineContext.Current.Resolve<IDepartmentService>(),
                AppEngineContext.Current.Resolve<IUserRegistrationService>(),
                AppEngineContext.Current.Resolve<IProfileService>(),
                AppEngineContext.Current.Resolve<UserSetting>(),
                AppEngineContext.Current.Resolve<IEmailService>(),
                general.AppContext,
                AppEngineContext.Current.Resolve<ICampaignService>(),
                AppEngineContext.Current.Resolve<IBuyerChannelService>(),
                AppEngineContext.Current.Resolve<IAuthenticationService>()
            );
        }

        public void TestGetAffiliates()
        {
            Console.WriteLine("Execution of GetAffiliates");

            aController.SetFakeContext("st=1");

            var res = aController.GetAffiliates();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestAffiliateIndex()
        {
            Console.WriteLine("Execution of Index");

            aController.SetFakeContext("st=1");

            var res = aController.Index();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestSetAffiliateStatus()
        {
            Console.WriteLine("Execution of Index");

            aController.SetFakeContext("id=1&status=1");

            var res = aController.SetAffiliateStatus();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestAffiliateList()
        {
            Console.WriteLine("Execution of List");

            aController.SetFakeContext("id=1&status=1");

            var res = aController.List();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestAffiliateItem()
        {
            Console.WriteLine("Execution of Item(1)");

            aController.SetFakeContext("id=1");

            var res = aController.Item(1);
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestInsertDelete()
        {
            var affiliateService = AppEngineContext.Current.Resolve<IAffiliateService>();

            var affiliateId = affiliateService.InsertAffiliate(new Affiliate
            {
                AddressLine1 = "",
                AddressLine2 = "",
                BillFrequency = "",
                City = "",
                CountryId = 1,
                CreatedOn = DateTime.Now,
                Email = "",
                FrequencyValue = 0,
                ManagerId = 1,
                Name = "",
                Phone = "",
                StateProvinceId = 3,
                ZipPostalCode = "",
                Website = ""
            });

            var affiliate = affiliateService.GetAffiliateById(affiliateId, true);

            affiliateService.DeleteAffiliate(affiliate);
        }
    }
}