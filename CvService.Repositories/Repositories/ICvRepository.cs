using System.Collections.Generic;
using CvService.Repositories.Pocos;

namespace CvService.Repositories.Repositories
{
  public interface ICvRepository
  {
    void Add(Cv cv);
    List<Cv> Get();
  }
}