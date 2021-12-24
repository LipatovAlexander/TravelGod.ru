using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TravelGod.ru.Infrastructure.Validation.Interfaces
{
    public interface IValidator<T>
    {
        IEnumerable<ModelValidationResult> Validate(T instance);
    }
}