using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication;
using HslCommunication.Profinet.LSIS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.LSIS.PLC.XGB_FastEnet
{
    public class XGBFastEnetDrive : NetworkDeviceBaseTemplate<XGBFastEnetConfig>
    {
        public override void DeviceConn(XGBFastEnetConfig config)
        {
            XGBFastEnet fastEnet = new XGBFastEnet();
            fastEnet.IpAddress = config.IP;
            fastEnet.Port = config.Port;
            fastEnet.SlotNo = config.SlotNo;
            fastEnet.SetCpuType = config.CpuType;
            fastEnet.CompanyID = config.CompanyID;
            OperateResult connect = fastEnet.ConnectServer();
            NetworkDevice = fastEnet;
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            ((XGBFastEnet)NetworkDevice).ConnectClose();
        }
    }
}
