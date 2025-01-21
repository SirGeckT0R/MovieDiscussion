using FluentValidation;
using MovieServiceDomain.Enums;

namespace MovieServiceApplication.UseCases.Movies.Commands.UpdateMovieCommand
{
    public class UpdateMovieValidator : AbstractValidator<UpdateMovieCommand>
    {
        public UpdateMovieValidator() {
            RuleFor(x => x.Id)
                .NotEmpty();

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
