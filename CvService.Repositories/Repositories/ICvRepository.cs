using System.Collections.Generic;
using CvService.Repositories.Pocos;

namespace CvService.Repositories.Repositories
{
  public interface ICvRepository
  {
    Cv Add(Cv cv);
    List<Cv> Get();
    Cv Get(int id, bool includeChildren = false);
    void Update(Cv cv);
    void Delete(int id);
  }
}