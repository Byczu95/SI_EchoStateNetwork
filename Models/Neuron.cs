using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SI_ESN
{
    class Neuron
    {
        //Output
        public double output;
        //Vector of input weights
        public double[] vWeights;
        //Value of error
        public double errorRate;
        //Learning Rate
        public double learningRate;

        public Neuron() { }

        //Calculate outcome for provided Input vector
        public void Calculate(double[] vInput)
        {
            //Calculating outcome
            output = 0;
            for(int i = 0; i < vWeights.Length; i++) { output += vInput[i] * vWeights[i]; }
        }

        //Learning form Input vector without teacher
        public void Learn(double[] vInput)
        {
            //Calculating outcome
            output = 0;
            for (int i = 0; i < vWeights.Length; i++) { output += vInput[i] * vWeights[i]; }

            //Learing without teacher
            for (int i = 0; i < vWeights.Length; i++)
            {
                vWeights[i] = vWeights[i] + output * learningRate * vInput[i];
            }
        }

        //Learning form Input vector with teacher
        public void Learn(double[] vInput, double expectedOutput)
        {
            //Calculating outcome
            output = 0;
            for (int i = 0; i < vWeights.Length; i++) { output += vInput[i] * vWeights[i]; }

            //Calculating signal error
            errorRate = expectedOutput - output;

            //Learing with teacher
            for (int i = 0; i < vWeights.Length; i++)
            {
                vWeights[i] = vWeights[i] + errorRate * learningRate * vInput[i];
            }
        }
    }
}
