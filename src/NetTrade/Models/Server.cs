using NetTrade.Abstractions.Interfaces;
using System;

namespace NetTrade.Models
{
    public class Server : IServer
    {
        private IRobot _robot;

        public DateTimeOffset CurrentTime { get; private set; }

        public void SetTime(IRobot robot, DateTimeOffset time)
        {
            if (_robot == null)
            {
                _robot = robot;
            }

            if (_robot != robot)
            {
                throw new ArgumentException($"{nameof(robot)} doesn't match the server robot");
            }

            CurrentTime = time;
        }
    }
}