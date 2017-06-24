using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryESN;
using MathNet.Numerics.LinearAlgebra;

namespace SurveyESN
{
    [Serializable]
    public class EchoStateNetwork
    {
        // Status
        public bool isTeached;

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
        public double rhoW; //
        public Data data;
        public double mse;

        //Konstruktor
        public EchoStateNetwork(int reservoirSize, double leakingRate)
        {
            isTeached = false;
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
            X = Matrix<double>.Build.Random(data.DataLenght - ignoredInitialResults, 2 + size) * 0; // 2 <- ilość parametrów wejścia i wyjścia 'x' i 'y'

            Yt = Matrix<double>.Build.Random(1, data.DataLenght - ignoredInitialResults); // data.DataLenght - 1 - ignoredInitialResults, 1 Macierz wartości spodziewanych
            Yt.SetRow(0, data.GetExpetedOutputArray(ignoredInitialResults, data.DataLenght-ignoredInitialResults)); //Ustawienie pomijanych wyników

            InputDataIntoReservoir(ignoredInitialResults); // Przejście pierszych pomijanych wyników

            double reg = (double)1; // Doładność double

            Matrix<double> X_T = X.Transpose();

            Wout = Yt * X * Matrix<double>.Build.DenseOfMatrix(X_T * X + reg * Matrix<double>.Build.DenseDiagonal(2 + size, 2 + size, 1)).Inverse();

            //Uczenie
            LearnProcess(ignoredInitialResults,ignoredInitialResults);
            isTeached = true;
        }

        public void InputDataIntoReservoir(int ignoredDataCount)
        {
            x = Matrix<double>.Build.Random(size, 1) * 0;

            for (int t = 0; t < data.InputData.Count; t++)
            {
                double[] u = data.InputData[t].ToArray();
                x = (1 - a) * x + a * Matrix<double>.Tanh(Win * Matrix<double>.Build.Dense(2, 1, u) + W * x);

                if (t >= ignoredDataCount)
                {
                    X.SetRow(t-ignoredDataCount, Vector<double>.Build.DenseOfArray(JoinArrays(u,x.Column(0).ToArray())));
                }
            }
        }

        private void LearnProcess(int initResults,int ignoredDataCount)
        {
            int testLenght = data.DataLenght - initResults;
            Y = Matrix<double>.Build.Dense(testLenght, 1);
            int count = data.InputData.Count;

            for (int t = initResults; t < data.DataLenght; t++)
            {
                double[] u = data.InputData[t].ToArray();
                x = (1 - a) * x + a * Matrix<double>.Tanh(Win * Matrix<double>.Build.Dense(2, 1, u) + W * x);

                Vector<double> temp;

                if (t >= ignoredDataCount)
                {
                    temp = Vector<double>.Build.DenseOfArray(JoinArrays(u, x.Column(0).ToArray()));

                    Matrix<double> temMat = Matrix<double>.Build.Dense(1, temp.Count);
                    temMat.SetRow(0, temp);

                    double y = (Wout * temMat.Transpose())[0, 0];
                    Y.SetRow(t-ignoredDataCount, Vector<double>.Build.Dense(1, y));
                }

                
            }

            mse = 0; // Minimal square error
            for (int i = 0; i < testLenght; i++)
            {
                double temp = Math.Abs(Y[i, 0]) - Math.Abs(Yt[0, i]);

                mse += Math.Sqrt(Math.Abs(temp));
            }
            mse /= testLenght;
        }

        public double Ask(double inputData)
        {
            double[] u = new double[2] { inputData, 0 };
            Matrix<double> x_local = (1 - a) * x + a * Matrix<double>.Tanh(Win * Matrix<double>.Build.Dense(2, 1, u) + W * x);

            Vector<double> temp = Vector<double>.Build.DenseOfArray(JoinArrays(u, x.Column(0).ToArray()));

            Matrix<double> temMat = Matrix<double>.Build.Dense(1, temp.Count);
            temMat.SetRow(0, temp);

            double y = (Wout * temMat.Transpose())[0, 0];

            return y;
        }

        public double[] JoinArrays(double[] a, double[] b)
        {
            double[] result = new double[a.Length + b.Length];

            for(int i = 0; i < a.Length; i++)
            {
                result[i] = a[i];
            }

            for (int i = 0; i < b.Length; i++)
            {
                result[a.Length + i] = b[i];
            }

            return result;
        } 
    }
}
