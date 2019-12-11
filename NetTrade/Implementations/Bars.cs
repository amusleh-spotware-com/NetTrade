using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Abstractions;

namespace NetTrade.Implementations
{
    public class Bars : IBars
    {
        public ITimeSeries Time => throw new NotImplementedException();

        public IDataSeries Open => throw new NotImplementedException();

        public IDataSeries High => throw new NotImplementedException();

        public IDataSeries Low => throw new NotImplementedException();

        public IDataSeries Close => throw new NotImplementedException();

        
    }
}
