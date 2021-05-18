using System;

namespace Aplicativo.View.Atributes
{
    public class DatabaseRequired : Attribute
    {
        public bool Required;

        public DatabaseRequired(bool Required)
        {
            this.Required = Required;
        }
    }
}
