using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Models;

namespace TravelGod.ru.DAL
{
    public class AvatarRepository : GenericRepository<Avatar>, IAvatarRepository
    {
        public AvatarRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<Avatar> CreateFromFormFileAsync(IFormFile formFile, string wwwroot, string name = null)
        {
            var fileRepo = new FileRepository(Context);
            var avatar = new Avatar
            {
                File = await fileRepo.CreateFromFormFileAsync(formFile, wwwroot, name)
            };
            return avatar;
        }
    }
}