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
            var layers = CreateLayers(new List<int> {2, 3, 3, 1});

            return layers;
        }
        
        public virtual IList<Layer> CreateLayers(List<int> neuronsCount)
        {
            var layers = new List<Layer>();

            Layer previousLayer = null;
            for (int i = 0; i < neuronsCount.Count; i++)
            {
                var layer = CreateLayer(0, i, neuronsCount[i], previousLayer);
                previousLayer = layer;
                layers.Add(layer);
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
