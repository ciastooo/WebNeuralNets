namespace WebNeuralNets.BusinessLogic.ActivationFunctions
{
    public interface IActivationFunction
    {
        double Count(double x);

        double Derivative(double x);
    }
}
