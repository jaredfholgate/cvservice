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
  public class CvController : BaseController
  {
    private readonly ICvService _cvService;

    public CvController(ICvService cvService)
    {
      _cvService = cvService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Cv>> Get()
    {
      return _cvService.Get(RootUrl);
    }

    [HttpGet("{id}")]
    public ActionResult<Cv> Get(int id)
    {
      return _cvService.Get(id, RootUrl);
    }

    [HttpPost]
    public ActionResult<Cv> Post([FromBody] Cv cv)
    {
      var newCv = _cvService.Add(cv, RootUrl);
      return newCv;
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] Cv cv)
    {
      cv.Id = id;
      _cvService.Update(cv);
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      _cvService.Delete(id);
    }
  }
}
