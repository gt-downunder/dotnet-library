using Microsoft.Extensions.Hosting;

namespace DotNet.Library.Extensions
{
    public static class EnvironmentEx
    {
        internal const string _local = "local";
        internal const string _test = "test";
        internal const string _dev = "dev";
        internal const string _uat = "uat";
        internal const string _prod = "prod";

        public static bool IsLocal(this IHostEnvironment env)
        {
            return env.EnvironmentName.EqualsIgnoreCase(_local);
        }

        public static bool IsTest(this IHostEnvironment env)
        {
            return env.EnvironmentName.EqualsIgnoreCase(_test);
        }

        public static bool IsDev(this IHostEnvironment env)
        {
            return env.EnvironmentName.EqualsIgnoreCase(_dev);
        }

        public static bool IsUat(this IHostEnvironment env)
        {
            return env.EnvironmentName.EqualsIgnoreCase(_uat);
        }

        public static bool IsProd(this IHostEnvironment env)
        {
            return env.EnvironmentName.EqualsIgnoreCase(_prod);
        }
    }
}
