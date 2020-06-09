using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication.Profinet.LSIS;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace Drive.LSIS.PLC.XGBC_net
{
    public class XGBCnetDrive : NetworkDeviceBaseTemplate<XGBCnetConfig>
    {
        SerialPort serialPort = new SerialPort();
        public override void DeviceConn(XGBCnetConfig config)
        {
            XGBCnet gBCnet = new XGBCnet();
            gBCnet.Station = config.StationNo;
            gBCnet.SerialPortInni(serialPort.PortName, serialPort.BaudRate, serialPort.DataBits, serialPort.StopBits, serialPort.Parity);
            gBCnet.Open();
            NetworkDevice = gBCnet;
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
            ((XGBCnet)NetworkDevice).Close();
        }
    }
}
