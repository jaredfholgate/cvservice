using CvService.Repositories.Repositories;
using CvService.Services.Models;
using System;
using AutoMapper;
using System.Collections.Generic;

namespace CvService.Services
{
  public class CompanyService : ICompanyService
  {
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
    {
      _companyRepository = companyRepository;
      _mapper = mapper;
    }

    public Company AddToCv(Company company, int cvId, string rootUrl)
    {
      var companyPoco = _mapper.Map<Repositories.Pocos.Company>(company);
      var newCompany =_companyRepository.AddToCv(companyPoco, cvId);
      var mappedCompany = _mapper.Map<Company>(newCompany);
      MapCompanyUrls(mappedCompany, cvId, rootUrl);
      return mappedCompany;
    }

    public List<Company> GetForCv(int cvId, string rootUrl)
    {
      var companies = _companyRepository.GetForCv(cvId);
      var mappedCompanies = _mapper.Map<List<Repositories.Pocos.Company>, List<Company>>(companies);
      mappedCompanies.ForEach(o => MapCompanyUrls(o, cvId, rootUrl));
      return mappedCompanies;
    }

    public Company Get(int id, string rootUrl)
    {
      var company = _companyRepository.Get(id);
      var mappedCompany = _mapper.Map<Company>(company);
      MapCompanyUrls(mappedCompany, mappedCompany.CvId, rootUrl);
      return mappedCompany;
    }

    public void Delete(int id)
    {
      _companyRepository.Delete(id);
    }

    public void Update(Company company)
    {
      if (company.Id == 0)
      {
        throw new Exception("The Company must have an Id");
      }
      var mappedCV = _mapper.Map<Repositories.Pocos.Company>(company);
      _companyRepository.Update(mappedCV);
    }

    public static void MapCompanyUrls(Company company, int cvId, string rootUrl)
    {
      var (CvUrl, CompanyUrl) = GetCompanyUrls(company.Id, cvId, rootUrl);
      company.CvUrl = CvUrl;
      company.Url = CompanyUrl;
    }

    private static (string CvUrl,  string CompanyUrl ) GetCompanyUrls(int id, int cvId, string rootUrl)
    {
      return (CvUrl: $"{rootUrl}/cv/{cvId}", CompanyUrl: $"{rootUrl}/cv/{cvId}/companies/{id}");
    }
  }
}
