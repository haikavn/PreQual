using System.Collections.Generic;
using System.Web.Mvc;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.Settings
{
    public class SettingTimeZoneInsertModel : BaseModel, IBaseInModel
    {
        public string SelectedTimeZone { get; set; }
    }
}