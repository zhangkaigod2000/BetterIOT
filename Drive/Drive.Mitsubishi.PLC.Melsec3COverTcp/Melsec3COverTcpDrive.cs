using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication;
using HslCommunication.Profinet.Melsec;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Mitsubishi.PLC.Melsec3COverTcp
{
    public class Melsec3COverTcpDrive : NetworkDeviceBaseTemplate<Melsec3COverTcpConfig>
    {
        public override void DeviceConn(Melsec3COverTcpConfig config)
        {
            MelsecA3CNet1OverTcp melsecA3C = new MelsecA3CNet1OverTcp();
            melsecA3C.IpAddress = config.IP;
            melsecA3C.Port = config.Port;
            melsecA3C.Station = config.StationNo;
            OperateResult connect = melsecA3C.ConnectServer();
            NetworkDevice = melsecA3C;
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            ((MelsecA3CNet1OverTcp)NetworkDevice).ConnectClose();
        }
    }
}
