using BetterIOT.Common.DriveConfig;
using BetterIOT.Hsl.Template;
using HslCommunication;
using HslCommunication.Profinet.FATEK;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.FATEK.PLC.ProgrameOverTcp
{
    public class FatekProgramOverTcpDrive : NetworkDeviceBaseTemplate<FatekProgramOverTcpConfig>
    {
        public override void DeviceConn(FatekProgramOverTcpConfig config)
        {
            FatekProgramOverTcp fatek = new FatekProgramOverTcp();
            fatek.IpAddress = config.IP;
            fatek.Port = config.Port;
            fatek.Station = config.StationNo;
            OperateResult connect = fatek.ConnectServer();
            NetworkDevice = fatek;
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            ((FatekProgramOverTcp)NetworkDevice).ConnectClose();
        }
    }
}
