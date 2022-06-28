using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.WebApi.Models.Settings
{
    public class SettingTimeZoneModel : BaseModel
    {
        public List<SelectListItem> TimeZones { get; internal set; }
    }
}