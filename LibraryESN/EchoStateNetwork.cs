using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace LibraryESN
{
    public class EchoStateNetwork
    {
        public bool teached;

        public int trainLenght; // Długość ciągu uczącego
        public int testLenght; // Długość ciągu wynikowego
        public int initLenght; // Długość ignorowanego ciągu wyników inicjalnych
        public double a;// LeakingRate
        public Matrix<double> Win; // Warstwa wejścia
        public Matrix<double> W; // Warstwa rezerwuaru
        public Matrix<double> Wout; // Powłoka wyjścia
        public Matrix<double> X; // Macierz zerowa
        public Matrix<double> Yt; // Oczekiwane wartości
        public Matrix<double> Y; // Macierz wyjścia
        public State state;
        public int errorLenght;

        public double rhoW;
        public int size; // Reservoir size

        RecurentNeuralNetwork reservoir; // Rekurencyjna sieć neuronowa

        public Data data;


        public EchoStateNetwork(int reservoirSize, double reservoirConnectivity,State dataState, int train, int test, int init,int error, double leakRate, string dataPath)
        {
            errorLenght = error;
            state = dataState;
            size = reservoirSize;
            trainLenght = train;
            testLenght = test;
            initLenght = init;
            a = leakRate;

            data = new Data(dataPath);

            //Initiate 
            Win = Matrix<double>.Build.Random(reservoirSize, 2) - 0.5;
            W = Matrix<double>.Build.Random(reservoirSize, reservoirSize) - 0.5;
            //rhoW = max(abs(eigenvalues(W))
            rhoW = W.Evd().EigenValues.AbsoluteMaximum().Real;
            W *= 1.25 / rhoW;

            X = Matrix<double>.Build.Random(trainLenght-initLenght, initLenght + 1 + reservoirSize) *0;

            reservoir = new RecurentNeuralNetwork(this);

            Yt = Matrix<double>.Build.Random(trainLenght - 1 - initLenght, 1);
            Yt.SetRow(0,data.GetExpetedOutput(trainLenght - 1 - initLenght));

            reservoir.ActivateReservoir();
            double reg = (double)1;

            Matrix<double> X_T = X.Transpose();

            Wout = Yt * X_T * Matrix<double>.Build.DenseOfMatrix(X * X_T + reg * Matrix<double>.Build.DenseDiagonal(2 + size, 2 + size, 1)).Inverse();
        }

        private double max(Vector<double> vector)
        {
            double output = 0;
            foreach(double d in vector)
            {
                if (output < d) output = d;
            }
            return output;
        }

        public double ActivateNetwork()
        {
            Y = Matrix<double>.Build.Dense(testLenght, 1);
            int count = data.InputData.Count;
            double[] u = data.InputData[trainLenght + 1].values.ToArray();
            for(int t = 0; t < count; t++)
            {
                reservoir.x = (1 - a) * reservoir.x + a * Matrix<double>.Tanh(Win * Matrix<double>.Build.Dense(1, u.Length, u) + W * reservoir.x);

                double[] temp = new double[u.Length + reservoir.x.ToArray().Length];
                double[] tempX = reservoir.x.Row(0).ToArray();
                for (int i = 0; i < temp.Length; i++)
                {

                    if (i < u.Length)
                    {
                        temp[i] = u[i];
                    }
                    else
                    {
                        temp[i] = tempX[i - tempX.Length];
                    }
                }

                double y = (Wout * Matrix<double>.Build.Dense(1,temp.Length,temp))[0,0];
                Y.SetRow(t,Vector<double>.Build.Dense(1,y));
                if (state == State.GenerativeMode)
                {
                    // TODO
                }
                else
                {
                    u = data.InputData[trainLenght + 1 + t].values.ToArray();
                }
            }
            double mse = 0; // Minimal square error
            for(int i = 0; i < errorLenght; i++)
            {
                mse += Math.Sqrt(Y[1, i] - Yt[1, i]);
            }
            mse /= errorLenght;
            return mse;
        }
    }

    public enum State
    {
        GenerativeMode,
        PredictiveMode
    }
}
