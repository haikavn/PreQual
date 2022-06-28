using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Buyers
{
    public class BuyerResponseModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public short TypeId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Manager { get; set; }
        public short Status { get; set; }
        public string IconPath { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public bool SendStatementReport { get; set; }
    }
}