using System.Threading.Tasks;
using TravelGod.ru.Models;

namespace TravelGod.ru.Services
{
    public class FileService
    {
        private readonly ApplicationContext _context;

        public FileService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<File> GetFileAsync(int id)
        {
            return await _context.Files.FindAsync(id);
        }

        public async Task<int> AddFileAsync(File file)
        {
            _context.Files.Add(file);
            return await _context.SaveChangesAsync();
        }
    }
}