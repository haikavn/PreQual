using System;
using System.Web.Mvc;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure;
using Adrack.Service.Accounting;
using Adrack.Service.Content;
using Adrack.Service.Directory;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Security;
using Adrack.Web.ContentManagement.Controllers;
using Adrack.Web.Framework.ViewEngines.Razor;

namespace UnitTest_Services
{
    public class ServicesTestBuyerController
    {
        private readonly BuyerController bController;

        /// <summary>
        /// </summary>
        public ServicesTestBuyerController(UnitTestAllServices general)
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

            bController = new BuyerController(
                AppEngineContext.Current.Resolve<IBuyerService>(),
                null,
                AppEngineContext.Current.Resolve<ICountryService>(),
                AppEngineContext.Current.Resolve<IStateProvinceService>(),
                AppEngineContext.Current.Resolve<IUserService>(),
                AppEngineContext.Current.Resolve<IAccountingService>(),
                AppEngineContext.Current.Resolve<IHistoryService>(),
                AppEngineContext.Current.Resolve<IPermissionService>(),
                general.AppContext,
                AppEngineContext.Current.Resolve<IRoleService>(),
                AppEngineContext.Current.Resolve<IDepartmentService>(),
                AppEngineContext.Current.Resolve<IUserRegistrationService>(),
                AppEngineContext.Current.Resolve<IProfileService>(),
                AppEngineContext.Current.Resolve<UserSetting>(),
                AppEngineContext.Current.Resolve<IEmailService>(),
                AppEngineContext.Current.Resolve<IBuyerChannelService>(),
                AppEngineContext.Current.Resolve<IAuthenticationService>(),
                AppEngineContext.Current.Resolve<ICampaignService>(),
                AppEngineContext.Current.Resolve<IBuyerChannelTemplateService>(),
                AppEngineContext.Current.Resolve<IBuyerChannelFilterConditionService>(),
                AppEngineContext.Current.Resolve<ICampaignTemplateService>(),
                AppEngineContext.Current.Resolve<IAffiliateService>(),
                AppEngineContext.Current.Resolve<IAffiliateChannelService>()
            );
        }

        public void TestGetBuyers()
        {
            //  SqlConnection conn=connector.getSqlConnection();
            Console.WriteLine("Execution of GetBuyers");

            bController.SetFakeContext("d=-1");

            var res = bController.GetBuyers();
            ///...........write code here

            Console.WriteLine("Success");

            // connector.CloseConnection(conn);
        }

        public void TestBuyersList()
        {
            Console.WriteLine("Execution of TestBuyersList");

            var res = bController.List();
            if (res == null) throw new Exception("TestBuyersList Returned null");

            Console.WriteLine("Success");
        }

        public void TestBuyersIndex()
        {
            Console.WriteLine("Execution of TestBuyersIndex");

            var res = bController.Index();
            if (res == null) throw new Exception("TestBuyersIndex Returned null");

            Console.WriteLine("Success");
        }

        public void PartialItem()
        {
            Console.WriteLine("Execution of TestBuyersPartialItem");

            var res = bController.PartialItem(1);
            if (res == null) throw new Exception("TestBuyersPartialItem Returned null");

            Console.WriteLine("Success");
        }

        public void TestInsertDelete()
        {
            var buyerService = AppEngineContext.Current.Resolve<IBuyerService>();

            var buyerId = buyerService.InsertBuyer(new Buyer
            {
                AddressLine1 = "",
                AddressLine2 = "",
                BillFrequency = "",
                City = "",
                CountryId = 1,
                CreatedOn = DateTime.Now,
                Email = "",
                FrequencyValue = 0,
                LastPosted = DateTime.Now,
                LastPostedSold = DateTime.Now,
                ManagerId = 1,
                Name = "",
                Phone = "",
                StateProvinceId = 3,
                ZipPostalCode = ""
            });

            var buyer = buyerService.GetBuyerById(buyerId);

            buyerService.DeleteBuyer(buyer);
        }
    }
}