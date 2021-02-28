using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherAPI.Helpers
{
    public class ZeroMQHelper
    {
        private readonly PublisherSocket _pubSocket;

        public ZeroMQHelper(string host, int port)
        {
            _pubSocket = new PublisherSocket();

            _pubSocket.Bind($@"tcp://{host}:{port}");
        }

        public void SendMessage(string topic, string message)
        {
            _pubSocket.SendMoreFrame(topic).SendFrame(message);
        }
    }
}
