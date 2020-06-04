using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Protocol;
using System;
using System.Text;
using System.Threading.Tasks;

namespace BetterIOT.Service.Base
{
    public class MQTTTranData
    {
        IMqttClient mtqqClient;
        IMqttClientOptions _IMqttClientOptions;

        string MyClientID;

        public DateTime SendTime = DateTime.Now;

        bool IsRun = true;
        string[] Topic;

        //获取到数据
        public delegate void OnGetDataHandler(object sender,string ClientId,string topic, string e);
        public event OnGetDataHandler OnGetData;

        public MQTTTranData(string ClientId,string MQTTerver,int MQTTPort,string MQTTUsr ,string MQTTPsw)
        {
            MyClientID = ClientId;
            MqttClientOptions options = new MqttClientOptions()
            {
                ClientId = ClientId
                //ClientId = "shop1002"
            };
            options.ChannelOptions = new MqttClientTcpOptions()
            {
                Server = MQTTerver,
                Port = MQTTPort
            };
            options.Credentials = new MqttClientCredentials()
            {
                Username = MQTTUsr,
                Password = System.Text.Encoding.UTF8.GetBytes(MQTTPsw)
            };
            options.CleanSession = true;
            options.KeepAlivePeriod = TimeSpan.FromSeconds(100);
            //options.aliv = TimeSpan.FromSeconds(10000);
            _IMqttClientOptions = options;
            mtqqClient = new MQTTnet.MqttFactory().CreateMqttClient();
            mtqqClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(e=>
            {
                try
                {
                    if (OnGetData != null)
                    {
                        OnGetData(this,e.ClientId,e.ApplicationMessage.Topic , Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
                    }
                }
                catch(Exception ex)
                {

                }
            });
            mtqqClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(async e =>
            {
                for (int i = 0; i <= Topic.GetUpperBound(0); i++)
                {
                    await mtqqClient.SubscribeAsync(new TopicFilter { Topic = Topic[i], QualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce });
                }
            });
            mtqqClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(async e =>
            {
                if (IsRun == true)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    try
                    {
                        Connect();
                    }
                    catch
                    {

                    }
                }
            });
        }

        public void Connect()
        {
            mtqqClient.ConnectAsync(_IMqttClientOptions);
        }

        public async void Disconnect()
        {
            if (mtqqClient.IsConnected == true)
            {
                IsRun = false;
                await mtqqClient.DisconnectAsync();
            }
        }


        public void SubTopic(string[] topic)
        {
            Topic = topic;
        }

        public void heartBeat()
        {
            if (mtqqClient.IsConnected == false)
            {
                Connect();
            }
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        public void SendMessage(string Message,string SendTopic)
        {
            try
            {
                var message = new MqttApplicationMessage
                {
                    Topic = SendTopic,// + "/" + MyClientID,
                    Payload = Encoding.UTF8.GetBytes(Message),
                    QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce
                };
                mtqqClient.PublishAsync(message);
                SendTime = DateTime.Now;
            }
            catch(Exception ex)
            {
                Connect();
            }
        }
        /// <summary>
        /// 发送信息
        /// </summary>
        public async void SendMessageTB(string Message, string SendTopic)
        {
            try
            {
                var message = new MqttApplicationMessage
                {
                    Topic = SendTopic,// + "/" + MyClientID,
                    Payload = Encoding.UTF8.GetBytes(Message),
                    QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce
                };
                await mtqqClient.PublishAsync(message);
                SendTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                Connect();
            }
        }

    }
}
