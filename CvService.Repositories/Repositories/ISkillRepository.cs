using System.Collections.Generic;
using CvService.Repositories.Pocos;

namespace CvService.Repositories.Repositories
{
  public interface ISkillRepository
  {
    void AddToCv(Skill skill, int cvId);
    void Delete(int id);
    Skill Get(int id);
    List<Skill> GetForCv(int cvId);
    void Update(Skill skill);
  }
}