using CvService.Repositories.Repositories;
using CvService.Services.Models;
using System;
using AutoMapper;
using System.Collections.Generic;

namespace CvService.Services
{
  public class CvService : ICvService
  {
    private readonly ICvRepository _cvRepository;
    private readonly IMapper _mapper;

    public CvService(ICvRepository cvRepository, IMapper mapper)
    {
      _cvRepository = cvRepository;
      _mapper = mapper;
    }

    public void Add(Cv cv)
    {
      var cvPoco = _mapper.Map<Repositories.Pocos.Cv>(cv);
      _cvRepository.Add(cvPoco);
    }

    public List<Cv> Get(string rootUrl)
    {
      var cvs = _cvRepository.Get();
      var mappedCvs = _mapper.Map<List<Repositories.Pocos.Cv>, List<Cv>>(cvs);
      mappedCvs.ForEach(o => o.Url = $"{rootUrl}/cv/{o.Id}");
      return mappedCvs;
    }
  }
}
