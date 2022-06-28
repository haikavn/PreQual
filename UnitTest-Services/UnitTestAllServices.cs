using Adrack.Core;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest_Services
{
    [TestClass]
    public class UnitTestAllServices
    {
        private static ServicesTestBuyerController runner;
        private static ServicesTestAffiliateController runnerAffiliate;
        private static ServicesTestLeadController runnerLead;
        private static ServicesTestAccountingController runnerAccounting;
        private static ServicesTestSupportController runnerSupport;
        private static ServicesTestAffiliateChannelController runnerAffiliateChannel;
        private static ServicesTestBuyerChannelController runnerBuyerChannel;
        private static ServicesTestCampaignController runnerCampaign;
        private static ServicesTestBlacklistController runnerBlacklist;
        private static ServicesTestDepartmentController runnerDepartment;
        private static ServicesTestRoleController runnerRole;
        private static ServicesTestAffiliateNoteController runnerAffiliateNote;

        public UnitTestAllServices()
        {
            InitTestingUser();

            if (runner == null)
                runner = new ServicesTestBuyerController(this);

            if (runnerAffiliate == null)
                runnerAffiliate = new ServicesTestAffiliateController(this);

            if (runnerLead == null)
                runnerLead = new ServicesTestLeadController(this);

            if (runnerAccounting == null)
                runnerAccounting = new ServicesTestAccountingController(this);

            if (runnerSupport == null)
                runnerSupport = new ServicesTestSupportController(this);

            if (runnerBuyerChannel == null)
                runnerBuyerChannel = new ServicesTestBuyerChannelController(this);

            if (runnerAffiliateChannel == null)
                runnerAffiliateChannel = new ServicesTestAffiliateChannelController(this);

            if (runnerCampaign == null)
                runnerCampaign = new ServicesTestCampaignController(this);

            if (runnerBlacklist == null)
                runnerBlacklist = new ServicesTestBlacklistController(this);

            if (runnerDepartment == null)
                runnerDepartment = new ServicesTestDepartmentController(this);

            if (runnerRole == null)
                runnerRole = new ServicesTestRoleController(this);

            if (runnerAffiliateNote == null)
                runnerAffiliateNote = new ServicesTestAffiliateNoteController(this);
        }

        public IAppContext AppContext { get; set; }

        public void InitTestingUser()
        {
            AppContext = AppEngineContext.Current.Resolve<IAppContext>();
        }

#if DEBUG
        [TestMethod]
        public void GetBuyers()
        {
            runner.TestGetBuyers();
        }

        [TestMethod]
        public void BuyersIndex()
        {
            runner.TestBuyersIndex();
        }

        [TestMethod]
        public void BuyersList()
        {
            runner.TestBuyersList();
        }

        [TestMethod]
        public void InsertDeleteBuyer()
        {
            runner.TestInsertDelete();
        }

        /// Affiliate
        [TestMethod]
        public void GetAffiliates()
        {
            runnerAffiliate.TestGetAffiliates();
        }

        [TestMethod]
        public void AffiliateIndex()
        {
            runnerAffiliate.TestAffiliateIndex();
        }

        [TestMethod]
        public void SetAffiliateStatus()
        {
            runnerAffiliate.TestSetAffiliateStatus();
        }

        [TestMethod]
        public void AffiliateList()
        {
            runnerAffiliate.TestAffiliateList();
        }

        [TestMethod]
        public void AffiliateItem()
        {
            runnerAffiliate.TestAffiliateList();
        }

        [TestMethod]
        public void InsertDeleteAffiliate()
        {
            runnerAffiliate.TestInsertDelete();
        }

        /// Affiliate note
        [TestMethod]
        public void GetAffiliateNotes()
        {
            runnerAffiliateNote.TestGetAffiliateNotes();
        }

        [TestMethod]
        public void AffiliateNoteIndex()
        {
            runnerAffiliateNote.TestAffiliateNoteIndex();
        }

        [TestMethod]
        public void InsertDeleteAffiliateNote()
        {
            runnerAffiliateNote.TestInsertDelete();
        }

        //AffiliateChannels
        [TestMethod]
        public void GetAffiliateChannels()
        {
            runnerAffiliateChannel.TestGetAffiliateChannels();
        }

        [TestMethod]
        public void TestGetAffiliateChannelXml()
        {
            runnerAffiliateChannel.TestGetAffiliateChannelXml();
        }

        [TestMethod]
        public void TestGetAffiliateResponses()
        {
            runnerAffiliateChannel.TestGetAffiliateResponses();
        }

        [TestMethod]
        public void GetCampaignTemp()
        {
            runnerAffiliateChannel.TestGetCampaignTemp();
        }

        //BuyerChannels
        [TestMethod]
        public void GetAllowedFrom()
        {
            runnerBuyerChannel.TestGetAllowedFrom();
        }

        [TestMethod]
        public void GetBuyerChannels()
        {
            runnerBuyerChannel.TestGetBuyerChannels();
        }

        [TestMethod]
        public void InsertDeleteBuyerChannel()
        {
            runnerBuyerChannel.TestInsertDelete();
        }

        [TestMethod]
        public void BCGetCampaignTemp()
        {
            runnerBuyerChannel.TestGetCampaignTemp();
        }

        [TestMethod]
        public void GetPostedData()
        {
            runnerBuyerChannel.TestGetPostedData();
        }

        [TestMethod]
        public void FillSubIdWhiteList()
        {
            runnerBuyerChannel.TestFillSubIdWhiteList();
        }

        /// Lead /////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////
        [TestMethod]
        public void LeadIndex()
        {
            runnerLead.TestLeadIndex();
        }

        [TestMethod]
        public void GetLeadsAjax()
        {
            runnerLead.TestLeadsAjax();
        }

        [TestMethod]
        public void BadIPClicksReport()
        {
            runnerLead.TestBadIPClicksReport();
        }

        [TestMethod]
        public void LeadByIdAjax()
        {
            runnerLead.TestLeadByIdAjax();
        }

        [TestMethod]
        public void GetLeadNotesAjax()
        {
            runnerLead.TestGetLeadNotesAjax();
        }

        [TestMethod]
        public void LeadItem()
        {
            runnerLead.TestLeadItem();
        }

        [TestMethod]
        public void GetErrorLeadsReportAjax()
        {
            runnerLead.TestGetErrorLeadsReportAjax();
        }

        [TestMethod]
        public void ErrorLeadsReportBuyer()
        {
            runnerLead.TestErrorLeadsReportBuyer();
        }

        [TestMethod]
        public void ErrorLeadsReportAffiliate()
        {
            runnerLead.TestErrorLeadsReportAffiliate();
        }

        [TestMethod]
        public void GetBadIPClicksReportAjax()
        {
            runnerLead.TestGetBadIPClicksReportAjax();
        }

        /// Accounting ///////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////
        [TestMethod]
        public void AccountingIndex()
        {
            runnerAccounting.TestAccountingIndex();
        }

        [TestMethod]
        public void GetAffiliateInvoices()
        {
            runnerAccounting.TestGetAffiliateInvoices();
        }

        [TestMethod]
        public void AffiliateInvoices()
        {
            runnerAccounting.TestAffiliateInvoices();
        }

        [TestMethod]
        public void GetBuyerInvoices()
        {
            runnerAccounting.TestGetBuyerInvoices();
        }

        [TestMethod]
        public void BuyerInvoices()
        {
            runnerAccounting.TestBuyerInvoices();
        }

        [TestMethod]
        public void BuyerInvoicesPartial()
        {
            runnerAccounting.TestBuyerInvoicesPartial();
        }

        [TestMethod]
        public void AffiliateInvoicesPartial()
        {
            runnerAccounting.TestAffiliateInvoicesPartial();
        }

        [TestMethod]
        public void BuyerInvoiceItem()
        {
            runnerAccounting.TestBuyerInvoiceItem();
        }

        [TestMethod]
        public void AffiliateInvoiceItem()
        {
            runnerAccounting.TestAffiliateInvoiceItem();
        }

        [TestMethod]
        public void GenerateInvoices()
        {
            runnerAccounting.TestGenerateInvoices();
        }

        [TestMethod]
        public void GenerateInvoiceAjax()
        {
            runnerAccounting.TestGenerateInvoiceAjax();
        }

        [TestMethod]
        public void RefundedLeads()
        {
            runnerAccounting.TestRefundedLeads();
        }

        [TestMethod]
        public void GetRefundedLeads()
        {
            runnerAccounting.TestGetRefundedLeads();
        }

        [TestMethod]
        public void BuyerPayments()
        {
            runnerAccounting.TestBuyerPayments();
        }

        [TestMethod]
        public void BuyersBalance()
        {
            runnerAccounting.TestBuyersBalance();
        }

        [TestMethod]
        public void ChangeRefundedStatus()
        {
            runnerAccounting.TestChangeRefundedStatus();
        }

        [TestMethod]
        public void GetBuyerDistrib()
        {
            runnerAccounting.TestGetBuyerDistrib();
        }

        [TestMethod]
        public void AddBuyerPayment()
        {
            runnerAccounting.TestAddBuyerPayment();
        }

        [TestMethod]
        public void DeleteBuyerPayment()
        {
            runnerAccounting.TestDeleteBuyerPayment();
        }

        [TestMethod]
        public void ComplexTestAddAndDeleteBuyerPayment()
        {
            runnerAccounting.ComplexTestAddAndDeleteBuyerPayment();
        }

        [TestMethod]
        public void CreateBulkInvoiceAffiliate()
        {
            runnerAccounting.CreateBulkInvoiceAffiliate();
        }

        [TestMethod]
        public void CreateBulkInvoiceBuyer()
        {
            runnerAccounting.CreateBulkInvoiceBuyer();
        }

        [TestMethod]
        public void DeleteAffiliateInvoiceAdjustment()
        {
            runnerAccounting.DeleteAffiliateInvoiceAdjustment();
        }

        [TestMethod]
        public void DeleteBuyerInvoiceAdjustment()
        {
            runnerAccounting.DeleteBuyerInvoiceAdjustment();
        }

        [TestMethod]
        public void AddAffiliateInvoiceAdjustment()
        {
            runnerAccounting.AddAffiliateInvoiceAdjustment();
        }

        [TestMethod]
        public void AddBuyerInvoiceAdjustment()
        {
            runnerAccounting.AddBuyerInvoiceAdjustment();
        }

        [TestMethod]
        public void GetAffiliatesBalanceAjax()
        {
            runnerAccounting.GetAffiliatesBalanceAjax();
        }

        [TestMethod]
        public void GetBuyersBalanceAjax()
        {
            runnerAccounting.GetBuyersBalanceAjax();
        }

        [TestMethod]
        public void AffiliatesBalance()
        {
            runnerAccounting.AffiliatesBalance();
        }

        [TestMethod]
        public void GenerateCustomInvoices()
        {
            runnerAccounting.GenerateCustomInvoices();
        }

        [TestMethod]
        public void AddRefundLeads()
        {
            runnerAccounting.AddRefundLeads();
        }

        /// Support ///////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////
        [TestMethod]
        public void SupportIndex()
        {
            runnerSupport.TestSupportIndex();
        }

        [TestMethod]
        public void GetSupportTickets()
        {
            runnerSupport.TestGetSupportTickets();
        }

        [TestMethod]
        public void GetSupportTicketsMessages()
        {
            runnerSupport.TestGetSupportTicketsMessages();
        }

        [TestMethod]
        public void SupportItem()
        {
            runnerSupport.TestSupportItem();
        }

        [TestMethod]
        public void ChangeTicketsStatus()
        {
            runnerSupport.TestChangeTicketsStatus();
        }

        [TestMethod]
        public void AddTicketsMessages()
        {
            runnerSupport.TestAddTicketsMessages();
        }

        //Campaign
        [TestMethod]
        public void GetCampaignFields()
        {
            runnerCampaign.TestGetCampaignFields();
        }

        [TestMethod]
        public void GetCampaigns()
        {
            runnerCampaign.TestGetCampaigns();
        }

        [TestMethod]
        public void GetCampaignsByVerticalId()
        {
            runnerCampaign.TestGetCampaignsByVerticalId();
        }

        [TestMethod]
        public void GetCampaignTemplates()
        {
            runnerCampaign.TestGetCampaignTemplates();
        }

        [TestMethod]
        public void GetCampaignTemplatesByVerticalId()
        {
            runnerCampaign.TestGetCampaignTemplatesByVerticalId();
        }

        [TestMethod]
        public void InsertDeleteCampaign()
        {
            runnerCampaign.TestInsertDelete();
        }

        [TestMethod]
        public void LoadCampaignTemplate()
        {
            runnerCampaign.TestLoadCampaignTemplate();
        }

        /// Black list
        [TestMethod]
        public void GetBlackListTypes()
        {
            runnerBlacklist.TestGetBlackListTypes();
        }

        [TestMethod]
        public void GetBlackListValues()
        {
            runnerBlacklist.TestGetBlackListValues();
        }

        [TestMethod]
        public void GetCustomBlackListValues()
        {
            runnerBlacklist.TestGetCustomBlackListValues();
        }

        [TestMethod]
        public void InsertDeleteBlacklist()
        {
            runnerBlacklist.TestInsertDelete();
        }

        [TestMethod]
        public void RemoveBlackListValue()
        {
            runnerBlacklist.TestRemoveBlackListValue();
        }

        [TestMethod]
        public void RemoveCustomBlackListValue()
        {
            runnerBlacklist.TestRemoveCustomBlackListValue();
        }

        /// Department
        [TestMethod]
        public void GetDepartments()
        {
            runnerDepartment.TestGetDepartments();
        }

        [TestMethod]
        public void InsertDeleteDepartment()
        {
            runnerDepartment.TestInsertDelete();
        }

        /// Role
        [TestMethod]
        public void GetRoles()
        {
            runnerRole.TestGetRoles();
        }

        [TestMethod]
        public void GetUsers()
        {
            runnerRole.TestGetUsers();
        }

        [TestMethod]
        public void InsertDeleteRole()
        {
            runnerDepartment.TestInsertDelete();
        }
#endif // DEBUG
    }
}