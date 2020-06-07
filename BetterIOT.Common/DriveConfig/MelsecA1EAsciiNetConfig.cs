using BetterIOT.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BetterIOT.Common.DriveConfig
{
    public class MelsecA1EAsciiNetConfig : ConfigBase
    {
        /// <summary>
        /// PLC IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
        public IEnumerable<MelsecA1EAsciiNetResult> Results { get; set; }
    }

    public class MelsecA1EAsciiNetResult : S7NetResult
    {

    }
}
