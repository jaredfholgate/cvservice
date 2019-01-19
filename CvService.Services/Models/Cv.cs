using System;
using System.Collections.Generic;
using System.Text;

namespace CvService.Services.Models
{
  public class Cv : CvData
  {
    public int Id { get; set; }
    public List<Link> Links { get; set; }
  }
}
