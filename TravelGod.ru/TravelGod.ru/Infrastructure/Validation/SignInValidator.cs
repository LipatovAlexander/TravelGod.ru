using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Infrastructure.Validation.Interfaces;
using TravelGod.ru.Models;
using TravelGod.ru.ViewModels;

namespace TravelGod.ru.Infrastructure.Validation
{
    public class SignInValidator : IValidator<SignInModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SignInValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ModelValidationResult> Validate(SignInModel instance)
        {
            var user = _unitOfWork.Users.FirstOrDefault(u => u.Login == instance.Login);
            if (user is null)
            {
                yield return new ModelValidationResult(nameof(instance.Password), "Неправильный логин или пароль.");
                yield break;
            }

            var actualPasswordHash = Cryptography.Cryptography.ComputeMd5HashString(instance.Password + user.PasswordSalt);
            if (actualPasswordHash != user.PasswordHash)
            {
                yield return new ModelValidationResult(nameof(instance.Password), "Неправильный логин или пароль.");
                yield break;
            }

            if (user.Status is not Status.Normal)
            {
                yield return new ModelValidationResult(nameof(instance.Login), "Аккаунт заблокирован.");
            }
        }
    }
}