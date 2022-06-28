// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="SharedData.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure;
using Adrack.Service.Configuration;
using Adrack.Service.Lead;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Helpers
{
    /// <summary>
    /// Class SharedData.
    /// </summary>
    public static class SharedData
    {
        /// <summary>
        /// The built in user type identifier
        /// </summary>
        private static long builtInUserTypeId = 0;

        /// <summary>
        /// The netowrk user type identifier
        /// </summary>
        private static long netowrkUserTypeId = 0;

        /// <summary>
        /// The buyer user type identifier
        /// </summary>
        private static long buyerUserTypeId = 0;

        /// <summary>
        /// The store user type identifier
        /// </summary>
        private static long storeUserTypeId = 0;

        /// <summary>
        /// The affiliate user type identifier
        /// </summary>
        private static long affiliateUserTypeId = 0;

        /// <summary>
        /// The clear cache processng
        /// </summary>
        private static bool clearCacheProcessng = false;

        /// <summary>
        /// Gets the user type identifier.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="userTypeId">The user type identifier.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Int64.</returns>
        private static long GetUserTypeId(string key, long userTypeId, long defaultValue)
        {
            if (userTypeId == 0)
            {
                var settingService = AppEngineContext.Current.Resolve<ISettingService>();
                Setting setting = settingService.GetSetting(key);
                if (setting != null)
                {
                    long id;
                    if (long.TryParse(setting.Value, out id))
                        userTypeId = id;
                }
                else
                    userTypeId = defaultValue;
            }

            return userTypeId;
        }

        public static void ResetBuyerChannelLeadsCount(long pingTreeId)
        {
            if (SharedData.BuyerChannelLeadsList == null)
                SharedData.BuyerChannelLeadsList = new ConcurrentDictionary<long, ConcurrentDictionary<long, int>>();

            if (BuyerChannelLeadsList.ContainsKey(pingTreeId))
                BuyerChannelLeadsList[pingTreeId].Clear();
        }

        public static void ResetBuyerChannelLeadsCount()
        {
            var pingTreeId = GetCurrentPingTreeId();
            if (pingTreeId != 0)
            {
                ResetBuyerChannelLeadsCount(pingTreeId);
            }
        }

        public static void ClearBuyerChannelLeadsCount()
        {
            if (SharedData.BuyerChannelLeadsList == null)
                SharedData.BuyerChannelLeadsList = new ConcurrentDictionary<long, ConcurrentDictionary<long, int>>();

            SharedData.BuyerChannelLeadsList.Clear();
        }
              
        public static void ResetBuyerChannelLeadsCount(long pingTreeId, List<PingTreeItem> pingTreeItems, int cycle = 0)
        {
            if (SharedData.BuyerChannelLeadsList == null)
                SharedData.BuyerChannelLeadsList = new ConcurrentDictionary<long, ConcurrentDictionary<long, int>>();

            if (!BuyerChannelLeadsList.ContainsKey(pingTreeId))
                BuyerChannelLeadsList[pingTreeId] = new ConcurrentDictionary<long, int>();

            if (SharedData.BuyerChannelLeadsList[pingTreeId].Count == 0)
            {                
                foreach (var pingTreeItem in pingTreeItems)
                {
                    //var counterValue = (double)(bc.LeadAcceptRate.HasValue ? (bc.LeadAcceptRate.Value == 0 ? -1 : bc.LeadAcceptRate.Value) : 1);

                    int counterValue = (int)(((double)pingTreeItem.Percent / (double)100) * (double)100);

                    //If you want to treat 0 value as 100 then please uncomment the following line
                    // if (counterValue < 0) counterValue=cycle;

                    /*if (counterValue != -1)
                    {
                        if (cycle==20)
                            counterValue = Math.Round(counterValue / 5.0);
                        if (cycle == 10)
                            counterValue = Math.Round(counterValue / 10.0);
                        if (cycle == 5)
                            counterValue = Math.Round(counterValue / 20.0);          
                    }*/

                    //Keith, uncomment this line if you want to exclude from the ping tree, all channels with 0 value
                    // if (counterValue < 0) continue;

                    if (counterValue <= 0)
                        counterValue = 1;
                    
                    SharedData.BuyerChannelLeadsList[pingTreeId].TryAdd(pingTreeItem.BuyerChannelId, (int)counterValue);
                }
            }
        }

        public static void ResetBuyerChannelLeadsCount(List<PingTreeItem> pingTreeItems)
        {
            var pingTreeId = GetCurrentPingTreeId();
            if (pingTreeId != 0)
                ResetBuyerChannelLeadsCount(pingTreeId, pingTreeItems);
        }

 
        public static void DecrementBuyerChannelLeadsCount(long pingTreeId, PingTreeItem pingTreeItem)
        {
            if (!SharedData.BuyerChannelLeadsList[pingTreeId].ContainsKey(pingTreeItem.BuyerChannelId))
                return;
                //SharedData.BuyerChannelLeadsList[campaignId].TryAdd(buyerChannel.Id, (int)buyerChannel.LeadAcceptRate);

            int value = 0;
            if (SharedData.BuyerChannelLeadsList[pingTreeId].TryGetValue(pingTreeItem.BuyerChannelId, out value))
            {
                if (SharedData.BuyerChannelLeadsList[pingTreeId].TryUpdate(pingTreeItem.BuyerChannelId, value - 1, value))
                {
                    value--;
                    DecrementPingTreeLeadsCount(pingTreeId);
                }
            }

            if (value <= 0)
            {
                SharedData.BuyerChannelLeadsList[pingTreeId].TryRemove(pingTreeItem.BuyerChannelId, out value);
            }
        }

        public static void DecrementBuyerChannelLeadsCount(PingTreeItem pingTreeItem)
        {
            var pingTreeId = GetCurrentPingTreeId();
            if (pingTreeId != 0)
                DecrementBuyerChannelLeadsCount(pingTreeId, pingTreeItem);
        }

        public static bool CheckBuyerChannelLeadsCount(long pingTreeId, PingTreeItem pingTreeItem)
        {
            if (!SharedData.BuyerChannelLeadsList[pingTreeId].ContainsKey(pingTreeItem.BuyerChannelId))
                return false;

            int value = 0;
            if (SharedData.BuyerChannelLeadsList[pingTreeId].TryGetValue(pingTreeItem.BuyerChannelId, out value))
            {
                if (value > 0 || value < 0) //include -1 case
                    return true;
            }
            else return true;

            return false;
        }

        public static bool CheckBuyerChannelLeadsCount(PingTreeItem pingTreeItem)
        {
            var pingTreeId = GetCurrentPingTreeId();
            if (pingTreeId == 0) return false;
            return CheckBuyerChannelLeadsCount(pingTreeId, pingTreeItem);
        }

        //Ping tee

        public static void ResetPingTreeLeadsCount(long pingTreeId)
        {
            if (SharedData.PingTreeCounts == null)
                SharedData.PingTreeCounts = new ConcurrentDictionary<long, int>();

            if (PingTreeCounts.ContainsKey(pingTreeId))
                PingTreeCounts[pingTreeId] = 0;
        }

        public static void ResetPingTreeLeadsCount()
        {
            if (SharedData.PingTreeCounts == null)
                SharedData.PingTreeCounts = new ConcurrentDictionary<long, int>();

            SharedData.PingTreeCounts.Clear();
        }

        public static void ResetPingTreeLeadsCount(long pingTreeId, int count)
        {
            if (SharedData.PingTreeCounts == null)
                SharedData.PingTreeCounts = new ConcurrentDictionary<long, int>();

            PingTreeCounts.TryAdd(pingTreeId, count);
        }

        public static void ResetPingTreeLeadsCount(int count)
        {
            var pingTreeId = GetCurrentPingTreeId();
            if (pingTreeId != 0)
            {
                ResetPingTreeLeadsCount(pingTreeId, count);
            }
        }

        public static void ResetPingTreeLeadsCount(IList<PingTree> pingTrees)
        {
            foreach(var pingTree in pingTrees)
            {
                ResetPingTreeLeadsCount(pingTree.Id, pingTree.Quantity);
            }
        }

        public static void DecrementPingTreeLeadsCount(long pingTreeId)
        {
            if (!SharedData.PingTreeCounts.ContainsKey(pingTreeId))
                return;
            //SharedData.BuyerChannelLeadsList[campaignId].TryAdd(buyerChannel.Id, (int)buyerChannel.LeadAcceptRate);

            int value = 0;

            if (SharedData.PingTreeCounts.TryGetValue(pingTreeId, out value))
            {
                if (SharedData.PingTreeCounts.TryUpdate(pingTreeId, value - 1, value))
                    value--;
            }

            if (value <= 0)
            {
                SharedData.PingTreeCounts.TryRemove(pingTreeId, out value);
            }
        }

        public static void DecrementPingTreeLeadsCount()
        {
            var pingTreeId = GetCurrentPingTreeId();
            if (pingTreeId != 0)
            {
                DecrementPingTreeLeadsCount(pingTreeId);
            }
        }

        public static bool CheckPingTreeLeadsCount(long pingTreeId)
        {
            if (!SharedData.PingTreeCounts.ContainsKey(pingTreeId))
                return false;

            int value = 0;
            if (SharedData.PingTreeCounts.TryGetValue(pingTreeId, out value))
            {
                if (value > 0 || value < 0) //include -1 case
                    return true;
            }
            else return true;

            return false;
        }

        public static bool CheckPingTreeLeadsCount()
        {
            long curPingTreeId = GetCurrentPingTreeId();
            if (curPingTreeId == 0) return false;
            return CheckPingTreeLeadsCount(curPingTreeId);
        }

        public static long GetCurrentPingTreeId()
        {
            var last = SharedData.PingTreeCounts.Where(x => x.Value > 0).Select(x => x.Key).FirstOrDefault();
            return last;
        }



        /// <summary>
        /// Gets the built in user type identifier.
        /// </summary>
        /// <value>The built in user type identifier.</value>
        public static UserTypes BuiltInUserTypeId
        {
            get
            {
                return UserTypes.Super;
            }
        }

        /// <summary>
        /// Gets the netowrk user type identifier.
        /// </summary>
        /// <value>The netowrk user type identifier.</value>
        public static UserTypes NetowrkUserTypeId
        {
            get
            {
                return UserTypes.Network;
            }
        }

        /// <summary>
        /// Gets the buyer user type identifier.
        /// </summary>
        /// <value>The buyer user type identifier.</value>
        public static UserTypes BuyerUserTypeId
        {
            get
            {
                return UserTypes.Buyer;
            }
        }

        /// <summary>
        /// Gets the affiliate user type identifier.
        /// </summary>
        /// <value>The affiliate user type identifier.</value>
        public static UserTypes AffiliateUserTypeId
        {
            get
            {
                return UserTypes.Affiliate;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [clear cache processng].
        /// </summary>
        /// <value><c>true</c> if [clear cache processng]; otherwise, <c>false</c>.</value>
        public static bool ClearCacheProcessng
        {
            get
            {
                return clearCacheProcessng;
            }
        }

        public static ConcurrentDictionary<long, ConcurrentDictionary<long, int>> BuyerChannelLeadsList { get; set; } = new ConcurrentDictionary<long, ConcurrentDictionary<long, int>>();

        public static ConcurrentDictionary<long, int> PingTreeCounts { get; set; } = new ConcurrentDictionary<long, int>();


        public static ConcurrentDictionary<string, DateTime> LoginTokenExpirations { get; set; } = new ConcurrentDictionary<string, DateTime>();

        // public static ConcurrentDictionary<long, bool> BuyerDoNotPresent { get; set; } = new ConcurrentDictionary<long, bool>();
    }
}