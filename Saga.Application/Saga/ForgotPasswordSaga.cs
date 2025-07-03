using MassTransit;
using RegisterConstracts.Commands;
using RegisterConstracts.Events.ForgotPasswordEvents;
using SagaCoordinator.Domain.Constracts.SagaStates;
using SagaCoordinator.Domain.Constracts.StartSaga;
using SagaCoordinator.Domain.Constracts.UpdateStatus;
using SagaCoordinator.Domain.Enums;

namespace SagaCoordinator.Application.Saga
{
    public class ForgotPasswordSaga : MassTransitStateMachine<ForgotPasswordSagaState>
    {
        public State CreateTokenPending { get; private set; } = default!;
        public State SendMailPending { get; private set; } = default!;
        public State Rollback { get; private set; } = default!;

        public Event<StartForgotPasswordSagaCommand> Start { get; private set; } = default!;
        public Event<ForgotPasswordCommand> ForgotPasswordCommand { get; private set; } = default!;
        public Event<CreateTokenEvent> CreateTokenEvent { get; private set; } = default!;
        public Event<CreateTokenFailed> CreateTokenFailed { get; private set; } = default!;
        public Event<SendTokenEvent> SendTokenEvent { get; private set; } = default!;
        public Event<SendTokenFailed> SendTokenFailed { get; private set; } = default!;
        public Event<EndForgotSagaFailed> EndForgotSagaFailed { get; private set; } = default!;

        [Obsolete]
        public ForgotPasswordSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => Start, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => ForgotPasswordCommand, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => CreateTokenEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => CreateTokenFailed, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => SendTokenEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => SendTokenFailed, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => EndForgotSagaFailed, x => x.CorrelateById(context => context.Message.CorrelationId));

            Initially(
                When(Start)
                    .Then(context =>
                    {
                        context.Instance.CorrelationId = context.Data.CorrelationId;
                        context.Instance.Email = context.Data.Email;
                        context.Instance.CreatedAt = DateTime.UtcNow;
                    })
                    .ThenAsync(async context =>
                    {
                        await context.Publish(new UpdateStatusSaga(
                            context.Instance.CorrelationId,
                            TypeSaga.ForgotPassword,
                            StatusSaga.Pending,
                            "Send request to create token"
                        ));
                    })
                    .Send(new Uri("queue:create-token-queue"), context => new ForgotPasswordCommand
                    {
                        CorrelationId = context.Instance.CorrelationId,
                        Email = context.Instance.Email!
                    })
                    .TransitionTo(CreateTokenPending)
            );
            During(CreateTokenPending,
                When(CreateTokenEvent)
                    .ThenAsync(async context =>
                    {
                        context.Instance.Token = context.Data.Token;

                        await context.Publish(new UpdateStatusSaga(
                            context.Instance.CorrelationId,
                            TypeSaga.ForgotPassword,
                            StatusSaga.Pending,
                            "Token created successfully"
                        ));
                    })
                    .Send(new Uri("queue:send-token-queue"), context => new SendTokenCommand
                    {
                        CorrelationId = context.Instance.CorrelationId,
                        Email = context.Instance.Email!,
                        Token = context.Instance.Token!
                    })
                    .TransitionTo(SendMailPending),

                When(CreateTokenFailed)
                    .Then(context =>
                    {
                        context.Instance.ErrorMessage = context.Data.Message;
                    })
                    .ThenAsync(async context =>
                    {
                        await context.Publish(new UpdateStatusSaga(
                            context.Instance.CorrelationId,
                            TypeSaga.ForgotPassword,
                            StatusSaga.Failed,
                            "Failed to create token"
                        ));
                    })
                    .Finalize()
            );

            During(SendMailPending,
                When(SendTokenEvent)
                    .Then(context =>
                    {
                        context.Instance.IsTokenSent = true;
                    })
                    .ThenAsync(async context =>
                    {
                        await context.Publish(new UpdateStatusSaga(
                            context.Instance.CorrelationId,
                            TypeSaga.ForgotPassword,
                            StatusSaga.Completed,
                            "Token sent successfully"
                        ));
                    })
                    .Finalize(),
                When(SendTokenFailed)
                    .Then(context =>
                    {
                        context.Instance.ErrorMessage = context.Data.Message;
                    })
                    .ThenAsync(async context =>
                    {
                        await context.Publish(new UpdateStatusSaga(
                            context.Instance.CorrelationId,
                            TypeSaga.ForgotPassword,
                            StatusSaga.Failed,
                            "Failed to send token"
                        ));
                    })
                    .TransitionTo(Rollback)
            );
            During(Rollback,
                When(EndForgotSagaFailed)
                    .ThenAsync(async context =>
                    {
                        await context.Publish(new UpdateStatusSaga(
                            context.Instance.CorrelationId,
                            TypeSaga.ForgotPassword,
                            StatusSaga.Failed,
                            "Rollback completed"
                        ));
                    })
                    .Finalize()
            );
        }
    }
}
