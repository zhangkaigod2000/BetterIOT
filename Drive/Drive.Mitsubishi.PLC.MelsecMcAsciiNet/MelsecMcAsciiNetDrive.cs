using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using System;
using System.Collections.Generic;
using System.Text;
using HslCommunication;
using HslCommunication.Profinet.Melsec;

namespace Drive.Mitsubishi.PLC.MelsecMcAsciiNe
{
    public class MelsecMcAsciiNetDrive : NetworkDeviceBaseTemplate<MelsecMcAsciiNetConfig>
    {
        public override void DeviceConn(MelsecMcAsciiNetConfig config)
        {
            MelsecMcAsciiNet melsecMcAsciiNet = new MelsecMcAsciiNet();
            melsecMcAsciiNet.IpAddress = config.IP;
            melsecMcAsciiNet.Port = config.Port;
            OperateResult connect = melsecMcAsciiNet.ConnectServer();
            NetworkDevice = melsecMcAsciiNet;
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            ((MelsecMcAsciiNet)NetworkDevice).ConnectClose();
        }
    }
}
