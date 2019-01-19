using System.Collections.Generic;
using CvService.Services.Models;

namespace CvService.Services
{
  public interface ICvService
  {
    Cv Add(CvData cv, string RootUrl);
    List<Cv> Get(string rootUrl);
    Cv Get(int id, string rootUrl);
    FullCv Get(int id, string rootUrl, bool includeChildren);
    void Delete(int id);
    void Update(int id, CvData cv);
  }
}