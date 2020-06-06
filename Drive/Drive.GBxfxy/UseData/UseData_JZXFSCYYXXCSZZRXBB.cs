using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.GBxfxy.UseData
{
    /// <summary>
    /// 上传用户信息传输装置软件版本
    /// </summary>
    public class UseData_JZXFSCYYXXCSZZRXBB : UseDataBase
    {
        public UseData_JZXFSCYYXXCSZZRXBB(byte[] UseBt) : base(UseBt)
        {
            MyDataType = 25;
            Len = 4;
            int ICount = UseBt[1];
            List<DataDetail> dataDetails = new List<DataDetail>();

            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("主版本号", UseBt[2].ToString());
            pairs.Add("次版本号", UseBt[3].ToString());
            dataDetails.Add(new DataDetail()
            {
                DeviceName = "用户信息传输装置软件版本",
                DataValue = pairs
            });


            DataDetails = dataDetails;
        }
    }
}
