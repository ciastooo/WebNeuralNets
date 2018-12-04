using WebNeuralNets.Models.DB;
using WebNeuralNets.Models.Dto;

namespace WebNeuralNets.BusinessLogic
{
    public interface INeuralNetCreator
    {
        NeuralNet CreateNeuralNet(NeuralNetDto neuralNetDto);
    }
}