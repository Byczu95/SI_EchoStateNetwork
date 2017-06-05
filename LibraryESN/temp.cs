using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryESN;
using MathNet.Numerics.LinearAlgebra;

namespace SurveyESN
{
    public class temp
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
        public Matrix<double> x;
        public Matrix<double> Yt; // Oczekiwane wartości
        public Matrix<double> Y; // Macierz wyjścia
        public double rhoW;
        public Data data;
        public double mse;

        //Konstruktor
        public temp(int reservoirSize, double leakingRate)
        {
            teached = false;
            InitializeParameters(reservoirSize, leakingRate); // <- a
            NormalizeWeights(); // Zapewnij rozrzut wag
        }

        // Przypisanie parametrów startowych
        private void InitializeParameters(int resSize, double alpha)
        {
            size = resSize;
            a = alpha;
        }

        // Normalizowanie macierzy wag neuronów
        private void NormalizeWeights()
        {
            Win = Matrix<double>.Build.Random(size, 2) - 0.5;
            W = Matrix<double>.Build.Random(size, size) - 0.5;

            rhoW = W.Evd().EigenValues.AbsoluteMaximum().Real; //rhoW = max(abs(eigenvalues(W))

            W *= 1.25 / rhoW; // Normalizacja
        }

        // Ładowanie danych
        public void Learn(string path, int ignoredInitialResults)
        {
            // Inicjacja zmiennych
            data = new Data(path);
            X = Matrix<double>.Build.Random(data.DataLenght - ignoredInitialResults, ignoredInitialResults + 1 + size) * 0;

            Yt = Matrix<double>.Build.Random(1, ignoredInitialResults); // data.DataLenght - 1 - ignoredInitialResults, 1 Macierz wartości spodziewanych
            Yt.SetRow(0, data.GetExpetedOutputArray(0, ignoredInitialResults)); //Ustawienie pomijanych wyników

            InputDataIntoReservoir(ignoredInitialResults); // Przejście pierszych pomijanych wyników

            double reg = (double)1; // Doładność double

            Matrix<double> X_T = X.Transpose();

            Wout = Yt * X_T * Matrix<double>.Build.DenseOfMatrix(X * X_T + reg * Matrix<double>.Build.DenseDiagonal(2 + size, 2 + size, 1)).Inverse();

            //Uczenie
            LearnProcess(ignoredInitialResults);
        }

        public void InputDataIntoReservoir(int ignoredDataCount)
        {
            x = Matrix<double>.Build.Random(size, 1) * 0;

            for (int t = 0; t < data.InputData.Count; t++)
            {
                double u = data.InputData[t].x;
                x = (1 - a) * x + a * Matrix<double>.Tanh(Win * Matrix<double>.Build.Dense(1, 1, u) + W * x);

                if (t >= ignoredDataCount)
                {
                    double[] temp = new double[1 + x.ToArray().Length];
                    double[] tempX = x.Row(0).ToArray();
                    temp[0] = u;
                    for (int i = 1; i < temp.Length; i++)
                    {
                        temp[i] = tempX[i - tempX.Length];
                    }
                    X.SetRow(t, Vector<double>.Build.DenseOfArray(temp));
                }
            }
        }

        private void LearnProcess(int initResults)
        {
            int testLenght = data.DataLenght - initResults;
            Y = Matrix<double>.Build.Dense(testLenght, 1);
            int count = data.InputData.Count;

            for (int t = initResults; t < data.DataLenght; t++)
            {
                double u = data.InputData[t].x;
                x = (1 - a) * x + a * Matrix<double>.Tanh(Win * u + W * x);

                double[] temp = new double[1 + x.ToArray().Length];
                double[] tempX = x.Row(0).ToArray();
                temp[0] = u;
                for (int i = 1; i < temp.Length; i++)
                {

                    temp[i] = tempX[i - tempX.Length];
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

        public double Ask(double inputData)
        {
            x = (1 - a) * x + a * Matrix<double>.Tanh(Win * inputData + W * x);

            double[] temp = new double[1 + x.ToArray().Length];
            double[] tempX = x.Row(0).ToArray();
            temp[0] = inputData;
            for (int i = 1; i < temp.Length; i++)
            {
                temp[i] = tempX[i - tempX.Length];
            }

            return (Wout * Matrix<double>.Build.Dense(1, temp.Length, temp))[0, 0];
        }
    }
}
