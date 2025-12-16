using Microsoft.Extensions.Hosting;
using Environments = DotNet.Library.Utilities.Constants.Environments;

namespace DotNet.Library.Extensions
{
    public static class EnvironmentEx
    {
        public static bool IsLocal(this IHostEnvironment env)
        {
            return env.EnvironmentName.EqualsIgnoreCase(Environments.Local);
        }

        public static bool IsTest(this IHostEnvironment env)
        {
            return env.EnvironmentName.EqualsIgnoreCase(Environments.Test);
        }

        public static bool IsUat(this IHostEnvironment env)
        {
            return env.EnvironmentName.EqualsIgnoreCase(Environments.Uat);
        }
        
        // Built-in ones already exist: IsDevelopment(), IsProduction(), IsStaging()
    }
}
