using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.FormBuilder
{
    public class FormBuilderGenerationPage
    {
        public string Name;
        long Step;
        public List<FormBuilderFieldGenerationOptions> Fields = new List<FormBuilderFieldGenerationOptions>();
    }
    public class FormBuilderGenerationTemplate
    {
        public string Name;
        public string AffiliateName { get; set; }
        public bool IsWizard;
        public List<FormBuilderGenerationPage> pages = new List<FormBuilderGenerationPage>();
        public List<FormBuilderFieldGenerationOptions> Fields=new List<FormBuilderFieldGenerationOptions>();

    }
}