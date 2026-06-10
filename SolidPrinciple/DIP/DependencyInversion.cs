// Dependency Inversion Principle (DIP) in C# states that high-level modules should not depend on low-level modules.
// Both should depend on abstractions (e.g., interfaces). Additionally, abstractions should not depend on details.
// Details (concrete implementations) should depend on abstractions. This principle helps to reduce coupling between different
// parts of the code and makes it easier to change and maintain.

// This code demonstrates the Dependency Inversion Principle (DIP) in C#. It shows how to design a notification system that can easily
// switch between different notification services (like Email and SMS) without changing the high-level business logic.
// The code includes both a flawed design that violates DIP and a correct design that adheres to it, using interfaces and dependency injection
// to achieve loose coupling between modules.

namespace SolidPrinciple.DIP
{
    public class DependencyInversion
    {
        public void DIPExample()
        {
            Console.WriteLine("Demonstrating Dependency Inversion Principle (DIP) in C#");
            // Using EmailNotificationService
            INotificationService emailService = new EmailNotificationService();
            NotificationManagerDIP notificationManagerEmail = new NotificationManagerDIP(emailService);
            notificationManagerEmail.SendNotification("Hello via Email!");

            // Using SmsService
            Console.WriteLine("\nNow switching to SMS service without changing NotificationManagerDIP!");
            INotificationService smsService = new SmsService();
            NotificationManagerDIP notificationManagerSms = new NotificationManagerDIP(smsService);
            notificationManagerSms.SendNotification("Hello via SMS!");
        }
    }

    // The Issue with the Current Design which violates DIP:
    // Low-level module (The detail)
    public class EmailService
    {
        public void SendEmail(string message)
        {
            Console.WriteLine($"Email sent: {message}");
        }
    }

    // High-level module (The business logic)
    public class NotificationManager
    {
        private EmailService _emailService;

        public NotificationManager()
        {
            // Tight coupling! NotificationManager is stuck with EmailService forever.
            _emailService = new EmailService();
        }

        public void SendNotification(string message)
        {
            _emailService.SendEmail(message);
        }
    }

    // The Correct Design adhering to DIP:
    // Abstraction (The interface)
    public interface INotificationService
    {
        void Send(string message);
    }

    // Low-level module (The detail) implementing the abstraction
    public class EmailNotificationService : INotificationService
    {
        public void Send(string message)
        {
            Console.WriteLine($"Email sent: {message}");
        }
    }

    // Another low-level module (The detail) implementing the abstraction
    public class SmsService : INotificationService
    {
        public void Send(string message)
        {
            Console.WriteLine($"SMS sent: {message}");
        }
    }

    // High-level module (The business logic) depending on the abstraction
    public class NotificationManagerDIP
    {
        private INotificationService _notificationService;
        // Dependency Injection through constructor
        public NotificationManagerDIP(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public void SendNotification(string message)
        {
            _notificationService.Send(message);
        }
    }

    // Example of using the correct design with Dependency Injection
}
