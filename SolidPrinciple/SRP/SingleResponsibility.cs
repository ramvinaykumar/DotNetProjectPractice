// The Single Responsibility Principle (SRP) is one of the five SOLID principles of object-oriented design.
// It states that a class should have only one reason to change, meaning that it should have only one responsibility or job.
// This principle helps to create more maintainable and flexible code by ensuring that each class has a clear

namespace SolidPrinciple.SRP
{
    public class SingleResponsibility
    {
        public void SRPExample()
        {
            Console.WriteLine("Single Responsibility Principle (SRP) Example:");
            UserService userService = new UserService();
            userService.RegisterUser("John Doe");
            userService.DeleteUser("John Doe");
        }

        public class UserRepository
        {
            public void Save(string username) => Console.WriteLine($"Saving {username} to DB");

            public void Delete(string username) => Console.WriteLine($"Deleting {username} from DB");
        }

        public class Logger
        {
            public void Log(string message) => File.WriteAllText("log.txt", message);
        }

        // High-level service only orchestrates the business logic
        public class UserService
        {
            private readonly UserRepository _repo = new UserRepository();
            private readonly Logger _logger = new Logger();

            public void RegisterUser(string username)
            {
                if (string.IsNullOrEmpty(username)) throw new Exception("Invalid username");

                _repo.Save(username);
                _logger.Log($"User {username} registered.");
            }

            public void DeleteUser(string username)
            {
                if (string.IsNullOrEmpty(username)) throw new Exception("Invalid username");

                _repo.Delete(username);
                _logger.Log($"User {username} deleted.");
            }
        }
    }

    // The Issue with the Current Design which violates SRP:
    public class UserService
    {
        public void RegisterUser(string username)
        {
            // 1. Business Logic
            if (string.IsNullOrEmpty(username)) throw new Exception("Invalid username");

            // 2. Database Persistence
            Console.WriteLine($"Saving {username} to the database...");

            // 3. Logging
            File.WriteAllText("log.txt", $"User {username} registered at {DateTime.Now}");
        }
    }
}
