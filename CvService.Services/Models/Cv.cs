using System;
using System.Collections.Generic;
using System.Text;

namespace CvService.Services.Models
{
  public class Cv
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string TagLine { get; set; }
    public string Blurb { get; set; }
    public List<Link> Links { get; set; }
  }
}
