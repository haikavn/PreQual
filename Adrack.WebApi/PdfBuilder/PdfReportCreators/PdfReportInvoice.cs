using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Lead.Reports;
using Adrack.PlanManagement;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.WebApi.Models.Buyers;
using Adrack.WebApi.PdfBuilder.HighCharts;
using Newtonsoft.Json;
using PdfSharp.Drawing;
using Swashbuckle.Swagger;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace Adrack.WebApi.PdfBuilder.PdfReportCreators
{
    public class PdfReportInvoiceContact
    {
        public string AdressLine1 = "589 Main St,STE 400";
        public string AdressLine2 = "Los Angeles CA,90028";
        public string Country = "United States";
        public string BuyerName = "PayDay America";

        public long CustomerID;
        public long InvoiceNumber;
        public DateTime InvoiceDate=DateTime.Now;
        public DateTime InvoicePeriodStart = DateTime.Now.AddMonths(-1);
        public DateTime InvoicePeriodEnd= DateTime.Now;

        public string CurrentPlanType = "Professional";
        public double PlanRate = 0.0;
        public double PingLimitation = 0.0;
        public double AveragePricePerPing = 0.0;
        public double PingsIncluded = 0.0;
        public double PingsProcessed = 0.0;
        public double AdditionalPings = 0.0;
        public double SubscriptionFeesAdditionalPings = 0;
        public double SubscriptionFees = 0.0;
        public string Last4Digits = "1234";
        public double Subtotal = 0.0;
        public double SalesTax = 0.0;
        public double AmountPaid = 0.0;
        public double TotalDue = 0.0;


        public string ApplyToTemplate(string template)
        {
            template = template.Replace("@buyername", this.BuyerName);
            template = template.Replace("@address1", this.AdressLine1);
            template = template.Replace("@address2", this.AdressLine2);
            template = template.Replace("@country", this.Country);
            template = template.Replace("@customerid", this.CustomerID.ToString());
            template = template.Replace("@invoicenum", this.InvoiceNumber.ToString());
            template = template.Replace("@invoicedate", this.InvoiceDate.ToString("MM/dd/yyyy"));
            template = template.Replace("@invoiceperiodstart", this.InvoicePeriodStart.ToString("MM/dd/yyyy"));
            template = template.Replace("@invoiceperiodend", this.InvoicePeriodEnd.ToString("MM/dd/yyyy"));
            template = template.Replace("@CurrentPlanType", this.CurrentPlanType);
            template = template.Replace("@PlanRate", this.PlanRate.ToString("0.##"));
            template = template.Replace("@PingLimitation", this.PingLimitation.ToString());
            template = template.Replace("@AveragePricePerPing", String.Format("{0:0.00}", this.AveragePricePerPing));
            template = template.Replace("@PingsIncluded", String.Format("{0:0.00}", this.PingsIncluded));
            template = template.Replace("@PingsProcessed", String.Format("{0:0.00}",this.PingsProcessed));
            template = template.Replace("@AdditionalPings", String.Format("{0:0.00}", this.AdditionalPings));
            template = template.Replace("@SubscriptionFeesAdditionalPings", this.AdditionalPings.ToString("0.###"));
            template = template.Replace("@SubscriptionFees", String.Format("{0:0.00}", this.PlanRate));
            template = template.Replace("@last4digits", String.Format("{0:0.00}", this.Last4Digits));
            template = template.Replace("@Subtotal", String.Format("{0:0.00}", this.Subtotal));
            template = template.Replace("@SalesTax", String.Format("{0:0.00}", this.SalesTax));
            template = template.Replace("@AmountPaid", String.Format("{0:0.00}",this.AmountPaid));
            template = template.Replace("@TotalDue", String.Format("{0:0.00}",this.TotalDue));
            template = template.Replace("@subscriber", this.BuyerName);





            return template;
        }

    }
    public class PdfReportInvoice : PdfReportCreator
    {

        public bool Simulated = false;

        public PdfReportInvoiceContact InvoiceInfo;

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly IReportService _reportService;

        private readonly IBuyerService _buyerService;

        //ArmanB call this function with BuyerId
        public void FillBuyerContactInformation(long buyerId)
        {
            Buyer buyer = _buyerService.GetBuyerById(buyerId);

            InvoiceInfo = new PdfReportInvoiceContact();
        }

        AdrackManagementPlan plan;


        public PdfReportInvoice(IPlanService planService, IReportService reportService, IBuyerService buyerService, string _folder = null) : base(reportService, _folder)
        {
            _reportService = reportService;
            _buyerService = buyerService;
            InvoiceInfo = new PdfReportInvoiceContact();
            
            /*
            var plans = planService.GetAllPlans();
            var json = plans[0].Object;
            plan = JsonConvert.DeserializeObject<AdrackManagementPlan>(json);

            if (plan.PricePlan.PaymentPeriod == EnumPaymentPeriod.Month)
                InvoiceInfo.InvoicePeriodStart = DateTime.Now.AddMonths(-1);

            if (plan.PricePlan.PaymentPeriod == EnumPaymentPeriod.Year)
                InvoiceInfo.InvoicePeriodStart = DateTime.Now.AddYears(-1);

            if (plan.PricePlan.PaymentPeriod == EnumPaymentPeriod.TwoYears)
                InvoiceInfo.InvoicePeriodStart = DateTime.Now.AddYears(-2);

            InvoiceInfo.InvoicePeriodEnd = DateTime.Now;

            var total = _reportService.GetRemainingsByPeriod(InvoiceInfo.InvoicePeriodStart, InvoiceInfo.InvoicePeriodEnd);
            
            InvoiceInfo.PingsProcessed = total.TotalPings;
            
            InvoiceInfo.PingLimitation = plan.PingLimits;
            InvoiceInfo.PingsIncluded = total.TotalPings;
            InvoiceInfo.AdditionalPings = 0;
            
            if (total.TotalPings> plan.PingLimits)
                InvoiceInfo.AdditionalPings=total.TotalPings - plan.PingLimits;

            InvoiceInfo.PlanRate = plan.PricePlan.Price;
            
            InvoiceInfo.Subtotal= InvoiceInfo.PlanRate-Math.Round(InvoiceInfo.PlanRate /100*20);
            InvoiceInfo.SalesTax = Math.Round(InvoiceInfo.PlanRate / 100 * 20);
            InvoiceInfo.AmountPaid = plan.PricePlan.Price;
            InvoiceInfo.TotalDue= plan.PricePlan.Price;

            InvoiceInfo.AveragePricePerPing = InvoiceInfo.PlanRate / total.TotalPings;
            */
            this.pdf_template = baseDirectory + "/PdfBuilder/Templates/InvoiceTemplate/template.html";
        }

        public async override Task GenerateReport(string fileName)
        {
            PdfGenerateConfig config = new PdfGenerateConfig();
            config.SetMargins(0);
            config.ManualPageSize = new XSize(842.0, 842.0*297.0/210.0);

            var path = Path.GetDirectoryName(pdf_template);         

            var template = File.ReadAllText(pdf_template);
            template = template.Replace("@path", path);
            template= InvoiceInfo.ApplyToTemplate(template);

            PdfSharp.Pdf.PdfDocument doc = new PdfSharp.Pdf.PdfDocument();     

            PdfGenerator.SplitHtmlIntoPagedPdf(template, "<!-- pagemarker -->", config, doc);

            doc.Save(this.output_folder + "/" + fileName);

            OnGenerationComplete(new EventArgs());
        }

        public async override Task GenerateInvoice(string fileName, PdfReportInvoiceContact InvoiceData)
        {
            /*
            InvoiceInfo.AdressLine1 = "aaaaaaaaaaa";
            InvoiceInfo.AdditionalPings = InvoiceData.AdditionalPings;
            InvoiceInfo.AdressLine1 = InvoiceData.AdressLine1;
            InvoiceInfo.AdressLine2 = InvoiceData.AdressLine2;
            InvoiceInfo.AmountPaid = InvoiceData.AmountPaid;
            InvoiceInfo.AveragePricePerPing = InvoiceData.AveragePricePerPing;
            InvoiceInfo.BuyerName = InvoiceData.BuyerName;
            InvoiceInfo.Country = InvoiceData.Country;
            InvoiceInfo.CurrentPlanType = InvoiceData.CurrentPlanType;
            InvoiceInfo.CustomerID = InvoiceData.CustomerID;
            */
            InvoiceInfo = InvoiceData;


            PdfGenerateConfig config = new PdfGenerateConfig();
            config.SetMargins(0);
            config.ManualPageSize = new XSize(842.0, 842.0 * 297.0 / 210.0);

            var path = Path.GetDirectoryName(pdf_template);

            var template = File.ReadAllText(pdf_template);
            template = template.Replace("@path", path);
            template = InvoiceInfo.ApplyToTemplate(template);

            PdfSharp.Pdf.PdfDocument doc = new PdfSharp.Pdf.PdfDocument();

            PdfGenerator.SplitHtmlIntoPagedPdf(template, "<!-- pagemarker -->", config, doc);

            doc.Save(fileName);

            OnGenerationComplete(new EventArgs());
        }

        public async override Task GenerateAddonInvoice(string fileName, PdfReportInvoiceContact InvoiceData)
        {
            InvoiceInfo = InvoiceData;

            PdfGenerateConfig config = new PdfGenerateConfig();
            config.SetMargins(0);
            config.ManualPageSize = new XSize(842.0, 842.0 * 297.0 / 210.0);

            this.pdf_template = baseDirectory + "/PdfBuilder/Templates/InvoiceTemplateAddon/template.html";

            var path = Path.GetDirectoryName(pdf_template);

            var template = File.ReadAllText(pdf_template);
            template = template.Replace("@path", path);
            template = InvoiceInfo.ApplyToTemplate(template);

            PdfSharp.Pdf.PdfDocument doc = new PdfSharp.Pdf.PdfDocument();

            PdfGenerator.SplitHtmlIntoPagedPdf(template, "<!-- pagemarker -->", config, doc);

            doc.Save(fileName);

            OnGenerationComplete(new EventArgs());
        }
    }
}