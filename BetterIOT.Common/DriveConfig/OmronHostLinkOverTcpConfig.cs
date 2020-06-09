using BetterIOT.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BetterIOT.Common.DriveConfig
{
    public class OmronHostLinkOverTcpConfig : NetworkDeviceConfigBase
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
        /// 设备标识号
        /// </summary>
        public byte SID { get; set; }
        /// <summary>
        /// PLC单元号地址一般为0
        /// </summary>
        public byte DA2 { get; set; } = 0;
        /// <summary>
        /// 上位机单元号地址
        /// </summary>
        public byte SA2 { get; set; }
        /// <summary>
        /// 等待时间
        /// </summary>
        public int ConnectTimeOut { get; set; }
        /// <summary>
        /// HslCommunication.Core.DataFormat
        /// </summary>
        public int ByteTransformDataFormat { get; set; }
        /// <summary>
        /// 读取失败的时候自动变换SA1的值
        /// </summary>
        public bool IsChangeSA1AfterReadFailed { get; set; }
    }
}
