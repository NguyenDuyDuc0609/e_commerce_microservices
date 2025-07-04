using MediatR;
using NotificationService.Application.Features.Dtos;
using NotificationService.Application.Features.Notification.Commands;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Features.Handler
{
    public class NotificationSendMailHandler(IUnitOfWork unitOfWork, INotificationStrategySelector notificationStrategySelector, INotificationStrategy notificationStrategy) : IRequestHandler<NotificationSendMailCommand, NotificationResult>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly INotificationStrategySelector _notificationStrategySelector = notificationStrategySelector;
        private readonly INotificationStrategy _notificationStrategy = notificationStrategy;

        public async Task<NotificationResult> Handle(NotificationSendMailCommand request, CancellationToken cancellationToken)
        {
            var notificationStrategy = _notificationStrategySelector.GetStrategy(request.NotificationMessage.Type);
            var result = await _notificationStrategy.SendAsync(request.NotificationMessage);
            if (!result.IsSuccess)
            {
                return new NotificationResult
                {
                    IsSuccess = false,
                    Message = result.Message
                };
            }

            var notificationSaved = await _unitOfWork.NotificationRepository!.AddNotificationAsync(
                request.NotificationMessage.UserId,
                request.NotificationMessage.Email,
                "Register email",
                request.NotificationMessage.HashEmail,
                NotificationType.RegisterEmail
            );
            if (!notificationSaved)
            {
                return new NotificationResult
                {
                    IsSuccess = false,
                    Message = "Failed to log notification"
                };
            }

            return new NotificationResult
            {
                IsSuccess = true,
                Message = "Notification sent successfully"
            };
        }
    }
}
