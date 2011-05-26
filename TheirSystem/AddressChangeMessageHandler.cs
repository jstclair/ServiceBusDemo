using System;
using System.Collections.Generic;
using Messages;
using NServiceBus;
using Utilities;

namespace TheirSystem
{
    public class AddressChangeMessageHandler : BaseMessageHandler<AddressChangeMessage>
    {
        private static readonly Dictionary<string, TheirSystemUserInfo> repository = new Dictionary<string, TheirSystemUserInfo>();

        private static readonly object repositoryLock = new object();

        public override void Handle(AddressChangeMessage message)
        {
            lock (repositoryLock)
            {
                ConsoleUtils.WriteLine(
                    ConsoleColor.White, 
                    "TheirSystem received event: [{0}]", 
                    message.EventId);

                if (repository.ContainsKey(message.SSN))
                {
                    ConsoleUtils.WriteLine(
                        ConsoleColor.DarkGreen,
                        "\tUpdating existing user: '{0}'", 
                        message.SSN);

                    repository[message.SSN].Addresses = message.Addresses;
                }
                else
                {
                    var msg = new MissingCustomerInformationRequest
                                  {
                                      SSN = message.SSN,
                                      EventId = Guid.NewGuid()
                                  };

                    ConsoleUtils.WriteLine(
                        ConsoleColor.Red, 
                        "\tNeed to add user '{0}'.\r\n\tSent message '{1}' at {2}",
                        message.SSN,
                        msg.EventId,
                        DateTime.Now.ToString("HH:mm:ss"));

                    // NOTE: Bus doesn't return a value -- Send is *not* a simple request/reply
                    Bus.Send("external", msg)
                       .Register(MissingCustomerInfoComplete, message);
                }
            }
        }

        private static void MissingCustomerInfoComplete(IAsyncResult ar)
        {
            var result = ar.AsyncState as CompletionResult;

            if (result == null ||
                result.Messages == null ||
                result.Messages.Length == 0 ||
                result.State == null)
            {
                return;
            }

            var addressChangeMessage = result.State as AddressChangeMessage;
            if (addressChangeMessage == null)
            {
                return;
            }

            var response = result.Messages[0] as MissingCustomerInformationResponse;
            if (response == null)
            {
                return;
            }

            ConsoleUtils.WriteLine(
                ConsoleColor.Green, 
                "\tReceived user info for '{0}' at {1} \r\n\t[{2}]",
                addressChangeMessage.SSN,
                DateTime.Now.ToString("HH:mm:ss"),
                response.EventId);

            lock (repositoryLock)
            {
                ConsoleUtils.WriteLine(
                    ConsoleColor.Yellow,
                    "\tAdding user information for '{0}' [{1} {2}]", 
                    addressChangeMessage.SSN,
                    response.FirstName,
                    response.LastName);

                var info = new TheirSystemUserInfo
                               {
                                   FirstName = response.FirstName, 
                                   LastName = response.LastName, 
                                   SSN = addressChangeMessage.SSN, 
                                   Addresses = addressChangeMessage.Addresses
                               };

                // comment out the following line to force all calls to go to ExternalSystem
                repository.AddOrUpdate(info.SSN, info);
            }
        }
    }
}