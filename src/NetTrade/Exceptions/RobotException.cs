using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Enums;

namespace NetTrade.Exceptions
{
    public class RobotException: Exception
    {
        public RobotException(RobotExceptionSource source, Exception innerException): base(source.ToString(), innerException)
        {
            RobotExceptionSource = source;
        }

        public RobotExceptionSource RobotExceptionSource { get; }
    }
}
