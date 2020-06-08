using BetterIOT.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BetterIOT.Common.DriveConfig
{
    public class MelsecLinksOverTcpConfig : NetworkDeviceConfigBase
    {
        /// <summary>
        /// PLC IP地址
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// PLC站号
        /// </summary>
        public byte StationNo { get; set; }
        /// <summary>
        /// 等待时间(范围0-15,单位ms)
        /// </summary>
        public byte WaittingTime { get; set; }
        /// <summary>
        /// 和校验
        /// </summary>
        public bool SumCheck { get; set; } = false;
    }
}
