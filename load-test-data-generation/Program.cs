using System;

namespace load_test_data_generation
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new Config();
            var contextProvider = new ContextProvider(config.ConnectionString);
            var userGenerator = new UserGenerator(contextProvider);
            var notificationGenerator = new NotificationGenerator(contextProvider);

            if (config.GenerateUsers)
            {
                Console.WriteLine("Generating users...");
                userGenerator.GenerateUsers();
            }

            if (config.NumberOfNotificationsToGenerate.HasValue)
            {
                var numberOfNotifications = config.NumberOfNotificationsToGenerate.Value;
                Console.WriteLine($"Generating {numberOfNotifications} notifications...");
                notificationGenerator.GenerateNotifications(numberOfNotifications);
            }

            Console.WriteLine("Data generated. Press any key to exit...");
            Console.ReadLine();
        }
    }
}
