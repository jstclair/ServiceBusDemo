using System;
using NServiceBus;

namespace Messages
{
    /* NOTE: Add [Recoverable] to ensure that messages are not lost even if the machine crashes during a
         send/receive */

    [Serializable]
    public class MissingCustomerInformationResponse : IMessage
    {
        public Guid EventId { get; set; }

        public string SSN { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}