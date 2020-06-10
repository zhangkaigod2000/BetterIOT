using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication;
using HslCommunication.Profinet.AllenBradley;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.AllenBradley.PLC.LogixTcp
{
    public class AllenBradleyNetDrive : NetworkDeviceBaseTemplate<AllenBradleyNetConfig>
    {
        public override void DeviceConn(AllenBradleyNetConfig config)
        {
            AllenBradleyNet allenBradleyNet = new AllenBradleyNet();
            allenBradleyNet.IpAddress = config.IP;
            allenBradleyNet.Port = config.Port;
            allenBradleyNet.Slot = config.Slot;
            allenBradleyNet.PortSlot = config.PortSlot;
            OperateResult connect = allenBradleyNet.ConnectServer();
            NetworkDevice = allenBradleyNet;
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            ((AllenBradleyNet)NetworkDevice).ConnectClose();
        }
    }
}
