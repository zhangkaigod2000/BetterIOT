using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.GBxfxy.UseData
{
    /// <summary>
    /// 上传用户信息传输装置状态
    /// </summary>
    public class UseData_JZXFSCYHXXZZ : UseDataBase
    {
        /// <summary>
        /// 上传用户信息传输装置
        /// </summary>
        /// <param name="UseBt"></param>
        public UseData_JZXFSCYHXXZZ(byte[] UseBt) : base(UseBt)
        {
            MyDataType = 21;
            Len = 9;
            byte[] TimeCode = new byte[6];

            List<DataDetail> dataDetails = new List<DataDetail>();
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            int iState = UseBt[2];
            TimeCode[0] = UseBt[3];
            TimeCode[1] = UseBt[4];
            TimeCode[2] = UseBt[5];
            TimeCode[3] = UseBt[6];
            TimeCode[4] = UseBt[7];
            TimeCode[5] = UseBt[8];
            string strState = Convert.ToString(iState, 2).PadLeft(8, '0');
            char[] cState = strState.ToArray();
            pairs.Add("监视状态", cState[7] == '0' ? "测试状态" : "正常监视状态");
            if (cState[6] != '0')
            {
                pairs.Add("报警状态", cState[6] == '0' ? "无火警" : "火警");
            }
            if (cState[5] != '0')
            {
                pairs.Add("故障状态", cState[5] == '0' ? "无故障" : "故障");
            }
            if (cState[4] != '0')
            {
                pairs.Add("主电状态", cState[4] == '0' ? "主电正常" : "主电故障");
            }
            if (cState[3] != '0')
            {
                pairs.Add("备电状态", cState[3] == '0' ? "备电正常" : "备电故障");
            }
            if (cState[2] != '0')
            {
                pairs.Add("通讯状态", cState[2] == '0' ? "通信信道正常" : "与监控中心通信信道故障");
            }
            if (cState[1] != '0')
            {
                pairs.Add("连接线状态", cState[1] == '0' ? "监测连接线正常" : "监测连接线故障");
            }
            DateTime TimeC = Convert.ToDateTime(DateTime.Now.Year.ToString().Substring(0, 2)
                + TimeCode[5].ToString().Trim().PadLeft(2, '0') + "-" + TimeCode[4].ToString().Trim().PadLeft(2, '0')
                + "-" + TimeCode[3].ToString().Trim().PadLeft(2, '0') + " " + TimeCode[2].ToString().Trim().PadLeft(2, '0')
                + ":" + TimeCode[1].ToString().Trim().PadLeft(2, '0') + ":" + TimeCode[0].ToString().Trim().PadLeft(2, '0'));

            pairs.Add("时间", TimeC.ToString("yyyy-MM-dd HH:mm:ss"));
            dataDetails.Add(new DataDetail()
            {
                DeviceName = "上传用户信息传输装置",
                DataValue = pairs
            });
            DataDetails = dataDetails;
        }
    }
}
