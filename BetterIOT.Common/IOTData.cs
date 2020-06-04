using LiteDB;
using System;

namespace BetterIOT.Common
{
    public class IOTData
    {
        [BsonId]
        public Guid ID { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 设备序列号
        /// </summary>
        public string DevSerialNo { get; set; }
        /// <summary>
        /// 设备型号
        /// </summary>
        public string DevModel { get; set; }
        /// <summary>
        /// 数据名称
        /// </summary>
        public string DataName { get; set; }
        /// <summary>
        /// 数据代码
        /// </summary>
        public string DataCode { get; set; }
        /// <summary>
        /// 数据值
        /// </summary>
        public string DataValue { get; set; }
        /// <summary>
        /// 获取时间
        /// </summary>
        public DateTime GTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 是否发送出去
        /// </summary>
        public bool Sended { get; set; } = false;
    }
}
