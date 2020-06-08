using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication.Profinet.Melsec;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Mitsubishi.PLC.MelsecMcAscii_Udp
{
    public class MelsecMcAsciiUdpDrive : NetworkDeviceBaseTemplate<MelsecMcAsciiUdpConfig>
    {
        public override void DeviceConn(MelsecMcAsciiUdpConfig config)
        {
            MelsecMcAsciiUdp melsecMcAsciiUdp = new MelsecMcAsciiUdp();
            melsecMcAsciiUdp.IpAddress = config.IP;
            melsecMcAsciiUdp.Port = config.Port;
            NetworkDevice = melsecMcAsciiUdp;
        }

        public override void DeviceDiscnn()
        {

        }
    }
}
