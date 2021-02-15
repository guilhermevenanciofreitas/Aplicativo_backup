using Aplicativo.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aplicativo.Utils
{

    public class Parameters
    {

        public Parameters(string Key, object Data)
        {
            this.Key = Key;
            this.Data = Data;
        }

        public string Key { get; set; }

        public object Data { get; set; }

    }

    public class Request
    {

        public List<Parameters> Parameters { get; set; } = new List<Parameters>();


        public string GetParameter(string Key)
        {
            return Parameters.FirstOrDefault(c => c.Key == Key)?.Data?.ToStringOrNull();
        }

    }

}