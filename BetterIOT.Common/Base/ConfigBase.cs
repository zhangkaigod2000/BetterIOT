using System;
using System.Collections.Generic;
using System.Text;

namespace BetterIOT.Common.Base
{
    public class ConfigBase
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public string DriveCode { get; set; }

        /// <summary>
        /// 循环周期
        /// </summary>
        public int CycleTime { get; set; }
    }
}
