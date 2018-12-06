using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebNeuralNets.Models.DB;
using WebNeuralNets.Models.Dto;

namespace WebNeuralNets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LayerController : ControllerBase
    {
        private readonly WebNeuralNetDbContext _dbContext;

        public LayerController(WebNeuralNetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetIteration(int neuralNetId, int? iteration = null)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var query = _dbContext.Layers.Where(l => l.NeuralNetId == neuralNetId && l.NeuralNet.UserId == userId);
                if(iteration.HasValue)
                {
                    query = query.Where(l => l.Iteration == iteration.Value);
                } else
                {
                    query = query.GroupBy(l => l.Iteration).OrderByDescending(l => l.Key).SelectMany(l => l.ToList());
                }
                var result = await query.Select(l => new LayerDto
                {
                    Id = l.Id,
                    Iteration = l.Iteration,
                    Neurons = l.Neurons.Select(n => new NeuronDto
                    {
                        Id = n.Id,
                        Bias = n.Bias,
                        PreviousDendrites = n.PreviousDendrites.Select(d => new DendriteDto
                        {
                            Id = d.Id,
                            Weight = d.Weight,
                            NextNeuronId = d.NextNeuronId,
                            PreviousNeuronId = d.PreviousNeuronId
                        }).ToList(),
                        NextDendrites = n.NextDendrites.Select(d => new DendriteDto
                        {
                            Id = d.Id,
                            Weight = d.Weight,
                            NextNeuronId = d.NextNeuronId,
                            PreviousNeuronId = d.PreviousNeuronId
                        }).ToList(),
                    }).ToList()
                }).ToListAsync();

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //[HttpPut("NeuralNet/{id:int}")]
        //public async Task<IActionResult> AddIteration(int neuralNetId, List<LayerDto> model)
        //{
        //    try
        //    {
        //        if(!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        if(model == null || model.Count < 2)
        //        {
        //            return BadRequest("VALIDATION_LAYERSCOUNT");
        //        }

        //        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //        var neuralNet = await _dbContext.NeuralNets.Where(nn => nn.Id == neuralNetId && nn.UserId == userId).FirstOrDefaultAsync();
        //        if(neuralNet == null)
        //        {
        //            return BadRequest("VALIDATION_NEURALNETINVALIDID");
        //        }

        //        var newIteration = neuralNet.Layers.Select(l => l.Iteration).OrderByDescending(l => l).FirstOrDefault() + 1;

        //        var dbModels = new List<Layer>();
                
        //        for(int i = 0; i < model.Count; i++)
        //        {
        //            var layer = model[i];
        //            var dbModel = new Layer
        //            {
        //                NeuralNetId = neuralNet.Id,
        //                Iteration = newIteration,
        //                Order = i,
        //                Neurons = new List<Neuron>()
        //            };
        //            if(layer.Neurons == null || layer.Neurons.Count < 1)
        //            {
        //                return BadRequest("VALIDATION_INVALIDNEURONCOUNT");
        //            }
        //            foreach(var neuron in layer.Neurons)
        //            {
        //                var dbNeuron = new Neuron
        //                {
        //                    Bias = neuron.Bias,
        //                    Delta = neuron.Delta,
        //                };
                        
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}
    }
}