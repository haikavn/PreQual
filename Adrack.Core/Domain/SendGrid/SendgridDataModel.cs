using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.SendGrid
{
    public class SendgridBaseDataModel
    {
        [JsonProperty("loginurl")]
        public string LoginUrl { get; set; }
        [JsonProperty("activationcode")]
        public string ActivationCode { get; set; }
        [JsonProperty("deactivationurl")]
        public string DeActivationUrl { get; set; }
        [JsonProperty("activationurl")]
        public string ActivationUrl { get; set; }
        [JsonProperty("forgotpasswordurl")]
        public string ForgotPasswordUrl { get; set; }
        [JsonProperty("applicationurl")]
        public string Link { get; set; }
        [JsonProperty("customername")]
        public string CustomerName { get; set; }
        [JsonProperty("expirationdate")]
        public string ExpirationDate { get; set; }
        [JsonProperty("tableinfo")]
        public string TableInfo { get; set; }
        [JsonProperty("applicationemail")]
        public string Email { get; set; }
        [JsonProperty("subscribe")]
        public bool Subscribe { get; set; }
        [JsonProperty("applicationname")]
        public string ApplicationName { get; set; }
        [JsonProperty("fullname")]
        public string FullName { get; set; }
        [JsonProperty("useremail")]
        public string UserEmail { get; set; }
        [JsonProperty("userpassword")]
        public string UserPassword { get; set; }
        [JsonProperty("useractivationurl")]
        public string UserActivationUrl { get; set; }

        [JsonProperty("buyername")]
        public string BuyerName { get; set; }
        [JsonProperty("buyerchannelname")]
        public string BuyerChannelName { get; set; }
        //[JsonProperty("buyerchannelname")]
        //public string BuyerChannelName { get; set; }

    }
}
