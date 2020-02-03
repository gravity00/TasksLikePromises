using System;
using System.Collections.Generic;
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
    }
}
