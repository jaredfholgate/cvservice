using CvService.Repositories.Contexts;
using CvService.Repositories.Pocos;
using CvService.Repositories.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CvService.Repositories.UnitTests
{
  [TestClass]
  public class CompanyRepositoryTests : BaseTest
  {
    private const string CvName = "Jared Holgate";
    private const string CvTagLine = "DevOps and Software Engineer";
    private const string CvBlurb = "Passionate DevOps and Software Engineer with 15+ years of experience, looking to deliver scalable, valuable, high quality products. Skilled in Software Engineering, Continuous Integration, Continuous Delivery, Agile and DevOps. At Maples acted as Architect and Lead Engineers for 6 teams. Implemented best practices, such as TDD, CI and CD. Mentored and trained the teams on SOLID and Design Patterns. Introduced technologies such as Elastic Search, RabbitMQ, Angular, React, Azure and .NET Core. Matured the CD pipeline with automated environment provisioning, database provisioning, SAST, DAST, SonarQube static analysis, release notes etc.";

    [TestMethod]
    public void CanPersistACompany()
    {
      //Arrange
      var context = GetSqlLiteContext();
      var cvRepository = new CvRepository(context);
      var companyRepository = new CompanyRepository(context);

      var cv = new Cv() { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb };
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

      var cv = new Cv() { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb }; 
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

      var cv = new Cv() { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb };
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
  }
}
