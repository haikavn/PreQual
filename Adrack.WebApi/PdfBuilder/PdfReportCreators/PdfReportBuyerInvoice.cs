using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Lead.Reports;
using Adrack.Service.Lead;
using Adrack.WebApi.Models.Buyers;
using Adrack.WebApi.PdfBuilder.HighCharts;
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

    public class BuyerChannelSourceData
    {
        public string Description;
        public long Count;
        public decimal Price;
        public decimal Amount;
    }

    
    public class PdfBuyerReportInvoiceContact
    {                

        public string AdressLine1 = "589 Main St,STE 400";        
        public string AdressLine2 = "Los Angeles CA,90028";
        public string CompanyName = "Adrack LLC";
        public string BuyerState = "LA";
        public string Country = "United States";
        
        public string BuyerName = "PayDay America";
        public string BuyerCountry = "United States";
        public string BuyerEMail = "test@test.com";
        public string BuyerSwift = "midlam";
        public string BuyerIBAN = "123456";
        public string BuyerPhone = "123456";
        public string BuyerAdressLine = "589 Main St,STE 400";


        public long InvoiceNumber;
        public DateTime InvoiceDate=DateTime.Now;
        public DateTime InvoicePeriodStart=DateTime.Now.AddMonths(-1);
        public DateTime InvoicePeriodEnd=DateTime.Now;

        

        public void InitFromBuyer(Buyer buyer)
        {
            this.BuyerName = buyer.Name;
            this.BuyerPhone = buyer.Phone;
            this.BuyerCountry = buyer.Country?.Name;
            this.BuyerEMail = buyer.Email;
            this.BuyerState = buyer.StateProvince?.Name;


        }
        public string ApplyToTemplate(string template)
        {
            template = template.Replace("@buyercompanyname", this.BuyerName);
            template = template.Replace("@buyeremail", this.BuyerEMail);
            template = template.Replace("@buyeriban", this.BuyerIBAN);
            template = template.Replace("@buyercountry", this.BuyerCountry);
            template = template.Replace("@buyerstate", this.BuyerState);
            

            template = template.Replace("@buyerswift", this.BuyerSwift);
            template = template.Replace("@buyerphone", this.BuyerPhone);
            template = template.Replace("@buyeraddress", this.BuyerAdressLine);
            
            template = template.Replace("@networkcompanyaddress1", this.AdressLine1);
            template = template.Replace("@networkcompanyaddress2", this.AdressLine2);
            template = template.Replace("@networkcompanyname", this.CompanyName.ToString());
            template = template.Replace("@networkcompanycountry", this.Country.ToString());


            template = template.Replace("@invoicenumber", this.InvoiceNumber.ToString());
            template = template.Replace("@invoicedate", this.InvoiceDate.ToString("MM/dd/yyyy"));
            template = template.Replace("@invoiceperiodstart", this.InvoicePeriodStart.ToString("MM/dd/yyyy"));
            template = template.Replace("@invoiceperiodend", this.InvoicePeriodEnd.ToString("MM/dd/yyyy"));

            return template;
        }



    }
    public class PdfReportBuyerInvoice : PdfReportCreator
    {

        public decimal TotalDue = 0;
        public bool Simulated = false;

        Buyer buyer;

        public PdfBuyerReportInvoiceContact InvoiceInfo;

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly IReportService _reportService;

        private readonly IBuyerChannelService _buyerChannelService;

        private readonly ILeadMainResponseService _leadMainResponseService;
        

        private readonly IBuyerService _buyerService;

        //ArmanB call this function with BuyerId
        public void FillBuyerContactInformation(long buyerId)
        {
            Buyer buyer = _buyerService.GetBuyerById(buyerId);

            InvoiceInfo = new PdfBuyerReportInvoiceContact();
        }


        public PdfReportBuyerInvoice(long buyerID, IReportService reportService, IBuyerService buyerService,
            ILeadMainResponseService leadMainResponseService,
            IBuyerChannelService buyerChannelService,
            string _folder = null) : base(reportService, _folder)
        {
            buyer = buyerService.GetBuyerById(buyerID);

            _leadMainResponseService = leadMainResponseService;
            _buyerChannelService = buyerChannelService;
            _reportService = reportService;
            
            _buyerService = buyerService;
            InvoiceInfo = new PdfBuyerReportInvoiceContact();

            this.pdf_template = baseDirectory + "/PdfBuilder/Templates/BuyerInvoiceTemplate/template.html";
        }
    
        
        private String ApplyBuyerChannelsListToTemplate(string template)
        {
            var channels = _buyerChannelService.GetAllBuyerChannels(buyer.Id);
            var channelMarker = template.Split(new string[] { "<!--channelmarker-->" }, StringSplitOptions.None)[1];
            var channelTable = template.Split(new string[] { "<!--channeltable-->" }, StringSplitOptions.None)[1];

            var channelTableHeader = template.Split(new string[] { "<!--channeltableheader-->" }, StringSplitOptions.None)[1];

            var channelList =channelTableHeader;
            decimal TotalDue = 0;
            var page = 0;
            var pageSize = 20;
            var cnt = 0;
            var totalCount = channels.Count;
            //for (var i = 0; i < 30; i++)
            
                foreach (var channel in channels)
                {
                    cnt++;
                    var row = channelMarker.Replace("@buyerchannelname", channel.Name);
                    short Status = -1;
                    var leadCount = _leadMainResponseService.GetLeadsCountByPeriod(channel.Id, DateTime.Now.AddYears(-10),
                        DateTime.Now, Status);


                    row = row.Replace("@buyerchannelleadcount", leadCount.ToString());
                    row = row.Replace("@buyerchannelleadprice", "$" + channel.BuyerPrice.ToString("N2"));

                    var data=_reportService.GetRemainingsByPeriod(DateTime.Now.AddYears(-1), DateTime.Now, channel.BuyerId, channel.Id);
                    row = row.Replace("@buyerchannelamount", data.TotalBuyerChannelSum.ToString("N2"));
                    TotalDue += data.TotalBuyerChannelSum;
                    channelList += row;

                    page++;
                    if (page>pageSize)
                    {
                        pageSize = 32;
                        page = 0;
                        channelList += "</table><!-- pagemarker -->";
                        if (cnt< totalCount)
                        channelList += channelTableHeader;
                    }
                    
                }

                
                
            

            if (page!=0 && page <= pageSize)
            {
                channelList += "</table>";                
            }

            template = template.Replace("@TotalDue", String.Format("{0:0.00}", TotalDue));
            return template.Replace(channelTable, channelList);

        }

        private String ApplyBuyerChannelsListToTemplateExternal(string template, List<BuyerChannelSourceData> source, string prefix, int firstPageSize=20)
        {
            var channels = source;

            var channelMarker = template.Split(new string[] { "<!--channelmarker"+ prefix + "-->" }, StringSplitOptions.None)[1];
            var channelTable = template.Split(new string[] { "<!--channeltable"+ prefix + "-->" }, StringSplitOptions.None)[1];

            var channelTableHeader = template.Split(new string[] { "<!--channeltableheader"+ prefix + "-->" }, StringSplitOptions.None)[1];

            var channelList = channelTableHeader;
            
            var page = 0;
            var pageSize = firstPageSize;
            var cnt = 0;
            var totalCount = channels.Count;
            //for (var i = 0; i < 30; i++)

            foreach (var channel in channels)
            {
                cnt++;
                var row = channelMarker.Replace("@buyerchannelname", channel.Description);
                short Status = -1;
                var leadCount = channel.Count;


                row = row.Replace("@buyerchannelleadcount", leadCount.ToString());
                row = row.Replace("@buyerchannelleadprice", "$" + channel.Price.ToString("N2"));

                var TotalBuyerChannelSum = channel.Amount;
                row = row.Replace("@buyerchannelamount", TotalBuyerChannelSum.ToString("N2"));
                TotalDue += TotalBuyerChannelSum;
                channelList += row;

                page++;
                if (page > pageSize)
                {
                    pageSize = 32;
                    page = 0;
                    channelList += "</table><!-- pagemarker -->";
                    if (cnt < totalCount)
                        channelList += channelTableHeader;
                }

            }





            if (page != 0 && page <= pageSize)
            {
                channelList += "</table>";
            }

            
            return template.Replace(channelTable, channelList);

        }

        List<BuyerChannelSourceData> _sourceDataReport;
        List<BuyerChannelSourceData> _sourceDataCustomAdjustment;

        public void SetSourceData(long buyerId, List<BuyerChannelSourceData> sourceDataReport,
            List<BuyerChannelSourceData> sourceDataCustomAdjustment)
        {
            buyer = _buyerService.GetBuyerById(buyerId);
            _sourceDataReport = sourceDataReport;
            _sourceDataCustomAdjustment = sourceDataCustomAdjustment;
        }

        public async override Task GenerateReport(string fileName)
        {
            PdfGenerateConfig config = new PdfGenerateConfig();
            config.SetMargins(0);
            config.ManualPageSize = new XSize(842.0, 842.0*297.0/210.0);

            var path = Path.GetDirectoryName(pdf_template);          
            
            var template = File.ReadAllText(pdf_template);
            template = template.Replace("@path", path);

            InvoiceInfo.InitFromBuyer(buyer);
            template = InvoiceInfo.ApplyToTemplate(template);

            PdfSharp.Pdf.PdfDocument doc = new PdfSharp.Pdf.PdfDocument();

            if (this._sourceDataReport != null)
            {
                
                template = ApplyBuyerChannelsListToTemplateExternal(template,_sourceDataCustomAdjustment,"",20);
                template = ApplyBuyerChannelsListToTemplateExternal(template, _sourceDataReport, "ca", 32);
                template = template.Replace("@TotalDue", String.Format("{0:0.00}", this.TotalDue));
            }
            else
                template = ApplyBuyerChannelsListToTemplate(template);

            PdfGenerator.SplitHtmlIntoPagedPdf(template, "<!-- pagemarker -->", config, doc);

            doc.Save(this.output_folder + "/" + fileName);
          
            OnGenerationComplete(new EventArgs());
        }

        public async override Task GenerateInvoice(string fileName, PdfReportInvoiceContact InvoiceData)
        {

        }
        public async override Task GenerateAddonInvoice(string fileName, PdfReportInvoiceContact InvoiceData)
        {

        }

        




    }
}