using System;
using System.IO;
using System.Web.Hosting;
using System.Xml;

namespace UnitTest_Services
{
    public class TestSettings
    {
        public TestSettings()
        {
            SettingsXmlDocument = new XmlDocument();
            Module = null;
        }

        public XmlDocument SettingsXmlDocument { get; set; }
        protected XmlNode Module { get; set; }

        public void Load(string moduleName, string fileName = "TestSettings.xml")
        {
            var path = GetRootPath("~/App_Data/");
            SettingsXmlDocument.Load(path + "\\" + fileName);
            var nodes = SettingsXmlDocument.GetElementsByTagName(moduleName);
            if (nodes.Count > 0)
                Module = nodes[0];
        }

        public string GetValue(string methodName, string fieldName)
        {
            if (Module == null) return "";

            foreach (XmlNode m in Module.ChildNodes)
                if (m.Name == methodName)
                    foreach (XmlNode f in m.ChildNodes)
                        if (f.Name == fieldName)
                            return f.InnerText;

            return "";
        }

        public virtual string GetRootPath(string rootPath)
        {
            if (HostingEnvironment.IsHosted) return HostingEnvironment.MapPath(rootPath);

            //not hosted. For example, run in unit tests
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug", "");

            rootPath = rootPath.Replace("~/", "").TrimStart('/').Replace('/', '\\');

            return Path.Combine(baseDirectory, rootPath);
        }
    }
}