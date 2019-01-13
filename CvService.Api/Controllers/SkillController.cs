using CvService.Services;
using CvService.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CvService.Api.Controllers
{
  [ApiController]
  public class SkillController : BaseController
  {
    private readonly ISkillService _skillService;

    public SkillController(ISkillService skillService)
    {
      _skillService = skillService;
    }

    [HttpGet("cv/{cvId}/skills")]
    public ActionResult<IEnumerable<Skill>> GetForCv(int cvId)
    {
      return _skillService.GetForCv(cvId, RootUrl);
    }

    [HttpGet("skill/{id}")]
    public ActionResult<Skill> Get(int id)
    {
      return _skillService.Get(id, RootUrl);
    }

    [HttpPost("cv/{cvId}/skills")]
    public ActionResult<Skill> Post(int cvId, [FromBody] Skill skill)
    {
      return _skillService.AddToCv(skill, cvId, RootUrl);
    }

    [HttpPut("skill/{id}")]
    public void Put(int id, [FromBody] Skill skill)
    {
      skill.Id = id;
      _skillService.Update(skill);
    }

    [HttpDelete("skill/{id}")]
    public void Delete(int id)
    {
      _skillService.Delete(id);
    }
  }
}
