using System;
using System.Diagnostics;

namespace RESS.Shared.Utils
{
    public static class Stopper
    {
        public static void TickTime(Action action)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            action.Invoke();
            stopWatch.Stop();
        }
    }
}