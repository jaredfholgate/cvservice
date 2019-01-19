using CvService.Services;
using CvService.Services.Models;
using Microsoft.AspNetCore.Http;
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

    /// <summary>
    /// Gets all the Skills for a specific Cv.
    /// </summary>
    /// <param name="cvId">The Id of the CV to get the skills for.</param>
    /// <response code="200">Successfully got the skills.</response>
    /// <response code="400">Bad request.</response>
    /// <returns>An array of skills</returns>
    [HttpGet("cv/{cvId}/skills")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public ActionResult<IEnumerable<Skill>> GetForCv(int cvId)
    {
      return StatusCode(StatusCodes.Status200OK, _skillService.GetForCv(cvId, RootUrl));
    }

    /// <summary>
    /// Get a skil by it's id.
    /// </summary>
    /// <param name="id">The Id of the skill to get.</param>
    /// <response code="200">Successfully got the skill.</response>
    /// <response code="400">Bad request.</response>
    /// <returns>A skill record.</returns>
    [HttpGet("skill/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public ActionResult<Skill> Get(int id)
    {
      return StatusCode(StatusCodes.Status200OK, _skillService.Get(id, RootUrl));
    }

    /// <summary>
    /// Add a skill to a Cv
    /// </summary>
    /// <param name="cvId">The Id of the CV to add the skill to.</param>
    /// <param name="skill">The skill details.</param>
    /// <response code="201">Successfully Added the Skill to the CV and returns the new Skill.</response>
    /// <response code="400">Bad request.</response>
    /// <returns>The skill that was added with it's Id.</returns>
    [HttpPost("cv/{cvId}/skills")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public ActionResult<Skill> Post(int cvId, [FromBody] Skill skill)
    {
      return StatusCode(StatusCodes.Status201Created, _skillService.AddToCv(skill, cvId, RootUrl));
    }

    /// <summary>
    /// Updates a specific Skill.
    /// </summary>
    /// <param name="id">The Id of the Skill to update.</param>
    /// <param name="skill">The Skill data to be updated.</param>
    /// <response code="200">Successful Update.</response>
    /// <response code="400">Bad request.</response>
    /// <returns>A status code.</returns>
    [HttpPut("skill/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult Put(int id, [FromBody] Skill skill)
    {
      _skillService.Update(id, skill);
      return StatusCode(StatusCodes.Status200OK);
    }

    /// <summary>
    /// Deletes a specific Skill.
    /// </summary>
    /// <param name="id">The Id of the Skill to delete.</param>
    /// <response code="204">Successful Deletion.</response>
    /// <response code="400">Bad request.</response>
    /// <returns>A status code.</returns>
    [HttpDelete("skill/{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult Delete(int id)
    {
      _skillService.Delete(id);
      return StatusCode(StatusCodes.Status204NoContent);
    }
  }
}
