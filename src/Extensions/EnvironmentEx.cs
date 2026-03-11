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

            /// <summary>Determines whether the hosting environment is "production" or "prod".</summary>
            /// <returns><c>true</c> if the environment name is "production" or "prod" (case-insensitive); otherwise, <c>false</c>.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the environment is <c>null</c>.</exception>
            public bool IsProduction()
            {
                ArgumentNullException.ThrowIfNull(env);
                return env.EnvironmentName.EqualsIgnoreCase(Environments.Prod) ||
                       env.EnvironmentName.EqualsIgnoreCase("Production");
            }

            /// <summary>Determines whether the hosting environment is "development".</summary>
            /// <returns><c>true</c> if the environment name is "development" (case-insensitive); otherwise, <c>false</c>.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the environment is <c>null</c>.</exception>
            public bool IsDevelopment()
            {
                ArgumentNullException.ThrowIfNull(env);
                return env.EnvironmentName.EqualsIgnoreCase("Development");
            }

            /// <summary>Determines whether the hosting environment is "staging".</summary>
            /// <returns><c>true</c> if the environment name is "staging" (case-insensitive); otherwise, <c>false</c>.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the environment is <c>null</c>.</exception>
            public bool IsStaging()
            {
                ArgumentNullException.ThrowIfNull(env);
                return env.EnvironmentName.EqualsIgnoreCase("Staging");
            }

            /// <summary>
            /// Determines whether the hosting environment matches any of the specified names.
            /// </summary>
            /// <param name="environmentNames">The environment names to check against.</param>
            /// <returns><c>true</c> if the environment matches any of the specified names; otherwise, <c>false</c>.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the environment or environment names are <c>null</c>.</exception>
            public bool IsEnvironment(params string[] environmentNames)
            {
                ArgumentNullException.ThrowIfNull(env);
                ArgumentNullException.ThrowIfNull(environmentNames);

                return environmentNames.Any(name =>
                    env.EnvironmentName.EqualsIgnoreCase(name));
            }
        }
    }
}
