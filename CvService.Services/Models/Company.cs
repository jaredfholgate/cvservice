using System;
using System.Collections.Generic;
using System.Text;

namespace CvService.Services.Models
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
    public int CvId { get; set; }
    public string Url { get; set; }
    public string CvUrl { get; set; }
  }
}
