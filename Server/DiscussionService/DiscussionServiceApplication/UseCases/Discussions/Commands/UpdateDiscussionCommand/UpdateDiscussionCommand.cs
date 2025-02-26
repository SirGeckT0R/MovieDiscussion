﻿using DiscussionServiceApplication.Interfaces.UseCases;
using MediatR;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.UpdateDiscussionCommand
{
    public record UpdateDiscussionCommand(Guid Id, string Title, string Description, Guid UpdatedBy) : ICommand<Unit>;
}
