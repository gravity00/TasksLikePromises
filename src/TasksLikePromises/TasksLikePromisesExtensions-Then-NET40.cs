#if NET40

using TasksLikePromises;

// ReSharper disable once CheckNamespace
namespace System.Threading.Tasks
{
    public static partial class TasksLikePromisesExtensions
    {
        public static Task<TResult> Then<T, TResult>(this Task<T> task, Func<T, Task<TResult>> onFulfilled)
        {
            task.NotNull(nameof(task));
            onFulfilled.NotNull(nameof(onFulfilled));

            if (task.IsCompleted)
            {
                if (task.Exception?.InnerException != null)
                    return Promise.Reject<TResult>(task.Exception.InnerExceptions);
                if (task.IsCanceled)
                    return Promise.Cancel<TResult>();

                try
                {
                    return onFulfilled(task.Result);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    return Promise.Reject<TResult>(e);
                }
            }

            var tcs = new TaskCompletionSource<TResult>();

            task.ContinueWith(t =>
            {
                if (t.Exception?.InnerException != null)
                {
                    tcs.SetException(t.Exception.InnerExceptions);
                    return;
                }

                if (t.IsCanceled)
                {
                    tcs.SetCanceled();
                    return;
                }

                Task<TResult> fulfilledTask;
                try
                {
                    fulfilledTask = onFulfilled(t.Result);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    tcs.SetException(e);
                    return;
                }

                if (fulfilledTask.IsCompleted)
                {
                    tcs.SetFromCompleted(fulfilledTask);
                    return;
                }

                fulfilledTask.ContinueWith(
                    ft => tcs.SetFromCompleted(ft), CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return tcs.Task;
        }

        public static Task<TResult> Then<T, TResult>(this Task<T> task, Func<T, TResult> onFulfilled)
        {
            task.NotNull(nameof(task));
            onFulfilled.NotNull(nameof(onFulfilled));

            if (task.IsCompleted)
            {
                if (task.Exception?.InnerException != null)
                    return Promise.Reject<TResult>(task.Exception.InnerExceptions);
                if (task.IsCanceled)
                    return Promise.Cancel<TResult>();

                try
                {
                    return Promise.Resolve(onFulfilled(task.Result));
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    return Promise.Reject<TResult>(e);
                }
            }

            var tcs = new TaskCompletionSource<TResult>();

            task.ContinueWith(t =>
            {
                if (t.Exception?.InnerException != null)
                {
                    tcs.SetException(t.Exception.InnerExceptions);
                    return;
                }

                if (t.IsCanceled)
                {
                    tcs.SetCanceled();
                    return;
                }

                try
                {
                    tcs.SetResult(onFulfilled(t.Result));
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    tcs.SetException(e);
                }
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return tcs.Task;
        }

        public static Task Then<T>(this Task<T> task, Func<T, Task> onFulfilled)
        {
            task.NotNull(nameof(task));
            onFulfilled.NotNull(nameof(onFulfilled));

            if (task.IsCompleted)
            {
                if (task.Exception?.InnerException != null)
                    return Promise.Reject(task.Exception.InnerExceptions);
                if (task.IsCanceled)
                    return Promise.Canceled;

                try
                {
                    return onFulfilled(task.Result);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    return Promise.Reject(e);
                }
            }

            var tcs = new TaskCompletionSource<bool>();

            task.ContinueWith(t =>
            {
                if (t.Exception?.InnerException != null)
                {
                    tcs.SetException(t.Exception.InnerExceptions);
                    return;
                }

                if (t.IsCanceled)
                {
                    tcs.SetCanceled();
                    return;
                }

                Task fulfilledTask;
                try
                {
                    fulfilledTask = onFulfilled(t.Result);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    tcs.SetException(e);
                    return;
                }

                if (fulfilledTask.IsCompleted)
                {
                    tcs.SetFromCompleted(fulfilledTask);
                    return;
                }

                fulfilledTask.ContinueWith(
                    ft => tcs.SetFromCompleted(ft), CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return tcs.Task;
        }

        public static Task Then<T>(this Task<T> task, Action<T> onFulfilled)
        {
            task.NotNull(nameof(task));
            onFulfilled.NotNull(nameof(onFulfilled));

            if (task.IsCompleted)
            {
                if (task.Exception?.InnerException != null)
                    return Promise.Reject(task.Exception.InnerExceptions);
                if (task.IsCanceled)
                    return Promise.Canceled;

                try
                {
                    onFulfilled(task.Result);
                    return Promise.Resolved;
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    return Promise.Reject(e);
                }
            }

            var tcs = new TaskCompletionSource<bool>();

            task.ContinueWith(t =>
            {
                if (t.Exception?.InnerException != null)
                {
                    tcs.SetException(t.Exception.InnerExceptions);
                    return;
                }

                if (t.IsCanceled)
                {
                    tcs.SetCanceled();
                    return;
                }

                try
                {
                    onFulfilled(t.Result);
                    tcs.SetResult(true);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    tcs.SetException(e);
                }
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return tcs.Task;
        }

        public static Task<TResult> Then<TResult>(this Task task, Func<Task<TResult>> onFulfilled)
        {
            task.NotNull(nameof(task));
            onFulfilled.NotNull(nameof(onFulfilled));

            if (task.IsCompleted)
            {
                if (task.Exception?.InnerException != null)
                    return Promise.Reject<TResult>(task.Exception.InnerExceptions);
                if (task.IsCanceled)
                    return Promise.Cancel<TResult>();

                try
                {
                    return onFulfilled();
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    return Promise.Reject<TResult>(e);
                }
            }

            var tcs = new TaskCompletionSource<TResult>();

            task.ContinueWith(t =>
            {
                if (t.Exception?.InnerException != null)
                {
                    tcs.SetException(t.Exception.InnerExceptions);
                    return;
                }

                if (t.IsCanceled)
                {
                    tcs.SetCanceled();
                    return;
                }

                Task<TResult> fulfilledTask;
                try
                {
                    fulfilledTask = onFulfilled();
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    tcs.SetException(e);
                    return;
                }

                if (fulfilledTask.IsCompleted)
                {
                    tcs.SetFromCompleted(fulfilledTask);
                    return;
                }

                fulfilledTask.ContinueWith(
                    ft => tcs.SetFromCompleted(ft), CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return tcs.Task;
        }

        public static Task<TResult> Then<TResult>(this Task task, Func<TResult> onFulfilled)
        {
            task.NotNull(nameof(task));
            onFulfilled.NotNull(nameof(onFulfilled));

            if (task.IsCompleted)
            {
                if (task.Exception?.InnerException != null)
                    return Promise.Reject<TResult>(task.Exception.InnerExceptions);
                if (task.IsCanceled)
                    return Promise.Cancel<TResult>();

                try
                {
                    return Promise.Resolve(onFulfilled());
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    return Promise.Reject<TResult>(e);
                }
            }

            var tcs = new TaskCompletionSource<TResult>();

            task.ContinueWith(t =>
            {
                if (t.Exception?.InnerException != null)
                {
                    tcs.SetException(t.Exception.InnerExceptions);
                    return;
                }

                if (t.IsCanceled)
                {
                    tcs.SetCanceled();
                    return;
                }

                try
                {
                    tcs.SetResult(onFulfilled());
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    tcs.SetException(e);
                }
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return tcs.Task;
        }

        public static Task Then(this Task task, Func<Task> onFulfilled)
        {
            task.NotNull(nameof(task));
            onFulfilled.NotNull(nameof(onFulfilled));

            if (task.IsCompleted)
            {
                if (task.Exception?.InnerException != null)
                    return Promise.Reject(task.Exception.InnerExceptions);
                if (task.IsCanceled)
                    return Promise.Canceled;

                try
                {
                    return onFulfilled();
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    return Promise.Reject(e);
                }
            }

            var tcs = new TaskCompletionSource<bool>();

            task.ContinueWith(t =>
            {
                if (t.Exception?.InnerException != null)
                {
                    tcs.SetException(t.Exception.InnerExceptions);
                    return;
                }

                if (t.IsCanceled)
                {
                    tcs.SetCanceled();
                    return;
                }

                Task fulfilledTask;
                try
                {
                    fulfilledTask = onFulfilled();
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    tcs.SetException(e);
                    return;
                }

                if (fulfilledTask.IsCompleted)
                {
                    tcs.SetFromCompleted(fulfilledTask);
                    return;
                }

                fulfilledTask.ContinueWith(
                    ft => tcs.SetFromCompleted(ft), CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return tcs.Task;
        }

        public static Task Then(this Task task, Action onFulfilled)
        {
            task.NotNull(nameof(task));
            onFulfilled.NotNull(nameof(onFulfilled));

            if (task.IsCompleted)
            {
                if (task.Exception?.InnerException != null)
                    return Promise.Reject(task.Exception.InnerExceptions);
                if (task.IsCanceled)
                    return Promise.Canceled;

                try
                {
                    onFulfilled();
                    return Promise.Resolved;
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    return Promise.Reject(e);
                }
            }

            var tcs = new TaskCompletionSource<bool>();

            task.ContinueWith(t =>
            {
                if (t.Exception?.InnerException != null)
                {
                    tcs.SetException(t.Exception.InnerExceptions);
                    return;
                }

                if (t.IsCanceled)
                {
                    tcs.SetCanceled();
                    return;
                }

                try
                {
                    onFulfilled();
                    tcs.SetResult(true);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    tcs.SetException(e);
                }
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return tcs.Task;
        }
    }
}

#endif
