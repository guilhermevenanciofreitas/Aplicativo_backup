using Microsoft.JSInterop;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Aplicativo.View.Helpers
{

    public class Error
    {

        public Error(Exception Exception)
        {
            this.Exception = Exception;
            this.StackTrace = new StackTrace(Exception, true);
        }

        //public Error(Exception Exception, StackTrace StackTrace)
        //{
        //    this.Exception = Exception;
        //    this.StackTrace = StackTrace;
        //}

        public Exception Exception;
        public StackTrace StackTrace;
    }

    public static class HelpErro
    {

        public static async Task Show(Error Error)
        {
            await App.JSRuntime.InvokeVoidAsync("alert", GetMessage(Error));
        }

        public static async Task Console(Error Error)
        {
            await App.JSRuntime.InvokeVoidAsync("console.log", GetMessage(Error));
        }

        public static string GetMessage(Error Error)
        {
            return 
                "Info: " + Error.Exception.Message + "\n" +
                "Namespace: " + Error.Exception.TargetSite.ReflectedType.FullName + "\n" +
                "File: " + Error.StackTrace.GetFrame(0).GetFileName() + "\n" +
                "Line: " + Error.StackTrace.GetFrame(0).GetFileLineNumber();
        }

    }
}