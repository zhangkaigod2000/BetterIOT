using BetterIOT.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BetterIOT.Common.DriveConfig
{
    public class OPCUAConfig : ConfigBase
    {
        /// <summary>
        /// 连接方式 0.匿名  1.账号密码  2.证书
        /// </summary>
        public int ConnType { get; set; } = 0;
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Passwd { get; set; }
        /// <summary>
        /// 证书路径
        /// </summary>
        public string CertPath { get; set; }
        /// <summary>
        /// 证书密钥
        /// </summary>
        public string CertKey { get; set; }
        /// <summary>
        /// OPC UA TCP连接路径(例:opc.tcp://118.24.36.220:62547/DataAccessServer)
        /// </summary>
        public string OPCUATCPPath { get; set; }

        public IEnumerable<OPCUANode> Nodes { get; set; }
    }

    public class OPCUANode
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// NodeID(例:ns=2;s=Devices/分厂一/车间二/ModbusTcp客户端/温度)
        /// </summary>
        public string NodeID { get; set; }

        /// <summary>
        /// 格式化
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 数据地址
        /// </summary>
        public string Address { get; set; }
        public string Unit { get; set; }
    }
}
