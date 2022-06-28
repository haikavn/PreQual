using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Adrack.Plugin;
using System.IO;
using System.Security.Policy;
using Adrack.PlanManagement;
using Adrack.DataUtils;

namespace Adrack.PluginManagement
{

    /// <summary>
    /// The record for storing plug-in reference, including separate AppDomain
    /// </summary>
    class PluginDomainRecord
    {
        public AppDomain domain;        
        public string name;
        public PluginLoader loader;
        
    }

    /// <summary>
    /// Provides functionality for loading and registering plugins in different application domains
    /// </summary>
    /// 
    [Serializable]
    public class PluginHost : Component, IAdrackPluginHost
    {

        string[] validPlugins=new string[0];

        public void setValidPlugins(string validList)
        {            
            if (validList == null) return;
            validPlugins=validList.Split(',');
        }

        public void setValidPlugins(List<string> validPluginsList)
        {

            validPlugins = validPluginsList.ToArray();
        }

        public bool isPluginValid(string pluginName)
        {
            for (int i = 0; i < validPlugins.Length; i++)
                if (validPlugins[i] == pluginName) return true;
            return false;
        }

        AssemblyReflectionManager manager = null;
        List<PluginLoader> plugins = new List<PluginLoader>();
        List<PluginDomainRecord> pluginRecords = new List<PluginDomainRecord>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public PluginHost()
        {
          
            
        }

        string lastConfig = "";
        /// <summary>
        /// Apply given configuration to all plugins
        /// </summary>
        public void applyConfiguration(string config)
        {
            lastConfig = config;
            for (int i = 0; i < plugins.Count; i++)
            {
                if (isPluginValid(plugins[i].getName()))
                plugins[i].setConfiguration(config);                
            }
        }


        public void removeFiles(string[] files)
        {
            for (int i = 0; i < plugins.Count; i++)
            {
                if (isPluginValid(plugins[i].getName()))
                plugins[i].removeFiles(files);
            }
        }

        public int getProgress()
        {
            for (int i = 0; i < plugins.Count; i++)
            {
                if (!isPluginValid(plugins[i].getName())) continue;
                int prog = plugins[i].getProgress();
                if (prog>0)
                return prog;
            }
            return 0;
        }


        /// <summary>
        /// Apply given configuration to all plugins
        /// </summary>
        public string executeCustomCommand(string command, string[] parameters)
        {
            for (int i = 0; i < plugins.Count; i++)
            {
                if (!isPluginValid(plugins[i].getName())) continue;
                string res=plugins[i].executeCustomCommand(command, parameters);
                if (res != null && res != "")
                    return res;
            }
            return null;
        }


        /// <summary>
        /// Collect files from all plugins with READY state
        /// </summary>
        /// <returns>
        /// List of collected FileInfo records
        /// </returns>
        public List<PluginFileInfo> collectFiles(bool reget)
        {
            bool valid = false;
            DMUtils.DebugLog("Collecting files, number of plug-ins:"+plugins.Count);

            List<PluginFileInfo> list = new List<PluginFileInfo>();
            for (int i = 0; i < plugins.Count; i++)
            {
                if (!isPluginValid(plugins[i].getName())) continue;
                              

                valid = true;
                
                if (reget)
                    if (plugins[i].getName() == "Test") continue;

                if (plugins[i].getStatus() == PluginState.READY)
                {
                    PluginFileInfo[] arr = plugins[i].getFiles();

                    for (int j = 0; j < arr.Length; j++)
                    {
                        FileInfo info = new FileInfo(arr[j].info.FullName);
                        PluginFileInfo fInfo = new PluginFileInfo();
                        fInfo.metadata = arr[j].metadata;
                        fInfo.pluginInfo = arr[j].pluginInfo;
                        fInfo.startDate = arr[j].startDate;
                        fInfo.endDate = arr[j].endDate;
                        fInfo.info = info;
                        DMUtils.DebugLog("File found from plugin:"+arr[j].info.FullName);
                        list.Add(fInfo);
                    }
                }
            }
            if (!valid) DMUtils.DebugLog("No enabled plugins");
            else
            DMUtils.DebugLog("Collected files:"+list.Count);
            return list;
        }




