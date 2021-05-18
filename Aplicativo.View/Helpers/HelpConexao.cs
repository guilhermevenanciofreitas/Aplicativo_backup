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

        public static async Task<Dominio> GetDominio()
        {

            var Dominio = await HelpCookie.Get("Dominio");

            if (string.IsNullOrEmpty(Dominio))
            {
                return new Dominio();
            }

            return JsonSerializer.Deserialize<Dominio>(Dominio);

        }

        public static string GetDatabase()
        {

            var Conexao = Dominio.Conexao.FirstOrDefault(c => c.Name == Dominio.Name);

            if (Conexao == null)
            {
                return null;
            }

            return Conexao.GetDatabase();

        }

        public static async Task SetName(string Name)
        {

            Dominio.Name = Name;

            await HelpCookie.Set("Dominio", JsonSerializer.Serialize(Dominio), 0);

        }

        public static async Task Add(Conexao Conexao)
        {

            Dominio.Conexao.Add(Conexao);

            await HelpCookie.Set("Dominio", JsonSerializer.Serialize(Dominio), 0);

        }

        public static async Task Remove(string Name)
        {

            foreach (var item in Dominio.Conexao.Where(c => c.Name == Name).ToList())
            {
                Dominio.Conexao.Remove(item);
            }

            await HelpCookie.Set("Dominio", JsonSerializer.Serialize(Dominio), 0);

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

        public bool? IsOnline { get; set; }

        public string GetDatabase()
        {
            return "Server=" + Server + ";Database=" + Database + ";User Id=" + UserId + ";Password=" + Password;
        }

        public string Status
        {
            get
            {
                if (IsOnline == true)
                {
                    return "online";
                }
                if (IsOnline == false)
                {
                    return "offline";
                }

                return "";

            }
        }

        public string Color { 
            get
            {
                if (IsOnline == true)
                {
                    return "#7fffd4";
                }
                if (IsOnline == false)
                {
                    return "red";
                }

                return "";
            } 
        }


    }

}