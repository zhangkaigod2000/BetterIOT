using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication;
using HslCommunication.Profinet.Omron;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Omron.PLC.FinsHostLinkOverTCP
{
    public class OmronHostLinkOverTcpDrive : NetworkDeviceBaseTemplate<OmronHostLinkOverTcpConfig>
    {
        public override void DeviceConn(OmronHostLinkOverTcpConfig config)
        {
            OmronHostLinkOverTcp omronHost = new OmronHostLinkOverTcp();
            omronHost.IpAddress = config.IP;
            omronHost.Port = config.Port;
            omronHost.UnitNumber = config.StationNo;
            omronHost.SID = config.SID;
            omronHost.DA2 = config.DA2;
            omronHost.SA2 = config.SA2;
            omronHost.ByteTransform.DataFormat = (HslCommunication.Core.DataFormat)config.ByteTransformDataFormat;
            OperateResult connect = omronHost.ConnectServer();
            NetworkDevice = omronHost;
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            ((OmronHostLinkOverTcp)NetworkDevice).ConnectClose();
        }
    }
}
