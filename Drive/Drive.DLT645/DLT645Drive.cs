using BetterIOT.Common;
using BetterIOT.Common.Base;
using BetterIOT.Common.DriveConfig;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace Drive.DLT645
{
    public class DLT645Drive : IDrive<DLT645Config>
    {
        SerialPort serialPort = new SerialPort();
        List<DLData> Commands;
        public override void DeviceConn(DLT645Config config)
        {
            Commands = DriveConfig.Commands.ToList();
            for (int i = 0; i < Commands.Count; i++)
            {//预处理command数据
                try
                {
                    Commands[i].Command = SendData(Commands[i].DevNO, Commands[i]);
                }
                catch (Exception ex)
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
                    serialPort.ReadTimeout = 3000;
                    serialPort.Open();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private string CalcReceive(byte[] btData, string format)
        {
            int Lenindex = 13;
            int Lens = btData[13];
            StringBuilder sb = new StringBuilder();
            for (int i = Lenindex + Lens; i > Lenindex; i--)
            {
                sb.Append((btData[i] - 0x33).ToString("X").PadLeft(2, '0'));
            }
            return (Convert.ToDouble(sb.ToString()) * Convert.ToDouble(format)).ToString();
        }


        public override void DeviceDiscnn()
        {
            serialPort.Close();
        }
        private byte[] SendData(string DEVNO, DLData data)
        {
            byte[] send;
            if (data.ByteCount == 2)
            {
                send = new byte[18];
            }
            else if (data.ByteCount == 4)
            {
                send = new byte[20];
            }
            else
            {
                send = new byte[20];
            }
            // FE FE FE FE 68 23 91 78 56 34 12 68 11 04 33 36 35 35 80 16
            //头包
            send[0] = send[1] = send[2] = send[3] = 0xFE;   //发4字节的0xFE
            send[4] = 0x68;
            //设备地址
            send[5] = Convert.ToByte(DEVNO.Substring(10, 2), 16);
            send[6] = Convert.ToByte(DEVNO.Substring(8, 2), 16);
            send[7] = Convert.ToByte(DEVNO.Substring(6, 2), 16);
            send[8] = Convert.ToByte(DEVNO.Substring(4, 2), 16);
            send[9] = Convert.ToByte(DEVNO.Substring(2, 2), 16);
            send[10] = Convert.ToByte(DEVNO.Substring(0, 2), 16);
            //分割
            send[11] = 0x68;
            send[12] = 0x11;
            //长度
            send[13] = (byte)data.ByteCount;
            //功能码区
            send[14] = Convert.ToByte(data.DataAddress.Substring(0, 2), 16);
            send[15] = Convert.ToByte(data.DataAddress.Substring(2, 2), 16);
            send[16] = Convert.ToByte(data.DataAddress.Substring(4, 2), 16);
            send[17] = Convert.ToByte(data.DataAddress.Substring(6, 2), 16);

            send[18] = Calculate_Parity(send, 4, 18);
            send[19] = 0x16;
            return send;
        }
        private byte Calculate_Parity(byte[] data, byte num1, byte num2) //num1是FE数量，num2是数组大小,奇偶校验
        {
            byte i;
            byte sum = 0;
            for (i = num1; i < num2; i++)
            {
                sum += data[i];
            }
            return sum;
        }

        public override IEnumerable<IOTData> GetData()
        {
            List<IOTData> iots = new List<IOTData>();
            foreach (DLData data in Commands)
            {
                byte[] Bt1 = new byte[200];
                serialPort.Write(data.Command, 0, data.Command.Length);
                System.Threading.Thread.Sleep(500);
                int bytesRead = serialPort.Read(Bt1, 0, 200);
                if (bytesRead > 0)
                {
                    try
                    {
                        //计算结果
                        IOTData _IOTData = new IOTData();
                        _IOTData.DataCode = data.Address;
                        _IOTData.DataValue = CalcReceive(Bt1, data.Xj);
                        _IOTData.GTime = DateTime.Now;
                        _IOTData.DriveCode = DriveConfig.DriveCode;
                        _IOTData.DriveType = DriveConfig.DriveType;
                        _IOTData.DataName = data.Name;
                        _IOTData.Unit = data.Unit;
                        if (_IOTData.DataValue.Trim().Length > 0)
                        {
                            iots.Add(_IOTData);
                        }
                    }
                    catch
                    {

                    }
                }
            }
            return iots;
        }
    }
}
