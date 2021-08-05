using Aplicativo.Utils;
using Aplicativo.View.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Build.Server.Controllers
{
    public class _Exception
    {

        public static Response Response(Exception ex, Response Response)
        {
            Response.StatusCode = Aplicativo.Utils.StatusCode.Error;
            Response.Data = HelpErro.GetMessage(new Error(ex));
            return Response;
        }

    }
}
