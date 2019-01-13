using CvService.Repositories.Contexts;
using CvService.Repositories.Pocos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CvService.Repositories.Repositories
{
  public class CompanyRepository : ICompanyRepository
  {
    private readonly CvContext _cvContext;

    public CompanyRepository(CvContext cvContext)
    {
      _cvContext = cvContext;
    }

    public Company AddToCv(Company company, int cvId)
    {
      var cv = _cvContext.Cvs.Single(o => o.Id == cvId);
      company.Cv = cv;
      _cvContext.Companies.Add(company);
      _cvContext.SaveChanges();
      return company;
    }

    public List<Company> GetForCv(int cvId)
    {
      return _cvContext.Companies.Where(o => o.Cv.Id == cvId).OrderByDescending(o => o.Start).ToList();
    }

    public Company Get(int id)
    {
      return _cvContext.Companies.Single(o => o.Id == id);
    }

    public void Update(Company company)
    {
      var attachedCompany = _cvContext.Companies.Single(o => o.Id == company.Id);
      attachedCompany.Start = company.Start;
      attachedCompany.End = company.End;
      attachedCompany.CompanyName = company.CompanyName;
      attachedCompany.Role = company.Role;
      attachedCompany.Location = company.Location;
      attachedCompany.Blurb = company.Blurb;
      _cvContext.SaveChanges();
    }

    public void Delete(int id)
    {
      var company = _cvContext.Companies.Single(o => o.Id == id);
      _cvContext.Companies.Remove(company);
      _cvContext.SaveChanges();
    }
  }
}
