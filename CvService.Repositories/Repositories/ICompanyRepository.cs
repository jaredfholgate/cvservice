using System.Collections.Generic;
using CvService.Repositories.Pocos;

namespace CvService.Repositories.Repositories
{
  public interface ICompanyRepository
  {
    void AddToCv(Company company, int cvId);
    void Delete(int id);
    Company Get(int id);
    List<Company> GetForCv(int cvId);
    void Update(Company company);
  }
}