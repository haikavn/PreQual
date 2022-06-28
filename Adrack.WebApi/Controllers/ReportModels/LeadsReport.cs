using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Web;

namespace Adrack.WebApi.Controllers.ReportModels
{
    [Serializable]
    public class LeadsReport
    {

        private static readonly DateTime Jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>Get extra long current timestamp</summary>
        

        public LeadInformationRecord Summary=new LeadInformationRecord();

        public List<LeadInformationRecord> Leads = new List<LeadInformationRecord>();

        public List<dynamic> Series;
        public void CalculateSummary()
        {
            Summary = new LeadInformationRecord();
            if (Leads.Count > 0)
            {
                Summary.DateStart = Leads[0].DateStart;
                Summary.DateEnd = Leads[0].DateEnd;
            }
            else
            {
                Summary.DateStart = Summary.DateEnd = DateTime.UtcNow;
            }
            foreach (var lead in Leads)
            {
                Summary.Cost += lead.Cost;
                Summary.Posted += lead.Posted;
                Summary.Profit += lead.Profit;
                Summary.Recieved += lead.Recieved;
                Summary.Revenue += lead.Revenue;
                Summary.Sold += lead.Sold;
                
            }
        }
        public List<dynamic>  GetSeries()
        {
            Hashtable series = new Hashtable();
            List<dynamic> output = new List<dynamic>();
            foreach (var lead in Leads)
            {
                Type myType = typeof(LeadInformationRecord);

                // Get the fields of the specified class.
                FieldInfo[] fields = myType.GetFields();
                for (int i = 0; i < fields.Length; i++)
                {
                    // Determine whether or not each field is a special name.
                    if (fields[i].Name.Contains("Date")) continue;
                    if (fields[i].Name.Contains("Label")) continue;
                    dynamic serie = series[fields[i].Name];
                    var value=fields[i].GetValue(lead);
                    var date = (lead.DateStart- Jan1St1970).TotalMilliseconds;
                    if (serie==null)
                    {
                        serie = new ExpandoObject();
                        serie.data = new List<List<double>>();
                        series[fields[i].Name]=serie;
                    }

                    serie.name = fields[i].Name;
                    serie.color = "auto";
                    serie.type = "auto";
                    var values = new List<double>();
                    values.Add(date);
                    values.Add((double)value);
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