using System.Collections.Generic;
using System.Linq;
using WebNeuralNets.BusinessLogic.ActivationFunctions;
using WebNeuralNets.Models.DB;

namespace WebNeuralNets.BusinessLogic
{
    public static class NeuralNetExtensions
    {
        public static void Propagate(this NeuralNet neuralNet, IActivationFunction activationFunction, IList<double> input)
        {
            var layers = neuralNet.Layers.OrderBy(l => l.Order).ToList();
            var inputLayer = layers.FirstOrDefault();
            var outputLayer = layers.LastOrDefault();

            int i = 0;
            foreach(var neuron in inputLayer.Neurons)
            {
                neuron.Value = input[i];
                i++;
            }

            foreach(var layer in layers.Skip(1))
            {
                foreach(var neuron in layer.Neurons)
                {
                    neuron.Value = neuron.Bias;
                    foreach(var previousNeuronDendrite in neuron.PreviousDendrites)
                    {
                        neuron.Value += previousNeuronDendrite.PreviousNeuron.Value * previousNeuronDendrite.Weight;
                    }
                    neuron.Value = activationFunction.Count(neuron.Value);
                }
            }
        }
    }
}
