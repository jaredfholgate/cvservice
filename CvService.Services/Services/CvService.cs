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

    public Cv Add(CvData cv, string rootUrl)
    {
      var cvPoco = _mapper.Map<Repositories.Pocos.Cv>(cv);
      var newCv =_cvRepository.Add(cvPoco);
      var mappedCv = _mapper.Map<Cv>(newCv);
      MapCvUrls(mappedCv, rootUrl);
      return mappedCv;
    }

    public List<Cv> Get(string rootUrl)
    {
      var cvs = _cvRepository.Get();
      var mappedCvs = _mapper.Map<List<Repositories.Pocos.Cv>, List<Cv>>(cvs);
      mappedCvs.ForEach(o => MapCvUrls(o, rootUrl));
      return mappedCvs;
    }

    public Cv Get(int id, string rootUrl)
    {
      var cv = _cvRepository.Get(id);
      var mappedCv = _mapper.Map<Cv>(cv);
      MapCvUrls(mappedCv, rootUrl);
      return mappedCv;
    }

    public FullCv Get(int id, string rootUrl, bool includeChildren)
    {
      var cv = _cvRepository.Get(id, includeChildren);
      var mappedCv = _mapper.Map<FullCv>(cv);
      MapCvUrls(mappedCv, rootUrl);

      foreach(var company in mappedCv.Companies)
      {
        CompanyService.MapCompanyUrls(company, id, rootUrl);
      }
      foreach (var skill in mappedCv.Skills)
      {
        SkillService.MapSkillUrls(skill, id, rootUrl);
      }
      return mappedCv;
    }

    public void Delete(int id)
    {
      _cvRepository.Delete(id);
    }

    public void Update(int id, CvData cv)
    {
      var mappedCV = _mapper.Map<Repositories.Pocos.Cv>(cv);
      mappedCV.Id = id;
      _cvRepository.Update(mappedCV);
    }

    private void MapCvUrls(Cv cv, string rootUrl)
    {
      var (CvUrl, CompaniesUrl, SkillsUrl) = GetCvUrls(cv.Id, rootUrl);
      cv.Links = new List<Link>()
      {
        new Link { Rel = "self", Href = CvUrl },
        new Link { Rel = "companies", Href = CompaniesUrl },
        new Link { Rel = "skills", Href = SkillsUrl }
      };
    }

    private (string CvUrl,  string CompaniesUrl, string SkillsUrl ) GetCvUrls(int id, string rootUrl)
    {
      return (CvUrl: $"{rootUrl}/cv/{id}", CompaniesUrl: $"{rootUrl}/cv/{id}/companies", SkillsUrl: $"{rootUrl}/cv/{id}/skills");
    }
  }
}
