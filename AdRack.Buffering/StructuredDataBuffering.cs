// ***********************************************************************
// Assembly         : AdRack.Buffering
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="StructuredDataBuffering.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Newtonsoft.Json;

namespace AdRack.Buffering
{
    /// <summary>
    ///     Converts existing XML files to lineral buffer and implements fast access and modification mechanism
    /// </summary>
    public static class StructuredDataBuffering
    {
        /// <summary>
        ///     Dates the time to unique identifier.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>Guid.</returns>
        public static Guid DateTimeToGuid(DateTime dateTime)
        {
            var guid = Guid.NewGuid();

            var gStr = guid.ToString();
            var guidEnd = gStr.Substring(gStr.IndexOf("-"), gStr.Length - gStr.IndexOf("-"));
            var guidStart = dateTime.ToString("yyyyMMdd");
            guidStart = guidStart.PadRight(8, '0');
            guid = new Guid(guidStart + guidEnd);

            return guid;
        }

        /// <summary>
        ///     Determines whether [is valid json] [the specified parse].
        /// </summary>
        /// <param name="strInput">The string input.</param>
        /// <param name="parse">if set to <c>true</c> [parse].</param>
        /// <returns><c>true</c> if [is valid json] [the specified parse]; otherwise, <c>false</c>.</returns>
        public static bool IsValidJson(this string strInput, bool parse)
        {
            strInput = strInput.Trim();
            if (strInput.StartsWith("{") && strInput.EndsWith("}") || //For object
                strInput.StartsWith("[") && strInput.EndsWith("]")) //For array
                try
                {
                    if (parse)
                    {
                        var obj = JsonConvert.DeserializeObject(strInput);
                    }

                    return true;
                }
                catch // not valid
                {
                    return false;
                }

            return false;
        }

        /// <summary>
        ///     Determines whether [is minimally valid XML] [the specified text].
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parse">if set to <c>true</c> [parse].</param>
        /// <returns><c>true</c> if [is minimally valid XML] [the specified text]; otherwise, <c>false</c>.</returns>
        public static bool IsMinimallyValidXml(string text, bool parse)
        {
            if (!string.IsNullOrEmpty(text) && text.TrimStart().StartsWith("<"))
            {
                Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(text));
                var settings = new XmlReaderSettings
                {
                    CheckCharacters = true,
                    ConformanceLevel = ConformanceLevel.Document,
                    DtdProcessing = DtdProcessing.Ignore,
                    IgnoreComments = true,
                    IgnoreProcessingInstructions = true,
                    IgnoreWhitespace = true,
                    ValidationFlags = XmlSchemaValidationFlags.None,
                    ValidationType = ValidationType.None
                };
                bool isValid;

                using (var xmlReader = XmlReader.Create(stream, settings))
                {
                    try
                    {
                        if (parse)
                            while (xmlReader.Read())
                                ; // This space intentionally left blank
                        isValid = true;
                    }
                    catch (XmlException)
                    {
                        isValid = false;
                    }
                }

                return isValid;
            }

            return false;
        }

