using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Adrack.Core.Helpers;
using Adrack.WebApi.Models;
using Newtonsoft.Json;

namespace Adrack.WebApi.ErrorHandler
{
    class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IExceptionHandler _innerHandler;

        public GlobalExceptionHandler(IExceptionHandler innerHandler)
        {
            if (innerHandler == null)
                throw new ArgumentNullException(nameof(innerHandler));

            _innerHandler = innerHandler;
        }

        public IExceptionHandler InnerHandler
        {
            get { return _innerHandler; }
        }

        public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            Handle(context);

            return Task.FromResult<object>(null);
        }

        public void Handle(ExceptionHandlerContext context)
        {
            string message = $"{WebHelper.GetSubdomain()}-{context.Exception?.Message}-{context.Exception?.InnerException?.Message}-{context.Exception?.StackTrace}";
            context.Result = new CustomErrorResult
            {
                Request = context.ExceptionContext.Request,
                Message = message//context.Exception?.InnerException?.Message?? context.Exception?.Message
            };
        }

        private class CustomErrorResult : IHttpActionResult
        {
            public HttpRequestMessage Request { get; set; }
            public string Message { get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var error = new ErrorViewModel()
                {
                    Message = $"Exception message: {Message}",
                    Status = (int)HttpStatusCode.BadRequest
                };
                var response = new HttpResponseMessage()
                {
                    Content = new ObjectContent<ErrorViewModel>(error, new JsonMediaTypeFormatter()),
                    RequestMessage = Request
                };
                return Task.FromResult(response);
            }
        }
    }

    
}