using System;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;
using System.IO;

namespace LibraryESN
{
    [Serializable]
    public class Data
    {
        public List<Input> InputData;
        public int DataLenght;

        public Data(string filePath)
        {
            InputData = new List<Input>();

            using (StreamReader sr = File.OpenText(filePath))
            {
                string s;
                string[] record;
                while ((s = sr.ReadLine()) != null)
                {
                    record = s.Split(';');
                    InputData.Add(new Input(double.Parse(record[0]),double.Parse(record[1])));
                }
                sr.Close();
                DataLenght = InputData.Count;
            }
        }

        public double[] GetExpetedOutputArray(int start, int lenght)
        {
            double[] result = new double[lenght];

            for(int i = start; i < lenght; i++)
            {
                result[i] = InputData[i].y;
            }

            return result;
        }
    }
    [Serializable]
    public class Input
    {
        public double x; 
        public double y;

        public Input(double _x, double _y)
        {
            
            x = _x;
            y = _y;
        }

        public double[] ToArray()
        {
            double[] d = new double[2];
            d[0] = x;
            d[1] = y;
            return d;
        }
    }
}