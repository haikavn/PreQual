// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="SqlServerDataProvider.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Adrack.Data
{
    /// <summary>
    /// Represents a Sql Server Data Provider
    /// Implements the <see cref="Adrack.Core.Infrastructure.Data.IDataProvider" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.Data.IDataProvider" />
    public class SqlServerDataProvider : IDataProvider
    {
        #region Utilities

        /// <summary>
        /// Parse Commands
        /// </summary>
        /// <param name="filePath">File Path</param>
        /// <param name="throwExceptionIfNonExists">Throw Exception If Non Exists</param>
        /// <returns>String Array Collection</returns>
        /// <exception cref="ArgumentException"></exception>
        protected virtual string[] ParseCommands(string filePath, bool throwExceptionIfNonExists)
        {
            if (!File.Exists(filePath))
            {
                if (throwExceptionIfNonExists)
                    throw new ArgumentException(string.Format("Specified file doesn't exist - {0}", filePath));
                else
                    return new string[0];
            }

            var statements = new List<string>();

            var stream = File.OpenRead(filePath);

            using (var streamReader = new StreamReader(stream))
            {
                var statement = "";
                while ((statement = ReadNextStatementFromStream(streamReader)) != null)
                {
                    statements.Add(statement);
                }
            }
            return statements.ToArray();
        }

        /// <summary>
        /// Read Next Statement From Stream
        /// </summary>
        /// <param name="streamReader">Stream Reader</param>
        /// <returns>String</returns>
        protected virtual string ReadNextStatementFromStream(StreamReader streamReader)
        {
            var stringBuilder = new StringBuilder();

            string lineOfText;

            while (true)
            {
                lineOfText = streamReader.ReadLine();

                if (lineOfText == null)
                {
                    if (stringBuilder.Length > 0)
                        return stringBuilder.ToString();
                    else
                        return null;
                }

                if (lineOfText.TrimEnd().ToUpper() == "GO")
                    break;

                stringBuilder.Append(lineOfText + Environment.NewLine);
            }

            return stringBuilder.ToString();
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Initialize Connection Factory
        /// </summary>
        public virtual void InitializeConnectionFactory()
        {
            var sqlConnectionFactory = new SqlConnectionFactory();

#pragma warning disable 0618
            Database.DefaultConnectionFactory = sqlConnectionFactory;
        }

        /// <summary>
        /// Initialize Database
        /// </summary>
        public virtual void InitializeDatabase()
        {
            InitializeConnectionFactory();
            SetDatabaseInitializer();
        }

        /// <summary>
        /// Set Database Initializer
        /// </summary>
        public virtual void SetDatabaseInitializer()
        {
            // Do Not Create The Objects In Database If It Deost Not Exist
            Database.SetInitializer<AppObjectContext>(null);
        }

        /// <summary>
        /// Gets a support database parameter object (used by stored procedures)
        /// </summary>
        /// <returns>Parameter</returns>
        public virtual DbParameter GetParameter()
        {
            return new SqlParameter();
        }

        #endregion Methods
    }
}