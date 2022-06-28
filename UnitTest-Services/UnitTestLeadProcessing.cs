using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest_Services
{
    [TestClass]
    public class UnitTestLeadProcessing
    {
        private static ServicesTestLeadProcessing runner;

        public UnitTestLeadProcessing()
        {
            TestSettings = new TestSettings();
            TestSettings.Load("", "LeadSample.xml");

            if (runner == null)
                runner = new ServicesTestLeadProcessing(this);
        }

        public TestSettings TestSettings { get; set; }

#if DEBUG
        [TestMethod]
        public void LeadProcessingTest2()
        {
            runner.ProcessData();
        }

        [TestMethod]
        public void TestXMLToQueryString()
        {
            runner.XmlToQueryString();
        }

        [TestMethod]
        public void LeadProcessingTest()
        {
            runner.ProcessData();
        }

        [TestMethod]
        public void ValidateBuyerChannelScheduleTest()
        {
            runner.ValidateBuyerChannelSchedule();
        }

        [TestMethod]
        public void PingTreeLeadCountTest()
        {
            runner.PingTreeLeadCount();
        }

        [TestMethod]
        public void ValidateDoNotPresentTest()
        {
            runner.ValidateDoNotPresent();
        }

        [TestMethod]
        public void CheckSubIdWhiteListTest()
        {
            runner.CheckSubIdWhiteList();
        }
#endif // DEBUG
    }
}