using System.Collections.Generic;
using WebNeuralNets.Models.DB;

namespace WebNeuralNets.BusinessLogic
{
    public interface INeuralNetTrainer
    {
        double[] Propagate(IList<Layer> layers, double[] input);
        double[] Propagate(NeuralNet neuralNet, double[] input);
        void Train(NeuralNet neuralNet, double[] input, double[] output);
    }
}