using Drive.GBxfxy.UseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.GBxfxy
{
    public class XfBusiness
    {
        public byte[] BusiNo = new byte[2];     //业务流水号
        public XfBusiness(byte[] BtData)
        {
            byte[] StartCode = new byte[2];     //启动符
            byte[] AgreementNO = new byte[2];     //协议版本号
            byte[] TimeCode = new byte[6];     //时间戳
            byte[] YAddr = new byte[6];     //源地址
            byte[] EAddr = new byte[6];     //目标地址
            byte[] DataLen = new byte[2];     //数据长度
            byte CmdData = new byte();     //命令字节
            byte[] UseData;                //应用数据
            byte CacData = new byte();     //校验和
            byte[] EndCode = new byte[2];   //结束符
            int iNu = 0;
            int UseDataLen = 0;           //应用数据长度
            StartCode[0] = BtData[0];
            StartCode[1] = BtData[1];
            BusiNo[0] = BtData[2];
            BusiNo[1] = BtData[3];
            AgreementNO[0] = BtData[4];
            AgreementNO[1] = BtData[5];
            TimeCode[0] = BtData[6];
            TimeCode[1] = BtData[7];
            TimeCode[2] = BtData[8];
            TimeCode[3] = BtData[9];
            TimeCode[4] = BtData[10];
            TimeCode[5] = BtData[11];
            YAddr[0] = BtData[12];
            YAddr[1] = BtData[13];
            YAddr[2] = BtData[14];
            YAddr[3] = BtData[15];
            YAddr[4] = BtData[16];
            YAddr[5] = BtData[17];
            EAddr[0] = BtData[18];
            EAddr[1] = BtData[19];
            EAddr[2] = BtData[20];
            EAddr[3] = BtData[21];
            EAddr[4] = BtData[22];
            EAddr[5] = BtData[23];
            DataLen[0] = BtData[24];
            DataLen[1] = BtData[25];
            CmdData = BtData[26];
            UseDataLen = DataLen[1] * 256 + DataLen[0];   //根据协议来看，低位在前面
            UseData = new byte[UseDataLen];
            iNu = 26; 
            iNu++;
            //找出应用数据部分
            for (int i = 0;i < UseDataLen; i++)
            {
                UseData[i] = BtData[iNu];
                iNu++;
            }
            CacData = BtData[iNu];
            iNu++;
            EndCode[0] = BtData[iNu];
            iNu++;
            EndCode[1] = BtData[iNu];
            TimeC = Convert.ToDateTime(DateTime.Now.Year.ToString().Substring(0, 2)
                + TimeCode[5].ToString().Trim().PadLeft(2, '0') + "-" + TimeCode[4].ToString().Trim().PadLeft(2, '0')
                + "-" + TimeCode[3].ToString().Trim().PadLeft(2, '0') + " " + TimeCode[2].ToString().Trim().PadLeft(2, '0')
                + ":" + TimeCode[1].ToString().Trim().PadLeft(2, '0') + ":" + TimeCode[0].ToString().Trim().PadLeft(2, '0'));
            AddrCode = Hex2Int(YAddr).ToString().PadLeft(14,'0');
            switch (CmdData)
            {
                case 0:
                    Cmd = "预留";
                    break;
                case 1:
                    Cmd = "控制命令";
                    break;
                case 2:
                    Cmd = "发送数据";
                    break;
                case 3:
                    Cmd = "确认";
                    break;
                case 4:
                    Cmd = "请求";
                    break;
                case 5:
                    Cmd = "应答";
                    break;
                case 6:
                    Cmd = "否认";
                    break;
                default:
                    Cmd = "未知的";
                    break;
            }
            switch(UseData[0])
            {
                case 24:
                    useData = new UseData_JZXFSCYHXXCZZZCZJL(UseData);
                    //getsss(BtData);
                    break;
                case 21:
                    useData = new UseData_JZXFSCYHXXZZ(UseData);
                    //getsss(BtData);
                    break;
                case 25:
                    useData = new UseData_JZXFSCYYXXCSZZRXBB(UseData);
                    break;
                case 2:
                    useData = new UseData_JZXFSSBBYXZT(UseData);
                    //getsss(BtData);
                    break;
                default:
                    useData = null;
                    break;
            }



        }

        private void getsss(byte[] ss)
        {
            string strTmp = "";
            for (int i = 0;i <= ss.GetUpperBound(0); i++)
            {
                strTmp = strTmp + Convert.ToString(ss[i], 16) + " ";
            }
            
            Console.WriteLine(strTmp);
        }

        /// <summary>
        /// 地址编号
        /// </summary>
        public string AddrCode { get; set; }

        /// <summary>
        ///时间戳
        /// </summary>
        public DateTime TimeC { get; set; }

        /// <summary>
        /// 命令定义
        /// </summary>
        public string Cmd { get; set; }

        public UseDataBase useData { get; set; }

        public string strBusNO()
        {
            return Hex2Int(BusiNo).ToString();
        }

        private int Hex2Int(byte[] Bts)
        {
            string strTmp = "";
            for (int i = Bts.GetUpperBound(0); i >= 0; i--)
            {
                strTmp = strTmp + String.Format("{0:X}", Bts[i]).PadLeft(2,'0');
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
    }
}
