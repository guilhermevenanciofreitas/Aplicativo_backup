using Aplicativo.View.Services;
using System;
using System.Threading.Tasks;

namespace Aplicativo.Browser.Services
{
    public class Camera : ICamera
    {
        public Task<byte[]> TakePhotoAsync()
        {
            throw new NotImplementedException();
        }
    }
}