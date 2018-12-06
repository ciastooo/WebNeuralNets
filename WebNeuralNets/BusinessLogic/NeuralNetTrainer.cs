using System.Collections.Generic;
using System.Linq;
using WebNeuralNets.BusinessLogic.ActivationFunctions;
using WebNeuralNets.Models.DB;

namespace WebNeuralNets.BusinessLogic
{
    public class NeuralNetTrainer : INeuralNetTrainer
    {
        private readonly IActivationFunction _activationFunction;

        NeuralNetTrainer(IActivationFunction activationFunction)
        {
            _activationFunction = activationFunction;
        }

        public void Propagate(NeuralNet neuralNet, IList<double> input)
        {
            Propagate(neuralNet.Layers.OrderBy(l => l.Order).ToList(), input);
        }

        public void Propagate(IList<Layer> layers, IList<double> input)
        {
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
                    neuron.Value = _activationFunction.Count(neuron.Value);
                }
            }
        }

        public void Train(NeuralNet neuralNet, List<double> input, List<double> output)
        {
            Propagate(neuralNet, input);
            var layers = neuralNet.Layers.OrderBy(l => l.Order).ToList();
            var inputLayer = layers.FirstOrDefault();
            var outputLayer = layers.LastOrDefault();

            int i = 0;
            foreach(var outputNeuron in outputLayer.Neurons)
            {
                outputNeuron.Delta = (outputNeuron.Value - output[i]) * _activationFunction.Derivative(outputNeuron.Value);
                foreach(var previousDendrite in outputNeuron.PreviousDendrites)
                {
                    previousDendrite.Delta = outputNeuron.Delta * previousDendrite.PreviousNeuron.Value;
                }
                i++;
            }

            for(i = layers.Count - 2; i > 0; i--)
            {
                var layer = layers[i];
                foreach(var neuron in layer.Neurons)
                {
                    neuron.Delta = 0;
                    foreach(var dendrite in neuron.NextDendrites)
                    {
                        neuron.Delta += dendrite.NextNeuron.Delta * dendrite.Weight;
                    }
                    neuron.Delta *= _activationFunction.Derivative(neuron.Value);
                    foreach (var dendrite in neuron.PreviousDendrites)
                    {
                        dendrite.Delta += neuron.Delta * dendrite.PreviousNeuron.Value;
                    }
                }
            }

            foreach (var layer in layers)
            {
                foreach (var neuron in layer.Neurons)
                {
                    foreach (var dendrite in neuron.NextDendrites)
                    {
                        dendrite.Weight -= dendrite.Delta * neuralNet.TrainingRate;
                    }
                }
            }
        }
    }
}
