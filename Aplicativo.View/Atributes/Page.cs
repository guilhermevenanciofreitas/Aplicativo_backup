using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Atributes
{
    public class Page : Attribute
    {
        public string Uri;

        public Page(string Uri)
        {
            this.Uri = Uri;
        }
    }
}
