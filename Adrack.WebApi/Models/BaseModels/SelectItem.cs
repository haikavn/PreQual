using Adrack.WebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.BaseModels
{
    public class SelectItem : BaseIdentifiedItem, ISelectedItem
    {
        public string Text { get; internal set; }
        public string Value { get; internal set; }
        public bool IsSelected { get; internal set; }
    }
}