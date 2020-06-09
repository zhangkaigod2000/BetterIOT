using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication;
using HslCommunication.Profinet.Melsec;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Mitsubishi.PLC.MelsecFx_SerialOverTcp
{
    public class MelsecFxSerialOverTcpDrive : NetworkDeviceBaseTemplate<MelsecFxSerialOverTcpConfig>
    {
        public override void DeviceConn(MelsecFxSerialOverTcpConfig config)
        {
            MelsecFxSerialOverTcp melsecFx = new MelsecFxSerialOverTcp();
            melsecFx.IpAddress = config.IP;
            melsecFx.Port = config.Port;
            OperateResult connect = melsecFx.ConnectServer();
            NetworkDevice = melsecFx;
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            ((MelsecFxSerialOverTcp)NetworkDevice).ConnectClose();
        }
    }
}
