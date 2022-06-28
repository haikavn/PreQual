using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models
{
    public class ErrorViewModel
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<ErrorViewModel> Errors { get; set; }

        public ErrorViewModel()
        {
            Errors = new List<ErrorViewModel>();
        }
    }
}