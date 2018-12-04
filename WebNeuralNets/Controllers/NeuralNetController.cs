using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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

        public NeuralNetController(WebNeuralNetDbContext dbContext, INeuralNetCreator neuralNetCreator)
        {
            _dbcontext = dbContext;
            _neuralNetCreator = neuralNetCreator;
        }

        [HttpPut]
        public async Task<IActionResult> Add(NeuralNetDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dbModel = _neuralNetCreator.CreateNeuralNet(model);
                    await _dbcontext.NeuralNets.AddAsync(dbModel);
                    await _dbcontext.SaveChangesAsync();

                    model.Id = dbModel.Id;
                    return Ok(model);
                }
                return BadRequest(ModelState);
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
                    Iterations = n.Layers.GroupBy(l => l.Iteration).Count()
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
                                                 Delta = n.Delta,
                                                 PreviousDendrites = n.PreviousDendrites.Select(d => new DendriteDto
                                                 {
                                                     Id = d.Id,
                                                     Delta = d.Delta,
                                                     Weight = d.Weight,
                                                     NextNeuronId = d.NextNeuronId,
                                                     PreviousNeuronId = d.PreviousNeuronId
                                                 }).ToList(),
                                                 NextDendrites = n.NextDendrites.Select(d => new DendriteDto
                                                 {
                                                     Id = d.Id,
                                                     Delta = d.Delta,
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
    }
}