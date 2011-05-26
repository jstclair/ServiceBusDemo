using System;
using System.Threading;
using Messages;
using NServiceBus;
using Utilities;

namespace ExternalSystem
{
    public class MissingCustomerInformationMessageHandler : BaseMessageHandler<MissingCustomerInformationRequest>
    {
        private static readonly UserRepository repository = new UserRepository();

        public override void Handle(MissingCustomerInformationRequest request)
        {
            ConsoleUtils.WriteLine(
               ConsoleColor.Red,
               "Received request: '{0}' - {1}\r\n\t[{2}]",
               request.SSN,
               DateTime.Now.ToShortTimeString(),
               request.EventId.ToString());

            int delay = ConfigUtils.RandomDelay;
            ////int delay = 8000;

            ConsoleUtils.WriteLine(ConsoleColor.Gray, "\tSleeping for {0} milliseconds...", delay);

            Thread.Sleep(delay);

            ExternalSystemUserInfo user;

            if (!repository.TryGetValue(request.SSN, out user))
            {
                user = new ExternalSystemUserInfo();
            }

            var response = new MissingCustomerInformationResponse
                {
                    EventId = request.EventId,
                    SSN = request.SSN,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };

            ConsoleUtils.WriteLine(
                ConsoleColor.Green,
                "Replying: '{0}' - {1}\r\n\t[{2}]",
                request.SSN,
                DateTime.Now.ToShortTimeString(),
                response.EventId);

            Bus.Reply(response);
        }
    }
}