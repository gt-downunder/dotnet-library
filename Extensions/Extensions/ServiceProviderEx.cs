using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DotNet.Library.Extensions
{
    public static class ServiceProviderEx
    {
        /// <summary>
        /// Asynchronously resolves a <typeparamref name="TService"/> service scope, executes a delegate function, and returns the result.
        /// The scoped <typeparamref name="TService"/> is disposed after the delegate is executed.
        /// </summary>
        /// <typeparam name="TService">The service to resolve from the <see cref="IServiceProvider"/></typeparam>
        /// <typeparam name="TResult">The type of result returned from the delegate function</typeparam>
        /// <param name="provider">The source provider</param>
        /// <param name="func">The delegate <see cref="Func{T, TResult}"/> to execute with the scoped <typeparamref name="TService"/></param>
        public static async Task<TResult> ExecuteWithScope<TService, TResult>(this IServiceProvider provider, Func<TService, Task<TResult>> func)
        {
            using IServiceScope scope = provider.CreateScope();
            TService service = scope.ServiceProvider.GetRequiredService<TService>();

            return await func.Invoke(service).ConfigureAwait(false);
        }

        /// <summary>
        /// Resolves a <typeparamref name="TService"/> service scope and executes a delegate action.
        /// The scoped <typeparamref name="TService"/> is disposed after the delegate is executed.
        /// </summary>
        /// <typeparam name="TService">The service to resolve from the <see cref="IServiceProvider"/></typeparam>
        /// <param name="provider">The source provider</param>
        /// <param name="action">The delegate <see cref="Action"/> to execute with the scoped <typeparamref name="TService"/></param>
        public static void ExecuteWithScope<TService>(this IServiceProvider provider, Action<TService> action)
        {
            using IServiceScope scope = provider.CreateScope();
            TService service = scope.ServiceProvider.GetRequiredService<TService>();

            action.Invoke(service);
        }
    }
}
