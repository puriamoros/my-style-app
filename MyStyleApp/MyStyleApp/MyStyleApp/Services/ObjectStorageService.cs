using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyStyleApp.Services
{
    public class ObjectStorageService<T>
    {
        private IFileStorageService _fileStorageService;
        private XmlSerializer _serializer;

        public ObjectStorageService(IFileStorageService fileStorageService)
        {
            this._fileStorageService = fileStorageService;
            this._serializer = new XmlSerializer(typeof(T));
        }

        public async Task DeleteFileAsync(string fileName)
        {
            await this._fileStorageService.DeleteFileAsync(fileName);
        }

        public async Task SaveToFileAsync(string fileName, T obj)
        {
            if (obj != null)
            {
                using (StringWriter sw = new StringWriter())
                {
                    this._serializer.Serialize(sw, obj);
                    await this._fileStorageService.WriteTextFileAsync(fileName, sw.ToString());
                }
            }
        }

        public async Task<T> LoadFromFileAsync(string fileName)
        {
            var obj = default(T);
            string content = await this._fileStorageService.ReadTextFileAsync(fileName);
            
            if (content.Length > 0)
            {
                using (StringReader sr = new StringReader(content))
                {
                    obj = (T)this._serializer.Deserialize(sr);
                }
            }

            return obj;
        }
    }
}
