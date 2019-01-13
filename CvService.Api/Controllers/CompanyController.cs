using CvService.Services;
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


    }
  }
}
