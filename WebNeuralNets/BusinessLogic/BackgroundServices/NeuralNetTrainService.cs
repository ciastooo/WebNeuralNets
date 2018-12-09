using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebNeuralNets.Models.DB;

namespace WebNeuralNets.BusinessLogic.BackgroundServices
{
    public class NeuralNetTrainService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INeuralNetTrainer _neuralNetTrainer;

        public NeuralNetTrainService(IServiceProvider serviceProvider, INeuralNetTrainer neuralNetTrainer)
        {
            _serviceProvider = serviceProvider;
            _neuralNetTrainer = neuralNetTrainer;
        }

        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetService<WebNeuralNetDbContext>();
                        var toTrain = await dbContext.FetchNeuralnetsToTrain();
                        foreach (var neuralNet in toTrain)
                        {
                            for (int i = 0; i < 1000; i++)
                            {
                                foreach (var trainingData in neuralNet.TrainingData)
                                {
                                    foreach (var trainingSet in trainingData.TrainingSet)
                                    {
                                        _neuralNetTrainer.Train(neuralNet, trainingSet.Input.ToArray(), trainingSet.Output.ToArray());
                                    }
                                }
                                neuralNet.TrainingIterations += 1;
                            }
                        }
                        await dbContext.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
