using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.Interfaces;
using Microsoft.Ajax.Utilities;

namespace Adrack.WebApi.Models.Lead
{
    public class AffiliateExtendedModel : IBaseInModel
    {
        public AffiliateExtendedModel()
        {
                ActionType = string.Empty;
                NotesJsonString = string.Empty;
        }
        public AffiliateModel Affiliate { get; set; }
        public string ActionType { get; internal set; }
        public string NotesJsonString { get; internal set; }
    }


}