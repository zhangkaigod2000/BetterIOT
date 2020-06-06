using BetterIOT.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BetterIOT.Common.DriveConfig
{
    public class DLT645Config : ConfigBase
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
        public IEnumerable<DLData> Commands { get; set; }
    }

    public class DLData
    {
        public string DevNO { get; set; }

        public string Name { get; set; }

        public string DataAddress { get; set; }

        public string Address { get; set; }
        /// <summary>
        /// 发送出去的请求命令
        /// </summary>
        public byte[] Command { get; set; }

        public int ByteCount { get; set; }

        public string Unit { get; set; }
        public string Xj { get; set; }
    }
}
