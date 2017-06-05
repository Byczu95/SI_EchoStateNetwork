using System;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;
using System.IO;

namespace LibraryESN
{
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
                    InputData.Add(new Input(int.Parse(record[0]),double.Parse(record[1])));
                }
                sr.Close();
                DataLenght = InputData.Count;
            }
        }

        public double[] GetExpetedOutputArray(int start, int lenght)
        {
            double[] result = new double[lenght - start];

            for(int i = start; i < lenght; i++)
            {
                result[i] = InputData[i].y;
            }

            return result;
        }
    }

    public class Input
    {
        public int x;
        public double y;

        public Input(int _x, double _y)
        {
            x = _x;
            y = _y;
        }
    }
}