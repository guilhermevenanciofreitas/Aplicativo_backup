using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Aplicativo.View.Helpers
{
    public class HelpLoading
    {

        public static async Task Show(string text = null) 
        {
            if (text == null) text = "Carregando...";
            await App.JSRuntime.InvokeVoidAsync("Loading.Show", text);
        }
        public static async Task Hide() 
        {
            await App.JSRuntime.InvokeVoidAsync("Loading.Hide");
        }

    }
}