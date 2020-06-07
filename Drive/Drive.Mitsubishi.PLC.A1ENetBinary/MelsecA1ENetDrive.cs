using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication;
using HslCommunication.Profinet.Melsec;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Mitsubishi.PLC.A1ENetBinary
{
    public class MelsecA1ENetDrive : NetworkDeviceBaseTemplate<MelsecA1ENetConfig>
    {
        public override void DeviceConn(MelsecA1ENetConfig config)
        {
            NetworkDevice = new MelsecA1ENet();
            ((MelsecA1ENet)NetworkDevice).IpAddress = config.IP;
            ((MelsecA1ENet)NetworkDevice).Port = config.Port;
            if (!System.Net.IPAddress.TryParse(config.IP, out System.Net.IPAddress address))
            {
                throw new Exception("IpAddress input wrong");
            }
            ((MelsecA1ENet)NetworkDevice).ConnectClose();
            OperateResult connect = ((MelsecA1ENet)NetworkDevice).ConnectServer();
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            ((MelsecA1ENet)NetworkDevice).ConnectClose();
        }
    }
}
