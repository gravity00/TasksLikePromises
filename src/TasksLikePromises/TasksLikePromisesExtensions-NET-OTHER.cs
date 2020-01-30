#if !NET40

// ReSharper disable once CheckNamespace
namespace System.Threading.Tasks
{
    public static partial class TasksLikePromisesExtensions
    {
        public static async Task<TResult> Then<T, TResult>(this Task<T> task, Func<T, Task<TResult>> onFulfilled)
        {
            task.NotNull(nameof(task));
            onFulfilled.NotNull(nameof(onFulfilled));

            var t = await task.ConfigureAwait(false);

            return await onFulfilled(t).ConfigureAwait(false);
        }

        public static async Task<TResult> Then<T, TResult>(this Task<T> task, Func<T, TResult> onFulfilled)
        {
            task.NotNull(nameof(task));
            onFulfilled.NotNull(nameof(onFulfilled));

            var t = await task.ConfigureAwait(false);

            return onFulfilled(t);
        }

        public static async Task Then<T>(this Task<T> task, Func<T, Task> onFulfilled)
        {
            task.NotNull(nameof(task));
            onFulfilled.NotNull(nameof(onFulfilled));

            var t = await task.ConfigureAwait(false);

            await onFulfilled(t).ConfigureAwait(false);
        }

        public static async Task Then<T>(this Task<T> task, Action<T> onFulfilled)
        {
            task.NotNull(nameof(task));
            onFulfilled.NotNull(nameof(onFulfilled));

            var t = await task.ConfigureAwait(false);

            onFulfilled(t);
        }

        public static async Task<TResult> Then<TResult>(this Task task, Func<Task<TResult>> onFulfilled)
        {
            task.NotNull(nameof(task));
            onFulfilled.NotNull(nameof(onFulfilled));

            await task.ConfigureAwait(false);

            return await onFulfilled().ConfigureAwait(false);
        }

        public static async Task<TResult> Then<TResult>(this Task task, Func<TResult> onFulfilled)
        {
            task.NotNull(nameof(task));
            onFulfilled.NotNull(nameof(onFulfilled));

            await task.ConfigureAwait(false);

            return onFulfilled();
        }

        public static async Task Then(this Task task, Func<Task> onFulfilled)
        {
            task.NotNull(nameof(task));
            onFulfilled.NotNull(nameof(onFulfilled));

            await task.ConfigureAwait(false);

            await onFulfilled().ConfigureAwait(false);
        }

        public static async Task Then(this Task task, Action onFulfilled)
        {
            task.NotNull(nameof(task));
            onFulfilled.NotNull(nameof(onFulfilled));

            await task.ConfigureAwait(false);

            onFulfilled();
        }
    }
}

#endif
