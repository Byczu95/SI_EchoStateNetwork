using System;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;

namespace LibraryESN
{
    public class Data
    {
        public List<double> ExpetedOutput;
        public List<Input> InputData;
        public int DataLenght;

        public Data(string filePath)
        {
        }

        internal Vector<double> GetExpetedOutput(int v)
        {
            //Wczytać pierwsze elementy
            throw new NotImplementedException();
        }
    }

    public class Input
    {
        public List<double> values;
    }
}