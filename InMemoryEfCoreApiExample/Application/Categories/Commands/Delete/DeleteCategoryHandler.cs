using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Common.Exceptions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Categories.Commands.Delete
{
    public sealed class DeleteCategoryHandler : ICommandHandler<DeleteCategoryCommand, bool>
    {
        ICategoryRepository _categoryRepository;
        IUnitOfWork _unitOfWork;

        public DeleteCategoryHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            this._categoryRepository = categoryRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
        {
            Category? category = await _categoryRepository.GetByIdAsync(command.id, cancellationToken);

            if (category is null)
            {
                throw new NotFoundException($"Category with id {command.id} not found.");
            }

            await _categoryRepository.DeleteAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
