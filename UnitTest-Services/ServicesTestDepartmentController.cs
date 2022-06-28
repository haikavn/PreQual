using System;
using System.Web.Mvc;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure;
using Adrack.Service.Lead;
using Adrack.Web.ContentManagement.Controllers;
using Adrack.Web.Framework.ViewEngines.Razor;

namespace UnitTest_Services
{
    public class ServicesTestDepartmentController
    {
        private readonly DepartmentController aController;

        /// <summary>
        /// </summary>
        public ServicesTestDepartmentController(UnitTestAllServices general)
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

            aController = new DepartmentController(
                AppEngineContext.Current.Resolve<IDepartmentService>()
            );
        }

        public void TestGetDepartments()
        {
            Console.WriteLine("Execution of GetDepartments");

            aController.SetFakeContext("st=1");

            var res = aController.GetDepartments();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestInsertDelete()
        {
            var departmentService = AppEngineContext.Current.Resolve<IDepartmentService>();

            var departmentId = departmentService.InsertDepartment(new Department
            {
                Name = ""
            });

            var department = departmentService.GetDepartmentById(departmentId);

            departmentService.DeleteDepartment(department);
        }
    }
}