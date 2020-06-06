using BetterIOT.Common;
using BetterIOT.Common.Base;
using BetterIOT.Common.Bus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BetterIOT.Service.Core
{
    public class DriveStatusService : BackgroundService
    {
        private readonly BusClient bus;
        private readonly ILogger<DriveStatusService> logger;
        IEnumerable<DriveListConfig> driveLists;
        Dictionary<string, Process> DriveProcess = new Dictionary<string, Process>();
        /// <summary>
        /// 线程取消
        /// </summary>
        CancellationToken _CancellationToken;
        public DriveStatusService(ILogger<DriveStatusService> logger)
        {
            this.logger = logger;
            bus = new BusClient();
            this.bus.Subscribe(BusOption.CTRL_START);
            this.bus.Subscribe(BusOption.CTRL_STOP);
            this.bus.OnReceived += Bus_OnReceived;
            driveLists = DriveListConfig.ReadConfig();
            StartAllDrive();    //服务启动的时候启动全部的设备
        }

        private void Bus_OnReceived(object sender, BusEventArgs e)
        {
            switch (e.Topic)
            {
                case BusOption.CTRL_START:       //启动消息
                    StartDrive(e.Message);
                    break;
                case BusOption.CTRL_STOP:       //关闭消息
                    StopDrive(e.Message);
                    break;
            }
        }
        /// <summary>
        /// 启动全部的驱动
        /// </summary>
        private void StartAllDrive()
        {
            string DriveConfigPath = AppDomain.CurrentDomain.BaseDirectory + "/DriveConfig/";
            string[] AllDriveConfig = System.IO.Directory.GetFiles(DriveConfigPath);      //全部的驱动配置文件
            foreach(string strConfig in AllDriveConfig)
            {
                string strTmp = System.IO.Path.GetFileNameWithoutExtension(strConfig);
                StartDrive(strTmp);
            }
        }

        /**关于设备的驱动配置和驱动程序的存放说明
         * 程序文件夹下的DriveConfig文件夹下面存放全部需要使用的设备采集的驱动的配置文件，文件是json格式的，文件的命名使用设备的序列号来命名，单设备不允许重复，
         * 严格意义上来说，一个项目中都不允许重复，所有的配置文件都必须继承于DriveConfigBase这个基础类。
         * Drive这个目录下面，存放了全部的我们设备驱动程序，按照文件夹名称等分类来进行排布。
         * 程序根目录下放一个json配置文件，用来存放设备驱动的文件夹，主程序等信息
         */
        private void StartDrive(string strStartDrive)
        {
            string strDriveConfigPath = AppDomain.CurrentDomain.BaseDirectory + "/DriveConfig/" + strStartDrive + ".json";
            //直接发送配置编号
            if (System.IO.File.Exists(strDriveConfigPath) == true)
            {
                try
                {
                    ConfigBase configBase = ConfigBase.ReadConfig<ConfigBase>(strDriveConfigPath);
                    //找到驱动所在的文件夹及文件
                    DriveListConfig config = driveLists.Single(n => n.DriveType == configBase.DriveType);
                    //启动程序
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.CreateNoWindow = true;
                    startInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory + "Drive/" + config.DrivePath;
                    startInfo.FileName = "dotnet";
                    startInfo.Arguments = startInfo.WorkingDirectory + "/" + config.DriveStartFile + " " + strDriveConfigPath;
                    startInfo.UseShellExecute = false;
                    if (DriveProcess.ContainsKey(strStartDrive) == true)
                    {
                        //存在进程的时候
                        DriveProcess[strStartDrive].Kill();
                        DriveProcess.Remove(strStartDrive);
                    }
                    Process process = Process.Start(startInfo);
                    DriveProcess.Add(strStartDrive, process);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.ToString());
                }

            }
        }

        private void StopDrive(string strStopDrive)
        {
            //直接发送配置编号
            if (DriveProcess.ContainsKey(strStopDrive) == true)
            {
                //存在进程的时候
                DriveProcess[strStopDrive].Kill();
                DriveProcess.Remove(strStopDrive);
            }
        }

        /// <summary>
        /// 处理心跳的事件
        /// </summary>
        /// <param name="e"></param>
        private void GetHeartBeat()
        {
            while (!_CancellationToken.IsCancellationRequested)
            {
                try
                {
                    Process[] proces = Process.GetProcesses();
                    foreach(string strkey in DriveProcess.Keys)
                    {
                        if (proces.Count(x=>x.Id == DriveProcess[strkey].Id) == 0)
                        {
                            //进程不存在了
                            if (Program.DriveOnlineInfo.ContainsKey(strkey) == true)
                            {
                                //存在
                                Program.DriveOnlineInfo.Remove(strkey);
                            }
                        }
                        else
                        {
                            //进程还在
                            if (Program.DriveOnlineInfo.ContainsKey(strkey) == false)
                            {
                                //不再列表中
                                Program.DriveOnlineInfo.Add(strkey, DateTime.Now);
                            }
                            else
                            {
                                //在列表中
                                Program.DriveOnlineInfo[strkey] = DateTime.Now;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.ToString());
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation("DriveStatusService started");

            _CancellationToken = stoppingToken;
            return Task.Factory.StartNew(GetHeartBeat);
        }

        public override void Dispose()
        {
            base.Dispose();
            this.bus?.Dispose();
        }
    }
}
