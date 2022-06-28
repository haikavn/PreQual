using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Settings
{
    public class SystemSettingsModel
    {
        [Required(ErrorMessage = "Posting details are required")]
        public PostingDetailsModel postingDetails { get; set; } = new PostingDetailsModel();

        [Required(ErrorMessage = "SMTP setting is required")]
        public SettingSmtpModel settingsSmtpModel { get; set; } = new SettingSmtpModel();

        [Range(20, 180,
        ErrorMessage = "Login Session must be between {1} and {2}.")]
        public int loginSession { get; set; } = 10;
    }
}