using NetTrade.Abstractions.Interfaces;
using NetTrade.Enums;
using NetTrade.Helpers;
using System;
using System.Timers;

namespace NetTrade.Timers
{
    public class DefaultTimer : ITimer
    {
        private readonly Timer _systemTimer;

        private readonly Mode _mode;

        public DefaultTimer(Mode mode)
        {
            _mode = mode;

            if (_mode == Mode.Live)
            {
                _systemTimer = new Timer();

                _systemTimer.Elapsed += SystemTimer_Elapsed;
            }
        }

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

            if (_mode == Mode.Live)
            {
                _systemTimer.Interval = Interval.TotalMilliseconds;

                _systemTimer.Start();
            }
        }

        public void Stop()
        {
            Enabled = false;

            OnTimerStopEvent?.Invoke(this);

            _systemTimer?.Stop();
        }

        public void SetInterval(TimeSpan interval)
        {
            Interval = interval;

            OnTimerIntervalChangedEvent?.Invoke(this, interval);
        }

        public void Dispose()
        {
            _systemTimer?.Dispose();
        }

        private void SystemTimer_Elapsed(object sender, ElapsedEventArgs e) => SetCurrentTime(DateTimeOffset.Now);
    }
}