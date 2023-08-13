using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.TelegramBot.Buttons;
using SpendingTracker.TelegramBot.Services.Abstractions;

namespace SpendingTracker.TelegramBot.Services;

internal class TelegramUserCurrentButtonGroupService : ITelegramUserCurrentButtonGroupService
{
    private readonly ITelegramUserCurrentButtonGroupRepository _telegramUserCurrentButtonGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TelegramUserCurrentButtonGroupService(
        ITelegramUserCurrentButtonGroupRepository telegramUserCurrentButtonGroupRepository,
        IUnitOfWork unitOfWork)
    {
        _telegramUserCurrentButtonGroupRepository = telegramUserCurrentButtonGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ButtonGroup> GetGroupByUserId(long id, CancellationToken cancellationToken)
    {
        var groupId = await _telegramUserCurrentButtonGroupRepository.GetIdByUserId(id, cancellationToken);
        return ButtonsGroupManager.GetInstance().GetById(groupId);
    }

    public async Task Update(long userId, ButtonGroup newGroup, CancellationToken cancellationToken)
    {
        await _telegramUserCurrentButtonGroupRepository.CreateOrUpdate(userId, newGroup.Id, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}