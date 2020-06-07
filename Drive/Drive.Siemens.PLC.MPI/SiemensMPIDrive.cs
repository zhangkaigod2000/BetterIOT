using BetterIOT.Common;
using BetterIOT.Common.Base;
using BetterIOT.Common.DriveConfig;
using HslCommunication;
using HslCommunication.Profinet.Siemens;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace Drive.Siemens.PLC.MPI
{
    public class SiemensMPIDrive : IDrive<SiemensMPIConfig>
    {
        SiemensMPI siemensMPI = null;

        SerialPort serialPort = new SerialPort();
        public override void DeviceConn(SiemensMPIConfig config)
        {
            siemensMPI = new SiemensMPI();
            try
            {
                SetPort();
                siemensMPI.SerialPortInni(serialPort.PortName, serialPort.BaudRate, serialPort.DataBits, serialPort.StopBits, serialPort.Parity);
                siemensMPI.Open();
                siemensMPI.Station = config.StationNo;
                OperateResult hand = siemensMPI.Handle();
                if (!hand.IsSuccess)
                {
                    Console.WriteLine("Hand Failed:" + hand.Message);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
            siemensMPI?.Close();
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
                            sResult = siemensMPI.ReadBool(result.DB).Content.ToString();
                            break;
                        case "STRING":
                            sResult = siemensMPI.ReadString(result.DB, Convert.ToUInt16(result.Len)).Content;
                            break;
                        case "INT":
                            sResult = siemensMPI.ReadInt32(result.DB).Content.ToString();
                            break;
                        case "FLOAT":
                            sResult = siemensMPI.ReadFloat(result.DB).Content.ToString(result.Format);
                            break;
                        case "DOUBLE":
                            sResult = siemensMPI.ReadDouble(result.DB).Content.ToString(result.Format);
                            break;
                        case "BYTE":
                            sResult = siemensMPI.ReadByte(result.DB).Content.ToString();
                            break;
                        case "SHORT":
                            sResult = siemensMPI.ReadInt16(result.DB).Content.ToString();
                            break;
                        case "USHORT":
                            sResult = siemensMPI.ReadUInt16(result.DB).Content.ToString();
                            break;
                        case "UINT":
                            sResult = siemensMPI.ReadUInt32(result.DB).Content.ToString();
                            break;
                        case "LONG":
                            sResult = siemensMPI.ReadInt64(result.DB).Content.ToString();
                            break;
                        case "ULONG":
                            sResult = siemensMPI.ReadUInt64(result.DB).Content.ToString();
                            break;
                        default:
                            sResult = siemensMPI.ReadString(result.DB, 1).Content;
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
