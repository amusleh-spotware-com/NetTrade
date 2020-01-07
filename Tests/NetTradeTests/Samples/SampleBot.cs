using NetTrade.Abstractions;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Attributes;

namespace NetTradeTests.Samples
{
    public class SampleBot : Robot
    {
        public SampleBot(IRobotSettings settings) : base(settings)
        {
        }

        [Parameter("Periods", DefaultValue = 10, MaxValue = 100, MinValue = 10, Step = 10)]
        public int Periods { get; set; }

        [Parameter("Deviation", DefaultValue = 1, MaxValue = 3, MinValue = 1, Step = 0.5)]
        public double Deviation { get; set; }

        [Parameter("Range", DefaultValue = 1000, MaxValue = 10000, MinValue = 1000, Step = 1000)]
        public long Range { get; set; }
    }
}