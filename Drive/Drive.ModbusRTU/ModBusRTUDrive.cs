using BetterIOT.Common;
using BetterIOT.Common.Base;
using BetterIOT.Common.DriveConfig;
using BetterIOT.Common.Func;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace Drive.ModbusRTU
{
    public class ModBusRTUDrive : IDrive<ModBusRTUConfig>
    {
        SerialPort serialPort = new SerialPort();
        List<SDTCommand> Commands;
        int StartNum = 3; //数据起始位

        /// <summary>
        /// 字符串运算
        /// </summary>
        CustomFormulas _CustomFormulas = new CustomFormulas();
        public override void DeviceConn(ModBusRTUConfig config)
        {
            Commands = config.Commands.ToList();
            for (int i = 0;i < Commands.Count;i++)
            {//预处理command数据
                try
                {
                    Commands[i].Command = new byte[8];
                    byte[] _TmpBt = new byte[6];
                    byte[] CRCByte;
                    _TmpBt[0] = Commands[i].Command[0] = (byte)Convert.ToInt32(Commands[i].Addr, 16);
                    _TmpBt[1] = Commands[i].Command[1] = (byte)Convert.ToInt32(Commands[i].GCode, 16);
                    _TmpBt[2] = Commands[i].Command[2] = (byte)Convert.ToInt32("00", 16);
                    _TmpBt[3] = Commands[i].Command[3] = (byte)Convert.ToInt32(Commands[i].StartR, 16);
                    _TmpBt[4] = Commands[i].Command[4] = (byte)Convert.ToInt32("00", 16);
                    _TmpBt[5] = Commands[i].Command[5] = (byte)Convert.ToInt32(Commands[i].ReadLen, 16);
                    CRCByte = CRC.CRC16_C(_TmpBt);
                    Commands[i].Command[6] = CRCByte[1];
                    Commands[i].Command[7] = CRCByte[0];
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            OpenPort();
        }
        private void OpenPort()
        {
            try
            {
                if (serialPort.IsOpen == true)
                {
                    return;
                }
                else
                {
                    //打开
                    serialPort.PortName = DriveConfig.PortName;
                    serialPort.BaudRate = DriveConfig.BaudRate;
                    switch (DriveConfig.Parity)
                    {
                        case "NONE":
                            serialPort.Parity = System.IO.Ports.Parity.None;
                            break;
                        case "EVEN":
                            serialPort.Parity = System.IO.Ports.Parity.Even;
                            break;
                        case "ODD":
                            serialPort.Parity = System.IO.Ports.Parity.Odd;
                            break;
                        case "MARK":
                            serialPort.Parity = System.IO.Ports.Parity.Mark;
                            break;
                        case "SPACE":
                            serialPort.Parity = System.IO.Ports.Parity.Space;
                            break;
                    }
                    serialPort.DataBits = DriveConfig.DataBits;
                    switch (DriveConfig.StopBits)
                    {
                        case "1":
                            serialPort.StopBits = System.IO.Ports.StopBits.One;
                            break;
                        case "1.5":
                            serialPort.StopBits = System.IO.Ports.StopBits.OnePointFive;
                            break;
                        case "2":
                            serialPort.StopBits = System.IO.Ports.StopBits.Two;
                            break;
                    }
                    serialPort.ReadTimeout = 1000;
                    serialPort.Open();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public override void DeviceDiscnn()
        {
            serialPort.Close();
        }

        public override IEnumerable<IOTData> GetData()
        {
            List<IOTData> iots = new List<IOTData>();
            foreach (SDTCommand SDTCmd in Commands)
            {
                try
                {
                    if (SDTCmd.Results.Count() == 0)
                    {
                        continue;
                    }
                    serialPort.Write(SDTCmd.Command, 0, SDTCmd.Command.Length);
                    byte[] Bt1 = new byte[2048];
                    System.Threading.Thread.Sleep(300);
                    int bytesRead = serialPort.Read(Bt1, 0, 2048);
                    if (bytesRead > 0)
                    {
                        IEnumerable<IOTData> tempdata = GetData(Bt1);
                        if (tempdata != null)
                        {
                            iots.AddRange(tempdata);
                        }
                        Bt1 = null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                System.Threading.Thread.Sleep(10);
            }
            return iots;
        }

        private IEnumerable<IOTData> GetData(byte[] ReplyMsg)
        {
            List<IOTData> Ds = new List<IOTData>();
            //大于0说明有收到数据！没有收到就说明发送出去的结果有异常
            int iNum = ReplyMsg[2];
            if (Commands.Any(n => n.Command[0] == ReplyMsg[0]) == false)
            {
                return null;
            }
            SDTCommand TCmd = Commands.Single(n => n.Command[0] == ReplyMsg[0]);
            //判断是否是同样的命令功能
            if (ReplyMsg[1] != TCmd.Command[1])
            {
                return null;
            }
            string strGroup = Guid.NewGuid().ToString("N");
            foreach (Result Rs in TCmd.Results)
            {
                StringBuilder Sb = new StringBuilder();
                Sb.Append(Rs.Expressions);
                int DataCount = ReplyMsg[2];
                //替换公式中的字符
                for (int i = 0; i < DataCount; i++)
                {
                    string strTmp1 = "[D" + i.ToString() + "]";
                    string strTmp2 = ReplyMsg[StartNum + i].ToString();
                    Sb.Replace(strTmp1, strTmp2);
                    strTmp2 = null;
                    strTmp1 = null;
                }
                try
                {
                    IOTData _IOTData = new IOTData();
                    _IOTData.DataCode = Rs.Address;
                    _IOTData.DataName = Rs.Name;
                    _IOTData.DataValue = _CustomFormulas.Computing(Sb.ToString());
                    _IOTData.DriveCode = DriveConfig.DriveCode;
                    _IOTData.DriveType = DriveConfig.DriveType;
                    _IOTData.GTime = DateTime.Now;
                    _IOTData.Unit = Rs.Unit;
                    if (_IOTData.DataValue.Trim().Length > 0)
                    {
                        Ds.Add(_IOTData);
                    }
                }
                catch
                {

                }
                Sb = null;
            }
            return Ds;
        }
    }
}
