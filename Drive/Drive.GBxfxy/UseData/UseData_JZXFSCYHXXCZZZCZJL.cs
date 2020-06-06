using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.GBxfxy.UseData
{
    public class UseData_JZXFSCYHXXCZZZCZJL : UseDataBase
    {
        /// <summary>
        /// 上传用户信息传输装置操作记录
        /// </summary>
        /// <param name="UseBt"></param>
        public UseData_JZXFSCYHXXCZZZCZJL(byte[] UseBt) : base(UseBt)
        {
            MyDataType = 24;
            Len = 8;
            int HeadLen = 2;
            int ICount = UseBt[1];

            List<DataDetail> dataDetails = new List<DataDetail>();
            for (int i = 0; i < ICount; i++)
            {
                Dictionary<string, string> pairs = new Dictionary<string, string>();
                int CtrlInfo = UseBt[i * Len + HeadLen];    //操作信息
                int UserCode = UseBt[i * Len + HeadLen + 1];    //操作员编号
                byte[] TimeCode = new byte[6];
                TimeCode[0] = UseBt[i * Len + HeadLen + 2];
                TimeCode[1] = UseBt[i * Len + HeadLen + 3];
                TimeCode[2] = UseBt[i * Len + HeadLen + 4];
                TimeCode[3] = UseBt[i * Len + HeadLen + 5];
                TimeCode[4] = UseBt[i * Len + HeadLen + 6];
                TimeCode[5] = UseBt[i * Len + HeadLen + 7];
                string strState = Convert.ToString(CtrlInfo, 2).PadLeft(8,'0');
                char[] cState = strState.ToArray();
                //if (cState[7] != '0')
                //{
                pairs.Add("复位操作", cState[7] == '0' ? "无操作" : "复位");
                //}
                if (cState[6] != '0')
                {
                    pairs.Add("消音操作", cState[6] == '0' ? "无操作" : "消音");
                }
                if (cState[5] != '0')
                {
                    pairs.Add("报警操作", cState[5] == '0' ? "无操作" : "手动报警");
                }
                if (cState[4] != '0')
                {
                    pairs.Add("警情操作", cState[4] == '0' ? "无操作" : "警情消除");
                }
                if (cState[3] != '0')
                {
                    pairs.Add("自检操作", cState[3] == '0' ? "无操作" : "自检");
                }
                if (cState[2] != '0')
                {
                    pairs.Add("查岗操作", cState[2] == '0' ? "无操作" : "查岗应答");
                }
                if (cState[1] != '0')
                {
                    pairs.Add("测试操作", cState[1] == '0' ? "无操作" : "测试");
                }
                DateTime TimeC = Convert.ToDateTime(DateTime.Now.Year.ToString().Substring(0, 2)
                + TimeCode[5].ToString().Trim().PadLeft(2, '0') + "-" + TimeCode[4].ToString().Trim().PadLeft(2, '0')
                + "-" + TimeCode[3].ToString().Trim().PadLeft(2, '0') + " " + TimeCode[2].ToString().Trim().PadLeft(2, '0')
                + ":" + TimeCode[1].ToString().Trim().PadLeft(2, '0') + ":" + TimeCode[0].ToString().Trim().PadLeft(2, '0'));

                pairs.Add("操作员编号", UserCode.ToString());
                pairs.Add("操作时间", TimeC.ToString("yyyy-MM-dd HH:mm:ss"));
                dataDetails.Add(new DataDetail()
                {
                    DeviceName = "用户信息传输装置操作记录" + (i + 1).ToString(),
                    DataValue = pairs
                });
            }


            DataDetails = dataDetails;
        }
    }
}
