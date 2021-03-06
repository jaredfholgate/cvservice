﻿using CvService.Repositories.Contexts;
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

    public Cv Add(Cv cv)
    {
      _cvContext.Cvs.Add(cv);
      _cvContext.SaveChanges();
      return cv;
    }

    public List<Cv> Get()
    {
      return _cvContext.Cvs.ToList();
    }

    public Cv Get(int id, bool includeChildren = false)
    {
      if(includeChildren)
      {
        var cv = _cvContext.Cvs.Include(c => c.Companies).Include(c => c.Skills).Single(o => o.Id == id);
        cv.Companies = cv.Companies.OrderByDescending(o => o.Start).ToList();
        cv.Skills = cv.Skills.OrderBy(o => o.Order).ToList();
        return cv;
      }
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
