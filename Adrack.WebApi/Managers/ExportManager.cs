// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ExportManager.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Service.Helpers;
using Adrack.Web.Managers;
using Adrack.WebApi;
using Adrack.WebApi.Helpers;
using AdRack.Buffering;
using Arack.Encryption;
using Nager.Date;
using Nager.Date.Model;
using static Adrack.Managers.RequestManager;

namespace Adrack.Managers
{
    /// <summary>
    ///     Class ExportManager.
    ///     Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class ExportManager : IDisposable
    {
        public XmlHelper XmlHelper = null;

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //timerTokenSource.Dispose();
                //waitHandler.Dispose();
            }
        }

        /// <summary>
        ///     Class UserState.
        /// </summary>
        public class UserState
        {
            /// <summary>
            ///     The context
            /// </summary>
            public RequestContext context = null;

            /// <summary>
            ///     The format
            /// </summary>
            public BuyerChannel format = null;

            /// <summary>
            ///     The manager
            /// </summary>
            public ExportManager manager = null;
        }

        protected class PingTreeChannel
        {
            public BuyerChannel BuyerChannel { get; set; }

            public PingTreeItem PingTreeItem { get; set; }
        }

        #region Private properties

        /// <summary>
        ///     The total posts
        /// </summary>
        private int totalPosts;

        /// <summary>
        ///     The result
        /// </summary>
        private readonly RequestResult result = new RequestResult();

        /// <summary>
        ///     The results
        /// </summary>
        private readonly List<RequestResult> results = new List<RequestResult>();

        /// <summary>
        ///     The processing time
        /// </summary>
        private double processingTime;

        /// <summary>
        ///     The wait handler
        /// </summary>
        private readonly ManualResetEvent waitHandler = new ManualResetEvent(false);

        /// <summary>
        ///     The matched buyers
        /// </summary>
        private readonly List<BuyerChannel> matchedBuyers = new List<BuyerChannel>();

        /// <summary>
        ///     Ping tree
        /// </summary>        
        private List<PingTreeChannel> pingTreeChannels = new List<PingTreeChannel>();

        private Dictionary<long, List<FilterData>> ChildFilterData { get; set; } = new Dictionary<long, List<FilterData>>();
        private Dictionary<long, bool> ParentFilterResult { get; set; } = new Dictionary<long, bool>();

        private Dictionary<string, string> MappingFields = new Dictionary<string, string>();

        #endregion Private properties

        #region Public properties

        /// <summary>
        ///     Gets the result.
        /// </summary>
        /// <value>The result.</value>
        public RequestResult Result => result;

        /// <summary>
        ///     Gets the results.
        /// </summary>
        /// <value>The results.</value>
        public List<RequestResult> Results => results;

        /// <summary>
        ///     Gets or sets the processing time.
        /// </summary>
        /// <value>The processing time.</value>
        public double ProcessingTime
        {
            get => processingTime;
            set => processingTime = value;
        }

        public decimal MaxResponsePrice { get; set; }

        public string CustomPriceRejectUrl { get; set; }

        public Dictionary<long, List<long>> SuperTierChannels = new Dictionary<long, List<long>>();

        #endregion Public properties

        #region Private methods

        private void RemoveBuyerChannel()
        {
            if (pingTreeChannels.Count > 0)
                pingTreeChannels.RemoveAt(0);
        }

        /// <summary>
        ///     Generates test response.
        /// </summary>
        /// <returns>System.String.</returns>
        protected string GenerateResponse(HttpRequestMessage request, short pingTreeTestMode, bool hasChild)
        {
            string[] responses =
            {
                "<Response><status>sold</status><message>sold</message><price>100</price><redirect>https://google.com</redirect></Response>",
                "<Response><status>reject</status><price>{{price}}</price><redirect></redirect></Response>",
                "<Response><status>error</status><price>0</price><redirect></redirect></Response>",
                "<Response><status>test</status><price>0</price><redirect></redirect></Response>"
            };

            var rnd = new Random();
            //int n = rnd.Next(0, 2);

            var n = rnd.Next(0, 100);
            var i = 0;

            if (n <= 40)
                i = 0;
            else if (n > 40 && n <= 45)
                i = 2;
            else if (n > 45)
                i = 1;

            double maximum = 15;
            double minimum = 10;

            if ((i == 0 || i == 2) && pingTreeTestMode > 0 && pingTreeTestMode != 4) i = 1;

            if (pingTreeTestMode == 3 && hasChild)
                i = 1;

            if (pingTreeTestMode == 5)
                i = 1;

            if (pingTreeTestMode == 6)
                return responses[0];

            if (pingTreeTestMode == 7)
                i = 1; //always price reject

            if (i == 1)
            {
                switch (pingTreeTestMode)
                {
                    //always reject
                    case 5:
                    case 0: responses[i] = "<Response><status>reject</status><redirect>" + request.RequestUri.GetLeftPart(UriPartial.Authority) + "</redirect></Response>"; break;

                    //always price reject
                    case 7:
                    case 1:
                        responses[1] = responses[1].Replace("{{price}}", (rnd.NextDouble() * (maximum - minimum) + minimum).ToString());
                        //Thread.Sleep(10000);
                        break;
                    //price reject if has child
                    case 3:
                    case 4:
                        if (hasChild)
                        {
                            responses[i] = responses[i].Replace("{{price}}", (rnd.NextDouble() * (maximum - minimum) + minimum).ToString());
                            //Thread.Sleep(10000);
                        }
                        else
                            responses[i] = "<Response><status>reject</status><redirect>" + request.RequestUri.GetLeftPart(UriPartial.Authority) + "</redirect></Response>";
                        break;
                    //random price reject
                    case 2:
                        n = rnd.Next(0, 1);
                        if (n == 0)
                            responses[i] = "<Response><status>reject</status><redirect></redirect></Response>";
                        else
                            responses[i] = responses[i].Replace("{{price}}", (rnd.NextDouble() * (maximum - minimum) + minimum).ToString());
                        break;
                    default:
                        responses[i] = responses[i].Replace("{{price}}", (rnd.NextDouble() * (maximum - minimum) + minimum).ToString());
                        Thread.Sleep(10000);
                        break;
                }

            }

            return responses[i];
        }

        /// <summary>
        ///     Gets the formats.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="l">The l.</param>
        /// <param name="leadContent">Content of the lead.</param>
        /// <returns>BuyerChannel[].</returns>
        private List<PingTreeChannel> GetBuyerChannels(RequestContext context, LeadMain l, LeadContent leadContent)
        {
            var pingTreeItems = context.CampaignService.GetPingTreeItems(SharedData.GetCurrentPingTreeId()).OrderBy(x => x.OrderNum).ToList();

            List<PingTreeChannel> list = new List<PingTreeChannel>();

            foreach(var item in pingTreeItems)
            {
                BuyerChannel buyerChannel = context.BuyerChannelService.GetBuyerChannelById(item.BuyerChannelId);
                if ((!buyerChannel.Deleted.HasValue || (buyerChannel.Deleted.HasValue && !buyerChannel.Deleted.Value)) && buyerChannel.Status == BuyerChannelStatuses.Active || buyerChannel.Status == BuyerChannelStatuses.Paused)
                {
                    PingTreeChannel pingTreeChannel = new PingTreeChannel()
                    {
                        BuyerChannel = buyerChannel,
                        PingTreeItem = item
                    };

                    list.Add(pingTreeChannel);
                }
            }

            /*var buyerChannels = context.BuyerChannelService
                    .GetAllBuyerChannelsByCampaignId(l.CampaignId,
                        !string.IsNullOrEmpty(leadContent.Zip) ? leadContent.Zip : "", leadContent.State,
                        leadContent.Age ?? (short)50);

            return buyerChannels;*/

            return list;
        }

        /// <summary>
        ///     Gets the fields.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="f">The f.</param>
        /// <returns>BuyerChannelTemplate[].</returns>
        private BuyerChannelTemplate[] GetFields(RequestContext context, BuyerChannel f)
        {
            return context.BuyerChannelTemplateService.GetAllBuyerChannelTemplatesByBuyerChannelId(f.Id).ToArray();
        }

        protected RequestResult.ResultTypes ProcessCustomPriceRejectResponse(RequestContext context, string response, BuyerChannel buyerChannel, out RequestResult.ResultTypes outResultType)
        {
            /*responsePrice = 0;
            var price = "";
            var redirect = "";
            var redirectNoCData = "";
            var msg = "";*/

            if (!buyerChannel.EnableCustomPriceReject.HasValue || !buyerChannel.EnableCustomPriceReject.Value)
            {
                outResultType = RequestResult.ResultTypes.Test;
                return RequestResult.ResultTypes.Test;
            }

            /*if (response.ToLower().Contains("price_reject"))
            {
                string[] priceRejectStrs = response.Split(new char[1] { ',' });
                if (priceRejectStrs.Length == 3 && priceRejectStrs[0].ToLower() == "price_reject")
                {
                    //CustomPriceRejectUrl = priceRejectStrs[2];
                    price = priceRejectStrs[1];
                    decimal.TryParse(price, out responsePrice);

                    result.Set(RequestResult.ResultTypes.Success, RequestResult.ErrorTypes.None, msg);
                    result.Price = price;
                    result.Redirect = redirect;
                    result.RedirectNoCData = redirectNoCData;
                    context.Extra["navkey"] = Helpers.GenerateString(10);

                    result.InternalUrl = Helpers.GetBaseUrl(context.HttpRequest) + "navigate/" +
                                            context.Extra["navkey"];

                    outResultType = RequestResult.ResultTypes.Success;
                    return RequestResult.ResultTypes.Success;
                }
                else if (priceRejectStrs.Length == 2 && priceRejectStrs[0].ToLower() == "accepted")
                {
                    result.Set(RequestResult.ResultTypes.Success, RequestResult.ErrorTypes.None, msg);
                    result.Price = price;
                    result.Redirect = redirect;
                    result.RedirectNoCData = redirectNoCData;
                    context.Extra["navkey"] = Helpers.GenerateString(10);

                    result.InternalUrl = Helpers.GetBaseUrl(context.HttpRequest) + "navigate/" +
                                            context.Extra["navkey"];

                    outResultType = RequestResult.ResultTypes.Success;
                    return RequestResult.ResultTypes.Success;
                }
            }*/

            outResultType = RequestResult.ResultTypes.Test;
            return RequestResult.ResultTypes.Test;
        }

        private string readValue(string[] arr, int index)
        {
            if (index < 0 && arr.Length >= -index)
                return arr[arr.Length + index];
            if (arr.Length >= index && index > 0)
                return arr[index - 1];
            return "";
        }

        private string ParseStringValue(string source, string[] arr, string referalInfo)
        {
            if (referalInfo == null) return "";

            if (referalInfo.Contains("="))
            {
                var index = source.IndexOf(referalInfo.Substring(0, referalInfo.Length - 1));
                if (index >= 0)
                {
                    var nextString = source.Substring(index + referalInfo.Length - 1);
                    var strings = nextString.Split(referalInfo[referalInfo.Length - 1]);
                    if (strings.Length > 0)
                        return strings[0];
                }
            }
            else
            {
                var index = Int32Parse(referalInfo);
                return readValue(arr, index);
            }
            return "";
        }

        private int Int32Parse(string value)
        {
            int res = Int32.MaxValue;
            if (value == null) return res;
            Int32.TryParse(value, out res);
            return res;
        }

        private short DetectResponseFormat(string response, BuyerChannel inputChannel)
        {
            short responseFormatDetection = (short)(inputChannel.ResponseFormat.HasValue ? inputChannel.ResponseFormat.Value : 0);
            if (responseFormatDetection == (short)ResponseFormat.Auto)
            {
                if (inputChannel.DataFormat == (short)PostFormat.Json) responseFormatDetection = (short)ResponseFormat.Json;
                else
                if (inputChannel.DataFormat == (short)PostFormat.XML) responseFormatDetection = (short)ResponseFormat.XML;
                else
                if (inputChannel.DataFormat == (short)PostFormat.QueryStringGET || inputChannel.DataFormat == (short)PostFormat.QueryStringPOST)
                    responseFormatDetection = (short)ResponseFormat.XML;
            }
            else
            if (responseFormatDetection == (short)ResponseFormat.Detect)
            {
                if (response != null && response.Length > 0)
                {
                    if (response[0] == '<')
                        responseFormatDetection = (short)ResponseFormat.XML;
                    else
                    if (response[0] == '[' || response[0] == '{')
                        responseFormatDetection = (short)ResponseFormat.Json;
                    else
                        responseFormatDetection = (short)ResponseFormat.String;
                }

            }
            return responseFormatDetection;
        }

        private void ProcessMultiBuyerAcceptance(RequestContext context, int accountId, BuyerChannel buyerChannel, out BuyerChannel accountIdBuyerChannel)
        {
            accountIdBuyerChannel = null;
            Buyer accountIdBuyer = context.BuyerService.GetBuyerByAccountId(accountId);
            if (accountIdBuyer != null && accountIdBuyer.Status == 1)
            {
                accountIdBuyerChannel = context.BuyerChannelService.GetBuyerChannelByBuyerIdAndUniqueMappingId(accountIdBuyer.Id, buyerChannel.ChannelMappingUniqueId);
                if (accountIdBuyerChannel == null)
                {
                    List<BuyerChannel> buyerChannels = (List<BuyerChannel>)context.BuyerChannelService.GetAllBuyerChannelsByBuyerId(accountIdBuyer.Id).Where(x => x.Status == BuyerChannelStatuses.Active).ToList();
                    buyerChannels.Add(buyerChannel);
                    buyerChannels = buyerChannels.OrderByDescending(x => x.BuyerPrice).ToList();
                    int index = buyerChannels.IndexOf(buyerChannel);
                    if (index >= 0)
                    {
                        if (index > 0) accountIdBuyerChannel = buyerChannels[index - 1];
                        else if (index < buyerChannels.Count - 1) accountIdBuyerChannel = buyerChannels[index + 1];
                    }
                }
            }
        }

        bool ValidateXMLResponseValue(XmlDocument doc, Hashtable hash, string fieldName, string fieldValue, short? acceptFrom)
        {
            if (String.IsNullOrEmpty(fieldName)) return false;
            if (fieldValue == null) return false;
            short acc = 0;

            if (acceptFrom != null) acc = acceptFrom.Value;

            var val = GetXMLResponseValue(doc, hash, fieldName, acc);
            return val == fieldValue;
        }

        string GetXMLResponseValue(XmlDocument doc, Hashtable hash, string fieldName, short acceptFrom)
        {
            if (String.IsNullOrEmpty(fieldName)) return "";
            var acString = acceptFrom.ToString();
            string obj = (string)hash[fieldName + acString];
            if (obj != null) return obj;
            var node = XmlHelper.FindElementsByTagName(doc, fieldName);
            if (node != null)
            {
                string val = "";
                if (acceptFrom == 0)
                    val = node.InnerText;
                else
                    val = node.Name;
                hash[fieldName + acString] = val;
                return val;
            }
            else
                hash[fieldName + acString] = "";

            return "";
        }

        /// <summary>
        ///     Processes the XML response.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="response">The response.</param>
        /// <param name="buyerChannel">The format.</param>
        /// <param name="list">The list.</param>
        /// <param name="lead">The lead.</param>
        /// <param name="responsePrice">The response price.</param>
        /// <param name="outResultType">Type of the out result.</param>
        /// <param name="customPriceRejectUrl">Type of the out custom price reject url.</param>
        /// <returns>RequestResult.ResultTypes.</returns>
        private RequestResult.ResultTypes ProcessXMLResponse(RequestContext context, string response,
             BuyerChannel buyerChannel,
             List<object> list, LeadMain lead, decimal affiliatePrice, out decimal responsePrice, out int accountId, out string customLeadId, out BuyerChannel accountIdBuyerChannel, out RequestResult.ResultTypes outResultType,
             out string message)
        {
            message = "";
            responsePrice = 0;
            accountId = 0;
            customLeadId = "";
            accountIdBuyerChannel = null;
            var price = "";
            var redirect = "";
            var redirectNoCData = "";
            var msg = "";

            short responseFormatDetection = DetectResponseFormat(response, buyerChannel);

            var outres = 0;
            var handlerFormat = buyerChannel.ResponseFormat;
            try
            {
                if (Int32.TryParse(buyerChannel.AcceptedField, out outres) || handlerFormat == (short)ResponseFormat.String)
                {
                    char delimiter = ',';
                    if (buyerChannel.Delimeter != null && buyerChannel.Delimeter.Length > 0)
                        delimiter = buyerChannel.Delimeter[0];

                    var splitted = response.Split(delimiter);


                    price = ParseStringValue(response, splitted, buyerChannel.PriceField);
                    Decimal.TryParse(price, out responsePrice);
                    redirect = ParseStringValue(response, splitted, buyerChannel.RedirectField);
                    customLeadId = ParseStringValue(response, splitted, buyerChannel.LeadIdField);

                    var accountIdParsed = ParseStringValue(response, splitted, buyerChannel.AccountIdField);

                    if (accountIdParsed != "")
                    {
                        int.TryParse(accountIdParsed, out accountId);

                        if (accountId > 0)
                        {
                            ProcessMultiBuyerAcceptance(context, accountId, buyerChannel, out accountIdBuyerChannel);
                        }
                    }

                    outResultType = RequestResult.ResultTypes.Error;
                    var stringMessage = ParseStringValue(response, splitted, buyerChannel.MessageField);
                    Result.Message = stringMessage;

                    result.Price = responsePrice.ToString();

                    if (ParseStringValue(response, splitted, buyerChannel.AcceptedField) == buyerChannel.AcceptedValue)
                    {
                        outResultType = RequestResult.ResultTypes.Success;
                        result.Price = affiliatePrice.ToString();
                    }
                    else
                    if (ParseStringValue(response, splitted, buyerChannel.RejectedField) == buyerChannel.RejectedValue)
                    {
                        outResultType = RequestResult.ResultTypes.Reject;
                    }
                    else
                    if (ParseStringValue(response, splitted, buyerChannel.PriceRejectField) == buyerChannel.PriceRejectValue)
                    {
                        if (responsePrice > 0 && buyerChannel.EnableCustomPriceReject.HasValue && buyerChannel.EnableCustomPriceReject.Value)
                            outResultType = RequestResult.ResultTypes.Reject;
                        else
                            outResultType = RequestResult.ResultTypes.Reject;
                    }
                    else
                    if (ParseStringValue(response, splitted, buyerChannel.ErrorField) == buyerChannel.ErrorValue)
                        outResultType = RequestResult.ResultTypes.Error;
                    else
                        stringMessage = "Incorrect parsing result, status not detected";

                    message = stringMessage;

                    result.Set(outResultType, RequestResult.ErrorTypes.None, stringMessage);

                    if (!redirect.StartsWith("https://") && !redirect.StartsWith("http://"))
                        redirect = "";

                    result.Redirect = redirect;
                    result.RedirectNoCData = redirect;

                    context.Extra["navkey"] = Adrack.WebApi.Helpers.Helpers.GenerateString(10);

                    if (redirect != "")
                    {
                        string leftPart = context.HttpRequest.RequestUri.GetLeftPart(UriPartial.Authority);
                        if (!string.IsNullOrEmpty(leftPart) && leftPart[leftPart.Length - 1] != '/')
                        {
                            leftPart += "/";
                        }
                        result.InternalUrl = leftPart + "navigate?id=" +
                                             context.Extra["navkey"];
                    }

                    return outResultType;
                }//
            }
            catch (Exception ex)
            {
                message = "String format processing error " + ex.Message;

                context.ProcessingLogService.InsertProcessingLog(new ProcessingLog
                { Created = Helpers.UtcNow(), LeadId = lead.Id, Message = ex.Message, Name = "ProcessXMLResponse" });
                outResultType = RequestResult.ResultTypes.Reject;
                return RequestResult.ResultTypes.Error;
            }

            var xmldoc = new XmlDocument();
            try
            {
                if (responseFormatDetection == (short)ResponseFormat.Json) response = StructuredDataBuffering.JsonToXmlString(response);
                xmldoc.LoadXml(response);
            }
            catch (Exception ex)
            {
                if (responseFormatDetection == (short)ResponseFormat.Json)
                    message = "JSON format processing error";
                else
                    message = "XML format processing error";

                context.ProcessingLogService.InsertProcessingLog(new ProcessingLog
                { Created = Helpers.UtcNow(), LeadId = lead.Id, Message = ex.Message, Name = "ProcessXMLResponse" });

                RequestResult.ErrorTypes errType = result.ErrorType;

                if (response.ToLower() != "requesttimeout")
                    errType = RequestResult.ErrorTypes.BuyerChannelResponseError;
                else
                    errType = RequestResult.ErrorTypes.RequestTimeoutError;

                result.Set(RequestResult.ResultTypes.Reject, errType, message);
                result.Price = price;
                result.Message = message;
                outResultType = RequestResult.ResultTypes.Error;
                return RequestResult.ResultTypes.Error;
            }


            Hashtable hashedFields = new Hashtable();


            //MESSAGE PARSING
            msg = GetXMLResponseValue(xmldoc, hashedFields, buyerChannel.MessageField, 0);

            //PRICE PARSING
            var priceString = GetXMLResponseValue(xmldoc, hashedFields, buyerChannel.PriceField, 0);
            if (!String.IsNullOrEmpty(priceString))
            {
                decimal.TryParse(priceString, out responsePrice);
            }

            // PARSIGN REJECT
            if (ValidateXMLResponseValue(xmldoc, hashedFields, buyerChannel.RejectedField, buyerChannel.RejectedValue, buyerChannel.RejectedFrom))
            {
                if (result.ResultType != RequestResult.ResultTypes.Success)
                {
                    if (msg == "")
                        msg = "Lead was not sold in marketplace";

                    result.Set(RequestResult.ResultTypes.Reject, RequestResult.ErrorTypes.None, msg);
                    result.Price = price;
                    result.Redirect = redirect;
                    result.RedirectNoCData = redirectNoCData;
                }

                if (String.IsNullOrEmpty(buyerChannel.PriceRejectField))
                    if (responsePrice > 0 && buyerChannel.EnableCustomPriceReject.HasValue && buyerChannel.EnableCustomPriceReject.Value)
                    {
                        customLeadId = GetXMLResponseValue(xmldoc, hashedFields, buyerChannel.LeadIdField, 0);

                        result.Set(RequestResult.ResultTypes.Success, RequestResult.ErrorTypes.None, msg);
                        context.Extra["navkey"] = Helpers.GenerateString(10);

                        string leftPart = context.HttpRequest.RequestUri.GetLeftPart(UriPartial.Authority);
                        if (!string.IsNullOrEmpty(leftPart) && leftPart[leftPart.Length - 1] != '/')
                        {
                            leftPart += "/";
                        }

                        result.InternalUrl = leftPart + "navigate?id=" +
                                             context.Extra["navkey"];
                        outResultType = RequestResult.ResultTypes.Reject;
                        return RequestResult.ResultTypes.Reject;
                    }

                outResultType = RequestResult.ResultTypes.Reject;
                return RequestResult.ResultTypes.Reject;
            }

            //PARSING PRICE REJECT
            if (ValidateXMLResponseValue(xmldoc, hashedFields, buyerChannel.PriceRejectField, buyerChannel.PriceRejectValue, 0))
            {
                if (responsePrice > 0 && buyerChannel.EnableCustomPriceReject.HasValue && buyerChannel.EnableCustomPriceReject.Value)
                {
                    customLeadId = GetXMLResponseValue(xmldoc, hashedFields, buyerChannel.LeadIdField, 0);

                    result.Set(RequestResult.ResultTypes.Success, RequestResult.ErrorTypes.None, msg);
                    context.Extra["navkey"] = Helpers.GenerateString(10);

                    string leftPart = context.HttpRequest.RequestUri.GetLeftPart(UriPartial.Authority);
                    if (!string.IsNullOrEmpty(leftPart) && leftPart[leftPart.Length - 1] != '/')
                    {
                        leftPart += "/";
                    }

                    result.InternalUrl = leftPart + "navigate?id=" +
                                         context.Extra["navkey"];
                    outResultType = RequestResult.ResultTypes.Reject;
                    return RequestResult.ResultTypes.Reject;
                }

                outResultType = RequestResult.ResultTypes.Reject;
                return RequestResult.ResultTypes.Reject;
            }
            //REDIRECT PARSING
            redirect = GetXMLResponseValue(xmldoc, hashedFields, buyerChannel.RedirectField, 0);
            if (!String.IsNullOrEmpty(redirect))
            {
                if (redirect.StartsWith("<!"))
                {
                    redirect = redirect.Replace("<![CDATA[", "");
                    redirect = redirect.Replace("]]>", "");
                }
                redirectNoCData = redirect;
            }

            //ACCOUNT ID PARSING
            var accountID = GetXMLResponseValue(xmldoc, hashedFields, buyerChannel.AccountIdField, 0);

            if (!string.IsNullOrEmpty(accountID) && !string.IsNullOrEmpty(buyerChannel.ChannelMappingUniqueId))
            {
                int.TryParse(accountID, out accountId);
                if (accountId > 0)
                {
                    ProcessMultiBuyerAcceptance(context, accountId, buyerChannel, out accountIdBuyerChannel);
                }
            }

            //ACCEPT ID PARSING
            if (ValidateXMLResponseValue(xmldoc, hashedFields, buyerChannel.AcceptedField, buyerChannel.AcceptedValue, buyerChannel.AcceptedFrom))
            {
                if (result.ResultType != RequestResult.ResultTypes.Success)
                {
                    result.Set(RequestResult.ResultTypes.Success, RequestResult.ErrorTypes.None, msg);
                    result.Price = price;
                    result.Redirect = redirect;
                    result.RedirectNoCData = redirectNoCData;
                    context.Extra["navkey"] = Helpers.GenerateString(10);

                    string leftPart = context.HttpRequest.RequestUri.GetLeftPart(UriPartial.Authority);
                    if (!string.IsNullOrEmpty(leftPart) && leftPart[leftPart.Length - 1] != '/')
                    {
                        leftPart += "/";
                    }

                    result.InternalUrl = leftPart + "navigate?id=" +
                                         context.Extra["navkey"];
                }

                outResultType = RequestResult.ResultTypes.Success;
                return RequestResult.ResultTypes.Success;
            }

            //PARSING ERROR            
            if (ValidateXMLResponseValue(xmldoc, hashedFields, buyerChannel.ErrorField, buyerChannel.ErrorValue, buyerChannel.ErrorFrom))
            {
                if (result.ResultType != RequestResult.ResultTypes.Success)
                {
                    result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.None, msg);
                    result.Price = price;
                    result.Redirect = redirect;
                    result.RedirectNoCData = redirectNoCData;
                }

                outResultType = RequestResult.ResultTypes.Error;
                return RequestResult.ResultTypes.Error;
            }

            //PARSING TEST
            if (ValidateXMLResponseValue(xmldoc, hashedFields, buyerChannel.TestField, buyerChannel.TestValue, buyerChannel.TestFrom))
            {
                if (result.ResultType != RequestResult.ResultTypes.Success)
                {
                    result.Set(RequestResult.ResultTypes.Test, RequestResult.ErrorTypes.None, msg);
                    result.Price = price;
                    result.Redirect = redirect;
                    result.RedirectNoCData = redirectNoCData;
                    result.InternalUrl = "";
                }

                outResultType = RequestResult.ResultTypes.Test;
                return RequestResult.ResultTypes.Test;
            }

            outResultType = RequestResult.ResultTypes.Reject;
            return RequestResult.ResultTypes.Reject;
        }

        protected void AppendLeadIdField(RequestContext context, XmlNode leadIdElement, XmlDocument xmldoc, LeadMain lead, Buyer buyer)
        {
            bool canAppend = false;

            if (leadIdElement == null)
            {
                canAppend = true;
                leadIdElement = xmldoc.CreateElement("LeadId");
            }

            if (buyer.CanSendLeadId.HasValue && buyer.CanSendLeadId.Value)
            {
                if (context.Extra.ContainsKey("DublLeadId"))
                {
                    leadIdElement.InnerText = context.Extra["DublLeadId"].ToString();
                }
                else
                {
                    leadIdElement.InnerText = lead.Id.ToString();
                }
            }
            else
                leadIdElement.InnerText = lead.Id.ToString();

            if (leadIdElement != null && canAppend)
                xmldoc.DocumentElement.AppendChild(leadIdElement);
        }

        /// <summary>
        ///     Sets the XML data value.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="dataobj">The dataobj.</param>
        /// <param name="ct">The ct.</param>
        /// <param name="buyerChannelField">The f.</param>
        /// <param name="value">The value.</param>
        /// <param name="lead">The lead.</param>
        private bool SetXMLDataValue(RequestContext context, object dataobj, CampaignField ct, BuyerChannel buyerChannel,
            BuyerChannelTemplate buyerChannelField, string value, LeadMain lead, Buyer buyer, bool processWithoutIntegration = false)
        {
            var datafield = XmlHelper.GetElementsByTagName(dataobj as XmlDocument, buyerChannelField.TemplateField);
            if (datafield == null && !processWithoutIntegration) return true;

            if (ct != null)
            {
                if (!MappingFields.ContainsKey(ct.TemplateField))
                    MappingFields[ct.TemplateField] = buyerChannelField.TemplateField;
            }

            bool removeFromHashedFields = false;

            if (string.IsNullOrEmpty(buyerChannelField.DefaultValue))
            {
                if ((ct.Validator == (short)Validators.DateTime || 
                    ct.Validator == (short)Validators.DateOfBirth) && !string.IsNullOrEmpty(buyerChannelField.DataFormat))
                {
                    var affiliateChannelField = context.AffiliateChannelTemplateService.GetAffiliateChannelTemplate(context.AffiliateChannel.Id, buyerChannelField.CampaignTemplateId);

                    if (affiliateChannelField != null)
                    {
                        DateTime inputDate;
                        if (Validator.IsValidDateTime(value, affiliateChannelField.DataFormat, out inputDate))
                        {
                            try
                            {
                                value = inputDate.ToString(buyerChannelField.DataFormat, CultureInfo.InvariantCulture);
                                removeFromHashedFields = true;
                            }
                            catch { }
                        }
                    }
                }

                if (buyerChannel.BuyerId == 3 &&
                    !string.IsNullOrEmpty(ct.TemplateField) &&
                    ct.TemplateField.ToUpper() == "REQUESTEDAMOUNT" &&
                    !string.IsNullOrEmpty(buyerChannelField.TemplateField) &&
                    (buyerChannelField.TemplateField.ToLower() == "maximum_loan_amount" || buyerChannelField.TemplateField.ToLower() == "minimum_loan_amount"))
                {
                    decimal loanAmount = 0;
                    if (decimal.TryParse(value, out loanAmount))
                    {
                        if (loanAmount >= 100 && loanAmount <= 499)
                        {
                            switch (buyerChannelField.TemplateField.ToLower())
                            {
                                case "minimum_loan_amount":
                                    value = "300";
                                    break;
                                case "maximum_loan_amount":
                                    value = "500";
                                    break;
                            }
                        }

                        if (loanAmount >= 500 && loanAmount <= 1000)
                        {
                            switch (buyerChannelField.TemplateField.ToLower())
                            {
                                case "minimum_loan_amount":
                                    value = "500";
                                    break;
                                case "maximum_loan_amount":
                                    value = "1000";
                                    break;
                            }
                        }

                        if (loanAmount >= 1001 && loanAmount <= 2500)
                        {
                            switch (buyerChannelField.TemplateField.ToLower())
                            {
                                case "minimum_loan_amount":
                                    value = "1000";
                                    break;
                                case "maximum_loan_amount":
                                    value = "2500";
                                    break;
                            }
                        }

                        if (loanAmount >= 2501 && loanAmount <= 5000)
                        {
                            switch (buyerChannelField.TemplateField.ToLower())
                            {
                                case "minimum_loan_amount":
                                    value = "2500";
                                    break;
                                case "maximum_loan_amount":
                                    value = "5000";
                                    break;
                            }
                        }
                    }
                }

                var matchings = (List<BuyerChannelTemplateMatching>)context.BuyerChannelTemplateMatchingService
                    .GetBuyerChannelTemplateMatchingsByTemplateId(buyerChannelField.Id);

                foreach (var matching in matchings)
                {
                    if (matching.InputValue.ToLower() == value.ToLower())
                    {
                        value = matching.OutputValue;
                        removeFromHashedFields = true;
                        break;
                    }
                    else
                    if (ct.Validator == (short)Validators.DateTime || ct.Validator == (short)Validators.DateOfBirth)
                    {
                        DateTime inputDate;
                        if (Validator.IsValidDateTime(value, matching.InputValue, out inputDate))
                        {
                            try
                            {
                                value = inputDate.ToString(matching.OutputValue, CultureInfo.InvariantCulture);
                                removeFromHashedFields = true;
                                break;
                            }
                            catch { }
                        }
                    }
                    else if (matching.InputValue == "[" + ct.TemplateField + "]")
                    {
                        if (context.Extra.ContainsKey("mode") && context.Extra["mode"].ToString() == "t")
                        {
                            XmlDocument xmlDocument = (dataobj as XmlDocument);
                            var testFields = xmlDocument.GetElementsByTagName("test");
                            if (testFields.Count == 0)
                            {
                                XmlElement testElement = xmlDocument.CreateElement("test");
                                testElement.InnerText = "1";
                                xmlDocument.DocumentElement.AppendChild(testElement);
                            }
                        }

                        if (context.AllFieldValues.ContainsKey(ct.TemplateField))
                        {
                            string fieldValue = context.AllFieldValues[ct.TemplateField];
                            int addressMonth = 0;
                            int bankMonths = 0;
                            int empTime = 0;
                            switch (ct.TemplateField.ToLower())
                            {
                                case "addressmonth":
                                    if (int.TryParse(fieldValue, out addressMonth))
                                    {
                                        switch(buyerChannelField.TemplateField.ToLower())
                                        {
                                            case "custyrsatcurradd":
                                                value = ((int)(addressMonth / 12)).ToString();
                                                break;
                                            case "custmnthsatcurradd":
                                                value = ((int)(addressMonth % 12)).ToString();
                                                break;
                                        }
                                    }
                                    break;
                                case "bankmonths":
                                    if (int.TryParse(fieldValue, out bankMonths))
                                    {
                                        value = DateTime.UtcNow.AddMonths(-bankMonths).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                                    }
                                    break;
                                case "emptime":
                                    if (int.TryParse(fieldValue, out empTime))
                                    {
                                        value = DateTime.UtcNow.AddMonths(-empTime).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                                    }
                                    break;
                                case "incometype":
                                    switch(fieldValue.ToLower())
                                    {
                                        case "part_employed": value = "P"; break;
                                        default: value = "F"; break;
                                    }
                                    break;
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(matching.OutputValue) && matching.OutputValue.ToLower() == "[uppercase]")
                    {
                        if (!string.IsNullOrEmpty(value))
                            value = value.ToUpper();
                    }
                    else if (!string.IsNullOrEmpty(matching.OutputValue) && matching.OutputValue.ToLower() == "[lowercase]")
                    {
                        if (!string.IsNullOrEmpty(value))
                            value = value.ToLower();
                    }
                }
                datafield.InnerText = value;
            }
            else
            {
                if (buyerChannelField.DefaultValue == "[DATE GUID]")
                {
                    datafield.InnerText = StructuredDataBuffering.DateTimeToGuid(Helpers.UtcNow()).ToString();
                }
                else if (buyerChannelField.DefaultValue == "[CUR DATE]")
                {
                    var dt = Helpers.UtcNow();
                    if (ct != null && ct.Validator == (short)Validators.DateTime)
                        try
                        {
                            datafield.InnerText = dt.ToString(ct.ValidatorSettings);
                            removeFromHashedFields = true;
                        }
                        catch
                        {
                            datafield.InnerText = value;
                            removeFromHashedFields = true;
                        }
                }
                else if (buyerChannelField.DefaultValue.ToLower() == "#leadid#")
                {
                    context.Extra["LeadIdFieldCreated"] = true;
                    AppendLeadIdField(context, datafield, dataobj as XmlDocument, lead, buyer);
                }
                else if (buyerChannelField.DefaultValue.ToLower() == "%leadid%")
                {
                    datafield.InnerText = lead.Id.ToString();
                }
                else if (buyerChannelField.DefaultValue.ToLower() == "[optional]")
                {
                    if (dataobj != null)
                    {
                        var elementsToRemove = (dataobj as XmlDocument).GetElementsByTagName(buyerChannelField.TemplateField);
                        foreach (XmlElement element in elementsToRemove)
                        {
                            if (element.ParentNode != null && element.ParentNode.Name == buyerChannelField.SectionName)
                            {
                                element.ParentNode.RemoveChild(element);
                                break;
                            }
                        }
                    }
                }
                else if (buyerChannelField.DefaultValue.ToLower() == "[required]")
                {
                    var receivedFields = (Dictionary<string, XmlNode>)context.Extra["ReceivedDataXmlDocFields"];
                    XmlNode datael = null;
                    
                    if (receivedFields.ContainsKey(ct.SectionName + "_" + ct.TemplateField))
                        datael = receivedFields[ct.SectionName + "_" + ct.TemplateField];

                    if (datael != null && string.IsNullOrEmpty(datael.InnerText))
                        return false;
                }
                else
                {
                    datafield.InnerText = buyerChannelField.DefaultValue;
                    removeFromHashedFields = true;
                }
            }

            if (ct != null && removeFromHashedFields && context.HashedFieldValues.ContainsKey(ct.TemplateField))
            {
                context.HashedFieldValues.Remove(ct.TemplateField);
            }

            return true;
        }

        /// <summary>
        ///     Validates the filter.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="tplfield">The tplfield.</param>
        /// <param name="field">The buyerChannel.</param>
        /// <param name="val">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ValidateFilter(RequestContext context, CampaignField tplfield, BuyerChannelTemplate field,
            object val, long parentId = 0)
        {
            bool filterResult = true;

            var filters =
                (List<BuyerChannelFilterCondition>)context.BuyerChannelFilterConditionService
                    .GetFilterConditionsByBuyerChannelIdAndCampaignTemplateId(field.BuyerChannelId,
                        field.CampaignTemplateId, -1);

            if (filters.Count == 0) return true;

            string fieldVal = val.ToString().ToLower();

            foreach (var f in filters)
            {
                if (f.ParentId.HasValue && f.ParentId.Value > 0)
                {
                    if (!ChildFilterData.ContainsKey(f.ParentId.Value))
                    {
                        ChildFilterData[f.ParentId.Value] = new List<FilterData>();
                    }

                    if (!ParentFilterResult.ContainsKey(f.ParentId.Value))
                        ParentFilterResult[f.ParentId.Value] = false;

                    ChildFilterData[f.ParentId.Value].Add(new FilterData() { CampaignField = tplfield, ChannelField = field, Value = val, Filter = f });
                    continue;
                }

                if (string.IsNullOrEmpty(f.Value)) continue;

                filterResult = ValidateFilterCondition(context, tplfield, f, val);

                ParentFilterResult[f.Id] = filterResult;

                if (!filterResult)
                {
                    if (context.BuyerChannelFilterConditionService.HasChildren(f.Id)) continue; // Checks for child filters
                    return filterResult;
                }
            }

            return true;
        }
        int Years(DateTime start, DateTime end)
        {
            return (end.Year - start.Year - 1) +
                (((end.Month > start.Month) ||
                ((end.Month == start.Month) && (end.Day >= start.Day))) ? 1 : 0);
        }

        protected bool ValidateFilterCondition(RequestContext context, CampaignField tplfield, BuyerChannelFilterCondition f, object val)
        {
            try
            {
                bool filterResult = false;
                string fieldVal = val.ToString().ToLower();
                string fieldValOriginal = fieldVal;
                var values = f.Value.Split(',');

                var ageFound = false;
                var dateFound = false;
                DateTime fieldDate = DateTime.Now;
                DateTime checkFieldDate = DateTime.Now;


                bool isNumericFilter =
                       (Conditions)f.Condition == Conditions.NumberGreater
                       ||
                       (Conditions)f.Condition == Conditions.NumberGreaterOrEqual
                       ||
                       (Conditions)f.Condition == Conditions.NumberLess
                       ||
                       (Conditions)f.Condition == Conditions.NumberLessOrEqual;

                if (tplfield.Validator == (short)Validators.DateOfBirth && context.Extra.ContainsKey("Age"))
                {
                    ageFound = true;
                    if (tplfield.ValidatorSettings == "")
                        tplfield.ValidatorSettings = "MM/dd/yyyy";
                    fieldVal = context.Extra["Age"].ToString();
                }


                if ((ageFound || tplfield.Validator == (short)Validators.DateTime) && isNumericFilter)
                {
                    dateFound = true;
                    if (tplfield.ValidatorSettings == "")
                        tplfield.ValidatorSettings = "MM/dd/yyyy";

                    if (!DateTime.TryParseExact(fieldValOriginal, tplfield.ValidatorSettings,
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out fieldDate))
                        return false;
                    var diff = fieldDate - DateTime.MinValue;
                    fieldVal = diff.TotalDays.ToString();
                }


                foreach (var s in values)
                {
                    var checkValue = s.Trim().ToLower();


                    /*if (ageFound && isNumericFilter && !Decimal.TryParse(checkValue,out checkValueNumber))
                    {
                        DateTime parsed=DateTime.Now;
                        if (!DateTime.TryParseExact(checkValue, tplfield.ValidatorValue,
                            CultureInfo.InvariantCulture,DateTimeStyles.None, out parsed))
                            return false;
                        checkValue = Years(parsed, DateTime.Now).ToString();
                    }*/


                    if (dateFound && isNumericFilter)
                    {
                        if (!DateTime.TryParseExact(checkValue, tplfield.ValidatorSettings,
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out checkFieldDate))
                            return false;
                        else filterResult = true;
                        var diff = checkFieldDate - DateTime.MinValue;
                        checkValue = diff.TotalDays.ToString();
                    }

                    switch ((Conditions)f.Condition)
                    {
                        case Conditions.Contains: // 1:
                            if (fieldValOriginal.Contains(checkValue)) filterResult = true;
                            
                            break;

                        case Conditions.NotContains: //2:
                            if (!fieldValOriginal.Contains(checkValue)) filterResult = true;
                            else return false;
                            break;

                        case Conditions.StartsWith: //3:
                            if (fieldValOriginal.StartsWith(checkValue)) filterResult = true;
                            
                            break;

                        case Conditions.EndsWith: //3::
                            if (fieldValOriginal.EndsWith(checkValue)) filterResult = true;
                            
                            break;

                        case Conditions.Equals: //                        case 5:
                            if (checkValue == fieldValOriginal) filterResult = true;
                            
                            break;

                        case Conditions.NotEquals: //                        case 6:
                            if (checkValue != fieldValOriginal) filterResult = true;
                            else return false;
                            break;

                        case Conditions.NumberGreater: //                        case 7:
                            if (tplfield.Validator == (short)Validators.Number ||
                                tplfield.Validator == (short)Validators.Decimal ||
                                tplfield.Validator == (short)Validators.RoutingNumber
                                || tplfield.Validator == (short)Validators.DateOfBirth
                                || tplfield.Validator == (short)Validators.DateTime
                                )
                            {
                                if (decimal.Parse(fieldVal) > decimal.Parse(checkValue)) filterResult = true;
                                else return false;
                            }
                            else
                            {
                                var res = string.Compare(fieldValOriginal, checkValue);
                                if (res > 0) filterResult = true;
                                else return false;
                            }

                            break;

                        case Conditions.NumberGreaterOrEqual: //8:
                            if (tplfield.Validator == (short)Validators.Number ||
                                tplfield.Validator == (short)Validators.Decimal
                                ||
                                tplfield.Validator == (short)Validators.RoutingNumber
                                || tplfield.Validator == (short)Validators.DateOfBirth
                                || tplfield.Validator == (short)Validators.DateTime
                                )
                            {
                                if (decimal.Parse(fieldVal) >= decimal.Parse(checkValue)) filterResult = true;
                                else return false;
                            }
                            else
                            {
                                var res = string.Compare(fieldValOriginal, checkValue);
                                if (res > 0 || res == 0) filterResult = true;
                                else return false;
                            }

                            break;

                        case Conditions.NumberLess: //9:
                            if (tplfield.Validator == (short)Validators.Number ||
                                tplfield.Validator == (short)Validators.Decimal
                                ||
                                tplfield.Validator == (short)Validators.RoutingNumber
                                || tplfield.Validator == (short)Validators.DateOfBirth
                                || tplfield.Validator == (short)Validators.DateTime
                                )
                            {
                                if (decimal.Parse(fieldVal) < decimal.Parse(checkValue)) filterResult = true;
                                else return false;
                            }
                            else
                            {
                                var res = string.Compare(fieldValOriginal, checkValue);
                                if (res < 0) filterResult = true;
                                else return false;
                            }

                            break;

                        case Conditions.NumberLessOrEqual: //10:
                            if (tplfield.Validator == (short)Validators.Number ||
                                tplfield.Validator == (short)Validators.Decimal
                                ||
                                tplfield.Validator == (short)Validators.RoutingNumber
                                || tplfield.Validator == (short)Validators.DateOfBirth
                                || tplfield.Validator == (short)Validators.DateTime
                                )
                            {
                                if (decimal.Parse(fieldVal) <= decimal.Parse(checkValue)) filterResult = true;
                                else return false;
                            }
                            else
                            {
                                var res = string.Compare(fieldValOriginal, checkValue);
                                if (res < 0 || res == 0) filterResult = true;
                                else return false;
                            }

                            break;

                        case Conditions.NumberRange:
                            if (tplfield.Validator == (short)Validators.AccountNumber)
                                if (val.ToString().Length > 4)
                                    val = fieldVal.Substring(0, 4);

                            var vals = checkValue.Split('-');

                            var d = decimal.Parse(fieldVal);
                            if (decimal.Parse(vals[0].Trim()) <= d && decimal.Parse(vals[1].Trim()) >= d) filterResult = true;
                            else return false;

                            var l = long.Parse(fieldVal);
                            if (long.Parse(vals[0].Trim()) <= l && long.Parse(vals[1].Trim()) >= l) filterResult = true;
                            else return false;
                            break;

                        case Conditions.StringByLength:
                            {
                                int len;
                                int.TryParse(checkValue, out len);
                                if (!Utils.HasConsecutiveChars(fieldValOriginal, len)) filterResult = true;
                                else return false;
                            }
                            break;
                    }
                }

                return filterResult;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ValidateChildFilters(RequestContext context, Campaign campaign, LeadMain lead, LeadContent leadContent, BuyerChannel buyerChannel, DateTime utcNow)
        {
            foreach (long parentId in ChildFilterData.Keys)
            {
                if (!ParentFilterResult[parentId]) continue;

                var filterDataList = ChildFilterData[parentId];

                foreach (FilterData filterData in filterDataList)
                {
                    if (!ValidateFilterCondition(context, filterData.CampaignField, (BuyerChannelFilterCondition)filterData.Filter, filterData.Value))
                    {
                        if (result.ResultType != RequestResult.ResultTypes.Success)
                        {
                            var res = result.ResultType;
                            var msg = result.Message;

                            AddResponse(context, lead, leadContent, campaign, buyerChannel, RequestResult.ResultTypes.Reject,
                            "F: " + ((BuyerChannelTemplate)filterData.ChannelField).TemplateField + " '" + filterData.Value + "' does not match the criteria",
                            responseTime: (Helpers.UtcNow() - utcNow).TotalMilliseconds,
                            errorType: RequestResult.ErrorTypes.FilterError, validator: filterData.CampaignField.Validator);

                            if (res == RequestResult.ResultTypes.Success)
                                result.Set(res, RequestResult.ErrorTypes.None, msg);
                        }

                        RemoveBuyerChannel();

                        return false;
                    }
                }
            }

            return true;
        }

        protected void ProcessCombinedValues(RequestContext context, XmlNode node, XmlDocument xmldoc, BuyerChannel buyerChannel)
        {
            Regex r = new Regex(@"#(.+?)#");
            MatchCollection mc = r.Matches(node.InnerText);

            string finalResult = node.InnerText;

            for (int i = 0; i < mc.Count; i++)
            {
                var key = mc[i].Groups[1].Value;
                var nl = XmlHelper.GetElementsByTagName(xmldoc, key);
                if (nl != null)
                {
                    if (context.HashedFieldValues.ContainsKey(key))
                        finalResult = finalResult.Replace("#" + mc[i].Groups[1].Value + "#", context.HashedFieldValues[key]);
                    else
                        finalResult = finalResult.Replace("#" + mc[i].Groups[1].Value + "#", nl.InnerText);
                }
            }

            node.InnerText = finalResult;
        }

        private void AddResponse(RequestContext context, LeadMain lead, LeadContent leadContent, Campaign campaign,
            BuyerChannel buyerChannel, RequestResult.ResultTypes resultType,
            string responseMessage, RequestResult.ResultTypes postedDataResultType = RequestResult.ResultTypes.Reject, RequestResult.ErrorTypes errorType = RequestResult.ErrorTypes.None,
            string resultMessage = "Lead was not sold in marketplace!", string posted = "", decimal buyerPrice = 0, decimal affiliatePrice = 0, double responseTime = 0, short? validator = null)
        {
            result.Set(postedDataResultType, errorType, resultMessage);
            if (campaign.CampaignType == (short)CampaignTypes.LeadCampaign)
            {
                InsertPostedData(context, lead, leadContent, buyerChannel, posted, postedDataResultType);
                InsertLeadMainResponse(context, buyerChannel, lead, leadContent, Helpers.UtcNow(), responseTime,
                    responseMessage, responseMessage, buyerPrice, affiliatePrice, resultType, errorType, validator);
            }
        }

        /// <summary>
        ///     The past seconds
        /// </summary>
        public int PastSeconds;

        /// <summary>
        ///     The past total seconds
        /// </summary>
        public int PastTotalSeconds;

        public DateTime StartDateUtc { get; set; }

        /// <summary>
        ///     The timer task
        /// </summary>
        private Task timerTask;

        /// <summary>
        ///     The timer lock
        /// </summary>
        private readonly object timerLock = new object();

        /// <summary>
        ///     The timer token source
        /// </summary>
        //private readonly CancellationTokenSource timerTokenSource = new CancellationTokenSource();

        /// <summary>
        ///     The timer token
        /// </summary>
        private CancellationToken timerToken;


        //public List<Task> CurrentTasks = new List<Task>();

        /// <summary>
        ///     Processes the task.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="lead">The lead.</param>
        /// <param name="leadContent">Content of the lead.</param>
        /// <param name="ls">The ls.</param>
        /// <param name="buyerChannel">The format.</param>
        /// <param name="campaign">The campaign.</param>
        /// <param name="buyer">The buyer.</param>
        /// <param name="lAffiliatePrice">The l affiliate price.</param>
        /// <param name="xmldoc">The xmldoc.</param>
        /// <param name="nodesToHash">The nodes to hash.</param>
        /// <param name="nodesToHashInSent">The nodes to hash in sent.</param>
        protected BuyerChannel ProcessTask(RequestContext context, LeadMain lead, LeadContent leadContent, LeadMainResponse ls,
            BuyerChannel buyerChannel, Campaign campaign, Buyer buyer,
            XmlDocument xmldoc, List<string> nodesToHash, List<string> nodesToHashInSent, out bool ignoreTimeout)
        {
            ignoreTimeout = false;

            if (result.ResultType == RequestResult.ResultTypes.Success) return buyerChannel;

            var lUtcNow = Helpers.UtcNow();

            var response = "";
            decimal lAffiliatePrice = 0;


            if (buyerChannel.FieldAppendEnabled.HasValue && buyerChannel.FieldAppendEnabled.Value)
            {
                var affDoc = context.Extra["ReceivedDataXmlDoc"] as XmlDocument;
                foreach (string key in MappingFields.Keys)
                {
                    var nl = XmlHelper.GetElementsByTagName(xmldoc, MappingFields[key]);
                    if (nl != null)
                    {
                        ProcessCombinedValues(context, nl, affDoc, buyerChannel);
                    }
                }
            }

            var hashedXml = xmldoc.OuterXml;
            var dataToSend = xmldoc.OuterXml;

            PostedData postedData = null;
            BuyerResponse buyerResponse = null;
            AffiliateChannel affiliateChannel = (AffiliateChannel)context.Extra["informat"];

            if ((context.PastSeconds(StartDateUtc) < buyerChannel.Timeout + buyerChannel.AfterTimeout || buyerChannel.Timeout == 0) && (!affiliateChannel.Timeout.HasValue || affiliateChannel.Timeout.Value == 0 || context.PastSeconds() < affiliateChannel.Timeout.Value))
            {
                ignoreTimeout = true;
                postedData = new PostedData
                {
                    BuyerChannelId = buyerChannel.Id,
                    Created = Helpers.UtcNow(),
                    LeadId = lead.Id,
                    Posted = hashedXml,
                    MinPrice = (decimal)leadContent.Minprice,
                    Status = (short)RequestResult.ResultTypes.Reject
                };

                entityPostedData.Add(postedData);
                //context.PostedDataService.InsertPostedData(pd);

                foreach (string key in context.HashedFieldValues.Keys)
                {
                    if (key.Length == 0) continue;
                    if (MappingFields.ContainsKey(key))
                    {
                        var nl = XmlHelper.GetElementsByTagName(xmldoc, MappingFields[key]);
                        if (nl != null)
                        {
                            nl.InnerText = context.HashedFieldValues[key];
                        }
                    }
                }

                foreach (var node in nodesToHashInSent)
                {
                    if (node.Length == 0) continue;
                    if (MappingFields.ContainsKey(node))
                    {
                        var nl = XmlHelper.GetElementsByTagName(xmldoc, MappingFields[node]);
                        if (nl != null)
                            nl.InnerText = ADLEncryptionManager.Encrypt(nl.InnerText);
                    }
                }

                dataToSend = xmldoc.OuterXml;

                if (buyerChannel.DataFormat == (short)PostFormat.Json)
                {
                    dataToSend = StructuredDataBuffering.XmlToJSON(dataToSend, false);
                    hashedXml = StructuredDataBuffering.XmlToJSON(hashedXml, false);
                    if (postedData != null)
                        postedData.Posted = hashedXml;
                }
                else if (buyerChannel.DataFormat == (short)PostFormat.QueryStringGET || buyerChannel.DataFormat == (short)PostFormat.QueryStringPOST)
                {
                    dataToSend = Helpers.XmlToQueryString(xmldoc);
                    hashedXml = Helpers.XmlToQueryString(hashedXml);
                    if (postedData != null)
                        postedData.Posted = hashedXml;
                }
            }

            var parsedFromCustomPriceRejectLabel = false;
            var isCustomPriceReject = false;
            decimal responsePrice = 0;
            customPriceRejectLabel:

            if (parsedFromCustomPriceRejectLabel && ignoreTimeout)
            {
                postedData = new PostedData
                {
                    BuyerChannelId = buyerChannel.Id,
                    Created = Helpers.UtcNow(),
                    LeadId = lead.Id,
                    Posted = hashedXml,
                    MinPrice = (decimal)leadContent.Minprice
                };

                entityPostedData.Add(postedData);
            }

            if (buyer.AlwaysSoldOption == (short)BuyerType.Online && campaign.CampaignType == (short)CampaignTypes.LeadCampaign)
            {
                var postingHeaders = Helpers.ReadPostingHeaders(buyerChannel);
                var postingHeadersStr = buyerChannel.PostingHeaders;              

                if (ignoreTimeout)
                {
                    var contentType = "application/x-www-form-urlencoded";

                    if (buyerChannel.DataFormat == (short)PostFormat.Json)
                        contentType = "application/json";
                    else
                    if (buyerChannel.DataFormat == (short)PostFormat.XML)
                        contentType = "application/xml";

                    var timeout = (buyerChannel.Timeout + buyerChannel.AfterTimeout);

                    if (affiliateChannel.Timeout.HasValue &&
                        affiliateChannel.Timeout.Value > 0)
                    {
                        timeout = (int)affiliateChannel.Timeout.Value - (int)Math.Round((double)context.PastSeconds() / 60, 0, MidpointRounding.AwayFromZero);
                        if (timeout < 0)
                            timeout = 0;
                    }

                    ignoreTimeout = true;
                    if (buyerChannel.PostingUrl != "auto")
                        response = Adrack.WebApi.Helpers.Helpers.PostXml((string.IsNullOrEmpty(CustomPriceRejectUrl) ? buyerChannel.PostingUrl : CustomPriceRejectUrl), dataToSend,
                            (timeout) * 1000,
                            contentType, postingHeaders, postingHeadersStr, (string.IsNullOrEmpty(CustomPriceRejectUrl) ? (buyerChannel.DataFormat != 3 ? "POST" : "GET") : buyerChannel.WinResponsePostMethod));
                    else
                        response = GenerateResponse(context.HttpRequest, context.PingTreeTestMode, !String.IsNullOrEmpty(buyerChannel.ChildChannels));

                    context.Extra["BuyerChannelRequestData"] = dataToSend;

                    CustomPriceRejectUrl = "";
                }
            }
            else
            {
                if (buyerChannel.DataFormat == (short)PostFormat.Email && !string.IsNullOrEmpty(buyerChannel.PostingUrl))
                {
                    string[] emails = buyerChannel.PostingUrl.Trim().Split(new char[1] { ',' });
                    foreach (string email in emails)
                    {
                        context.EmailService.SendLeadEmail(lead.Id, email.Trim(), buyer.Name);
                    }
                }

                response = "Always sold";

                if (context.Extra.ContainsKey("sensitive") && result.ResultType != RequestResult.ResultTypes.Success)
                {
                    var lsd = (LeadSensitiveData)context.Extra["sensitive"];
                    lsd.Created = lead.Created;
                    lsd.LeadId = lead.Id;
                    context.LeadSensitiveDataService.InsertLeadSensitiveData(lsd);
                }
            }

            if (ignoreTimeout)
            {
                buyerResponse = new BuyerResponse
                {
                    BuyerId = buyerChannel.BuyerId,
                    BuyerChannelId = buyerChannel.Id,
                    LeadId = lead.Id,
                    Created = Helpers.UtcNow(),
                    PostedData = "",// parsedFromCustomPriceRejectLabel? dataToSend : hashedXml,
                    Response = response
                };
                entityBuyerResponse.Add(buyerResponse);
                //context.BuyerResponseService.InsertBuyerResponse(br);

                ls.Response = response;

                if (!parsedFromCustomPriceRejectLabel)
                    responsePrice = 0;
                var ourResponseType = RequestResult.ResultTypes.Test;

                var res = RequestResult.ResultTypes.Test;

                if (campaign.CampaignType == (short)CampaignTypes.LeadCampaign)
                {
                    if (buyer.AlwaysSoldOption == (short)BuyerType.Online)
                    {
                        int accountId = 0;
                        string customLeadId = "";

                        res = ProcessCustomPriceRejectResponse(context, response, buyerChannel, out ourResponseType);
                        if (res != RequestResult.ResultTypes.Success)
                        {
                            //standard processing
                            BuyerChannel accountIdBuyerChannel = null;

                            var message = "";
                            decimal checkedResponsePrice = 0;
                            res = ProcessXMLResponse(context, response, buyerChannel, null, lead, lAffiliatePrice, out checkedResponsePrice,
                                out accountId, out customLeadId, out accountIdBuyerChannel, out ourResponseType, out message);

                            if (!parsedFromCustomPriceRejectLabel)
                                responsePrice = checkedResponsePrice;

                            if (res == RequestResult.ResultTypes.Reject)
                            {
                                decimal minCheck = leadContent.Minprice != null ? leadContent.Minprice.Value : 0;
                                if (buyerChannel.BuyerPriceOption == BuyerPriceOptions.Dynamic  && responsePrice > 0 && responsePrice < minCheck)
                                {
                                    response = "Price Reject / Min Price Error";
                                    result.Redirect = "";
                                    result.InternalUrl = "";
                                    result.Set(res, RequestResult.ErrorTypes.None, "Price Reject / Min Price Error");
                                    responsePrice = 0;
                                }
                            }

                            if (res == RequestResult.ResultTypes.Error)
                            {
                                Result.Message = message;
                            }

                            if (res == RequestResult.ResultTypes.Success && accountId > 0 && accountIdBuyerChannel != null && accountIdBuyerChannel.Id != buyerChannel.Id)
                            {
                                buyerChannel = accountIdBuyerChannel;
                                if (postedData != null) postedData.BuyerChannelId = accountIdBuyerChannel.Id;
                                if (buyerResponse != null) buyerResponse.BuyerChannelId = accountIdBuyerChannel.Id;
                            }
                        }

                        if (/*res == RequestResult.ResultTypes.Success && */responsePrice > 0 && (buyerChannel.EnableCustomPriceReject.HasValue && buyerChannel.EnableCustomPriceReject.Value))
                        {
                            dataToSend = (!string.IsNullOrEmpty(buyerChannel.PriceRejectWinResponse) ? buyerChannel.PriceRejectWinResponse.Replace("{price}", responsePrice.ToString()).
                                Replace("{leadid}", customLeadId.ToString()) : dataToSend);
                            hashedXml = dataToSend;
                            isCustomPriceReject = true;
                            result.Set(res, RequestResult.ErrorTypes.None, "Custom Price Reject");
                            CustomPriceRejectUrl = buyerChannel.WinResponseUrl;
                        }

                        if (res == RequestResult.ResultTypes.Error &&
                            result.ResultType != RequestResult.ResultTypes.Success)
                            result.Set(RequestResult.ResultTypes.Error, this.Result.ErrorType, Result.Message);

                        if (string.IsNullOrEmpty(result.Redirect))
                        {
                            var redirectUrl = buyerChannel.RedirectUrl;
                            if (string.IsNullOrEmpty(redirectUrl)) redirectUrl = "";
                            result.Redirect = "<![CDATA[" + redirectUrl + "]]>";
                            result.RedirectNoCData = redirectUrl;
                        }
                    }
                    else
                    {
                        lead.Status = (short)RequestResult.ResultTypes.Success;
                        ourResponseType = RequestResult.ResultTypes.Success;
                        res = RequestResult.ResultTypes.Success;
                        result.Set(RequestResult.ResultTypes.Success, RequestResult.ErrorTypes.None, "");
                        context.Extra["navkey"] = Helpers.GenerateString(10);

                        string leftPart = context.HttpRequest.RequestUri.GetLeftPart(UriPartial.Authority);
                        if (!string.IsNullOrEmpty(leftPart) && leftPart[leftPart.Length - 1] != '/')
                        {
                            leftPart += "/";
                        }

                        result.InternalUrl = leftPart + "navigate?id=" +
                                             context.Extra["navkey"];

                        decimal aprice;
                        decimal.TryParse(lAffiliatePrice.ToString(), out aprice);

                        result.Price = Math.Round(aprice, 1).ToString();

                        if (context.Extra.ContainsKey("ZipCode") && context.Extra["ZipCode"] != null &&
                            context.Extra["ZipCode"].ToString().Length > 0)
                        {
                            if (!string.IsNullOrEmpty(buyerChannel.RedirectUrl))
                            {
                                result.Redirect = "<![CDATA[" + buyerChannel.RedirectUrl + "]]>";
                                result.RedirectNoCData = buyerChannel.RedirectUrl;
                            }
                            else
                            if (!string.IsNullOrEmpty(context.AffiliateRedirectUrl))
                            {
                                result.Redirect = "<![CDATA[" + context.AffiliateRedirectUrl + "]]>";
                                result.RedirectNoCData = context.AffiliateRedirectUrl;
                            }
                        }
                        else if (!string.IsNullOrEmpty(context.AffiliateRedirectUrl))
                        {
                            result.Redirect = "<![CDATA[" + context.AffiliateRedirectUrl + "]]>";
                            result.RedirectNoCData = context.AffiliateRedirectUrl;
                        }
                        else if (!string.IsNullOrEmpty(buyerChannel.RedirectUrl))
                        {
                            result.Redirect = "<![CDATA[" + buyerChannel.RedirectUrl + "]]>";
                            result.RedirectNoCData = buyerChannel.RedirectUrl;
                        }

                        lead.BuyerChannelId = buyerChannel.Id;
                        lead.SoldDate = Helpers.UtcNow();
                    }

                    long bChannelId;
                    long.TryParse(context.Extra["BuyerChannelId"].ToString(), out bChannelId);

                    if (bChannelId > 0 && bChannelId == buyerChannel.Id || context.Extra.ContainsKey("mode") &&
                        context.Extra["mode"].ToString() == "test")
                    {
                        res = RequestResult.ResultTypes.Test;
                        result.Set(RequestResult.ResultTypes.Test, RequestResult.ErrorTypes.None, "");
                    }

                    if (result.ResultType == RequestResult.ResultTypes.Success && buyerChannel.BuyerPriceOption == BuyerPriceOptions.Dynamic && responsePrice == 0)
                    {
                        responsePrice = buyerChannel.BuyerPrice;
                    }

                    decimal BuyerPrice = 0;

                    CalculatePrices(context, leadContent, buyerChannel, affiliateChannel, responsePrice, out lAffiliatePrice, out BuyerPrice);

                    if (lAffiliatePrice > BuyerPrice && result.ResultType == RequestResult.ResultTypes.Success)
                    {
                        res = Result.ResultType = RequestResult.ResultTypes.Reject;
                        result.ResultType = RequestResult.ResultTypes.Reject;
                        Result.ErrorType = RequestResult.ErrorTypes.MinPriceError;
                        lead.Status = (short)RequestResult.ResultTypes.Reject;
                        lead.BuyerChannelId = null;
                        Result.Message = "Lead was not sold in the market place!";
                        responsePrice = 0;
                        result.Redirect = result.RedirectNoCData = result.InternalUrl = "";
                        response = "Min Price Error";
                        isCustomPriceReject = false;
                        CustomPriceRejectUrl = "";
                        if (buyerResponse != null)
                            buyerResponse.Response = response;
                        if (postedData != null)
                            postedData.Status = (short)result.ResultType;
                    }

                    var status = (short)res;
                    short? timeoutErrorType = null;
                    if (context.PastSeconds(StartDateUtc) > buyerChannel.Timeout && context.PastSeconds(StartDateUtc) <= buyerChannel.AfterTimeout && result.ErrorType != RequestResult.ErrorTypes.RequestTimeoutError)
                        timeoutErrorType = (short)RequestResult.ErrorTypes.RequestTimeoutError;

                    //if (!parsedFromCustomPriceRejectLabel) //added by arman
                    {

                        var leadResponse = new LeadMainResponse
                        {
                            ResponseTime = (Helpers.UtcNow() - lUtcNow).TotalMilliseconds,
                            Response = response,
                            BuyerId = buyerChannel.BuyerId,
                            ResponseError = result.Message,
                            LeadId = lead.Id,
                            BuyerPrice = BuyerPrice,
                            AffiliatePrice = lAffiliatePrice,
                            BuyerChannelId = buyerChannel.Id,
                            AffiliateChannelId = (context.Extra["informat"] as AffiliateChannel).Id,
                            State = leadContent.State,
                            Status = (short)status,
                            AffiliateId = lead.AffiliateId,
                            CampaignId = lead.CampaignId,
                            Created = Helpers.UtcNow(),
                            CampaignType = lead.CampaignType,
                            MinPrice = (decimal)leadContent.Minprice,
                            ErrorType = (short)(result.ResultType == RequestResult.ResultTypes.Reject && responsePrice > 0 ? (short)RequestResult.ErrorTypes.MinPriceError : (timeoutErrorType.HasValue ? timeoutErrorType.Value : 0)),
                            Ssn = (leadContent != null ? leadContent.Ssn : "")
                        };

                        entityListMainResponse.Add(leadResponse);
                    }

                    if (result.ResultType == RequestResult.ResultTypes.Reject)
                    {
                        if (responsePrice > 0) //price reject checking
                        {
                            //check if this channel is super tier
                            if (SuperTierChannels.ContainsKey(buyerChannel.Id))
                            {
                                //buyerChannels.Remove(buyerChannel); //do not send to super tier more
                                SuperTierChannels.Remove(buyerChannel.Id); //open represent tier channels
                            }
                            else
                            if (!isCustomPriceReject)
                            {
                                buyerChannel.BuyerPrice = responsePrice;
                                if (priceRejectLocks.ContainsKey(buyerChannel.Id))
                                {
                                    priceRejectLocks.Remove(buyerChannel.Id);
                                }
                                else
                                {
                                    priceRejectLocks[buyerChannel.Id] = 1;
                                    pingTreeChannels = pingTreeChannels.OrderByDescending(x => x.BuyerChannel.BuyerPrice).ToList();
                                }
                            }
                        }

                        entityBuyerUpdates.Add(buyer);
                    }
                    else if (result.ResultType == RequestResult.ResultTypes.Test)
                    {
                        //write test handler here if necessary
                    }
                    else if (result.ResultType == RequestResult.ResultTypes.Success &&
                             //ourResponseType == RequestResult.ResultTypes.Success && !parsedFromCustomPriceRejectLabel)
                             ourResponseType == RequestResult.ResultTypes.Success)
                    {
                        if (CheckCredit(context, lead, leadContent, campaign, buyerChannel))
                        {
                            buyer.LastPostedSold = lead.Created;
                            entityBuyerUpdates.Add(buyer);
                            //context.BuyerService.UpdateBuyer(buyer);
                            lead.BuyerChannelId = buyerChannel.Id;
                            lead.BuyerPrice = BuyerPrice;
                            lead.AffiliatePrice = lAffiliatePrice;
                            lead.SoldDate = Helpers.UtcNow();
                            result.Price = Math.Round(lAffiliatePrice, 2).ToString();/* buyerChannel.BuyerPriceOption == 0
                                ? Math.Round(lAffiliatePrice, 2).ToString()
                                : Math.Round(responsePrice, 2).ToString();*/

                            buyerCredits[buyer.Id] = null;

                            context.AccountingService.UpdateBuyerBalance(buyerChannel.BuyerId, BuyerPrice);
                        }
                        else
                        {
                            buyerCredits[buyer.Id] = false;
                            RemoveBuyerChannel();
                        }
                    }
                    else
                    {
                        //context.BuyerService.UpdateBuyer(buyer);
                        entityBuyerUpdates.Add(buyer);
                    }
                    if (!parsedFromCustomPriceRejectLabel)
                        if (!priceRejectLocks.ContainsKey(buyerChannel.Id))// && responsePrice == 0) //remove channel if no price reject
                            RemoveBuyerChannel();
                }
                else
                {
                    res = RequestResult.ResultTypes.Reject;
                    result.Set(RequestResult.ResultTypes.Reject, RequestResult.ErrorTypes.None, "");
                    context.Extra["navkey"] = "";
                    result.InternalUrl = "";
                    result.Redirect = "";
                    result.RedirectNoCData = "";
                    lead.BuyerChannelId = buyerChannel.Id;
                    lead.Status = (short)RequestResult.ResultTypes.Reject;
                    RemoveBuyerChannel();
                }

                if (postedData != null)
                {
                    postedData.Status = (short)result.ResultType;
                }

                if (!parsedFromCustomPriceRejectLabel)
                    if (!string.IsNullOrEmpty(CustomPriceRejectUrl))
                    {
                        parsedFromCustomPriceRejectLabel = true;
                        goto customPriceRejectLabel;
                    }
            }
            else
                RemoveBuyerChannel();

            return buyerChannel;
        }

        /// <summary>
        ///     Sends the data.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="lead">The lead.</param>
        /// <param name="buyerChannel">The buyer channel.</param>
        /// <param name="campaign">The campaign.</param>
        /// <param name="buyerChannelTemplates">List of buyer channel templates.</param>
        /// <param name="ls">The ls.</param>
        /// <param name="leadContent">Content of the lead.</param>
        /// <param name="buyer">The buyer.</param>
        /// <param name="AffiliatePrice">The affiliate price.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="Exception"></exception>
        private bool SendData(RequestContext context, LeadMain lead, BuyerChannel buyerChannel, Campaign campaign,
            BuyerChannelTemplate[] buyerChannelTemplates, LeadMainResponse ls, LeadContent leadContent, Buyer buyer,
            decimal AffiliatePrice, XmlDocument campaignXml)
        {
            var utcNow = Helpers.UtcNow();

            ChildFilterData.Clear();

            if (!context.MinProcessingMode)
                if (context.DublicateMonitor && !string.IsNullOrEmpty(leadContent.Ssn) && campaign.CampaignType == (short)CampaignTypes.LeadCampaign &&
                (buyer.MaxDuplicateDays > 0 || buyerChannel.MaxDuplicateDays.HasValue && buyerChannel.MaxDuplicateDays.Value > 0) &&
                result.ResultType != RequestResult.ResultTypes.Success &&
                context.LeadMainResponseService.GetDublicateLeadByBuyer(
                    leadContent.Ssn,
                    utcNow.AddDays(buyerChannel.MaxDuplicateDays.HasValue && buyerChannel.MaxDuplicateDays.Value > 0
                        ? -buyerChannel.MaxDuplicateDays.Value
                        : -buyer.MaxDuplicateDays),
                    buyerChannel.MaxDuplicateDays.HasValue && buyerChannel.MaxDuplicateDays.Value > 0 ? buyerChannel.Id : buyer.Id,
                    buyerChannel.MaxDuplicateDays.HasValue && buyerChannel.MaxDuplicateDays.Value > 0 ? false : true))
                {
                    var res = result.ResultType;
                    var msg = result.Message;

                    result.Set(RequestResult.ResultTypes.Reject, RequestResult.ErrorTypes.None,
                        "Lead was not sold in the marketplace");

                    InsertPostedData(context, lead, leadContent, buyerChannel, "Max duplicates per day reached", result.ResultType);

                    var leadResponse = new LeadMainResponse
                    {
                        ResponseTime = 0,
                        Response = "Max duplicates per day reached",
                        BuyerId = buyerChannel.BuyerId,
                        ResponseError = "Max duplicates per day reached",
                        LeadId = lead.Id,
                        BuyerPrice = 0,
                        AffiliatePrice = 0,
                        BuyerChannelId = buyerChannel.Id,
                        AffiliateChannelId = (context.Extra["informat"] as AffiliateChannel).Id,
                        State = leadContent.State,
                        Status = (short)result.ResultType,
                        AffiliateId = lead.AffiliateId,
                        CampaignId = lead.CampaignId,
                        Created = Helpers.UtcNow(),
                        CampaignType = lead.CampaignType,
                        MinPrice = (decimal)leadContent.Minprice,
                        Ssn = (leadContent != null ? leadContent.Ssn : "")
                    };

                    //context.LeadMainResponseService.InsertLeadMainResponse(leadResponse);
                    entityListMainResponse.Add(leadResponse);

                    if (res == RequestResult.ResultTypes.Success) result.Set(res, RequestResult.ErrorTypes.None, msg);

                    RemoveBuyerChannel();

                    return false;
                }

            var dailyCapCalculated = 0;
            if (!context.MinProcessingMode && buyer.DailyCap > 0)
            {
                if (context.BuyerDailyCaps.ContainsKey(buyer.Id))
                    dailyCapCalculated = context.BuyerDailyCaps[buyer.Id];
                else
                {
                    dailyCapCalculated = context.LeadMainService.GetLeadCountByBuyer(utcNow, buyer.Id);
                    context.BuyerDailyCaps.Add(buyer.Id, dailyCapCalculated);
                }
            }

            if (!context.MinProcessingMode && buyer.DailyCap > 0)
                if (campaign.CampaignType == (short)CampaignTypes.LeadCampaign 
                    &&
                    result.ResultType != RequestResult.ResultTypes.Success 
                    &&
                    dailyCapCalculated > buyer.DailyCap)
                {
                    var res = result.ResultType;
                    var msg = result.Message;

                    result.Set(RequestResult.ResultTypes.Reject, RequestResult.ErrorTypes.DailyCapReached,
                        "Daily cap reached");

                    InsertPostedData(context, lead, leadContent, buyerChannel, result.Message, RequestResult.ResultTypes.Reject);

                    InsertLeadMainResponse(context, buyerChannel, lead, leadContent,
                        Helpers.UtcNow(),
                        0, result.Message, result.Message, 0, 0, RequestResult.ResultTypes.Reject,
                        RequestResult.ErrorTypes.DailyCapReached, null);

                    if (buyerChannel.CapReachedNotification)
                    {
                        if (!buyerChannel.CapReachEmailCount.HasValue)
                            buyerChannel.CapReachEmailCount = 0;

                        if (buyerChannel.CapReachEmailCount.Value < 3)
                        {
                            string[] emails = buyerChannel.NotificationEmail.Split(new char[1] { ',' });
                            foreach (string email in emails)
                            {
                                context.EmailService.SendCapReachNotification(buyer.Name, buyerChannel.Name, buyer.Name, email, EmailOperatorEnums.LeadNative);
                            }
                        }

                        buyerChannel.CapReachEmailCount++;

                        if (buyerChannel.CapReachEmailCount > 30)
                            buyerChannel.CapReachEmailCount = 0;

                        context.BuyerChannelService.UpdateBuyerChannel(buyerChannel);
                    }

                    if (res == RequestResult.ResultTypes.Success) result.Set(res, RequestResult.ErrorTypes.None, msg);

                    RemoveBuyerChannel();

                    return false;
                }

            object dataobj = null;


            var ReceivedDataXmlDocFields = (Dictionary<string, XmlNode>)context.Extra["ReceivedDataXmlDocFields"];

            XmlNode datael = null;
            totalPosts = 0;

            var dataElValue = "";

            var nodesToHash = new List<string>();
            var nodesToHashInSent = new List<string>();

            if (buyerChannelTemplates.Length == 0 && result.ResultType != RequestResult.ResultTypes.Success)
            {
                var res = result.ResultType;
                var msg = result.Message;

                result.Set(RequestResult.ResultTypes.Reject, RequestResult.ErrorTypes.IntegrationError,
                    "Buyer channel integration error");

                if (campaign.CampaignType == (short)CampaignTypes.LeadCampaign)
                {
                    InsertPostedData(context, lead, leadContent, buyerChannel, result.Message, RequestResult.ResultTypes.Reject);
                    InsertLeadMainResponse(context, buyerChannel, lead, leadContent,
                        Helpers.UtcNow(),
                        0, result.Message, result.Message, 0, 0, RequestResult.ResultTypes.Reject,
                        RequestResult.ErrorTypes.IntegrationError, null);
                }

                if (res == RequestResult.ResultTypes.Success) result.Set(res, RequestResult.ErrorTypes.None, msg);

                RemoveBuyerChannel();

                return false;
            }

            var ctpl = (Dictionary<long, CampaignField>)context.Extra["CampaignTemplates"];
            var aftpl = (Dictionary<long, AffiliateChannelTemplate>)context.Extra["AChannelTemplates"];

            context.Extra["LeadIdFieldCreated"] = false;

            foreach (var f in buyerChannelTemplates)
            {
                datael = null;

                dataElValue = "";

                CampaignField ct = null;

                if (dataobj == null)
                    try
                    {
                        dataobj = new XmlDocument();
                        if (!string.IsNullOrEmpty(buyerChannel.XmlTemplate))
                        {
                            ///context.BuyerChannelService.GetBuyerChannelById
                            dataobj = context.BuyerChannelService.GetBuyerChannelXML(buyerChannel);
                            //(dataobj as XmlDocument).LoadXml(buyerChannel.XmlTemplate);

                        }
                        else
                        {
                            //arman commented
                            //(dataobj as XmlDocument).LoadXml(campaign.XmlTemplate);
                            dataobj = campaignXml;
                            if (dataobj == null)
                                throw new Exception("Intregration failed");
                        }
                    }
                    catch
                    {
                        if (result.ResultType != RequestResult.ResultTypes.Success)
                        {
                            var res = result.ResultType;
                            var msg = result.Message;

                            result.Set(RequestResult.ResultTypes.Reject, RequestResult.ErrorTypes.IntegrationError,
                                "Buyer channel integration error");

                            InsertPostedData(context, lead, leadContent, buyerChannel, result.Message, RequestResult.ResultTypes.Reject);
                            InsertLeadMainResponse(context, buyerChannel, lead, leadContent,
                                Helpers.UtcNow(),

                                0, result.Message, result.Message, 0, 0, RequestResult.ResultTypes.Reject,
                                RequestResult.ErrorTypes.IntegrationError, null);

                            if (res == RequestResult.ResultTypes.Success)
                                result.Set(res, RequestResult.ErrorTypes.None, msg);
                        }

                        RemoveBuyerChannel();

                        return false;
                    }

                if (ctpl.ContainsKey(f.CampaignTemplateId))
                    ct = ctpl[
                        f.CampaignTemplateId];

                if (ct != null && string.IsNullOrEmpty(f.DefaultValue))
                {
                    AffiliateChannelTemplate atpl = null;

                    if (aftpl.ContainsKey(f.CampaignTemplateId))
                        atpl = aftpl[
                            f.CampaignTemplateId];

                    if (atpl == null) continue;

                    if (!ReceivedDataXmlDocFields.ContainsKey(atpl.SectionName + "_" + atpl.TemplateField)) continue;

                    datael = ReceivedDataXmlDocFields[atpl.SectionName + "_" + atpl.TemplateField];

                    if (datael == null)
                    {
                        continue;
                    }

                    dataElValue = datael.InnerText;

                    if (!string.IsNullOrEmpty(buyerChannel.RedirectUrl))
                        buyerChannel.RedirectUrl = buyerChannel.RedirectUrl.Replace($"%{ct.TemplateField}%", dataElValue);
                }
                else if (string.IsNullOrEmpty(f.DefaultValue))
                {
                    var datafield = XmlHelper.GetElementsByTagName(dataobj as XmlDocument, f.TemplateField);
                    if (datafield != null)
                    {
                        bool hasChildren = false;
                        foreach (XmlNode childNode in datafield.ChildNodes)
                        {
                            if (childNode.NodeType == XmlNodeType.Element)
                            {
                                hasChildren = true;
                                break;
                            }
                        }
                        if (!hasChildren)
                            datafield.InnerText = "";
                    }
                    continue;
                }
                else
                {
                    if (!SetXMLDataValue(context, dataobj, ct, buyerChannel, f, dataElValue, lead, buyer, true))
                    {
                        if (result.ResultType != RequestResult.ResultTypes.Success)
                        {
                            var res = result.ResultType;
                            var msg = result.Message;

                            AddResponse(context, lead, leadContent, campaign, buyerChannel, RequestResult.ResultTypes.Reject,
                                "Field '" + f.TemplateField + "' is required", errorType: RequestResult.ErrorTypes.FilterError,
                                responseTime: (Helpers.UtcNow() - utcNow).TotalMilliseconds, validator: ct.Validator);

                            if (res == RequestResult.ResultTypes.Success)
                                result.Set(res, RequestResult.ErrorTypes.None, msg);
                        }

                        RemoveBuyerChannel();

                        return false;
                    }

                    if (ct != null && !string.IsNullOrEmpty(buyerChannel.RedirectUrl))
                    {
                        buyerChannel.RedirectUrl = buyerChannel.RedirectUrl.Replace($"%{ct.TemplateField}%", dataElValue);
                    }
                    continue;
                }

                if (datael == null) continue;

                var filter = false;
                if (datael.Attributes["filter"] != null)
                    bool.TryParse(datael.Attributes["filter"].Value, out filter);

                if (ct.Validator == (short)Validators.SubId && buyerChannel.SubIdWhiteListEnabled.HasValue && buyerChannel.SubIdWhiteListEnabled.Value)
                {
                    string subIdValue = "";

                    if (context.HashedFieldValues.ContainsKey(datael.Name))
                        subIdValue = context.HashedFieldValues[datael.Name];

                    if (!string.IsNullOrEmpty(subIdValue) && context.SubIdWhiteListService.CheckSubIdWhiteList(subIdValue, buyerChannel.Id) == 0)
                    {
                        if (result.ResultType != RequestResult.ResultTypes.Success)
                        {
                            var res = result.ResultType;
                            var msg = result.Message;

                            AddResponse(context, lead, leadContent, campaign, buyerChannel, RequestResult.ResultTypes.Reject,
                            "Field '" + datael.Name + "' did not pass sub id white list", errorType: RequestResult.ErrorTypes.FilterError,
                            responseTime: (Helpers.UtcNow() - utcNow).TotalMilliseconds, validator: ct.Validator);

                            if (res == RequestResult.ResultTypes.Success)
                                result.Set(res, RequestResult.ErrorTypes.None, msg);
                        }

                        RemoveBuyerChannel();

                        return false;
                    }
                }


                if ((ct.Validator != (short)Validators.Zip || !buyerChannel.EnableZipCodeTargeting) && (ct.Validator != (short)Validators.State || !buyerChannel.EnableStateTargeting) &&
                    (ct.Validator != (short)Validators.DateOfBirth || !buyerChannel.EnableAgeTargeting))
                    if (!ValidateFilter(context, ct, f, dataElValue))
                    {
                        if (result.ResultType != RequestResult.ResultTypes.Success)
                        {
                            if (filter) throw new Exception(ct.TemplateField + " filter did not pass");

                            var res = result.ResultType;
                            var msg = result.Message;

                            AddResponse(context, lead, leadContent, campaign, buyerChannel, RequestResult.ResultTypes.Reject,
                            f.TemplateField + ": '" + dataElValue + "' does not match the criteria", errorType: RequestResult.ErrorTypes.FilterError,
                            responseTime: (Helpers.UtcNow() - utcNow).TotalMilliseconds, validator: ct.Validator);

                            if (res == RequestResult.ResultTypes.Success)
                                result.Set(res, RequestResult.ErrorTypes.None, msg);
                        }

                        RemoveBuyerChannel();

                        return false;
                    }


                if (ct != null)
                {
                    if ((bool)ct.IsHash)
                        nodesToHash.Add(datael.Name);

                    if (ct.Validator == (short)Validators.SubId)
                    {
                        nodesToHashInSent.Add(datael.Name);
                        //context.HashedFieldValues.Remove(datael.Name);
                    }
                }

                SetXMLDataValue(context, dataobj, ct, buyerChannel, f, dataElValue, lead, buyer);
            }

            if (!ValidateChildFilters(context, campaign, lead, leadContent, buyerChannel, utcNow))
            {
                return false;
            }

            totalPosts++;

            if (dataobj == null && campaign.CampaignType == (short)CampaignTypes.LeadCampaign)
            {
                if (result.ResultType != RequestResult.ResultTypes.Success)
                {
                    var res = result.ResultType;
                    var msg = result.Message;

                    AddResponse(context, lead, leadContent, campaign, buyerChannel, RequestResult.ResultTypes.Reject,
                    "Check the buyer channel integration", errorType: RequestResult.ErrorTypes.None,
                    responseTime: (Helpers.UtcNow() - utcNow).TotalMilliseconds);

                    if (res == RequestResult.ResultTypes.Success) result.Set(res, RequestResult.ErrorTypes.None, msg);
                }

                RemoveBuyerChannel();

                return false;
            }

            /*if (context.Extra.ContainsKey("LeadIdFieldCreated") && !((bool)context.Extra["LeadIdFieldCreated"]))
            {
                AppendLeadIdField(context, null, dataobj as XmlDocument, lead, buyer);
            }*/

            //var dataxml = dataobj == null ? "" : (dataobj as XmlDocument).OuterXml.Replace("UTF-16", "UTF-8");

            bool ignoreTimeout = false;

            if (campaign.CampaignType == (short)CampaignTypes.LeadCampaign && result.ResultType != RequestResult.ResultTypes.Success)
            {
                buyerChannel = ProcessTask(context, lead, leadContent, ls, buyerChannel, campaign, buyer,
                    dataobj == null ? null : dataobj as XmlDocument, nodesToHash, nodesToHashInSent, out ignoreTimeout);

                AffiliateChannel affiliateChannel = (AffiliateChannel)context.Extra["informat"];

                if (!ignoreTimeout && buyer.AlwaysSoldOption == 0 && (context.PastSeconds(StartDateUtc) >= buyerChannel.Timeout + buyerChannel.AfterTimeout || (affiliateChannel.Timeout.HasValue && affiliateChannel.Timeout.Value > 0 && context.PastSeconds() >= affiliateChannel.Timeout.Value)) && result.ErrorType != RequestResult.ErrorTypes.BuyerChannelResponseError)
                {
                    if (result.ResultType != RequestResult.ResultTypes.Success)
                    {
                        var res = result.ResultType;
                        var msg = result.Message;


                        RequestResult.ErrorTypes timeoutErrorType = RequestResult.ErrorTypes.RequestTimeoutError;

                        if (buyerChannel.TimeoutNotification && !string.IsNullOrEmpty(buyerChannel.NotificationEmail))
                        {
                            bool canChangeStatus = (buyerChannel.StatusAutoChange.HasValue ? buyerChannel.StatusAutoChange.Value : false);
                            short statusChangeMinutes = (buyerChannel.StatusChangeMinutes.HasValue ? buyerChannel.StatusChangeMinutes.Value : (short)10);

                            string timeoutMessage = "";

                            if (!buyerChannel.CurrentStatusChangeNum.HasValue)
                                buyerChannel.CurrentStatusChangeNum = 0;

                            if (!buyerChannel.ChangeStatusAfterCount.HasValue)
                                buyerChannel.ChangeStatusAfterCount = 0;

                            if (canChangeStatus)
                            {
                                if (buyerChannel.CurrentStatusChangeNum >= buyerChannel.ChangeStatusAfterCount)
                                {
                                    timeoutErrorType = RequestResult.ErrorTypes.RequestTimeoutPausedError;

                                    buyerChannel.StatusExpireDate = DateTime.UtcNow.AddMinutes(statusChangeMinutes);
                                    buyerChannel.StatusStr = "Channel timeout";
                                    buyerChannel.Status = BuyerChannelStatuses.Paused;
                                    buyerChannel.CurrentStatusChangeNum = 0;
                                    context.BuyerChannelService.UpdateBuyerChannel(buyerChannel);
                                    timeoutMessage = "The channel will be paused for " + statusChangeMinutes.ToString() + " minutes. It will be active after that time.";


                                    string[] emails = buyerChannel.NotificationEmail.Trim().Split(new char[1] { ',' });
                                    foreach (string email in emails)
                                    {
                                        context.EmailService.SendTimeoutNotification(buyer.Name, buyerChannel.Name, buyer.Name, timeoutMessage, email.Trim(), EmailOperatorEnums.LeadNative);
                                    }
                                }
                                else
                                {
                                    buyerChannel.CurrentStatusChangeNum++;
                                    context.BuyerChannelService.UpdateBuyerChannel(buyerChannel);
                                }
                            }
                        }

                        AddResponse(context, lead, leadContent, campaign, buyerChannel, RequestResult.ResultTypes.Reject,
                        "Channel timeout", errorType: RequestResult.ErrorTypes.None,
                        responseTime: (Helpers.UtcNow() - utcNow).TotalMilliseconds);

                        if (res == RequestResult.ResultTypes.Success)
                            result.Set(res, RequestResult.ErrorTypes.None, msg);
                    }

                    RemoveBuyerChannel();

                    return false;
                }
            }
            else if (!ignoreTimeout)
            {
                if (result.ResultType != RequestResult.ResultTypes.Success)
                {
                    var res = result.ResultType;
                    var msg = result.Message;

                    AddResponse(context, lead, leadContent, campaign, buyerChannel, RequestResult.ResultTypes.Reject,
                    "Channel timeout", errorType: RequestResult.ErrorTypes.None,
                    responseTime: (Helpers.UtcNow() - utcNow).TotalMilliseconds);

                    if (buyerChannel.TimeoutNotification)
                    {
                        string[] emails = buyerChannel.NotificationEmail.Split(new char[1] { ',' });
                        foreach (string email in emails)
                        {
                            context.EmailService.SendTimeoutNotification(buyer.Name, buyerChannel.Name, buyer.Name, "", email, EmailOperatorEnums.LeadNative);
                        }
                    }

                    if (res == RequestResult.ResultTypes.Success)
                        result.Set(res, RequestResult.ErrorTypes.None, msg);
                }

                RemoveBuyerChannel();
            }

            lock (timerLock)
            {
                PastSeconds = 0;
            }

            return true;
        }

        #endregion Private methods

        #region Public methods

        public ExportManager()
        {
            MaxResponsePrice = 0;
        }

        /// <summary>
        ///     Updates the buyer.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="campaign">The campaign.</param>
        /// <param name="buyer">The buyer.</param>
        /// <param name="lead">The lead.</param>
        private void UpdateBuyer(RequestContext context, Campaign campaign, Buyer buyer, LeadMain lead)
        {
            //if (campaign.CampaignType != 0) return;
            //buyer.LastPosted = Helpers.UtcNow();
            //entityBuyerUpdates.Add(buyer);
            //context.BuyerService.UpdateBuyer(buyer);
        }

        /// <summary>
        ///     Inserts the posted data.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="lead">The lead.</param>
        /// <param name="leadContent">Content of the lead.</param>
        /// <param name="buyerChannel">The buyer channel.</param>
        /// <param name="posted">The posted.</param>
        protected void InsertPostedData(RequestContext context, LeadMain lead, LeadContent leadContent,
            BuyerChannel buyerChannel, string posted, RequestResult.ResultTypes status)
        {
            var postedDataEntity = new PostedData
            {
                BuyerChannelId = buyerChannel.Id,
                Created = Helpers.UtcNow(),
                LeadId = lead.Id,
                Posted = posted,
                MinPrice = (decimal)leadContent.Minprice,
                Status = (short)status
            };
            entityPostedData.Add(postedDataEntity);
            //context.PostedDataService.InsertPostedData(pd);
        }

        List<LeadMainResponse> entityListMainResponse = new List<LeadMainResponse>();
        List<PostedData> entityPostedData = new List<PostedData>();
        List<BuyerResponse> entityBuyerResponse = new List<BuyerResponse>();
        List<Buyer> entityBuyerUpdates = new List<Buyer>();

        /// <summary>
        ///     Inserts the lead main response.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="format">The format.</param>
        /// <param name="lead">The lead.</param>
        /// <param name="leadContent">Content of the lead.</param>
        /// <param name="now">The now.</param>
        /// <param name="responseTime">The response time.</param>
        /// <param name="response">The response.</param>
        /// <param name="responseError">The response error.</param>
        /// <param name="buyerPrice">The buyer price.</param>
        /// <param name="affiliatePrice">The affiliate price.</param>
        /// <param name="status">The status.</param>
        /// <param name="errorType">Type of the error.</param>
        /// <param name="validator">The validator.</param>
        private void InsertLeadMainResponse(RequestContext context, BuyerChannel format, LeadMain lead,
            LeadContent leadContent, DateTime now, double responseTime, string response, string responseError,
            decimal buyerPrice, decimal affiliatePrice, RequestResult.ResultTypes status,
            RequestResult.ErrorTypes errorType, short? validator)
        {
            var leadResponse = new LeadMainResponse
            {
                ResponseTime = responseTime,
                Response = response,
                BuyerId = format.BuyerId,
                ResponseError = responseError,
                LeadId = lead.Id,
                BuyerPrice = buyerPrice,
                AffiliatePrice = 0,
                BuyerChannelId = format.Id,
                AffiliateChannelId = (context.Extra["informat"] as AffiliateChannel).Id,
                State = leadContent.State,
                Status = (short)status,
                ErrorType = (short)errorType,
                AffiliateId = lead.AffiliateId,
                CampaignId = lead.CampaignId,
                Created = now, //((DateTime)context.Extra["StartDate"]).AddMilliseconds((DateTime.UtcNow - now).TotalMilliseconds);//TZ
                CampaignType = lead.CampaignType,
                MinPrice = (decimal)leadContent.Minprice,
                Validator = validator,
                Ssn = (leadContent != null ? leadContent.Ssn : "")
            };
            entityListMainResponse.Add(leadResponse);
            //context.LeadMainResponseService.InsertLeadMainResponse(leadResponse);
        }

        /// <summary>
        ///     Validates the cool off period.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="obj">The object.</param>
        /// <param name="format">The format.</param>
        /// <param name="buyer">The buyer.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ValidateCoolOffPeriod(RequestContext context, object obj, BuyerChannel format, Buyer buyer)
        {

            if (buyer.CoolOffEnabled.HasValue && buyer.CoolOffEnabled.Value && buyer.CoolOffStart.HasValue &&
                buyer.CoolOffEnd.HasValue)
            {
                var created = ((LeadMain)obj).Created;
                created = context.LeadCreated ??
                          context.SettingService.GetTimeZoneDate(created, null, context.TimeZoneSetting);

                if (created >= buyer.CoolOffStart.Value && created <= buyer.CoolOffEnd.Value)
                    return false;
            }

            return true;
        }

        private readonly double TOLERANCE = 0.000000001;

        /// <summary>
        ///     Validates the holiday.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="campaign">The campaign.</param>
        /// <param name="obj">The object.</param>
        /// <param name="buyerChannel">The format.</param>
        /// <param name="buyer">The buyer.</param>
        /// <returns>System.Decimal.</returns>
        public string ValidateHoliday(RequestContext context, Campaign campaign, object obj, BuyerChannel buyerChannel,
            Buyer buyer)
        {
            var country = context.CountryService.GetCountryById(buyerChannel.CountryId.HasValue ? buyerChannel.CountryId.Value : 80);
            var createdOriginal = ((LeadMain)obj).Created;

            var systemTimeZone = TimeZoneInfo.FindSystemTimeZoneById(buyerChannel.TimeZone);
            var customTimeZone = systemTimeZone != null ? systemTimeZone.BaseUtcOffset.TotalHours : 0;
            var createdTimeZone = context.SettingService.GetTimeZoneDate(createdOriginal, null, context.TimeZoneSetting, customTimeZone);

            var holidays = context.BuyerChannelService.GetBuyerChannelHolidays(buyerChannel.Id);

            foreach(var holiday in holidays)
            {
                DateTime dateTime = holiday.HolidayDate;
                PublicHoliday publicHoliday = null;

                publicHoliday = DateSystem.GetPublicHoliday(buyerChannel.HolidayYear, country.TwoLetteroCode.ToUpper()).Where(x => x.Name == holiday.Name).FirstOrDefault();
                if (publicHoliday != null)
                {
                    dateTime = publicHoliday.Date;
                }

                if (dateTime.Year == createdTimeZone.Year &&
                    dateTime.Month == createdTimeZone.Month &&
                    dateTime.Day == createdTimeZone.Day)
                {
                    return $"Holiday day: {holiday.Name}";
                }
            }

            return "";
        }

        /// <summary>
        ///     Validates the schedule.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="campaign">The campaign.</param>
        /// <param name="obj">The object.</param>
        /// <param name="buyerChannel">The format.</param>
        /// <param name="buyer">The buyer.</param>
        /// <returns>System.Decimal.</returns>
        public decimal ValidateSchedule(RequestContext context, Campaign campaign, object obj, BuyerChannel buyerChannel,
            Buyer buyer)
        {
            var createdOriginal = ((LeadMain)obj).Created;
            var dateCalc = false;
            decimal schedulePrice = 0;
            TimeSpan ts;

            var systemTimeZone = TimeZoneInfo.FindSystemTimeZoneById(buyerChannel.TimeZone);
            var customTimeZone = systemTimeZone != null ? systemTimeZone.BaseUtcOffset.TotalHours : 0;
            var createdTimeZone = context.SettingService.GetTimeZoneDate(createdOriginal, null, context.TimeZoneSetting, customTimeZone);

            var buyerChannelScheduleDays = context.BuyerChannelService.GetBuyerChannelScheduleDays(buyerChannel.Id);

            foreach(var buyerChannelSchedule in buyerChannelScheduleDays)
            {
                if (buyerChannelSchedule.NoLimit) continue;

                if (createdTimeZone.DayOfWeek == (DayOfWeek)(buyerChannelSchedule.DayValue - 1))
                {
                    var buyerChannelScheduleTimePeriods = context.BuyerChannelService.GetBuyerChannelScheduleTimePeriod(buyerChannelSchedule.Id);

                    foreach(var buyerChannelScheduleTimePeriod in buyerChannelScheduleTimePeriods)
                    {
                        if (buyerChannelScheduleTimePeriod.Quantity < 0) continue;

                        if (buyerChannelScheduleTimePeriod.Price.HasValue)
                            schedulePrice = (decimal)buyerChannelScheduleTimePeriod.Price;
                        else
                            schedulePrice = 0;

                        var h = (double)Math.Floor((decimal)buyerChannelScheduleTimePeriod.FromTime / 60);
                        if (Math.Abs(h - 24) < TOLERANCE) h = 0;
                        var span = TimeSpan.FromMinutes(buyerChannelScheduleTimePeriod.FromTime);
                        var fromDate = new DateTime(createdTimeZone.Year, createdTimeZone.Month, createdTimeZone.Day, (int)h, span.Minutes,
                            span.Seconds);

                        h = (double)Math.Floor((decimal)buyerChannelScheduleTimePeriod.ToTime / 60);
                        span = TimeSpan.FromMinutes(buyerChannelScheduleTimePeriod.ToTime);
                        var hh = h;
                        if (h == 24) h = 0;

                        var toDate = new DateTime(createdTimeZone.Year, createdTimeZone.Month, createdTimeZone.Day, (int)h, span.Minutes,
                            span.Seconds);
                        if (hh == 24 || hh == 0) toDate = toDate.AddDays(1);

                        /*if (customTimeZone != 0)
                        {
                            fromDate = context.SettingService.GetUTCDate(fromDate, null, context.TimeZoneSetting, customTimeZone);
                            toDate = context.SettingService.GetUTCDate(toDate, null, context.TimeZoneSetting, customTimeZone);
                            //now move from UTC to system time zone
                            fromDate = context.SettingService.GetTimeZoneDate(fromDate, null, context.TimeZoneSetting, customTimeZone);
                            toDate = context.SettingService.GetTimeZoneDate(toDate, null, context.TimeZoneSetting, customTimeZone);
                        }*/

                        if (createdTimeZone < fromDate || createdTimeZone > toDate) return -1;

                        var qty = context.LeadMainResponseService.GetLeadsCountByPeriod(buyerChannel.Id, fromDate, toDate,
                            (buyerChannelScheduleTimePeriod.LeadStatus.HasValue ? buyerChannelScheduleTimePeriod.LeadStatus.Value : (short)-1));

                        if (qty >= buyerChannelScheduleTimePeriod.Quantity && buyerChannelScheduleTimePeriod.Quantity > 0) return -1;

                        if (buyerChannelScheduleTimePeriod.HourMax > 0)
                        {
                            var mqty = context.LeadMainResponseService.GetLeadsCountByPeriod(buyerChannel.Id,
                                createdTimeZone.AddHours(-1), createdTimeZone, (buyerChannelScheduleTimePeriod.LeadStatus.HasValue ? buyerChannelScheduleTimePeriod.LeadStatus.Value : (short)-1));

                            if (mqty > buyerChannelScheduleTimePeriod.HourMax && buyerChannelScheduleTimePeriod.HourMax > 0) return -1;
                        }

                        if (buyer.LastPostedSold != null)
                        {
                            ts = Helpers.UtcNow() - (DateTime)buyer.LastPostedSold;
                            if (ts.TotalSeconds < buyerChannelScheduleTimePeriod.SoldWait && buyerChannelScheduleTimePeriod.SoldWait > 0) return -1;
                        }

                        if (buyer.LastPosted != null)
                        {
                            ts = Helpers.UtcNow() - (DateTime)buyer.LastPosted;
                            if (ts.TotalSeconds < buyerChannelScheduleTimePeriod.PostedWait && buyerChannelScheduleTimePeriod.PostedWait > 0) return -1;
                        }
                    }
                }
            }

            return schedulePrice;
        }

        /// <summary>
        ///     Validates the minimum price.
        /// </summary>
        /// <param name="campaign">The campaign.</param>
        /// <param name="buyerChannel">The buyer channel.</param>
        /// <param name="affiliateChannel">The affiliate channel.</param>
        /// <param name="leadContent">Content of the lead.</param>
        /// <returns>System.Decimal.</returns>
        private decimal ValidateMinPrice(RequestContext context, Campaign campaign, BuyerChannel buyerChannel,
            AffiliateChannel affiliateChannel, LeadContent leadContent)
        {
            var AffiliatePrice = buyerChannel.AffiliatePrice;
            short AffiliatePriceOption = buyerChannel.AffiliatePriceOption;

            if (AffiliatePriceOption == (short)BuyerChannelPriceMethods.TakeFromAffilateChannel)
            {
                short affiliatePriceMethod = (affiliateChannel.AffiliatePriceMethod.HasValue ? affiliateChannel.AffiliatePriceMethod.Value : (short)0);
                AffiliatePrice = (affiliateChannel.AffiliatePrice.HasValue ? affiliateChannel.AffiliatePrice.Value : 0);

                switch (affiliatePriceMethod)
                {
                    case (short)AffiliateChannelPriceMethods.Fixed: AffiliatePrice = (affiliateChannel.AffiliatePrice.HasValue ? affiliateChannel.AffiliatePrice.Value : 0); break;
                    case (short)AffiliateChannelPriceMethods.Revshare: AffiliatePriceOption = 1; break;
                    case (short)AffiliateChannelPriceMethods.TakeFromAffilate:
                        affiliatePriceMethod = (context.Affiliate.DefaultAffiliatePriceMethod.HasValue ? context.Affiliate.DefaultAffiliatePriceMethod.Value : (short)0);

                        if (affiliatePriceMethod == (short)AffiliateChannelPriceMethods.Fixed)
                        {
                            AffiliatePrice = (context.Affiliate.DefaultAffiliatePrice.HasValue ? context.Affiliate.DefaultAffiliatePrice.Value : 0); break;
                        }
                        else
                        {
                            AffiliatePriceOption = (short)AffiliateChannelPriceMethods.Revshare;
                        }
                        break;
                }
            }

            if (AffiliatePriceOption == (short)AffiliateChannelPriceMethods.Revshare)
            {
                if (buyerChannel.BuyerPriceOption == (short)BuyerChannelPriceMethods.Fixed)
                    if (AffiliatePrice + affiliateChannel.NetworkMinimumRevenue > buyerChannel.BuyerPrice) return -1;
            }
            else
            {
                if (leadContent.Minprice > AffiliatePrice) return -1;
            }

            return AffiliatePrice;
        }

        private void CalculatePrices(RequestContext context, LeadContent leadContent, BuyerChannel buyerChannel, AffiliateChannel affiliateChannel, decimal responsePrice, out decimal affiliatePrice, out decimal buyerPrice)
        {
            affiliatePrice = 0;
            buyerPrice = 0;

            buyerPrice = buyerChannel.BuyerPriceOption == 0 ? buyerChannel.BuyerPrice : responsePrice;

            if (buyerChannel.AlwaysBuyerPrice.HasValue && buyerChannel.AlwaysBuyerPrice.Value)
            {
                affiliatePrice = buyerPrice;
            }

            short AffiliatePriceOption = buyerChannel.AffiliatePriceOption;
            decimal lAffiliatePrice2 = buyerChannel.AffiliatePrice;

            if (AffiliatePriceOption == (short)BuyerChannelPriceMethods.TakeFromAffilateChannel)
            {
                short affiliatePriceMethod = (affiliateChannel.AffiliatePriceMethod.HasValue ? affiliateChannel.AffiliatePriceMethod.Value : (short)0);
                lAffiliatePrice2 = (affiliateChannel.AffiliatePrice.HasValue ? affiliateChannel.AffiliatePrice.Value : 0);

                switch (affiliatePriceMethod)
                {
                    case (short)AffiliateChannelPriceMethods.Fixed: lAffiliatePrice2 = (affiliateChannel.AffiliatePrice.HasValue ? affiliateChannel.AffiliatePrice.Value : 0); break;
                    case (short)AffiliateChannelPriceMethods.Revshare: AffiliatePriceOption = 1; break;
                    case (short)AffiliateChannelPriceMethods.TakeFromAffilate:
                        affiliatePriceMethod = (context.Affiliate.DefaultAffiliatePriceMethod.HasValue ? context.Affiliate.DefaultAffiliatePriceMethod.Value : (short)0);

                        if (affiliatePriceMethod == (short)AffiliateChannelPriceMethods.Fixed)
                        {
                            lAffiliatePrice2 = (context.Affiliate.DefaultAffiliatePrice.HasValue ? context.Affiliate.DefaultAffiliatePrice.Value : 0); break;
                        }
                        else
                        {
                            AffiliatePriceOption = (short)AffiliateChannelPriceMethods.Revshare;
                        }
                        break;
                }
            }

            if (AffiliatePriceOption == (short)AffiliateChannelPriceMethods.Revshare)
            {
                if (buyerPrice -
                    (buyerPrice * affiliateChannel.NetworkTargetRevenue / 100) -
                    (buyerPrice * (100 - lAffiliatePrice2) / 100)
                    >=
                    leadContent.Minprice)
                    affiliatePrice = buyerPrice -
                        (buyerPrice * affiliateChannel.NetworkTargetRevenue / 100) -
                        (buyerPrice * (100 - lAffiliatePrice2) / 100);
                else
                    affiliatePrice = (decimal)leadContent.Minprice;
            }
            else
            {
                if (lAffiliatePrice2 >= leadContent.Minprice)
                    affiliatePrice = lAffiliatePrice2;
                else
                    affiliatePrice = (decimal)leadContent.Minprice;
            }
        }

        /// <summary>
        ///     Checks the credit.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="lead">The lead.</param>
        /// <param name="leadContent">Content of the lead.</param>
        /// <param name="campaign">The campaign.</param>
        /// <param name="buyerChannel">The buyer channel.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool CheckCredit(RequestContext context, LeadMain lead, LeadContent leadContent, Campaign campaign,
            BuyerChannel buyerChannel, int testFalse = 0)
        {

            if (testFalse == 1 || campaign.CampaignType == (short)CampaignTypes.LeadCampaign && !context.AccountingService.CheckCredit(buyerChannel.BuyerId))
            {
                GenerateCreditResponse(context, lead, leadContent, campaign, buyerChannel);
                return false;
            }

            return true;
        }

        private void GenerateCreditResponse(RequestContext context, LeadMain lead, LeadContent leadContent, Campaign campaign,
            BuyerChannel buyerChannel)
        {
            InsertPostedData(context, lead, leadContent, buyerChannel, "", RequestResult.ResultTypes.Reject);
            InsertLeadMainResponse(context, buyerChannel, lead, leadContent, Helpers.UtcNow(), 0,
                "Not enough balance", "Not enough balance", buyerChannel.BuyerPrice, buyerChannel.AffiliatePrice,
                RequestResult.ResultTypes.Reject, RequestResult.ErrorTypes.NotEnoughBalance, null);

            lead.Status = (short)RequestResult.ResultTypes.Reject;
            Result.Set(RequestResult.ResultTypes.Reject, RequestResult.ErrorTypes.NotEnoughBalance,
                "Lead was not sold in the marketplace");
        }



        public void SaveDatabaseChanges(RequestContext context)
        {
            context.PostedDataService.InsertPostedDataList(entityPostedData);
            context.LeadMainResponseService.InsertLeadMainResponseList(entityListMainResponse);
            context.BuyerResponseService.InsertBuyerResponseList(entityBuyerResponse);
            //context.BuyerService.UpdateBuyerList(entityBuyerUpdates);
        }
        Hashtable buyerCredits;
        Hashtable priceRejectLocks;

        protected bool CheckPrioritizedFilter(BuyerChannel buyerChannel, LeadContent leadContent)
        {
            if (buyerChannel.EnableZipCodeTargeting)
            {
                switch(buyerChannel.ZipCodeCondition)
                {
                    case (short)Conditions.Contains:
                        if (!buyerChannel.ZipCodeTargeting.Contains(leadContent.Zip)) return false; break;
                    case (short)Conditions.NotContains:
                        if (buyerChannel.ZipCodeTargeting.Contains(leadContent.Zip)) return false; break;
                    case (short)Conditions.StartsWith:
                        if (!buyerChannel.ZipCodeTargeting.StartsWith(leadContent.Zip)) return false; break;
                    case (short)Conditions.EndsWith:
                        if (!buyerChannel.ZipCodeTargeting.EndsWith(leadContent.Zip)) return false; break;
                    case (short)Conditions.Equals: if (buyerChannel.ZipCodeTargeting != leadContent.Zip) return false; break;
                    case (short)Conditions.NotEquals: if (buyerChannel.ZipCodeTargeting == leadContent.Zip) return false; break;
                }
            }

            if (buyerChannel.EnableStateTargeting)
            {
                switch (buyerChannel.StateCondition)
                {
                    case (short)Conditions.Contains:
                        if (!buyerChannel.StateTargeting.Contains(leadContent.State)) return false; break;
                    case (short)Conditions.NotContains:
                        if (buyerChannel.StateTargeting.Contains(leadContent.State)) return false; break;
                    case (short)Conditions.StartsWith:
                        if (!buyerChannel.StateTargeting.StartsWith(leadContent.State)) return false; break;
                    case (short)Conditions.EndsWith:
                        if (!buyerChannel.StateTargeting.EndsWith(leadContent.State)) return false; break;
                    case (short)Conditions.Equals: if (buyerChannel.StateTargeting != leadContent.State) return false; break;
                    case (short)Conditions.NotEquals: if (buyerChannel.StateTargeting == leadContent.State) return false; break;
                }
            }

            if (buyerChannel.EnableAgeTargeting && ((buyerChannel.MinAgeTargeting > 0 && leadContent.Age < buyerChannel.MinAgeTargeting) || (buyerChannel.MaxAgeTargeting > 0 && leadContent.Age > buyerChannel.MaxAgeTargeting)))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Processes the data.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="lead">The lead.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ProcessData(RequestContext context, LeadMain lead)
        {
            CustomPriceRejectUrl = "";

            if (!context.MinProcessingMode)
            {
                /*timerToken = timerTokenSource.Token;
                timerTask = Task.Factory.StartNew(() =>
                {
                    while (!timerToken.IsCancellationRequested)
                    {
                        lock (timerLock)
                        {
                            PastSeconds++;
                            PastTotalSeconds++;
                        }

                        Thread.Sleep(1000);
                    }
                }, timerToken);*/
            }
            matchedBuyers.Clear();

            var leadContent = (LeadContent)context.Extra["leadContent"];

            result.Set(RequestResult.ResultTypes.Reject, RequestResult.ErrorTypes.None, "Lead was not sold in the marketplace");

            TimeSpan ts;

            var campaign = context.CampaignService.GetCampaignById(lead.CampaignId);
            var campaignXML = context.CampaignService.GetCampaignByIdXml(lead.CampaignId);

            if (campaignXML == null)
            {
                return false;
            }

            var ls = new LeadMainResponse();
            pingTreeChannels = GetBuyerChannels(context, lead, leadContent).ToList();

            BuyerChannelTemplate[] buyerChannelTemplates = null;

            var affiliateChannel = (AffiliateChannel)context.Extra["informat"];

            lock (timerLock)
            {
                PastTotalSeconds = 0;
            }

            if (pingTreeChannels.Count == 0)
            {
                context.Extra["AffiliateResponseMessage"] =
                    "No active channels found. Maybe one of target filters did not pass (ZIP,AGE,STATE)";
                result.Set(RequestResult.ResultTypes.Reject, RequestResult.ErrorTypes.None, "");

                return false;
            }

            long bChannelId;
            long.TryParse(context.Extra["BuyerChannelId"].ToString(), out bChannelId);

            var buyerChannelProcessed = false;

            var buyersHash = new Hashtable();

            /*var cycleSize = campaign.PingTreeCycle.HasValue ? campaign.PingTreeCycle.Value : 0;
            if (cycleSize > 0)
                SharedData.ResetBuyerChannelLeadsCount(lead.CampaignId, buyerChannels, (int)cycleSize);*/

            SharedData.ResetBuyerChannelLeadsCount(pingTreeChannels.Select(x => x.PingTreeItem).ToList());

            buyerCredits = new Hashtable();
            priceRejectLocks = new Hashtable();


            while (pingTreeChannels.Count > 0)
            {
                StartDateUtc = Helpers.UtcNow();

                MappingFields.Clear();

                var pingTreeChannel = pingTreeChannels[0];
                var buyerChannel = pingTreeChannels[0].BuyerChannel;
                var pingTreeItem = pingTreeChannels[0].PingTreeItem;

                if (buyerChannel.Status == BuyerChannelStatuses.Paused)
                {
                    bool canChangeStatus = (buyerChannel.StatusAutoChange.HasValue ? buyerChannel.StatusAutoChange.Value : false);
                    short statusChangeMinutes = (buyerChannel.StatusChangeMinutes.HasValue ? buyerChannel.StatusChangeMinutes.Value : (short)10);

                    if (canChangeStatus && 
                        buyerChannel.StatusExpireDate.HasValue && 
                        (DateTime.UtcNow - buyerChannel.StatusExpireDate.Value).TotalSeconds > statusChangeMinutes)
                    {
                        buyerChannel.Status = BuyerChannelStatuses.Active;
                        buyerChannel.StatusExpireDate = null;
                        buyerChannel.StatusStr = "";
                        context.BuyerChannelService.UpdateBuyerChannel(buyerChannel);
                    }
                    else
                    {
                        RemoveBuyerChannel();
                        continue;
                    }
                }

                if (result.ResultType == RequestResult.ResultTypes.Success)
                {
                    break;
                }

                if(buyerChannel.BuyerPrice > 0 && leadContent.Minprice > 0 && buyerChannel.BuyerPrice < leadContent.Minprice)
                {
                    RemoveBuyerChannel();
                    continue;
                }

                if (context.Extra.ContainsKey("threeMinDublLeadId"))
                {
                    long threeMinDublLeadId = 0;
                    if (long.TryParse(context.Extra["threeMinDublLeadId"].ToString(), out threeMinDublLeadId))
                    {
                        if (context.LeadMainResponseService.GetDublicateLeadByBuyer("", DateTime.UtcNow, buyerChannel.Id, false, threeMinDublLeadId))
                        {
                            RemoveBuyerChannel();
                            continue;
                        }
                    }
                }

                var buyerid = buyerChannel.BuyerId;

                var buyer = (Buyer)buyersHash[buyerid];

                if (buyer == null)
                {
                    buyer = context.BuyerService.GetBuyerById(buyerid);
                    buyersHash[buyerid] = buyer;
                }

                if (context.PrioritizedEnabled && !CheckPrioritizedFilter(buyerChannel, leadContent))
                {
//                    if (cycleSize > 0)
                        if (SharedData.CheckBuyerChannelLeadsCount(pingTreeItem))
                            SharedData.DecrementBuyerChannelLeadsCount(pingTreeItem);
                    RemoveBuyerChannel();
                    continue;
                }

                if (!context.MinProcessingMode)
                {
                    if (!context.BuyerChannelService.CheckAllowedAffiliateChannel(affiliateChannel.Id, buyerChannel.Id)
                        &&
                        context.Extra["mode"].ToString() != "test")
                    {
                        //if (cycleSize > 0)
                            if (SharedData.CheckBuyerChannelLeadsCount(pingTreeItem))
                                SharedData.DecrementBuyerChannelLeadsCount(pingTreeItem);

                        RemoveBuyerChannel();
                        continue;
                    }
                }

                var alreadyPosted = context.LeadMainResponseService.GetLeadMainResponsesByLeadIdBuyerChannelId(lead.Id, buyerChannel.Id);
                if (alreadyPosted != null && alreadyPosted.Status != (short)RequestResult.ResultTypes.Error)
                {
                    if (pingTreeChannels.Count > 0)
                        pingTreeChannels.RemoveAt(0);
                    continue;
                }

                buyerChannelProcessed = true;

                try
                {
                    if (!string.IsNullOrEmpty(buyerChannel.ChildChannels)) //super tier checking
                    {
                        string[] strs = buyerChannel.ChildChannels.Split(new char[1] { ',' });
                        List<long> representTiers = new List<long>();
                        long l = 0;

                        foreach (string s in strs)
                        {
                            if (long.TryParse(s, out l))
                            {
                                representTiers.Add(l);
                            }
                        }

                        if (representTiers.Count > 0)
                            SuperTierChannels[buyerChannel.Id] = representTiers;
                    }
                    else
                    if (SuperTierChannels.Count > 0)
                    {
                        bool representTierFound = false;
                        foreach (long key in SuperTierChannels.Keys)
                        {
                            if (SuperTierChannels[key].Contains(buyerChannel.Id))
                            {
                                representTierFound = true;
                                break;
                            }
                        }

                        if (representTierFound)
                        {
                            //if (cycleSize > 0)
                                if (SharedData.CheckBuyerChannelLeadsCount(pingTreeItem))
                                    SharedData.DecrementBuyerChannelLeadsCount(pingTreeItem);
                            RemoveBuyerChannel();
                            continue;
                        }
                    }

                    //end super tier checking
                    //ping tree checking
                    //if (cycleSize > 0)
                    {
                        if (!SharedData.CheckBuyerChannelLeadsCount(pingTreeItem))
                        {
                            RemoveBuyerChannel();
                            continue;
                        }

                        SharedData.DecrementBuyerChannelLeadsCount(pingTreeItem);
                    }

                    if (bChannelId > 0 && buyerChannel.Id != bChannelId)
                    {
                        RemoveBuyerChannel();
                        continue;
                    }

                    if (buyerChannel.Status != BuyerChannelStatuses.Active || (buyer != null && buyer.Status != (short)ActivityStatuses.Active))
                    {
                        RemoveBuyerChannel();
                        continue;
                    }

                    ls = new LeadMainResponse();

                    if (!context.MinProcessingMode)
                    {
                        if (buyerCredits[buyerChannel.BuyerId] == null)
                        {
                            if (!CheckCredit(context, lead, leadContent, campaign, buyerChannel))
                            {
                                buyerCredits[buyerChannel.BuyerId] = false;
                                RemoveBuyerChannel();
                                continue;
                            }
                            else
                                buyerCredits[buyerChannel.BuyerId] = true;
                        }
                        else
                            if ((bool)buyerCredits[buyerChannel.BuyerId] == false)
                        {
                            GenerateCreditResponse(context, lead, leadContent, campaign, buyerChannel);
                            RemoveBuyerChannel();
                            continue;
                        }
                    }

                    object obj = lead;

                    if (!context.MinProcessingMode && !ValidateCoolOffPeriod(context, obj, buyerChannel, buyer))
                        if (campaign.CampaignType == (short)CampaignTypes.LeadCampaign)
                        {
                            AddResponse(context, lead, leadContent, campaign, buyerChannel, RequestResult.ResultTypes.ScheduleError,
                            "Cool off period", postedDataResultType: RequestResult.ResultTypes.Reject, errorType: RequestResult.ErrorTypes.ScheduleCapLimit, affiliatePrice: buyerChannel.BuyerPrice, buyerPrice: buyerChannel.AffiliatePrice);

                            RemoveBuyerChannel();
                            continue;
                        }

                    var schedulePrice = context.MinProcessingMode ? 0 : ValidateSchedule(context, campaign, obj, buyerChannel, buyer);

                    if (schedulePrice == -1)
                    {
                        AddResponse(context, lead, leadContent, campaign, buyerChannel, RequestResult.ResultTypes.ScheduleError,
                        "Schedule cap limit", postedDataResultType: RequestResult.ResultTypes.ScheduleError, errorType: RequestResult.ErrorTypes.ScheduleCapLimit,
                        affiliatePrice: buyerChannel.BuyerPrice, buyerPrice: buyerChannel.AffiliatePrice);

                        if (buyerChannel.CapReachedNotification)
                        {
                            string[] emails = buyerChannel.NotificationEmail.Split(new char[1] { ',' });
                            foreach (string email in emails)
                            {
                                context.EmailService.SendCapReachNotification(buyer.Name, buyerChannel.Name, buyer.Name, email, EmailOperatorEnums.LeadNative);
                            }
                        }

                        RemoveBuyerChannel();
                        continue;
                    }

                    var holidayMessage = context.MinProcessingMode ? "" : ValidateHoliday(context, campaign, obj, buyerChannel, buyer);

                    if (!string.IsNullOrEmpty(holidayMessage))
                    {
                        AddResponse(context, lead, leadContent, campaign, buyerChannel, RequestResult.ResultTypes.ScheduleError,
                        holidayMessage, postedDataResultType: RequestResult.ResultTypes.ScheduleError, errorType: RequestResult.ErrorTypes.ScheduleCapLimit,
                        affiliatePrice: buyerChannel.BuyerPrice, buyerPrice: buyerChannel.AffiliatePrice);

                        RemoveBuyerChannel();
                        continue;
                    }

                    var AffiliatePrice = schedulePrice;

                    if (campaign.CampaignType == (short)CampaignTypes.LeadCampaign)
                    {
                        AffiliatePrice = context.MinProcessingMode ? 0 : ValidateMinPrice(context, campaign, buyerChannel, affiliateChannel, leadContent);

                        if (AffiliatePrice == -1 && result.ResultType != RequestResult.ResultTypes.Success)
                        {
                            var rt = result.ResultType;
                            var message = result.Message;

                            AddResponse(context, lead, leadContent, campaign, buyerChannel, RequestResult.ResultTypes.Reject,
                            "Min price error", errorType: RequestResult.ErrorTypes.None);

                            if (rt == RequestResult.ResultTypes.Success)
                                result.Set(rt, RequestResult.ErrorTypes.None, message);

                            RemoveBuyerChannel();
                            continue;
                        }


                        var dnpMessage = "";
                        bool doNotPresentPassed = ValidateDoNotPresent(leadContent, context, buyer, out dnpMessage);
                        if (!doNotPresentPassed)
                        {
                            var rt = result.ResultType;
                            var message = result.Message;

                            AddResponse(context, lead, leadContent, campaign, buyerChannel, RequestResult.ResultTypes.Reject,
                            dnpMessage, errorType: RequestResult.ErrorTypes.None);

                            if (rt == RequestResult.ResultTypes.Success)
                                result.Set(rt, RequestResult.ErrorTypes.None, message);

                            RemoveBuyerChannel();
                            continue;
                        }
                    }

                    buyerChannelTemplates = GetFields(context, buyerChannel);
                    if (buyerChannelTemplates == null)
                    {
                        RemoveBuyerChannel();
                        return false;
                    }

                    UpdateBuyer(context, campaign, buyer, lead);

                    lock (timerLock)
                    {
                        PastSeconds = 0;
                    }


                    var res = SendData(context, lead, buyerChannel, campaign, buyerChannelTemplates, ls, leadContent, buyer,
                        schedulePrice <= 0.00001m ? AffiliatePrice : schedulePrice, campaignXML);
                    if (result.ResultType == RequestResult.ResultTypes.Success)
                    {
                        break;
                    }
                    //throw new Exception("test exception");
                }
                catch (Exception pingTreeException)
                {
                    var errorMessage = "Error (Invalid Data): " + pingTreeException.Message;
                    result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.InvalidData,
                           errorMessage);

                    InsertPostedData(context, lead, leadContent, buyerChannel, "", RequestResult.ResultTypes.Reject);
                    InsertLeadMainResponse(context, buyerChannel, lead, leadContent, Helpers.UtcNow(), 0,
                        errorMessage, errorMessage, 0, 0, RequestResult.ResultTypes.Error,
                        RequestResult.ErrorTypes.InvalidData, null);


                    pingTreeChannels.Remove(pingTreeChannel);
                }
            }//end while


            if (!buyerChannelProcessed)
                context.Extra["AffiliateResponseMessage"] = "No buyer channel attached";

            ts = Helpers.UtcNow() - context.StartDateUtc;

            ProcessingTime = (double)lead.ProcessingTime;

            //timerTokenSource.Cancel();

            return true;
        }

        public bool ValidateDoNotPresent(LeadContent content, RequestContext context, Buyer buyer, out string message)
        {
            bool doNotPresentPassed = true;
            message = "Do not present check did not pass";
            try
            {
                if (buyer.DoNotPresentStatus.HasValue && buyer.DoNotPresentStatus.Value > 0)
                {
                    string dnpSsn = content.Ssn;
                    string dnpEmail = content.Email;

                    if (context.HashedFieldValues.ContainsKey("SSN"))
                    {
                        dnpSsn = context.HashedFieldValues["SSN"];
                    }
                    else
                    if (context.HashedFieldValues.ContainsKey("ssn"))
                    {
                        dnpSsn = context.HashedFieldValues["ssn"];
                    }

                    if (String.IsNullOrEmpty(dnpSsn))
                    {
                        var key = "SSN";
                        if (!context.AllFieldValues.ContainsKey(key))
                            key = "ssn";

                        if (context.AllFieldValues.ContainsKey(key))
                            dnpSsn = context.AllFieldValues[key];
                    }

                    if (String.IsNullOrEmpty(dnpEmail))
                    {
                        var key = "EMAIL";
                        if (!context.AllFieldValues.ContainsKey(key))
                            key = "email";
                        dnpEmail = context.AllFieldValues[key];
                    }



                    if (String.IsNullOrEmpty(dnpSsn))
                    {
                        message = "Do Not Present checking failure: SSN not present";
                        return false;
                    }

                    if (dnpSsn.Length > 4)
                    {
                        dnpSsn = dnpSsn.Substring(dnpSsn.Length - 4);
                    }
                    else
                    {
                        message = "Do Not Present checking failure: invalid SSN";
                        return false;
                    }

                    if (!String.IsNullOrEmpty(dnpEmail))
                    {
                        var index = dnpEmail.IndexOf("@");
                        if (index > 0)
                        {
                            dnpEmail = dnpEmail.Substring(0, index);
                        }
                        else
                        {
                            message = "Do Not Present checking failure: invalid EMAIL";
                            return false;
                        }
                    }
                    else
                    {
                        message = "Do Not Present checking failure: EMAIL not present";
                        return false;
                    }

                    if (buyer.DoNotPresentStatus.Value == 1)
                    {
                        if (context.DoNotPresentService.CheckDoNotPresent(dnpEmail, dnpSsn, buyer.Id) == 1 && result.ResultType != RequestResult.ResultTypes.Success)
                        {
                            message = "Lead blocked by DoNotPresent local DB check | SSN:" + dnpSsn + ", EMAIL:" + dnpEmail;
                            doNotPresentPassed = false;
                        }
                    }
                    else if (buyer.DoNotPresentStatus.Value == 2)
                    {
                        string doNotPresentRequest = buyer.DoNotPresentRequest;
                        if (string.IsNullOrEmpty(doNotPresentRequest))
                            doNotPresentRequest = "ssn={ssn}" + dnpSsn + "&email={email}" + dnpEmail + "&buyerid=" + buyer.Id.ToString();

                        doNotPresentRequest = doNotPresentRequest.Replace("{ssn}", dnpSsn).Replace("{email}", dnpEmail);
                        doNotPresentRequest = doNotPresentRequest.Trim();

                        string dnpResultStr = Helpers.Post(buyer.DoNotPresentUrl.Trim(), doNotPresentRequest, method: (!string.IsNullOrEmpty(buyer.DoNotPresentPostMethod) ? buyer.DoNotPresentPostMethod : "POST"));
                        XmlDocument dnpDataXmlDoc;
                        dnpResultStr = Helpers.CorrectData(context, dnpResultStr, out dnpDataXmlDoc);
                        XmlNodeList dnpField = dnpDataXmlDoc.GetElementsByTagName(buyer.DoNotPresentResultField);
                        if (dnpField.Count > 0 && dnpField[0].InnerText.ToLower() == (!string.IsNullOrEmpty(buyer.DoNotPresentResultValue) ? buyer.DoNotPresentResultValue.ToLower() : ""))
                        {
                            message = "Lead blocked by buyer's DoNotPresent API check | SSN:" + dnpSsn + ", EMAIL:" + dnpEmail;
                            doNotPresentPassed = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                message = "DNP checking failure:" + e.Message;
                return false;
            }

            return doNotPresentPassed;
        }

        /// <summary>
        ///     Processes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="lead">The lead.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Process(RequestContext context, LeadMain lead)
        {
            try
            {
                ProcessData(context, lead);
            }
            finally
            {
                SaveDatabaseChanges(context);
            }
            return true;
        }

        #endregion Public methods
    }
}