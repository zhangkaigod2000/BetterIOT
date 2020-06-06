using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Drive.GBxfxy
{
    /// <summary>
    /// 反向链接接收端
    /// </summary>
    public class SocketServer 
    {
        /// <summary>
        /// 服务器节点
        /// </summary>
        IPEndPoint serverEndPoint;
        /// <summary>
        /// 服务器连接
        /// </summary>
        List<Socket> serverSockets = new List<Socket>();
        /// <summary>
        /// 客户端连接集合
        /// </summary>
        public List<Socket> clientSockets = new List<Socket>();

        static object obj = new object();

        /// <summary>
        /// 线程取消
        /// </summary>
        CancellationTokenSource _CancellationTokenSource = null;

        /// <summary>
        /// 数据刷新频率
        /// </summary>
        public int RefreshRate { get; set; }

        /// <summary>
        /// 线程开关
        /// </summary>
        protected bool ifRun = false;
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }



        /// <summary>
        /// 获取字符串
        /// </summary>
        public delegate void OnGetStrDataHandler(object sender, byte[] buffer);
        public event OnGetStrDataHandler OnGetStrData;

        public void Start()
        {
            IPAddress[] ipadddress = Dns.GetHostAddresses(Dns.GetHostName()); //
            ifRun = true;
            var aa = ipadddress.ToList();
            aa.Add(new IPAddress(new byte[] { 0, 0, 0, 0 }));
            ipadddress = aa.ToArray();
            IPAddress Ipa = null;
            //设置服务器节点
            for (int i = 0; i <= ipadddress.GetUpperBound(0); i++)
            {
                if (ipadddress[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    try
                    {
                        Ipa = ipadddress[i];
                        serverEndPoint = new IPEndPoint(Ipa, Port);
                        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        serverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                        // 绑定
                        serverSocket.Bind(serverEndPoint);
                        // 监听
                        serverSocket.Listen(200);
                        serverSockets.Add(serverSocket);
                        serverSocket.BeginAccept(OnConnectRequest, serverSocket);
                    }
                    catch(Exception ex)
                    {
                        //if (OnDeviceError != null)
                        //{
                        //    OnDeviceError(null, ex);
                        //}
                    }
                }
            }
        }
        
        void _Computing_OnSocketClosed(object sender, Socket Client)
        {
            try
            {
                clientSockets.Remove(Client);
            }
            catch
            {
            }
        }

        //当有客户端连接时的处理
        protected void OnConnectRequest(IAsyncResult ar)
        {
            //初始化一个SOCKET，用于其它客户端的连接
            Socket server1 = (Socket)ar.AsyncState;
            try
            {
                Socket Client = server1.EndAccept(ar);
                StateObject state = new StateObject();
                state.workSocket = Client;
                clientSockets.Add(Client);
                Client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch(Exception ex)
            {
                //OnDeviceError(null, ex);
            }
            try
            {
                //等待新的客户端连接
                server1.BeginAccept(new AsyncCallback(OnConnectRequest), server1);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 异步获取数据
        /// </summary>
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // 从输入参数异步state对象中获取state和socket对象
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                //从远程设备读取数据  
                int bytesRead = client.EndReceive(ar);
                if (bytesRead > 0)
                {
                    try
                    {
                        byte[] byReplay = new byte[30];
                        byReplay[0] = 64;
                        byReplay[1] = 64;
                        byReplay[2] = state.buffer[2];
                        byReplay[3] = state.buffer[3];
                        byReplay[4] = 1;
                        byReplay[5] = 1;

                        byReplay[6] = state.buffer[6];
                        byReplay[7] = state.buffer[7];
                        byReplay[8] = state.buffer[8];
                        byReplay[9] = state.buffer[9];
                        byReplay[10] = state.buffer[10];
                        byReplay[11] = state.buffer[11];

                        byReplay[18] = state.buffer[12];
                        byReplay[19] = state.buffer[13];
                        byReplay[20] = state.buffer[14];
                        byReplay[21] = state.buffer[15];
                        byReplay[22] = state.buffer[16];
                        byReplay[23] = state.buffer[17];

                        byReplay[12] = state.buffer[18];
                        byReplay[13] = state.buffer[19];
                        byReplay[14] = state.buffer[20];
                        byReplay[15] = state.buffer[21];
                        byReplay[16] = state.buffer[22];
                        byReplay[17] = state.buffer[23];

                        byReplay[24] = 0;
                        byReplay[25] = 0;
                        byReplay[26] = 3;
                        byReplay[27] = (byte)CheckSum(byReplay);//和校验
                        byReplay[28] = 35;
                        byReplay[29] = 35;
                        client.Send(byReplay);
                        OnGetStrData?.Invoke(this, state.buffer);
                    }
                    catch(Exception ee)
                    {
                    }
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    lock (client)
                    {
                        clientSockets.Remove(client);
                        client.Close(1000);
                        client.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                //if (OnDeviceError != null)
                //{
                //    OnDeviceError(null, e);
                //}
            }
        }

        private int CheckSum(byte[] bt)
        {
            int isum = 0;
            for(int i = 2;i <= 26;i++)
            {
                isum = isum + bt[i];
            }
            while(isum >= 255)
            {
                isum = isum - 255;
            }
            return isum;
        }

        public void Stop()
        {
            try
            {
                //OnDeviceLog(this, "驱动停止");
                _CancellationTokenSource.Cancel();
                ifRun = false;
                for (int i = 0; i < serverSockets.Count; i++)
                {
                    serverSockets[i].Close();
                }
            }
            catch(Exception ex)
            {
                //if (OnDeviceError != null)
                //{
                //    OnDeviceError(null, ex);
                //}
            }
        }
    }
}
