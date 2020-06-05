using System;
using System.Collections.Generic;
using System.Text;

namespace BetterIOT.Common
{
    /// <summary>
    /// 驱动控制命令
    /// </summary>
    public class DriveControl
    {
        public const string CMD_SHUTDOWN = "shutdown";

        public string DriveCode { get; set; }

        public string Cmd { get; set; }
    }
}
