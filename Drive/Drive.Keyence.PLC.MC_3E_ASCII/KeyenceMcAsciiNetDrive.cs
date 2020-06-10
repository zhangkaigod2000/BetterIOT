using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication;
using HslCommunication.Profinet.Keyence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Keyence.PLC.MC_3E_ASCII
{
    public class KeyenceMcAsciiNetDrive : NetworkDeviceBaseTemplate<KeyenceMcAsciiNetConfig>
    {
        public override void DeviceConn(KeyenceMcAsciiNetConfig config)
        {
            KeyenceMcAsciiNet keyenceMc = new KeyenceMcAsciiNet();
            keyenceMc.IpAddress = config.IP;
            keyenceMc.Port = config.Port;
            OperateResult connect = keyenceMc.ConnectServer();
            NetworkDevice = keyenceMc;
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            ((KeyenceMcAsciiNet)NetworkDevice).ConnectClose();
        }
    }
}
