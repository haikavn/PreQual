using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core;
using Adrack.Core.Domain.Lead;

namespace Adrack.WebApi.Models.AffiliateChannel
{
    public class AffiliateChannelPostingDetailsReturnModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string RequestFormat { get; set; }
        public string ChannelId { get; set; }
        public string Password { get; set; }
        public string ResponseFormat { get; set; }
        public string ResponseEncoding { get; set; }
        public string RequestService { get; set; }
        public string PostSpecificationUrl { get; set; }
        public string TechContact { get; set; }
        public List<CodeSamples> CodeSamples { get; set; } = new List<CodeSamples>();
        
        public TestingExamples TestingExamples { get; set; } = new TestingExamples();

        public TestRequest TestRequest { get; set; } = new TestRequest();

    }

    public class CodeSamples
    {
        public string TabName { get; set; }
        public string CodeSample { get; set; }
        
    }

    public class TestingExamples
    {
        public string LeadImportXMLExample { get; set; }
        public List<RequestFields> RequestFields { get; set; } = new List<RequestFields>();
        public List<CodeSamples> ResponseExamples { get; set; } = new List<CodeSamples>();
    }

    public class TestRequest
    {
        public string XML { get; set; }
        public string Response { get; set; }
    }

    public class RequestFields
    {
        public string Field { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Format { get; set; }
        public string Status { get; set; }
    }
}