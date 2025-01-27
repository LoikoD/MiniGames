using System;

namespace CodeBase.Common.Timer
{
    public interface ITimer
    {
        event Action<double> OnTimeUpdated;

        void Init(double currentTime = 0);
        void Stop();
    }
}