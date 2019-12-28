using System;

namespace NetTrade.Implementations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class ParameterAttribute : Attribute
    {
        public ParameterAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public object DefaultValue { get; set; }

        public object MinValue { get; set; }

        public object MaxValue { get; set; }

        public object Step { get; set; }

        public string Group { get; set; }
    }
}