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
  public class CompanyController : BaseController
  {
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
      _companyService = companyService;

    }

    /// <summary>
    /// Gets all the companies for a specific CV.
    /// </summary>
    /// <param name="cvId">The Id of the CV to get the companies for.</param>
    /// <response code="200">Successfully got the companies.</response>
    /// <response code="400">Bad request.</response>
    /// <returns>An array of companies</returns>
    [HttpGet("cv/{cvId}/companies")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public ActionResult<IEnumerable<Company>> GetForCv(int cvId)
    {
      return StatusCode(StatusCodes.Status200OK, _companyService.GetForCv(cvId, RootUrl));
    }

    /// <summary>
    /// Get a company by it's id.
    /// </summary>
    /// <param name="id">The Id of the company to get.</param>
    /// <response code="200">Successfully got the company.</response>
    /// <response code="400">Bad request.</response>
    /// <returns>A company record.</returns>
    [HttpGet("company/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public ActionResult<Company> Get(int id)
    {
      return StatusCode(StatusCodes.Status200OK, _companyService.Get(id, RootUrl));
    }

    /// <summary>
    /// Add a company to a Cv.
    /// </summary>
    /// <param name="cvId">The Id of the CV to add the company to.</param>
    /// <param name="company">The company details.</param>
    /// <response code="201">Successfully Added the company to the CV and returns the new company with it's Id.</response>
    /// <response code="400">Bad request.</response>
    /// <returns>The company that was added with it's Id.</returns>
    [HttpPost("cv/{cvId}/companies")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public ActionResult<Company> Post(int cvId, [FromBody] CompanyData company)
    {
      return StatusCode(StatusCodes.Status201Created, _companyService.AddToCv(company, cvId, RootUrl));
    }

    /// <summary>
    /// Updates a specific company.
    /// </summary>
    /// <param name="id">The Id of the company to update.</param>
    /// <param name="company">The company data to be updated.</param>
    /// <response code="200">Successful Update.</response>
    /// <response code="400">Bad request.</response>
    /// <returns>A status code.</returns>
    [HttpPut("company/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult Put(int id, [FromBody] CompanyData company)
    {
      _companyService.Update(id, company);
      return StatusCode(StatusCodes.Status200OK);
    }

    /// <summary>
    /// Deletes a specific company.
    /// </summary>
    /// <param name="id">The Id of the company to delete.</param>
    /// <response code="204">Successful Deletion.</response>
    /// <response code="400">Bad request.</response>
    /// <returns>A status code.</returns>
    [HttpDelete("company/{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult Delete(int id)
    {
      _companyService.Delete(id);
      return StatusCode(StatusCodes.Status204NoContent);
    }
  }
}
