using MyStyleApp.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(MyStyleApp.WinPhone.Services.FileStorageService))]
namespace MyStyleApp.WinPhone.Services
{
    public class FileStorageService : IFileStorageService
    {
        private StorageFolder _filesFolder;
        
        public FileStorageService()
        {
            this._filesFolder = ApplicationData.Current.LocalFolder;
        }

        public async Task<string> ReadTextFileAsync(string fileName)
        {
            var file = await this._filesFolder.GetFileAsync(fileName);
            return await FileIO.ReadTextAsync(file);
        }

        public async Task WriteTextFileAsync(string fileName, string content)
        {
            var file = await this._filesFolder.CreateFileAsync(
                fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, content);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var file = await this._filesFolder.GetFileAsync(fileName);
            await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
        }
    }
}
