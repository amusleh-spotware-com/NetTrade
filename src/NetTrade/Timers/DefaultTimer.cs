using NetTrade.Abstractions.Interfaces;
using NetTrade.Helpers;
using System;

namespace NetTrade.Timers
{
    public class DefaultTimer : ITimer
    {
        public TimeSpan Interval { get; private set; }

        public bool Enabled { get; private set; }

        public DateTimeOffset LastElapsedTime { get; private set; }

        public event OnTimerElapsedHandler OnTimerElapsedEvent;

        public event OnTimerStartHandler OnTimerStartEvent;

        public event OnTimerStopHandler OnTimerStopEvent;

        public event OnTimerIntervalChangedHandler OnTimerIntervalChangedEvent;

        public void SetCurrentTime(DateTimeOffset currentTime)
        {
            if (!Enabled)
            {
                return;
            }

            if (currentTime - LastElapsedTime >= Interval)
            {
                LastElapsedTime = currentTime;

                OnTimerElapsedEvent?.Invoke(this);
            }
        }

        public void Start()
        {
            Enabled = true;

            OnTimerStartEvent?.Invoke(this);
        }

        public void Stop()
        {
            Enabled = false;

            OnTimerStopEvent?.Invoke(this);
        }

        public void SetInterval(TimeSpan interval)
        {
            Interval = interval;

            OnTimerIntervalChangedEvent?.Invoke(this, interval);
        }
    }
}