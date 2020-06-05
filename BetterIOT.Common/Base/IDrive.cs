using BetterIOT.Common.Bus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BetterIOT.Common.Base
{
    /// <summary>
    /// 驱动的基础类型
    /// </summary>
    public abstract class IDrive<T> where T : ConfigBase
    {
        private readonly BusClient bus;
        private bool ISRun = false;
        T DriveConfig;
        public void Start(T config)
        {
            DriveConfig = config;
            DeviceConn(config);
            this.bus.Subscribe(BusOption.CMD_INPUT);
            this.bus.OnReceived += Bus_OnReceived;
            ISRun = true;
            Task.Factory.StartNew(HeartBeart); //开启心跳
            while(ISRun)
            {
                IEnumerable<IOTData> iOTs = GetData();
                if (iOTs != null)
                {
                    bus.Publish(BusOption.CMD_INPUT, JsonSerializer.Serialize(iOTs));
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
        public abstract IEnumerable<IOTData> GetData();
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

        private void HeartBeart()
        {
            DriveHeartBeat heartBeat = new DriveHeartBeat();
            heartBeat.DriveCode = DriveConfig.DriveCode;
            while (ISRun)
            {
                heartBeat.hTime = DateTime.Now;
                bus.Publish(BusOption.CMD_INPUT, JsonSerializer.Serialize(heartBeat));
                System.Threading.Thread.Sleep(5000);
            }
        }
    }
}
