using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Adrack.WebApi.Models.Settings
{
    public class SettingSmtpModel : BaseModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "SMTP display name is required")]
        public string SmtpDisplayName { get; set; }
        [Required(ErrorMessage = "SMTP email is required")]
        public string SmtpEmail { get; set; }
        [Required(ErrorMessage = "SMTP host is required")]
        public string SmtpHost { get; set; }
        public string SmtpHostDescription { get; set; }
        [Required(ErrorMessage = "SMTP post is required")]
        public int SmtpPort { get; set; }
        public string PortDescription { get; set; }
        [Required(ErrorMessage = "SMTP user name is required")]
        public string SmtpUsername { get; set; }
        [Required(ErrorMessage = "SMTP password is required")]
        public string SmtpPassword { get; set; }
        public bool SmtpEnableSsl { get; set; } = true;
        public string SmtpEnableSslDescription { get; set; }
        public bool SmtpUseDefaultCredentials { get; set; } = true;
        public string SmtpUseDefaultCredentialsDescription { get; set; }
       
       
        
        
    }
}