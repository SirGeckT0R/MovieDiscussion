﻿using FluentValidation;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.RemoveUserConnectionCommand
{
    public class RemoveUserConnectionValidator : AbstractValidator<RemoveUserConnectionCommand>
    {
        public RemoveUserConnectionValidator()
        {
            RuleFor(x => x.ConnectionId)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.");
        }
    }
}
