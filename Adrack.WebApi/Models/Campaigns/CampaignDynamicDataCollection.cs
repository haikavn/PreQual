using Newtonsoft.Json;

namespace Adrack.WebApi.Models.Campaigns
{
    public class CampaignDynamicDataCollection
    {
        #region fields

        private readonly dynamic _loadedData;

        #endregion

        #region constructors

        public CampaignDynamicDataCollection(string json)
        {
            _loadedData = JsonConvert.DeserializeObject(json);
        }

        #endregion

        #region properties and indexers

        public CampaignDynamicDataItem this[int index] => new CampaignDynamicDataItem(_loadedData[index]);
        public int Count => _loadedData.Count;

        #endregion
    }
}