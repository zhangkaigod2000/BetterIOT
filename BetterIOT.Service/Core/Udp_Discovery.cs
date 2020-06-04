using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BetterIOT.Service.Core
{
    public class Udp_Discovery : BackgroundService
    {
        private readonly ILogger<Udp_Discovery> logger;
        UdpClient udpClient;
        IPEndPoint ipEndPoint;

        /// <summary>
        /// 线程取消
        /// </summary>
        CancellationToken _CancellationToken;
        public Udp_Discovery(ILogger<Udp_Discovery> logger)
        {
            this.logger = logger;
            udpClient = new UdpClient(10);
            ipEndPoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 35);
        }

        /// <summary>
        /// 心跳连接执行
        /// </summary>
        private void HeartBeat()
        {
            while (!_CancellationToken.IsCancellationRequested)
            {
                try
                {
                    byte[] data = udpClient.Receive(ref ipEndPoint);
                    //数据包要求 192.168.0.100:90:192.168.0.30这样的写法
                    //服务器ip:端口:客户端ip
                    string strTmp = Encoding.UTF8.GetString(data);

                    string[] str1 = strTmp.Split(':');
                    //返回数据包
                    IPEndPoint TmpEP = new IPEndPoint(IPAddress.Parse(str1[0]), Convert.ToInt32(str1[1]));
                    byte[] btip;
                    btip = Encoding.UTF8.GetBytes(str1[2]);
                    udpClient.Send(btip, btip.Length, TmpEP);
                }
                catch (Exception ex)
                {

                }
                Thread.Sleep(50);
            }
        }

        public override void Dispose()
        {
            try
            {
                udpClient.Close();
            }
            catch (Exception ex)
            {

            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation("Discovery started");
            _CancellationToken = stoppingToken;
            return Task.Factory.StartNew(HeartBeat, _CancellationToken);
        }
    }
}
