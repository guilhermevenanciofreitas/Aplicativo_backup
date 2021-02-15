using Aplicativo.View.Services;
using System.IO;
using System.Threading.Tasks;

namespace Aplicativo.Binding.Services
{

    public class Camera : ICamera
    {

        public async Task<byte[]> TakePhotoAsync()
        {

            var Photo = await Xamarin.Essentials.MediaPicker.CapturePhotoAsync(new Xamarin.Essentials.MediaPickerOptions() { });

            if (Photo == null) return null;

            var Stream = await Photo.OpenReadAsync();

            using (BinaryReader BinaryReader = new BinaryReader(Stream))
            {
                return BinaryReader.ReadBytes((int)Stream?.Length);
            }


        }

    }
}