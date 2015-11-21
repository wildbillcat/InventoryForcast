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
            double[] yValues = new double[] { 6, 7, 9, 15, 21 };
            double[] xValues = new double[] { 20, 28, 31, 38, 40 };
            double xValueToPredict = 30;

            double[] t = Linear.Forcast(xValueToPredict, xValues, yValues);
            System.Console.WriteLine(t[2]);
            System.Console.ReadLine();

            System.Console.WriteLine((DateTime.Now.Date.Year * 12 * DateTime.Now.Date.Month));
            System.Console.ReadLine();
        }
    }
}
