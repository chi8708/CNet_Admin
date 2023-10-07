using CNet.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace CNet.Web.Api
{
    public class AuthenticationFailureResult : IActionResult
    {
        public AuthenticationFailureResult(string reasonPhrase, HttpRequest request,HttpResponse response)
        {
            ReasonPhrase = reasonPhrase;
            Request = request;
            Response = response;
        }

        public string ReasonPhrase { get; set; }

        public HttpRequest Request { get; set; }

        public HttpResponse Response { get; set; }

        public Task ExecuteResultAsync(ActionContext context)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponse Execute()
        {
            //HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            //{
            //    RequestMessage = Request,
            //    ReasonPhrase = ReasonPhrase,
            //    Content =new StringContent("Unauthorized")//cts 添加
            //};

         
            DataRes<bool> res = new DataRes<bool>()
            {
                code = ResCode.Unauthorized,
                data = false,
                msg = ReasonPhrase
            };
            Response.Headers.Add("sysCode", "401");
            Response.StatusCode =Convert.ToInt32(ResCode.Unauthorized);
            var bytes=Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(res));
            Response.Body.Write(bytes);
            return Response;

            //CTS修改
            //HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            //{
            //    Content = new StringContent(JsonConvert.SerializeObject(res), Encoding.UTF8, "application/json"),//cts 添加
            //    ReasonPhrase= ReasonPhrase,

            //};

            //response.Headers.Add("sysCode", "401");
            //return response;
        }
    }
}