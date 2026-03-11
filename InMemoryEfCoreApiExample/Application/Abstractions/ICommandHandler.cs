using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstractions
{
    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult> 
    {
        Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
    }
}
