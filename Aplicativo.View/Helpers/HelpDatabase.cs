using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Aplicativo.View.Helpers
{
    public static class HelpDatabase
    {

        public static async Task<List<T>> ToList<T>(this HelpQuery<T> Query, string Link = "api/Default/Query")
        {

            var Database = Query.Database;

            if (Query.Database == null)
            {
                Database = HelpConexao.GetDatabase();

                if (Database == null)
                {
                    throw new Exception("Nenhuma conexão informada!");
                }
            }

            var Request = new Request();

            Request.Parameters.Add(new Parameters("Database", Database));
            Request.Parameters.Add(new Parameters("Query", Query));

            return await HelpHttp.Send<List<T>>(Link, Request);

        }

        public static async Task<T> FirstOrDefault<T>(this HelpQuery<T> Query, string Link = "api/Default/Query")
        {

            Query.AddTake(1);

            return (await ToList(Query, Link)).FirstOrDefault();

        }

        public static async Task<T> Update<T>(this HelpQuery<T> Query, T Object, bool RemoveIncludes = true, string Link = "api/Default/Update")
        {
            return (await Update(Query, new List<T> { Object }, RemoveIncludes, Link)).FirstOrDefault();
        }

        public static async Task<List<T>> Update<T>(this HelpQuery<T> Query, List<T> Object, bool RemoveIncludes = true, string Link = "api/Default/Update")
        {

            var Database = Query.Database;

            if (Query.Database == null)
            {
                Database = HelpConexao.GetDatabase();

                if (Database == null)
                {
                    throw new Exception("Nenhuma conexão informada!");
                }
            }

            var Request = new Request();

            Request.Parameters.Add(new Parameters("Database", Database));
            Request.Parameters.Add(new Parameters("Table", typeof(T).Name));
            Request.Parameters.Add(new Parameters(typeof(T).Name, Object));
            Request.Parameters.Add(new Parameters("RemoveIncludes", RemoveIncludes));

            return await HelpHttp.Send<List<T>>(Link, Request);

        }

    }
}