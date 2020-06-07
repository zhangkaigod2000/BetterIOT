using BetterIOT.Common;
using BetterIOT.Common.Base;
using BetterIOT.Common.DriveConfig;
using HslCommunication;
using HslCommunication.Profinet.Siemens;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace Drive.Siemens.PLC.PPI
{
    public class SiemensPPIDrive : IDrive<SiemensPPIConfig>
    {
        SiemensPPI siemensPPI = null;
        SerialPort serialPort = new SerialPort();
        public override void DeviceConn(SiemensPPIConfig config)
        {
            siemensPPI = new SiemensPPI();
            SetPort();
            siemensPPI.SerialPortInni(serialPort.PortName, serialPort.BaudRate, serialPort.DataBits, serialPort.StopBits, serialPort.Parity);
            siemensPPI.Open();
            siemensPPI.Station = config.StationNo;
            siemensPPI.RtsEnable = config.RtsEnable;
            //OperateResult start = siemensPPI.Start();
            //if (start.IsSuccess) MessageBox.Show("Start Success!");
            //else MessageBox.Show(start.Message);
        }

        private void SetPort()
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
        }

        public override void DeviceDiscnn()
        {
            //OperateResult stop = siemensPPI.Stop();
            siemensPPI?.Close();
        }

        public override IEnumerable<IOTData> GetData()
        {
            List<IOTData> iOTs = new List<IOTData>();

            foreach (SiemensMPIResult result in DriveConfig.Results)
            {
                try
                {
                    string sResult;
                    switch (result.DataType.ToUpper())
                    {
                        case "BOOL":
                            sResult = siemensPPI.ReadBool(result.DB).Content.ToString();
                            break;
                        case "STRING":
                            sResult = siemensPPI.ReadString(result.DB, Convert.ToUInt16(result.Len)).Content;
                            break;
                        case "INT":
                            sResult = siemensPPI.ReadInt32(result.DB).Content.ToString();
                            break;
                        case "FLOAT":
                            sResult = siemensPPI.ReadFloat(result.DB).Content.ToString(result.Format);
                            break;
                        case "DOUBLE":
                            sResult = siemensPPI.ReadDouble(result.DB).Content.ToString(result.Format);
                            break;
                        case "BYTE":
                            sResult = siemensPPI.ReadByte(result.DB).Content.ToString();
                            break;
                        case "SHORT":
                            sResult = siemensPPI.ReadInt16(result.DB).Content.ToString();
                            break;
                        case "USHORT":
                            sResult = siemensPPI.ReadUInt16(result.DB).Content.ToString();
                            break;
                        case "UINT":
                            sResult = siemensPPI.ReadUInt32(result.DB).Content.ToString();
                            break;
                        case "LONG":
                            sResult = siemensPPI.ReadInt64(result.DB).Content.ToString();
                            break;
                        case "ULONG":
                            sResult = siemensPPI.ReadUInt64(result.DB).Content.ToString();
                            break;
                        default:
                            sResult = siemensPPI.ReadString(result.DB, 1).Content;
                            break;
                    }
                    iOTs.Add(new IOTData
                    {
                        DataCode = result.Address,
                        DataValue = sResult,
                        DataName = result.Name,
                        DriveCode = DriveConfig.DriveCode,
                        DriveType = DriveConfig.DriveType,
                        GTime = DateTime.Now,
                        Unit = result.Unit
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return iOTs;
        }
    }
}
