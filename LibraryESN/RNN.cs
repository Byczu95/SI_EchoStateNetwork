using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace LibraryESN
{
    [Serializable]
    public class RNN
    {
        EchoStateNetwork esn;
        public Matrix<double> x;

        public RNN(EchoStateNetwork echoStateNetwork)
        {
            esn = echoStateNetwork;
        }

        public void ActivateReservoir()
        {
            //x = Matrix<double>.Build.Random(esn.size, 1) * 0;

            //for(int t = 0; t < esn.testLenght; t++)
            //{
            //    double[] u = esn.data.InputData[t].values.ToArray();
            //    x = (1 - esn.a) * x + esn.a * Matrix<double>.Tanh(esn.Win * Matrix<double>.Build.Dense(1,u.Length,u) + esn.W * x);

            //    if (t > esn.initLenght)
            //    {
            //        double[] temp = new double[u.Length + x.ToArray().Length];
            //        double[] tempX = x.Row(0).ToArray();
            //        for (int i = 0; i < temp.Length; i++)
            //        {
                        
            //            if (i < u.Length)
            //            {
            //                temp[i] = u[i];
            //            }
            //            else
            //            {
            //                temp[i] = tempX[i-tempX.Length];
            //            }
            //        }
            //        esn.X.SetRow(t, Vector<double>.Build.DenseOfArray(temp));
            //    }
            //}
        }
    }
}
