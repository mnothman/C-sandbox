namespace TaskManagerSandbox.Services;

public interface INotificationService
{
    Task SendTaskAssignedNotificationAsync(string taskTitle, string assignedTo, string assignedBy);
    Task SendTaskCompletedNotificationAsync(string taskTitle, string completedBy);
    Task SendTaskOverdueNotificationAsync(string taskTitle, string assignedTo);
    Task SendWelcomeNotificationAsync(string username, string email);
} 