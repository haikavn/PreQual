using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.Settings
{
    public class EmailTemplateModel : BaseIdentifiedItem, IBaseInModel
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool Active { get; internal set; }
        public bool IsAuthorized { get; internal set; }
        public string SmtpAccountId { get; set; }
        public string AttachmentId { get; set; }
        public string Bcc { get; set; }
    }
}