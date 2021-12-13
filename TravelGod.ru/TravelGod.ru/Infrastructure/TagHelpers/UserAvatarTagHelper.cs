using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Models;

namespace TravelGod.ru.Infrastructure.TagHelpers
{
    [HtmlTargetElement("user-avatar", Attributes = "asp-user", TagStructure = TagStructure.WithoutEndTag)]
    public class UserAvatarTagHelper : TagHelper
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserAvatarTagHelper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User AspUser { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagOnly;
            output.TagName = "img";
            output.Attributes.SetAttribute("style", "object-fit: cover");
            output.Attributes.SetAttribute("alt", "аватарка");

            if (AspUser.Avatar?.File is null)
            {
                var defaultAvatar = _unitOfWork.Files.FindById(1);
                AspUser.Avatar = new Avatar
                {
                    File = defaultAvatar,
                    FileId = defaultAvatar.Id,
                    User = AspUser,
                    UserId = AspUser.Id
                };
            }

            if (AspUser.Avatar.File is not null)
            {
                output.Attributes.SetAttribute("src",
                    $"data:image/jpeg;base64,{Convert.ToBase64String(AspUser.Avatar.File.BinaryData)}");
                output.Attributes.SetAttribute("title", AspUser.Avatar.File.Name);
            }
        }
    }
}