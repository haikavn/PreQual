using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Adrack.WebApi.XMLImportExport
{
    /// <summary>
    /// 
    /// Defines repositiory rule
    /// </summary>
    class RepositoryRule
    {
        /// <summary>
        /// Must return true if data is valid
        /// </summary>
        /// <param name="paramName">
        /// Name of parameter to validate
        /// </param>
        /// <param name="paramValue">
        /// Param value to validate
        /// </param>
        /// <returns></returns>
        public virtual bool IsValid(string paramName, string paramValue)
        {
            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public virtual bool IsValid(string paramValue)
        {

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="binary"></param>
        /// <returns></returns>
        public virtual bool IsValid(byte[] binary)
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public virtual bool IsValid(Stream stream)
        {
            return true;
        }
     
    }
}
