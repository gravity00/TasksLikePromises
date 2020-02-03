// ReSharper disable once CheckNamespace
namespace System.Threading.Tasks
{
    public static partial class TasksLikePromisesExtensions
    {
        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        internal static void NotNull<T>(this T instance, string paramName) where T : class
        {
            if (instance == null)
                throw new ArgumentNullException(paramName);
        }

#if NET40

        private static void SetFromCompleted<T>(this TaskCompletionSource<T> tcs, Task<T> task)
        {
            if (task.Exception?.InnerException != null)
                tcs.SetException(task.Exception.InnerExceptions);
            else if (task.IsCanceled)
                tcs.SetCanceled();
            else
                tcs.SetResult(task.Result);
        }

        private static void SetFromCompleted(this TaskCompletionSource<bool> tcs, Task task)
        {
            if (task.Exception?.InnerException != null)
                tcs.SetException(task.Exception.InnerExceptions);
            else if (task.IsCanceled)
                tcs.SetCanceled();
            else
                tcs.SetResult(true);
        }

#endif
    }
}
