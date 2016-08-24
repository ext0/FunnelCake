using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunnelCake.Funnels
{
    public class MappedFunnel
    {
        private short[] _reserved = new short[] { 0 };
        private short _id;
        private String _identifier;

        public short Id
        {
            get
            {
                return _id;
            }
        }

        public MappedFunnel(short id, String identifier)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException("id");
            }
            else if (_reserved.Where((x) => { return x == id; }).Count() != 0)
            {
                throw new ArgumentException("id");
            }
            _id = id;
            _identifier = identifier;
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is MappedFunnel)
            {
                return (obj as MappedFunnel)._id == _id;
            }
            return false;
        }
    }
}
