using System;
using Messages;

namespace Utilities
{
    public static class Addresses
    {
        public static Address WorkAddress
        {
            get
            {
                return new Address
                           {
                               Street1 = "Nagelgården 6",
                               Street2 = "c/o Reaktor",
                               City = "Bergen",
                               Country = "Norway",
                               PostNumber = "5004"
                           };
            }
        }

        public static Address HomeAddress
        {
            get
            {
                return new Address
                           {
                               Street1 = "Strandgaten 66",
                               Street2 = String.Empty,
                               City = "Bergen",
                               Country = "Norway",
                               PostNumber = "5004"
                           };
            }
        }
    }
}