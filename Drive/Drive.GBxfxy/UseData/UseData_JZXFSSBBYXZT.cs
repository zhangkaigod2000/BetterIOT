using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.GBxfxy.UseData
{
    public class UseData_JZXFSSBBYXZT : UseDataBase
    {
        Dictionary<int, string> MMM = new Dictionary<int, string>();
        /// <summary>
        /// 上传建筑消防设施部件运行状态
        /// </summary>
        /// <param name="UseBt"></param>
        public UseData_JZXFSSBBYXZT(byte[] UseBt) : base(UseBt)
        {
            MakeMMM();
            MyDataType = 2;
            Len = 40;

            //开始分解
            List<DataDetail> dataDetails = new List<DataDetail>();

            Dictionary<string, string> pairs = new Dictionary<string, string>();

            int SysType = UseBt[0];   //系统类型
            int SysAddr = UseBt[1];    //系统 地址
            int CompType = UseBt[4];    //部件类型 应该是2 测试环境改为3
            if (MMM.Count(e => e.Key == CompType) == 0)
            {
                pairs.Add("部件类型", "预留");
            }
            else
            {
                pairs.Add("部件类型", MMM[CompType]);
            }

            byte[] CompAddr = new byte[4];        //部件地址
            CompAddr[0] = UseBt[6];
            CompAddr[1] = UseBt[5];
            CompAddr[2] = UseBt[4];
            CompAddr[3] = UseBt[3];
            //pairs.Add("部件地址", Hex2Int(CompAddr).ToString().PadLeft(9, '0'));
            byte[] CompState = new byte[2];        //部件状态
            CompState[0] = UseBt[7];
            CompState[1] = UseBt[8];
            int iState = CompState[1] * 256 + CompState[0];   //根据协议来看，低位在前面

            string strState = Convert.ToString(iState, 2).PadLeft(16, '0');
            char[] cState = strState.ToArray();
            pairs.Add("运行状态", cState[15] == '0' ? "测试状态" : "正常运行状态");
            if (cState[14] != '0')
            {
                pairs.Add("报警状态", cState[14] == '0' ? "无火警" : "火警");
            }
            if (cState[13] != '0')
            {
                pairs.Add("故障状态", cState[13] == '0' ? "无故障" : "故障");
            }
            if (cState[12] != '0')
            {
                pairs.Add("屏蔽状态", cState[12] == '0' ? "无屏蔽" : "屏蔽");
            }
            if (cState[11] != '0')
            {
                pairs.Add("监管状态", cState[11] == '0' ? "无监管" : "监管");
            }
            if (cState[10] != '0')
            {
                pairs.Add("启停状态", cState[10] == '0' ? "停止" : "启动");
            }
            if (cState[9] != '0')
            {
                pairs.Add("延时状态", cState[9] == '0' ? "未延时" : "延时");
            }
            if (cState[8] != '0')
            {
                pairs.Add("电源状态", cState[8] == '0' ? "电源正常" : "电源故障");
            }
            dataDetails.Add(new DataDetail()
            {
                DeviceName = "部件",
                DataValue = pairs
            });
            DataDetails = dataDetails;

        }

        private int Hex2Int(byte[] Bts)
        {
            string strTmp = "";
            for (int i = Bts.GetUpperBound(0); i >= 0; i--)
            {
                strTmp = strTmp + String.Format("{0:X}", Bts[i]).PadLeft(2, '0');
            }
            int Result = Hex2Ten(strTmp);
            return Result;
        }

        private int Hex2Ten(string hex)
        {
            int ten = 0;
            for (int i = 0, j = hex.Length - 1; i < hex.Length; i++)
            {
                ten += HexChar2Value(hex.Substring(i, 1)) * ((int)Math.Pow(16, j));
                j--;
            }
            return ten;
        }

        private int HexChar2Value(string hexChar)
        {
            switch (hexChar)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    return Convert.ToInt32(hexChar);
                case "a":
                case "A":
                    return 10;
                case "b":
                case "B":
                    return 11;
                case "c":
                case "C":
                    return 12;
                case "d":
                case "D":
                    return 13;
                case "e":
                case "E":
                    return 14;
                case "f":
                case "F":
                    return 15;
                default:
                    return 0;
            }
        }

        private void MakeMMM()
        {
            MMM.Add(0, "通用");
            MMM.Add(1, "火灾报警控制器");
            MMM.Add(10, "可燃气体探测器");
            MMM.Add(11, "点型可燃气体探测器");
            MMM.Add(12, "独立式可燃气体探测器");
            MMM.Add(13, "线型可燃气体探测器");
            MMM.Add(16, "电气火灾监控报警器");
            MMM.Add(17, "剩余电流式电气火灾监控探测器");
            MMM.Add(18, "测温式电气火灾监控探测器");
            MMM.Add(21, "探测回路");
            MMM.Add(22, "火灾显示盘");
            MMM.Add(23, "手动火灾报警按钮");
            MMM.Add(24, "消火栓按钮");
            MMM.Add(25, "火灾探测器");
            MMM.Add(30, "感温火灾探测器");
            MMM.Add(31, "点型感温火灾探测器");
            MMM.Add(32, "点型感温火灾探测器（S型）");
            MMM.Add(33, "点型感温火灾探测器（R型）");
            MMM.Add(34, "线型感温火灾探测器");
            MMM.Add(35, "线型感温火灾探测器（S型）");
            MMM.Add(36, "线型感温火灾探测器（R型）");
            MMM.Add(37, "光纤感温火灾探测器");
            MMM.Add(40, "感烟火灾探测器");
            MMM.Add(41, "点型离子感烟火灾探测器");
            MMM.Add(42, "点型光电感烟火灾探测器");
            MMM.Add(43, "线型光束感烟火灾探测器");
            MMM.Add(44, "吸气式感烟火灾探测器");
            MMM.Add(50, "复合式火灾探测器");
            MMM.Add(51, "复合式感烟感温火灾探测器");
            MMM.Add(52, "复合式感光感温火灾探测器");
            MMM.Add(53, "复合式感光感烟火灾探测器");
            MMM.Add(61, "紫外火焰探测器");
            MMM.Add(62, "红外火焰探测器");
            MMM.Add(69, "感光火焰探测器");
            MMM.Add(74, "气体探测器");
            MMM.Add(78, "图像摄像方式火灾探测器");
            MMM.Add(79, "感声火灾探测器");
            MMM.Add(81, "气体灭火控制器");
            MMM.Add(82, "消防电气控制装置");
            MMM.Add(83, "消防控制室图形显示装置");
            MMM.Add(84, "模块");
            MMM.Add(85, "输入模块");
            MMM.Add(86, "输出模块");
            MMM.Add(87, "输入/输出模块");
            MMM.Add(88, "中继模块");
            MMM.Add(91, "消防水泵");
            MMM.Add(92, "消防水箱");
            MMM.Add(95, "喷淋泵");
            MMM.Add(96, "水流指示器");
            MMM.Add(97, "信号阀");
            MMM.Add(98, "报警阀");
            MMM.Add(99, "压力开关");
            MMM.Add(101, "阀驱动装置");
            MMM.Add(102, "防火门");
            MMM.Add(103, "防火阀");
            MMM.Add(104, "通风空调");
            MMM.Add(105, "泡沫液泵");
            MMM.Add(106, "管网电磁阀");
            MMM.Add(111, "防烟排烟风机");
            MMM.Add(113, "排烟防火阀");
            MMM.Add(114, "常闭送风口");
            MMM.Add(115, "排烟口");
            MMM.Add(116, "电控挡烟垂壁");
            MMM.Add(117, "防火卷帘控制器");
            MMM.Add(118, "防火门监控器");
            MMM.Add(121, "报警装置");
        }
    }
}
