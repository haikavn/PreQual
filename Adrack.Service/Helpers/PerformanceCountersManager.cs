using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Service.Helpers
{
    public class PermoformanceCountersManager
    {
        PerformanceCounter[] PerfCounters = new PerformanceCounter[10];

        public enum ADO_Net_Performance_Counters
        {
            NumberOfActiveConnectionPools,
            NumberOfReclaimedConnections,
            HardConnectsPerSecond,
            HardDisconnectsPerSecond,
            NumberOfActiveConnectionPoolGroups,
            NumberOfInactiveConnectionPoolGroups,
            NumberOfInactiveConnectionPools,
            NumberOfNonPooledConnections,
            NumberOfPooledConnections,
            NumberOfStasisConnections
            // The following performance counters are more expensive to track.
            // Enable ConnectionPoolPerformanceCounterDetail in your config file.
            //     SoftConnectsPerSecond
            //     SoftDisconnectsPerSecond
            //     NumberOfActiveConnections
            //     NumberOfFreeConnections
        }

        public void SetUpPerformanceCounters()
        {
            this.PerfCounters = new PerformanceCounter[10];
            string instanceName = GetInstanceName();
            Type apc = typeof(ADO_Net_Performance_Counters);
            int i = 0;
            foreach (string s in Enum.GetNames(apc))
            {
                this.PerfCounters[i] = new PerformanceCounter();
                this.PerfCounters[i].CategoryName = ".NET Data Provider for SqlServer";
                this.PerfCounters[i].CounterName = s;
                this.PerfCounters[i].InstanceName = instanceName;
                i++;
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int GetCurrentProcessId();

        public string GetInstanceName()
        {
            //This works for Winforms apps.
            string instanceName =
                System.Reflection.Assembly.GetEntryAssembly()?.GetName()?.Name;

            // Must replace special characters like (, ), #, /, \\
            string instanceName2 =
                AppDomain.CurrentDomain.FriendlyName.ToString().Replace('(', '[')
                .Replace(')', ']').Replace('#', '_').Replace('/', '_').Replace('\\', '_');

            // For ASP.NET applications your instanceName will be your CurrentDomain's
            // FriendlyName. Replace the line above that sets the instanceName with this:
            // instanceName = AppDomain.CurrentDomain.FriendlyName.ToString().Replace('(','[')
            // .Replace(')',']').Replace('#','_').Replace('/','_').Replace('\\','_');

            string pid = GetCurrentProcessId().ToString();
            instanceName = instanceName2 + "[" + pid + "]";
            return instanceName;
        }

        public List<string> GetPerformanceCounters()
        {
            List<string> list = new List<string>();
            foreach (PerformanceCounter p in this.PerfCounters)
            {
                list.Add(string.Format("{0} = {1}", p.CounterName, p.NextValue()));
            }
            return list;
        }
    }

}
