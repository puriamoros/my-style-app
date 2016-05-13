using System;
using Xamarin.Forms;
using MyStyleApp.Services;
using System.Threading.Tasks;
using System.IO;

[assembly: Dependency(typeof(MyStyleApp.Droid.Services.FileStorageService))]
namespace MyStyleApp.Droid.Services
{
    public class FileStorageService : IFileStorageService
    {
        private string _filesFolder;

        public FileStorageService()
        {
            this._filesFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        public async Task<string> ReadTextFileAsync(string fileName)
        {
            var filePath = Path.Combine(this._filesFolder, fileName);
            return File.ReadAllText(filePath);
        }

        public async Task WriteTextFileAsync(string fileName, string content)
        {
            var filePath = Path.Combine(this._filesFolder, fileName);
            File.WriteAllText(filePath, content);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(this._filesFolder, fileName);
            File.Delete(filePath);
        }
    }
}