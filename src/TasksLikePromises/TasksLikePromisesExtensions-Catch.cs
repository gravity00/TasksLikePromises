#if !NET40

// ReSharper disable once CheckNamespace
namespace System.Threading.Tasks
{
    public static partial class TasksLikePromisesExtensions
    {
        public static async Task<T> Catch<T>(this Task<T> task, Func<Exception, Task<T>> onRejected)
        {
            task.NotNull(nameof(task));
            onRejected.NotNull(nameof(onRejected));

            try
            {
                return await task.ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return await onRejected(e).ConfigureAwait(false);
            }
        }

        public static async Task<T> Catch<T>(this Task<T> task, Func<Exception, T> onRejected)
        {
            task.NotNull(nameof(task));
            onRejected.NotNull(nameof(onRejected));

            try
            {
                return await task.ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return onRejected(e);
            }
        }

        public static async Task Catch(this Task task, Func<Exception, Task> onRejected)
        {
            task.NotNull(nameof(task));
            onRejected.NotNull(nameof(onRejected));

            try
            {
                await task.ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                await onRejected(e).ConfigureAwait(false);
            }
        }

        public static async Task Catch(this Task task, Action<Exception> onRejected)
        {
            task.NotNull(nameof(task));
            onRejected.NotNull(nameof(onRejected));

            try
            {
                await task.ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                onRejected(e);
            }
        }
    }
}

#endif
