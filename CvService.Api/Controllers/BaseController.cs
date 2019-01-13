using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CvService.Api.Controllers
{
  public class BaseController : ControllerBase
  {
    public string RootUrl {  get { return $"{Request.Scheme}://{Request.Host.Value}"; } }
  }
}
