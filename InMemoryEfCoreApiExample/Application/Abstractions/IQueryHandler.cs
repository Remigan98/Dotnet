using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstractions
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(TQuery query, CancellationToken cancellationToken);
    }
}
