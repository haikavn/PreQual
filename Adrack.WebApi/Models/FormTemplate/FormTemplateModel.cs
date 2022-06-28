using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.FormTemplate;
using FormBuilder = Adrack.Core.Domain.Lead;

namespace Adrack.WebApi.Models.Common
{
    public class AffSubId
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class FormTemplateModelProperty
    {
        public string FormStyle = "";
        public string FormWidth = "";
        public string FormHeight = "";
        public string FormClass = "";
        public string FormBackground = "";
        public bool ShowNextPrevButton = true;
    }

    public class FormTemplateReferralProperties
    {
        public string ChannelId = "";
        public string Password = "";
        public List<AffSubId> AffSubIds = new List<AffSubId>();
        public string ReferringUrl = "";
        public long MinPrice = 0;
        public string SubmitReturnURL="/";
    }

    public class FormTemplatePageProperty
    {
        public long Step;
        public string XMLSectionName;
        public string TitleText;
        public string DescriptionText;
        public string TitleStyle;
        public string DescriptionStyle;
        public string TitleClass;
        public string DescriptionClass;
        public bool ShowNextPrevButton = true;
    }

    public class FormProperties
    {
        public FormTemplateReferralProperties ReferralProperties = new FormTemplateReferralProperties();
        public FormTemplateModelProperty LayerProperties = new FormTemplateModelProperty();
        public List<FormTemplatePageProperty> PageProperties = new List<FormTemplatePageProperty>();
    }
    public class FormTemplateModel
    {
        public Adrack.Core.Domain.Lead.FormTemplate FormTemplate { get; set; }
        public List<FormTemplateItemModel> FormTemplateItems { get; set; } = new List<FormTemplateItemModel>();

        public FormProperties Properties = new FormProperties();
    }
}