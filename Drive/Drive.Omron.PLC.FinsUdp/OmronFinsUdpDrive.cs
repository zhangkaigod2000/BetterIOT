using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication.Profinet.Omron;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Omron.PLC.FinsUdp
{
    public class OmronFinsUdpDrive : NetworkDeviceBaseTemplate<OmronFinsUdpConfig>
    {
        public override void DeviceConn(OmronFinsUdpConfig config)
        {
            OmronFinsUdp omronFinsUdp = new OmronFinsUdp(config.IP, config.Port);
            omronFinsUdp.SA1 = config.SA1;
            omronFinsUdp.SA2 = config.SA2;
            omronFinsUdp.ByteTransform.DataFormat = (HslCommunication.Core.DataFormat)config.ByteTransformDataFormat;
        }

        public override void DeviceDiscnn()
        {
            
        }
    }
}
