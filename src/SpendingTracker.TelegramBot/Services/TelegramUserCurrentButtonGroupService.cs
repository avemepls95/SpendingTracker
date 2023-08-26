using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.TelegramBot.Internal.Abstractions;
using SpendingTracker.TelegramBot.Internal.Buttons;
using SpendingTracker.TelegramBot.Services.Abstractions;

namespace SpendingTracker.TelegramBot.Services;

internal class TelegramUserCurrentButtonGroupService : ITelegramUserCurrentButtonGroupService
{
    private readonly ITelegramUserCurrentButtonGroupRepository _telegramUserCurrentButtonGroupRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IButtonsGroupManager _buttonsGroupManager;

    public TelegramUserCurrentButtonGroupService(
        ITelegramUserCurrentButtonGroupRepository telegramUserCurrentButtonGroupRepository,
        IUnitOfWork unitOfWork,
        IButtonsGroupManager buttonsGroupManager)
    {
        _telegramUserCurrentButtonGroupRepository = telegramUserCurrentButtonGroupRepository;
        _unitOfWork = unitOfWork;
        _buttonsGroupManager = buttonsGroupManager;
    }

    public async Task<ButtonGroup> GetGroupByUserId(long id, CancellationToken cancellationToken)
    {
        var groupId = await _telegramUserCurrentButtonGroupRepository.GetIdByUserId(id, cancellationToken);
        var group = await _buttonsGroupManager.GetById(groupId);
        return group;
    }

    public async Task Update(long userId, ButtonGroup newGroup, CancellationToken cancellationToken)
    {
        await _telegramUserCurrentButtonGroupRepository.CreateOrUpdate(userId, newGroup.Id, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}