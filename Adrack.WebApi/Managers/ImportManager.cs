// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ImportManager.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure;
using Adrack.Service.Helpers;
using Adrack.Web.Managers;
using Adrack.WebApi.Helpers;
using Arack.Encryption;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using static Adrack.Managers.RequestManager;

namespace Adrack.Managers
{
    /// <summary>
    ///     Class ImportManager.
    /// </summary>
    public class ImportManager
    {
        public XmlHelper XmlHelper = null;

        #region Private properties

        /// <summary>
        ///     The result
        /// </summary>
        private readonly RequestResult result = new RequestResult();

        /// <summary>
        ///     The processing time
        /// </summary>
        private double processingTime;

        /// <summary>
        ///     The custom data
        /// </summary>
        private string _customData = "";

        private Dictionary<long, List<FilterData>> ChildFilterData { get; set; } = new Dictionary<long, List<FilterData>>();
        private Dictionary<long, bool> ParentFilterResult { get; set; } = new Dictionary<long, bool>();

        #endregion Private properties

        #region Public properties

        /// <summary>
        ///     Gets or sets the custom data.
        /// </summary>
        /// <value>The custom data.</value>
        public string CustomData
        {
            get => _customData;
            set => _customData = value;
        }

        /// <summary>
        ///     Gets the result.
        /// </summary>
        /// <value>The result.</value>
        public RequestResult Result => result;

        /// <summary>
        ///     Gets or sets the processing time.
        /// </summary>
        /// <value>The processing time.</value>
        public double ProcessingTime
        {
            get => processingTime;
            set => processingTime = value;
        }

        #endregion Public properties

        #region Private methods

        /// <summary>
        ///     Gets the fields.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="affiliateChannelId">The affiliate channel identifier.</param>
        /// <returns>AffiliateChannelTemplate[].</returns>
        private AffiliateChannelTemplate[] GetFields(RequestContext context, long affiliateChannelId)
        {
            return context.AffiliateChannelTemplateService
                .GetAllAffiliateChannelTemplatesByAffiliateChannelId(affiliateChannelId).ToArray();
        }

        /// <summary>
        ///     Gets the input data.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        private string GetInputData(RequestContext context)
        {
            return context.PostedDataBody;
            /*var bytes = new byte[context.HttpRequest.InputStream.Length];
            context.HttpRequest.InputStream.Read(bytes, 0, (int)context.HttpRequest.InputStream.Length);
            return HttpUtility.UrlDecode(Encoding.UTF8.GetString(bytes));*/
        }

