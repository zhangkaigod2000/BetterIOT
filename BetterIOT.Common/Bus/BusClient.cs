using NetMQ;
using NetMQ.Sockets;
using System;
using System.Threading.Tasks;

namespace BetterIOT.Common.Bus
{
    public class BusClient : IDisposable
    {
        private readonly PublisherSocket _pusSocket;
        private readonly SubscriberSocket _subSocket;

        public event EventHandler<BusEventArgs> OnReceived;

        public BusClient()
        {
            _pusSocket = new PublisherSocket();            
            _pusSocket.Connect(BusOption.PublisherAddress);

            _subSocket = new SubscriberSocket();
            _subSocket.Connect(BusOption.SubscriberAddress);

            Task.Run(() =>
            {
                while (true)
                {
                    string topicReceived = _subSocket.ReceiveFrameString();
                    string messageReceived = _subSocket.ReceiveFrameString();

                    this.OnReceived?.Invoke(this, new BusEventArgs { Topic = topicReceived, Message = messageReceived });
                }
            });
        }

        public void Publish(string topic, string message)
        {
            //try
            //{
                _pusSocket.SendMoreFrame(topic).SendFrame(message);
            //}
            //catch { }
        }

        public void Subscribe(string topic)
        {
            _subSocket.Subscribe(topic);
        }

        public void Dispose()
        {
            this._subSocket?.Dispose();
            this._pusSocket?.Dispose();
        }
    }
}
