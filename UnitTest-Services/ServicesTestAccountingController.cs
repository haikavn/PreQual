using System;
using System.Web.Mvc;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Accounting;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Directory;
using Adrack.Service.Lead;
using Adrack.Web.ContentManagement.Controllers;
using Adrack.Web.Framework.ViewEngines.Razor;

namespace UnitTest_Services
{
    public class ServicesTestAccountingController
    {
        private readonly AccountingController accountingController;

        /// <summary>
        /// </summary>
        public ServicesTestAccountingController(UnitTestAllServices general)
        {
            General = general;
            Settings = new TestSettings();
            Settings.Load("ServicesTestAccountingController");

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

            accountingController = new AccountingController(
                AppEngineContext.Current.Resolve<IAccountingService>(),
                AppEngineContext.Current.Resolve<IAffiliateService>(),
                AppEngineContext.Current.Resolve<IBuyerService>(),
                AppEngineContext.Current.Resolve<ISettingService>(),
                general.AppContext,
                AppEngineContext.Current.Resolve<IRepository<Buyer>>(),
                AppEngineContext.Current.Resolve<IRepository<Affiliate>>(),
                AppEngineContext.Current.Resolve<ICountryService>(),
                AppEngineContext.Current.Resolve<IHistoryService>(),
                AppEngineContext.Current.Resolve<ILeadMainResponseService>(),
                AppEngineContext.Current.Resolve<ILeadMainService>()
            );
        }

        private UnitTestAllServices General { get; }

        private TestSettings Settings { get; }

