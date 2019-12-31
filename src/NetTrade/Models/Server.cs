using NetTrade.Abstractions.Interfaces;
using System;

namespace NetTrade.Models
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