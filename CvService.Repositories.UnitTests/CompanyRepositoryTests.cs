using CvService.Repositories.Contexts;
using CvService.Repositories.Pocos;
using CvService.Repositories.Repositories;
using CvService.Tests.Shared;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CvService.Repositories.UnitTests
{
  [TestClass]
  public class CompanyRepositoryTests : BaseTest
  {
    [TestMethod]
    public void CanPersistACompany()
    {
      //Arrange
      var context = GetSqlLiteContext();
      var cvRepository = new CvRepository(context);
      var companyRepository = new CompanyRepository(context);

      var cv = new Cv() { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };
      cvRepository.Add(cv);
      var cvId = cvRepository.Get()[0].Id;
      
      //Act
      var company = new Company() { Start = DateTime.Parse("2000-08-01"), End = DateTime.Parse("2002-05-01"), CompanyName = "Carlsberg UK", Role = "Quality Assurance Technician", Location = "Leeds" };
      companyRepository.AddToCv(company, cvId);

      //Assert
      var result = companyRepository.GetForCv(cvId)[0];

      Assert.AreEqual(company.Start, result.Start);
      Assert.AreEqual(company.End, result.End);
      Assert.AreEqual(company.CompanyName, result.CompanyName);
      Assert.AreEqual(company.Role, result.Role);
      Assert.AreEqual(company.Location, result.Location);
      Assert.AreEqual(company.Blurb, result.Blurb);
      Assert.AreEqual(cvId, result.Cv.Id);
    }

    [TestMethod]
    public void CanUpdateCompany()
    {
      //Arrange
      var context = GetSqlLiteContext();
      var cvRepository = new CvRepository(context);
      var companyRepository = new CompanyRepository(context);

      var cv = new Cv() { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb }; 
      cvRepository.Add(cv);
      var cvId = cvRepository.Get()[0].Id;
      var company = new Company() { Start = DateTime.Parse("2000-08-01"), End = DateTime.Parse("2002-05-01"), CompanyName = "Carlsberg UK", Role = "Quality Assurance Technician", Location = "Leeds" };
      companyRepository.AddToCv(company, cvId);
      var companyId = companyRepository.GetForCv(cvId)[0].Id;

      //Act
      var companyUpdate = new Company() { Id = companyId, Start = DateTime.Parse("2000-08-02"), End = DateTime.Parse("2002-05-02"), CompanyName = "Carlsberg US", Role = "Quality Assurance Monkey", Location = "Montreal" };
      companyRepository.Update(companyUpdate);

      //Assert
      var result = companyRepository.Get(companyId);

      Assert.AreEqual(companyUpdate.Start, result.Start);
      Assert.AreEqual(companyUpdate.End, result.End);
      Assert.AreEqual(companyUpdate.CompanyName, result.CompanyName);
      Assert.AreEqual(companyUpdate.Role, result.Role);
      Assert.AreEqual(companyUpdate.Location, result.Location);
      Assert.AreEqual(companyUpdate.Blurb, result.Blurb);
      Assert.AreEqual(cvId, result.Cv.Id);
    }

    [TestMethod]
    public void CanDeleteCompany()
    {
      //Arrange
      var context = GetSqlLiteContext();
      var cvRepository = new CvRepository(context);
      var companyRepository = new CompanyRepository(context);

      var cv = new Cv() { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };
      cvRepository.Add(cv);
      var cvId = cvRepository.Get()[0].Id;
      var company = new Company() { Start = DateTime.Parse("2000-08-01"), End = DateTime.Parse("2002-05-01"), CompanyName = "Carlsberg UK", Role = "Quality Assurance Technician", Location = "Leeds" };
      companyRepository.AddToCv(company, cvId);
      var companyId = companyRepository.GetForCv(cvId)[0].Id;

      //Act
      companyRepository.Delete(companyId);

      //Assert
      Assert.AreEqual(0, companyRepository.GetForCv(cvId).Count);
    }


    [TestMethod]
    public void MultipleCompaniesAreReturnedInDateOrder()
    {
      //Arrange
      var context = GetSqlLiteContext();
      var cvRepository = new CvRepository(context);
      var companyRepository = new CompanyRepository(context);

      var cv = new Cv() { Name = Constants.CvName, TagLine = Constants.CvTagLine, Blurb = Constants.CvBlurb };
      cvRepository.Add(cv);
      var cvId = cvRepository.Get()[0].Id;

      //Act
      var company1 = new Company() { Start = DateTime.Parse("2000-08-01"), End = DateTime.Parse("2002-05-01"), CompanyName = "Carlsberg UK", Role = "Quality Assurance Technician", Location = "Leeds" };
      companyRepository.AddToCv(company1, cvId);
      var company2 = new Company() { Start = DateTime.Parse("2017-01-01"), End = null, CompanyName = "Maples Group", Role = "Senior Software Engineering Manager (DevOps)", Location = "Montreal", Blurb = @"• Introduced micro service and event sourcing architecture.
• Innovated with infrastructure automation,
        containers and cloud.
• Implemented application monitoring and identity management.
• Reduced cycle time to < 2 weeks for all apps.
• Reduced production bugs to < 1 per deployment across all apps.
 • Introduced skills matrix and development plans for Engineers.
 • Matured test automation and significantly reduced regression time." };
      companyRepository.AddToCv(company2, cvId);
      var company3 = new Company() { Start = DateTime.Parse("2012-11-01"), End = DateTime.Parse("2017-01-01"), CompanyName = "Maples and Calder", Role = "Software Development Manger", Location = "Cayman Islands", Blurb = @"• Introduced Continuous Integration and Continuous Delivery.
• Reduced cycle time to 4 weeks from over 6 months.
• Reduced production bugs from 30+ to <5 per deployment.
• Trained engineers in TDD, SOLID and Design Patterns.
• Introduced coding and architectural standards and pull requests.
• Introduced Git, trunk-based development and database source control." };
      companyRepository.AddToCv(company3, cvId);
      var company4 = new Company() { Start = DateTime.Parse("2003-12-01"), End = DateTime.Parse("2007-04-01"), CompanyName = "Carlsberg UK", Role = "Systems Engineer", Location = "Leeds" };
      companyRepository.AddToCv(company4, cvId);
      var company5 = new Company() { Start = DateTime.Parse("2002-05-01"), End = DateTime.Parse("2003-12-01"), CompanyName = "Carlsberg UK", Role = "Quality System Manager", Location = "Leeds" };
      companyRepository.AddToCv(company5, cvId);
      var company6 = new Company() { Start = DateTime.Parse("2007-04-01"), End = DateTime.Parse("2007-11-01"), CompanyName = "Skipton Financial Services", Role = "Business Systems Developer", Location = "Skipton" };
      companyRepository.AddToCv(company6, cvId);
      var company7 = new Company() { Start = DateTime.Parse("2007-11-01"), End = DateTime.Parse("2009-12-01"), CompanyName = "Cascade HR", Role = "Project Web Developer", Location = "Leeds" };
      companyRepository.AddToCv(company7, cvId);
      var company8 = new Company() { Start = DateTime.Parse("2010-01-01"), End = DateTime.Parse("2011-12-01"), CompanyName = "Maples and Calder", Role = "Software Developer", Location = "Leeds" };
      companyRepository.AddToCv(company8, cvId);
      var company9 = new Company() { Start = DateTime.Parse("2011-12-01"), End = DateTime.Parse("2012-11-01"), CompanyName = "Maples and Calder", Role = "eCommerce Software Team Leader", Location = "Leeds" };
      companyRepository.AddToCv(company9, cvId);

      //Assert
      var results = companyRepository.GetForCv(cvId);

      var startDate = DateTime.Parse("3000-01-01");
      foreach(var result in results)
      {
        Assert.IsTrue(result.Start < startDate);
        startDate = result.Start;
      }
    }
  }
}
