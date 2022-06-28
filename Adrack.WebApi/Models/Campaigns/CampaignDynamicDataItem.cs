using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Campaigns
{
    public class CampaignDynamicDataItem
    {
        #region fields

        private readonly dynamic _loadedData;

        #endregion

        #region constructors

        public CampaignDynamicDataItem(dynamic data)
        {
            _loadedData = data;
            Id = int.Parse(IdString);
            string[] validatorStr = ValidatorString.Split(';');

            short.TryParse(validatorStr[0], out var validatorResult);
            Validator = validatorResult;

            if (validatorResult == 1)
            {
                short.TryParse(validatorStr[1], out var minLengthResult);
                MinLength = minLengthResult;

                short.TryParse(validatorStr[2], out var maxLengthResult);
                MaxLength = maxLengthResult;
            }

            ValidatorValue = string
                .Join(";", validatorStr, 1, validatorStr.Length - 1);

            bool.TryParse(Required, out var requiredResult);
            IsRequired = requiredResult;

            bool.TryParse(IsHashString, out var isHashResult);
            IsHash = isHashResult;

            bool.TryParse(IsHiddenString, out var isHiddenResult);
            IsHidden = isHiddenResult;

            bool.TryParse(IsFilterableString, out var isFilterableResult);
            IsFilterable = isFilterableResult;
        }

        #endregion

        #region properties and indexers

        public long Id { get; set; }
        public bool IsFilterable { get; set; }
        public bool IsHash { get; set; }
        public bool IsHidden { get; set; }
        public bool IsRequired { get; }
        public short MaxLength { get; }
        public short MinLength { get; }
        public string ValidatorValue { get; set; }
        public short Validator { get; set; }

        public string TemplateField => _loadedData[1].ToString();
        public string DatabaseField => _loadedData[2].ToString();
        public string Comments => _loadedData[4].ToString();
        public string Possible => _loadedData[5].ToString();
        public string Parent => _loadedData[10].ToString();

        private string IdString => _loadedData[0].ToString();
        private string ValidatorString => _loadedData[3].ToString();
        private string Required => _loadedData[6].ToString();
        private string IsHashString => _loadedData[7].ToString();
        private string IsHiddenString => _loadedData[8].ToString();
        private string IsFilterableString => _loadedData[9].ToString();

        public int Count => _loadedData.Count;

        #endregion
    }
}