#if !NET40

// ReSharper disable once CheckNamespace
namespace System.Threading.Tasks
{
    public static partial class TasksLikePromisesExtensions
    {
        public static async Task<T> Finally<T>(this Task<T> task, Func<Task> onFinally)
        {
            task.NotNull(nameof(task));
            onFinally.NotNull(nameof(onFinally));

            try
            {
                return await task.ConfigureAwait(false);
            }
            finally
            {
                await onFinally().ConfigureAwait(false);
            }
        }

        public static async Task<T> Finally<T>(this Task<T> task, Action onFinally)
        {
            task.NotNull(nameof(task));
            onFinally.NotNull(nameof(onFinally));

            try
            {
                return await task.ConfigureAwait(false);
            }
            finally
            {
                onFinally();
            }
        }

        public static async Task Finally(this Task task, Func<Task> onFinally)
        {
            task.NotNull(nameof(task));
            onFinally.NotNull(nameof(onFinally));

            try
            {
                await task.ConfigureAwait(false);
            }
            finally
            {
                await onFinally().ConfigureAwait(false);
            }
        }

        public static async Task Finally(this Task task, Action onFinally)
        {
            task.NotNull(nameof(task));
            onFinally.NotNull(nameof(onFinally));

            try
            {
                await task.ConfigureAwait(false);
            }
            finally
            {
                onFinally();
            }
        }
    }
}

#endif
