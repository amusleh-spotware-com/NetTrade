using System;

namespace NetTrade.Abstractions.Interfaces
{
    public interface IServer
    {
        DateTimeOffset CurrentTime { get; }

        void SetTime(IRobot robot, DateTimeOffset time);
    }
}