using System.Threading.Tasks;

namespace Mitheti.Core.Watcher
{
    public static class Extensions
    {
        public static Task ThrowNoExceptionOnCancelled(this Task task)
        {
            return task.ContinueWith((task) =>
            {
                if (!task.IsCanceled && task.Exception != null)
                {
                    throw task.Exception;
                }
            });
        }
    }
}
