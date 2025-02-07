using FluentValidation;
using Microsoft.Extensions.Logging;

namespace UserServiceApplication.Services
{
    public abstract class BaseService<T>(IValidator<T> validator, ILogger<BaseService<T>> logger) where T : class
    {
        private readonly IValidator<T> _validator = validator;
        private readonly ILogger<BaseService<T>> _logger = logger;

        public void Validate(T model)
        {
            var errorsList = _validator.Validate(model)
                                        .Errors
                                        .Where(x => x != null)
                                        .ToList();
            if (errorsList.Count != 0)
            {
                _logger.LogError("Validation failed: {ValidationResults}", string.Join(",", errorsList.Select(x => x.ErrorMessage)));

                throw new ValidationException(errorsList);
            }
        }
    }
}
