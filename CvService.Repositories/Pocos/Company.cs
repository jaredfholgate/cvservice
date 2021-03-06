﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CvService.Repositories.Pocos
{
  public class Company
  {
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime? End { get; set; }
    public string CompanyName { get; set; }
    public string Role { get; set; }
    public string Location { get; set; }
    public string Blurb { get; set; }
    public Cv Cv { get; set; }
  }
}
