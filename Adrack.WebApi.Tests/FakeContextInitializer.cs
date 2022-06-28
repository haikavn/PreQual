using Adrack.Core;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Validation.Providers;
using System.Web.Mvc;
using DataAnnotationsModelValidatorProvider = System.Web.Mvc.DataAnnotationsModelValidatorProvider;

namespace Adrack.WebApi.Tests
{
    public class FakeContextInitializer
    {
        public FakeContextInitializer()
        {
            AppEngineContextInitialize(false);
            InitTestingUser();
        }

        private void InitTestingUser()
        {
            AppContext = AppEngineContext.Current.Resolve<IAppContext>();
        }

        private void AppEngineContextInitialize(bool v)
        {
            // Disable IIS Information Request
            MvcHandler.DisableMvcResponseHeader = true;

            // Initialize Engine Context
            AppEngineContext.Initialize(false);

            // Remove All View Engines
            ViewEngines.Engines.Clear();

            // Add Web Application Razor View Engine
            //ViewEngines.Engines.Add(new WebAppRazorViewEngine());

            // Add Functionality On Top Of The Default Model Metadata Provider

            // Registering Rebular MVC
            //AreaRegistration.RegisterAllAreas();

            // Fluent Validation
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            AppContext = AppEngineContext.Current.Resolve<IAppContext>();

            //AppContext.AppTestingUser = new User();
            //AppContext.AppTestingUser.Email = "testingUser@adrack.com";
            //AppContext.AppTestingUser.Id = 2000000000;

            //var vController = new VerticalController(
            //    AppEngineContext.Current.Resolve<IVerticalService>()
            //);
        }


        public IAppContext AppContext { get; private set; }
    }
}
