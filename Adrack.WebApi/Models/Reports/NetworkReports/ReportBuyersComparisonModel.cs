using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.NetworkReports
{
    public class ReportBuyersComparisonSectionModel
    {
        public int Value1 { get; set; }

        public int Value2 { get; set; }

        public int Value3 { get; set; }

        public ReportBuyersComparisionPercentModel Percent1 { get; set; } = new ReportBuyersComparisionPercentModel();
        public ReportBuyersComparisionPercentModel Percent2 { get; set; } = new ReportBuyersComparisionPercentModel();

        public ReportBuyersComparisionPercentModel Percent3 { get; set; } = new ReportBuyersComparisionPercentModel();

    }

    public class ReportBuyersComparisonRevenueSectionModel
    {
        public decimal Value1 { get; set; }

        public decimal Value2 { get; set; }

        public decimal Value3 { get; set; }

        public ReportBuyersComparisionPercentModel Percent1 { get; set; } = new ReportBuyersComparisionPercentModel();
        public ReportBuyersComparisionPercentModel Percent2 { get; set; } = new ReportBuyersComparisionPercentModel();

        public ReportBuyersComparisionPercentModel Percent3 { get; set; } = new ReportBuyersComparisionPercentModel();
    }

    public class ReportBuyersComparisionPercentModel
    {
        public double Percent { get; set; } = 0;

        public short Direction { get; set; } = 0;
    }

    public class ReportBuyersComparisonRowModel
    {
        public string Name { get; set; }
        public ReportBuyersComparisonSectionModel PostedLeads { get; set; }

        public ReportBuyersComparisonSectionModel SoldLeads { get; set; }

        public ReportBuyersComparisonSectionModel RejectedLeads { get; set; }

        public ReportBuyersComparisonSectionModel RedirectedLeads { get; set; }

        public ReportBuyersComparisonRevenueSectionModel Revenue { get; set; }

        public ReportBuyersComparisonRowModel()
        {
                PostedLeads = new ReportBuyersComparisonSectionModel();
                SoldLeads = new ReportBuyersComparisonSectionModel();
                RejectedLeads = new ReportBuyersComparisonSectionModel();
                RedirectedLeads = new ReportBuyersComparisonSectionModel();
                Revenue = new ReportBuyersComparisonRevenueSectionModel();
        }

    }

    public class ReportBuyersComparisonModel
    {
        public List<ReportBuyersComparisonRowModel> Rows { get; set; }

        public ReportBuyersComparisonModel()
        {
            Rows = new List<ReportBuyersComparisonRowModel>();
        }
    }
}