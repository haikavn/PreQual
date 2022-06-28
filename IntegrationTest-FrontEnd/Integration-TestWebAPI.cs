using System;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest_FrontEnd
{
    [TestClass]
    public class UnitTestWeb
    {
        private static readonly string TestServer = "http://localhost:7457/";

        private string GetWebUrl(string url, string parameters)
        {
            var dataBytes = Encoding.UTF8.GetBytes(parameters);

            var request = (HttpWebRequest) WebRequest.Create(url + "?" + parameters);

            request.Timeout = 100000;
            request.Method = "POST";
            request.ContentType = "text/xml";
            request.ContentLength = dataBytes.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(dataBytes, 0, dataBytes.Length);
                stream.Close();
            }

            var response = (HttpWebResponse) request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }

#if DEBUG
        [TestMethod]
        public void WEB_GetLeadsAjax()
        {
            var res = GetWebUrl(TestServer + "Management/Lead/GetLeadsAjax/",
                "dates=01/01/2016:01/01/2017&page=1&pagesize=50");

            Console.WriteLine("Execution of WEB_GetLeadsAjax");

            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        [TestMethod]
        public void WEB_GetLeadByIdAjax()
        {
            var res = GetWebUrl(TestServer + "Management/Lead/GetLeadByIdAjax/", "leadid=33100");

            Console.WriteLine("Execution of WEB_GetLeadByIdAjax");

            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        [TestMethod]
        public void WEB_GetBadIPClicksReportAjax()
        {
            var res = GetWebUrl(TestServer + "Management/Lead/GetBadIPClicksReportAjax/",
                "dates=01/01/2016:01/01/2017");

            Console.WriteLine("Execution of WEB_GetBadIPClicksReportAjax");

            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        [TestMethod]
        public void WEB_GetErrorLeadsReportAjax()
        {
            var res = GetWebUrl(TestServer + "Management/Lead/GetErrorLeadsReportAjax",
                "dates=01/01/2016:01/01/2017&error=0");

            Console.WriteLine("Execution of WEB_GetErrorLeadsReportAjax");

            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        [TestMethod]
        public void WEB_GetLeadNotesAjax()
        {
            var res = GetWebUrl(TestServer + "Management/Lead/GetLeadNotesAjax", "leadid=33100");

            Console.WriteLine("Execution of WEB_GetLeadNotesAjax");

            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        [TestMethod]
        public void WEB_GetBuyers()
        {
            var res = GetWebUrl(TestServer + "GetBuyers", "");

            Console.WriteLine("Execution of WEB_GetBuyers");

            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        [TestMethod]
        public void WEB_GetAffiliates()
        {
            var res = GetWebUrl(TestServer + "GetAffiliates", "");

            Console.WriteLine("Execution of WEB_GetAffiliates");

            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        [TestMethod]
        public void WEB_GetBuyerInvoices()
        {
            var res = GetWebUrl(TestServer + "Management/Accounting/GetBuyerInvoices", "pagesize=10");

            Console.WriteLine("Execution of WEB_GetBuyerInvoices");

            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        [TestMethod]
        public void WEB_GetAffiliateInvoices()
        {
            var res = GetWebUrl(TestServer + "Management/Accounting/GetAffiliateInvoices", "pagesize=10");

            Console.WriteLine("Execution of WEB_GetAffiliateInvoices");

            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        [TestMethod]
        public void WEB_GetTimezoneNow()
        {
            var res = GetWebUrl(TestServer + "Management/common/GetTimezoneNow", "");

            Console.WriteLine("Execution of WEB_GetTimezoneNow");

            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        [TestMethod]
        public void WEB_GetBuyerPayments()
        {
            var res = GetWebUrl(TestServer + "Management/Accounting/GetBuyerPayments", "");

            Console.WriteLine("Execution of WEB_GetBuyerPayments");

            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        [TestMethod]
        public void WEB_GetRefundedLeads()
        {
            var res = GetWebUrl(TestServer + "Management/Accounting/GetRefundedLeads", "pagesize=10");

            Console.WriteLine("Execution of WEB_GetRefundedLeads");

            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        [TestMethod]
        public void WEB_GetBuyerReportByBuyerChannel()
        {
            var res = GetWebUrl(TestServer + "Management/Report/GetBuyerReportByBuyerChannel", "pagesize=10");

            Console.WriteLine("Execution of WEB_GetBuyerReportByBuyerChannel");

            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        [TestMethod]
        public void WEB_GetReportBuyersByDates()
        {
            var res = GetWebUrl(TestServer + "Management/report/GetReportBuyersByDates", "pagesize=10");

            Console.WriteLine("Execution of WEB_GetReportBuyersByDates");

            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        [TestMethod]
        public void WEB_GetReportBuyersByAffiliateChannels()
        {
            var res = GetWebUrl(TestServer + "Management/report/GetReportBuyersByAffiliateChannels",
                "pagesize=10");

            Console.WriteLine("Execution of WEB_GetReportBuyersByAffiliateChannels");

            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        [TestMethod]
        public void WEB_GetReportBuyersByStates()
        {
            var res = GetWebUrl(TestServer + "Management/report/GetReportBuyersByStates", "pagesize=10");

            Console.WriteLine("Execution of WEB_GetReportBuyersByStates");

            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        [TestMethod]
        public void WEB_GetReportBuyersByLeadNotes()
        {
            var res = GetWebUrl(TestServer + "Management/report/GetReportBuyersByLeadNotes", "pagesize=10");

            Console.WriteLine("Execution of WEB_GetReportBuyersByLeadNotes");

            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }

        [TestMethod]
        public void WEB_GetReportBuyersByReactionTime()
        {
            var res = GetWebUrl(TestServer + "Management/report/GetReportBuyersByReactionTime", "pagesize=10");

            Console.WriteLine("Execution of WEB_GetReportBuyersByReactionTime");

            if (res == null) throw new Exception("Returned null");

            Console.WriteLine("Success");
        }
#endif // DEBUG
    }
}