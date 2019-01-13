using System;
using System.Collections.Generic;
using System.Text;

namespace CvService.Services.Models
{
  public class FullCv : Cv
  {
    public List<Company> Companies { get; set; }
    public List<Skill> Skills { get; set; }
  }
}