        /// <summary>
        /// Collect files from all plugins with READY state
        /// </summary>
        /// <returns>
        /// List of collected FileInfo records
        /// </returns>
        public void refresh()
        {
            try
            {
                int toUnload = 0;
                List<FileInfo> list = new List<FileInfo>();
                for (int i = 0; i < plugins.Count; i++)
                {                    
                    if (plugins[i].getStatus() == PluginState.READY)
                    {
                        if (toUnload == 1)
                            throw new Exception("Test unload");

                    }
                }
            }
            catch (Exception e)
            {
                DMUtils.DebugLog("Unloading plugin");
                this.loadPlugins("plugins");
                this.applyConfiguration(lastConfig);
            }

            //this.loadPlugins("plugins");
            //this.applyConfiguration(lastConfig);
         
        }


        /// <summary>
        /// Returns list of registered plug-ins
        /// </summary>
        /// <returns>
        /// List of registered plug-ins.
        /// </returns>
        public List<PluginLoader> getPlugins()
        {
            return plugins;
        }


        /// <summary>
        /// Registers plugin in host list
        /// </summary>
        /// <param name="plugin">Reference to IAdrackPlugin based interface</param>
        /// <returns>True if operation success or false if plugin already registered </returns>
        public bool registerMe(IAdrackPlugin plugin)
        {  
            
            return true;
        }
        /// <summary>
        /// Removes plugin for registration list
        /// </summary>
        /// <param name="plugin">IAdrackPlugin based object</param>
        /// <returns>True if operation success or false if plugin was not registered</returns>
        public bool unregisterMe(IAdrackPlugin plugin)
        {
          
            return true;
        }

        /// <summary>
        /// Loads all plugins from specific folder
        /// </summary>
        /// <param name="folderName">Folder name, relative to service .EXE location</param>
        public void loadPlugins(string folderName)
        {
            DMUtils.DebugLog("Loading plug-ins");
            this.plugins = new List<PluginLoader>();
            this.pluginRecords = new List<PluginDomainRecord>();

            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            if (!Directory.Exists(path + "\\"+folderName)) return;
            string[] dirs = Directory.GetDirectories(path+"\\"+folderName);

            for (int i = 0; i < dirs.Length; i++)
            {
                string[] pluginFiles = Directory.GetFiles(dirs[i], "*.dll");
                for (var j = 0; j < pluginFiles.Length; j++)
                {
                    loadPlugin(pluginFiles[j], dirs[i]);
                }
            }
            if (plugins.Count == 0)
            {
                DMUtils.DebugLog("No plugin loaded");
            }
        }
        /// <summary>
        /// Unload plugin loaded from specific file
        /// </summary>
        /// <param name="fileName">
        /// Dll file name
        /// </param>
        public void unloadPlugin(string fileName)
        {
            for (int i = 0; i < pluginRecords.Count; i++)
            {
                if (pluginRecords[i].name == fileName)
                {
                    PluginDomainRecord rec = pluginRecords[i];
                    pluginRecords.Remove(rec);
                    plugins.Remove(rec.loader);
                    AppDomain.Unload(rec.domain);
                    rec = null;
                    return;
                }
            }
        }

