// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="Helpers.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using AdRack.Buffering;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace Adrack
{
    /// <summary>
    ///     Class Helpers.
    /// </summary>
    public class Helpers
    {
        private static double seconds = 0;
        private static int days = 30;

        private static double coef = 0.015;

        private static double dayLimit = 10000;

        private static bool dateSimulation = false;

        public static DateTime UtcNow()
        {
            if (dateSimulation && days >= 0)
            {
                seconds += 1.0 * coef;
                if (seconds > dayLimit * coef)
                {
                    days--;
                    Random rnd = new Random();
                    dayLimit = rnd.Next(10000, 20000);
                    seconds = 0;
                }

                return DateTime.UtcNow.AddDays(-days).AddSeconds(seconds);
            }
            else
            {
                return DateTime.UtcNow;
            }
        }
        public static string GetServerIPAddress()
        {
            string strHostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            return ipAddress.ToString();
        }

        /// <summary>
        ///     Gets the base URL.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>System.String.</returns>
        public static string GetBaseUrl(HttpRequestBase request)
        {
            var scheme = "http";

            try
            {
                if (request.IsSecureConnection)
                    scheme = "https";
                else
                    scheme = "http";
            }
            catch
            {
                scheme = "http";
            }

            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/") appUrl += "/";

            var baseUrl = string.Format("{0}://{1}{2}", scheme, request.Url.Authority, appUrl);
            //var baseUrl = string.Format("{1}{2}", scheme, request.Url.Authority, appUrl);

            return baseUrl;
        }

        /// <summary>
        ///     Posts the XML.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="xml">The XML.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="postingHeaders">The posting headers.</param>
        /// <returns>System.String.</returns>
        public static string PostXml(string url, string xml, int timeout, string contentType,
            Dictionary<string, string> postingHeaders, string postingHeadersStr, string method = "POST")
        {
            try
            {
                if (method == null)
                    method = "POST";

                if (method.ToLower() == "get")
                {
                    url += "?" + xml;
                }

                var data = Encoding.UTF8.GetBytes(xml);

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls12;

                var request = (HttpWebRequest)WebRequest.Create(url);

                request.Timeout = timeout == 0 ? 30000 : timeout;
                request.Method = method.ToUpper();
                request.ContentType = contentType;

                //if (!string.IsNullOrEmpty(contentType))
                  //  request.Headers.Add("Content-Type", contentType);

                foreach (var key in postingHeaders.Keys) request.Headers.Add(key, postingHeaders[key]);

                XmlDocument soapEnvelopeXml = new XmlDocument();

                if (method.ToLower() == "post")
                {
                    if (!string.IsNullOrEmpty(postingHeadersStr))
                    {
                        postingHeadersStr = postingHeadersStr.Replace("<!--Input Data-->", xml);
                        data = Encoding.UTF8.GetBytes(postingHeadersStr);
                    }
                    request.ContentLength = data.Length;

                    using (var stream = request.GetRequestStream())
                    {  
                        stream.Write(data, 0, data.Length);
                    }
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                //if (responseString.Length > 1490)
                  //  responseString = responseString.Substring(0, 1490);
                return responseString;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.Timeout)
                {
                    return "RequestTimeout";
                }

                try
                {
                    if (contentType == "application/json")
                    {
                        using (WebResponse response = ex.Response)
                        {
                            HttpWebResponse httpResponse = (HttpWebResponse)response;
                            using (Stream data2 = response.GetResponseStream())
                            {
                                string text = new StreamReader(data2).ReadToEnd();
                                return text;
                            }
                        }
                    }
                }
                catch { }
                return ex.Message;
            }
        }

        public static string Post(string url, string data, string contentType = "application/x-www-form-urlencoded", string method = "POST")
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            if (method.ToLower() == "get")
            {
                url += "?" + data;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentType = contentType;
            request.Method = method;

            if (method.ToLower() == "post")
            {
                request.ContentLength = dataBytes.Length;
                using (Stream requestBody = request.GetRequestStream())
                {
                    requestBody.Write(dataBytes, 0, dataBytes.Length);
                }
            }
            

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        ///     Generates the string.
        /// </summary>
        /// <param name="len">The length.</param>
        /// <returns>System.String.</returns>
        public static string GenerateString(int len)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[len];
            var random = new Random(Guid.NewGuid().GetHashCode());

            for (var i = 0; i < stringChars.Length; i++) stringChars[i] = chars[random.Next(chars.Length)];

            var finalString = new string(stringChars);

            return finalString;
        }

        /// <summary>
        ///     Corrects the data.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="data">The data.</param>
        /// <param name="cmp">The CMP.</param>
        /// <param name="xmldoc">The xmldoc.</param>
        /// <returns>System.String.</returns>
        public static string CorrectData(Adrack.Managers.RequestContext context, string data, out XmlDocument xmldoc)
        {
            xmldoc = new XmlDocument();

            if (data.IsValidJson(false))
                data = StructuredDataBuffering.JsonToXmlString(data);

            try
            {
                xmldoc.LoadXml(data);
            }
            catch
            {
                return null;
            }

            return data;
        }

        public static void FillFromQuery(Adrack.Managers.RequestContext context, XmlDocument xmldoc)
        {
            //queryString
            foreach(string key in context.HttpRequest.QueryString.AllKeys)
            {
                XmlNodeList nodeList = xmldoc.GetElementsByTagName(key);
                if(nodeList.Count > 0)
                {
                    nodeList[0].InnerText = context.HttpRequest.QueryString[key];
                }
            }
        }

        public static string XmlToQueryString(XmlNode doc, int level=0)
        {
            string result = "";
            if (doc.FirstChild == null || !(doc.FirstChild is XmlElement))
            {
                //if (!String.IsNullOrEmpty(doc.InnerText)) //HAYK 290520
                {
                    result = "&" + doc.Name + "=" + HttpUtility.UrlEncode(doc.InnerText);
                }
                return result;
            }
            else
            foreach (XmlNode node in doc.ChildNodes)
            {
                if (node.ChildNodes != null)// && node.ChildNodes.Count > 0) // HAYK 290520
                {
                    result+=XmlToQueryString(node,level+1); 
                }
            }
            if (level == 0 && result.StartsWith("&"))
                return result.Substring(1); //remove first &
            else
                return result;           
        }

        public static string XmlToQueryString(string xml, int level = 0)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string result = "";
            if (doc.FirstChild == null || !(doc.FirstChild is XmlElement))
            {
               //if (!String.IsNullOrEmpty(doc.InnerText)) //HAYK 230520
                {
                    result = "&" + doc.Name + "=" + HttpUtility.UrlEncode(doc.InnerText);
                }
                return result;
            }
            else
                foreach (XmlNode node in doc.ChildNodes)
                {
                    if (node.ChildNodes != null && node.ChildNodes.Count > 0)
                    {
                        result += XmlToQueryString(node, level + 1);
                    }
                }
            if (level == 0 && result.StartsWith("&"))
                return result.Substring(1); //remove first &
            else
                return result;
        }
    }
}