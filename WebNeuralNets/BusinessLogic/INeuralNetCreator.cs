using System.Collections.Generic;
using WebNeuralNets.Models.DB;
using WebNeuralNets.Models.Dto;

namespace WebNeuralNets.BusinessLogic
{
    public interface INeuralNetCreator
    {
        NeuralNet CreateNeuralNet(NeuralNetDto neuralNetDto);
        IList<Layer> CreateLayerIteration(NeuralNet neuralNet);
        IList<Layer> CreateLayers(List<int> neuronsCount);
    }
}