        /// <summary>
        ///     Validates the value.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="tplfield">The tplfield.</param>
        /// <param name="val">The value.</param>
        /// <param name="result">The result.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ValidateValue(RequestContext context, CampaignField tplfield, AffiliateChannelTemplate affiliateChannelField, string val, out string result)
        {
            result = "";

            if (tplfield.ValidatorSettings == null)
                tplfield.ValidatorSettings = "";

            if (tplfield.Validator == (short)Validators.None) return true;

            if (!tplfield.Required && val.Length == 0)
            {
                result = tplfield.TemplateField + " is required";
                return true;
            }

            if (tplfield.MinLength > 0 && val.Length < tplfield.MinLength &&
                tplfield.Validator == (short)Validators.String)
            {
                result = tplfield.TemplateField + " is too short";
                return false;
            }

            if (tplfield.MaxLength > 0 && val.Length > tplfield.MaxLength &&
                tplfield.Validator == (short)Validators.String)
            {
                result = tplfield.TemplateField + " is too long";
                return false;
            }

            var dob = DateTime.Now;

            switch (tplfield.Validator)
            {
                case (short)Validators.None: return true;
                case (short)Validators.Number:
                    if (Validator.IsValidNumber(val))
                    {
                        return true;
                    }
                    else
                    {
                        result = tplfield.TemplateField + " is not a valid number";
                        return false;
                    }

                case (short)Validators.Email:
                    if (Validator.IsValidEmailAddress2(val))
                    {
                        return true;
                    }
                    else
                    {
                        result = tplfield.TemplateField + " is not a email";
                        return false;
                    }
                case (short)Validators.AccountNumber: return true;
                case (short)Validators.Ssn:
                    if (Validator.IsValidSSN(val))
                    {
                        return true;
                    }
                    else
                    {
                        result = tplfield.TemplateField + " is not a valid SSN number";
                        return false;
                    }
                case (short)Validators.Zip:
                    if (Validator.IsValidZIPCode(val, tplfield.ValidatorSettings))
                    {
                        return true;
                    }
                    else
                    {
                        result = tplfield.TemplateField + " is not a valid ZIP";
                        return false;
                    }
                case (short)Validators.Phone:
                    if (Validator.IsValidUSPhoneNumber(val))
                    {
                        return true;
                    }
                    else
                    {
                        result = tplfield.TemplateField + " is not a valid Phone number";
                        return false;
                    }
                case (short)Validators.DateTime:
                    if (Validator.IsValidDateTime(val, (!string.IsNullOrEmpty(affiliateChannelField.DataFormat) ? affiliateChannelField.DataFormat : tplfield.ValidatorSettings), out dob))
                    {
                        return true;
                    }
                    else
                    {
                        result = tplfield.TemplateField + " is not a valid DateTime";
                        return false;
                    }
                case (short)Validators.DateOfBirth:
                    var res = Validator.IsValidDateTime(val, (!string.IsNullOrEmpty(affiliateChannelField.DataFormat) ? affiliateChannelField.DataFormat : tplfield.ValidatorSettings), out dob);
                    if (res)
                    {
                        var tsage = DateTime.Now - dob;
                        context.Extra["Age"] = (int)(tsage.TotalDays / 365);
                        return true;
                    }
                    else
                    {
                        result = tplfield.TemplateField + " is not a valid Date of Birth";
                        return false;
                    }

                case (short)Validators.State:
                    if (Validator.IsValidUSState(val))
                    {
                        return true;
                    }
                    else
                    {
                        result = tplfield.TemplateField + " is not a valid US state";
                        return false;
                    }
                case (short)Validators.RoutingNumber:
                    if (Validator.IsValidRoutingNumber(val))
                    {
                        return true;
                    }
                    else
                    {
                        result = tplfield.TemplateField + " is not a valid routing number";
                        return false;
                    }
                case (short)Validators.Decimal:
                    {
                        var str = tplfield.ValidatorSettings.Split(';');

                        var before = 10;
                        var after = 2;
                        if (str.Length > 0)
                            if (!int.TryParse(str[0], out before))
                                before = 2;

                        if (str.Length > 1)
                            if (!int.TryParse(str[1], out after))
                                after = 10;

                        if (Validator.IsValidDecimal(val, before, after)) return true;

                        result = tplfield.TemplateField + " is not a valid decimal number";
                        return false;
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

        protected bool ValidateFilterCondition(RequestContext context, CampaignField tplfield, AffiliateChannelFilterCondition f, string val)
        {
            try
            {
                bool filterResult = false;

                var ageFound = false;
                var dateFound = false;
                DateTime fieldDate = DateTime.Now;
                DateTime checkFieldDate = DateTime.Now;
                Decimal checkValueNumber = 0;
                
                val = val.ToLower();
                string valOriginal = val;
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
                    val = context.Extra["Age"].ToString();
                }
               
                if ((ageFound || tplfield.Validator == (short)Validators.DateTime) && isNumericFilter)
                {
                    dateFound = true;
                    if (tplfield.ValidatorSettings == "")
                        tplfield.ValidatorSettings = "MM/dd/yyyy";

                    if (!DateTime.TryParseExact(valOriginal.ToString(), tplfield.ValidatorSettings,
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out fieldDate))
                        return false;
                    var diff = fieldDate - DateTime.MinValue;
                    val = diff.TotalDays.ToString();
                }

                var values = f.Value.Split(',');
                
                foreach (var s in values)
                {
                    var checkValue = s.Trim().ToLower();

                    /*if (ageFound && isNumericFilter && !Decimal.TryParse(checkValue, out checkValueNumber))
                    {
                        DateTime parsed = DateTime.Now;
                        if (!DateTime.TryParseExact(checkValue, tplfield.ValidatorValue,
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
                            return false;
                        checkValue = Years(parsed, DateTime.Now).ToString();
                    }*/

                    if (dateFound && isNumericFilter)
                    {
                        if (!DateTime.TryParseExact(checkValue, tplfield.ValidatorSettings,
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out checkFieldDate))
                            return false;
                        var diff = checkFieldDate - DateTime.MinValue;
                        checkValue = diff.TotalDays.ToString();
                    }

                    switch ((Conditions)f.Condition)
                    {
                        case Conditions.Contains:
                            if (valOriginal.Contains(checkValue)) filterResult = true;
                           
                            break;
                            //AA,BB,CC,LA,TT
                        case Conditions.NotContains:
                            if (!valOriginal.Contains(checkValue)) filterResult = true;
                            else return false;
                            break;

                        case Conditions.StartsWith:
                            if (valOriginal.StartsWith(checkValue)) filterResult = true;
                           
                            break;

                        case Conditions.EndsWith:
                            if (valOriginal.EndsWith(checkValue)) filterResult = true;
                           
                            break;

                        case Conditions.Equals:
                            if (checkValue == valOriginal) filterResult = true;
                          
                            break;

                        case Conditions.NotEquals:
                            if (checkValue != valOriginal) filterResult = true;
                            else return false;
                            break;

                        case Conditions.NumberGreater:
                            if (tplfield.Validator == (short)Validators.Number ||
                                tplfield.Validator == (short)Validators.Decimal
                                || tplfield.Validator == (short)Validators.RoutingNumber
                                || tplfield.Validator == (short)Validators.DateOfBirth
                                || tplfield.Validator == (short)Validators.DateTime)
                            {
                                if (decimal.Parse(val) > decimal.Parse(checkValue)) filterResult = true;
                                else return false;
                            }
                            else
                            {
                                var res = string.Compare(val, checkValue);
                                if (res > 0) filterResult = true;
                                else return false;
                            }

                            break;

                        case Conditions.NumberGreaterOrEqual:
                            if (tplfield.Validator == (short)Validators.Number ||
                                tplfield.Validator == (short)Validators.Decimal
                                || tplfield.Validator == (short)Validators.RoutingNumber
                                || tplfield.Validator == (short)Validators.DateOfBirth
                                || tplfield.Validator == (short)Validators.DateTime)
                            {
                                if (decimal.Parse(val) >= decimal.Parse(checkValue)) filterResult = true;
                                else return false;
                            }
                            else
                            {
                                var res = string.Compare(valOriginal, checkValue);
                                if (res >= 0) filterResult = true;
                                else return false;
                            }

                            break;

                        case Conditions.NumberLess:
                            if (tplfield.Validator == (short)Validators.Number ||
                                tplfield.Validator == (short)Validators.Decimal
                                || tplfield.Validator == (short)Validators.RoutingNumber
                                || tplfield.Validator == (short)Validators.DateOfBirth
                                || tplfield.Validator == (short)Validators.DateTime)
                            {
                                if (decimal.Parse(val) < decimal.Parse(checkValue)) filterResult = true;
                                else return false;
                            }
                            else
                            {
                                var res = string.Compare(valOriginal, checkValue);
                                if (res < 0) filterResult = true;
                                else return false;
                            }

                            break;

                        case Conditions.NumberLessOrEqual:
                            if (tplfield.Validator == (short)Validators.Number ||
                                tplfield.Validator == (short)Validators.Decimal
                                ||
                                tplfield.Validator == (short)Validators.RoutingNumber
                                || tplfield.Validator == (short)Validators.DateOfBirth
                                || tplfield.Validator == (short)Validators.DateTime)
                            {
                                if (decimal.Parse(val) <= decimal.Parse(checkValue)) filterResult = true;
                                else return false;
                            }
                            else
                            {
                                var res = string.Compare(valOriginal, checkValue);
                                if (res <= 0) filterResult = true;
                                else return false;
                            }

                            break;

                        case Conditions.NumberRange:
                            if (tplfield.Validator == (short)Validators.AccountNumber)
                                if (val.Length > 4)
                                    val = val.Substring(0, 4);

                            var vals = checkValue.Split('-');

                            var d = decimal.Parse(val);
                            if (decimal.Parse(vals[0].Trim()) <= d && decimal.Parse(vals[1].Trim()) >= d) filterResult = true;
                            else return false;

                            var l = long.Parse(val.ToString());
                            if (long.Parse(vals[0].Trim()) <= l && long.Parse(vals[1].Trim()) >= l) filterResult = true;
                            else return false;

                            break;

                        case Conditions.StringByLength:
                            {
                                int len;
                                int.TryParse(checkValue, out len);
                                if (!Utils.HasConsecutiveChars(valOriginal, len)) filterResult = true;
                                else return false;
                            }
                            break;
                    }
                }

                return filterResult;
            }
            catch (Exception e)
            {
                return false;
            }
        }

            /// <summary>
            ///     Validates the filter.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <param name="tplfield">The tplfield.</param>
            /// <param name="field">The field.</param>
            /// <param name="val">The value.</param>
            /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
            private bool ValidateFilter(RequestContext context, CampaignField tplfield, AffiliateChannelTemplate field,
            object val)
            {
                if (!string.IsNullOrEmpty(field.DefaultValue) && val.ToString() != field.DefaultValue) return false;
                if (tplfield.IsFilterable.HasValue && !tplfield.IsFilterable.Value) return true;

                bool filterResult = true;

                var filters =
                    (List<AffiliateChannelFilterCondition>)context.AffiliateChannelFilterConditionService
                        .GetFilterConditionsByAffiliateChannelIdAndCampaignTemplateId(field.AffiliateChannelId,
                            field.CampaignTemplateId, -1);

                if (filters.Count == 0) return true;              

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

                    filterResult = ValidateFilterCondition(context,tplfield, f, val.ToString());

                    ParentFilterResult[f.Id] = filterResult;

                    if (!filterResult)
                    {
                        if (context.AffiliateChannelFilterConditionService.HasChildren(f.Id)) continue; // Checks for child filters
                        return filterResult;
                    }
                }

                return true;
            }

        public bool ValidateChildFilters(RequestContext context)
        {
            foreach(long parentId in ChildFilterData.Keys)
            {
                if (!ParentFilterResult[parentId]) continue;

                var filterDataList = ChildFilterData[parentId];

                foreach(FilterData filterData in filterDataList)
                {
                    if (!ValidateFilterCondition(context, filterData.CampaignField, (AffiliateChannelFilterCondition)filterData.Filter, filterData.Value.ToString()))
                    {
                        result.Validator = filterData.CampaignField.Validator;
                        result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.FilterError,
                            ((AffiliateChannelTemplate)filterData.ChannelField).TemplateField + " '" + filterData.Value.ToString() + "' does not match the criteria");

                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        ///     The duration
        /// </summary>
        public double duration;

        /// <summary>
        ///     Gets the XML dictionary.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="fields">The fields.</param>
        private void GetXMLDictionary(XmlNode parent, Dictionary<string, XmlNode> fields)
        {
            foreach (XmlNode node in parent.ChildNodes)
                if (node.NodeType == XmlNodeType.Element)
                {
                    fields[node.Name] = node;
                    GetXMLDictionary(node, fields);
                }
        }

        /// <summary>
        ///     Processes the XML fields.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="lead">The lead.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ProcessXMLFields(RequestContext context, XmlNode parent, LeadMain lead)
        {
            foreach (XmlNode node in parent.ChildNodes)
                if (node.NodeType == XmlNodeType.Element)
                    if (node.ParentNode != null)
                    {
                        var tplfield =
                            context.CampaignTemplateService.GetCampaignTemplateBySectionAndName(node.ParentNode.Name,
                                node.Name,lead.CampaignId);

                        if (tplfield == null)
                        {
                            result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.MissingField,
                                "Template field is missing");
                            return false;
                        }

                        object value = node.InnerText;

                        var pi = lead.GetType().GetProperty(tplfield.DatabaseField, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                        if (pi != null)
                            try
                            {
                                var t = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;

                                if (t == typeof(decimal))
                                    value = decimal.Parse(value.ToString(), CultureInfo.CurrentCulture.NumberFormat);
                                else if (t == typeof(short))
                                    value = short.Parse(value.ToString(), CultureInfo.CurrentCulture.NumberFormat);
                                else if (t == typeof(int))
                                    value = int.Parse(value.ToString(), CultureInfo.CurrentCulture.NumberFormat);
                                else if (t == typeof(DateTime))
                                    value = DateTime.Parse(value.ToString(), CultureInfo.CurrentCulture.DateTimeFormat);

                                if (context.BlackListService.CheckBlackListValue(tplfield.DatabaseField.ToLower(),
                                        value.ToString()))
                                {
                                    result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.None,
                                        "The value '" + value + "' is in black list");
                                    return false;
                                }

                                pi.SetValue(lead, value);
                            }
                            catch
                            {
                                result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.None,
                                    "Can not set value of '" + tplfield.DatabaseField + "'");
                                return false;
                            }

                        if (!ProcessXMLFields(context, node, lead)) return false;
                    }

            return true;
        }

        /// <summary>
        ///     Fills the XML nodes.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="list">The list.</param>
        private void FillXMLNodes(XmlNode parent, Dictionary<string, XmlNode> list)
        {
            foreach (XmlNode node in parent.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element) continue;
                list[node.Name + (node.ParentNode == null ? "" : node.ParentNode.Name)] = node;
                FillXMLNodes(node, list);
            }
        }

        /// <summary>
        ///     Fills the sensitive data.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        private void FillSensitiveData(RequestContext context, string field, string value)
        {
            if (context.Extra.ContainsKey("sensitive"))
            {
                var lsd = (LeadSensitiveData)context.Extra["sensitive"];

                if (lsd != null)
                {
                    if (string.IsNullOrEmpty(lsd.Data1))
                        lsd.Data1 = field + ": " + value;
                    else if (string.IsNullOrEmpty(lsd.Data2))
                        lsd.Data2 = field + ": " + value;
                    else if (string.IsNullOrEmpty(lsd.Data3))
                        lsd.Data3 = field + ": " + value;
                    else if (string.IsNullOrEmpty(lsd.Data4))
                        lsd.Data4 = field + ": " + value;
                    else if (string.IsNullOrEmpty(lsd.Data5))
                        lsd.Data5 = field + ": " + value;
                    else if (string.IsNullOrEmpty(lsd.Data6))
                        lsd.Data6 = field + ": " + value;
                    else if (string.IsNullOrEmpty(lsd.Data7))
                        lsd.Data7 = field + ": " + value;
                    else if (string.IsNullOrEmpty(lsd.Data8))
                        lsd.Data8 = field + ": " + value;
                    else if (string.IsNullOrEmpty(lsd.Data9))
                        lsd.Data9 = field + ": " + value;
                    else if (string.IsNullOrEmpty(lsd.Data10))
                        lsd.Data10 = field + ": " + value;
                }
            }
        }

        /// <summary>
        ///     Validates the white ip list.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="leadContent">Content of the lead.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected bool? ValidateWhiteIpList(RequestContext context, LeadContent leadContent)
        {
            if (!context.MinProcessingMode && context.Affiliate != null && !string.IsNullOrEmpty(leadContent.Ip) &&
                (!string.IsNullOrEmpty(context.Affiliate.WhiteIp) || !string.IsNullOrEmpty(context.WhiteIp)))
            {
                var whiteIp = context.Affiliate.WhiteIp.Replace("\r\n", "\n");

                if (string.IsNullOrEmpty(whiteIp))
                    whiteIp = context.WhiteIp.Replace("\r\n", "\n");

                var ips = whiteIp.Split('\n');
                if (ips.Length == 0) return true;

                foreach (var ip in ips)
                    if (leadContent.Ip == ip)
                        return false;
            }

            return null;
        }

        /// <summary>
        ///     Processes the XML data.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="data">The data.</param>
        /// <param name="campaign">The campaign.</param>
        /// <param name="format">The format.</param>
        /// <param name="lead">The lead.</param>
        /// <param name="leadContent">Content of the lead.</param>
        /// <returns>XmlDocument.</returns>
        private XmlDocument ProcessXMLData(RequestContext context, XmlDocument dataxml, Campaign campaign,
            AffiliateChannel format, LeadMain lead, LeadContent leadContent)
        {
            var now = DateTime.Now;
            var blackListItemsCount = context.BlackListService.GetCustomBlackListItemsCount(format.Id, 1);

            if (dataxml == null)
            {
                result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.InvalidData, "Invalid XML data");
                return null;
            }

            var affiliateChannelFields = GetFields(context, format.Id);

            var dataFound = false;
            XmlNode datael = null;

            string dbname;
            object value;
            PropertyInfo pi;

            XmlNode nodelist;

            foreach (var affiliateChannelField in affiliateChannelFields)
            {
                if (affiliateChannelField.CampaignTemplateId <= 0) continue;

                var tplfield = context.CampaignTemplateService.GetCampaignTemplateById(affiliateChannelField.CampaignTemplateId, true);

                if (tplfield == null) continue;

                nodelist = XmlHelper.GetElementsByTagName(dataxml, affiliateChannelField.TemplateField);

                if (nodelist == null && tplfield.Required)
                {
                    result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.MissingField,
                        "XML field '" + affiliateChannelField.TemplateField + "' is missing");
                    return null;
                }

                if (nodelist == null && tplfield != null &&
                    (string.Compare(tplfield.DatabaseField, "none", true) != 0 && tplfield.Required))
                {
                    result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.MissingField,
                        "XML field '" + affiliateChannelField.TemplateField + "' is missing");
                    return null;
                }

