using Adrack.WebApi.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.Lead
{
    public class BlackListModel : IBaseOutModel
    {
        public long BlackListTypeId { get; internal set; }
        public short BlackListType { get; internal set; }
        public string Name { get; internal set; }
        public long ParentId { get; internal set; }
        public List<SelectItem> BlackListNames { get; internal set; }
        public List<SelectItem> Conditions { get; internal set; }
    }
}