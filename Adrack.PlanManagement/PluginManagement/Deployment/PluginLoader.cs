using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Adrack.Plugin;

namespace Adrack.PluginManagement
{
    /// <summary>
    /// Internal class. Loads plugin in isolated environment. Provides access method to plugin working result
    /// </summary>
    public class PluginLoader : MarshalByRefObject
    {
        IAdrackPlugin plugin;
        public string getName()
        {
            return plugin.name;
        }

        public string getDescription()
        {
            return plugin.description;
        }


        public bool hasPlugin()
        {
            return plugin != null;
        }

        public int getStatus()
        {
            return plugin.GetStatus();
        }
        public PluginFileInfo[] getFiles()
        {
            return plugin.GetFiles();
        }

        public void removeFiles(string[] files)
        {
            plugin.RemoveFiles(files);
        }

        public void setPluginHost(IAdrackPluginHost host)
        {
            plugin.host = host;
        }


        public int getProgress()
        {
            return plugin.GetProgress();
        }

        public string executeCustomCommand(string command, string[] parameters)
        {
            return plugin.ExecuteCustomFunction(command, parameters);
        }

        public void LoadPlugins(string assemblyName)
        {
            plugin = null;
            var assemb = Assembly.LoadFrom(assemblyName);

            var types = from type in assemb.GetTypes()
                        where typeof(IAdrackPlugin).IsAssignableFrom(type)
                        select type;

            var instances = types.Select(
                v => (IAdrackPlugin)Activator.CreateInstance(v)).ToArray();

            plugin = instances[0];

        }

        public void setConfiguration(string config)
        {
            plugin.SetConfiguration(config);
        }
    }

}
