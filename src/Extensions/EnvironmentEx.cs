using Microsoft.Extensions.Hosting;
using Environments = Grondo.Utilities.Environments;

namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IHostEnvironment"/> to check custom environment names.
    /// </summary>
    public static class EnvironmentEx
    {
        extension(IHostEnvironment env)
        {
            /// <summary>Determines whether the hosting environment is "local".</summary>
            public bool IsLocal() =>
                env.EnvironmentName.EqualsIgnoreCase(Environments.Local);

            /// <summary>Determines whether the hosting environment is "test".</summary>
            public bool IsTest() =>
                env.EnvironmentName.EqualsIgnoreCase(Environments.Test);

            /// <summary>Determines whether the hosting environment is "uat".</summary>
            public bool IsUat() =>
                env.EnvironmentName.EqualsIgnoreCase(Environments.Uat);

            // Built-in ones already exist: IsDevelopment(), IsProduction(), IsStaging()
        }
    }
}
