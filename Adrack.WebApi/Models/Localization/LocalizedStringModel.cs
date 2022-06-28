using Adrack.Core.Domain.Localization;
using Adrack.WebApi.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Localization
{
    public class LocalizedStringModel : BaseIdentifiedItem
    {
        public long LanguageId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public static explicit operator LocalizedStringModel(LocalizedString localizedString)
        {
            return new LocalizedStringModel()
            {
               Id = localizedString.Id,
               Key = localizedString.Key,
               Value = localizedString.Value,
               LanguageId = localizedString.LanguageId
            };
        }
    }
}