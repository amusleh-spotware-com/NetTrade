using NetTrade.Abstractions.Interfaces;
using NetTrade.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace NetTrade.Models
{
    public class TimerContainer : ITimerContainer
    {
        private readonly List<ITimer> _timers = new List<ITimer>();

        private readonly Timer _systemTimer;

        private readonly Mode _mode;

        public TimerContainer(Mode mode)
        {
            _mode = mode;

            if (_mode == Mode.Live)
            {
                _systemTimer = new Timer();
                _systemTimer.Elapsed += SystemTimer_Elapsed;
            }
        }

        public IReadOnlyList<ITimer> Timers => _timers;

        public void Dispose()
        {
            _systemTimer.Dispose();

            _timers.Clear();
        }

        public void PauseSystemTimer()
        {
            _systemTimer?.Stop();
        }

        public void RegisterTimer(ITimer timer)
        {
            _timers.Add(timer);

            if (_mode == Mode.Live)
            {
                _systemTimer.Stop();

                _systemTimer.Interval = _timers.Select(iTimer => iTimer.Interval.TotalMilliseconds).Min();

                _systemTimer.Start();
            }
        }

        public bool RemoveTimer(ITimer timer)
        {
            if (!_timers.Contains(timer))
            {
                return false;
            }

            _timers.Remove(timer);

            if (_mode == Mode.Live && _timers.Count == 0)
            {
                _systemTimer.Stop();
            }

            return true;
        }

        public void ResumeSystemTimer()
        {
            _systemTimer?.Start();
        }

        public void SetCurrentTime(DateTimeOffset currentTime)
        {
            foreach (var timer in Timers)
            {
                timer.SetCurrentTime(currentTime);
            }
        }

        private void SystemTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SetCurrentTime(DateTimeOffset.Now);
        }
    }
}