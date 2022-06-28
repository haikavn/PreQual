using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Adrack.WebApi
{
    public class PlainTextFormatter : MediaTypeFormatter
    {
        public PlainTextFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/xml"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/json"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }

        public override bool CanReadType(Type type) =>
            type == typeof(string);

        public override bool CanWriteType(Type type) =>
            type == typeof(string);

        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var streamReader = new StreamReader(readStream);
            return await streamReader.ReadToEndAsync();
        }

        public override async Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext, CancellationToken cancellationToken)
        {
            var streamReader = new StreamWriter(writeStream);
            await streamReader.WriteAsync((string)value);
        }
    }
}