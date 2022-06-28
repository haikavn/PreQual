
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Adrack.Plugin
{

    [Serializable]
    public class PluginFileInfo
    {
        public FileInfo info;
        public string metadata;
        public string pluginInfo;
        public string startDate;
        public string endDate;
        public int utcHoursDifference; 
    }
    /// <summary>
    /// Provides FAS plugin interface. Excludes callback connection.
    /// Agent side must check the status of plugin before gettign data
    /// Plugin must return collected files or stream after working process has been finished
    /// </summary>
    public interface IAdrackPlugin : IAdrackPluginBase
    {
        /// <summary>
        /// Plugin API Function
        /// Starts plug-in. This function must initialize any procedures (such as temporary folder creation) for plugin
        /// </summary>
        /// <returns>
        /// True if starting operation success. 
        /// Function must throw an Exception of can describe a reason of fail, or must return false in case if starting process is failed
        /// </returns>
        bool Start();
        /// <summary>
        /// Plugin API Function
        /// Stops plug-in working process, can be called after Start() or Execute()
        /// </summary>
        /// <returns>True if stopping success</returns>
        bool Stop();

        /// <summary>
        /// Plugin API Function.
        /// Execute plug-in job
        /// </summary>
        void Execute();


        int GetProgress();
       
        /// <summary>
        /// Plugin API Function.
        /// Execute custom function
        /// </summary>
        string ExecuteCustomFunction(string ActionName, string[] parameters);
        /// <summary>
        /// Plugin API Function.
        /// Returns status of plugin, described i PluginStatus class
        /// </summary>
        /// <returns>On of predefined values of PluginStatus class</returns>
        int GetStatus();

        /// <summary>
        /// Plugin API Function.
        /// Sets configuration of plugin via JSON string
        /// </summary>
        /// <param name="jonString">
        /// Definition of configuration data, comping with JSON format (To be defined)
        /// </param>
        void SetConfiguration(string jonString);

        /// <summary>
        /// Plugin API Function.
        /// Returns configuration string of plugin via JSON format
        /// </summary>
        /// <returns>Configuration string</returns>
        string GetConfiguration();


        void RemoveFiles(string[] fileNames);
        

        /// <summary>
        /// Plugin API Function.
        /// Returns FileInfo structured collected by plugin job
        /// </summary>
        /// <returns>FileInfo array</returns>
        PluginFileInfo[] GetFiles();

        /// <summary>
        /// Returns Stream interfaces of collected data or real-time streams
        /// </summary>
        /// <returns>Stream array</returns>
        Stream[] GetStreams();
        
        /// <summary>
        /// Plugin API Function.
        /// Reference to plugin host
        /// </summary>
        IAdrackPluginHost host { get; set; }
        
    }
}
