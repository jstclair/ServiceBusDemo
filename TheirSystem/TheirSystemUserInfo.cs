using Messages;

namespace TheirSystem
{
    internal class TheirSystemUserInfo
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string SSN { get; set; }

        public Address[] Addresses { get; set; }
    }
}