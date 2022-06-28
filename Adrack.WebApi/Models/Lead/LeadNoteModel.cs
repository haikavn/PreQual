using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class LeadNoteModel
    {
        public string Created { get; internal set; }
        public string Title { get; internal set; }
        public string Note { get; internal set; }
        public string Author { get; internal set; }
    }
}