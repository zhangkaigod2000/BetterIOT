using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication;
using HslCommunication.Profinet.Omron;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Omron.PLC.CipNet
{
    public class OmronCipNetDrive : NetworkDeviceBaseTemplate<OmronCipNetConfig>
    {
        public override void DeviceConn(OmronCipNetConfig config)
        {
            OmronCipNet cipNet = new OmronCipNet();
            cipNet.IpAddress = config.IP;
            cipNet.Port = config.Port;
            cipNet.Slot = config.Slot;
            OperateResult connect = cipNet.ConnectServer();
            NetworkDevice = cipNet;
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            ((OmronCipNet)NetworkDevice).ConnectClose();
        }
    }
}
