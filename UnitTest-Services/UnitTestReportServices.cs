using Adrack.Core;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest_Services
{
    [TestClass]
    public class UnitTestReportServices
    {
        private static ServicesTestReportController runner;

        public UnitTestReportServices()
        {
            InitTestingUser();

            if (runner == null)
                runner = new ServicesTestReportController(this);
        }

        public IAppContext AppContext { get; set; }

        public void InitTestingUser()
        {
            AppContext = AppEngineContext.Current.Resolve<IAppContext>();
        }

#if DEBUG
        [TestMethod]
        public void TestGetBuyerReportByBuyerChannel()
        {
            runner.TestGetBuyerReportByBuyerChannel();
        }

        [TestMethod]
        public void TestGetReportAffiliatesByAffiliateChannels()
        {
            runner.TestGetReportAffiliatesByAffiliateChannels();
        }

        [TestMethod]
        public void TestGetReportAffiliatesByCampaigns()
        {
            runner.TestGetReportAffiliatesByCampaigns();
        }

        [TestMethod]
        public void TestGetReportAffiliatesByStates()
        {
            runner.TestGetReportAffiliatesByStates();
        }

        [TestMethod]
        public void TestGetReportBuyersByAffiliateChannels()
        {
            runner.TestGetReportBuyersByAffiliateChannels();
        }

        [TestMethod]
        public void TestGetReportBuyersByCampaigns()
        {
            runner.TestGetReportBuyersByCampaigns();
        }

        [TestMethod]
        public void TestGetReportBuyersByDates()
        {
            runner.TestGetReportBuyersByDates();
        }

        [TestMethod]
        public void TestGetReportBuyersByLeadNotes()
        {
            runner.TestGetReportBuyersByLeadNotes();
        }

        [TestMethod]
        public void TestGetReportBuyersByReactionTime()
        {
            runner.TestGetReportBuyersByReactionTime();
        }

        [TestMethod]
        public void TestGetReportBuyersByStates()
        {
            runner.TestGetReportBuyersByStates();
        }

        [TestMethod]
        public void TestGetReportByDays()
        {
            runner.TestGetReportByDays();
        }

        [TestMethod]
        public void TestGetReportByHour()
        {
            runner.TestGetReportByHour();
        }

        [TestMethod]
        public void TestGetReportByMinutes()
        {
            runner.TestGetReportByMinutes();
        }

        [TestMethod]
        public void TestGetReportByYear()
        {
            runner.TestGetReportByYear();
        }

        [TestMethod]
        public void TestGetReportTotals()
        {
            runner.TestGetReportTotals();
        }

        [TestMethod]
        public void TestGetReportTotalsBuyer()
        {
            runner.TestGetReportTotalsBuyer();
        }
#endif // DEBUG
    }
}