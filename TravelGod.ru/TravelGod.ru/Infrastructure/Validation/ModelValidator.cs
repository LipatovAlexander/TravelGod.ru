using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using TravelGod.ru.Infrastructure.Validation.Interfaces;

namespace TravelGod.ru.Infrastructure.Validation
{
    class ModelValidator<TModel> : IModelValidator
    {
        private readonly IEnumerable<IValidator<TModel>> _validators;
        public ModelValidator(IEnumerable<IValidator<TModel>> validators) =>
            this._validators = validators;

        public IEnumerable<ModelValidationResult> Validate(
            ModelValidationContext ctx)
        {
            if (ctx.Model is TModel model)
                return Validate(model);
            return System.Array.Empty<ModelValidationResult>();
        }

        private IEnumerable<ModelValidationResult> Validate(TModel model) =>
            from validator in this._validators
            from validationResult in validator.Validate(model)
            select validationResult;
    }
}