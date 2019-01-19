using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CvService.Services;
using CvService.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CvService.Api.Controllers
{
  [Route("cv")]
  [ApiController]
  public class CvController : BaseController
  {
    private readonly ICvService _cvService;

    public CvController(ICvService cvService)
    {
      _cvService = cvService;
    }

    /// <summary>
    /// Gets all the CV's.
    /// </summary>
    /// <response code="200">Successfully got the CV's.</response>
    /// <response code="400">Bad request.</response>
    /// <returns>Returns all the CV's.</returns>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public ActionResult<IEnumerable<Cv>> Get()
    {
      return StatusCode(StatusCodes.Status200OK, _cvService.Get(RootUrl));
    }

    /// <summary>
    /// Gets a CV.
    /// </summary>
    /// <param name="id">The Id of the CV to get.</param>
    /// <response code="200">Successfully found the CV.</response>
    /// <response code="400">Bad request.</response>
    /// <returns>A CV record without it's child records.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public ActionResult<Cv> Get(int id)
    {
      return StatusCode(StatusCodes.Status200OK, _cvService.Get(id, RootUrl));
    }

    /// <summary>
    /// Gets a full CV, including all it's child records.
    /// </summary>
    /// <param name="id">The Id of the CV to get.</param>
    /// <response code="200">Successfully found the CV.</response>
    /// <response code="400">Bad request.</response>
    /// <returns>A full CV with all it's child records.</returns>
    [HttpGet("{id}/full")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public ActionResult<FullCv> GetFull(int id)
    {
      return StatusCode(StatusCodes.Status200OK,_cvService.Get(id, RootUrl, true));
    }

    /// <summary>
    /// Adds a new CV.
    /// </summary>
    /// <param name="cv">The CV data to be saved.</param>
    /// <response code="201">Successfully Added the Cv and returns the new CV.</response>
    /// <response code="400">Bad request.</response>
    /// <returns>A status code and the new CV with it's Id</returns>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public ActionResult<Cv> Post([FromBody] CvData cv)
    {
      return StatusCode(StatusCodes.Status201Created, _cvService.Add(cv, RootUrl));
    }

    /// <summary>
    /// Updates a specific CV.
    /// </summary>
    /// <param name="id">The Id of the CV to be updated.</param>
    /// <param name="cv">The CV data to be updated.</param>
    /// <response code="200">Successful Update.</response>
    /// <response code="400">Bad request.</response>
    /// <returns>A status code.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult Put(int id, [FromBody] CvData cv)
    {
      _cvService.Update(id, cv);
      return StatusCode(StatusCodes.Status200OK);
    }

    /// <summary>
    /// Delete a specific CV.
    /// </summary>
    /// <param name="id">The Id of the CV to delete.</param>
    /// <response code="204">Successful Deletion.</response>
    /// <response code="400">Bad request.</response>
    /// <returns>A status code.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult Delete(int id)
    {
       _cvService.Delete(id);
      return StatusCode(StatusCodes.Status204NoContent);
    }
  }
}
