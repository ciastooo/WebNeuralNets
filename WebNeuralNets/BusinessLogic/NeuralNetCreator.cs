using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebNeuralNets.Models.DB;
using WebNeuralNets.Models.Dto;

namespace WebNeuralNets.BusinessLogic
{
    public class NeuralNetCreator : INeuralNetCreator
    {
        private readonly IHttpContextAccessor _httpContext;

        public NeuralNetCreator(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public virtual NeuralNet CreateNeuralNet(NeuralNetDto neuralNetDto)
        {
            var neuralNet = new NeuralNet
            {
                Name = neuralNetDto.Name,
                Description = neuralNetDto.Description,
                TrainingRate = neuralNetDto.TrainingRate,
                Training = false,
                TrainingIterations = 0,
                UserId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value
            };

            neuralNet.Layers = CreateLayerIteration(neuralNet);

            return neuralNet;
        }

        public virtual IList<Layer> CreateLayerIteration(NeuralNet neuralNet)
        {
            var layers = new List<Layer>();

            if (neuralNet.Layers != null && neuralNet.Layers.Count > 0)
            {
                // TODO: copying existing last iteration to a new one
            }
            else
            {
                var inputLayer = CreateLayer(0, 0, 2, null);
                var firstHiddenLayer = CreateLayer(0, 1, 3, inputLayer);
                var secondHiddenLayer = CreateLayer(0, 2, 3, firstHiddenLayer);
                var outputLayer = CreateLayer(0, 3, 1, secondHiddenLayer);
                layers = new List<Layer>
                {
                    inputLayer,
                    firstHiddenLayer,
                    secondHiddenLayer,
                    outputLayer
                };
            }

            return layers;
        }

        private Layer CreateLayer(int iteration, int layerOrder, int neuronsCount, Layer previousLayer)
        {
            var layer = new Layer
            {
                Iteration = iteration,
                Order = layerOrder
            };
            var neurons = new List<Neuron>();

            for (int i = 0; i < neuronsCount; i++)
            {
                var neuron = CreateNeuron(previousLayer);
                neurons.Add(neuron);
            }

            layer.Neurons = neurons;

            return layer;
        }

        private Neuron CreateNeuron(Layer previousLayer)
        {
            var neuron = new Neuron
            {
                Bias = new Random().NextDouble(),
                PreviousDendrites = new List<Dendrite>(),
                NextDendrites = new List<Dendrite>()
            };

            if (previousLayer != null && previousLayer.Neurons != null)
            {
                foreach (var previousNeuron in previousLayer.Neurons)
                {
                    var dendrite = CreateDendrite();
                    previousNeuron.NextDendrites.Add(dendrite);
                    neuron.PreviousDendrites.Add(dendrite);
                }
            }

            return neuron;
        }

        private Dendrite CreateDendrite()
        {
            var dendrite = new Dendrite
            {
                Weight = new Random().NextDouble()
            };

            return dendrite;
        }
    }
}
