using System.Threading.Tasks;

namespace MyStyleApp.Services
{
    public interface IFileStorageService
    {
        Task WriteTextFileAsync(string fileName, string content);
        Task<string> ReadTextFileAsync(string fileName);
        Task DeleteFileAsync(string fileName);
    }
}
