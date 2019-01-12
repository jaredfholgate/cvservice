using CvService.Repositories.Contexts;
using CvService.Repositories.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CvService.Repositories.Repositories
{
  public class CvRepository : ICvRepository
  {
    private readonly CvContext _cvContext;

    public CvRepository(CvContext cvContext)
    {
      _cvContext = cvContext;
    }

    public void Add(Cv cv)
    {
      _cvContext.Cvs.Add(cv);
      _cvContext.SaveChanges();
    }

    public List<Cv> Get()
    {
      return _cvContext.Cvs.ToList();
    }
  }
}
