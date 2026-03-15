using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Categories.Dtos
{
    public sealed record CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;

        public CategoryDto() { }

        public CategoryDto(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public CategoryDto(CategoryDto copy)
        {
            ArgumentNullException.ThrowIfNull(copy);

            Id = copy.Id;
            Name = copy.Name;
            Description = copy.Description;
        }

        public CategoryDto(Category category)
        {
            ArgumentNullException.ThrowIfNull(category);

            Id = category.Id;
            Name = category.Name;
            Description = category.Description;
        }
    }
}
