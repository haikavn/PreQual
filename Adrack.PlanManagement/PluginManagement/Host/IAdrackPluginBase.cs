using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Plugin
{
    /// <summary>
    /// Generic plugin interface
    /// </summary>
    public interface IAdrackPluginBase
    {
            /// <summary>            
            /// Returns description of plug-in interface
            /// </summary>
            string description { get; }
            
            /// <summary>
            /// Returns name of plugin base
            /// </summary>
            
            string name { get; }
            
            /// <summary>
            /// Returns state of plugin from PluginState definitions
            /// </summary>
            int state { get; }
    }
}
