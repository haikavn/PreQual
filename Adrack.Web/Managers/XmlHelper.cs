// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="XmlHelper.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections;
using System.Xml;

namespace Adrack
{
    /// <summary>
    ///     Class XmlHelper.
    /// </summary>
    public class XmlHelper
    {
        private readonly Hashtable elementsCache = new Hashtable();

        public XmlNode GetElementsByTagName(XmlDocument doc, string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            var code = doc.GetHashCode();
            var node = (XmlNode)elementsCache[code + name];
            if (node != null)
                return node;

            node = FindElementsByTagName(doc, name);

            elementsCache[code + name] = node;

            return node;
        }

        public XmlNode FindElementsByTagName(XmlDocument doc, string name)
        {
            var node = doc.GetElementsByTagName(name);
            if (node.Count > 0) return node[0];
            /*foreach (XmlNode node in doc.ChildNodes)
            {
                if (node.Name == name) return node;
                if (node.ChildNodes != null && node.ChildNodes.Count > 0)
                {
                    var res = FindElementsByTagName(node, name);
                    if (res != null) return res;
                }
            }

             return null;*/
            return null;
        }
        
    }
}