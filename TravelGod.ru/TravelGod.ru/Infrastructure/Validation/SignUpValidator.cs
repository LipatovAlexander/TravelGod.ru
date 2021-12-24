using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Infrastructure.Validation.Interfaces;
using TravelGod.ru.ViewModels;

namespace TravelGod.ru.Infrastructure.Validation
{
    public class SignUpValidator : IValidator<SignUpModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SignUpValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ModelValidationResult> Validate(SignUpModel instance)
        {
            if (_unitOfWork.Users.FirstOrDefault(u => u.Login == instance.Login) is not null)
            {
                yield return new ModelValidationResult(nameof(instance.Login), "Логин уже занят.");
            }
        }
    }
}