using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebNeuralNets.BusinessLogic;
using WebNeuralNets.Models.DB;
using WebNeuralNets.Models.Dto;

namespace WebNeuralNets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NeuralNetController : ControllerBase
    {
        private readonly WebNeuralNetDbContext _dbcontext;
        private readonly INeuralNetCreator _neuralNetCreator;
        private readonly INeuralNetTrainer _neuralNetTrainer;

        public NeuralNetController(WebNeuralNetDbContext dbContext, INeuralNetCreator neuralNetCreator, INeuralNetTrainer neuralNetTrainer)
        {
            _dbcontext = dbContext;
            _neuralNetCreator = neuralNetCreator;
            _neuralNetTrainer = neuralNetTrainer;
        }

        [HttpPut]
        public async Task<IActionResult> Add(string name, double trainingRate, string description)
        {
            try
            {
                if (string.IsNullOrEmpty(name) || trainingRate < 0.01)
                {
                    return BadRequest();
                }

                var model = new NeuralNetDto
                {
                    Name = name,
                    Description = description,
                    TrainingRate = trainingRate
                };

                var dbModel = _neuralNetCreator.CreateNeuralNet(model);
                await _dbcontext.NeuralNets.AddAsync(dbModel);
                await _dbcontext.SaveChangesAsync();

                return Ok(dbModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var result = await _dbcontext.NeuralNets.Where(n => n.UserId == userId).Select(n => new NeuralNetDto
                {
                    Id = n.Id,
                    Name = n.Name,
                    Description = n.Description,
                    Iterations = n.Layers.GroupBy(l => l.Iteration).Count(),
                    TrainingIterations = n.TrainingIterations,
                    TrainingRate = n.TrainingRate,
                    AverageError = n.AverageError
                }).ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var result = await _dbcontext.NeuralNets.Where(nn => nn.Id == id && nn.UserId == userId).Select(nn => new NeuralNetDto
                    {
                        Id = nn.Id,
                        Description = nn.Description,
                        Name = nn.Name,
                        TrainingRate = nn.TrainingRate,
                        Iterations = nn.Layers.GroupBy(l => l.Iteration).Count(),
                        Training = nn.Training,
                        TrainingIterations = nn.TrainingIterations,
                        AverageError = nn.AverageError,
                        TrainingData = nn.TrainingData.Select(td => new TrainingDataDto
                        {
                            Id = td.Id,
                            Name = td.Name,
                            TrainingSet = td.TrainingSet.Select(ts => new TrainingSetDto
                            {
                                Input = ts.Input.ToList(),
                                Output = ts.Output.ToList(),
                            }).ToList()
                        }).ToList(),
                        Layers = nn.Layers.GroupBy(l => l.Iteration)
                                         .OrderByDescending(l => l.Key)
                                         .SelectMany(l => l.ToList())
                                         .OrderBy(l => l.Order)
                                         .Select(l => new LayerDto
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
                                         }).ToList()
                    }).FirstOrDefaultAsync();

                    return Ok(result);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(NeuralNetDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var dbModel = await _dbcontext.NeuralNets.Where(n => n.Id == model.Id && n.UserId == userId).FirstOrDefaultAsync();

                    if (dbModel == null)
                    {
                        return NotFound();
                    }

                    dbModel.Name = model.Name;
                    dbModel.Description = model.Description;
                    dbModel.Training = model.Training;

                    await _dbcontext.SaveChangesAsync();

                    return Ok();
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var dbModel = await _dbcontext.NeuralNets.Where(n => n.Id == id && n.UserId == userId).FirstOrDefaultAsync();

                if (dbModel == null)
                {
                    return NotFound();
                }

                _dbcontext.NeuralNets.Remove(dbModel);
                await _dbcontext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{id:int}/Propagate")]
        public async Task<IActionResult> Propagate(int id, List<double> input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var neuralNet = await _dbcontext.NeuralNets.Include(nn => nn.Layers)
                                                                    .ThenInclude(l => l.Neurons)
                                                                    .ThenInclude(n => n.PreviousDendrites)
                                                               .Include(nn => nn.Layers)
                                                                    .ThenInclude(l => l.Neurons)
                                                                    .ThenInclude(n => n.NextDendrites)
                                                               .Where(nn => nn.Id == id && nn.UserId == userId).FirstOrDefaultAsync();

                    if (neuralNet == null)
                    {
                        return BadRequest();
                    }

                    var inputLayer = neuralNet.Layers.OrderBy(l => l.Order).FirstOrDefault().Neurons.Count();
                    if (inputLayer != input.Count)
                    {
                        return BadRequest();
                    }

                    var outputLayerValues = _neuralNetTrainer.Propagate(neuralNet, input.ToArray());
                    return Ok(outputLayerValues);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{id:int}/TrainingData")]
        public async Task<IActionResult> UploadTrainingData(int id, TrainingDataDto model)
        {
            try
            {
                if (!ModelState.IsValid || model.TrainingSet == null)
                {
                    return BadRequest("VALIDATION_INVALIDTRAININGDATA");
                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var neuralNet = await _dbcontext.NeuralNets.Include(nn => nn.Layers).ThenInclude(l => l.Neurons).Where(nn => nn.Id == id && nn.UserId == userId).FirstOrDefaultAsync();
                if (neuralNet == null)
                {
                    return BadRequest();
                }
                var inputLayer = neuralNet.Layers.OrderBy(l => l.Order).FirstOrDefault().Neurons.Count();
                var outputLayer = neuralNet.Layers.OrderByDescending(l => l.Order).FirstOrDefault().Neurons.Count();

                if (model.TrainingSet.Any(ts => ts.Input.Count != inputLayer || ts.Output.Count != outputLayer))
                {
                    return BadRequest("VALIDATION_INVALIDTRAININGDATA");
                }

                var dbTrainingData = new TrainingData
                {
                    Name = model.Name,
                    NeuralNetId = id,
                    TrainingSet = model.TrainingSet.Select(ts => new TrainingSet
                    {
                        Input = ts.Input,
                        Output = ts.Output
                    }).ToList()
                };

                await _dbcontext.TrainingData.AddAsync(dbTrainingData);
                await _dbcontext.SaveChangesAsync();
                model.Id = dbTrainingData.Id;

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{id:int}/TrainingData")]
        public async Task<IActionResult> GetTrainingData(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var trainingData = await _dbcontext.TrainingData.Where(t => t.NeuralNetId == id && t.NeuralNet.UserId == userId)
                                                                .Select(t => new TrainingDataDto
                                                                {
                                                                    Id = t.Id,
                                                                    Name = t.Name,
                                                                    TrainingSet = t.TrainingSet.Select(ts => new TrainingSetDto
                                                                    {
                                                                        Input = ts.Input.ToList(),
                                                                        Output = ts.Output.ToList()
                                                                    }).ToList()
                                                                }).ToListAsync();

                return Ok(trainingData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("{id:int}/Train")]
        public async Task<IActionResult> Train(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var dbModel = await _dbcontext.NeuralNets.Where(n => n.Id == id && n.UserId == userId).FirstOrDefaultAsync();

                if (dbModel == null)
                {
                    return NotFound();
                }

                dbModel.Training = !dbModel.Training;

                await _dbcontext.SaveChangesAsync();

                return Ok(dbModel.Training ? "NEURALNET_TRAININGACTIVATED" : "NEURALNET_TRAININGDISACTIVATED");

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("CurrentlyTraining")]
        public async Task<IActionResult> GetCurrentlyTraining()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var result = await _dbcontext.NeuralNets.Where(n => n.UserId == userId && n.Training).Select(nn => nn.Id).ToListAsync();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id:int}/TrainingData/{setId:int}")]
        public async Task<IActionResult> GetTrainingData(int id, int setId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var trainingData = await _dbcontext.TrainingData.Where(t => t.Id == setId && t.NeuralNetId == id && t.NeuralNet.UserId == userId).FirstOrDefaultAsync();
                if (trainingData == null)
                {
                    return NotFound();
                }

                _dbcontext.TrainingData.Remove(trainingData);
                await _dbcontext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Dendrite/{id:int}")]
        public async Task<IActionResult> UpdateWeight(int id, double newWeight)
        {
            try
            {
                var dbModel = await _dbcontext.Dendrites.Where(d => d.Id == id).FirstOrDefaultAsync();

                if (dbModel == null)
                {
                    return NotFound();
                }

                dbModel.Weight = newWeight;

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