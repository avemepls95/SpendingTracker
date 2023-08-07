using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace SpendingTracker.Dispatcher.DataTransfer.Dispatcher
{
    /// <summary>
    /// Абстрактный класс для реализации Command Handler'a используемого
    /// для обработки комманд медиатра
    /// Пример использования:
    /// medportal-integration-service\src\MedPortalIntegration.Handlers\EducationModule\Commands\ManipulateEducationModule\ManipulateEducationModuleCommandHandler.cs
    /// </summary>
    /// <typeparam name="TCommand">Тип обрабатываемой команды</typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class CommandHandler<TCommand> : IRequestHandler<CommandWrapper<TCommand>>
    {
        public async Task Handle(CommandWrapper<TCommand> request, CancellationToken cancellationToken)
        {
            await Handle(request.Command, cancellationToken);
        }

        public abstract Task Handle(TCommand command, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Абстрактный класс для реализации Command Handler'a используемого
    /// для обработки комманд медиатра
    /// Отличается тем что возвращает значнение типа TResult
    /// Пример использования:
    /// medportal-integration-service\src\MedPortalIntegration.Handlers\EducationModule\Commands\ManipulateEducationModule\ManipulateEducationModuleCommandHandler.cs
    /// </summary>
    /// <typeparam name="TCommand">Тип обрабатываемой команды</typeparam>
    [ExcludeFromCodeCoverage]
    public abstract class CommandHandler<TCommand, TResult> :
        IRequestHandler<CommandWrapper<TCommand, TResult>, TResult>
    {
        public async Task<TResult> Handle(CommandWrapper<TCommand, TResult> request, CancellationToken cancellationToken)
        {
            return await Handle(request.Command, cancellationToken);
        }

        public abstract Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
    }
}