                datael = null;

                //foreach (XmlNode node in nodelist)
                if (nodelist != null &&
                    (nodelist.ParentNode == null && nodelist.Name == affiliateChannelField.SectionName ||
                    nodelist.ParentNode != null && nodelist.ParentNode.Name == affiliateChannelField.SectionName))
                    datael = nodelist;

                if (datael == null) continue;

                if (tplfield.Validator == (short)Validators.Zip)
                    context.Extra["ZipCode"] = datael.InnerText;
                else
                    context.Extra["ZipCode"] = "";

                value = datael.InnerText;

                var validate = false;
                var filter = false;

                if (datael.Attributes["validate"] != null)
                    bool.TryParse(datael.Attributes["validate"].Value, out validate);

                if (datael.Attributes["filter"] != null)
                    bool.TryParse(datael.Attributes["filter"].Value, out filter);

                string validationError;
                if (!context.MinProcessingMode && !ValidateValue(context, tplfield, affiliateChannelField, 
                        string.IsNullOrEmpty(affiliateChannelField.DefaultValue) ? value.ToString() : affiliateChannelField.DefaultValue, out validationError))
                {
                    if (validate) throw new Exception(tplfield.TemplateField + " validation did not pass");

                    result.Validator = tplfield.Validator;
                    result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.InvalidData, validationError);
                    return null;
                }

