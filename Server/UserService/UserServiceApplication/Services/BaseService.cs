using FluentValidation;

namespace UserServiceApplication.Services
{
    public abstract class BaseService<T>(IValidator<T> validator) where T : class
    {
        private readonly IValidator<T> _validator = validator;

        public void Validate(T model)
        {
            var errorsList = _validator.Validate(model)
                                        .Errors
                                        .Where(x => x != null)
                                        .ToList();
            if (errorsList.Count != 0)
            {
                throw new ValidationException(errorsList);
            }
        }
    }
}
