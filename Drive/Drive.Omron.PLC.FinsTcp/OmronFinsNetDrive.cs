using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication;
using HslCommunication.Profinet.Omron;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Omron.PLC.FinsTcp
{
    public class OmronFinsNetDrive : NetworkDeviceBaseTemplate<OmronFinsNetConfig>
    {
        public override void DeviceConn(OmronFinsNetConfig config)
        {
            OmronFinsNet omronFinsNet = new OmronFinsNet();
            omronFinsNet.ConnectTimeOut = config.ConnectTimeOut;
            omronFinsNet.IpAddress = config.IP;
            omronFinsNet.Port = config.Port;
            omronFinsNet.SA1 = config.SA1;
            omronFinsNet.SA2 = config.SA2;
            omronFinsNet.ByteTransform.DataFormat = (HslCommunication.Core.DataFormat)config.ByteTransformDataFormat;
            omronFinsNet.IsChangeSA1AfterReadFailed = config.IsChangeSA1AfterReadFailed;
            OperateResult connect = omronFinsNet.ConnectServer();
            NetworkDevice = omronFinsNet;
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            ((OmronFinsNet)NetworkDevice).ConnectClose();
        }
    }
}
