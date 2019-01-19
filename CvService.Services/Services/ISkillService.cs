using System.Collections.Generic;
using CvService.Services.Models;

namespace CvService.Services
{
  public interface ISkillService
  {
    Skill AddToCv(SkillData skill, int cvId, string rootUrl);
    void Delete(int id);
    Skill Get(int id, string rootUrl);
    List<Skill> GetForCv(int cvId, string rootUrl);
    void Update(int id, SkillData skill);
  }
}