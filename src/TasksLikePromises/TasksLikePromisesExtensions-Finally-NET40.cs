#if NET40

using TasksLikePromises;

// ReSharper disable once CheckNamespace
namespace System.Threading.Tasks
{
    public static partial class TasksLikePromisesExtensions
    {
        public static Task<T> Finally<T>(this Task<T> task, Func<Task> onFinally)
        {
            task.NotNull(nameof(task));
            onFinally.NotNull(nameof(onFinally));

            TaskCompletionSource<T> tcs;
            if (task.IsCompleted)
            {
                Task finallyTask;
                try
                {
                    finallyTask = onFinally();
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    return Promise.Reject<T>(e);
                }

                if (finallyTask.IsCompleted)
                {
                    if (finallyTask.Exception?.InnerException != null)
                        return Promise.Reject<T>(finallyTask.Exception.InnerExceptions);

                    return finallyTask.IsCanceled ? Promise.Cancel<T>() : task;
                }

                tcs = new TaskCompletionSource<T>();

                finallyTask.ContinueWith(ft =>
                {
                    if (ft.Exception?.InnerException != null)
                    {
                        tcs.SetException(ft.Exception.InnerExceptions);
                        return;
                    }

                    if (ft.IsCanceled)
                    {
                        tcs.SetCanceled();
                        return;
                    }

                    tcs.SetFromCompleted(task);
                }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

                return tcs.Task;
            }

            tcs = new TaskCompletionSource<T>();

            task.ContinueWith(t =>
            {
                Task finallyTask;
                try
                {
                    finallyTask = onFinally();
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    tcs.SetException(e);
                    return;
                }

                if (finallyTask.IsCompleted)
                {
                    if (finallyTask.Exception?.InnerException != null)
                    {
                        tcs.SetException(finallyTask.Exception.InnerExceptions);
                        return;
                    }

                    if (finallyTask.IsCanceled)
                    {
                        tcs.SetCanceled();
                        return;
                    }

                    tcs.SetFromCompleted(t);
                    return;
                }

                finallyTask.ContinueWith(ft =>
                {
                    if (ft.Exception?.InnerException != null)
                    {
                        tcs.SetException(ft.Exception.InnerExceptions);
                        return;
                    }

                    if (ft.IsCanceled)
                    {
                        tcs.SetCanceled();
                        return;
                    }

                    tcs.SetFromCompleted(t);
                }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return tcs.Task;
        }

        public static Task<T> Finally<T>(this Task<T> task, Action onFinally)
        {
            task.NotNull(nameof(task));
            onFinally.NotNull(nameof(onFinally));

            if (task.IsCompleted)
            {
                try
                {
                    onFinally();
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    return Promise.Reject<T>(e);
                }

                return task;
            }

            var tcs = new TaskCompletionSource<T>();

            task.ContinueWith(t =>
            {
                try
                {
                    onFinally();
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    tcs.SetException(e);
                    return;
                }

                tcs.SetFromCompleted(t);
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return tcs.Task;
        }
        
        public static Task Finally(this Task task, Func<Task> onFinally)
        {
            task.NotNull(nameof(task));
            onFinally.NotNull(nameof(onFinally));

            TaskCompletionSource<bool> tcs;
            if (task.IsCompleted)
            {
                Task finallyTask;
                try
                {
                    finallyTask = onFinally();
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    return Promise.Reject(e);
                }

                if (finallyTask.IsCompleted)
                {
                    if (finallyTask.Exception?.InnerException != null)
                        return Promise.Reject(finallyTask.Exception.InnerExceptions);

                    return finallyTask.IsCanceled ? Promise.Canceled : task;
                }

                tcs = new TaskCompletionSource<bool>();

                finallyTask.ContinueWith(ft =>
                {
                    if (ft.Exception?.InnerException != null)
                    {
                        tcs.SetException(ft.Exception.InnerExceptions);
                        return;
                    }

                    if (ft.IsCanceled)
                    {
                        tcs.SetCanceled();
                        return;
                    }

                    tcs.SetFromCompleted(task);
                }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

                return tcs.Task;
            }

            tcs = new TaskCompletionSource<bool>();

            task.ContinueWith(t =>
            {
                Task finallyTask;
                try
                {
                    finallyTask = onFinally();
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    tcs.SetException(e);
                    return;
                }

                if (finallyTask.IsCompleted)
                {
                    if (finallyTask.Exception?.InnerException != null)
                    {
                        tcs.SetException(finallyTask.Exception.InnerExceptions);
                        return;
                    }

                    if (finallyTask.IsCanceled)
                    {
                        tcs.SetCanceled();
                        return;
                    }

                    tcs.SetFromCompleted(t);
                    return;
                }

                finallyTask.ContinueWith(ft =>
                {
                    if (ft.Exception?.InnerException != null)
                    {
                        tcs.SetException(ft.Exception.InnerExceptions);
                        return;
                    }

                    if (ft.IsCanceled)
                    {
                        tcs.SetCanceled();
                        return;
                    }

                    tcs.SetFromCompleted(t);
                }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return tcs.Task;
        }

        public static Task Finally(this Task task, Action onFinally)
        {
            task.NotNull(nameof(task));
            onFinally.NotNull(nameof(onFinally));

            if (task.IsCompleted)
            {
                try
                {
                    onFinally();
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    return Promise.Reject(e);
                }

                return task;
            }

            var tcs = new TaskCompletionSource<bool>();

            task.ContinueWith(t =>
            {
                try
                {
                    onFinally();
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    tcs.SetException(e);
                    return;
                }

                tcs.SetFromCompleted(t);
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return tcs.Task;
        }
    }
}

#endif
