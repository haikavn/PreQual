using Adrack.Service.Lead;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Adrack.WebApi.PdfBuilder.PdfReportCreators
{
    public abstract class PdfReportCreator
    {

        protected string baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

        public event EventHandler GenerationComplete;

        protected virtual void OnGenerationComplete(EventArgs e)
        {
            EventHandler handler = GenerationComplete;
            handler?.Invoke(this, e);
        }

        protected IReportService report_service;
        protected string output_folder = "";

        protected string pdf_template = "";

        public PdfReportCreator()
        {

        }
        public PdfReportCreator(IReportService _service, string _folder = null)
        {

            this.report_service = _service;           

            if (_folder == null)
                output_folder = baseDirectory + "Content\\Uploads\\Pdf";
            else
                output_folder = _folder;
        }

        public abstract Task GenerateReport(string fileName);
        public abstract Task GenerateInvoice(string fileName, PdfReportInvoiceContact InvoiceData);
        
        public abstract Task GenerateAddonInvoice(string fileName, PdfReportInvoiceContact InvoiceData);
        
    }
}