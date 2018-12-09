using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public class ExportController : ControllerBase
    {
        private readonly WebNeuralNetDbContext _dbcontext;

        public ExportController(WebNeuralNetDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        [HttpGet("json/{id:int}")]
        public async Task<IActionResult> ExportNeuralNetJson(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await GetNeuralNet(id);

                    if (result == null)
                    {
                        return BadRequest();
                    }

                    using (var stream = new MemoryStream())
                    {
                        var resultJson = JsonConvert.SerializeObject(result);
                        var writer = new StreamWriter(stream);
                        writer.Write(resultJson);
                        writer.Flush();
                        stream.Seek(0, SeekOrigin.Begin);
                        return File(stream.GetBuffer(), "application/octet-stream", $"neuralNet_{id}.json");
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("xml/{id:int}")]
        public async Task<IActionResult> ExportNeuralNetXml(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await GetNeuralNet(id);

                    if (result == null)
                    {
                        return BadRequest();
                    }

                    using (var stream = new MemoryStream())
                    {
                        XmlSerializer serializer = new XmlSerializer(result.GetType());
                        serializer.Serialize(XmlWriter.Create(stream), result);
                        stream.Flush();
                        stream.Seek(0, SeekOrigin.Begin);
                        return File(stream.GetBuffer(), "application/octet-stream", $"neuralNet_{id}.xml");
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private async Task<NeuralNetDto> GetNeuralNet(int id)
        {
            var userId = HttpContext.Session.GetString("_id");
            return await _dbcontext.NeuralNets.Where(nn => nn.Id == id && nn.UserId == userId).Select(nn => new NeuralNetDto
            {
                Id = nn.Id,
                Description = nn.Description,
                Name = nn.Name,
                TrainingRate = nn.TrainingRate,
                Iterations = nn.Layers.GroupBy(l => l.Iteration).Count(),
                Training = nn.Training,
                TrainingIterations = nn.TrainingIterations,
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
        }
    }
}