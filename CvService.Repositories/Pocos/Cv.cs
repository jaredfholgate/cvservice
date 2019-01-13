using System;
using System.Collections.Generic;
using System.Text;

namespace CvService.Repositories.Pocos
{
  public class Cv
  { 
    public int Id { get; set; }
    public string Name { get; set; }
    public string TagLine { get; set; }
    public string Blurb { get; set; }
    public List<Company> Companies { get; set; }
    public List<Skill> Skills { get; set; }
  }
}
