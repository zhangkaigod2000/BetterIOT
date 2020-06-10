using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication;
using HslCommunication.Profinet.AllenBradley;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.AllenBradley.PLC.MicroCip
{
    public class AllenBradleyMicroCipDrive : NetworkDeviceBaseTemplate<AllenBradleyMicroCipConfig>
    {
        public override void DeviceConn(AllenBradleyMicroCipConfig config)
        {
            AllenBradleyMicroCip allenBradleyMicroCip = new AllenBradleyMicroCip();
            allenBradleyMicroCip.IpAddress = config.IP;
            allenBradleyMicroCip.Port = config.Port;
            allenBradleyMicroCip.Slot = config.Slot;
            allenBradleyMicroCip.PortSlot = config.PortSlot;
            OperateResult connect = allenBradleyMicroCip.ConnectServer();
            NetworkDevice = allenBradleyMicroCip;
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            ((AllenBradleyMicroCip)NetworkDevice).ConnectClose();
        }
    }
}
