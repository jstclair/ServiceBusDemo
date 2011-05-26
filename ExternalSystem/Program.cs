using System;
using NServiceBus;
using NServiceBus.Serializers;
using NServiceBus.Unicast.Config;
using NServiceBus.Unicast.Subscriptions.Msmq.Config;
using NServiceBus.Unicast.Transport.Msmq.Config;
using ObjectBuilder.SpringFramework;
using Utilities;

namespace ExternalSystem
{
    internal class Program
    {
        private const string SystemName = "ExternalSystem";
        private const string StartupMessage = SystemName + " listening for events. To exit, press 'q' and then 'Enter'.";
        private static IBus bus;

        private static void Main()
        {
            Console.Title = SystemName;
            
            Builder builder = GetBuilder();
            bus = builder.Build<IBus>();
            bus.Start();

            ConsoleUtils.WriteLine(ConsoleColor.White, StartupMessage);
            Console.WriteLine();

            while (ConsoleUtils.UserHasNotEnteredQuitSequence("q"))
            {
            }
        }

        private static Builder GetBuilder()
        {
            Builder builder = new Builder();

            Configure.BinarySerializer.With(builder);

            new ConfigMsmqTransport(builder)
                .IsTransactional(false)
                .PurgeOnStartup(false);

            /*
             * Comment out previous section, and uncomment following
             * to get automatic retries of unhandled messages
             */
            /*new ConfigMsmqTransport(builder)
                .IsTransactional(true)
                .PurgeOnStartup(false); */

            new ConfigUnicastBus(builder)
                .ImpersonateSender(false)
                .SetMessageHandlersFromAssembliesInOrder(typeof(MissingCustomerInformationMessageHandler).Assembly);

            return builder;
        }
    }
}