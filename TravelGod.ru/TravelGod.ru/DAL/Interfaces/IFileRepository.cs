using Microsoft.AspNetCore.Http;
using TravelGod.ru.Models;

namespace TravelGod.ru.DAL.Interfaces
{
    public interface IFileRepository : IGenericRepository<File>
    {
        File CreateFromFormFile(IFormFile formFile, string wwwroot, string name);
    }
}