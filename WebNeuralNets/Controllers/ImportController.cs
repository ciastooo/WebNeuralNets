using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using WebNeuralNets.Models.DB;
using WebNeuralNets.Models.Dto;

namespace WebNeuralNets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly WebNeuralNetDbContext _dbcontext;

        public ImportController(WebNeuralNetDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Json(NeuralNetDto neuralNet)
        {
            try
            {
                var userId = HttpContext.Session.GetString("_id");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var dbModel = new NeuralNet
                {
                    Name = neuralNet.Name,
                    Description = neuralNet.Description,
                    TrainingRate = neuralNet.TrainingRate,
                    TrainingIterations = neuralNet.TrainingIterations,
                    UserId = userId,
                    Layers = new List<Layer>()
                };

                for (int i = 0; i < neuralNet.Layers.Count; i++)
                {
                    var layer = neuralNet.Layers[i];
                    var dbLayer = new Layer
                    {
                        Iteration = layer.Iteration,
                        Order = i,
                        Neurons = new List<Neuron>()
                    };
                    for (int j = 0; j < layer.Neurons.Count; j++)
                    {
                        var neuron = layer.Neurons[j];
                        var dbNeuron = new Neuron
                        {
                            Bias = neuron.Bias,
                            PreviousDendrites = new List<Dendrite>(),
                            NextDendrites = new List<Dendrite>(),
                        };
                        if (i > 0)
                        {
                            for (int k = 0; k < neuron.PreviousDendrites.Count; k++)
                            {
                                var dendrite = neuron.PreviousDendrites[k];
                                var dbDendrite = new Dendrite
                                {
                                    NextNeuron = dbNeuron,
                                    Weight = dendrite.Weight,
                                    PreviousNeuron = dbModel.Layers.Skip(i-1).First().Neurons.Skip(k).First()
                                };
                                dbNeuron.PreviousDendrites.Add(dbDendrite);
                            }
                        }

                        dbLayer.Neurons.Add(dbNeuron);
                    }
                    dbModel.Layers.Add(dbLayer);
                }

                await _dbcontext.NeuralNets.AddAsync(dbModel);
                await _dbcontext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}