using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryESN;
using MathNet.Numerics.LinearAlgebra;

namespace SurveyESN
{
    class temp
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

            Yt = Matrix<double>.Build.Random(data.DataLenght - 1 - ignoredInitialResults, 1);
            Yt.SetRow(0, data.InputData[data.DataLenght - 1 - ignoredInitialResults].);

            InputDataIntoReservoir(); // Przejście pierszych pomijanych wyników

            double reg = (double)1; // Doładność double

            Matrix<double> X_T = X.Transpose();

            Wout = Yt * X_T * Matrix<double>.Build.DenseOfMatrix(X * X_T + reg * Matrix<double>.Build.DenseDiagonal(2 + size, 2 + size, 1)).Inverse();

            //Uczenie
            LearnProcess(ignoredInitialResults);
        }

        public void InputDataIntoReservoir()
        {
            x = Matrix<double>.Build.Random(size, 1) * 0;

            for (int t = 0; t < testLenght; t++)
            {
                double[] u = data.InputData[t].values.ToArray();
                x = (1 - a) * x + a * Matrix<double>.Tanh(Win * Matrix<double>.Build.Dense(1, u.Length, u) + W * x);

                if (t > initLenght)
                {
                    double[] temp = new double[u.Length + x.ToArray().Length];
                    double[] tempX = x.Row(0).ToArray();
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
                    X.SetRow(t, Vector<double>.Build.DenseOfArray(temp));
                }
            }
        }
    }
}
