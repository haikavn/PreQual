using Adrack.WebApi.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.FormBuilder
{
    public class FormBuilderGenerationTemplate
    {
        public string Name;
        public string AffiliateName { get; set; }
        public bool IsWizard;

        public string FormStyle = "";
        public string FormWidth = "";
        public string FormHeight = "";
        public string FormClass = "";
        public string FormBackground = "";
        public bool ShowNextPrevButton = true;
        public string SubmitReturnURL = "/";
        public string ChannelId = "";
        public string Password = "";
        public string ReferringUrl = "";
        public List<AffSubId> AffSubIds = new List<AffSubId>();
        public long MinPrice;

        public List<FormTemplatePageProperty> PageProperties = new List<FormTemplatePageProperty>();

        //public List<FormBuilderGenerationPage> pages = new List<FormBuilderGenerationPage>();
        public List<FormBuilderFieldGenerationOptions> Fields=new List<FormBuilderFieldGenerationOptions>();

    }
}