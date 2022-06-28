using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Adrack.WebApi.Models;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Infrastructure.Web.Attributes
{
    public class ValidateModelAttribute : ActionFilterAttribute


    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                var errors = actionContext.ModelState.
                                        SelectMany(state => state.Value.Errors).
                                        Aggregate("", (current, error) => current + (!string.IsNullOrEmpty(error.ErrorMessage) ? (error.ErrorMessage + ". ") : "Invalid request"));

                ErrorViewModel errorViewModel = new ErrorViewModel()
                {
                    Errors = new List<ErrorViewModel>(),
                    Message = errors,
                    Status = (int)HttpStatusCode.BadRequest
                };

                actionContext.Response = actionContext.Request.CreateResponse<ErrorViewModel>(HttpStatusCode.BadRequest, errorViewModel);
            }
        }
    }
}