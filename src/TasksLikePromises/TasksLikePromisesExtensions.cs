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
    }
}
