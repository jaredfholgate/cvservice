using System.Collections.Generic;
using CvService.Services.Models;

namespace CvService.Services
{
  public interface ICompanyService
  {
    Company AddToCv(Company company, int cvId, string rootUrl);
    void Delete(int id);
    Company Get(int id, string rootUrl);
    List<Company> GetForCv(int cvId, string rootUrl);
    void Update(Company company);
  }
}