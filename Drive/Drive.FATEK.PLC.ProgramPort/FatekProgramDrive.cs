using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication.Profinet.FATEK;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace Drive.FATEK.PLC.ProgramPort
{
    public class FatekProgramDrive : NetworkDeviceBaseTemplate<FatekProgramConfig>
    {
        SerialPort serialPort = new SerialPort();
        public override void DeviceConn(FatekProgramConfig config)
        {
            FatekProgram fatek = new FatekProgram();
            SetPort();
            fatek.SerialPortInni(serialPort.PortName, serialPort.BaudRate, serialPort.DataBits, serialPort.StopBits, serialPort.Parity);
            fatek.Station = config.StationNo;
            fatek.Open();
            NetworkDevice = fatek;
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
            ((FatekProgram)NetworkDevice).Close();
        }
    }
}
