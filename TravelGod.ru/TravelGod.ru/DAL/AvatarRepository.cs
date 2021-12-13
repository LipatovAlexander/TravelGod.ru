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

        public Avatar CreateFromFormFile(IFormFile formFile, string wwwroot, string name = null)
        {
            var fileRepo = new FileRepository(Context);
            var avatar = new Avatar
            {
                File = fileRepo.CreateFromFormFile(formFile, wwwroot, name)
            };
            return avatar;
        }
    }
}