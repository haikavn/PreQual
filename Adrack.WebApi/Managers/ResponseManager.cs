// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ResponseManager.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Click;
using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Helpers;
using System;
using System.Threading.Tasks;

namespace Adrack.Managers
{
    /// <summary>
    ///     Class ResponseManager.
    /// </summary>
    public class ResponseManager
    {
        #region Private properties

        /// <summary>
        ///     The response
        /// </summary>
        private string response = "";

        #endregion Private properties

        #region Public properties

        /// <summary>
        ///     Gets or sets the response.
        /// </summary>
        /// <value>The response.</value>
        public string Response
        {
            get => response;
            set => response = value;
        }

        #endregion Public properties

        #region Private methods

        /// <summary>
        ///     Prepares the XML response.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="format">The format.</param>
        /// <param name="result">The result.</param>
        /// <param name="isexport">if set to <c>true</c> [isexport].</param>
        public void PrepareXMLResponse(RequestContext context, AffiliateChannel format, RequestResult result,
            bool isexport)
        {
            string declaration = "<?xml version='1.0' encoding='utf-8'?>".Replace("\\", "");


            if (format == null)
            {
                var returnString = "";
                //arman change

                switch (result.ErrorType)
                {
                    case RequestResult.ErrorTypes.Dropped:
                        returnString =
                            $"{declaration}<response><id></id><status>Error</status><message>Dropped</message><price></price><redirect></redirect></response>";
                        break;

                    case RequestResult.ErrorTypes.InvalidData:
                        returnString =
                            $"{declaration}<response><id></id><status>Error</status><message>Invalid data</message><price></price><redirect></redirect></response>";
                        break;

                    default:
                        returnString =
                            $"{declaration}<response><id></id><status>Error</status><message>Affiliate channel not found</message><price></price><redirect></redirect></response>";
                        break;
                }

                response = returnString;
                return;
            }

            //var xmldoc = new XmlDocument();

            var buildXmlString = "";

            long bChannelId;
            long.TryParse(context.Extra["BuyerChannelId"].ToString(), out bChannelId);

            if (!context.Extra.ContainsKey("BuyerChannelRequestData") ||
                string.IsNullOrEmpty(context.Extra["BuyerChannelRequestData"].ToString()) || bChannelId == 0)
                buildXmlString = "<response><id>{0}</id><status>{1}</status><message>{2}</message><price>{3}</price><redirect>{4}</redirect>{5}</response>";
            else
                buildXmlString = "<response><id>{0}</id><status>{1}</status><message>{2}</message><price>{3}</price><redirect>{4}</redirect><postedtobuyer>{5}</postedtobuyer>{6}</response>";

            //var nlist = xmldoc.GetElementsByTagName("status");
            var status = "";

            switch (result.ResultType)
            {
                case RequestResult.ResultTypes.Success:
                    status = "sold";
                    break;

                case RequestResult.ResultTypes.Reject:
                    status = "reject";
                    break;

                case RequestResult.ResultTypes.Error:
                    status = "reject";
                    if (string.IsNullOrEmpty(result.Message))
                        result.Message = "Lead was not sold in the marketplace";
                    break;

                case RequestResult.ResultTypes.Test:
                    status = "test";
                    break;

                default:
                    status = result.ResultType.ToString();
                    break;
            }

            var id = (context.Extra["lead"] as LeadMain).Id.ToString();

            var message = result.Message;


            var price = result.Price;

            var redirect = "";
            if (context.AllowAffiliateRedirect)
                if (result.InternalUrl.Length > 0)
                    redirect = "![CDATA[" + result.InternalUrl + "]]";

            var addXml = "";
            if (result.AdditionalXml != null)
                addXml = result.AdditionalXml.OuterXml;

            if ((result.ResultType == RequestResult.ResultTypes.Success ||
                 result.ResultType == RequestResult.ResultTypes.Test) && context.AllowAffiliateRedirect)
            {
                if (result.ResultType == RequestResult.ResultTypes.Test)
                    context.Extra["navkey"] = Helpers.GenerateString(10);

                var ru = new RedirectUrl
                {
                    Clicked = false,
                    Created = DateTime.UtcNow
                };
                ru.ClickDate = ru.Created;
                ru.LeadId = (context.Extra["lead"] as LeadMain).Id;
                ru.Url = result.RedirectNoCData;
                if (context.Extra.ContainsKey("navkey"))
                    ru.NavigationKey = context.Extra["navkey"].ToString().ToLower();
                else
                    ru.NavigationKey = "";

                ru.Device = "";
                ru.Ip = "";
                ru.Title = result.Title;
                ru.Description = result.Description;
                ru.Address = result.Address;
                ru.ZipCode = result.ZipCode;
                context.RedirectUrlService.InsertRedirectUrl(ru);
            }


            if (!context.Extra.ContainsKey("BuyerChannelRequestData") ||
                string.IsNullOrEmpty(context.Extra["BuyerChannelRequestData"].ToString()) || bChannelId == 0)
                response = declaration + string.Format(buildXmlString, id, status, message, price, redirect, addXml);
            else
                response = declaration + string.Format(buildXmlString, id, status, message, price, redirect, context.Extra["BuyerChannelRequestData"].ToString(), addXml);
        }

