using BetterIOT.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BetterIOT.Common.DriveConfig
{
    public class XGBCnetConfig : NetworkDeviceConfigBase
    {
        /// <summary>
        /// 端口
        /// </summary>
        public string PortName { get; set; }
        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate { get; set; }
        /// <summary>
        /// 校验
        /// </summary>
        public string Parity { get; set; }
        /// <summary>
        /// 数据位
        /// </summary>
        public int DataBits { get; set; }
        /// <summary>
        /// 停止位
        /// </summary>
        public string StopBits { get; set; }
        /// <summary>
        /// PLC站号
        /// </summary>
        public byte StationNo { get; set; }
    }
}
