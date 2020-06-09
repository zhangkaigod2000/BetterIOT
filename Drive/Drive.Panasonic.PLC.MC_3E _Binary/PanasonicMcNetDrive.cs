using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication;
using HslCommunication.Profinet.Panasonic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Panasonic.PLC.MC_3E__Binary
{
    public class PanasonicMcNetDrive : NetworkDeviceBaseTemplate<PanasonicMcNetConfig>
    {
        public override void DeviceConn(PanasonicMcNetConfig config)
        {
            PanasonicMcNet panasonic_net = new PanasonicMcNet();
            panasonic_net.IpAddress = config.IP;
            panasonic_net.Port = config.Port;
            OperateResult connect = panasonic_net.ConnectServer();
            NetworkDevice = panasonic_net;
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            ((PanasonicMcNet)NetworkDevice).ConnectClose();
        }
    }
}