        /// <summary>
        ///     Jsons to XML.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        /// <returns>XDocument.</returns>
        public static XDocument JsonToXml(string jsonString)
        {
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(jsonString)))
            {
                var quotas = new XmlDictionaryReaderQuotas();
                return XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(stream, quotas));
            }
        }

        /// <summary>
        ///     Replaces the first.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="search">The search.</param>
        /// <param name="replace">The replace.</param>
        /// <returns>System.String.</returns>
        public static string ReplaceFirst(string text, string search, string replace)
        {
            var pos = text.IndexOf(search);
            if (pos < 0) return text;
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        /// <summary>
        ///     Jsons to XML string.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        /// <returns>System.String.</returns>
        public static string JsonToXmlString(string jsonString)
        {
            var doc = JsonToXml(jsonString);
            var writer = new StringWriter();
            if (doc.Root.FirstNode == doc.Root.LastNode)
                (doc.Root.FirstNode as XElement).Save(writer, SaveOptions.None);
            else
                doc.Root.Save(writer, SaveOptions.None);

            if (doc.Root.Name.LocalName.ToLower() == "root")
                doc.Root.Name = "tplroot";

            return ReplaceFirst(doc.ToString(), "encoding=\"utf-16\"", "encoding=\"utf-8\"");
            ;
        }

        /// <summary>
        ///     XMLs to json.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="includeRoot">if set to <c>true</c> [include root].</param>
        /// <returns>System.String.</returns>
        public static string XmlToJSON(string xml, bool includeRoot)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            return XmlToJSON(doc, includeRoot);
        }

        /// <summary>
        ///     XMLs to json.
        /// </summary>
        /// <param name="xmlDoc">The XML document.</param>
        /// <param name="includeRoot">if set to <c>true</c> [include root].</param>
        /// <returns>System.String.</returns>
        public static string XmlToJSON(XmlDocument xmlDoc, bool includeRoot)
        {
            var sbJSON = new StringBuilder();
            sbJSON.Append("{ ");
            XmlToJSONnode(sbJSON, xmlDoc.DocumentElement, includeRoot);
            sbJSON.Append("}");
            return sbJSON.ToString();
        }

        //  XmlToJSONnode:  Output an XmlElement, possibly as part of a higher array
        /// <summary>
        ///     XMLs to jso nnode.
        /// </summary>
        /// <param name="sbJSON">The sb json.</param>
        /// <param name="node">The node.</param>
        /// <param name="showNodeName">if set to <c>true</c> [show node name].</param>
        private static void XmlToJSONnode(StringBuilder sbJSON, XmlElement node, bool showNodeName)
        {
            if (showNodeName)
            {
                sbJSON.Append("\"" + SafeJSON(node.Name) + "\": ");
                //sbJSON.Append("{");//HAYK
            }

            // Build a sorted list of key-value pairs
            //  where   key is case-sensitive nodeName
            //          value is an ArrayList of string or XmlElement
            //  so that we know whether the nodeName is an array or not.
            var childNodeNames = new SortedList<string, object>();

            //  Add in all node attributes
            /*if (node.Attributes != null)
                foreach (XmlAttribute attr in node.Attributes)
                    StoreChildNode(childNodeNames, attr.Name, attr.InnerText);*/ //HAYK

            //  Add in all nodes
            foreach (XmlNode cnode in node.ChildNodes)
            {
                /*if (cnode is XmlText)
                    StoreChildNode(childNodeNames, "value", cnode.InnerText);
                else if (cnode is XmlElement)
                    StoreChildNode(childNodeNames, cnode.Name, cnode);*/
                StoreChildNode(childNodeNames, cnode.Name, cnode.InnerText);
            }

            // Now output all stored info
            foreach (var childname in childNodeNames.Keys)
            {
                var alChild = (List<object>) childNodeNames[childname];
                if (alChild.Count == 1)
                {
                    OutputNode(childname, alChild[0], sbJSON, true);
                }
                else
                {
                    sbJSON.Append(" \"" + SafeJSON(childname) + "\": [ ");
                    foreach (var Child in alChild)
                        OutputNode(childname, Child, sbJSON, false);
                    sbJSON.Remove(sbJSON.Length - 2, 2);
                    sbJSON.Append(" ], ");
                }
            }

            sbJSON.Remove(sbJSON.Length - 2, 2);

            //if (showNodeName)
              //  sbJSON.Append(" }"); // HAYK
        }

        //  StoreChildNode: Store data associated with each nodeName
        //                  so that we know whether the nodeName is an array or not.
        /// <summary>
        ///     Stores the child node.
        /// </summary>
        /// <param name="childNodeNames">The child node names.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="nodeValue">The node value.</param>
        private static void StoreChildNode(SortedList<string, object> childNodeNames, string nodeName, object nodeValue)
        {
            // Pre-process contraction of XmlElement-s
            if (nodeValue is XmlElement)
            {
                // Convert  <aa></aa> into "aa":null
                //          <aa>xx</aa> into "aa":"xx"
                var cnode = (XmlNode) nodeValue;
                if (cnode.Attributes.Count == 0)
                {
                    var children = cnode.ChildNodes;
                    if (children.Count == 0)
                        nodeValue = null;
                    else if (children.Count == 1 && children[0] is XmlText)
                        nodeValue = ((XmlText) children[0]).InnerText;
                }
            }

            // Add nodeValue to ArrayList associated with each nodeName
            // If nodeName doesn't exist then add it
            List<object> ValuesAL;

            if (childNodeNames.ContainsKey(nodeName))
            {
                ValuesAL = (List<object>) childNodeNames[nodeName];
            }
            else
            {
                ValuesAL = new List<object>();
                childNodeNames[nodeName] = ValuesAL;
            }

            ValuesAL.Add(nodeValue);
        }

        /// <summary>
        ///     Outputs the node.
        /// </summary>
        /// <param name="childname">The childname.</param>
        /// <param name="alChild">The al child.</param>
        /// <param name="sbJSON">The sb json.</param>
        /// <param name="showNodeName">if set to <c>true</c> [show node name].</param>
        private static void OutputNode(string childname, object alChild, StringBuilder sbJSON, bool showNodeName)
        {
            if (alChild == null)
            {
                if (showNodeName)
                    sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
                sbJSON.Append("null");
            }
            else if (alChild is string)
            {
                if (showNodeName)
                    sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
                var sChild = (string) alChild;
                sChild = sChild.Trim();
                sbJSON.Append("\"" + SafeJSON(sChild) + "\"");
            }
            else
            {
                XmlToJSONnode(sbJSON, (XmlElement) alChild, showNodeName);
            }

            sbJSON.Append(", ");
        }

        // Make a string safe for JSON
        /// <summary>
        ///     Safes the json.
        /// </summary>
        /// <param name="sIn">The s in.</param>
        /// <returns>System.String.</returns>
        private static string SafeJSON(string sIn)
        {
            var sbOut = new StringBuilder(sIn.Length);
            foreach (var ch in sIn)
            {
                if (char.IsControl(ch) || ch == '\'')
                {
                    var ich = (int) ch;
                    sbOut.Append(@"\u" + ich.ToString("x4"));
                    continue;
                }

                if (ch == '\"' || ch == '\\' || ch == '/')
                {
                    sbOut.Append('\\');
                }

                sbOut.Append(ch);
            }

            return sbOut.ToString();
        }
    }
}