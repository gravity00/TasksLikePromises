﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TasksLikePromises
{
    /// <summary>
    /// Extensions and helpers for <see cref="Task"/> instances.
    /// </summary>
    public static class PromiseEx
    {
        /// <summary>
        /// Represents a task in the completed state
        /// </summary>
        public static Task Completed { get; } = FromResult(true);

        /// <summary>
        /// Represents a task in the canceled state
        /// </summary>
        public static Task Canceled { get; } = FromCanceled<bool>();

        /// <summary>
        /// Creates a completed task with the given value
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="value">The value to be used</param>
        /// <returns>Completed task holding the given value</returns>
        public static Task<T> FromResult<T>(T value)
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
        public static Task<T> FromException<T>(Exception exception)
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
        public static Task<T> FromException<T>(IEnumerable<Exception> exceptions)
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
        public static Task FromException(Exception exception) => FromException<bool>(exception);

        /// <summary>
        /// Creates a faulted task with the given exceptions as the reason
        /// </summary>
        /// <param name="exceptions">The exceptions to be used</param>
        /// <returns>Faulted task holding the given exception</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task FromException(IEnumerable<Exception> exceptions) => FromException<bool>(exceptions);

        /// <summary>
        /// Creates a canceled task
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <returns></returns>
        public static Task<T> FromCanceled<T>()
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetCanceled();
            return tcs.Task;
        }
    }
}
