// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="DataSettingManager.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Hosting;

namespace Adrack.Core.Infrastructure.Data
{
    /// <summary>
    /// Represents a Data Setting Manager
    /// </summary>
    public partial class DataSettingManager
    {
        #region Constants

        /// <summary>
        /// Separator
        /// </summary>
        protected const char separator = ':';

        /// <summary>
        /// File Name
        /// </summary>

#if DEBUG
        protected const string dataSettingFileName = "ConnectionStringDebug.txt";
#else
            protected const string dataSettingFileName = "ConnectionStringRelease.txt";
#endif

        #endregion Constants

        #region Utilities

        /// <summary>
        /// Get Root Path
        /// </summary>
        /// <param name="rootPath">Root Path</param>
        /// <returns>String Item</returns>
        public virtual string GetRootPath(string rootPath)
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HostingEnvironment.MapPath(rootPath);
            }
            else
            {
                //not hosted. For example, run in unit tests
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug", "");

                rootPath = rootPath.Replace("~/", "").TrimStart('/').Replace('/', '\\');

                return Path.Combine(baseDirectory, rootPath);
            }
        }

        /// <summary>
        /// Parse Setting
        /// </summary>
        /// <param name="valueString">Value String</param>
        /// <returns>Data Setting</returns>
        protected virtual DataSetting ParseSetting(string valueString)
        {
            var shellDataSetting = new DataSetting();

            if (String.IsNullOrEmpty(valueString))
                return shellDataSetting;

            var dataSetting = new List<string>();

            using (var stringReader = new StringReader(valueString))
            {
                string valueRead;

                while ((valueRead = stringReader.ReadLine()) != null)
                    dataSetting.Add(valueRead);
            }

            foreach (var setting in dataSetting)
            {
                var separatorIndex = setting.IndexOf(separator);

                if (separatorIndex == -1)
                {
                    continue;
                }

                string key = setting.Substring(0, separatorIndex).Trim();

                string value = setting.Substring(separatorIndex + 1).Trim();

                switch (key)
                {
                    case "DataProvider":
                        shellDataSetting.DataProvider = value;
                        break;

                    case "DataConnectionString":
                        shellDataSetting.DataConnectionString = value;
                        break;

                    default:
                        shellDataSetting.RawDataSetting.Add(key, value);
                        break;
                }
            }

            return shellDataSetting;
        }

        /// <summary>
        /// Compose Setting
        /// </summary>
        /// <param name="dataSetting">Data Setting</param>
        /// <returns>String</returns>
        protected virtual string ComposeSetting(DataSetting dataSetting)
        {
            if (dataSetting == null)
                return "";

            return string.Format("DataProvider: {0}{2}DataConnectionString: {1}{2}", dataSetting.DataProvider, dataSetting.DataConnectionString, Environment.NewLine);
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Load Setting
        /// </summary>
        /// <param name="dataSettingFilePath">Data Setting File Path</param>
        /// <returns>Data Setting</returns>
        public virtual DataSetting LoadSetting(string dataSettingFilePath = null, bool loadFromEnvironmentVariables = true)
        {
            if (loadFromEnvironmentVariables)
            {
                string dbConnectionString = "";//Environment.GetEnvironmentVariable("DataConnectionString");
                string dataProvider = "";//Environment.GetEnvironmentVariable("DataProvider");

                if (string.IsNullOrEmpty(dataProvider))
                    dataProvider = ConfigurationManager.AppSettings["DataProvider"];

                if (string.IsNullOrEmpty(dataProvider))
                    dataProvider = "SQLServer";

                if (string.IsNullOrEmpty(dbConnectionString))
                {
                    var dbConnectionStringSetting = ConfigurationManager.ConnectionStrings["DataConnectionString"];
                    if (dbConnectionStringSetting != null)
                        dbConnectionString = dbConnectionStringSetting.ConnectionString;
                }

                if (string.IsNullOrEmpty(dbConnectionString))
                    dbConnectionString = ConfigurationManager.AppSettings["DataConnectionString"];

                string defaultDatabaseName = ConfigurationManager.AppSettings["DataConnectionDefaultDatabase"];

                if (!string.IsNullOrEmpty(dbConnectionString) && !string.IsNullOrEmpty(dataProvider))
                {
                    if (!string.IsNullOrEmpty(defaultDatabaseName))
                        dbConnectionString = string.Format(dbConnectionString, defaultDatabaseName);
                    var shellDataSetting = new DataSetting();
                    shellDataSetting.DataConnectionString = dbConnectionString;
                    shellDataSetting.DataProvider = dataProvider;
                    return shellDataSetting;
                }
            }

            return new DataSetting();

            /*string rootPath = "";
            if (String.IsNullOrEmpty(dataSettingFilePath))
            {
                rootPath = GetRootPath("~/App_Data/");
                dataSettingFilePath = Path.Combine(rootPath, dataSettingFileName);
            }

            if (File.Exists(dataSettingFilePath))
            {
                string text = File.ReadAllText(dataSettingFilePath);
                return ParseSetting(text);
            }
            else
                return new DataSetting();*/
        }


        /// <summary>
        /// Save Setting
        /// </summary>
        /// <param name="dataSetting">Data Setting</param>
        /// <exception cref="ArgumentNullException">dataSetting</exception>
        public virtual void SaveSetting(DataSetting dataSetting)
        {
            if (dataSetting == null)
                throw new ArgumentNullException("dataSetting");

            string dataSettingFilePath = Path.Combine(GetRootPath("~/App_Data/"), dataSettingFileName);

            if (!File.Exists(dataSettingFilePath))
            {
                using (File.Create(dataSettingFilePath))
                {
                    //we use 'using' to close the file after it's created
                }
            }

            var textString = ComposeSetting(dataSetting);

            File.WriteAllText(dataSettingFilePath, textString);
        }

        #endregion Methods

    }
}