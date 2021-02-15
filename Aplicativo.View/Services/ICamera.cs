using System.Threading.Tasks;

namespace Aplicativo.View.Services
{

    public interface ICamera
    {
        Task<byte[]> TakePhotoAsync();
    }

}