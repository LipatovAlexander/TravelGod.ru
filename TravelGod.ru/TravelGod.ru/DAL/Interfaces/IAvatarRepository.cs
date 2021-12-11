using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TravelGod.ru.Models;

namespace TravelGod.ru.DAL.Interfaces
{
    public interface IAvatarRepository : IGenericRepository<Avatar>
    {
        Task<Avatar> CreateFromFormFileAsync(IFormFile formFile, string wwwroot, string name = null);
    }
}