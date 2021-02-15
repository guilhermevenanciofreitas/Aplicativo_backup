using Aplicativo.Utils;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Aplicativo.View.Helpers
{
    public static class HelpHttp
    {

        public static string Url { get; set; } = ""; //"http://192.168.0.6:7070/";

        public async static Task Send(HttpClient Http, string Action, Request Request)
        {
            var Response = await Post(Http, Action, Request);
            Tratament(Response);
        }

        public async static Task<T> Send<T>(HttpClient Http, string Action, Request Request) where T : class
        {
            var Response = await Post(Http, Action, Request);
            Tratament(Response);
            return JsonConvert.DeserializeObject<T>(Response.Data.ToString());
        }

        public async static Task<Response> Post(HttpClient Http, string Action, Request Request)
        {
            var Result = await Http.PostAsJsonAsync(Path.Combine(Url, Action), Request);
            return await Result.Content.ReadFromJsonAsync<Response>();
        }

        public static void Tratament(Response Response)
        {
            switch (Response.StatusCode)
            {
                case StatusCode.Success:
                    return;

                case StatusCode.LoginExpired:
                    throw new Exception("LoginExpired");

                default:
                    throw new Exception("HelpHttp.NotImplemented");

            }
        }

    }
}