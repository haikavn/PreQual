using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Vertical
{
    public class VerticalListModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public string IconName { get; set; }

        public IList<IdNameModel> Campaigns { get; set; }
    }
}