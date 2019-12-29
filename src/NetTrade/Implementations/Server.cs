using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Interfaces;

namespace NetTrade.Implementations
{
    public class Server : IServer
    {
        private DateTimeOffset? _currentTime;

        public DateTimeOffset CurrentTime
        {
            get => _currentTime.HasValue ? _currentTime.Value : DateTimeOffset.Now;
            set => _currentTime = value;
        }
    }
}
