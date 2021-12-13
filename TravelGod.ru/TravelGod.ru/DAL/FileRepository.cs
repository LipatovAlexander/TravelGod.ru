using System.IO;
using Microsoft.AspNetCore.Http;
using TravelGod.ru.DAL.Interfaces;
using File = TravelGod.ru.Models.File;

namespace TravelGod.ru.DAL
{
    public class FileRepository : GenericRepository<File>, IFileRepository
    {
        public FileRepository(ApplicationContext context) : base(context)
        {
        }

        public File CreateFromFormFile(IFormFile formFile, string wwwroot, string name)
        {
            byte[] imageData;
            using (var binaryReader = new BinaryReader(formFile.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int) formFile.Length);
            }

            var file = new File {Name = formFile.FileName, BinaryData = imageData};
            Create(file);
            return file;
        }
    }
}