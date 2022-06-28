using System.Collections.Generic;
using Adrack.WebApi.Infrastructure.Web.Helpers;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Models.Settings
{
    public class SettingEmailTemplatesModel : BasePagedModel
    {
        #region constructors

        public SettingEmailTemplatesModel()
        {
            this.SetInstanceValues();

            EmailTemplates = new List<EmailTemplateModel>();
        }

        #endregion

        #region fields

        public List<EmailTemplateModel> EmailTemplates { get; internal set; }
        
        #endregion
    }
}