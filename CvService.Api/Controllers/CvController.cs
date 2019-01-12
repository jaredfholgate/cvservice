using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CvService.Services;
using CvService.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace CvService.Api.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class CvController : ControllerBase
  {
    private readonly ICvService _cvService;

    public CvController(ICvService cvService)
    {
      _cvService = cvService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Cv>> Get()
    {
      return _cvService.Get($"{Request.Scheme}://{Request.Host.Value}");
    }

    [HttpGet("{id}")]
    public ActionResult<string> Get(int id)
    {
      return "value";
    }

    [HttpPost]
    public void Post([FromBody] Cv value)
    {
      _cvService.Add(value);
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] Cv value)
    {
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
