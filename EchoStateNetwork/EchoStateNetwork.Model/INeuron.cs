using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoStateNetwork.Model
{
    interface INeuron
    {
        double input { get; set; }
        double output { get; set; }
        double[] vWeights { get; set; }

        void Calculate();
    }
}
