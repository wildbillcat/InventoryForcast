using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsCalc
{
    public static class Linear
    {
        public static double[] Forcast(double _xValueToPredict, double[] _xValues, double[] _yValues)
        {
            double xBar = _xValues.Average();
            double yBar = _yValues.Average();
            //a + bx
            double b = GetSlopeValue(_xValues, _yValues, xBar, yBar); //Slope
            double a = GetInterceptValue(b, xBar, yBar); //intercept
            return new double[] { a, b, (a + (b * _xValueToPredict)) };
        }//

        static double GetInterceptValue(double _b, double _xBar, double _yBar)
        {
            return (_yBar - (_b * _xBar));
        }

        static double GetSlopeValue(double[] _xValues, double[] _yValues, double _xBar, double _yBar)
        {
            List<double> numerator = new List<double>();
            List<double> denominator = new List<double>();
            for (int i = 0; i < _yValues.Count(); i++)
            {
                double y = _yValues[i];
                double x = _xValues[i];
                double compute = (x - _xBar) * (y - _yBar);
                numerator.Add(compute);

                double xMinusxBar = x - _xBar;
                double computeDenom = xMinusxBar * xMinusxBar;
                denominator.Add(computeDenom);
            }

            double result = numerator.Sum() / denominator.Sum();
            return result;
        }
    }
}
