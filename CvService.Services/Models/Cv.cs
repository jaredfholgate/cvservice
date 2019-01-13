﻿using System;
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
    public string Url { get; set; }
    public string CompaniesUrl { get; set; }
    public string SkillsUrl { get; set; }
  }
}