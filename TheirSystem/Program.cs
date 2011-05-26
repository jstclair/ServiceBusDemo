using System;
using Messages;
using NServiceBus;
using NServiceBus.Serializers;
using NServiceBus.Unicast.Config;
using NServiceBus.Unicast.Subscriptions.Msmq.Config;
using NServiceBus.Unicast.Transport.Msmq.Config;
using ObjectBuilder.SpringFramework;
using Utilities;

namespace TheirSystem
{
    internal class Program
    {
        private const string SystemName = "TheirSystem";
        private const string StartupMessage = SystemName + " listening for events. To exit, press 'q' and then 'Enter'.";

        private static void Main()
        {
            Console.Title = SystemName;

            Builder builder = GetBuilder();
            IBus bus = builder.Build<IBus>();
            bus.Start();

            bus.Subscribe(typeof(AddressChangeMessage));

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
                .IsTransactional(true)
                .PurgeOnStartup(false);

            new ConfigUnicastBus(builder)
                .ImpersonateSender(false)
                .SetMessageHandlersFromAssembliesInOrder(typeof(AddressChangeMessageHandler).Assembly);

            return builder;
        }
    }
}