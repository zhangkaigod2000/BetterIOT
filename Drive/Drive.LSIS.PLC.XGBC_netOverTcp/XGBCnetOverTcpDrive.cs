using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication;
using HslCommunication.Profinet.LSIS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.LSIS.PLC.XGBC_netOverTcp
{
    public class XGBCnetOverTcpDrive : NetworkDeviceBaseTemplate<XGBCnetOverTcpConfig>
    {
        public override void DeviceConn(XGBCnetOverTcpConfig config)
        {
            XGBCnetOverTcp xGBCnet = new XGBCnetOverTcp();
            xGBCnet.IpAddress = config.IP;
            xGBCnet.Port = config.Port;
            xGBCnet.Station = config.StationNo;
            OperateResult connect = xGBCnet.ConnectServer();
            NetworkDevice = xGBCnet;
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            ((XGBCnetOverTcp)NetworkDevice).ConnectClose();
        }
    }
}
