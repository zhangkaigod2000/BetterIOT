using BetterIOT.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BetterIOT.Common.Base
{
    public class NetworkDeviceConfigBase : ConfigBase
    {

        public IEnumerable<NetworkDeviceResult> Results { get; set; }
    }

    /// <summary>
    /// 结果类
    /// </summary>
    public class NetworkDeviceResult
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据所属DB
        /// </summary>
        public string DB { get; set; }

        /// <summary>
        /// 起始位
        /// </summary>
        public string Start { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public int Len { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }

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
