using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicativo.View.Helpers.Exceptions
{
    public class LoginRequiredException : Exception
    {
        public LoginRequiredException() : base()
        {
            App.NavigationManager.NavigateTo("Login/Entrar");
        }
    }
}