        #endregion Private methods

        #region Public methods

        /// <summary>
        ///     Prepares the response.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="format">The format.</param>
        /// <param name="result">The result.</param>
        /// <param name="isexport">if set to <c>true</c> [isexport].</param>
        public void PrepareResponse(RequestContext context, AffiliateChannel format, RequestResult result,
            bool isexport)
        {
            if (format != null && format.Status == 2 ||
                context.Extra.ContainsKey("mode") && context.Extra["mode"].ToString() == "test")
                result.ResultType = RequestResult.ResultTypes.Test;

            string declaration = "<?xml version='1.0' encoding='utf-8'?>".Replace("\\", "");

            if (format == null)
            {
                switch (result.ErrorType)
                {
                    case RequestResult.ErrorTypes.Dropped:
                        response =
                            $"{declaration}<response><id></id><status>Error</status><message>Dropped</message><price></price><redirect></redirect></response>";
                        break;

                    case RequestResult.ErrorTypes.InvalidData:
                        response =
                            $"{declaration}<response><id></id><status>Error</status><message>Invalid data</message><price></price><redirect></redirect></response>";
                        break;

                    case RequestResult.ErrorTypes.SystemOnHold:
                        response =
                            $"{declaration}<response><id></id><status>Reject</status><message>Affiliate channel not found</message><price></price><redirect></redirect></response>";
                        break;

                    default:
                        response =
                            $"{declaration}<response><id></id><status>Error</status><message>Affiliate channel not found</message><price></price><redirect></redirect></response>";
                        break;
                }

                return;
            }

            PrepareXMLResponse(context, format, result, isexport);

            if (result.ResultType == RequestResult.ResultTypes.Success && 
                context.HttpRequest != null &&
                context.HttpRequest.Properties != null &&
                context.HttpRequest.Properties.ContainsKey("clickid"))
            {
                string key = context.HttpRequest.Properties["clickid"]?.ToString();

                if (!string.IsNullOrEmpty(key))
                {
                    try
                    {
                        ClickChannel clickChannel = context.ClickService.GetClickChannelByAccessKey(key);
                        if (clickChannel != null && clickChannel.AffiliateChannelId == format.Id)
                        {
                            var postBackUrls = context.ClickService.GetClickPostBackUrls(clickChannel.Id);
                            foreach (var postBackUrl in postBackUrls)
                            {
                                if (!string.IsNullOrEmpty(postBackUrl.PostingUrl) && !string.IsNullOrEmpty(postBackUrl.PostingParams))
                                {
                                    string[] postingParams = postBackUrl.PostingParams.Split(new char[1] { ',' });
                                    string postingUrl = postBackUrl.PostingUrl;

                                    foreach (string postinParam in postingParams)
                                    {
                                        postingUrl = postingUrl.Replace("{" + postinParam + "}", context.HttpRequest.Properties[postinParam]?.ToString());
                                    }

                                    postingUrl = postingUrl.Replace("{price}", result.Price);

                                    Task.Run(() =>
                                    {
                                        Helpers.Post(postingUrl, "", method: "GET");
                                    });

                                    context.ClickService.InsertClickPostbackUrlLog(new ClickPostbackUrlLog()
                                    {
                                        Created = DateTime.UtcNow,
                                        LeadId = (context.Extra["lead"] as LeadMain).Id,
                                        PostedUrl = postingUrl
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        #endregion Public methods
    }
}