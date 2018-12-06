using System;

namespace WebNeuralNets.BusinessLogic.ActivationFunctions
{
    public class Sigmoid : IActivationFunction
    {
        public double Count(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        public double Derivative(double x)
        {
            return x * (1 - x);
        }
    }
}
