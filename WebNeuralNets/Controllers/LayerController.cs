using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebNeuralNets.BusinessLogic;
using WebNeuralNets.Models.DB;
using WebNeuralNets.Models.Dto;

namespace WebNeuralNets.Controllers
{
    [Route("api/NeuralNet/[controller]")]
    [ApiController]
    [Authorize]
    public class LayerController : ControllerBase
    {
        private readonly WebNeuralNetDbContext _dbContext;
        private readonly INeuralNetCreator _neuralNetCreator;

        public LayerController(WebNeuralNetDbContext dbContext, INeuralNetCreator neuralNetCreator)
        {
            _dbContext = dbContext;
            _neuralNetCreator = neuralNetCreator;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetIteration(int id, int? iteration = null)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var query = _dbContext.Layers.Where(l => l.NeuralNetId == id && l.NeuralNet.UserId == userId);
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

        [HttpPost("{id:int}")]
        public async Task<IActionResult> UpdateLayers(int id, List<int> neuronsCount)
        {
            try
            {
                if (neuronsCount == null || neuronsCount.Count < 2 || neuronsCount.Any(n => n < 1))
                {
                    return BadRequest("VALIDATION_WRONGNEURONSCOUNT");
                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var neuralNet = await _dbContext.NeuralNets.Where(nn => nn.Id == id && nn.UserId == userId).Include(nn => nn.Layers).Include(nn => nn.TrainingData).FirstOrDefaultAsync();

                if (neuralNet == null)
                {
                    return NotFound();
                }

                _dbContext.Layers.RemoveRange(neuralNet.Layers);
                _dbContext.TrainingData.RemoveRange(neuralNet.TrainingData);

                var newLayers = _neuralNetCreator.CreateLayers(neuronsCount);
                neuralNet.Layers = newLayers;
                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}