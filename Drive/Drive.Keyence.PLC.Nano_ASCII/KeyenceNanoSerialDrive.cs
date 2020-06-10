using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication.Profinet.Keyence;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace Drive.Keyence.PLC.Nano_ASCII
{
    public class KeyenceNanoSerialDrive : NetworkDeviceBaseTemplate<KeyenceNanoSerialConfig>
    {
        SerialPort serialPort = new SerialPort();
        public override void DeviceConn(KeyenceNanoSerialConfig config)
        {
            KeyenceNanoSerial keyenceNano = new KeyenceNanoSerial();
            SetPort();
            keyenceNano.SerialPortInni(serialPort.PortName, serialPort.BaudRate, serialPort.DataBits, serialPort.StopBits, serialPort.Parity);
            keyenceNano.Open();
            NetworkDevice = keyenceNano;
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
            ((KeyenceNanoSerial)NetworkDevice).Close();
        }
    }
}
