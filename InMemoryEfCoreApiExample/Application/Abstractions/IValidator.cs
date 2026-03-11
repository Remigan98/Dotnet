using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstractions
{
    public interface IValidator<T>
    {
        void Validate(T instance);
    }
}
