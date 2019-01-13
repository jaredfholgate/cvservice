using System.Collections.Generic;
using CvService.Services.Models;

namespace CvService.Services
{
  public interface ICvService
  {
    Cv Add(Cv cv, string RootUrl);
    List<Cv> Get(string rootUrl);
    Cv Get(int id, string rootUrl);
    void Delete(int id);
    void Update(Cv cv);
  }
}