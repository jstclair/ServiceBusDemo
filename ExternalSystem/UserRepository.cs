using System.Collections.Generic;
using Utilities;

namespace ExternalSystem
{
    internal class UserRepository
    {
        private static readonly Dictionary<string, ExternalSystemUserInfo> users;

        static UserRepository()
        {
            users = new Dictionary<string, ExternalSystemUserInfo>();

            users.Add(ConfigUtils.SSNs[0], new ExternalSystemUserInfo { FirstName = "John", LastName = "St. Clair" });
            users.Add(ConfigUtils.SSNs[1], new ExternalSystemUserInfo { FirstName = "Robert", LastName = "St. Clair" });
            users.Add(ConfigUtils.SSNs[2], new ExternalSystemUserInfo { FirstName = "Pat", LastName = "St. Clair" });
            users.Add(ConfigUtils.SSNs[3], new ExternalSystemUserInfo { FirstName = "Thomas", LastName = "St. Clair" });
            users.Add(ConfigUtils.SSNs[4], new ExternalSystemUserInfo { FirstName = "Nicholas", LastName = "St. Clair" });
        }

        public ExternalSystemUserInfo this[string key]
        {
            get
            {
                return users[key];
            }
        }

        public bool ContainsKey(string key)
        {
            return users.ContainsKey(key);
        }

        public bool TryGetValue(string key, out ExternalSystemUserInfo value)
        {
            return users.TryGetValue(key, out value);
        }
    }
}