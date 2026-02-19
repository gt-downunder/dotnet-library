using Microsoft.Extensions.DependencyInjection;

namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IServiceProvider"/> to simplify scoped service resolution.
    /// </summary>
    public static class ServiceProviderEx
    {
        extension(IServiceProvider provider)
        {
            /// <summary>
            /// Creates a new service scope, resolves a service of type <typeparamref name="TService"/>,
            /// executes the specified asynchronous function, and returns its result.
            /// The scope and resolved service are disposed after execution.
            /// </summary>
            /// <typeparam name="TService">The type of service to resolve.</typeparam>
            /// <typeparam name="TResult">The type of result returned by the function.</typeparam>
            /// <param name="func">The asynchronous function to execute with the resolved service.</param>
            /// <returns>A task representing the asynchronous operation, containing the result of <paramref name="func"/>.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the provider or <paramref name="func"/> is null.</exception>
            public async Task<TResult> UseScopedAsync<TService, TResult>(Func<TService, Task<TResult>> func) where TService : notnull
            {
                ArgumentNullException.ThrowIfNull(provider);
                ArgumentNullException.ThrowIfNull(func);

                using IServiceScope scope = provider.CreateScope();
                TService service = scope.ServiceProvider.GetRequiredService<TService>();
                return await func(service).ConfigureAwait(false);
            }

            /// <summary>
            /// Creates a new service scope, resolves a service of type <typeparamref name="TService"/>,
            /// and executes the specified asynchronous function.
            /// The scope and resolved service are disposed after execution.
            /// </summary>
            /// <typeparam name="TService">The type of service to resolve.</typeparam>
            /// <param name="func">The asynchronous function to execute with the resolved service.</param>
            /// <returns>A task representing the asynchronous operation.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the provider or <paramref name="func"/> is null.</exception>
            public async Task UseScopedAsync<TService>(Func<TService, Task> func) where TService : notnull
            {
                ArgumentNullException.ThrowIfNull(provider);
                ArgumentNullException.ThrowIfNull(func);

                using IServiceScope scope = provider.CreateScope();
                TService service = scope.ServiceProvider.GetRequiredService<TService>();
                await func(service).ConfigureAwait(false);
            }

            /// <summary>
            /// Creates a new service scope, resolves a service of type <typeparamref name="TService"/>,
            /// and executes the specified action.
            /// The scope and resolved service are disposed after execution.
            /// </summary>
            /// <typeparam name="TService">The type of service to resolve.</typeparam>
            /// <param name="action">The action to execute with the resolved service.</param>
            /// <exception cref="ArgumentNullException">Thrown if the provider or <paramref name="action"/> is null.</exception>
            public void UseScoped<TService>(Action<TService> action) where TService : notnull
            {
                ArgumentNullException.ThrowIfNull(provider);
                ArgumentNullException.ThrowIfNull(action);

                using IServiceScope scope = provider.CreateScope();
                TService service = scope.ServiceProvider.GetRequiredService<TService>();
                action(service);
            }
        }
    }
}

