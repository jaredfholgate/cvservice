using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace CvService.Services
{
  public class Mapper
  {
    public IMapper GetMapper()
    {
      var config = new MapperConfiguration(cfg => {
        cfg.CreateMap<Models.Cv, Repositories.Pocos.Cv>();
        cfg.CreateMap<Models.CvData, Repositories.Pocos.Cv>();
        cfg.CreateMap<Repositories.Pocos.Cv, Models.Cv>();
        cfg.CreateMap<Repositories.Pocos.Cv, Models.FullCv>();
        cfg.CreateMap<Models.Company, Repositories.Pocos.Company>();
        cfg.CreateMap<Models.CompanyData, Repositories.Pocos.Company>();
        cfg.CreateMap<Repositories.Pocos.Company, Models.Company>().ForMember(o => o.CvId, o => o.MapFrom(oo => oo.Cv.Id));
        cfg.CreateMap<Models.Skill, Repositories.Pocos.Skill>();
        cfg.CreateMap<Models.SkillData, Repositories.Pocos.Skill>();
        cfg.CreateMap<Repositories.Pocos.Skill, Models.Skill>().ForMember(o => o.CvId, o => o.MapFrom(oo => oo.Cv.Id));
      });

      return config.CreateMapper();
    }
  }
}
