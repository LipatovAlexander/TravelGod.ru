using Microsoft.AspNetCore.Http;
using TravelGod.ru.Models;

namespace TravelGod.ru.DAL.Interfaces
{
    public interface IAvatarRepository : IGenericRepository<Avatar>
    {
        Avatar CreateFromFormFile(IFormFile formFile, string wwwroot, string name = null);
    }
}