using System;

namespace Messages
{
    [Serializable]
    public class Address
    {
        public string Street1 { get; set; }

        public string Street2 { get; set; }

        public string City { get; set; }

        public string PostNumber { get; set; }

        public string Country { get; set; }
    }
}