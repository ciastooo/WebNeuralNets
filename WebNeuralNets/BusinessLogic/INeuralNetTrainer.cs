using System.Collections.Generic;
using WebNeuralNets.Models.DB;

namespace WebNeuralNets.BusinessLogic
{
    public interface INeuralNetTrainer
    {
        void Propagate(IList<Layer> layers, IList<double> input);
        void Propagate(NeuralNet neuralNet, IList<double> input);
        void Train(NeuralNet neuralNet, List<double> input, List<double> output);
    }
}