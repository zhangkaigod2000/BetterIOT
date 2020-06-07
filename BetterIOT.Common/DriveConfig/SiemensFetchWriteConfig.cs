using BetterIOT.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BetterIOT.Common.DriveConfig
{
    public class SiemensFetchWriteConfig : ConfigBase
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
        /// 结果值
        /// </summary>
        public IEnumerable<SiemensFetchWriteResult> Results { get; set; }

    }

    public class SiemensFetchWriteResult : S7NetResult
    {

    }
}
