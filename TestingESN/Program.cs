using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryESN;

namespace TestingESN
{
    class Program
    {
        static void Main(string[] args)
        {
            EchoStateNetwork esn = new EchoStateNetwork(1000, 0.05, State.PredictiveMode, 2000, 2000, 100, 500, 0.3, "");
            Console.ReadLine();
        }
    }
}
