using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace TravelGod.ru.Infrastructure.Validation
{
    class ValidationProvider : IModelValidatorProvider
    {
        private readonly IServiceProvider _provider;
        public ValidationProvider(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void CreateValidators(ModelValidatorProviderContext ctx)
        {
            try
            {
                var validatorType = typeof(ModelValidator<>)
                    .MakeGenericType(ctx.ModelMetadata.ModelType);
                var validator =
                    (IModelValidator)_provider.GetRequiredService(validatorType);
                ctx.Results.Add(new ValidatorItem { Validator = validator });
            }
            catch
            {
                // ignored
            }
        }
    }
}