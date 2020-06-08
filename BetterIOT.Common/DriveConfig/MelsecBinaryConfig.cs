using BetterIOT.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BetterIOT.Common.DriveConfig
{
    public class MelsecBinaryConfig : NetworkDeviceConfigBase
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
        /// 连接超时时间(毫秒)
        /// </summary>
        public int ConnTimeOut { get; set; } = 5000;
    }
}
