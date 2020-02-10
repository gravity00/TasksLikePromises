using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TasksLikePromises
{
    /// <summary>
    /// Extensions and helpers for <see cref="Task"/> instances.
    /// </summary>
    public static class Promise
    {
        /// <summary>
        /// Represents a task in the completed state
        /// </summary>
        public static Task Resolved { get; } = Resolve(true);

        /// <summary>
        /// Represents a task in the canceled state
        /// </summary>
        public static Task Canceled { get; } = Cancel<bool>();

        /// <summary>
        /// Creates a completed task with the given value
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="value">The value to be used</param>
        /// <returns>Completed task holding the given value</returns>
        public static Task<T> Resolve<T>(T value)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetResult(value);
            return tcs.Task;
        }

        /// <summary>
        /// Creates a faulted task with the given exception as the reason
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="exception">The exception to be used</param>
        /// <returns>Faulted task holding the given exception</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task<T> Reject<T>(Exception exception)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetException(exception);
            return tcs.Task;
        }

        /// <summary>
        /// Creates a faulted task with the given exceptions as the reason
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="exceptions">The exceptions to be used</param>
        /// <returns>Faulted task holding the given exception</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task<T> Reject<T>(IEnumerable<Exception> exceptions)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetException(exceptions);
            return tcs.Task;
        }

        /// <summary>
        /// Creates a faulted task with the given exception as the reason
        /// </summary>
        /// <param name="exception">The exception to be used</param>
        /// <returns>Faulted task holding the given exception</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task Reject(Exception exception) => Reject<bool>(exception);

        /// <summary>
        /// Creates a faulted task with the given exceptions as the reason
        /// </summary>
        /// <param name="exceptions">The exceptions to be used</param>
        /// <returns>Faulted task holding the given exception</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task Reject(IEnumerable<Exception> exceptions) => Reject<bool>(exceptions);

        /// <summary>
        /// Creates a canceled task
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <returns></returns>
        public static Task<T> Cancel<T>()
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetCanceled();
            return tcs.Task;
        }

        /// <summary>
        /// Creates a task that will be completed after the given
        /// milliseconds have passed.
        /// </summary>
        /// <param name="delay">The time to delay, in milliseconds</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static Task Timeout(int delay)
        {
            if (delay < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(delay), delay, "Delay must be greater or equal to zero");
            }

#if NET40
            var tcs = new TaskCompletionSource<bool>();

#pragma warning disable CA2000 // Dispose objects before losing scope
            var timer = new Timer(_ => tcs.SetResult(true), null, delay, System.Threading.Timeout.Infinite);
#pragma warning restore CA2000 // Dispose objects before losing scope

            return tcs.Task.Finally(() => timer.Dispose());
#else
            return Task.Delay(delay);
#endif
        }
    }
}
