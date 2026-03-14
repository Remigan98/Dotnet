using Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Categories.Commands.Delete
{
    public sealed record DeleteCategoryCommand(int id) : ICommand<bool>;
}
