using BetterIOT.Common;
using BetterIOT.Common.Base;
using BetterIOT.Common.Bus;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace BetterIOT.Hsl.Template
{
    public abstract class NetworkDeviceBaseTemplate<T> where T : NetworkDeviceConfigBase
    {
        BusClient bus;
        private bool ISRun = false;
        protected T DriveConfig;
        protected IReadWriteNet NetworkDevice = null;
        public void Start(string ConfigFilePath)
        {
            string strConfig = System.IO.File.ReadAllText(ConfigFilePath);
            T config = JsonSerializer.Deserialize<T>(strConfig);
            DriveConfig = config;
            DeviceConn(config);
            bus = new BusClient();
            this.bus.Subscribe(BusOption.CMD_INPUT);
            this.bus.OnReceived += Bus_OnReceived;
            ISRun = true;
            while (ISRun)
            {
                IEnumerable<IOTData> iOTs = GetData();
                if (iOTs != null)
                {
                    if (iOTs.Count() > 0)
                    {
                        bus.Publish(BusOption.CMD_INPUT, JsonSerializer.Serialize(iOTs));
                    }
                }
                System.Threading.Thread.Sleep(config.CycleTime);
            }
            DeviceDiscnn();
        }

        /// <summary>
        /// 开始设备通讯
        /// </summary>
        public abstract void DeviceConn(T config);

        /// <summary>
        /// 采集数据的方法
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IOTData> GetData()
        {
            List<IOTData> iOTs = new List<IOTData>();

            foreach (NetworkDeviceResult result in DriveConfig.Results)
            {
                try
                {
                    string sResult;
                    switch (result.DataType.ToUpper())
                    {
                        case "BOOL":
                            sResult = NetworkDevice.ReadBool(result.DB).Content.ToString();
                            break;
                        case "STRING":
                            sResult = NetworkDevice.ReadString(result.DB, Convert.ToUInt16(result.Len)).Content;
                            break;
                        case "INT":
                            sResult = NetworkDevice.ReadInt32(result.DB).Content.ToString();
                            break;
                        case "FLOAT":
                            sResult = NetworkDevice.ReadFloat(result.DB).Content.ToString(result.Format);
                            break;
                        case "DOUBLE":
                            sResult = NetworkDevice.ReadDouble(result.DB).Content.ToString(result.Format);
                            break;
                        case "SHORT":
                            sResult = NetworkDevice.ReadInt16(result.DB).Content.ToString();
                            break;
                        case "USHORT":
                            sResult = NetworkDevice.ReadUInt16(result.DB).Content.ToString();
                            break;
                        case "UINT":
                            sResult = NetworkDevice.ReadUInt32(result.DB).Content.ToString();
                            break;
                        case "LONG":
                            sResult = NetworkDevice.ReadInt64(result.DB).Content.ToString();
                            break;
                        case "ULONG":
                            sResult = NetworkDevice.ReadUInt64(result.DB).Content.ToString();
                            break;
                        default:
                            sResult = NetworkDevice.ReadString(result.DB, 1).Content;
                            break;
                    }
                    iOTs.Add(new IOTData
                    {
                        DataCode = result.Address,
                        DataValue = sResult,
                        DataName = result.Name,
                        DriveCode = DriveConfig.DriveCode,
                        DriveType = DriveConfig.DriveType,
                        GTime = DateTime.Now,
                        Unit = result.Unit
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return iOTs;
        }
        /// <summary>
        /// 设备断开连接
        /// </summary>
        public abstract void DeviceDiscnn();

        private void Bus_OnReceived(object sender, BusEventArgs e)
        {
            if (e.Topic == BusOption.CMD_INPUT)
            {
                //接收到了控制命令
                DriveControl Ctrl = JsonSerializer.Deserialize<DriveControl>(e.Message);
                if (Ctrl.Cmd == DriveControl.CMD_SHUTDOWN)
                {
                    ISRun = false;
                }
            }
        }
    }
}
