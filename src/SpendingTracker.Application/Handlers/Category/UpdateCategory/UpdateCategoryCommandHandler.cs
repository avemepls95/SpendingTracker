using SpendingTracker.Application.Handlers.Category.UpdateCategory.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Models.Request;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Category.UpdateCategory;

internal class UpdateCategoryCommandHandler : CommandHandler<UpdateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        await _categoryRepository.UpdateCategory(
            new UpdateCategoryModel
            {
                Id = command.Id,
                Title = command.Title.Trim()
            },
            cancellationToken);

        await _unitOfWork.SaveAsync(cancellationToken);
    }
}