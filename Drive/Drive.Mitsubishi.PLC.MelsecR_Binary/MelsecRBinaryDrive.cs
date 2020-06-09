using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication;
using HslCommunication.Profinet.Melsec;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Mitsubishi.PLC.MelsecR_Binary
{
    public class MelsecRBinaryDrive : NetworkDeviceBaseTemplate<MelsecRBinaryConfig>
    {
        public override void DeviceConn(MelsecRBinaryConfig config)
        {
            MelsecMcRNet mcRNet = new MelsecMcRNet();
            mcRNet.IpAddress = config.IP;
            mcRNet.Port = config.Port;
            mcRNet.ConnectTimeOut = config.ConnectTimeOut;
            OperateResult connect = mcRNet.ConnectServerAsync().Result;
            NetworkDevice = mcRNet;
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            ((MelsecMcRNet)NetworkDevice).ConnectClose();
        }
    }
}