        /// <summary>
        /// Returns true if plugin from given file already loaded
        /// </summary>
        /// <param name="fileName">
        /// Plugin file name
        /// </param>
        /// <returns>True if plugin already loaded</returns>
        public bool isPluginRegistered(string fileName)
        {
            for (int i = 0; i < pluginRecords.Count; i++)
            {
                if (pluginRecords[i].name == fileName)
                    return true;
            }
            return false;
        }

        
        /// <summary>
        /// Loads plugin from given dll file and working directory
        /// </summary>
        /// <param name="fileName">dll file name</param>
        /// <param name="dir">working directory where plugin is located</param>
        public void loadPlugin(string fileName, string dir)
		{

            
            if (isPluginRegistered(fileName)) return;
            DMUtils.DebugLog("Checking DLL file " + fileName);
			

       
			// load the dll
            PluginDomainRecord reg=null;
			try
			{
				// load it


                if (manager==null)
                manager=new AssemblyReflectionManager();
                
                string domainName=fileName;
                bool success = manager.LoadAssembly(fileName, domainName);

           
                if (!success)
                    return;

                success = false;
                var results = manager.Reflect(fileName, (a) =>
                {                
                    var names = new List<string>();
                    var types = a.GetTypes();
                    foreach (var t in types)
                    {                        
                        names.Add(t.Name);
                    }
                    return names;
                });


                
                foreach (string name in results)
                {
                    if (name == "FASPlugin")
                        success = true;
                }

                

                manager.UnloadAssembly(fileName);

                if (!success)
                {
                    DMUtils.DebugLog("Not validated for FASPlugin entry:" + fileName);
                    return;
                }

                DMUtils.DebugLog("VALIDATING PLUGIN");

                AppDomainSetup setup = new AppDomainSetup();
                setup.ApplicationName = Path.GetFileNameWithoutExtension(fileName);
                setup.ApplicationBase = Path.GetDirectoryName(fileName);
                
                setup.CachePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AddInsCache");
                setup.ShadowCopyDirectories = Path.GetDirectoryName(fileName);
                setup.ShadowCopyFiles = "true";

                AppDomain _appDomain = AppDomain.CreateDomain(Path.GetFileNameWithoutExtension(fileName));

                string baseDir = _appDomain.BaseDirectory;
                //System.Reflection.Assembly ass = null;
                //byte[] assemblyBuffer = File.ReadAllBytes(fileName);

                //AssemblyName assemblyName=AssemblyName.GetAssemblyName(fileName);

                //Assembly a1 = _appDomain.Load(assemblyName);

                _appDomain.Load(typeof(PluginLoader).Assembly.FullName);

                PluginLoader loader = (PluginLoader)Activator.CreateInstance(
                        _appDomain,
                        typeof(PluginLoader).Assembly.FullName,
                        typeof(PluginLoader).FullName,
                        false,
                        BindingFlags.Public | BindingFlags.Instance,
                        null,
                        null,
                        null,
                        null).Unwrap();

                loader.LoadPlugins(fileName);

                if (!loader.hasPlugin())
                {
                    AppDomain.Unload(_appDomain);
                    return;

                }
                
                //FileInfo[] infos = loader.getFiles();
                //name = infos[0].FullName;
               // AppDomainSetup setup = AppDomain.CurrentDomain.SetupInformation;
                //setup.ApplicationBase = dir;

              //  Evidence adevidence = AppDomain.CurrentDomain.Evidence;

//                AppDomain appDomain;
  //              appDomain = AppDomain.CreateDomain(fileName, adevidence,dir,dir,true);

                reg = new PluginDomainRecord();

                reg.domain = _appDomain;
                reg.name = fileName;
                
                //loader.setPluginHost(this);
                reg.loader = loader;
                pluginRecords.Add(reg);
                this.plugins.Add(loader);
                //AssemblyName assemblyName=AssemblyName.GetAssemblyName(fileName);
               //byte[] raw=File.ReadAllBytes(fileName);
                //ass = appDomain.Load(raw);
                //ass = Assembly.LoadFrom(fileName);
				/*if (success)
				{
                    ObjType = ass.GetType(arg + ".FASPlugin");

				}   
                else
                {
                    string[] erParams = { "message", "Plugin loading failed "+fileName };
                    throw new FASException("Plugin loadin failed", true, 1,
                        Utils.CreateErrorParamsString(null, erParams), EventType.Error, null, true);
                }*/
                
			}
            catch (Exception ex)
			{
                DMUtils.DebugLog("Error: LOADING_PLUGIN "+ex.Message);
                new Exception(ex.Message, ex);
			}
            DMUtils.DebugLog("Loading plugin success: " + fileName);
		}


    }
}


