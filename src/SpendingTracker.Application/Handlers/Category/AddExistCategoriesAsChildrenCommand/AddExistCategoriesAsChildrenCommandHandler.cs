using FluentValidation;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Category.AddExistCategoriesAsChildrenCommand;

internal class AddExistCategoriesAsChildrenCommandHandler : CommandHandler<Contracts.AddExistCategoriesAsChildrenCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddExistCategoriesAsChildrenCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(Contracts.AddExistCategoriesAsChildrenCommand command, CancellationToken cancellationToken)
    {
        var parentCategory = await _categoryRepository.GetById(command.ParentId, cancellationToken);
        var parentCategoryUserId = parentCategory.OwnerId;
        
        var childCategories = await _categoryRepository.GetByIds(command.ChildIds, cancellationToken);
        var childCategoriesWithAnotherUser = childCategories.Where(c => c.OwnerId != parentCategoryUserId).ToArray();
        if (childCategoriesWithAnotherUser.Any())
        {
            var childCategoryIdsWithAnotherUserAsString = string.Join(" ,", childCategoriesWithAnotherUser.Select(c => c.Id));
            throw new ValidationException("Некоторые из дочерних категорий принадлежат другому пользователю. " +
                                          $"Идентификаторы: {childCategoryIdsWithAnotherUserAsString}");
        }

        await _categoryRepository.AddChildrenCategories(parentCategory, childCategories, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}