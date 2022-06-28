using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Controllers.ReportModels
{
    [Serializable]
    public class TopBarChartsModel
    {
     
            public List<BarchartInformationRecord> Records = new List<BarchartInformationRecord>();
            public BarchartInformationRecord Summary;
            public List<string> Fields;
            public List<dynamic> Series;

            public TopBarChartsModel()
            {
                Fields = new List<string>();
                Fields.Add("Buyers");
                Fields.Add("Affiliates");
                Fields.Add("BuyerChannels");
                //Fields.Add("Sold");
                //Fields.Add("Revenue");
                //Fields.Add("Cost");
                //Fields.Add("Profit");
            }
            public void CalculateSummary()
            {
                Summary = new BarchartInformationRecord();

                var startItem = Records.OrderBy(x => x.dateStart).FirstOrDefault();
                var endItem = Records.OrderByDescending(x => x.dateEnd).FirstOrDefault();

                Summary.dateStart = startItem != null ? startItem.dateStart : DateTime.UtcNow;
                Summary.dateEnd = endItem != null ? endItem.dateEnd : DateTime.UtcNow;
                foreach (var lead in Records)
                {
                    //Summary.Affiliates += lead.Affiliates;
                    //Summary.Buyers += lead.Buyers;
                    //Summary.Profit += lead.Profit;
                    //Summary.Recieved += lead.Recieved;
                    //Summary.Revenue += lead.Revenue;
                    //Summary.Sold += lead.Sold;

                }
            }
            public List<dynamic> GetSeries()
            {
                Hashtable series = new Hashtable();
                List<dynamic> output = new List<dynamic>();
                foreach (var lead in Records)
                {
                    Type myType = typeof(BarchartInformationRecord);

                    // Get the fields of the specified class.

                    for (int i = 0; i < this.Fields.Count; i++)
                    {
                        // Determine whether or not each field is a special name.
                        var field = myType.GetField(this.Fields[i]);
                        dynamic serie = series[this.Fields[i]];
                        var value = field.GetValue(lead);
                        var label = lead.label;
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
