using System.Collections.Generic;
using CvService.Services.Models;

namespace CvService.Services
{
  public interface ICvService
  {
    void Add(Cv cv);
    List<Cv> Get(string rootUrl);
  }
}