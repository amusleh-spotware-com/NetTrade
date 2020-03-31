<h1>Timers</h1>

When your robot is running you might need to use a timer to run a code in a time interval, its one of the most things in algo trading and also a challenge, as in the backtesting mode the conventional .Net timers will not work due to iteration over time in backtest you can't get the behavior you want to from a conventional .Net timer, so to solve this issue NetTrade came with its own timers.

A NetTrade timer allows you to get same behavior on both backtesting and live trading, and a NetTrade robot can have several of these timers with different time intervals.

The robot interface has a TimerContainer, this container allows you to register your NetTrade times so your robot will know when to stop/pause/start the timers based on robot running state.

Your timer class must implement the NetTrade ITimer interface, or you can use the NetTrade default timer class at NetTrade.Timers namespace.

The below code example shows you how to use the timers on your robots:

```c#
    [Robot(Name = "Timer Bot", Group = "Sample")]
    public class TimerSampleBot : Robot
    {
        [Parameter("Interval", DefaultValue = "01:00:00")]
        public TimeSpan Interval { get; set; }

        public override void OnStart()
        {
            var timer = new DefaultTimer(Mode);

            timer.OnTimerElapsedEvent += Timer_OnTimerElapsedEvent;

            timer.SetInterval(Interval);

            TimerContainer.RegisterTimer(timer);

            timer.Start();
        }

        private void Timer_OnTimerElapsedEvent(object sender)
        {
            System.Diagnostics.Trace.WriteLine($"Timer_OnTimerElapsedEvent: {Server.CurrentTime.TimeOfDay}");
        }
    }
```

Don't forget to register your timer at TimerContainer, otherwise it will not work.