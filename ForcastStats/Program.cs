using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatsCalc;

namespace ForcastStats
{
    class Program
    {
        static void Main(string[] args)
        {
            //new List<double> {0.0, 0.0, 0.0, 0.0};
            double[] yValues = new double[] { 0, 1, 1, 0, 1, 0 };
            double[] xValues = new double[] { 8, 7, 6, 5, 4, 3 };
            double xValueToPredict = 9;

            double[] t = Linear.Forcast(xValueToPredict, xValues, yValues);
            System.Console.WriteLine(t[2]);
            System.Console.ReadLine();

            System.Console.WriteLine((DateTime.Now.Date.Year * 12 * DateTime.Now.Date.Month));
            System.Console.ReadLine();
        }
    }
}
