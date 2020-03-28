using NetTrade.Abstractions.Interfaces;
using System;
using System.Collections.Generic;

namespace NetTrade.Models
{
    public class TimerContainer : ITimerContainer
    {
        private readonly List<ITimer> _timers = new List<ITimer>();

        public IReadOnlyList<ITimer> Timers => _timers;

        public void RegisterTimer(ITimer timer)
        {
            _timers.Add(timer);
        }

        public bool RemoveTimer(ITimer timer)
        {
            if (!_timers.Contains(timer))
            {
                return false;
            }

            _timers.Remove(timer);

            return true;
        }

        public void SetCurrentTime(DateTimeOffset currentTime)
        {
            foreach (var timer in Timers)
            {
                timer.SetCurrentTime(currentTime);
            }
        }
    }
}