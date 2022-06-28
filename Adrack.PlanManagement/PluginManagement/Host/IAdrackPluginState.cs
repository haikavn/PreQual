using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Plugin
{
    /// <summary>
    /// Plugin state constants
    /// </summary>
    public class PluginState
    {
        /// <summary>
        /// Means that plug-in busy and GetFiles or GetStreams method should be called
        /// </summary>
         public const int BUSY=1;
        /// <summary>
        /// Means that plug-in is collecting data
        /// </summary>
         public const int COLLECTING_DATA=2;
        /// <summary>
        /// Plug-in is ready for reading data
        /// </summary>
         public const int READY = 0;
    }
}
