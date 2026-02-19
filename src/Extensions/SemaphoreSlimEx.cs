namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="SemaphoreSlim"/>.
    /// </summary>
    public static class SemaphoreSlimEx
    {
        extension(SemaphoreSlim semaphore)
        {
            /// <summary>
            /// Asynchronously waits to enter the semaphore, returning an <see cref="IAsyncDisposable"/>
            /// that releases the semaphore when disposed. This enables the <c>await using</c> pattern:
            /// <code>
            /// await using var _ = semaphore.LockAsync();
            /// // critical section
            /// </code>
            /// </summary>
            /// <param name="cancellationToken">An optional cancellation token.</param>
            /// <returns>An <see cref="IAsyncDisposable"/> that releases the semaphore on disposal.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the semaphore is null.</exception>
            public async Task<IAsyncDisposable> LockAsync(CancellationToken cancellationToken = default)
            {
                ArgumentNullException.ThrowIfNull(semaphore);

                await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
                return new SemaphoreReleaser(semaphore);
            }
        }

        private sealed class SemaphoreReleaser(SemaphoreSlim semaphore) : IAsyncDisposable
        {
            public ValueTask DisposeAsync()
            {
                semaphore.Release();
                return ValueTask.CompletedTask;
            }
        }
    }
}

