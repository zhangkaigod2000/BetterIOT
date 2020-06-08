using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication;
using HslCommunication.Profinet.Melsec;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Mitsubishi.PLC.MelsecFx_LinksOverTcp
{
    public class MelsecLinksOverTcpDrive : NetworkDeviceBaseTemplate<MelsecLinksOverTcpConfig>
    {
        public override void DeviceConn(MelsecLinksOverTcpConfig config)
        {
            MelsecFxLinksOverTcp linksOverTcp = new MelsecFxLinksOverTcp();
            linksOverTcp.IpAddress = config.IP;
            linksOverTcp.Port = config.Port;
            linksOverTcp.SumCheck = config.SumCheck;
            linksOverTcp.Station = config.StationNo;
            linksOverTcp.WaittingTime = config.WaittingTime;
            OperateResult connect = linksOverTcp.ConnectServer();
            NetworkDevice = linksOverTcp;
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            ((MelsecFxLinksOverTcp)NetworkDevice).ConnectClose();
        }
    }
}
