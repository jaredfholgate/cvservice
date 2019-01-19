﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CvService.Services.Models
{
  public class Skill : SkillData
  {
    public int Id { get; set; }
    public int CvId { get; set; }
    public List<Link> Links { get; set; }
  }
}
