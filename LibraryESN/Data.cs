using System;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;
using System.IO;

namespace LibraryESN
{
    public class Data
    {
        
        public List<double> ExpetedOutput;
        public List<Input> InputData;

        public Data(string filePath)
        {
            using (StreamReader sr = File.OpenText(filePath))
            {
                string s = "";
                string[] record;
                while ((s = sr.ReadLine()) != null)
                {
                    record = s.Split(';');
                    InputData.Add(new Input(int.Parse(record[0]),double.Parse(record[1])));
                }
                sr.Close();
            }
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
        int x;
        double y;

        public Input(int _x, double _y)
        {
            x = _x;
            y = _y;
        }
    }
}