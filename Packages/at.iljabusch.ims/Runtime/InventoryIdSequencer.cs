using System.Threading;

namespace IMS
{
    public static class AtomicIntSequencer
    {
        private static int _currentValue;

        /// <summary>
        ///     Atomically increments the counter and returns the new value.
        /// </summary>
        public static int GetNext()
        {
            return Interlocked.Increment(ref _currentValue);
        }

        /// <summary>
        ///     Resets the counter to zero.
        /// </summary>
        public static void Reset()
        {
            Interlocked.Exchange(ref _currentValue, 0);
        }
    }
}