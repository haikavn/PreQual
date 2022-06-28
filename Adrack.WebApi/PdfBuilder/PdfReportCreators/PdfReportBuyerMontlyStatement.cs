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


    public class PdfReportBuyerMontlyStatementContactInfo
    {
        public string ContactInformation = "Account Manager";
        public string ContactEmail = "support@adrack.com";
        public string ContactPhone = "1(123)123-1212";
        public string CompanyName = "Adrack Company";
        public string ContactPhotoUrl = "contactlogo.png";

        public string ApplyToTemplate(string template)
        {
            template = template.Replace("@contactname", this.ContactInformation);
            template = template.Replace("@contactmail", this.ContactEmail);
            template = template.Replace("@contactphone", this.ContactPhone);
            template = template.Replace("@companyname", this.CompanyName);
            template = template.Replace("@contactlogo", this.ContactPhotoUrl);
            return template;
        }

    }
    public class PdfReportBuyerMontlyStatement : PdfReportCreator
    {

        public bool Simulated = false;

        public PdfReportBuyerMontlyStatementContactInfo ContactInformation;

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly IReportService _reportService;

        private readonly IBuyerService _buyerService;

        //ArmanB call this function with BuyerId
        public void FillBuyerContactInformation(long buyerId)
        {
            Buyer buyer = _buyerService.GetBuyerById(buyerId);

            ContactInformation.CompanyName = buyer.Name;
            ContactInformation.ContactEmail = buyer.Email;
            ContactInformation.ContactPhone = buyer.Phone;
            ContactInformation.ContactPhotoUrl = buyer.IconPath;
            ContactInformation.ContactInformation = buyer.AddressLine1;
        }

        long buyerId;
        public PdfReportBuyerMontlyStatement(long buyerId,IReportService reportService, IBuyerService buyerService, string _folder = null) : base(reportService, _folder)
        {
            this.buyerId = buyerId;
            _reportService = reportService;
            _buyerService = buyerService;
            ContactInformation = new PdfReportBuyerMontlyStatementContactInfo();

            this.pdf_template = baseDirectory + "/PdfBuilder/Templates/BuyerMontlyReportTemplate/template.html";
        }
        dynamic GetSeriesOptions(string _type, List<string> _categories, DateTime beginOfCurrentMonth,
            List<int> dataCurrentMonth, List<int> dataPrevMonth)
        {
            var exportWidth = 800;
            var exportHeight = 400;

            var _exporting = new
            {
                sourceWidth = exportWidth,
                sourceHeight = exportHeight,
                scale = 4,
                chartOptions = new
                {
                    subtitle = ""
                }
            };
            var options = new
            {
                exporting = _exporting,
                legend = new
                {
                    align = "center",
                    verticalAlign = "top",
                    layout = "horizontal",
                    symbolHeight = 11,
                    symbolWidth = 11,
                    symbolRadius = 0
                },
                chart = new
                {
                    type = _type
                },
                credits = new
                {
                    enabled = false
                },
                title = new
                {
                    text = ""
                },
                plotOptions = new
                {
                    series = new
                    {
                        dataLabels = new
                        {
                            enabled = true
                        }
                    }
                },
                xAxis = new
                {
                    categories = _categories,
                    gridLineColor = "#EEEEEE",
                    gridLineWidth = 1,
                    tickmarkPlacement = "on",
                },
                yAxis = new
                {
                    gridLineColor = "#EEEEEE",
                    gridLineWidth = 0,
                    tickmarkPlacement = "on",
                },
                series = new[]
               {
                    new {
                        name=beginOfCurrentMonth.ToString("MMMM", CultureInfo.InvariantCulture),
                        color ="#0280B7",
                        data = dataCurrentMonth
                    },
                    new {
                        name=beginOfCurrentMonth.AddMonths(-1).ToString("MMMM", CultureInfo.InvariantCulture),
                        color="#99CF60",
                        data = dataPrevMonth
                    }
                }
            };
            return options;
        }
        
        private int GetCurrentMonthByPriceSoldCount(DateTime currentMonth, int price, string State)
        {
            //Hayk
            return new Random(price).Next(1, 50);
        }

        private int GetPreviousMonthByPriceSoldCount(DateTime previousMonth, int price, string State)
        {
            //Hayk
            return new Random(price * 10).Next(1, 50);
        }

        private string GenerateReportTable(string template, DateTime currentMonth,
            DateTime previousMonth,
            List<int> prices,
            List<string> top10States)
        {
            string header = "<th>State</th>";
            for (var i = 0; i < prices.Count; i++)
            {
                header += "<th>$" + prices[i] + "</th>";
            }

            var tableRows = "";
            template = template.Replace("@pricecategories", header);

            List<int> prevMonthValues = new List<int>();
            List<int> currentMonthValues = new List<int>();
            foreach (var state in top10States)
            {
                var row = "<tr class='tablerows'>";
                row += "<td class='tablecell'>" + state + "</td>";

                foreach (var price in prices)
                {
                    var valueCurrent = GetCurrentMonthByPriceSoldCount(currentMonth, price, state);
                    var valuePrevious = GetPreviousMonthByPriceSoldCount(previousMonth, price, state);
                    currentMonthValues.Add(valueCurrent);
                    prevMonthValues.Add(valuePrevious);
                    var change = (int)((double)valueCurrent / (double)valuePrevious * 100.0 - 100.0);

                    if (change > 0) //green arrow
                        row += @"<td class='tablecell'>
                        <sup class=""supstile"" style = ""color:green; font-weight:bold;""> &#8593;</sup>&nbsp;" + valueCurrent.ToString() + @"
                        <span class=""tablecellsub""><br>" + change.ToString() + @"%</span>
                        </td>";
                    else
                        row += @"<td class='tablecell'>
                        <sup class=""supstile"" style = ""color:red; font-weight:bold;""> &#8595;</sup>&nbsp;" + valueCurrent + @"
                        <span class=""tablecellsub""><br>" + change + @"%</span>
                        </td>";
                }

                row += "</tr>";
                tableRows += row;
            }

            template = template.Replace("@tablechange", GetChangeOfGraphs(currentMonthValues, prevMonthValues));
            template = template.Replace("@tablerows", tableRows);
            return template;
        }

        private string GenerateReportTable_Z(string template, DateTime currentMonth,
            DateTime previousMonth,
            List<int> prices,
            List<string> top10States)
        {
            List<int> prevMonthValues = new List<int>();
            List<int> currentMonthValues = new List<int>();


            var nowDate = DateTime.UtcNow;

            if (DateTime.UtcNow.Day < 28)
                nowDate = nowDate.AddMonths(-1);

            DateTime beginOfCurrentMonth = new DateTime(nowDate.Year, nowDate.Month, 1, 0, 0, 0);
            DateTime endOfCurrentMonth = beginOfCurrentMonth.AddMonths(1).AddDays(-1); //last day of month

            DateTime beginOfPreviousMonth = beginOfCurrentMonth.AddMonths(-1);
            DateTime endOfPreviousMonth = beginOfPreviousMonth.AddMonths(1).AddDays(-1);

            IList<ReportBuyersByPricesAndStates> currentPricesAndTopStates = this._reportService.ReportBuyersByPricesAndStates(beginOfCurrentMonth, endOfCurrentMonth, 0);
            IList<ReportBuyersByPricesAndStates> previousPricesAndTopStates = this._reportService.ReportBuyersByPricesAndStates(beginOfPreviousMonth, endOfPreviousMonth, 0);

            var currentStatesNames = currentPricesAndTopStates.Distinct();

            HashSet<int> pricesVals = new HashSet<int>();

            string header = "<th>State</th>";
            foreach (ReportBuyersByPricesAndStates p in currentStatesNames)
            {
                pricesVals.Add((int)p.BuyerPrice);
            }

            int k = 0;
            foreach (int i in pricesVals)
            {
                if(k++ >= 10)
                {
                    break;
                }
                header += "<th>$" + i + "</th>";
            }
            var tableRows = "";
            template = template.Replace("@pricecategories", header);


            foreach (ReportBuyersByPricesAndStates state in currentStatesNames)
            {
                var row = "<tr class='tablerows'>";
                row += "<td class='tablecell'>" + state.State + "</td>";
                
                for (var i = 1; i <= 10; i++)
                {
                    var currentPrice = currentPricesAndTopStates.Where(a => a.State == state.State && a.BuyerPrice >= i && a.BuyerPrice <= i+1).FirstOrDefault()?.BuyerPrice ?? 0;
                    var previousPrice = previousPricesAndTopStates.Where(a => a.State == state.State && a.BuyerPrice >= i && a.BuyerPrice <= i + 1).FirstOrDefault()?.BuyerPrice ?? 0;

                    var valueCurrent = currentPrice; //  GetCurrentMonthByPriceSoldCount(currentMonth, 0, "CA");
                    var valuePrevious = previousPrice; // GetPreviousMonthByPriceSoldCount(previousMonth, 0, "CA");

                    currentMonthValues.Add((int)valueCurrent);
                    prevMonthValues.Add((int)valuePrevious);

                    var change = 0;
                    if (valuePrevious!=0) change=(int)((double)valueCurrent / (double)valuePrevious * 100.0 - 100.0);

                    if (change > 0) //green arrow
                        row += @"<td class='tablecell'>
                        <sup class=""supstile"" style = ""color:green; font-weight:bold;""> &#8593;</sup>&nbsp;" + valueCurrent.ToString() + @"
                        <span class=""tablecellsub""><br>" + change.ToString() + @"%</span>
                        </td>";
                    else
                        row += @"<td class='tablecell'>
                        <sup class=""supstile"" style = ""color:red; font-weight:bold;""> &#8595;</sup>&nbsp;" + valueCurrent + @"
                        <span class=""tablecellsub""><br>" + change + @"%</span>
                        </td>";
                }

                row += "</tr>";
                tableRows += row;
            }

            template = template.Replace("@tablechange", GetChangeOfGraphs(currentMonthValues, prevMonthValues));
            template = template.Replace("@tablerows", tableRows);
            return template;
        }

        private string GetChangeOfGraphs(List<int> currentData, List<int> previousData)
        {
            var sumCurrent = (double)currentData.Sum();
            var sumPrevious = (double)previousData.Sum();
            if (sumPrevious == 0) return "0";

            var change = (int)(sumCurrent / sumPrevious * 100.0 - 100.0);

            var sup = "";
            if (change > 0)
                sup = @"<sup class=""supstile"" style = ""color:green; font-weight:bold;""> &#8593;</sup>&nbsp;";

            if (change < 0)
                sup = @" <sup class=""supstile"" style = ""color: red; font - weight:bold; "" > &#8595;</sup>&nbsp;";

            return sup + change.ToString() + " % ";
        }
        private async Task<string> GeneratePDFOutputGraphs(string template)
        {
            var exporter = new HighchartsExporter();

            var nowDate = DateTime.UtcNow;

            if (DateTime.UtcNow.Day < 28)
                nowDate = nowDate.AddMonths(-1);

            var beginOfCurrentMonth = new DateTime(nowDate.Year, nowDate.Month, 1, 0, 0, 0);


            template = template.Replace("@curmonth", beginOfCurrentMonth.ToString("MMMM", CultureInfo.InvariantCulture).ToUpper());
            template = template.Replace("@curyear", beginOfCurrentMonth.Year.ToString());

            var endOfCurrentMonth = beginOfCurrentMonth.AddMonths(1).AddDays(-1); //last day of month

            //Purchased Leads
            //Change from previous month

            var days = (endOfCurrentMonth - beginOfCurrentMonth).Days;

            List<string> daysCategories = new List<string>();
            List<int> currentMonthPurchasedLeads = new List<int>();
            List<int> previousMonthPurchasedLeads = new List<int>();
            for (var i = 1; i <= days; i++)
            {
                daysCategories.Add(i.ToString());
                DateTime dayForCurrentMonth = beginOfCurrentMonth.AddDays(i - 1);
                DateTime dayForPreviousMonth = beginOfCurrentMonth.AddMonths(-1).AddDays(i - 1);

                //Hayk, ArmanZ

                //Kardal 
                currentMonthPurchasedLeads.Add(new Random(i).Next(100, 200)); //read the data for dayForCurrentMonth
                previousMonthPurchasedLeads.Add(new Random(i * 10).Next(50, 150)); //read the data dayForPreviousMonth
            }

            var options = GetSeriesOptions("line", daysCategories, beginOfCurrentMonth,
                currentMonthPurchasedLeads, previousMonthPurchasedLeads);

            var graph1 = await exporter.GetImageBytesFromOptions_Works(options);
            template = template.Replace("@graphchange1", GetChangeOfGraphs(currentMonthPurchasedLeads, previousMonthPurchasedLeads));
            template = template.Replace("@purchasedleads", currentMonthPurchasedLeads.Sum().ToString());
            template = template.Replace("@graph1", graph1);


            /////////////////////////////////////////
            ///Sale Statistics by Lead Price
            ///Change from previous month
            ///

            List<int> prices = new List<int>();
            List<string> priceCategories = new List<string>();

            for (var i = 1; i < 10; i++) //get grouped prices
            {
                prices.Add(i);

            }

            List<int> currentMonthLeadPriceAmount = new List<int>();
            List<int> previousMonthLeadPriceAmount = new List<int>();
            for (var i = 0; i < prices.Count; i++)
            {
                priceCategories.Add("$" + prices[i].ToString());

                currentMonthLeadPriceAmount.Add(new Random(i).Next(100, 200));
                previousMonthLeadPriceAmount.Add(new Random(i * 10).Next(50, 150));
            }

            options = GetSeriesOptions("line", priceCategories, beginOfCurrentMonth,
                            currentMonthLeadPriceAmount, previousMonthLeadPriceAmount);
            var graph2 = await exporter.GetImageBytesFromOptions_Works(options);
            template = template.Replace("@graphchange2", GetChangeOfGraphs(currentMonthLeadPriceAmount, previousMonthLeadPriceAmount));
            template = template.Replace("@graph2", graph2);

            /////////////////////////////////////////
            ///Sale Statistics by Day of the Week
            ///Change from previous month
            ///

            List<string> daysOfWeek = new List<string>();
            daysOfWeek.Add("Monday");
            daysOfWeek.Add("Tuesday");
            daysOfWeek.Add("Wednesday");
            daysOfWeek.Add("Thursday");
            daysOfWeek.Add("Friday");
            daysOfWeek.Add("Saturday");
            daysOfWeek.Add("Sunday");




            List<int> currentMonthLeadDayOfWeekValues = new List<int>();
            List<int> previousMonthLeadDayOfWeekValues = new List<int>();
            for (var i = 0; i < daysOfWeek.Count; i++)
            {
                currentMonthLeadDayOfWeekValues.Add(new Random(i).Next(100, 200));
                previousMonthLeadDayOfWeekValues.Add(new Random(i * 10).Next(50, 150));
            }

            options = GetSeriesOptions("column", daysOfWeek, beginOfCurrentMonth,
                            currentMonthLeadDayOfWeekValues, previousMonthLeadDayOfWeekValues);
            var graph3 = await exporter.GetImageBytesFromOptions_Works(options);
            template = template.Replace("@graphchange3", GetChangeOfGraphs(currentMonthLeadDayOfWeekValues, previousMonthLeadDayOfWeekValues));
            template = template.Replace("@graph3", graph3);

            ////////Sale Statistics by Hour
            /////( Change from previous month: +30 % )
            ///
            var hours = 24;

            List<string> hoursCategories = new List<string>();
            List<int> currentMonthSaleLeadsByHours = new List<int>();
            List<int> previousMonthSaleLeadsByHours = new List<int>();
            for (var i = 0; i < hours; i++)
            {
                hoursCategories.Add(i.ToString() + ":00");
                currentMonthSaleLeadsByHours.Add(new Random(i).Next(100, 200));
                previousMonthSaleLeadsByHours.Add(new Random(i * 10).Next(50, 150));
            }

            options = GetSeriesOptions("line", hoursCategories, beginOfCurrentMonth,
                currentMonthSaleLeadsByHours, previousMonthSaleLeadsByHours);
            var graph4 = await exporter.GetImageBytesFromOptions_Works(options);
            template = template.Replace("@graphchange4", GetChangeOfGraphs(currentMonthSaleLeadsByHours, previousMonthSaleLeadsByHours));
            template = template.Replace("@graph4", graph4);

            /////////////
            ///Sale Statistics by Top 10 States
            List<string> top10States = new List<string>();
            top10States.Add("AB");
            top10States.Add("AK");
            top10States.Add("AZ");
            top10States.Add("IL");
            top10States.Add("CO");
            top10States.Add("CT");
            top10States.Add("DC");
            top10States.Add("DE");
            top10States.Add("FL");

            List<int> currentMonthLeadByStateValues = new List<int>();
            List<int> previousMonthLeadByStateValues = new List<int>();
            for (var i = 0; i < top10States.Count; i++)
            {
                currentMonthLeadByStateValues.Add(new Random(i).Next(100, 200));
                previousMonthLeadByStateValues.Add(new Random(i * 10).Next(50, 150));
            }

            options = GetSeriesOptions("column", top10States, beginOfCurrentMonth,
                            currentMonthLeadByStateValues, previousMonthLeadByStateValues);
            var graph5 = await exporter.GetImageBytesFromOptions_Works(options);
            template = template.Replace("@graphchange5", GetChangeOfGraphs(currentMonthLeadByStateValues, previousMonthLeadByStateValues));
            template = template.Replace("@graph5", graph5);


            ////Sale Statistics by Top States Table
            ///Change from previous month

            template = ContactInformation.ApplyToTemplate(template);
            template = GenerateReportTable(template, beginOfCurrentMonth, beginOfCurrentMonth.AddMonths(-1), prices, top10States);

            return template;
        }

        private async Task<string> GeneratePDFOutputGraphs_Z(string template, long buyerId)
        {
            var exporter = new HighchartsExporter();

            var nowDate = DateTime.UtcNow;

            if (DateTime.UtcNow.Day < 28)
                nowDate = nowDate.AddMonths(-1);

            DateTime beginOfCurrentMonth = new DateTime(nowDate.Year, nowDate.Month, 1, 0, 0, 0);


            template = template.Replace("@curmonth", beginOfCurrentMonth.ToString("MMMM", CultureInfo.InvariantCulture).ToUpper());
            template = template.Replace("@curyear", beginOfCurrentMonth.Year.ToString());

            DateTime endOfCurrentMonth = beginOfCurrentMonth.AddMonths(1).AddDays(-1); //last day of month

            DateTime beginOfPreviousMonth = beginOfCurrentMonth.AddMonths(-1);
            DateTime endOfPreviousMonth = beginOfPreviousMonth.AddMonths(1).AddDays(-1);

            List<ReportByDays> ReportDaysCurrentMonth = (List<ReportByDays>)_reportService.ReportByDays(beginOfCurrentMonth, endOfCurrentMonth, 0, buyerId);
            List<ReportByDays> ReportDaysPreviousMonth = (List<ReportByDays>)_reportService.ReportByDays(beginOfPreviousMonth, endOfPreviousMonth, 0, buyerId);


            //Purchased Leads
            //Change from previous month

            var days = (endOfCurrentMonth - beginOfCurrentMonth).Days;

            List<string> daysCategories = new List<string>();
            List<int> currentMonthPurchasedLeads = new List<int>();
            List<int> previousMonthPurchasedLeads = new List<int>();
            for (var i = 1; i <= days; i++)
            {
                daysCategories.Add(i.ToString());
                DateTime dayForCurrentMonth = beginOfCurrentMonth.AddDays(i - 1);
                DateTime dayForPreviousMonth = beginOfCurrentMonth.AddMonths(-1).AddDays(i - 1);

                var currentLeads = ReportDaysCurrentMonth.Where(a => a.Created.Day == i).FirstOrDefault()?.Total ?? 0;
                var previousLeads = ReportDaysPreviousMonth.Where(a => a.Created.Day == i).FirstOrDefault()?.Total ?? 0;

                currentMonthPurchasedLeads.Add(currentLeads);
                previousMonthPurchasedLeads.Add(previousLeads);
            }

            var options = GetSeriesOptions("line", daysCategories, beginOfCurrentMonth,
                currentMonthPurchasedLeads, previousMonthPurchasedLeads);

            var graph1 = await exporter.GetImageBytesFromOptions_Works(options);
            template = template.Replace("@graphchange1", GetChangeOfGraphs(currentMonthPurchasedLeads, previousMonthPurchasedLeads));
            template = template.Replace("@purchasedleads", currentMonthPurchasedLeads.Sum().ToString());
            template = template.Replace("@graph1", graph1);


            /////////////////////////////////////////
            ///Sale Statistics by Lead Price
            ///Change from previous month
            ///

            List<ReportBuyersByPrices> ReportPricesCurrentMonth = (List<ReportBuyersByPrices>)_reportService.ReportBuyersByPrices(beginOfCurrentMonth, endOfCurrentMonth, buyerId.ToString(), "", "", 0, 10);
            List<ReportBuyersByPrices> ReportPricesPreviousMonth = (List<ReportBuyersByPrices>)_reportService.ReportBuyersByPrices(beginOfPreviousMonth, endOfPreviousMonth, buyerId.ToString(), "", "", 0, 10);

            
            ReportPricesCurrentMonth.OrderBy(a => a.BuyerPrice);
            ReportPricesPreviousMonth.OrderBy(a => a.BuyerPrice);

            List<int> pricesUngrouped = new List<int>();
            List<int> prices = new List<int>();
            List<string> priceCategories = new List<string>();

            foreach(ReportBuyersByPrices r in ReportPricesCurrentMonth)
            {
                pricesUngrouped.Add((int)r.BuyerPrice);
            }

            foreach (ReportBuyersByPrices r in ReportPricesPreviousMonth)
            {
                pricesUngrouped.Add((int)r.BuyerPrice);
            }

            pricesUngrouped.Sort();

            var groupingStep = pricesUngrouped.Count / 10;

            groupingStep = groupingStep == 0 ? 1 : groupingStep;

            List<int> currentMonthLeadPriceAmount = new List<int>();
            List<int> previousMonthLeadPriceAmount = new List<int>();
            for (var i = 0; i < pricesUngrouped.Count; i+=groupingStep) //get grouped prices
            {
                //int price = ReportPricesCurrentMonth.Where(a => a.BuyerPrice == i).FirstOrDefault()?.Quantity ?? 0;
                int price = pricesUngrouped[i];
                prices.Add(price);

                var quantityCurrent = ReportPricesCurrentMonth.Where(a => a.BuyerPrice >= pricesUngrouped[i] && a.BuyerPrice <= pricesUngrouped[i]).Sum(a => a.Quantity);
                currentMonthLeadPriceAmount.Add(quantityCurrent);
                var quantityPrev = ReportPricesPreviousMonth.Where(a => a.BuyerPrice >= pricesUngrouped[i] && a.BuyerPrice <= pricesUngrouped[i]).Sum(a => a.Quantity);
                previousMonthLeadPriceAmount.Add(quantityPrev);
            }

            
            for (var i = 0; i < prices.Count; i++)
            {
                priceCategories.Add("$" + prices[i].ToString());
            }

            options = GetSeriesOptions("line", priceCategories, beginOfCurrentMonth,
                            currentMonthLeadPriceAmount, previousMonthLeadPriceAmount);
            var graph2 = await exporter.GetImageBytesFromOptions_Works(options);
            template = template.Replace("@graphchange2", GetChangeOfGraphs(currentMonthLeadPriceAmount, previousMonthLeadPriceAmount));
            template = template.Replace("@graph2", graph2);

            /////////////////////////////////////////
            ///Sale Statistics by Day of the Week
            ///Change from previous month
            ///

            List<string> daysOfWeek = new List<string>();
            daysOfWeek.Add("Monday");
            daysOfWeek.Add("Tuesday");
            daysOfWeek.Add("Wednesday");
            daysOfWeek.Add("Thursday");
            daysOfWeek.Add("Friday");
            daysOfWeek.Add("Saturday");
            daysOfWeek.Add("Sunday");

            IList<ReportByWeekdayTotal> currentMonthReportByWeekDays = this._reportService.ReportByWeekDayTotals(beginOfCurrentMonth, endOfCurrentMonth);
            IList<ReportByWeekdayTotal> previousMonthReportByWeekDays = this._reportService.ReportByWeekDayTotals(beginOfPreviousMonth, endOfPreviousMonth);


            List<int> currentMonthLeadDayOfWeekValues = new List<int>();
            List<int> previousMonthLeadDayOfWeekValues = new List<int>();
            for (var i = 0; i < daysOfWeek.Count; i++)
            {
                int currentWeekDay = currentMonthReportByWeekDays.Where(a => a.Weekday == i).FirstOrDefault()?.Weekday ?? 0;
                int previousWeekDay = previousMonthReportByWeekDays.Where(a => a.Weekday == i).FirstOrDefault()?.Weekday ?? 0;

                currentMonthLeadDayOfWeekValues.Add(currentWeekDay);
                previousMonthLeadDayOfWeekValues.Add(previousWeekDay);
            }

            options = GetSeriesOptions("column", daysOfWeek, beginOfCurrentMonth,
                            currentMonthLeadDayOfWeekValues, previousMonthLeadDayOfWeekValues);
            var graph3 = await exporter.GetImageBytesFromOptions_Works(options);
            template = template.Replace("@graphchange3", GetChangeOfGraphs(currentMonthLeadDayOfWeekValues, previousMonthLeadDayOfWeekValues));
            template = template.Replace("@graph3", graph3);



            ////////Sale Statistics by Hour
            /////( Change from previous month: +30 % )
            ///

            IList<ReportByHourTotal> currentMonthReportByHourTotals = this._reportService.ReportByHourTotals(beginOfCurrentMonth, endOfCurrentMonth);
            IList<ReportByHourTotal> previousMonthReportByHourTotals = this._reportService.ReportByHourTotals(beginOfPreviousMonth, endOfPreviousMonth);

            var hours = 24;

            List<string> hoursCategories = new List<string>();
            List<int> currentMonthSaleLeadsByHours = new List<int>();
            List<int> previousMonthSaleLeadsByHours = new List<int>();
            for (var i = 0; i < hours; i++)
            {
                hoursCategories.Add(i.ToString() + ":00");
                
                int currentMonthSaleLeads = currentMonthReportByHourTotals.Where(a => a.Hour == i).FirstOrDefault()?.Leads ?? 0;
                int previousMonthSaleLeads = previousMonthReportByHourTotals.Where(a => a.Hour == i).FirstOrDefault()?.Leads ?? 0;

                currentMonthSaleLeadsByHours.Add(currentMonthSaleLeads);
                previousMonthSaleLeadsByHours.Add(previousMonthSaleLeads);

            }

            options = GetSeriesOptions("line", hoursCategories, beginOfCurrentMonth,
                currentMonthSaleLeadsByHours, previousMonthSaleLeadsByHours);
            var graph4 = await exporter.GetImageBytesFromOptions_Works(options);
            template = template.Replace("@graphchange4", GetChangeOfGraphs(currentMonthSaleLeadsByHours, previousMonthSaleLeadsByHours));
            template = template.Replace("@graph4", graph4);

            /////////////
            ///Sale Statistics by Top 10 States
            List<string> top10States = new List<string>();
            
            IList<ReportTopStates> currentTopStates = this._reportService.ReportByTopStates(beginOfCurrentMonth, endOfCurrentMonth, 10, 0, 0, 0);
            IList<ReportTopStates> previousTopStates = this._reportService.ReportByTopStates(beginOfPreviousMonth, endOfPreviousMonth, 10, 0, 0, 0);


            List<int> currentMonthLeadByStateValues = new List<int>();
            List<int> previousMonthLeadByStateValues = new List<int>();
            int k = 0;
            foreach(ReportTopStates r in currentTopStates)
            {
                top10States.Add(r.State);

                currentMonthLeadByStateValues.Add((int)r.Counts);
                if (previousTopStates.Count > 0)
                {
                    previousMonthLeadByStateValues.Add((int)(previousTopStates[k++]?.Counts ?? 0));
                }
            }

            options = GetSeriesOptions("column", top10States, beginOfCurrentMonth,
                            currentMonthLeadByStateValues, previousMonthLeadByStateValues);
            var graph5 = await exporter.GetImageBytesFromOptions_Works(options);
            template = template.Replace("@graphchange5", GetChangeOfGraphs(currentMonthLeadByStateValues, previousMonthLeadByStateValues));
            template = template.Replace("@graph5", graph5);


            ////Sale Statistics by Top States Table
            ///Change from previous month

            template = ContactInformation.ApplyToTemplate(template);
            template = GenerateReportTable_Z(template, beginOfCurrentMonth, beginOfCurrentMonth.AddMonths(-1), prices, top10States);

            return template;
        }

        public async override Task GenerateReport(string fileName)
        {
            PdfGenerateConfig config = new PdfGenerateConfig();
            config.SetMargins(0);
            config.ManualPageSize = new XSize(842, 600);

            var path = Path.GetDirectoryName(pdf_template);

            if (!ContactInformation.ContactPhotoUrl.Contains("/")
                &&
                !ContactInformation.ContactPhotoUrl.Contains("\\"))
                ContactInformation.ContactPhotoUrl = path + "\\" + ContactInformation.ContactPhotoUrl;

            var template = File.ReadAllText(pdf_template);
            template = template.Replace("@path", path);



            PdfSharp.Pdf.PdfDocument doc = new PdfSharp.Pdf.PdfDocument();
            if (this.Simulated)
                template = await GeneratePDFOutputGraphs(template);
            else
                template = await GeneratePDFOutputGraphs_Z(template, buyerId);

            PdfGenerator.SplitHtmlIntoPagedPdf(template, "<!-- pagemarker -->", config, doc);

            doc.Save(this.output_folder + "\\" + fileName);

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