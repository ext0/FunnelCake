using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunnelCake.Funnels
{
    public class MappedFunnel
    {
        private short _id;
        private String _identifier;

        public short Id
        {
            get
            {
                return _id;
            }
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
