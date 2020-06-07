using BetterIOT.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BetterIOT.Common.DriveConfig
{
    public class SiemensPPIOverTcpConfig : ConfigBase
    {
        /// <summary>
        /// PLC IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; } = 102;
        /// <summary>
        /// 西门子PLC站号
        /// </summary>
        public byte StationNo { get; set; }

        public IEnumerable<SiemensPPIOverTcpResult> Results { get; set; }

    }

    public class SiemensPPIOverTcpResult : S7NetResult
    {

    }
}
