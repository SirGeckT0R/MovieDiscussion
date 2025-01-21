using FluentValidation;
using MovieServiceDomain.Enums;

namespace MovieServiceApplication.UseCases.Movies.Commands.AddMovieCommand
{
    public class AddMovieValidator : AbstractValidator<AddMovieCommand>
    {
        public AddMovieValidator() {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.ReleaseDate)
                .NotEmpty()
                .Must(x => x < DateTime.Now);

            RuleFor(x => x.Genres)
                .NotEmpty();

            RuleFor(x => x.CrewMembers)
                .NotEmpty()
                .Must(x => x.All(y => Enum.IsDefined(typeof(Role), y.Role)));
        }
    }
}
