using System.Threading.Tasks;

namespace Aplicativo.View.Services
{

    public interface IBarCodeScanner
    {
        Task<string> ScanAsync();
    }

}