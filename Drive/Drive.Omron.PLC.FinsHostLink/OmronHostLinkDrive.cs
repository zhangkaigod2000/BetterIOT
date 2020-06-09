using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication.Profinet.Omron;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace Drive.Omron.PLC.FinsHostLink
{
    public class OmronHostLinkDrive : NetworkDeviceBaseTemplate<OmronHostLinkConfig>
    {
        SerialPort serialPort = new SerialPort();
        public override void DeviceConn(OmronHostLinkConfig config)
        {
            OmronHostLink hostLink = new OmronHostLink();
            SetPort();
            hostLink.SerialPortInni(serialPort.PortName, serialPort.BaudRate, serialPort.DataBits, serialPort.StopBits, serialPort.Parity);
            hostLink.UnitNumber = config.StationNo;
            hostLink.SID = config.SID;
            hostLink.DA2 = config.DA2;
            hostLink.SA2 = config.SA2;
            hostLink.ByteTransform.DataFormat = (HslCommunication.Core.DataFormat)config.ByteTransformDataFormat;
            hostLink.Open();
            NetworkDevice = hostLink;
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
            ((OmronHostLink)NetworkDevice).Close();
        }
    }
}
