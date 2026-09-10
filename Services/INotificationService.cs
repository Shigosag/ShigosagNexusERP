using System;

namespace ShigosagNexusERP.Services;

public enum NotificationType
{
    Information,
    Success,
    Warning,
    Error
}

public interface INotificationService
{
    event Action<string, NotificationType>? NotificationTriggered;
    void Notify(string message, NotificationType type = NotificationType.Information);
}
