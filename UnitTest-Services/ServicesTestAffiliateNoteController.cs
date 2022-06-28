using System;
using System.Web.Mvc;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure;
using Adrack.Service.Configuration;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Web.ContentManagement.Controllers;
using Adrack.Web.Framework.ViewEngines.Razor;

namespace UnitTest_Services
{
    public class ServicesTestAffiliateNoteController
    {
        private readonly AffiliateNoteController aController;

        /// <summary>
        /// </summary>
        public ServicesTestAffiliateNoteController(UnitTestAllServices general)
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

            aController = new AffiliateNoteController(
                AppEngineContext.Current.Resolve<IAffiliateNoteService>(),
                AppEngineContext.Current.Resolve<ISettingService>(),
                AppEngineContext.Current.Resolve<ILocalizedStringService>()
            );
        }

        public void TestGetAffiliateNotes()
        {
            Console.WriteLine("Execution of GetAffiliateNotes");

            aController.SetFakeContext("st=1");

            var res = aController.GetAffiliateNotes();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestAffiliateNoteIndex()
        {
            Console.WriteLine("Execution of Index");

            aController.SetFakeContext("st=1");

            var res = aController.Index();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestInsertDelete()
        {
            var affiliateNoteService = AppEngineContext.Current.Resolve<IAffiliateNoteService>();

            var affiliateNoteId = affiliateNoteService.InsertAffiliateNote(new AffiliateNote
            {
                Created = DateTime.Now,
                AffiliateId = 1,
                Note = ""
            });

            var affiliateNote = affiliateNoteService.GetAffiliateNoteById(affiliateNoteId);

            affiliateNoteService.DeleteAffiliateNote(affiliateNote);
        }
    }
}