using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UnitTest_Services
{
    public static class FakeHttpContext
    {
        public static HttpRequestBase GetFakeRequest(string data = "")
        {
            var bytes = Encoding.ASCII.GetBytes(data);
            HttpRequestBase request = new HttpRequestWrapper(new HttpRequest("test", "http://localhost", data));
            //request.InputStream.Write(bytes, 0, bytes.Length);
            return request;
        }

        public static HttpResponseBase GetFakeResponse(string data = "")
        {
            var stream = new MemoryStream();
            var csvWriter = new StreamWriter(stream, Encoding.UTF8);
            csvWriter.Write(data);
            var response = new HttpResponse(csvWriter);
            return new HttpResponseWrapper(response);
        }

        public static void SetFakeContext(this Controller controller, string queryString = "", string data = "")
        {
            var request = new HttpRequest("test", "http://localhost", queryString);
            var stream = new MemoryStream();
            var csvWriter = new StreamWriter(stream, Encoding.UTF8);
            csvWriter.Write(data);
            var responce = new HttpResponse(csvWriter);
            var context = new HttpContext(request, responce);
            HttpContext.Current = context;

            var wrapper = new HttpContextWrapper(HttpContext.Current);

            var contextController =
                new ControllerContext(
                    new RequestContext(wrapper,
                        new RouteData()), controller);

            controller.ControllerContext = contextController;
        }
    }
}