using Aplicativo.View.Services;
using System.Threading.Tasks;
using ZXing.Mobile;

namespace Aplicativo.Binding.Services
{

    public class BarCodeScanner : IBarCodeScanner
    {

        public async Task<string> ScanAsync()
        {

            var OptionsCustom = new MobileBarcodeScanningOptions()
            {
                 AutoRotate = true,
            };

            var Scanner = new MobileBarcodeScanner()
            {
                TopText = "Aproxime a câmera do código de barra",
                BottomText = "Toque na tela para focar",
            };

            return (await Scanner.Scan(OptionsCustom))?.Text;

        }

    }
}