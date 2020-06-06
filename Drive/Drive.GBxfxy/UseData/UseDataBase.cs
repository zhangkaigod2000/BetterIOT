using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.GBxfxy.UseData
{
    public abstract class UseDataBase
    {

        /// <summary>
        /// 类型标志
        /// </summary>
        public static int DataType { get; set; }

        /// <summary>
        /// 信息数目
        /// </summary>
        public int MsgCount { get; set; }

        public UseDataBase(byte[] UseBt)
        {
            DataType = UseBt[0];
            MsgCount = UseBt[1];
        }

        public int Len;

        /// <summary>
        /// 我处理的数据内容
        /// </summary>
        public int MyDataType { get; set; }

        public IEnumerable<DataDetail> DataDetails { get; set; }
    }
}
