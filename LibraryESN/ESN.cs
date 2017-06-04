using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace LibraryESN
{
    class ESN
    {
        // Status
        public bool teached;

        // Parametry sieci
        public double a; // Stopień wycieku
        public int size; // Wielkość rezerwuaru

        // Zmiene pośrednie i pomocnicze
        public Matrix<double> Win; // Warstwa wejścia
        public Matrix<double> W; // Warstwa rezerwuaru
        public Matrix<double> Wout; // Powłoka wyjścia
        public Matrix<double> X; // Macierz zerowa
        public Matrix<double> Yt; // Oczekiwane wartości
        public Matrix<double> Y; // Macierz wyjścia
        public double rhoW;
        public Data data;
        public double mse;

        public RNN reservoir; // Sieć rekurencyjana rezerwuar

        //Konstruktor
        public ESN(int reservoirSize, double leakingRate)
        {
            teached = false;
            InitializeParameters(reservoirSize,leakingRate); // <- a
            NormalizeWeights(); // Zapewnij rozrzut wag
            InitializeRNN(); // <- reservoirSize, connectivity
        }

        // Inicjowanie sieci rekurencyjnej
        private void InitializeRNN()
        {
            //reservoir = new RecurentNeuralNetwork(this);
        }

        // Normalizowanie macierzy wag neuronów
        private void NormalizeWeights()
        {
            Win = Matrix<double>.Build.Random(size, 2) - 0.5;
            W = Matrix<double>.Build.Random(size, size) - 0.5;

            rhoW = W.Evd().EigenValues.AbsoluteMaximum().Real; //rhoW = max(abs(eigenvalues(W))
        }

        // Przypisanie parametrów startowych
        private void InitializeParameters(int resSize,double alpha)
        {
            size = resSize;
            a = alpha;
        }

        // Ładowanie danych
        public void Learn(string path, int ignoredInitialResults)
        {
            // Inicjacja zmiennych
            data = new Data(path);
            X = Matrix<double>.Build.Random(data.DataLenght - ignoredInitialResults, ignoredInitialResults + 1 + size) * 0;

            Yt = Matrix<double>.Build.Random(data.DataLenght - 1 - ignoredInitialResults, 1);
            Yt.SetRow(0, data.GetExpetedOutput(data.DataLenght - 1 - ignoredInitialResults));

            reservoir.ActivateReservoir(); // Przejście pierszych pomijanych wyników

            double reg = (double)1; // Doładność double

            Matrix<double> X_T = X.Transpose();

            Wout = Yt * X_T * Matrix<double>.Build.DenseOfMatrix(X * X_T + reg * Matrix<double>.Build.DenseDiagonal(2 + size, 2 + size, 1)).Inverse();

            //Uczenie
            LearnProcess(ignoredInitialResults);
        }

        private void LearnProcess(int initResults)
        {
            int testLenght = data.DataLenght - initResults;
            Y = Matrix<double>.Build.Dense(testLenght, 1);
            int count = data.InputData.Count;
            
            for(int t = initResults; t < data.DataLenght; t++)
            {
                double[] u = data.InputData[t].values.ToArray();
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

                double y = (Wout * Matrix<double>.Build.Dense(1, temp.Length, temp))[0, 0];
                Y.SetRow(t, Vector<double>.Build.Dense(1, y));
            }
            mse = 0; // Minimal square error
            for (int i = 0; i < testLenght; i++)
            {
                mse += Math.Sqrt(Y[1, i] - Yt[1, i]);
            }
            mse /= testLenght;
        }

        public double Ask(double[] inputData)
        {
            reservoir.x = (1 - a) * reservoir.x + a * Matrix<double>.Tanh(Win * Matrix<double>.Build.Dense(1, inputData.Length, inputData) + W * reservoir.x);

            double[] temp = new double[inputData.Length + reservoir.x.ToArray().Length];
            double[] tempX = reservoir.x.Row(0).ToArray();
            for (int i = 0; i < temp.Length; i++)
            {

                if (i < inputData.Length)
                {
                    temp[i] = inputData[i];
                }
                else
                {
                    temp[i] = tempX[i - tempX.Length];
                }
            }

            return (Wout * Matrix<double>.Build.Dense(1, temp.Length, temp))[0, 0];
        }
    }
}
