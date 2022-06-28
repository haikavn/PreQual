using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Adrack.WebApi.Controllers.ReportModels
{
    [Serializable]
    public class TopRecordsReport 
    {
        public List<LeadInformationRecord> Records=new List<LeadInformationRecord>();
        public LeadInformationRecord Summary;
        public List<string> Fields;
        public List<dynamic> Series;

        public TopRecordsReport()
        {
            Fields = new List<string>();
            Fields.Add("Recieved");
            Fields.Add("Posted");
            Fields.Add("Sold");
            Fields.Add("Revenue");
            Fields.Add("Cost");
            Fields.Add("Profit");
    }
        public void CalculateSummary()
        {
            Summary = new LeadInformationRecord();

            var startItem = Records.OrderBy(x => x.DateStart).FirstOrDefault();
            var endItem = Records.OrderByDescending(x => x.DateEnd).FirstOrDefault();

            Summary.DateStart = startItem != null ? startItem.DateStart : DateTime.UtcNow;
            Summary.DateEnd = endItem != null ? endItem.DateEnd : DateTime.UtcNow;
            foreach (var lead in Records)
            {
                Summary.Cost += lead.Cost;
                Summary.Posted += lead.Posted;
                Summary.Profit += lead.Profit;
                Summary.Recieved += lead.Recieved;
                Summary.Revenue += lead.Revenue;
                Summary.Sold += lead.Sold;

            }
        }
        public List<dynamic> GetSeries()
        {
            Hashtable series = new Hashtable();
            List<dynamic> output = new List<dynamic>();
            foreach (var lead in Records)
            {
                Type myType = typeof(LeadInformationRecord);

                // Get the fields of the specified class.
                
                for (int i = 0; i < this.Fields.Count; i++)
                {
                    // Determine whether or not each field is a special name.
                    var field=myType.GetField(this.Fields[i]);
                    dynamic serie = series[this.Fields[i]];
                    var value = field.GetValue(lead);
                    var label = lead.Label;
                    if (serie == null)
                    {
                        serie = new ExpandoObject();
                        serie.name = this.Fields[i];
                        serie.data = new List<List<object>>();
                        series[Fields[i]] = serie;
                    }

                    serie.name = Fields[i];
                    serie.color = "auto";
                    serie.type = "auto";
                    var values = new List<object>();
                    values.Add(label);
                    values.Add(value);
                    serie.data.Add(values);
                }
            }
            foreach (var key in series.Keys)
            {
                output.Add(series[key] as dynamic);
            }
            this.Series = output;
            return output;
        }
    }
}
