using System;
using Messages;
using NServiceBus;
using NServiceBus.Serializers;
using NServiceBus.Unicast.Config;
using NServiceBus.Unicast.Subscriptions.Msmq.Config;
using NServiceBus.Unicast.Transport.Msmq.Config;
using ObjectBuilder.SpringFramework;
using Utilities;

namespace OurSystem
{
    internal class Program
    {
        private const string SystemName = "OurSystem";
        private const string StartupMessage =
            @"Press 'Enter' to publish a message. Enter a number and press 'Enter' to publish that number of events. To exit, press 'q' and then 'Enter'.";

        private static IBus bus;

        private static void Main()
        {
            Console.Title = SystemName;

            Builder builder = GetBuilder();
            bus = builder.Build<IBus>();
            bus.Start();

            ConsoleUtils.WriteLine(ConsoleColor.White, StartupMessage);
            Console.WriteLine();

            string read = Console.ReadLine();
            while (ConsoleUtils.UserHasNotEnteredQuitSequence(read, "q"))
            {
                int messagesToSend = GetNumberOfMessagesToSend(read);
                PublishAddressChange(messagesToSend);
                read = Console.ReadLine();
            }
        }

        private static void PublishAddressChange(int messagesToSend)
        {
            for (int i = 0; i < messagesToSend; i++)
            {
                AddressChangeMessage addressChangeMessage = GetAddressChangeMessage();
                PublishAddressChange(addressChangeMessage);
            }
        }

        private static AddressChangeMessage GetAddressChangeMessage()
        {
            return new AddressChangeMessage
            {
                EventId = Guid.NewGuid(),
                SSN = ConfigUtils.RandomSocialSecurityNumber,
                Addresses = new[]
                                {
                                    Addresses.HomeAddress, 
                                    Addresses.WorkAddress
                                }
            };
        }

        private static void PublishAddressChange(AddressChangeMessage addressChangeMessage)
        {
            bus.Publish(addressChangeMessage);

            ConsoleUtils.WriteLine(
                ConsoleColor.Green,
                "Send address change message: '{0}'\r\n\t[{1}] {2}.",
                addressChangeMessage.SSN,
                addressChangeMessage.EventId,
                DateTime.Now.ToString("HH:mm:ss"));
        }

        private static int GetNumberOfMessagesToSend(string read)
        {
            int number;
            if (!int.TryParse(read, out number))
            {
                number = 1;
            }

            return number;
        }

        private static Builder GetBuilder()
        {
            Builder builder = new Builder();

            new ConfigMsmqSubscriptionStorage(builder);

            Configure.BinarySerializer.With(builder);

            new ConfigMsmqTransport(builder)
                .IsTransactional(true)
                .PurgeOnStartup(false);

            new ConfigUnicastBus(builder)
                .ImpersonateSender(false);

            return builder;
        }
    }
}