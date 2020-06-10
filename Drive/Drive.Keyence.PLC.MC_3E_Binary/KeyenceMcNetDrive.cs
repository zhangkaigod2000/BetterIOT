using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication;
using HslCommunication.Profinet.Keyence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Keyence.PLC.MC_3E_Binary
{
    public class KeyenceMcNetDrive : NetworkDeviceBaseTemplate<KeyenceMcNetConfig>
    {
        public override void DeviceConn(KeyenceMcNetConfig config)
        {
            KeyenceMcNet keyenceMc = new KeyenceMcNet();
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
            ((KeyenceMcNet)NetworkDevice).ConnectClose();
        }
    }
}
