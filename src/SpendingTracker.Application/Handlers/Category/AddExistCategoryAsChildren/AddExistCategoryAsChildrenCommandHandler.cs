using FluentValidation;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Category.AddExistCategoryAsChildren;

internal class AddExistCategoryAsChildrenCommandHandler : CommandHandler<Contracts.AddExistCategoryAsChildrenCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddExistCategoryAsChildrenCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(
        Contracts.AddExistCategoryAsChildrenCommand command,
        CancellationToken cancellationToken)
    {
        var parentCategory = await _categoryRepository.GetById(command.ParentId, cancellationToken);
        var parentCategoryUserId = parentCategory.OwnerId;
        
        var childCategory = await _categoryRepository.GetById(command.ChildId, cancellationToken);
        if (childCategory.OwnerId != parentCategoryUserId)
        {
            throw new ValidationException("Дочерния категория принадлежит другому пользователю");
        }

        await EnsureValidDependencies(command.UserId, parentCategory, childCategory, cancellationToken);

        await _categoryRepository.AddChildCategory(parentCategory, childCategory, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    private async Task EnsureValidDependencies(
        UserKey userId,
        Domain.Categories.Category parent,
        Domain.Categories.Category child,
        CancellationToken cancellationToken)
    {
        var categoriesTree = await _categoryRepository.GetUserCategoriesTree(userId, cancellationToken);
        foreach (var firstLevelCategoryNode in categoriesTree)
        {
            if (firstLevelCategoryNode.Id == child.Id)
            {
                continue;
            }

            var nodeInBranch = FindInTreeBranch(parent, firstLevelCategoryNode);
            if (nodeInBranch is null)
            {
                continue;
            }

            var childHasParent = ChildHasParent(nodeInBranch, child);
            if (childHasParent)
            {
                throw new ValidationException("Указанная родительская категория является дочерней");
            }
        }

        return;

        Domain.Categories.Category? FindInTreeBranch(
            Domain.Categories.Category targetCategory,
            Domain.Categories.Category branchRoot)
        {
            if (branchRoot.Id == targetCategory.Id)
            {
                return branchRoot;
            }

            foreach (var parent in branchRoot.Parents)
            {
                return FindInTreeBranch(targetCategory, parent);
            }

            return null;
        }

        bool ChildHasParent(Domain.Categories.Category child, Domain.Categories.Category parent)
        {
            if (child.Parents.Any(p => p.Id == parent.Id))
            {
                return true;
            }

            foreach (var nextParent in child.Parents)
            {
                return ChildHasParent(nextParent, parent);
            }

            return false;
        }
    } 
}