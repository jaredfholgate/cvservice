using CvService.Repositories.Contexts;
using CvService.Repositories.Pocos;
using Microsoft.EntityFrameworkCore;
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

    public Cv Get(int id)
    {
      return _cvContext.Cvs.Single(o => o.Id == id);
    }

    public void Update(Cv cv)
    {
      var attachedCv = _cvContext.Cvs.Single(o => o.Id == cv.Id);
      attachedCv.Name = cv.Name;
      attachedCv.TagLine = cv.TagLine;
      attachedCv.Blurb = cv.Blurb;
      _cvContext.SaveChanges();
    }

    public void Delete(int id)
    {
      var cv = _cvContext.Cvs.Single(o => o.Id == id);
      _cvContext.Cvs.Remove(cv);
      _cvContext.SaveChanges();
    }
  }
}
