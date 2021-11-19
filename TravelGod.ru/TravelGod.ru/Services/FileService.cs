using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TravelGod.ru.Models;
using File = TravelGod.ru.Models.File;

namespace TravelGod.ru.Services
{
    public class FileService
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ApplicationContext _context;

        public FileService(ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<File> GetFileAsync(int id)
        {
            return await _context.Files.FindAsync(id);
        }

        public async Task AddFileAsync(File file)
        {
            _context.Files.Add(file);
            await _context.SaveChangesAsync();
        }

        public async Task<File> AddFileAsync(User user, IFormFile formFile)
        {
            var path = "CustomFiles/Avatars/" + user.Id +
                       Path.GetExtension(formFile.FileName).ToLowerInvariant();
            await using (var fileStream =
                new FileStream(Path.Combine(_appEnvironment.WebRootPath, path), FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }

            var file = new File {Name = formFile.FileName, Path = path};
            _context.Files.Add(file);
            await _context.SaveChangesAsync();
            return file;
        }
    }
}