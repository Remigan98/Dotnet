using Application.Abstractions;
using Application.Categories.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Categories.Queries
{
    public sealed record GetCategoryByName(string name) : IQuery<CategoryDto>;
}
