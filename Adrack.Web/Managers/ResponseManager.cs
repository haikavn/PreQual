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

using Adrack.Core.Domain.Lead;
using System;

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
            if (format == null)
            {
                var returnString = "";
                //arman change

                switch (result.ErrorType)
                {
                    case RequestResult.ErrorTypes.Dropped:
                        returnString =
                            "<?xml version='1.0' encoding='utf-8' ?><response><id></id><status>Error</status><message>Dropped</message><price></price><redirect></redirect></response>";
                        break;

                    case RequestResult.ErrorTypes.InvalidData:
                        returnString =
                            "<?xml version='1.0' encoding='utf-8'?><response><id></id><status>Error</status><message>Invalid data</message><price></price><redirect></redirect></response>";
                        break;

                    default:
                        returnString =
                            "<?xml version='1.0' encoding='utf-8'?><response><id></id><status>Error</status><message>Affiliate channel not found</message><price></price><redirect></redirect></response>";
                        break;
                }

                response = returnString;
                return;
            }

            //var xmldoc = new XmlDocument();

            var buildXmlString =
                "<?xml version='1.0' encoding='utf-8'?><response><id>{0}</id><status>{1}</status><message>{2}</message><price>{3}</price><redirect>{4}</redirect>{5}</response>";

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

                ru.CampaignId = (context.Extra["lead"] as LeadMain).CampaignId;
                ru.AffiliateId = (context.Extra["lead"] as LeadMain).AffiliateId;
                ru.AffiliateChannelId = (context.Extra["lead"] as LeadMain).AffiliateChannelId;

                long? buyerChannelId = (context.Extra["lead"] as LeadMain).BuyerChannelId;

                ru.BuyerChannelId = buyerChannelId;

                if (buyerChannelId.HasValue)
                {
                    var buyerChannel = context.BuyerChannelService.GetBuyerChannelById(buyerChannelId.Value, true);
                    if (buyerChannel != null)
                        ru.BuyerId = buyerChannel.BuyerId;
                }

                ru.State = (context.Extra["leadContent"] as LeadContent).State;

                context.RedirectUrlService.InsertRedirectUrl(ru);
            }

            response = string.Format(buildXmlString.Replace("\\", ""), id, status, message, price, redirect, addXml);
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

            if (format == null)
            {
                switch (result.ErrorType)
                {
                    case RequestResult.ErrorTypes.Dropped:
                        response =
                            "<?xml version='1.0' encoding='utf-8' ?><response><id></id><status>Error</status><message>Dropped</message><price></price><redirect></redirect></response>";
                        break;

                    case RequestResult.ErrorTypes.InvalidData:
                        response =
                            "<?xml version='1.0' encoding='utf-8'?><response><id></id><status>Error</status><message>Invalid data</message><price></price><redirect></redirect></response>";
                        break;

                    case RequestResult.ErrorTypes.SystemOnHold:
                        response =
                            "<?xml version='1.0' encoding='utf-8'?><response><id></id><status>Reject</status><message></message><price></price><redirect></redirect></response>";
                        break;

                    default:
                        response =
                            "<?xml version='1.0' encoding='utf-8'?><response><id></id><status>Error</status><message>Affiliate channel not found</message><price></price><redirect></redirect></response>";
                        break;
                }

                return;
            }

            PrepareXMLResponse(context, format, result, isexport);
        }

        #endregion Public methods
    }
}