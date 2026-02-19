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
            /// <returns><c>true</c> if the environment name is "local" (case-insensitive); otherwise, <c>false</c>.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the environment is <c>null</c>.</exception>
            public bool IsLocal()
            {
                ArgumentNullException.ThrowIfNull(env);
                return env.EnvironmentName.EqualsIgnoreCase(Environments.Local);
            }

            /// <summary>Determines whether the hosting environment is "test".</summary>
            /// <returns><c>true</c> if the environment name is "test" (case-insensitive); otherwise, <c>false</c>.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the environment is <c>null</c>.</exception>
            public bool IsTest()
            {
                ArgumentNullException.ThrowIfNull(env);
                return env.EnvironmentName.EqualsIgnoreCase(Environments.Test);
            }

            /// <summary>Determines whether the hosting environment is "uat".</summary>
            /// <returns><c>true</c> if the environment name is "uat" (case-insensitive); otherwise, <c>false</c>.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the environment is <c>null</c>.</exception>
            public bool IsUat()
            {
                ArgumentNullException.ThrowIfNull(env);
                return env.EnvironmentName.EqualsIgnoreCase(Environments.Uat);
            }

            // Built-in ones already exist: IsDevelopment(), IsProduction(), IsStaging()
        }
    }
}
