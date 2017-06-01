using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryESN
{
    public class RecurentNeuralNetwork
    {
        private double[,] _network;
        private Random _rnd;
        private int _networkSize;

        public RecurentNeuralNetwork(int networkSize, double connectionRate)
        {
            _network = new double[networkSize, networkSize];
            _rnd = new Random();
            _networkSize = networkSize;

            for (int i = 0; i < _networkSize; i++)
            {
                for(int j = 0; j < _networkSize; j++)
                {
                    if (_rnd.NextDouble() <= connectionRate) _network[i, j] = _rnd.NextDouble();
                }
            }
        }

        public List<NeuronPath> Calculate(double data)
        {
            int i = 0;
            int j = 0;
            double output = data;

            List<NeuronPath> path = new List<NeuronPath>();

            int count = 0;

            do
            {
                j = _rnd.Next(0,_networkSize+1);
                if (j == _networkSize)
                    break;
                else
                {
                    if (_network[i, j] == 0) { }
                    else
                    {
                        output = 1 - (output * _network[i, j]); //Higly temporary
                        path.Add(new NeuronPath(i, j, output));
                        i = j;
                    }
                }
            } while (true);
            return path;
        }

        public double[,] Network
        {
            get
            {
                return _network;
            }
        }

        public double GetNeuron(int i,int j)
        {
            return _network[i, j];
        }

        public void WriteOutNetwork()
        {
            for (int i = 0; i < _networkSize; i++)
            {
                for (int j = 0; j < _networkSize; j++)
                {
                    Console.Write("{0:0.0000} ", _network[i, j]);
                }
                Console.WriteLine();
            }
        }
    }

    public class NeuronPath
    {
        public int i;
        public int j;
        public double output;

        public NeuronPath(int i,int j,double output)
        {
            this.i = i;
            this.j = j;
            this.output = output;
        }
    }
}
