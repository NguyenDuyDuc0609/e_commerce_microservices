using MassTransit;
using RegisterConstracts.Commands;
using RegisterConstracts.Events;
using SagaCoordinator.Domain.Constracts.SagaStates;
using SagaCoordinator.Domain.Constracts.StartSaga;
using SagaCoordinator.Domain.Constracts.UpdateStatus;
using SagaCoordinator.Domain.Enums;

namespace SagaCoordinator.Application.Saga
{
    public class RegisterSaga : MassTransitStateMachine<RegisterSagaState>
    {
        public State DatabasePending { get; private set; } = null!;
        public State NotificationPending { get; private set; } = null!;
        public State Rollback { get; private set; } = null!;

        public Event<StartRegisterSagaCommand> RegisterCommand { get; private set; } = null!;
        public Event<NotificationRegisterCommand> RegisterNotification { get; private set; } = null!;
        public Event<DeleteRegisterCommand> DeleteRegisterCommand { get; private set; } = null!;
        public Event<NotificationEvent> NotificationSuccess { get; private set; } = null!;
        public Event<UserCreationFailedEvent> UserCreationFailed { get; private set; } = null!;
        public Event<UserCreatedEvent> UserCreatedEvent { get; private set; } = null!;
        public Event<NotificationFailed> NotificationFailed { get; private set; } = null!;
        public Event<UserDeletedEvent> UserDeletedEvent { get; private set; } = null!;
        public Event<TimeoutCheckCommand> TimeoutCheckCommand { get; private set; } = null!;

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
                        context.Instance.ExpireAt = DateTime.UtcNow.AddSeconds(30);
                    })
                    .ThenAsync(async context =>
                    {
                        await context.Publish(new UpdateStatusSaga(
                            context.Data.CorrelationId,
                            TypeSaga.Register,
                            StatusSaga.Pending,
                            "Registering send from saga user"
                        ));
                    })
                    .Send(new Uri("queue:register-queue"), context => new RegisterUserCommand
                    {
                        CorrelationId = context.Data.CorrelationId,
                        Username = context.Data.Username,
                        Email = context.Data.Email,
                        PasswordHash = context.Data.PasswordHash,
                        PhoneNumber = context.Data.PhoneNumber,
                        Address = context.Data.Address
                    })
                    .TransitionTo(DatabasePending)
            );

            During(DatabasePending,
                When(TimeoutCheckCommand)
                    .If(context => context.Instance.ExpireAt.HasValue &&
                                   DateTime.UtcNow >= context.Instance.ExpireAt.Value,
                        binder => binder
                            .ThenAsync(async context => {
                                await context.Publish(new UpdateStatusSaga(
                                    context.Data.CorrelationId,
                                    TypeSaga.Register,
                                    StatusSaga.Failed,
                                    "Auth service down, resgister failed"
                            ));
                        })
                    .Finalize()
                    ),
                When(UserCreatedEvent)
                    .Then(context =>
                    {
                        context.Instance.UserId = context.Data.UserId;
                        context.Instance.HashEmail = context.Data.HashEmail;
                    })
                    .ThenAsync(async context =>
                    {
                        await context.Publish(new UpdateStatusSaga(
                            context.Data.CorrelationId,
                            TypeSaga.Register,
                            StatusSaga.Pending,
                            "User created successfully, sending notification"
                        ));
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
                    .ThenAsync(async context =>
                    {
                        await context.Publish(new UpdateStatusSaga(
                            context.Data.CorrelationId,
                            TypeSaga.Register,
                            StatusSaga.Failed,
                            context.Data.Message
                        ));
                    })
                    .Finalize()
            );

            During(NotificationPending,
                When(TimeoutCheckCommand)
                    .If(context => context.Instance.ExpireAt.HasValue &&
                                   DateTime.UtcNow >= context.Instance.ExpireAt.Value,
                        binder => binder
                            .ThenAsync(async context => {
                                await context.Publish(new UpdateStatusSaga(
                                    context.Data.CorrelationId,
                                    TypeSaga.Register,
                                    StatusSaga.Failed,
                                    "Notification service down, register failed"
                            ));
                            })
                            .Send(new Uri("queue:register-delete-queue"), context => new DeleteRegisterCommand
                            {
                                CorrelationId = context.Data.CorrelationId,
                                UserId = context.Instance.UserId ?? Guid.Empty,
                            })
                            .TransitionTo(Rollback)
                    ),
                When(NotificationSuccess)
                    .ThenAsync(async context =>
                    {
                        await context.Publish(new UpdateStatusSaga(
                            context.Data.CorrelationId,
                            TypeSaga.Register,
                            StatusSaga.Completed,
                            context.Data.Message
                        ));
                    })
                    .Finalize(),

                When(NotificationFailed)
                    .ThenAsync(async context =>
                    {
                        await context.Publish(new UpdateStatusSaga(
                            context.Data.CorrelationId,
                            TypeSaga.Register,
                            StatusSaga.Failed,
                            context.Data.Message
                        ));
                    })
                    .Send(new Uri("queue:register-delete-queue"), context => new DeleteRegisterCommand
                    {
                        CorrelationId = context.Data.CorrelationId,
                        UserId = context.Instance.UserId ?? Guid.Empty,
                    })
                    .TransitionTo(Rollback)
            );

            During(Rollback,
                When(UserDeletedEvent)
                    .ThenAsync(async context =>
                    {
                        await context.Publish(new UpdateStatusSaga(
                            context.Data.CorrelationId,
                            TypeSaga.Register,
                            StatusSaga.Failed,
                            "User has been removed"
                        ));
                    })
                    .Finalize()
            );
        }
    }
}
