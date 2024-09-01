using Microsoft.UI.Dispatching;

namespace ZipViewer.Helpers
{
    public static class ThreadingHelper
    {
        private static DispatcherQueue dispatcher;

        public static void InitializeForMainThread()
        {
            dispatcher = DispatcherQueue.GetForCurrentThread();
        }

        public static bool TryEnqueue(Action action)
        {
            return dispatcher.TryEnqueue(new DispatcherQueueHandler(action));
        }
    }
}
