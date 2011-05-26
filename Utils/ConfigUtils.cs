using System;
using System.Configuration;

namespace Utilities
{
    public static class ConfigUtils
    {
        private static readonly Random random = new Random();
        public static readonly string[] SSNs = new[] { "15046916391", "15046916392", "15046916393", "15046916394", "15046916395" };

        public static int RandomDelay
        {
            get
            {
                int minDelay, maxDelay;
                if (!int.TryParse(ConfigurationManager.AppSettings["minDelay"], out minDelay))
                {
                    minDelay = 2000;
                }

                if (!int.TryParse(ConfigurationManager.AppSettings["maxDelay"], out maxDelay))
                {
                    maxDelay = 14000;
                }

                int delay = random.Next(minDelay, maxDelay);

                // if the random delay is greater than the average of min and max delays, 
                // return 1.5 times the min delay, 
                // otherwise the random delay
                return delay > (minDelay + maxDelay) / 2 ? (minDelay * 2) / 3 : delay;
            }
        }

        public static string RandomSocialSecurityNumber
        {
            get
            {
                return SSNs[GetRandomIndex()];
            }
        }

        private static int GetRandomIndex()
        {
            return random.Next(SSNs.Length);
        }
    }
}