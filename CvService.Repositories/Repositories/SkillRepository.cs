using CvService.Repositories.Contexts;
using CvService.Repositories.Pocos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CvService.Repositories.Repositories
{
  public class SkillRepository : ISkillRepository
  {
    private readonly CvContext _cvContext;

    public SkillRepository(CvContext cvContext)
    {
      _cvContext = cvContext;
    }

    public Skill AddToCv(Skill skill, int cvId)
    {
      var cv = _cvContext.Cvs.Single(o => o.Id == cvId);
      skill.Cv = cv;
      _cvContext.Skills.Add(skill);
      _cvContext.SaveChanges();
      return skill;
    }

    public List<Skill> GetForCv(int cvId)
    {
      return _cvContext.Skills.Where(o => o.Cv.Id == cvId).OrderBy(o => o.Order).ToList();
    }

    public Skill Get(int id)
    {
      return _cvContext.Skills.Single(o => o.Id == id);
    }

    public void Update(Skill skill)
    {
      var attachedSkill = _cvContext.Skills.Single(o => o.Id == skill.Id);
      attachedSkill.Name = skill.Name;
      attachedSkill.Blurb = skill.Blurb;
      attachedSkill.Order = skill.Order;
      _cvContext.SaveChanges();
    }

    public void Delete(int id)
    {
      var skill = _cvContext.Skills.Single(o => o.Id == id);
      _cvContext.Skills.Remove(skill);
      _cvContext.SaveChanges();
    }
  }
}
