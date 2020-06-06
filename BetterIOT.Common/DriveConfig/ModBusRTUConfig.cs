using BetterIOT.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BetterIOT.Common.DriveConfig
{
    /// <summary>
    /// ModBusRTU的数据配置
    /// </summary>
    public class ModBusRTUConfig : ConfigBase
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
        /// 发起命令
        /// </summary>
        public IEnumerable<SDTCommand> Commands { get; set; }
    }


    public class SDTCommand
    {
        /// <summary>
        /// 设备地址
        /// </summary>
        public string Addr { get; set; }
        /// <summary>
        /// 功能码
        /// </summary>
        public string GCode { get; set; }
        /// <summary>
        /// 起始位置
        /// </summary>
        public string StartR { get; set; }
        /// <summary>
        /// 读取长度
        /// </summary>
        public string ReadLen { get; set; }
        [JsonIgnore]
        /// <summary>
        /// 发送出去的请求命令
        /// </summary>
        public byte[] Command { get; set; } = null;

        /// <summary>
        /// 运算可以获取到的结果
        /// </summary>
        public IEnumerable<Result> Results { get; set; }
    }

    /// <summary>
    /// 计算结果部分
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 计算表达式
        /// </summary>
        public string Expressions { get; set; }

        /// <summary>
        /// 数据返回地址
        /// </summary>
        public string Address { get; set; }
        public string Unit { get; set; }
    }
}
