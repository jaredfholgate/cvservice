using CvService.Repositories.Repositories;
using CvService.Services.Models;
using System;
using AutoMapper;
using System.Collections.Generic;

namespace CvService.Services
{
  public class SkillService : ISkillService
  {
    private readonly ISkillRepository _skillRepository;
    private readonly IMapper _mapper;

    public SkillService(ISkillRepository skillRepository, IMapper mapper)
    {
      _skillRepository = skillRepository;
      _mapper = mapper;
    }

    public Skill AddToCv(Skill skill, int cvId, string rootUrl)
    {
      var skillPoco = _mapper.Map<Repositories.Pocos.Skill>(skill);
      var newSkill =_skillRepository.AddToCv(skillPoco, cvId);
      var mappedSkill = _mapper.Map<Skill>(newSkill);
      MapSkillUrls(mappedSkill, cvId, rootUrl);
      return mappedSkill;
    }

    public List<Skill> GetForCv(int cvId, string rootUrl)
    {
      var skills = _skillRepository.GetForCv(cvId);
      var mappedSkills = _mapper.Map<List<Repositories.Pocos.Skill>, List<Skill>>(skills);
      mappedSkills.ForEach(o => MapSkillUrls(o, cvId, rootUrl));
      return mappedSkills;
    }

    public Skill Get(int id, string rootUrl)
    {
      var skill = _skillRepository.Get(id);
      var mappedSkill = _mapper.Map<Skill>(skill);
      MapSkillUrls(mappedSkill, mappedSkill.CvId, rootUrl);
      return mappedSkill;
    }

    public void Delete(int id)
    {
      _skillRepository.Delete(id);
    }

    public void Update(Skill skill)
    {
      if (skill.Id == 0)
      {
        throw new Exception("The Skill must have an Id");
      }
      var mappedSkill = _mapper.Map<Repositories.Pocos.Skill>(skill);
      _skillRepository.Update(mappedSkill);
    }

    public static void MapSkillUrls(Skill skill, int cvId, string rootUrl)
    {
      var (CvUrl, SkillUrl) = GetSkillUrls(skill.Id, cvId, rootUrl);
      skill.CvUrl = CvUrl;
      skill.Url = SkillUrl;
    }

    private static (string CvUrl,  string SkillUrl ) GetSkillUrls(int id, int cvId, string rootUrl)
    {
      return (CvUrl: $"{rootUrl}/cv/{cvId}", SkillUrl: $"{rootUrl}/cv/{cvId}/skills/{id}");
    }
  }
}
