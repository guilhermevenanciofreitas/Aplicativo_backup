using Aplicativo.View.Services;
using System;
using System.Threading.Tasks;

namespace Aplicativo.Browser.Services
{
    public class BarCodeScanner : IBarCodeScanner
    {
        public Task<string> ScanAsync()
        {
            throw new NotImplementedException();
        }
    }
}