using MediatR;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Reviews.Commands.UpdateReviewCommand
{
    public record UpdateReviewCommand : ICommand<Unit>
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
        public string Text { get; set; } = string.Empty;

        public UpdateReviewCommand()
        {
        }

        public UpdateReviewCommand(Guid id, int value, string text)
        {
            Id = id;
            Value = value;
            Text = text;
        }
    }
}
