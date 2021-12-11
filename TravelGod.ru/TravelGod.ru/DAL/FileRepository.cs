using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Models;
using File = TravelGod.ru.Models.File;

namespace TravelGod.ru.DAL
{
    public class FileRepository : GenericRepository<File>, IFileRepository
    {
        public FileRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<File> CreateFromFormFileAsync(IFormFile formFile, string wwwroot, string name = null)
        {
            var path = name is not null
                ? "CustomFiles/Avatars/" + name + Path.GetExtension(formFile.FileName).ToLowerInvariant()
                : "CustomFiles/Avatars/" + formFile.FileName;
            await using (var fileStream =
                new FileStream(Path.Combine(wwwroot, path), FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }

            var file = new File {Name = formFile.FileName, Path = path};
            Create(file);
            return file;
        }
    }
}