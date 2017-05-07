using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SI_ESN
{
    class Connection
    {
        public Neuron source;
        public Neuron target;

        public Connection(Neuron source,Neuron target)
        {
            this.source = source;
            this.target = target;
        }
    }
}
