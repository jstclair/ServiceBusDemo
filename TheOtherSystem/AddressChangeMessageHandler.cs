using System;
using System.Collections.Generic;
using Messages;
using NServiceBus;
using Utilities;

namespace TheOtherSystem
{
    public class AddressChangeMessageHandler : IMessageHandler<AddressChangeMessage>
    {
        private static readonly Dictionary<string, TheOtherSystemUserInfo> repository =
            new Dictionary<string, TheOtherSystemUserInfo>();

        private static readonly object repositoryLock = new object();

        public void Handle(AddressChangeMessage message)
        {
            lock (repositoryLock)
            {
                ConsoleUtils.WriteLine(
                    ConsoleColor.White,
                    "TheOtherSystem received event: [{0}]", 
                    message.EventId);

                if (repository.ContainsKey(message.SSN))
                {
                    ConsoleUtils.WriteLine(
                        ConsoleColor.DarkGreen, 
                        "Updating existing user: '{0}'",
                        message.SSN);

                    repository[message.SSN].Addresses = message.Addresses;
                }
                else
                {
                    ConsoleUtils.WriteLine(
                        ConsoleColor.Green, 
                        "Adding user {0}",
                        message.SSN);

                    TheOtherSystemUserInfo info = new TheOtherSystemUserInfo
                                                      {
                                                          SSN = message.SSN,
                                                          Addresses = message.Addresses
                                                      };
                    repository.Add(info.SSN, info);
                }
            }
        }
    }
}