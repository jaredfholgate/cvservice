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
        cfg.CreateMap<Repositories.Pocos.Cv, Models.Cv>();
      });

      return config.CreateMapper();
    }
  }
}
