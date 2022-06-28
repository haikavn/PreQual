using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Service.Configuration;
using Adrack.Service.Lead;
using Adrack.WebApi.Extensions;
using Adrack.WebApi.Models.Common;
using Adrack.WebApi.Models.New.Lead;
using Newtonsoft.Json;
using Adrack.WebApi.Models.FormTemplate;
using Adrack.WebApi.FormBuilder;
using iTextSharp.text.pdf.qrcode;
using Adrack.Core.Domain.Message;
using Adrack.Web.Framework.Security;
using System.Web;
using System.IO;
using Adrack.Service.Common;
using Adrack.Service.Content;
using iTextSharp.text;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/formBuilder")]
    public class FormBuilderController : BaseApiController
    {
        private readonly ISettingService _settingService;
        private readonly IAppContext _appContext;
        private readonly ILeadMainService _leadMainService;
        private readonly IAffiliateService _affiliateService;
        private readonly IAffiliateChannelService _affiliateChannelService;
        private readonly IBuyerService _buyerService;
        private readonly IBuyerChannelService _buyerChannelService;
        private readonly ICampaignService _campaignService;
        private readonly IAffiliateResponseService _affiliateResponseService;
        private readonly ILeadMainResponseService _leadMainResponseService;
        private readonly ILeadContentDublicateService _leadContentDuplicateService;
        private readonly IFormTemplateService _formTemplateService;
        private readonly IAffiliateChannelTemplateService _affiliateChannelTemplateService;

        private readonly IStorageService _storageService;
        private string UploadAffiliateChannelFieldIconFolderUrl =>
            $"{Request.RequestUri.GetLeftPart(UriPartial.Authority)}/Content/Uploads/Icons/AffiliateChannelField/";

        protected string blobPath = "uploads";


        public FormBuilderController(
            ISettingService settingService,
            IAppContext appContext,
            ILeadMainService leadMainService,
            IAffiliateService affiliateService,
            IAffiliateChannelService affiliateChannelService,
            IBuyerService buyerService,
            IBuyerChannelService buyerChannelService,
            ICampaignService campaignService,
            IAffiliateResponseService affiliateResponseService,
            ILeadMainResponseService leadMainResponseService,
            ILeadContentDublicateService leadContentDuplicateService,
            IFormTemplateService formTemplateService,
            IAffiliateChannelTemplateService affiliateChannelTemplateService,
            IStorageService storageService)
        {
            _settingService = settingService;
            _appContext = appContext;
            _leadMainService = leadMainService;
            _affiliateService = affiliateService;
            _affiliateChannelService = affiliateChannelService;
            _buyerService = buyerService;
            _buyerChannelService = buyerChannelService;
            _campaignService = campaignService;
            _affiliateResponseService = affiliateResponseService;
            _leadMainResponseService = leadMainResponseService;
            _leadContentDuplicateService = leadContentDuplicateService;
            _formTemplateService = formTemplateService;
            _affiliateChannelTemplateService = affiliateChannelTemplateService;
            _storageService = storageService;
        }

        [HttpPost]
        [Route("uploadAffiliateChannelFieldIcon/{affiliateChannelId}/{formId}/{affiliateChannelFieldId}")]
        public IHttpActionResult UploadAffiliateChannelFieldIcon(long affiliateChannelId, long formId, long affiliateChannelFieldId)
        {
            try
            {
                var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(affiliateChannelId);
                if (affiliateChannel == null)
                {
                    return HttpBadRequest("This AffiliateChannel does not exist");
                }

                var form = _formTemplateService.GetFormTemplateById(formId);
                if (form == null)
                {
                    return HttpBadRequest("This Form does not exist");
                }

                var affiliateChannelField = _affiliateChannelTemplateService.GetAffiliateChannelTemplateById(affiliateChannelFieldId);
                if (affiliateChannelField == null)
                {
                    return HttpBadRequest("This AffiliateChannelField does not exist");
                }

                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files["Icon"] != null)
                {
                    var file = httpRequest.Files.Get("Icon");
                    if (file != null)
                    {
                        var ext = Path.GetExtension(file.FileName);
                        var imageName = $"{affiliateChannelId}_{formId}_{affiliateChannelFieldId}{ext}";

                        return Ok(_storageService.Upload(blobPath, file.InputStream, file.ContentType, imageName).AbsoluteUri);

                        ext = Path.GetExtension(file.FileName);
                        imageName = $"{affiliateChannelId}_{formId}_{affiliateChannelFieldId}{ext}";
                        var relativePath = "~/Content/Uploads/Icons/AffiliateChannelField/";

                        Stream fs = file.InputStream;
                        BinaryReader br = new BinaryReader(fs);
                        byte[] bytes = br.ReadBytes((Int32) fs.Length);

                        var targetFolder = HttpContext.Current.Server.MapPath(relativePath);
                        var targetPath = Path.Combine(targetFolder, imageName);

                        /*
                        decimal size = Math.Round(((decimal)file.ContentLength / (decimal)1024), 2);
                        if (size > 1048576) //1MB
                        {
                            return HttpBadRequest("File size must not exceed 1MB.");
                        }
                        */

                        var validationResult = ValidationHelper.ValidateImage(bytes,
                            file.FileName.Split('.')[file.FileName.Split('.').Length - 1]
                            , new List<string> {"png", "jpg", "jpeg", "gif"}, 1024, 768, 1048576);

                        if (!validationResult.Item1)
                        {
                            return HttpBadRequest(validationResult.Item2);
                        }

                        file.SaveAs(targetPath);
                        file.InputStream.Position = 0;

                        return Ok($"{UploadAffiliateChannelFieldIconFolderUrl}{imageName}");

                    }
                    else
                    {
                        return HttpBadRequest("Icon does not exist");
                    }
                }
                else
                {
                    return HttpBadRequest("Icon does not exist");
                }
                
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("uploadFormItemIcon")]
        public IHttpActionResult UploadFormItemIcon()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files["Icon"] != null)
                {
                    var file = httpRequest.Files.Get("Icon");
                    if (file != null)
                    {
                        var ext = Path.GetExtension(file.FileName);
                        var fileName = Path.GetRandomFileName();
                        var imageName = $"{fileName}{ext}";

                        var uri = _storageService.Upload(blobPath, file.InputStream, file.ContentType, imageName);
                        return Ok(uri.AbsoluteUri);

                        var relativePath = "~/Content/Uploads/Icons/Form/Tmp/";

                        Stream fs = file.InputStream;
                        BinaryReader br = new BinaryReader(fs);
                        byte[] bytes = br.ReadBytes((Int32)fs.Length);

                        var targetFolder = HttpContext.Current.Server.MapPath(relativePath);
                        var targetPath = Path.Combine(targetFolder, imageName);

                        var validationResult = ValidationHelper.ValidateImage(bytes,
                            file.FileName.Split('.')[file.FileName.Split('.').Length - 1]
                            , new List<string> { "png", "jpg", "jpeg", "gif" }, 1024, 768, 1048576);

                        if (!validationResult.Item1)
                        {
                            return HttpBadRequest(validationResult.Item2);
                        }

                        file.SaveAs(targetPath);
                        file.InputStream.Position = 0;

                        return Ok($"{imageName}");
                    }
                    else
                    {
                        return HttpBadRequest("Icon does not exist");
                    }
                }
                else
                {
                    return HttpBadRequest("Icon does not exist");
                }

            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("saveFormTemplate")]
        public IHttpActionResult SaveFormTemplate([FromBody]FormTemplateModel formTemplateModel)
        {
            long result = 0;
            try
            {
                if (!string.IsNullOrWhiteSpace(formTemplateModel.FormTemplate.Name))
                {
                    formTemplateModel.FormTemplate.Created = DateTime.UtcNow;
                    formTemplateModel.FormTemplate.LastModified = DateTime.UtcNow;

                    if (formTemplateModel.Properties!=null)
                        formTemplateModel.FormTemplate.Properties = JsonConvert.SerializeObject(formTemplateModel.Properties);

                    var affiliateChannelId = _affiliateChannelService.GetAffiliateChannelById(formTemplateModel.FormTemplate.AffiliateChannelId);
                    if (affiliateChannelId == null)
                    {
                        return HttpBadRequest("This AffiliateChannelId does not exist");
                    }

                    result = _formTemplateService.InsertFormTemplate(formTemplateModel.FormTemplate);

                    if (formTemplateModel.FormTemplateItems.Any())
                    {
                        var relativePathTmp = "~/Content/Uploads/Icons/Form/Tmp/";
                        var targetFolderTmp = HttpContext.Current.Server.MapPath(relativePathTmp);

                        var relativePathOrg = "~/Content/Uploads/Icons/Form/FormItems/";
                        var targetFolderOrg = HttpContext.Current.Server.MapPath(relativePathOrg);

                        foreach (var item in formTemplateModel.FormTemplateItems)
                        {
                            var templateItem = MapFromModel(item);
                            templateItem.FormTemplateId = result;
                            templateItem.Created = DateTime.UtcNow;
                            item.Id = _formTemplateService.InsertFormTemplateItem(templateItem);

                            /*
                            var style = item.StylesJson.Where(x => x.Name.ToLower() == "imageurl").FirstOrDefault();

                            //move file
                            var iconNameTmp = item.TmpIconName;
                            if (string.IsNullOrEmpty(iconNameTmp))
                            {
                                if (item.StylesJson != null)
                                {
                                    if (style != null)
                                    {
                                        iconNameTmp = style.Value;
                                    }
                                }
                            }
                            */

                            /*
                            if (!string.IsNullOrEmpty(iconNameTmp))
                            {
                                try
                                {
                                    
                                    string contentType = "";
                                    iconNameTmp = Path.GetFileName(iconNameTmp);
                                    Stream stream = new MemoryStream();
                                    contentType = _storageService.DownloadFile(blobPath, iconNameTmp, stream);
                                    var extTmp = Path.GetExtension(iconNameTmp);
                                    var iconNameOrg = $"{formTemplateModel.FormTemplate.Id}-{item.Id}{extTmp}";
                                    var url = _storageService.Upload(blobPath, stream, contentType, iconNameOrg);
                                    //_storageService.DeleteFile(blobPath, iconNameTmp);
                                    
                                }
                                catch
                                {

                                }

                                //var targetPathTmp = Path.Combine(targetFolderTmp, iconNameTmp);

                                //var extTmp = Path.GetExtension(targetPathTmp);
                                //var iconNameOrg = $"{item.FormTemplateId}-{item.Id}{extTmp}";

                                //var targetPathOrg = Path.Combine(targetFolderOrg, iconNameOrg);

                                //if (File.Exists(targetPathTmp))
                                //{
                                //    File.Move(targetPathTmp, targetPathOrg);
                                //}
                            }
                            */
                        }

                    }
                }
                else if (string.IsNullOrEmpty(formTemplateModel.FormTemplate.Name))
                {
                    return HttpBadRequest("Form builder Name template can not be empty");
                }
                return Ok(formTemplateModel);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }



        [HttpPost]
        [Route("cloneFormTemplate/{currentFormTemplateId}/{formTemplateName}/{affiliateChannelId}")]
        public IHttpActionResult CloneFormTemplate(long currentFormTemplateId, string formTemplateName, long affiliateChannelId)
        {
            var cloneFormTemplate = new FormTemplate(); 
            try
            {
                var currentFormTemplate = _formTemplateService.GetFormTemplateById(currentFormTemplateId);
                if (currentFormTemplate == null)
                {
                    return HttpBadRequest("This FormTemplate does not exist");
                }

                if (!string.IsNullOrWhiteSpace(formTemplateName))
                {
                    var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(affiliateChannelId);
                    if (affiliateChannel == null) 
                        return HttpBadRequest("This AffiliateChannel does not exist");

                    cloneFormTemplate.Name = formTemplateName;
                    cloneFormTemplate.Created = DateTime.UtcNow;
                    cloneFormTemplate.LastModified = DateTime.UtcNow;
                    cloneFormTemplate.AffiliateChannelId = affiliateChannelId;
                    cloneFormTemplate.IntegrationType = currentFormTemplate.IntegrationType;
                    cloneFormTemplate.Submissions = currentFormTemplate.Submissions;
                    cloneFormTemplate.Type = currentFormTemplate.Type;
                    cloneFormTemplate.Properties = currentFormTemplate.Properties;

                    long cloneFormTemplateId = _formTemplateService.InsertFormTemplate(cloneFormTemplate);


                    var currentTemplateItems = _formTemplateService.GetFormTemplateItemsByTemplateId(currentFormTemplate.Id);
                    if (currentTemplateItems.Any())
                    {
                        var cloneAffiliateChannelTemplates = _affiliateChannelTemplateService.GetAllAffiliateChannelTemplatesByAffiliateChannelId(affiliateChannelId);

                        foreach (var currentItem in currentTemplateItems)
                        {
                            long? referringFieldId = null;
                            if (currentItem.ReferringFieldId != null)
                            {
                                AffiliateChannelTemplate currentAffiliateChannelTemplateItem = _affiliateChannelTemplateService.GetAffiliateChannelTemplateById((long)currentItem.ReferringFieldId);
                                if (currentAffiliateChannelTemplateItem != null)
                                {
                                    var cloneAffiliateChannelTemplateItem = cloneAffiliateChannelTemplates.FirstOrDefault(x => String.Equals(x.TemplateField, currentAffiliateChannelTemplateItem.TemplateField, StringComparison.CurrentCultureIgnoreCase));
                                    if(cloneAffiliateChannelTemplateItem != null)
                                        referringFieldId = cloneAffiliateChannelTemplateItem.Id;
                                }
                            }

                            FormTemplateItem cloneItem = new FormTemplateItem()
                            {
                                Id = 0,
                                FormTemplateId = cloneFormTemplateId,
                                Created = DateTime.UtcNow,
                                Column = currentItem.Column,
                                Name = currentItem.Name,
                                InlineList = currentItem.InlineList,
                                HelperText = currentItem.HelperText,
                                Height = currentItem.Height,
                                IsRequired = currentItem.IsRequired,
                                Label = currentItem.Label,
                                NeedsValidation = currentItem.NeedsValidation,
                                Options = currentItem.Options,
                                HelperStyle = currentItem.HelperStyle,
                                LabelStyle = currentItem.LabelStyle,
                                PlaceHolderStyle = currentItem.PlaceHolderStyle,
                                ParentId = currentItem.ParentId,
                                PlaceHolderText = currentItem.PlaceHolderText,
                                Type = currentItem.Type,
                                Step = currentItem.Step,
                                ReferringFieldId = referringFieldId,
                                ResponseFormat = currentItem.ResponseFormat,
                                StylesJson = currentItem.StylesJson,
                                ValidationRegex = currentItem.ValidationRegex,
                                Width = currentItem.Width,
                                Value = currentItem.Value,
                                
                            };

                            cloneItem.Id = _formTemplateService.InsertFormTemplateItem(cloneItem);


                            try
                            {
                                 if (!string.IsNullOrEmpty(cloneItem.StylesJson))
                                 {
                                     List<FormTemplateItemStyles> objStylesJson = JsonConvert.DeserializeObject<List<FormTemplateItemStyles>>(cloneItem.StylesJson);
                                     var style = objStylesJson.FirstOrDefault(x => x.Name.ToLower() == "imageurl");

                                     if (style != null)
                                     {
                                         var iconNameTmp = style.Value;

                                         if (!string.IsNullOrEmpty(iconNameTmp))
                                         {
                                             Stream stream = new MemoryStream();
                                             string currentIconNameTmp = Path.GetFileName(iconNameTmp);
                                             string contentType = _storageService.DownloadFile(blobPath, currentIconNameTmp, stream);

                                             string cloneIconNameTmp = cloneItem.Id + "-" + currentIconNameTmp;
                                             var tUrl = _storageService.Upload(blobPath, stream, contentType, cloneIconNameTmp);

                                             style.Value = tUrl.AbsoluteUri;
                                             var tFormTemplateItem = _formTemplateService.GetFormTemplateItemById(cloneItem.Id);
                                             tFormTemplateItem.StylesJson = JsonConvert.SerializeObject(objStylesJson);
                                             _formTemplateService.UpdateFormTemplateItem(tFormTemplateItem);
                                         }
                                     }
                                 }
                            }
                            catch (Exception ex)
                            {
                                return HttpBadRequest(ex.Message);
                            }
                        }
                    }
                }
                else 
                {
                    return HttpBadRequest("Form builder Name template can not be empty");
                }

                return Ok(cloneFormTemplate);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("updateFormTemplate")]
        public IHttpActionResult UpdateFormTemplate([FromBody] FormTemplateModel formTemplateModel)
        {
            try
            {
                var existed = _formTemplateService.GetFormTemplateById(formTemplateModel.FormTemplate.Id);
                if (existed == null)
                {
                    return HttpBadRequest("Provided form template doesn't exist");
                }

                //var affiliateChannelId = _affiliateChannelService.GetAffiliateChannelById(formTemplateModel.FormTemplate.AffiliateChannelId);
                //if (affiliateChannelId == null)
                //{
                //    return HttpBadRequest("This AffiliateChannelId does not exist");
                //}

                if (formTemplateModel.Properties != null)
                    existed.Properties = JsonConvert.SerializeObject(formTemplateModel.Properties);

                existed.Id = formTemplateModel.FormTemplate.Id;
                existed.Name = formTemplateModel.FormTemplate.Name;
                existed.IntegrationType = formTemplateModel.FormTemplate.IntegrationType;
                existed.Type = formTemplateModel.FormTemplate.Type;
                existed.Submissions = formTemplateModel.FormTemplate.Submissions;
                existed.LastModified = DateTime.UtcNow; ;

                _formTemplateService.UpdateFormTemplate(existed);

                //Template Items
                _formTemplateService.DeleteFormTemplateItems(existed);

                if (formTemplateModel.FormTemplateItems.Any())
                {
                    var relativePathTmp = "~/Content/Uploads/Icons/Form/Tmp/";
                    var targetFolderTmp = HttpContext.Current.Server.MapPath(relativePathTmp);

                    var relativePathOrg = "~/Content/Uploads/Icons/Form/FormItems/";
                    var targetFolderOrg = HttpContext.Current.Server.MapPath(relativePathOrg);

                    foreach (var item in formTemplateModel.FormTemplateItems)
                    {
                        var templateItem = MapFromModel(item);
                        templateItem.FormTemplateId = existed.Id;
                        templateItem.Created = DateTime.UtcNow;
                        item.Id = _formTemplateService.InsertFormTemplateItem(templateItem);

                        //move file
                        /*
                        var iconNameTmp = item.TmpIconName;
                        if (string.IsNullOrEmpty(iconNameTmp))
                        {
                            if (item.StylesJson != null)
                            {
                                var style = item.StylesJson.Where(x => x.Name.ToLower() == "imageurl").FirstOrDefault();
                                if (style != null)
                                    iconNameTmp = style.Value;
                            }
                        }

                        if (!string.IsNullOrEmpty(iconNameTmp))
                        {
                            try
                            {
                                
                                string contentType = "";
                                iconNameTmp = Path.GetFileName(iconNameTmp);
                                Stream stream = new MemoryStream();
                                contentType = _storageService.DownloadFile(blobPath, iconNameTmp, stream);
                                var extTmp = Path.GetExtension(iconNameTmp);
                                var iconNameOrg = $"{existed.Id}-{item.Id}{extTmp}";
                                _storageService.Upload(blobPath, stream, contentType, iconNameOrg);
                                //_storageService.DeleteFile(blobPath, iconNameTmp);
                                


                            }
                            catch
                            {

                            }

                            //var targetPathTmp = Path.Combine(targetFolderTmp, iconNameTmp);
                            //var targetPathOrg = Path.Combine(targetFolderOrg, iconNameOrg);

                            //if (File.Exists(targetPathTmp))
                            //{
                            //    File.Move(targetPathTmp, targetPathOrg);
                            //}
                        }
                        */
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }


        /*
        [HttpPost]
        [Route("updateFormTemplate")]
        public IHttpActionResult UpdateFormTemplate([FromBody]FormTemplate formTemplate)
        {
            try
            {
                var existed = _formTemplateService.GetFormTemplateById(formTemplate.Id);
                if (existed == null)
                {
                    return HttpBadRequest("Provided form template doesn't exist");
                }
                else
                {
                    existed.Id = formTemplate.Id;
                    existed.Name = formTemplate.Name;
                    existed.IntegrationType = formTemplate.IntegrationType;
                    existed.Type = formTemplate.Type;
                    existed.Submissions = formTemplate.Submissions;
                    existed.LastModified = DateTime.UtcNow; ;

                    _formTemplateService.UpdateFormTemplate(formTemplate);

                }
                return Ok();
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }
        */


        private FormBuilderGenerationTemplate MapGenerationTemplateFrom(FormTemplateModel source)
        {
            FormBuilderGenerationTemplate template = new FormBuilderGenerationTemplate();
            template.Name = source.FormTemplate.Name;
            if (source.FormTemplate.Properties!=null)
            {
                source.Properties = JsonConvert.DeserializeObject<FormProperties>(source.FormTemplate.Properties);
            }
            if (source.Properties != null && source.Properties.LayerProperties != null)
            {
                template.FormStyle = source.Properties.LayerProperties.FormStyle;
                template.FormClass = source.Properties.LayerProperties.FormClass;
                template.FormHeight = source.Properties.LayerProperties.FormHeight;
                template.FormWidth = source.Properties.LayerProperties.FormWidth;
                template.FormBackground = source.Properties.LayerProperties.FormBackground;
                template.ShowNextPrevButton = source.Properties.LayerProperties.ShowNextPrevButton;
                template.ReferringUrl = source.Properties.ReferralProperties.ReferringUrl;
                template.SubmitReturnURL = source.Properties.ReferralProperties.SubmitReturnURL;
                template.ChannelId = source.Properties.ReferralProperties.ChannelId;
                template.Password = source.Properties.ReferralProperties.Password;
                template.AffSubIds = source.Properties.ReferralProperties.AffSubIds;
                template.MinPrice = source.Properties.ReferralProperties.MinPrice;
            }

            if (source.Properties != null && source.Properties.PageProperties != null)
            {
                template.PageProperties = source.Properties.PageProperties;                
            }

            foreach (var item in source.FormTemplateItems)
            {
                
                FormBuilderFieldGenerationOptions options = new FormBuilderFieldGenerationOptions();
                template.Fields.Add(options);
                options.Name = item.Name;
                options.Properties.Height = item.Height;
                options.Properties.Width = item.Width;
                options.Properties.Label = item.Label;
                options.Properties.PlaceHolderText = item.PlaceHolderText;
                options.Properties.Required = item.IsRequired;
                options.Properties.ValidationRule = item.NeedsValidation? item.ValidationRegex: null;
                options.Properties.HelperText = item.HelperText;
                options.Step = item.Step;


                foreach (var style in item.StylesJson)
                {
                    var field = options.Style.GetType().GetField(style.Name);
                    if (field != null)
                        field.SetValue(options.Style, style.Value);

                    field = options.Layout.GetType().GetField(style.Name);
                    if (field!=null)                    
                        field.SetValue(options.Layout, style.Value);

                    field = options.Properties.GetType().GetField(style.Name);
                    if (field != null)
                    {
                        if (field.Name=="DataFormat")
                        {
                            //var val=Enum.Parse(typeof(EnumDataFormat), style.Value);
                            field.SetValue(options.Properties, style.Value);
                        }
                        else
                        if (field.Name== "Required")
                        {
                            field.SetValue(options.Properties, style.Value=="True");
                        }
                        else
                        if (field.Name == "Width" || field.Name=="Height")
                        {
                            if(!string.IsNullOrEmpty(style.Value))
                                field.SetValue(options.Properties, Int32.Parse(style.Value));
                        }
                        else
                            field.SetValue(options.Properties, style.Value);
                    }

                }                
            }
            return template;
        }

        private void MapGenerationTemplateTo(FormTemplateModel model, FormBuilderGenerationTemplate source)
        {
            foreach (var options in source.Fields)
            {
                FormTemplateItemModel item = new FormTemplateItemModel();
                item.Height=(short)options.Properties.Height;
                item.Width=(short)options.Properties.Width;
                item.Label=options.Properties.Label;
                item.PlaceHolderText=options.Properties.PlaceHolderText;
                item.IsRequired=(bool)options.Properties.Required;
                item.NeedsValidation = options.Properties.ValidationRule != null;
                item.ValidationRegex = options.Properties.ValidationRule;
                item.Value = options.Properties.DefaultValue;                
                item.Step = (short)options.Step;
                item.HelperText=options.Properties.HelperText;
                
                var fields = options.Style.GetType().GetFields();
                item.StylesJson = new List<FormTemplateItemStyles>();

                foreach (var field in fields)
                {
                    FormTemplateItemStyles style = new FormTemplateItemStyles();
                    style.Name = field.Name;
                    if (field.GetValue(options.Style)!=null)
                        style.Value = field.GetValue(options.Style).ToString();
                    item.StylesJson.Add(style);
                }
                fields = options.Properties.GetType().GetFields();
                foreach (var field in fields)
                {
                    FormTemplateItemStyles style = new FormTemplateItemStyles();
                    style.Name = field.Name;
                    if (field.GetValue(options.Properties)!=null)
                        style.Value = field.GetValue(options.Properties).ToString();
                    item.StylesJson.Add(style);
                }

                fields = options.Layout.GetType().GetFields();
                foreach (var field in fields)
                {
                    FormTemplateItemStyles style = new FormTemplateItemStyles();
                    style.Name = field.Name;
                    if (field.GetValue(options.Layout) != null)
                        style.Value = field.GetValue(options.Layout).ToString();
                    item.StylesJson.Add(style);
                }

                item.Name = options.Name;
                item.ResponseFormat = "";
                model.FormTemplateItems.Add(item);
            }
            model.FormTemplate.Name = source.AffiliateName;            
            model.FormTemplate.IntegrationType = IntegrationType.EmbeddedForm;            
        }

        [HttpGet]
        [Route("getFormTemplateById/{id}")]
        public IHttpActionResult GetFormTemplateById(long id)
        {
            try
            {
                var existed = _formTemplateService.GetFormTemplateById(id);
                if (existed == null)
                {
                    return HttpBadRequest("Provided form template doesn't exist");
                }
                else
                {
                    var result = new FormTemplateModel();
                    try
                    {
                        result.Properties = JsonConvert.DeserializeObject<FormProperties>(existed.Properties);
                    }
                    catch
                    {

                    }

                    var templateItems = _formTemplateService.GetFormTemplateItemsByTemplateId(existed.Id);
                    result.FormTemplate = existed;
                    foreach (var item in templateItems)
                    {
                        result.FormTemplateItems.Add(MapFromEntity(item));
                    }

                    
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [HttpPost]
        [Route("getJSForm/{id}")]
        [ContentManagementApiAuthorize(true)]
        [OverrideAuthorization]
        public IHttpActionResult GetFormGeneratorTemplateJs(long id)
        {
            try
            {
                var existed = _formTemplateService.GetFormTemplateById(id);
                if (existed == null)
                {
                    return HttpBadRequest("Provided form template doesn't exist");
                }
                else
                {
                    var templateItems = _formTemplateService.GetFormTemplateItemsByTemplateId(existed.Id);
                    var result = new FormTemplateModel();
                    result.FormTemplate = existed;
                    foreach (var item in templateItems)
                    {
                        item.Name = item.Name.Trim();
                        result.FormTemplateItems.Add(MapFromEntity(item));
                    }


                    return Ok(MapGenerationTemplateFrom(result));
                }
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("addTestTemplate")]
        public IHttpActionResult AddTestTempalate()
        {
            try
            {
                FormBuilderGenerationTemplate gen = new FormBuilderGenerationTemplate();
                gen.AffiliateName = "Test";

                //page 1 (2x2)

                var gridPos = 1;

                int format = (int)EnumDataFormat.Text;

                for (var i=0; i<4; i++)
                {
                    FormBuilderFieldGenerationOptions options = new FormBuilderFieldGenerationOptions();
                    options.TestFill(gridPos,"A", FormBuilderFieldGenerationOptions.EnumColumnPostion.Left,(EnumDataFormat)format);
                    format++;
                    gridPos++;
                    if (gridPos > 2)
                        gridPos = gridPos = 1;
                    options.Step = 0;
                    gen.Fields.Add(options);
                }

                //page 1 (2x4)
                gridPos = 1;
                for (var i = 0; i < 8; i++)
                {
                    FormBuilderFieldGenerationOptions options = new FormBuilderFieldGenerationOptions();
                    options.TestFill(gridPos,"B", FormBuilderFieldGenerationOptions.EnumColumnPostion.Right, (EnumDataFormat)format);
                    format++;
                    gridPos++;
                    if (gridPos > 2)
                        gridPos = gridPos = 1;
                    options.Step = 1;
                    gen.Fields.Add(options);
                }

                //page 3 (3x3)
                gridPos = 1;
                for (var i = 0; i < 9; i++)
                {
                    FormBuilderFieldGenerationOptions options = new FormBuilderFieldGenerationOptions();
                    options.TestFill(gridPos,"B", FormBuilderFieldGenerationOptions.EnumColumnPostion.MiddleTop, (EnumDataFormat)format);
                    format++;
                    if (format > 17)
                        format = 0;

                    gridPos++;
                    if (gridPos > 3)
                        gridPos = gridPos = 1;
                    options.Step = 2;
                    gen.Fields.Add(options);
                }

                FormTemplateModel model = new FormTemplateModel();
                model.FormTemplate = new FormTemplate();
                model.FormTemplate.AffiliateChannelId = 1;
                model.FormTemplate.Type = FormTemplateType.SingleForm;
                model.FormTemplate.IntegrationType = IntegrationType.EmbeddedForm;                
                model.FormTemplateItems = new List<FormTemplateItemModel>();
                
                model.Properties.LayerProperties.FormBackground = "black";
                var prop = new FormTemplatePageProperty();
                prop.DescriptionClass = "test";
                model.Properties.PageProperties.Add(prop);
                prop = new FormTemplatePageProperty();
                prop.DescriptionText = "aa";
                model.Properties.PageProperties.Add(prop);

                model.FormTemplate.Properties = JsonConvert.SerializeObject(model.Properties);
                MapGenerationTemplateTo(model, gen);
                model.Properties.LayerProperties.ShowNextPrevButton = false;
                SaveFormTemplate(model);
                return Ok("Ok");
                
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getFormTemplates")]
        public IHttpActionResult GetFormTemplates()
        {
            try
            {
                List<FormTemplateUIModel> list = new List<FormTemplateUIModel>();
                var formTemplates = _formTemplateService.GetAllFormTemplates();
                foreach(var formTemplate in formTemplates)
                {
                    long campaignId = 0;
                    var affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(formTemplate.AffiliateChannelId);
                    if (affiliateChannel != null && affiliateChannel.CampaignId.HasValue)
                        campaignId = affiliateChannel.CampaignId.Value;

                    list.Add(new FormTemplateUIModel()
                    {
                        Id = formTemplate.Id,
                        AffiliateChannelId = formTemplate.AffiliateChannelId,
                        CampaignId = campaignId,
                        Created = formTemplate.Created,
                        IntegrationType = formTemplate.IntegrationType,
                        LastModified = formTemplate.LastModified,
                        Name = formTemplate.Name,
                        Properties = formTemplate.Properties,
                        Submissions = formTemplate.Submissions,
                        Type = formTemplate.Type
                    });
                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getFormTemplatesByAffiliateChannelId/{affiliateChannelId}")]
        public IHttpActionResult GetFormTemplatesByAffiliateChannelId(long affiliateChannelId)
        {
            try
            {
                var formTemplates = _formTemplateService.GetAllFormTemplates(affiliateChannelId);
                return Ok(formTemplates);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("deleteFormTemplate/{id}")]
        public IHttpActionResult deleteFormTemplate(long id)
        {
            try
            {
                var existed = _formTemplateService.GetFormTemplateById(id);
                if (existed == null)
                {
                    return HttpBadRequest("Provided form template doesn't exist");
                }
                else
                {
                    _formTemplateService.DeleteFormTemplateItems(existed);
                    _formTemplateService.DeleteFormTemplate(existed);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("saveFormTemplateItem")]
        public IHttpActionResult SaveFormTemplateItem([FromBody]FormTemplateItemModel formTemplateItemModel)
        {
            var result = 0.0;
            try
            {
                if (formTemplateItemModel.ReferringFieldId.HasValue)
                {
                    var affiliateChannelTemplate = _affiliateChannelTemplateService.GetAffiliateChannelTemplateById(formTemplateItemModel.ReferringFieldId.Value);
                    if (affiliateChannelTemplate == null)
                    {
                        return HttpBadRequest("Wrong Referring Field id");
                    }
                }
                var existed = _formTemplateService.GetFormTemplateById(formTemplateItemModel.FormTemplateId);
                if (existed != null)
                {
                    var formTemplateItem = new FormTemplateItem();
                    if (!string.IsNullOrWhiteSpace(formTemplateItemModel.Name))
                    {
                        formTemplateItem = MapFromModel(formTemplateItemModel);
                        formTemplateItem.Created = DateTime.UtcNow;                        
                        result = _formTemplateService.InsertFormTemplateItem(formTemplateItem);


                        //move file
                        var iconNameTmp = formTemplateItemModel.TmpIconName;
                        if (!string.IsNullOrEmpty(iconNameTmp))
                        {
                            var relativePathTmp = "~/Content/Uploads/Icons/Form/Tmp/";
                            var targetFolderTmp = HttpContext.Current.Server.MapPath(relativePathTmp);
                            var targetPathTmp = Path.Combine(targetFolderTmp, iconNameTmp);

                            var extTmp = Path.GetExtension(targetPathTmp);
                            var iconNameOrg = $"{formTemplateItemModel.FormTemplateId}-{result}{extTmp}";

                            var relativePathOrg = "~/Content/Uploads/Icons/Form/FormItems/";
                            var targetFolderOrg = HttpContext.Current.Server.MapPath(relativePathOrg);
                            var targetPathOrg = Path.Combine(targetFolderOrg, iconNameOrg);

                            if (File.Exists(targetPathTmp))
                            {
                                File.Move(targetPathTmp, targetPathOrg);
                            }
                        }


                    }
                    else if (string.IsNullOrEmpty(formTemplateItem.Name))
                    {
                        return HttpBadRequest("Form builder item Name can not be empty");
                    }
                    return Ok(result);
                }
                else
                {
                    return HttpBadRequest("Provided form template doesn't exist");
                }
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("updateFormTemplateItem")]
        public IHttpActionResult UpdateFormTemplateItem([FromBody]FormTemplateItemModel formTemplateItemModel)
        {
            try
            {
                var existed = _formTemplateService.GetFormTemplateItemById(formTemplateItemModel.Id);
                if (existed == null)
                {
                    return HttpBadRequest("Provided form template item doesn't exist");
                }
                else
                {
                    if (formTemplateItemModel.ReferringFieldId.HasValue)
                    {
                        var affiliateChannelTemplate = _affiliateChannelTemplateService.GetAffiliateChannelTemplateById(formTemplateItemModel.ReferringFieldId.Value);
                        if (affiliateChannelTemplate == null)
                        {
                            return HttpBadRequest("Wrong Referring Field id");
                        }
                    }
                    MapFromModelUpdate(formTemplateItemModel, existed);
                    existed.Id = formTemplateItemModel.Id;
                    _formTemplateService.UpdateFormTemplateItem(existed);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("getFormTemplateItemById/{id}")]
        public IHttpActionResult GetFormTemplateItemById(long id)
        {
            try
            {
                var existed = _formTemplateService.GetFormTemplateItemById(id);
                if (existed == null)
                {
                    return HttpBadRequest("Provided form template Item doesn't exist");
                }
                else
                {
                    return Ok(MapFromEntity(existed));
                }
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getFormTemplateItemsByTemplateId/{templateId}")]
        public IHttpActionResult GetFormTemplateItems(long templateId)
        {
            var result = new List<FormTemplateItemModel>();
            try
            {
                var formTemplateItems = _formTemplateService.GetFormTemplateItemsByTemplateId(templateId);
                if (formTemplateItems != null && formTemplateItems.Any())
                {
                    foreach (var item in formTemplateItems)
                    {
                        result.Add(MapFromEntity(item));
                    }
                    
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("deleteFormTemplateItem/{id}")]
        public IHttpActionResult deleteFormTemplateItem(long id)
        {
            try
            {
                var existed = _formTemplateService.GetFormTemplateItemById(id);
                if (existed == null)
                {
                    return HttpBadRequest("Provided form template doesn't exist");
                }
                else
                {
                    _formTemplateService.DeleteFormTemplateItem(existed);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("deleteFormTemplateItemsByTemplateId/{id}")]
        public IHttpActionResult deleteFormTemplateItems(long id)
        {
            try
            {
                var existed = _formTemplateService.GetFormTemplateById(id);
                if (existed == null)
                {
                    return HttpBadRequest("Provided form template doesn't exist");
                }
                else
                {
                    _formTemplateService.DeleteFormTemplateItems(existed);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        #region PrivateMethods
        private FormTemplateItem MapFromModel(FormTemplateItemModel form)
        {
           
            var result = new FormTemplateItem();
            var helperStyle = "";
            //form.HelperStyle.Bold ? "Bold," : form.HelperStyle.Italic ? "Italic," : form.HelperStyle.Underline ? "Underline" : "";
            var labelStyle = "";// form.LabelStyle.Bold ? "Bold," : form.LabelStyle.Italic ? "Italic," : form.LabelStyle.Underline ? "Underline" : "";
            var placeHolderStyle = "";// form.PlaceHolderStyle.Bold ? "Bold," : form.PlaceHolderStyle.Italic ? "Italic," : form.PlaceHolderStyle.Underline ? "Underline" : "";
            result = new FormTemplateItem()
            {
                Id = form.Id,
                Column = form.Column,
                Created = form.Created,
                Name = form.Name,
                InlineList = form.InlineList,
                HelperText = form.HelperText,
                Height = form.Height,
                IsRequired = form.IsRequired,
                Label = form.Label,
                FormTemplateId = form.FormTemplateId,
                NeedsValidation = form.NeedsValidation,
                Options = string.Join(",", form.Options),
                HelperStyle = helperStyle,
                LabelStyle = labelStyle,
                PlaceHolderStyle = placeHolderStyle,
                ParentId = form.ParentId,
                PlaceHolderText = form.PlaceHolderText,
                Type = form.Type,
                Step = form.Step,
                ReferringFieldId = form.ReferringFieldId,
                ResponseFormat = form.ResponseFormat,
                StylesJson = JsonConvert.SerializeObject(form.StylesJson),
                ValidationRegex = form.ValidationRegex,
                Width = form.Width,
                Value = form.Value
            };
            return result;
        }

        private void MapFromModelUpdate(FormTemplateItemModel from, FormTemplateItem to)
        {
            var result = new FormTemplateItem();
            var helperStyle = from.HelperStyle.Bold ? "Bold," : from.HelperStyle.Italic ? "Italic," : from.HelperStyle.Underline ? "Underline" : "";
            var labelStyle = from.LabelStyle.Bold ? "Bold," : from.LabelStyle.Italic ? "Italic," : from.LabelStyle.Underline ? "Underline" : "";
            var placeHolderStyle = from.PlaceHolderStyle.Bold ? "Bold," : from.PlaceHolderStyle.Italic ? "Italic," : from.PlaceHolderStyle.Underline ? "Underline" : "";
            to.Id = from.Id;
            to.Column = from.Column;
            to.Created = from.Created;
            to.Name = from.Name;
            to.InlineList = from.InlineList;
            to.HelperText = from.HelperText;
            to.Height = from.Height;
            to.IsRequired = from.IsRequired;
            to.Label = from.Label;
            to.FormTemplateId = from.FormTemplateId;
            to.NeedsValidation = from.NeedsValidation;
            to.Options = string.Join(",", from.Options);
            to.HelperStyle = helperStyle;
            to.LabelStyle = labelStyle;
            to.PlaceHolderStyle = placeHolderStyle;
            to.ParentId = from.ParentId;
            to.PlaceHolderText = from.PlaceHolderText;
            to.Type = from.Type;
            to.Step = from.Step;
            to.ReferringFieldId = from.ReferringFieldId;
            to.ResponseFormat = from.ResponseFormat;
            to.StylesJson = JsonConvert.SerializeObject(from.StylesJson);
            to.ValidationRegex = from.ValidationRegex;
            to.Width = from.Width;
        }

        private FormTemplateItemModel MapFromEntity(FormTemplateItem from)
        {
            var result = new FormTemplateItemModel();
            //var helperStyle = from.HelperStyle.Bold ? "Bold," : from.HelperStyle.Italic ? "Italic," : from.HelperStyle.Underline ? "Underline" : "";
            //var labelStyle = from.LabelStyle.Bold ? "Bold," : from.LabelStyle.Italic ? "Italic," : from.LabelStyle.Underline ? "Underline" : "";
            //var placeHolderStyle = from.PlaceHolderStyle.Bold ? "Bold," : from.PlaceHolderStyle.Italic ? "Italic," : from.PlaceHolderStyle.Underline ? "Underline" : "";
            result = new FormTemplateItemModel()
            {
                Id = from.Id,
                Column = from.Column,
                Created = from.Created,
                Name = from.Name,
                InlineList = from.InlineList,
                HelperText = from.HelperText,
                Height = from.Height,
                IsRequired = from.IsRequired,
                Label = from.Label,
                FormTemplateId = from.FormTemplateId,
                NeedsValidation = from.NeedsValidation,
                Options = from.Options != null ? from.Options.Split(',').ToList() : new List<string>(),
                HelperStyle = new TextContentStyle()
                {
                    Bold = from.HelperStyle.ToLower().Contains("bold"),
                    Italic = from.HelperStyle.ToLower().Contains("italic"),
                    Underline = from.HelperStyle.ToLower().Contains("underline")
                },
                LabelStyle = new TextContentStyle()
                {
                    Bold = from.LabelStyle.ToLower().Contains("bold"),
                    Italic = from.LabelStyle.ToLower().Contains("italic"),
                    Underline = from.LabelStyle.ToLower().Contains("underline")
                },
                PlaceHolderStyle = new TextContentStyle()
                {
                    Bold = from.PlaceHolderStyle.ToLower().Contains("bold"),
                    Italic = from.PlaceHolderStyle.ToLower().Contains("italic"),
                    Underline = from.PlaceHolderStyle.ToLower().Contains("underline")
                },
                ParentId = from.ParentId,
                PlaceHolderText = from.PlaceHolderText,
                Type = from.Type,
                Step = from.Step,
                ReferringFieldId = from.ReferringFieldId,
                ResponseFormat = from.ResponseFormat,
                StylesJson = !string.IsNullOrWhiteSpace(from.StylesJson) ? JsonConvert.DeserializeObject<List<FormTemplateItemStyles>>(from.StylesJson) : new List<FormTemplateItemStyles>(),
                ValidationRegex = from.ValidationRegex,
                Width = from.Width
            };
            return result;
        }
        #endregion
    }
}