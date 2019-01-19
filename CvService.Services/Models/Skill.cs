﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CvService.Services.Models
{
  public class Skill
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Blurb { get; set; }
    public int Order { get; set; }
    public int CvId { get; set; }
    public List<Link> Links { get; set; }
  }
}
