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
            RecurentNeuralNetwork rnn = new RecurentNeuralNetwork(1000, 0.75);

            double data = 0.817357;
            List<NeuronPath> path = rnn.Calculate(data);

            Console.WriteLine("Step : {0,3} Neuron [{1,3},{2,3}] = {3:0.0000}", 0, 0, 0, data);

            int step = 1;

            foreach (NeuronPath np in path)
            {
                Console.WriteLine("Step : {0,3} Neuron [{1,3},{2,3}] = {3:0.0000}", step, np.i, np.j, np.output);
                step++;
            }

            Console.ReadLine();
        }
    }
}
