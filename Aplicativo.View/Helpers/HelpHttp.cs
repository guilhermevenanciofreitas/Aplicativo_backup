using Aplicativo.Utils;
using Aplicativo.View.Helpers.Exceptions;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Aplicativo.View.Helpers
{
    public static class HelpHttp
    {

        public static string Url { get; set; } = ""; //"http://192.168.0.6:7070/";

        public async static Task<T> Send<T>(string Action, Request Request) where T : class
        {

            var Response = await Post(Action, Request);

            Tratament(Response);

            if (Response.Data == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(Response.Data.ToString());

        }

        public async static Task<Response> Post(string Action, Request Request)
        {
            var Result = await App.Http.PostAsJsonAsync(Path.Combine(Url, Action), Request);

            if (!Result.IsSuccessStatusCode) throw new Exception("Problema na conexão com o servidor!");
        
            if (string.IsNullOrEmpty((await Result.Content.ReadAsStringAsync())))
            {
                return null;
            }

            return await Result.Content.ReadFromJsonAsync<Response>();
        }

        public static void Tratament(Response Response)
        {
            switch (Response.StatusCode)
            {
                case StatusCode.Success:
                    return;

                case StatusCode.Error:
                    throw new Exception(Response.Data.ToString());

                case StatusCode.LoginRequired:
                    throw new LoginRequiredException();

                default:
                    throw new Exception("HelpHttp.NotImplemented");

            }
        }
    }
}