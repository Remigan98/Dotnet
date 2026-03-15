using Application.Abstractions;
using Application.Categories.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Categories.Commands.Update
{
    public sealed record UpdateCategoryCommand(int Id, CategoryDto CategoryDto) : ICommand<CategoryDto>;
}
