using System;

namespace NetTrade.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class RobotAttribute : Attribute
    {
        public string Name { get; set; }

        public string Group { get; set; }

        public object Tag { get; set; }
    }
}