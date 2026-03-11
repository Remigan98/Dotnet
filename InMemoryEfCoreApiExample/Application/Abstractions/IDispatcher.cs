using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstractions
{
    public interface IDispatcher
    {
        Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
        Task<TResult> Query<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    }
}
