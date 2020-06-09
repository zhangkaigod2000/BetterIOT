using BetterIOT.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BetterIOT.Common.DriveConfig
{
    public class OmronHostLinkConfig : NetworkDeviceConfigBase
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
