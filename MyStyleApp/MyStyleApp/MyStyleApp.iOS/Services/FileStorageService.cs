using MyStyleApp.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(MyStyleApp.iOS.Services.FileStorageService))]
namespace MyStyleApp.iOS.Services
{
    public class FileStorageService: IFileStorageService
    {
        private string _filesFolder;

        public FileStorageService()
        {
            // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
            // (they don't want non-user-generated data in Documents)
            //string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            //string _filesFolder = Path.Combine(documentsPath, "../Library/"); // Library folder
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