                if (!context.MinProcessingMode && !ValidateFilter(context, tplfield, affiliateChannelField, value) ||
                    tplfield.Required && value.ToString().Length == 0)
                {
                    
                    if (filter) throw new Exception(tplfield.TemplateField + " filter did not pass");

                    result.Validator = tplfield.Validator;
                    result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.FilterError,
                        datael.Name + " '" + value.ToString() + "' does not match the criteria");
                    return null;
                }

                if (!context.MinProcessingMode && blackListItemsCount > 0)
                {
                    var bl = (List<CustomBlackListValue>)context.BlackListService.GetCustomBlackListValues(format.Id,
                        1, affiliateChannelField.CampaignTemplateId);

                    var blItem = bl.FirstOrDefault();

                    if (blItem != null && !string.IsNullOrEmpty(blItem.Value))
                    {
                        string[] valueStrings = blItem.Value.Split(new char[1] { ',' });

                        foreach (string s in valueStrings)
                        {
                            //if (bl.Select(x => x.Value == value.ToString()).FirstOrDefault())
                            if (s.Trim() == value.ToString())
                            {
                                result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.None,
                                    "Black list error. Validation failed for '" + datael.Name + '"');
                                return null;
                            }
                        }
                    }
                }

                dbname = tplfield.DatabaseField;

                if (string.IsNullOrEmpty(dbname)) continue;

                pi = leadContent.GetType().GetProperty(tplfield.DatabaseField, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                context.AllFieldValues[datael.Name] = value.ToString();
                

                if (pi != null)
                {
                    try
                    {
                        var t = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;

                        if (t == typeof(decimal))
                        {
                            if (value.ToString().Length == 0)
                            {
                                value = (decimal)0;
                            }
                            else
                            {
                                value = decimal.Parse(value.ToString(), CultureInfo.CurrentCulture.NumberFormat);
                            }
                        }
                        else if (t == typeof(short))
                        {
                            if (value.ToString().Length == 0)
                            {
                                value = (short)0;
                            }
                            else
                            {
                                value = short.Parse(value.ToString(), CultureInfo.CurrentCulture.NumberFormat);
                            }
                        }
                        else if (t == typeof(int))
                        {
                            if (value.ToString().Length == 0)
                            {
                                value = (int)0;
                            }
                            else
                            {
                                value = int.Parse(value.ToString(), CultureInfo.CurrentCulture.NumberFormat);
                            }
                        }
                        else if (t == typeof(DateTime))
                        {
                            string dataFormat = (!string.IsNullOrEmpty(affiliateChannelField.DataFormat) ? affiliateChannelField.DataFormat : tplfield.ValidatorSettings);
                            if (string.IsNullOrEmpty(dataFormat))
                                dataFormat = "MM/dd/yyyy";

                            if (tplfield.Validator == (short)Validators.DateOfBirth ||
                                tplfield.Validator == (short)Validators.DateTime)
                                value = DateTime.ParseExact(value.ToString(), dataFormat,
                                    CultureInfo.InvariantCulture);
                            else
                                value = DateTime.Parse(value.ToString(), CultureInfo.CurrentCulture.DateTimeFormat);
                        }
                        else if (tplfield.IsHash.HasValue && tplfield.IsHash.Value && value.ToString().Length > 0)
                        {
                            context.HashedFieldValues[datael.Name] = value.ToString();
                            FillSensitiveData(context, datael.Name, value.ToString());

                            if (tplfield.Validator == (short)Validators.AccountNumber ||
                                tplfield.Validator == (short)Validators.Ssn ||
                                tplfield.Validator == (short)Validators.RoutingNumber ||
                                string.Compare(tplfield.TemplateField, "ssn", true) == 0 ||
                                string.Compare(tplfield.TemplateField, "dlnumber", true) == 0 ||
                                string.Compare(tplfield.TemplateField, "accountnumber", true) == 0)

                                value = ADLEncryptionManager.GetStringSha256Hash(value.ToString());
                            else
                                value = ADLEncryptionManager.Encrypt(value.ToString());
                            datael.InnerText = value.ToString();
                        }

                        var now2 = DateTime.Now;

                        if (!context.MinProcessingMode &&
                            context.BlackListService.CheckBlackListValue(tplfield.DatabaseField.ToLower(),
                                value.ToString()))
                        {
                            result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.None,
                                "The '" + tplfield.TemplateField + "' field value is in black list");
                            return null;
                        }

                        var ts2 = DateTime.Now - now2;

                        pi.SetValue(leadContent, value);

                        dataFound = true;
                    }
                    catch (Exception e)
                    {
                        result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.NoData, "Field "+datael.Name+". Validation failed with message: "+e.Message);
                        return null;
                    }
                }
                else if (tplfield.IsHash.HasValue && tplfield.IsHash.Value && datael.InnerText.Length > 0)
                {
                    context.HashedFieldValues[datael.Name] = value.ToString();
                    FillSensitiveData(context, datael.Name, datael.InnerText);

                    if (tplfield.Validator == (short)Validators.AccountNumber ||
                        tplfield.Validator == (short)Validators.Ssn ||
                        tplfield.Validator == (short)Validators.RoutingNumber ||
                        string.Compare(tplfield.TemplateField, "ssn", true) == 0 ||
                        string.Compare(tplfield.TemplateField, "dlnumber", true) == 0 ||
                        string.Compare(tplfield.TemplateField, "accountnumber", true) == 0)
                        datael.InnerText = ADLEncryptionManager.GetStringSha256Hash(datael.InnerText);
                    else
                        datael.InnerText = ADLEncryptionManager.Encrypt(datael.InnerText);
                }

                context.Extra["ZipCode"] = leadContent.Zip;

                var ts = DateTime.Now - now;
                if (ts.TotalSeconds > duration) duration = ts.TotalSeconds;
            }

            if (!ValidateChildFilters(context))
            {
                return null;
            }

            /*
             //not sure if we need this option, comment from Arman
             var whiteIpResult = ValidateWhiteIpList(context, leadContent);

             if (whiteIpResult.HasValue && !whiteIpResult.Value)
             {
                 result.Set(RequestResult.ResultTypes.Reject, RequestResult.ErrorTypes.None, "Ip address not allowed");
                 return null;
             }*/

            if (!dataFound)
            {
                result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.NoData, "No data");
                return null;
            }

            return dataxml;
        }

        /// <summary>
        ///     Gets the XML fields.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="list">The list.</param>
        /// <returns>Dictionary&lt;System.String, XmlNode&gt;.</returns>
        protected Dictionary<string, XmlNode> GetXmlFields(XmlNode parent, Dictionary<string, XmlNode> list)
        {
            if (parent == parent.OwnerDocument.DocumentElement) list["root_" + parent.Name] = parent;

            foreach (XmlNode node in parent.ChildNodes)
                if (node.NodeType == XmlNodeType.Element)
                {
                    list[parent.Name + "_" + node.Name] = node;
                    list = GetXmlFields(node, list);
                }

            return list;
        }

        #endregion Private methods

        #region Public methods

        /// <summary>
        ///     Identifies the campaign and channel.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="xmldoc">The xmldoc.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="AffiliateXmlField">The affiliate XML field.</param>
        /// <param name="campaign">The campaign.</param>
        /// <param name="format">The format.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected bool IdentifyCampaignAndChannel(RequestContext context, XmlDocument xmldoc,
            out string AffiliateXmlField, out Campaign campaign, out AffiliateChannel format)
        {
            campaign = null;
            AffiliateXmlField = "CHANNELID";
            format = null;

            var st = context.SettingService.GetSetting("System.AffiliateXmlField");
            if (st != null) AffiliateXmlField = st.Value;

            string affiliateChannelId = "";
            XmlNode nodes = null;


            if (context.HttpRequest.Method == HttpMethod.Post)
            {
                nodes = XmlHelper.GetElementsByTagName(xmldoc, AffiliateXmlField);

                if (nodes == null)
                    nodes = XmlHelper.GetElementsByTagName(xmldoc, AffiliateXmlField.ToLower());

                if (nodes == null)
                {
                    result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.NotExistingDBRecord,
                        "Posted XML does not contain '" + AffiliateXmlField + "' field");
                    return false;
                }

                affiliateChannelId = nodes.InnerText;
            }
            else
            {
                affiliateChannelId = context.HttpRequest.Properties[AffiliateXmlField].ToString();
            }

            format = context.AffiliateChannelService.GetAffiliateChannelByKey(affiliateChannelId);

            if (format != null && format.CampaignId != null)
                campaign = context.CampaignService.GetCampaignById(format.CampaignId.Value);

            if (campaign != null)
            {
                if (context.HttpRequest.Method == HttpMethod.Get)
                {
                    xmldoc.LoadXml(campaign.DataTemplate);
                    Helpers.FillFromQuery(context, xmldoc);
                    context.Extra["ReceivedData"] = xmldoc.OuterXml;
                    context.Extra["ReceivedDataXmlDoc"] = xmldoc;
                }
                context.PrioritizedEnabled = (campaign.PrioritizedEnabled.HasValue ? campaign.PrioritizedEnabled.Value : true);

                var pingTrees = context.CampaignService.GetPingTrees(campaign.Id);
                if (!SharedData.CheckPingTreeLeadsCount())
                    SharedData.ResetPingTreeLeadsCount(pingTrees);
            }

            nodes = XmlHelper.GetElementsByTagName(xmldoc, "PingTreeTestMode");

            context.PingTreeTestMode = 0;
            if (nodes != null)
            {
                short pingTreeTestMode = 0;
                short.TryParse(nodes.InnerText, out pingTreeTestMode);
                context.PingTreeTestMode = pingTreeTestMode;
            }

            return true;
        }

        /// <summary>
        ///     Processes the data.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="isachannel">if set to <c>true</c> [isachannel].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ProcessData(RequestContext context, bool isachannel)
        {
            var lead = new LeadMain
            {
                Status = (short)RequestResult.ResultTypes.Processing,
                ErrorType = (short)RequestResult.ErrorTypes.None
            };

            var leadContent = new LeadContent
            {
                Minprice = 0,
                State = ""
            };

            Campaign campaign = null;
            leadContent.MinpriceStr = "";

            context.Extra["lead"] = lead;
            context.Extra["leadContent"] = leadContent;

            context.Extra["sensitive"] = new LeadSensitiveData();

            var data = GetInputData(context);
            if (data.Length == 0) data = CustomData;

            XmlDocument receviedDataXmlDoc = new XmlDocument();
            if (context.HttpRequest.Method == HttpMethod.Post)
            {
                data = Helpers.CorrectData(context, data, out receviedDataXmlDoc);

                if (string.IsNullOrEmpty(data))
                {
                    result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.InvalidData, "XML is invalid");
                    return false;
                }

                context.Extra["ReceivedData"] = data;
                context.Extra["ReceivedDataXmlDoc"] = receviedDataXmlDoc;
            }

            var xmldoc = receviedDataXmlDoc;
            string AffiliateXmlField;

            AffiliateChannel format;

            if (!IdentifyCampaignAndChannel(context, xmldoc, out AffiliateXmlField, out campaign, out format))
                return false;          

            var ReceivedDataXmlDocFields = new Dictionary<string, XmlNode>();
            context.Extra["ReceivedDataXmlDocFields"] =
                GetXmlFields(receviedDataXmlDoc.DocumentElement, ReceivedDataXmlDocFields);

            if (context.SystemOnHold)
            {
                result.Set(RequestResult.ResultTypes.Reject, RequestResult.ErrorTypes.SystemOnHold, "Rejected");
                return false;
            }

            if (campaign == null || campaign != null && campaign.IsDeleted.HasValue && campaign.IsDeleted.Value)
            {
                result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.NotExistingDBRecord,
                    "Campaign does not exist");
                return false;
            }

            if (campaign.Status != ActivityStatuses.Active)
            {
                result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.NotExistingDBRecord,
                    "Campaign is not active");
                return false;
            }

            if (format == null || format != null && format.IsDeleted.HasValue && format.IsDeleted.Value)
            {
                result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.NotExistingDBRecord,
                    "Affiliate channel does not exist");
                return false;
            }

            var affiliate = context.AffiliateService.GetAffiliateById(format.AffiliateId, true); //GlobalCacheTest

            context.Affiliate = affiliate;
            context.Extra["informat"] = format;
            context.AffiliateChannel = format;

            if (affiliate.Status != (short)ActivityStatuses.Active)
            {
                result.Set(RequestResult.ResultTypes.Reject, RequestResult.ErrorTypes.None,
                    "Affiliate is not active");
                return false;
            }

            if (format.Status != (short)ActivityStatuses.Active)
            {
                result.Set(RequestResult.ResultTypes.Reject, RequestResult.ErrorTypes.None,
                    "Affiliate channel is not active");
                return false;
            }

            if (format.Status == (short)ActivityStatuses.Test)
            {
                result.Set(RequestResult.ResultTypes.Reject, RequestResult.ErrorTypes.None,
                    "Affiliate channel has been suspended by network");
                return false;
            }

            var campaignTemplates =
                (List<CampaignField>)context.CampaignTemplateService.GetCampaignTemplatesByCampaignId(campaign.Id);
            var cmtpl = new Dictionary<long, CampaignField>();
            foreach (var t in campaignTemplates) cmtpl[t.Id] = t;
            context.Extra["CampaignTemplates"] = cmtpl;

            var aChannelTemplates = (List<AffiliateChannelTemplate>)context.AffiliateChannelTemplateService
                .GetAllAffiliateChannelTemplatesByAffiliateChannelId(format.Id);
            var atpl = new Dictionary<long, AffiliateChannelTemplate>();
            foreach (var t in aChannelTemplates) atpl[t.CampaignTemplateId] = t;
            context.Extra["AChannelTemplates"] = atpl;

            lead.CampaignId = format.CampaignId ?? 0;
            lead.AffiliateId = format.AffiliateId;
            lead.AffiliateChannelId = format.Id;

            var resultingValue = true;

            lead.CampaignType = campaign.CampaignType;

            leadContent.CampaignType = campaign.CampaignType;

             var processedXmlDoc = ProcessXMLData(context, receviedDataXmlDoc, campaign, format, lead, leadContent);

            if (processedXmlDoc == null)
                resultingValue = false;
            else
                lead.ReceivedData = processedXmlDoc.OuterXml;

            if (format.Status == (short)ActivityStatuses.Test) return resultingValue;

            if (resultingValue && campaign.CampaignType == CampaignTypes.LeadCampaign)
                 resultingValue = ProcessLead(context, campaign, lead, leadContent, processedXmlDoc);

            var timeSpan = Helpers.UtcNow() - context.StartDateUtc;
            lead.ProcessingTime = Math.Round(timeSpan.TotalSeconds, 1);
            ProcessingTime = (double)lead.ProcessingTime;

            return resultingValue;
        }

        /// <summary>
        ///     Gets the duplicate lead.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="campaign">The campaign.</param>
        /// <param name="lead">The lead.</param>
        /// <param name="leadContent">Content of the lead.</param>
        /// <param name="xmldoc2">The xmldoc2.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected bool? GetDuplicateLead(RequestContext context, Campaign campaign, LeadMain lead,
            LeadContent leadContent, XmlDocument xmldoc2)
        {
            var dublFound = false;
            LeadContent dubllead = null;

            if (campaign.CampaignType == (short)CampaignTypes.LeadCampaign && context.DublicateMonitor)
            {
                dubllead = context.LeadContentService.GetDublicateLead(leadContent.Ssn,
                    context.StartDateUtc.AddMinutes(-3), lead.AffiliateId);

                if (dubllead != null)
                {
                    if (dubllead.Minprice.HasValue && leadContent.Minprice < dubllead.Minprice)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            //while (SharedData.GetCounter(leadContent.Ssn + lead.AffiliateChannelId.ToString()) - 1 > 0)
                            while (context.AffiliateResponseService.CheckAffiliateResponse(dubllead.LeadId, dubllead.Minprice.Value) == null)
                            {
                                Thread.Sleep(1000);
                            }

                        }).Wait(300000);
                    }

                    var leadMain = context.LeadMainService.GetLeadMainById(dubllead.LeadId);
                    leadMain.ReceivedData = xmldoc2.OuterXml;
                    dubllead = context.LeadContentService.GetLeadContentById(dubllead.Id);

                    //if (leadMain.Status == (short)RequestResult.ResultTypes.Reject) // HAYK - 190620
                    if (leadMain.Status != (short)RequestResult.ResultTypes.Success) // HAYK - 190620
                    {
                        if (leadContent.Minprice >= dubllead.Minprice)
                        {
                            result.Set(RequestResult.ResultTypes.MinPriceError, RequestResult.ErrorTypes.None,
                                "Dublicated lead - Min price error");
                            return null;
                        }

                        dublFound = true;

                        if (!string.IsNullOrEmpty(dubllead.MinpriceStr))
                            dubllead.MinpriceStr += leadContent.Minprice == null ? "" : "," + leadContent.Minprice;
                        else dubllead.MinpriceStr = leadContent.Minprice == null ? "" : leadContent.Minprice.ToString();

                        dubllead.Minprice = leadContent.Minprice;
                        leadContent = dubllead;
                        lead = leadMain;

                        context.Extra["lead"] = lead;
                        context.Extra["leadContent"] = leadContent;
                        context.Extra["threeMinDublLeadId"] = lead.Id;

                        if (lead == null)
                        {
                            result.Set(RequestResult.ResultTypes.Error, RequestResult.ErrorTypes.None,
                                "Lead not found " + dubllead.LeadId);
                            return null;
                        }
                    }
                    /*else
                    {
                        if (!string.IsNullOrEmpty(dubllead.MinpriceStr))
                            dubllead.MinpriceStr += leadContent.Minprice == null ? "" : "," + leadContent.Minprice;
                        else dubllead.MinpriceStr = leadContent.Minprice == null ? "" : leadContent.Minprice.ToString();

                        dubllead.Minprice = leadContent.Minprice;
                        context.LeadContentService.UpdateLeadContent(dubllead);
                    }*/ // HAYK - 190620
                }
            }

            return dublFound;
        }

        /// <summary>
        ///     Checks for dublicate.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="lead">The lead.</param>
        /// <param name="leadContent">Content of the lead.</param>
        /// <param name="leadId">The lead identifier.</param>
        protected void CheckForDublicate(RequestContext context, LeadMain lead, LeadContent leadContent, long leadId)
        {
            if (context.MinProcessingMode) return;

            if (!string.IsNullOrEmpty(leadContent.Ssn))
            {
                var dubl = context.LeadContentService.CheckForDublicate(leadContent.Ssn, lead.Id);

                if (dubl.Count > 0)
                {
                    int lastDublIndex = dubl.Count - 1;

                    context.Extra["DublLeadId"] = dubl[lastDublIndex].LeadId;

                    if (dubl.Count > 1)
                        context.Extra["DublLeadId"] = dubl[0].LeadId;

                    var dl = new LeadContentDublicate
                    {
                        LeadId = dubl[lastDublIndex].LeadId,
                        OriginalLeadId = 0,
                        Ip = dubl[lastDublIndex].Ip,
                        Minprice = dubl[lastDublIndex].Minprice,
                        Firstname = dubl[lastDublIndex].Firstname,
                        Lastname = dubl[lastDublIndex].Lastname,
                        Address = dubl[lastDublIndex].Address,
                        City = dubl[lastDublIndex].City,
                        State = dubl[lastDublIndex].State,
                        Zip = dubl[lastDublIndex].Zip,
                        HomePhone = dubl[lastDublIndex].HomePhone,
                        CellPhone = dubl[lastDublIndex].CellPhone,
                        BankPhone = dubl[lastDublIndex].BankPhone,
                        Email = dubl[lastDublIndex].Email,
                        PayFrequency = dubl[lastDublIndex].PayFrequency,
                        Directdeposit = dubl[lastDublIndex].Directdeposit,

                        AccountType = dubl[lastDublIndex].AccountType,
                        IncomeType = dubl[lastDublIndex].IncomeType,
                        NetMonthlyIncome = dubl[lastDublIndex].NetMonthlyIncome,
                        Emptime = dubl[lastDublIndex].Emptime,
                        AddressMonth = dubl[lastDublIndex].AddressMonth,
                        Dob = dubl[lastDublIndex].Dob,

                        Age = dubl[lastDublIndex].Age,
                        RequestedAmount = dubl[lastDublIndex].RequestedAmount,
                        Ssn = dubl[lastDublIndex].Ssn,
                        String1 = dubl[lastDublIndex].String1,
                        String2 = dubl[lastDublIndex].String2,
                        String3 = dubl[lastDublIndex].String3,
                        String4 = dubl[lastDublIndex].String4,
                        String5 = dubl[lastDublIndex].String5,

                        Created = dubl[lastDublIndex].Created,
                        AffiliateId = dubl[lastDublIndex].AffiliateId,
                        CampaignType = dubl[lastDublIndex].CampaignType
                    };

                    dl.AffiliateId = dubl[lastDublIndex].AffiliateId;
                    dl.CampaignType = dubl[lastDublIndex].CampaignType;

                    context.LeadContentDublicateService.InsertLeadContentDublicate(dl);

                    var dublDiff = Helpers.UtcNow() - dubl[lastDublIndex].Created;

                    if ((int)dublDiff.TotalMinutes <= 3)
                        lead.Warning = 2;
                    else
                        lead.Warning = 1;
                }
            }
        }

        /// <summary>
        ///     Processes the lead.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="campaign">The campaign.</param>
        /// <param name="lead">The lead.</param>
        /// <param name="leadContent">Content of the lead.</param>
        /// <param name="xmldoc2">The xmldoc2.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected bool ProcessLead(RequestContext context, Campaign campaign, LeadMain lead, LeadContent leadContent,
            XmlDocument xmldoc2)
        {
            leadContent.MinpriceStr = leadContent.Minprice == null ? "" : leadContent.Minprice.ToString();

            var dublicateLead = context.MinProcessingMode
                ? null
                : GetDuplicateLead(context, campaign, lead, leadContent, xmldoc2);

            if (!context.MinProcessingMode && !dublicateLead.HasValue) return false;

            if (campaign.CampaignType == (short)CampaignTypes.LeadCampaign)
            {
                try
                {
                    lead.Created = Helpers.UtcNow();
                    leadContent.Created = lead.Created;
                    context.LeadCreated =
                        context.SettingService.GetTimeZoneDate(lead.Created, null, context.TimeZoneSetting);
                }
                catch
                {
                }

                try
                {
                    if (leadContent.Dob == null)
                        leadContent.Dob = DateTime.Now;
                    var tsage = DateTime.Now - (DateTime)leadContent.Dob;
                    leadContent.Age = (short)(tsage.TotalDays / 365);
                }
                catch
                {
                }

                long leadId = 0;
                context.Extra["DublLeadId"] = 0;

                if ((dublicateLead.HasValue && !dublicateLead.Value) || context.MinProcessingMode)
                {
                    lead.Warning = 0;
                    lead.RealIp = Utils.GetIPAddress();

                    leadId = context.LeadMainService.InsertLeadMain(lead);
                    context.Extra["DublLeadId"] = leadId;
                }
                else
                {
                    leadId = lead.Id;
                }

                leadContent.LeadId = leadId;
                leadContent.AffiliateId = lead.AffiliateId; //

                if ((dublicateLead.HasValue && !dublicateLead.Value) || context.MinProcessingMode)
                    context.LeadContentService.InsertLeadContent(leadContent);

                CheckForDublicate(context, lead, leadContent, leadId);
            }

            /* if (context.MinProcessingMode)
             {
                 result.Set(RequestResult.ResultTypes.Reject, RequestResult.ErrorTypes.None, "The lead was rejected");
                 return false;
             }*/

            return true;
        }

        #endregion Public methods
    }
}