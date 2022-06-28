using System;
using System.Web.Mvc;
using Adrack.Core.Domain.Security;
using Adrack.Core.Infrastructure;
using Adrack.Service.Directory;
using Adrack.Service.Helpers;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Adrack.Web.ContentManagement.Controllers;
using Adrack.Web.Framework.ViewEngines.Razor;

namespace UnitTest_Services
{
    public class ServicesTestRoleController
    {
        private readonly RoleController aController;

        /// <summary>
        /// </summary>
        public ServicesTestRoleController(UnitTestAllServices general)
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

            aController = new RoleController(
                AppEngineContext.Current.Resolve<IRoleService>(),
                AppEngineContext.Current.Resolve<IUserTypeService>(),
                AppEngineContext.Current.Resolve<IPermissionService>(),
                AppEngineContext.Current.Resolve<IUserService>()
            );
        }

        public void TestGetRoles()
        {
            Console.WriteLine("Execution of GetRoles");

            aController.SetFakeContext("st=1");

            var res = aController.GetRoles();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestGetUsers()
        {
            Console.WriteLine("Execution of GetUsers");

            aController.SetFakeContext("st=1");

            var res = aController.GetUsers();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestInsertDelete()
        {
            var roleService = AppEngineContext.Current.Resolve<IRoleService>();

            var roleId = roleService.InsertRole(new Role
            {
                Name = "",
                Active = true,
                BuiltIn = true,
                Deleted = false,
                Description = "",
                Key = "",
                UserType = SharedData.BuiltInUserTypeId
            });

            var role = roleService.GetRoleById(roleId);

            roleService.DeleteRole(role);
        }
    }
}