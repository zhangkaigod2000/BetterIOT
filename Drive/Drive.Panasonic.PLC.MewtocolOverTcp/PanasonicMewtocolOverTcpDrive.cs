using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication;
using HslCommunication.Profinet.Panasonic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Panasonic.PLC.MewtocolOverTcp
{
    public class PanasonicMewtocolOverTcpDrive : NetworkDeviceBaseTemplate<PanasonicMewtocolOverTcpConfig>
    {
        public override void DeviceConn(PanasonicMewtocolOverTcpConfig config)
        {
            PanasonicMewtocolOverTcp panasonicMewtocol = new PanasonicMewtocolOverTcp(config.StationNo);
            panasonicMewtocol.IpAddress = config.IP;
            panasonicMewtocol.Port = config.Port;
            OperateResult connect = panasonicMewtocol.ConnectServer();
            NetworkDevice = panasonicMewtocol;
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            ((PanasonicMewtocolOverTcp)NetworkDevice).ConnectClose();
        }
    }
}
