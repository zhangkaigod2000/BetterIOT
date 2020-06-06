using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.GBxfxy.UseData
{
    /// <summary>
    /// 数据明细
    /// </summary>
    public class DataDetail
    {
        public string DeviceName { get; set; }

        public Dictionary<string,string> DataValue { get; set; }
    }
}
