using BetterIOT.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BetterIOT.Common.DriveConfig
{
    public class XGBFastEnetConfig : NetworkDeviceConfigBase
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
        /// 
        /// </summary>
        public byte SlotNo { get; set; }
        /// <summary>
        /// PLC CPU 类型
        /// </summary>
        public string CpuType { get; set; }
        public string CompanyID { get; set; }
    }
}
