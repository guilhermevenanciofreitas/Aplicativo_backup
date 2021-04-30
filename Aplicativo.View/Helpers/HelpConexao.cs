using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Aplicativo.View.Helpers
{

    public static class HelpConexao
    {

        public static Dominio Dominio { get; set; } = new Dominio();

        public static async Task<Dominio> GetDominio(IJSRuntime JSRuntime)
        {

            var Dominio = await HelpCookie.Get(JSRuntime, "Dominio");

            if (string.IsNullOrEmpty(Dominio))
            {
                return new Dominio();
            }

            return JsonSerializer.Deserialize<Dominio>(Dominio);

        }

        public static async Task SetName(IJSRuntime JSRuntime, string Name)
        {

            Dominio.Name = Name;

            await HelpCookie.Set(JSRuntime, "Dominio", JsonSerializer.Serialize(Dominio), 30);

        }

        public static async Task Add(IJSRuntime JSRuntime, Conexao Conexao)
        {

            Dominio.Conexao.Add(Conexao);

            await HelpCookie.Set(JSRuntime, "Dominio", JsonSerializer.Serialize(Dominio), 30);

        }

        public static async Task Remove(IJSRuntime JSRuntime, string Name)
        {

            foreach (var item in Dominio.Conexao.Where(c => c.Name == Name).ToList())
            {
                Dominio.Conexao.Remove(item);
            }

            await HelpCookie.Set(JSRuntime, "Dominio", JsonSerializer.Serialize(Dominio), 30);

        }

    }

    [Serializable]
    public class Dominio
    {

        public string Name { get; set; }

        public List<Conexao> Conexao { get; set; } = new List<Conexao>();

    }

    [Serializable]
    public class Conexao
    {

        public string Name { get; set; }

        public string Server { get; set; }
        public string Database { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }

    }

}