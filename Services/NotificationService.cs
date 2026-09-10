using System;

namespace ShigosagNexusERP.Services;

public class NotificationService : INotificationService
{
    public event Action<string, NotificationType>? NotificationTriggered;

    public void Notify(string message, NotificationType type = NotificationType.Information)
    {
        NotificationTriggered?.Invoke(message, type);
    }
}
