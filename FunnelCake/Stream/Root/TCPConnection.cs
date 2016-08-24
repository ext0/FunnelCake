using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FunnelCake.Stream.Root
{
    public class TCPConnection
    {
        private string _hostname;
        private int _port;
        private TcpClient _client;
        private NetworkStream _internalStream;

        public NetworkStream Stream
        {
            get
            {
                return _internalStream;
            }
        }

        private SocketError _socketStatus;

        public SocketError Status
        {
            get
            {
                return _socketStatus;
            }
        }

        public TCPConnection(string hostname, int port)
        {
            _hostname = hostname;
            _port = port;
        }

        public SocketError TryConnect()
        {
            try
            {
                _client = new TcpClient(_hostname, _port);
                _socketStatus = SocketError.Success;
                _internalStream = _client.GetStream();
            }
            catch(SocketException e)
            {
                _socketStatus = e.SocketErrorCode;
            }
            return _socketStatus;
        }
    }
}
