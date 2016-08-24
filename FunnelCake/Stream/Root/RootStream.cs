using FunnelCake.Funnels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FunnelCake.Stream.Root
{
    public class RootStream
    {
        private static int BUFFER_SIZE = 1024;

        private byte[] _loadBuffer;
        private TCPConnection _connection;
        private NetworkStream _stream;
        private Dictionary<MappedFunnel, LinkedList<StreamBlock>> _queue;
        private bool ALive
        {
            get
            {
                return _connection.Stream.Ca
            }
        }

        public RootStream(TCPConnection connection)
        {
            _connection = connection;
            _stream = _connection.Stream;
            _queue = new Dictionary<MappedFunnel, LinkedList<StreamBlock>>();
            _loadBuffer = new byte[BUFFER_SIZE];
        }
        public void Bootstrap()
        {
            ThreadPool.QueueUserWorkItem(delegate {

            });
        }
    }
}
