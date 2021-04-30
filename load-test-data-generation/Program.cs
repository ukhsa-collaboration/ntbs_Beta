using System;

namespace load_test_data_generation
{
    class Program
    {
        static void Main(string[] args)
        {
            var contextProvider = new ContextProvider(Config.ConnectionString);
            var userGenerator = new UserGenerator(contextProvider);
            var notificationGenerator = new NotificationGenerator(contextProvider);

#pragma warning disable CS0162 // Unreachable code detected
            if (Config.GenerateUsers)
            {
                Console.WriteLine("Generating users...");
                userGenerator.GenerateUsers();
            }

            if (Config.NumberOfNotificationsToGenerate.HasValue)
            {
                var numberOfNotifications = Config.NumberOfNotificationsToGenerate.Value;
                Console.WriteLine($"Generating {numberOfNotifications} notifications...");
                notificationGenerator.GenerateNotifications(numberOfNotifications);
            }
#pragma warning restore CS0162 // Unreachable code detected
            Console.WriteLine("Data generated. Press any key to exit...");
            Console.ReadLine();
        }

        private static class Config
        {
            public const string ConnectionString = "data source=localhost;initial catalog=ntbsDev;trusted_connection=true;MultipleActiveResultSets=true;";

            public const bool GenerateUsers = false;
            public static readonly int? NumberOfNotificationsToGenerate = null;
        }
    }
}
