using MassTransit;
using SagaCoordinator.Application.Dtos;
using SagaCoordinator.Domain.Constracts.Register;
using SagaCoordinator.Domain.Constracts.SagaStates;

namespace SagaCoordinator.Application.Saga
{
    public class RegisterSaga : MassTransitStateMachine<RegisterSagaState>
    {
        public State DatabasePending { get; private set; } = null!;
        public State NotificationPending { get; private set; } = null!;
        public State Rollback { get; private set; } = null!;

        public Event<RegisterUserCommand> RegisterCommand { get; private set; } = null!;
        public Event<NotificationRegisterCommand> RegisterNotification { get; private set; } = null!;
        public Event<DeleteRegisterCommand> DeleteRegisterCommand { get; private set; } = null!;
        public Event<NotificationSuccess> NotificationSuccess { get; private set; } = null!;
        public Event<UserCreationFailedEvent> UserCreationFailed { get; private set; } = null!;
        public Event<UserCreatedEvent> UserCreatedEvent { get; private set; } = null!;
        public Event<NotificationFailed> NotificationFailed { get; private set; } = null!;

        [Obsolete]
        public RegisterSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => RegisterCommand, x => x.CorrelateById(m => m.Message.CorrelationId));
            Event(() => RegisterNotification, x => x.CorrelateById(m => m.Message.CorrelationId));
            Event(() => DeleteRegisterCommand, x => x.CorrelateById(m => m.Message.CorrelationId));
            Event(() => UserCreatedEvent, x => x.CorrelateById(m => m.Message.CorrelationId));
            Event(() => UserCreationFailed, x => x.CorrelateById(m => m.Message.CorrelationId));
            Event(() => NotificationSuccess, x => x.CorrelateById(m => m.Message.CorrelationId));
            Event(() => NotificationFailed, x => x.CorrelateById(m => m.Message.CorrelationId));

            Initially(
                When(RegisterCommand)
                    .Then(context =>
                    {
                        context.Instance.CreatedAt = DateTime.UtcNow;
                        context.Instance.CorrelationId = context.Data.CorrelationId;
                        context.Instance.Username = context.Data.Username;
                        context.Instance.Email = context.Data.Email;
                        context.Instance.PhoneNumber = context.Data.PhoneNumber;
                        context.Instance.Address = context.Data.Address;
                    })
                    .Send(new Uri("queue:register-queue"), context => context.Data)
                    .TransitionTo(DatabasePending)
            );

            During(DatabasePending,
                When(UserCreatedEvent)
                    .Then(context =>
                    {
                        context.Instance.UserId = context.Data.UserId;
                        context.Instance.HashEmail = context.Data.HashEmail;
                    })
                    .Send(new Uri("queue:register-sendmail-queue"), context => new NotificationRegisterCommand
                    {
                        CorrelationId = context.Data.CorrelationId,
                        UserId = context.Data.UserId,
                        Username = context.Instance.Username,
                        Email = context.Instance.Email,
                        HashEmail = context.Data.HashEmail
                    })
                    .TransitionTo(NotificationPending),

                When(UserCreationFailed)
                    .Then(async context =>
                    {
                        await context.RespondAsync(new MessageResult
                        {
                            Message = context.Data.Message,
                            IsSuccess = false
                        });
                    })
                    .Finalize()
            );

            During(NotificationPending,
                When(NotificationSuccess)
                    .Then(async context =>
                    {
                        await context.RespondAsync(new MessageResult
                        {
                            Message = "Đăng ký thành công",
                            IsSuccess = true
                        });
                    })
                    .Finalize(),

                When(NotificationFailed)
                    .Then(async context =>
                    {
                        await context.RespondAsync(new MessageResult
                        {
                            Message = context.Data.Message,
                            IsSuccess = false
                        });
                    })
                    .Send(new Uri("queue:register-delete-queue"), context => new DeleteRegisterCommand
                    {
                        CorrelationId = context.Data.CorrelationId,
                        UserId = context.Instance.UserId ?? Guid.Empty,
                    })
                    .TransitionTo(Rollback)
            );

            During(Rollback,
                When(DeleteRegisterCommand)
                    .Then(async context =>
                    {
                        await context.RespondAsync(new MessageResult
                        {
                            Message = "Đăng ký thất bại, đã xóa dữ liệu",
                            IsSuccess = false
                        });
                    })
                    .Finalize()
            );
        }
    }
}
