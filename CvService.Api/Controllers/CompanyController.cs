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
  public class CompanyController : BaseController
  {
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
      _companyService = companyService;

    }

    [HttpGet("cv/{cvId}/companies")]
    public ActionResult<IEnumerable<Company>> GetForCv(int cvId)
    {
      return _companyService.GetForCv(cvId, RootUrl);
    }

    [HttpGet("company/{id}")]
    public ActionResult<Company> Get(int id)
    {
      return _companyService.Get(id, RootUrl);
    }

    [HttpPost("cv/{cvId}/companies")]
    public ActionResult<Company> Post(int cvId, [FromBody] Company company)
    {
      return _companyService.AddToCv(company, cvId, RootUrl);
    }

    [HttpPut("company/{id}")]
    public void Put(int id, [FromBody] Company company)
    {
      company.Id = id;
      _companyService.Update(company);
    }

    [HttpDelete("company/{id}")]
    public void Delete(int id)
    {
      _companyService.Delete(id);
    }
  }
}
