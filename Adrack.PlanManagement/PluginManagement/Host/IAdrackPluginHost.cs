using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Plugin
{
    /// <summary>
    /// Provides plugin registratio interface. Plugin is registered in the calling side host, but it can perform some action
    /// after registration
    /// </summary>
    
    public interface IAdrackPluginHost
    {
        /// <summary>
        /// Plugin API Function
        /// Called after plugin is loaded in Agent side
        /// </summary>
        /// <param name="plugin">
        /// IFASPlugin interface based plugin
        /// </param>
        /// <returns>
        /// True if registration success
        /// </returns>
        bool registerMe(IAdrackPlugin plugin);
        /// <summary>
        /// Plugin API Function
        /// Called before unloading plugin from Agent side. Must clear all temporary data referenced by plugin
        /// </summary>
        /// <param name="plugin">
        /// IFASPlugin interface based plugin
        /// </param>
        /// <returns></returns>
        bool unregisterMe(IAdrackPlugin plugin);
    }

}
