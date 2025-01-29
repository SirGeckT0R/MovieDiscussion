using FluentValidation;
using MovieServiceDomain.Enums;

namespace MovieServiceApplication.UseCases.Movies.Commands.UpdateMovieCommand
{
    public class UpdateMovieValidator : AbstractValidator<UpdateMovieCommand>
    {
        public UpdateMovieValidator() {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Movie's {PropertyName} is required");

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Movie's {PropertyName} is empty or too long");

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Movie's {PropertyName} is empty or too long");

            RuleFor(x => x.ReleaseDate)
                .NotEmpty()
                .Must(x => x < DateTime.Now)
                .WithMessage("Movie's {PropertyName} is required");

            RuleFor(x => x.Genres)
                .NotEmpty()
                .WithMessage("Movie has to have at least one genre");

            RuleFor(x => x.CrewMembers)
                .NotEmpty()
                .Must(x => x.All(y => Enum.IsDefined(typeof(CrewRole), y.Role)))
                .WithMessage("Movie has to have at least one crew member");
        }
    }
}
