using System;
using System.Collections.Generic;
using System.Text;

namespace NetTrade.Interfaces
{
    public interface IServer
    {
        DateTimeOffset CurrentTime { get; }
    }
}
