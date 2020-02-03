#if NET40

using TasksLikePromises;

// ReSharper disable once CheckNamespace
namespace System.Threading.Tasks
{
    public static partial class TasksLikePromisesExtensions
    {
        public static Task<T> Catch<T>(this Task<T> task, Func<Exception, Task<T>> onRejected)
        {
            task.NotNull(nameof(task));
            onRejected.NotNull(nameof(onRejected));

            if (task.IsCompleted)
            {
                if (task.IsCanceled || task.Exception?.InnerException == null)
                    return task;

                try
                {
                    return onRejected(task.Exception.InnerException);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    return Promise.Reject<T>(e);
                }
            }

            var tcs = new TaskCompletionSource<T>();

            task.ContinueWith(t =>
            {
                if (t.Exception?.InnerException == null)
                {
                    if (t.IsCanceled)
                    {
                        tcs.SetCanceled();
                        return;
                    }

                    tcs.SetResult(t.Result);
                    return;
                }

                Task<T> rejectedTask;
                try
                {
                    rejectedTask = onRejected(t.Exception.InnerException);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    tcs.SetException(e);
                    return;
                }

                if (rejectedTask.IsCompleted)
                {
                    tcs.SetFromCompleted(rejectedTask);
                    return;
                }

                rejectedTask.ContinueWith(
                    rt => tcs.SetFromCompleted(rt), CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return tcs.Task;
        }

        public static Task<T> Catch<T>(this Task<T> task, Func<Exception, T> onRejected)
        {
            task.NotNull(nameof(task));
            onRejected.NotNull(nameof(onRejected));

            if (task.IsCompleted)
            {
                if (task.IsCanceled || task.Exception?.InnerException == null)
                    return task;

                try
                {
                    return Promise.Resolve(onRejected(task.Exception.InnerException));
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    return Promise.Reject<T>(e);
                }
            }

            var tcs = new TaskCompletionSource<T>();

            task.ContinueWith(t =>
            {
                if (t.Exception?.InnerException == null)
                {
                    if (t.IsCanceled)
                    {
                        tcs.SetCanceled();
                        return;
                    }

                    tcs.SetResult(t.Result);
                    return;
                }

                try
                {
                    tcs.SetResult(onRejected(t.Exception.InnerException));
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

        public static Task Catch(this Task task, Func<Exception, Task> onRejected)
        {
            task.NotNull(nameof(task));
            onRejected.NotNull(nameof(onRejected));

            if (task.IsCompleted)
            {
                if (task.IsCanceled || task.Exception?.InnerException == null)
                    return task;

                try
                {
                    return onRejected(task.Exception.InnerException);
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
                if (t.Exception?.InnerException == null)
                {
                    if (t.IsCanceled)
                    {
                        tcs.SetCanceled();
                        return;
                    }

                    tcs.SetResult(true);
                    return;
                }

                Task rejectedTask;
                try
                {
                    rejectedTask = onRejected(t.Exception.InnerException);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    tcs.SetException(e);
                    return;
                }

                if (rejectedTask.IsCompleted)
                {
                    tcs.SetFromCompleted(rejectedTask);
                    return;
                }

                rejectedTask.ContinueWith(
                    rt => tcs.SetFromCompleted(rt), CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return tcs.Task;
        }

        public static Task Catch(this Task task, Action<Exception> onRejected)
        {
            task.NotNull(nameof(task));
            onRejected.NotNull(nameof(onRejected));

            if (task.IsCompleted)
            {
                if (task.IsCanceled || task.Exception?.InnerException == null)
                    return task;

                try
                {
                    onRejected(task.Exception.InnerException);
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
                if (t.Exception?.InnerException == null)
                {
                    if (t.IsCanceled)
                    {
                        tcs.SetCanceled();
                        return;
                    }

                    tcs.SetResult(true);
                    return;
                }

                try
                {
                    onRejected(t.Exception.InnerException);
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
