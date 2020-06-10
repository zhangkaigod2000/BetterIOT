using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication;
using HslCommunication.Profinet.Fuji;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Fuji.PLC.SPBOverTcp
{
    public class FujiSPBOverTcpDrive : NetworkDeviceBaseTemplate<FujiSPBOverTcpConfig>
    {
        public override void DeviceConn(FujiSPBOverTcpConfig config)
        {
            FujiSPBOverTcp fujiSPBOverTcp = new FujiSPBOverTcp();
            fujiSPBOverTcp.IpAddress = config.IP;
            fujiSPBOverTcp.Port = config.Port;
            fujiSPBOverTcp.Station = config.StationNo;
            OperateResult connect = fujiSPBOverTcp.ConnectServer();
            NetworkDevice = fujiSPBOverTcp;
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            ((FujiSPBOverTcp)NetworkDevice).ConnectClose();
        }
    }
}
