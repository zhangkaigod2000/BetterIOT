using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication;
using HslCommunication.Profinet.Keyence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Keyence.PLC.Nano_SerialOverTcp
{
    public class KeyenceNanoSerialOverTcpDrive : NetworkDeviceBaseTemplate<KeyenceNanoSerialOverTcpConfig>
    {
        public override void DeviceConn(KeyenceNanoSerialOverTcpConfig config)
        {
            KeyenceNanoSerialOverTcp keyenceNano = new KeyenceNanoSerialOverTcp();
            keyenceNano.IpAddress = config.IP;
            keyenceNano.Port = config.Port;
            OperateResult connect = keyenceNano.ConnectServer();
            NetworkDevice = keyenceNano;
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            ((KeyenceNanoSerialOverTcp)NetworkDevice).ConnectClose();
        }
    }
}
