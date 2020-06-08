using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication;
using HslCommunication.Profinet.Melsec;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Mitsubishi.PLC.Melsec_Binary
{
    public class MelsecBinaryDrive : NetworkDeviceBaseTemplate<MelsecBinaryConfig>
    {
        public override void DeviceConn(MelsecBinaryConfig config)
        {
            MelsecMcNet melsecMc = new MelsecMcNet();
            melsecMc.IpAddress = config.IP;
            melsecMc.Port = config.Port;
            melsecMc.ConnectTimeOut = config.ConnTimeOut;
            NetworkDevice = melsecMc;
            OperateResult connect = melsecMc.ConnectServerAsync().Result;
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            ((MelsecMcNet)NetworkDevice).ConnectClose();
        }
    }
}
