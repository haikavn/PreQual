using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.CustomBlackList
{
    public class CustomBlackListValueModel
    {
        public long ChannelId { get; set; }
        public short ChannelType { get; set; }
        public List<string> Values { get; set; }
        public long TemplateFieldId { get; set; }
    }
}