using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TravelGod.ru.Models;

namespace TravelGod.ru.DAL.Interfaces
{
    public interface IFileRepository : IGenericRepository<File>
    {
        Task<File> CreateFromFormFileAsync(IFormFile formFile, string wwwroot, string name);
    }
}