        public void TestAccountingIndex()
        {
            Console.WriteLine("Execution of AccountingIndex");

            accountingController.SetFakeContext("st=" + Settings.GetValue("TestAccountingIndex", "st"));

            var res = accountingController.Index();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestGetAffiliateInvoices()
        {
            Console.WriteLine("Execution of GetAffiliateInvoices");

            accountingController.SetFakeContext("st=" + Settings.GetValue("TestGetAffiliateInvoices", "st"));

            var res = accountingController.GetAffiliateInvoices();

            Console.WriteLine("Success");
        }

        public void TestAffiliateInvoices()
        {
            Console.WriteLine("Execution of AffiliateInvoices");

            accountingController.SetFakeContext("st=" + Settings.GetValue("TestAffiliateInvoices", "st"));

            var res = accountingController.AffiliateInvoices();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestGetBuyerInvoices()
        {
            Console.WriteLine("Execution of GetBuyerInvoices");

            accountingController.SetFakeContext("st=" + Settings.GetValue("TestGetBuyerInvoices", "st"));

            var res = accountingController.GetBuyerInvoices();

            Console.WriteLine("Success");
        }

        public void TestBuyerInvoices()
        {
            Console.WriteLine("Execution of BuyerInvoices");

            accountingController.SetFakeContext("st=" + Settings.GetValue("TestBuyerInvoices", "st"));

            var res = accountingController.BuyerInvoices();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestBuyerInvoicesPartial()
        {
            Console.WriteLine("Execution of BuyerInvoicesPartial");

            accountingController.SetFakeContext("id=" + Settings.GetValue("TestBuyerInvoicesPartial", "id"));

            var res = accountingController.BuyerInvoicesPartial(1);
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestAffiliateInvoicesPartial()
        {
            Console.WriteLine("Execution of AffiliateInvoicesPartial");

            accountingController.SetFakeContext("id=" + Settings.GetValue("AffiliateInvoicesPartial", "id"));

            var res = accountingController.AffiliateInvoicesPartial(1);
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestBuyerInvoiceItem()
        {
            Console.WriteLine("Execution of BuyerInvoiceItem");

            accountingController.SetFakeContext("id=" + Settings.GetValue("TestBuyerInvoiceItem", "id"));

            var res = accountingController.BuyerInvoiceItem(1);

            Console.WriteLine("Success");
        }

        public void TestAffiliateInvoiceItem()
        {
            Console.WriteLine("Execution of AffiliateInvoiceItem");

            accountingController.SetFakeContext("id=" + Settings.GetValue("TestAffiliateInvoiceItem", "id"));

            var res = accountingController.AffiliateInvoiceItem(1);
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestGenerateInvoices()
        {
            Console.WriteLine("Execution of GenerateInvoices");

            accountingController.SetFakeContext("id=" + Settings.GetValue("TestGenerateInvoices", "id"));

            var res = accountingController.GenerateInvoices();

            Console.WriteLine("Success");
        }

        public void TestGenerateCustomInvoices()
        {
            Console.WriteLine("Execution of GenerateCustomInvoices");

            accountingController.SetFakeContext("id=" + Settings.GetValue("TestGenerateCustomInvoices", "id"));

            var res = accountingController.GenerateCustomInvoices();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestGenerateInvoiceAjax()
        {
            Console.WriteLine("Execution of GenerateCustomInvoices");

            accountingController.SetFakeContext("buyerid=" + Settings.GetValue("TestGenerateInvoiceAjax", "buyerid") +
                                                "&dateto=" + Settings.GetValue("TestGenerateInvoiceAjax", "dateto"));

            var res = accountingController.GenerateInvoiceAjax();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestRefundedLeads()
        {
            Console.WriteLine("Execution of RefundedLeads");

            accountingController.SetFakeContext("buyerid=" + Settings.GetValue("TestRefundedLeads", "buyerid") +
                                                "&dateto=" + Settings.GetValue("TestRefundedLeads", "dateto"));

            var res = accountingController.RefundedLeads();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void TestGetRefundedLeads()
        {
            Console.WriteLine("Execution of GetRefundedLeads");

            accountingController.SetFakeContext("buyerid=" + Settings.GetValue("TestGetRefundedLeads", "buyerid") +
                                                "&dateto=" + Settings.GetValue("TestGetRefundedLeads", "dateto"));

            var res = accountingController.GetRefundedLeads();

            Console.WriteLine("Success");
        }

        public void TestBuyerPayments()
        {
            Console.WriteLine("Execution of BuyerPayments");

            accountingController.SetFakeContext("buyerid=" + Settings.GetValue("TestBuyerPayments", "buyerid"));

            var res = accountingController.GetBuyerPayments();

            Console.WriteLine("Success");
        }

        public void TestBuyersBalance()
        {
            Console.WriteLine("Execution of BuyersBalance");

            accountingController.SetFakeContext("buyerid=" + Settings.GetValue("TestBuyersBalance", "buyerid"));

            var res = accountingController.BuyersBalance();

            Console.WriteLine("Success");
        }

        public void TestChangeRefundedStatus()
        {
            Console.WriteLine("Execution of ChangeRefundedStatus");

            accountingController.SetFakeContext("id=" + Settings.GetValue("TestChangeRefundedStatus", "id") +
                                                "&status=" + Settings.GetValue("TestChangeRefundedStatus", "status") +
                                                "&note=" + Settings.GetValue("TestChangeRefundedStatus", "note"));

            var res = accountingController.ChangeRefundedStatus();

            Console.WriteLine("Success");
        }

        public void TestGetBuyerDistrib()
        {
            Console.WriteLine("Execution of GetBuyerDistrib");

            accountingController.SetFakeContext("BuyerId=" + Settings.GetValue("TestGetBuyerDistrib", "BuyerId"));

            var res = accountingController.GetBuyerDistrib();

            Console.WriteLine("Success");
        }

        //
        public void TestAddBuyerPayment()
        {
            Console.WriteLine("Execution of TestAddBuyerPayment");

            accountingController.SetFakeContext("affiliateid=" +
                                                Settings.GetValue("TestAddBuyerPayment", "affiliateid") + "&amount=" +
                                                Settings.GetValue("TestAddBuyerPayment", "amount") + "&date=" +
                                                Settings.GetValue("TestAddBuyerPayment", "date") + "&note=" +
                                                Settings.GetValue("TestAddBuyerPayment", "note") + "&method=" +
                                                Settings.GetValue("TestAddBuyerPayment", "method"));

            var res = accountingController.AddBuyerPayment();

            Console.WriteLine("Success");
            if (res != null)
            {
                var insertedId = long.Parse(((JsonResult) res).Data.ToString());
            }
        }

        public void TestDeleteBuyerPayment()
        {
            Console.WriteLine("Execution of TestDeleteBuyerPayment");

            accountingController.SetFakeContext("id=" + Settings.GetValue("TestDeleteBuyerPayment", "id"));

            var res = accountingController.DeleteBuyerPayment();

            Console.WriteLine("Success");
        }

        public void ComplexTestAddAndDeleteBuyerPayment()
        {
            Console.WriteLine("Execution of ComplexTestAddAndDeleteBuyerPayment");

            accountingController.SetFakeContext("affiliateid=" +
                                                Settings.GetValue("ComplexTestAddAndDeleteBuyerPayment",
                                                    "affiliateid") + "&amount=" +
                                                Settings.GetValue("ComplexTestAddAndDeleteBuyerPayment", "amount") +
                                                "&date=" +
                                                Settings.GetValue("ComplexTestAddAndDeleteBuyerPayment", "date") +
                                                "&note=" +
                                                Settings.GetValue("ComplexTestAddAndDeleteBuyerPayment", "note") +
                                                "&method=" + Settings.GetValue("ComplexTestAddAndDeleteBuyerPayment",
                                                    "method"));

            var res = accountingController.AddBuyerPayment();
            if (res != null)
            {
                var insertedId = long.Parse(((JsonResult) res).Data.ToString());

                accountingController.SetFakeContext("id=" + insertedId);

                var res2 = accountingController.DeleteBuyerPayment();
            }

            Console.WriteLine("Success");
        }

        public void ComplexTestAddAndDeleteRefundLeads()
        {
            Console.WriteLine("Execution of TestAddRefundLeads");

            accountingController.SetFakeContext(
                "leadid=" + Settings.GetValue("ComplexTestAddAndDeleteRefundLeads", "leadid") + "&note=" +
                Settings.GetValue("ComplexTestAddAndDeleteRefundLeads", "note"));

            var res = accountingController.AddRefundLeads();
            if (res == null) throw new Exception("Returned null");

            var insertedId = long.Parse(((JsonResult) res).Data.ToString());

            accountingController.SetFakeContext("id=" + insertedId);

            var res2 = accountingController.DeleteRefundedLeds();
            if (res2 == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void CreateBulkInvoiceAffiliate()
        {
            Console.WriteLine("Execution of CreateBulkInvoiceAffiliate");

            accountingController.SetFakeContext("affiliateid=" +
                                                Settings.GetValue("CreateBulkInvoiceAffiliate", "affiliateid") +
                                                "&dateto=" + Settings.GetValue("CreateBulkInvoiceAffiliate", "dateto"));

            var res = accountingController.CreateBulkInvoiceAffiliate();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void CreateBulkInvoiceBuyer()
        {
            Console.WriteLine("Execution of CreateBulkInvoiceBuyer");

            accountingController.SetFakeContext("buyerid=" + Settings.GetValue("CreateBulkInvoiceBuyer", "buyerid") +
                                                "&dateto=" + Settings.GetValue("CreateBulkInvoiceBuyer", "dateto"));

            var res = accountingController.CreateBulkInvoiceBuyer();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void PdfAffiliate()
        {
            Console.WriteLine("Execution of PdfAffiliate");

            accountingController.SetFakeContext("");

            var res = accountingController.PdfAffiliate(1);
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void DeleteAffiliateInvoiceAdjustment()
        {
            Console.WriteLine("Execution of DeleteAffiliateInvoiceAdjustment");

            accountingController.SetFakeContext("adjustmentid=" +
                                                Settings.GetValue("DeleteAffiliateInvoiceAdjustment", "adjustmentid"));

            var res = accountingController.DeleteAffiliateInvoiceAdjustment();

            Console.WriteLine("Success");
        }

        public void DeleteBuyerInvoiceAdjustment()
        {
            Console.WriteLine("Execution of DeleteBuyerInvoiceAdjustment");

            accountingController.SetFakeContext("adjustmentid=" +
                                                Settings.GetValue("DeleteBuyerInvoiceAdjustment", "adjustmentid"));

            var res = accountingController.DeleteBuyerInvoiceAdjustment();

            Console.WriteLine("Success");
        }

        public void AddAffiliateInvoiceAdjustment()
        {
            Console.WriteLine("Execution of AddAffiliateInvoiceAdjustment");

            accountingController.SetFakeContext("ainvoiceid=" +
                                                Settings.GetValue("AddAffiliateInvoiceAdjustment", "ainvoiceid") +
                                                "&price=" +
                                                Settings.GetValue("AddAffiliateInvoiceAdjustment", "price") + "&name=" +
                                                Settings.GetValue("AddAffiliateInvoiceAdjustment", "a") + "&qty=" +
                                                Settings.GetValue("AddAffiliateInvoiceAdjustment", "qty"));

            var res = accountingController.AddAffiliateInvoiceAdjustment();

            Console.WriteLine("Success");
        }

        public void AddBuyerInvoiceAdjustment()
        {
            Console.WriteLine("Execution of AddBuyerInvoiceAdjustment");

            accountingController.SetFakeContext("binvoiceid=" +
                                                Settings.GetValue("AddBuyerInvoiceAdjustment", "binvoiceid") +
                                                "&price=" + Settings.GetValue("AddBuyerInvoiceAdjustment", "price") +
                                                "&name=" + Settings.GetValue("AddBuyerInvoiceAdjustment", "price") +
                                                "&qty=" + Settings.GetValue("AddBuyerInvoiceAdjustment", "qty"));

            var res = accountingController.AddBuyerInvoiceAdjustment();

            Console.WriteLine("Success");
        }

        public void GetAffiliatesBalanceAjax()
        {
            Console.WriteLine("Execution of GetAffiliatesBalanceAjax");

            accountingController.SetFakeContext("buyerid=" + Settings.GetValue("GetAffiliatesBalanceAjax", "buyerid"));

            var res = accountingController.GetAffiliatesBalanceAjax();

            Console.WriteLine("Success");
        }

        public void GetBuyersBalanceAjax()
        {
            Console.WriteLine("Execution of GetBuyersBalanceAjax");

            accountingController.SetFakeContext("buyerid=" + Settings.GetValue("GetBuyersBalanceAjax", "buyerid"));

            var res = accountingController.GetBuyersBalanceAjax();

            Console.WriteLine("Success");
        }

        public void AffiliatesBalance()
        {
            Console.WriteLine("Execution of AffiliatesBalance");

            accountingController.SetFakeContext("buyerid=" + Settings.GetValue("AffiliatesBalance", "buyerid"));

            var res = accountingController.AffiliatesBalance();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void GenerateInvoices()
        {
            Console.WriteLine("Execution of GenerateInvoices");

            accountingController.SetFakeContext("p=" + Settings.GetValue("GenerateInvoices", "p"));

            var res = accountingController.GenerateInvoices();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void GenerateCustomInvoices()
        {
            Console.WriteLine("Execution of GenerateCustomInvoices");

            accountingController.SetFakeContext("p=" + Settings.GetValue("GenerateCustomInvoices", "p"));

            var res = accountingController.GenerateCustomInvoices();
            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        public void AddRefundLeads()
        {
            Console.WriteLine("Execution of GenerateCustomInvoices");

            accountingController.SetFakeContext("leadid=" + Settings.GetValue("AddRefundLeads", "leadid") +
                                                "&buyerid=" + Settings.GetValue("AddRefundLeads", "buyerid") +
                                                "&note=" + Settings.GetValue("AddRefundLeads", "note1"));

            var res = accountingController.AddRefundLeads();

            Console.WriteLine("Success");
        }
    }
}