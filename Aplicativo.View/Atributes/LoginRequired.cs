using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Atributes
{
    public class LoginRequired : Attribute
    {
        public bool Required;

        public LoginRequired(bool Required)
        {
            this.Required = Required;
        }
    }
}
