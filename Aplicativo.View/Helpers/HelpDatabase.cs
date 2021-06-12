using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Microsoft.JSInterop;
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

            //Request.Parameters.Add(new Parameters("Database", Database));

            Query.Database = Database;

            Request.Parameters.Add(new Parameters("Query", Query));

            return await HelpHttp.Send<List<T>>(Link, Request);

        }

        public static async Task<T> FirstOrDefault<T>(this HelpQuery<T> Query, string Link = "api/Default/Query")
        {

            Query.AddTake(1);

            return (await ToList(Query, Link)).FirstOrDefault();

        }

        //public static async Task<object> Update<T>(HelpUpdate HelpUpdate, string Link = "api/Default/Update")
        //{
        //    return (await Update(new List<HelpUpdate> { HelpUpdate }, Link)).FirstOrDefault();
        //}

        //public static async Task<List<object>> Update(List<HelpUpdate> HelpUpdate, string Link = "api/Default/Update")
        //{

        //    var Database = ""; //Query.Database;

        //    //if (Query.Database == null)
        //    //{
        //        Database = HelpConexao.GetDatabase();

        //        if (Database == null)
        //        {
        //            throw new Exception("Nenhuma conexão informada!");
        //        }
        //    //}

        //    var Request = new Request();

        //    Request.Parameters.Add(new Parameters("Database", Database));
        //    Request.Parameters.Add(new Parameters("Update", HelpUpdate));

        //    return await HelpHttp.Send<List<object>>(Link, Request);

        //}

        public static async Task<List<Dictionary<string, object>>> SaveChanges(this HelpUpdate HelpUpdate, string Link = "api/Default/Update")
        {

            var Database = ""; //Query.Database;

            //if (Query.Database == null)
            //{
            Database = HelpConexao.GetDatabase();

            if (Database == null)
            {
                throw new Exception("Nenhuma conexão informada!");
            }
            //}

            var Request = new Request();

            Request.Parameters.Add(new Parameters("Database", Database));
            Request.Parameters.Add(new Parameters("Update", HelpUpdate.Update));

            return await HelpHttp.Send<List<Dictionary<string, object>>>(Link, Request);

            //for(var i = 0; i < Items.Count; i++)
            //{
            //    //await App.JSRuntime.InvokeVoidAsync("console.log", Items[i]);
            //    HelpUpdate.Update[i].Object = Items[i];
            //}

            //foreach(var Item in Items){

            //}


        }

    }
}