using BetterIOT.Common;
using BetterIOT.Common.Base;
using BetterIOT.Common.DriveConfig;
using BetterIOT.Common.Func;
using Drive.GBxfxy.UseData;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.GBxfxy
{
    public class GBxfxyDrive : IDrive<GBxfxyConfig>
    {
        List<IOTData> iOTDatas = new List<IOTData>();
        SocketServer socketServer;
        object Lockobj = new object();
        public override void DeviceConn(GBxfxyConfig config)
        {
            socketServer = new SocketServer();
            socketServer.OnGetStrData += SocketServer_OnGetStrData;
            socketServer.Port = DriveConfig.SocketPort;
            socketServer.Start();
        }

        private void SocketServer_OnGetStrData(object sender, byte[] BTData)
        {
            XfBusiness xfBusiness = new XfBusiness(BTData);
            List<IOTData> Ds = new List<IOTData>();
            string strGroup = Guid.NewGuid().ToString("N");
            Ds.Add(new IOTData
            {
                DataCode = "XfAddrCode",
                DataValue = xfBusiness.AddrCode,
                GTime = DateTime.Now,
                DriveType = DriveConfig.DriveType,
                DriveCode = DriveConfig.DriveCode,
                Unit = "-",
                DataName = "设备地址"
            });
            Ds.Add(new IOTData
            {
                DataCode = "XFBusinessNO",
                DataValue = xfBusiness.strBusNO(),
                GTime = DateTime.Now,
                DriveType = DriveConfig.DriveType,
                DriveCode = DriveConfig.DriveCode,
                Unit = "-",
                DataName = "业务编号"
            });
            Ds.Add(new IOTData
            {
                DataCode = "XFCommand",
                DataValue = xfBusiness.Cmd,
                GTime = DateTime.Now,
                DriveType = DriveConfig.DriveType,
                DriveCode = DriveConfig.DriveCode,
                Unit = "-",
                DataName = "命令"
            });
            Ds.Add(new IOTData
            {
                DataCode = "XFTime",
                DataValue = xfBusiness.TimeC.ToString("yyyy-MM-dd HH:mm:ss"),
                GTime = DateTime.Now,
                DriveType = DriveConfig.DriveType,
                DriveCode = DriveConfig.DriveCode,
                Unit = "-",
                DataName = "设备时间"
            });
            try
            {
                foreach (DataDetail detail in xfBusiness.useData.DataDetails)
                {
                    Ds.Add(new IOTData
                    {
                        DataCode = "XFDeviceName",
                        DataValue = detail.DeviceName,
                        GTime = DateTime.Now,
                        DriveType = DriveConfig.DriveType,
                        DriveCode = DriveConfig.DriveCode,
                        Unit = "-",
                        DataName = "设备名称"
                    });
                    foreach (string strkey in detail.DataValue.Keys)
                    {
                        Ds.Add(new IOTData
                        {
                            DataCode = detail.DeviceName + "_" + strkey,
                            DataValue = detail.DataValue[strkey],
                            GTime = DateTime.Now,
                            DriveType = DriveConfig.DriveType,
                            DriveCode = DriveConfig.DriveCode,
                            Unit = "-",
                            DataName = strkey
                        });
                    }
                }
            }
            catch
            {

            }
            lock (Lockobj)
            {
                iOTDatas.AddRange(Ds);
            }
        }

        public override void DeviceDiscnn()
        {
            socketServer.Stop();
        }

        public override IEnumerable<IOTData> GetData()
        {
            List<IOTData> iOTs;
            lock (Lockobj)
            {
                iOTs = DeepClone.Clone<IOTData>(iOTDatas);
                iOTDatas.Clear();
            }
            return iOTs;
        }
    }
}
