using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SI_ESN
{
    class Network
    {
        //Number of neurons
        public int size;
        public double learningRate;
        public Neuron[] reservoir;
        public List<Connection> connections;
        Random rnd;

        public Network(int size, double learningRate)
        {
            rnd = new Random();
            this.size = size;
            connections = new List<Connection>();
            Console.WriteLine("Generating Network...");
            Console.WriteLine("\tGenerating Neurons...");
            GenerateNeurons();
            Console.WriteLine("\tGenerating Connections...");
            GenerateConnections();
            Console.WriteLine("\tGenerating Starting Weights...");
            GenerateStartingWeights();
            Console.WriteLine("Finished!");
        }

        private void GenerateStartingWeights()
        {
            foreach (Neuron n in reservoir)
            {
                //Counting input connections for neuron
                int counter = 0;
                foreach(Connection c in connections)
                {
                    if (c.target == n) counter++;
                }

                //Initializing vector of weights
                n.vWeights = new double[counter];

                //Setting starting weights
                for(int i = 0; i < counter; i++)
                {
                    n.vWeights[i] = rnd.NextDouble();
                }

                //Setting learningRate
                n.learningRate = learningRate;
            }
        }

        private void GenerateConnections()
        {
            //1% of all possible connections
            int connectionCount = ((size * (size - 1)) / 200);

            for(int i = 0; i < connectionCount; i++)
            {
                bool noNewConnction = true;
                do
                {
                    int randA = rnd.Next(size - 1);
                    int randB;
                    do
                    {
                        randB = rnd.Next(size - 1);
                    } while (randA == randB);

                    Connection temp = new Connection(reservoir[randA], reservoir[randB]);

                    if (!connections.Contains(temp))
                    {
                        connections.Add(temp);
                        noNewConnction = false;
                    }
                } while (noNewConnction);
            }
        }

        private void GenerateNeurons()
        {
            reservoir = new Neuron[size];
            for(int i = 0; i < size; i++)
            {
                reservoir[i] = new Neuron();
            }
        }
    }
}
