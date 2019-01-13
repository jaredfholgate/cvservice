using CvService.Repositories.Repositories;
using CvService.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CvService.Services.UnitTests
{
  [TestClass]
  public class CompanyServiceTests : BaseTest
  {
    private const string CvName = "Jared Holgate";
    private const string CvTagLine = "DevOps and Software Engineer";
    private const string CvBlurb = "Passionate DevOps and Software Engineer with 15+ years of experience, looking to deliver scalable, valuable, high quality products. Skilled in Software Engineering, Continuous Integration, Continuous Delivery, Agile and DevOps. At Maples acted as Architect and Lead Engineers for 6 teams. Implemented best practices, such as TDD, CI and CD. Mentored and trained the teams on SOLID and Design Patterns. Introduced technologies such as Elastic Search, RabbitMQ, Angular, React, Azure and .NET Core. Matured the CD pipeline with automated environment provisioning, database provisioning, SAST, DAST, SonarQube static analysis, release notes etc.";
    private const string RootUrl = "http://testurl";

    [TestMethod]
    public void AddAndRetieveCompany()
    {
      //Arrange
      var context = GetSqlLiteContext();
      var cvService = new CvService(new CvRepository(context), new Mapper().GetMapper());
      var companyService = new CompanyService(new CompanyRepository(context), new Mapper().GetMapper());
      
      var cv = new Cv() { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb };
      var cvId = cvService.Add(cv, RootUrl).Id;

      //Act
      var company = new Company() { Start = DateTime.Parse("2000-08-01"), End = DateTime.Parse("2002-05-01"), CompanyName = "Carlsberg UK", Role = "Quality Assurance Technician", Location = "Leeds" };
      var newCompany = companyService.AddToCv(company, cvId, RootUrl);

      //Assert
      var companies = companyService.GetForCv(cvId, RootUrl);
      var result = companies[0];

      Assert.AreEqual(company.Start, result.Start);
      Assert.AreEqual(company.End, result.End);
      Assert.AreEqual(company.CompanyName, result.CompanyName);
      Assert.AreEqual(company.Role, result.Role);
      Assert.AreEqual(company.Location, result.Location);
      Assert.AreEqual(company.Blurb, result.Blurb);
      Assert.AreEqual(cvId, result.CvId);
    }

    [TestMethod]
    public void UpdateCompany()
    {
      //Arrange
      var context = GetSqlLiteContext();
      var cvService = new CvService(new CvRepository(context), new Mapper().GetMapper());
      var companyService = new CompanyService(new CompanyRepository(context), new Mapper().GetMapper());

      var cv = new Cv() { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb };
      var cvId = cvService.Add(cv, RootUrl).Id;
      var company = new Company() { Start = DateTime.Parse("2000-08-01"), End = DateTime.Parse("2002-05-01"), CompanyName = "Carlsberg UK", Role = "Quality Assurance Technician", Location = "Leeds" };
      var newCompany = companyService.AddToCv(company, cvId, RootUrl);
      var companyId = newCompany.Id;

      //Act
      var companyUpdate = new Company() { Id = companyId, Start = DateTime.Parse("2000-08-02"), End = DateTime.Parse("2002-05-02"), CompanyName = "Carlsberg US", Role = "Quality Assurance Monkey", Location = "Montreal" };
      companyService.Update(companyUpdate);

      //Assert
      var result = companyService.Get(companyId, RootUrl);

      Assert.AreEqual(companyUpdate.Start, result.Start);
      Assert.AreEqual(companyUpdate.End, result.End);
      Assert.AreEqual(companyUpdate.CompanyName, result.CompanyName);
      Assert.AreEqual(companyUpdate.Role, result.Role);
      Assert.AreEqual(companyUpdate.Location, result.Location);
      Assert.AreEqual(companyUpdate.Blurb, result.Blurb);
      Assert.AreEqual(cvId, result.CvId);
    }

    [TestMethod]
    public void CheckUrlsAreCorrect()
    {
      //Arrange
      var context = GetSqlLiteContext();
      var cvService = new CvService(new CvRepository(context), new Mapper().GetMapper());
      var companyService = new CompanyService(new CompanyRepository(context), new Mapper().GetMapper());

      var cv = new Cv() { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb };
      var cvId = cvService.Add(cv, RootUrl).Id;

      //Act
      var company1 = new Company() { Start = DateTime.Parse("2000-08-01"), End = DateTime.Parse("2002-05-01"), CompanyName = "Carlsberg UK", Role = "Quality Assurance Technician", Location = "Leeds" };
      companyService.AddToCv(company1, cvId, RootUrl);
      var company2 = new Company() { Start = DateTime.Parse("2017-01-01"), End = null, CompanyName = "Maples Group", Role = "Senior Software Engineering Manager (DevOps)", Location = "Montreal", Blurb = @"• Introduced micro service and event sourcing architecture.
• Innovated with infrastructure automation,
        containers and cloud.
• Implemented application monitoring and identity management.
• Reduced cycle time to < 2 weeks for all apps.
• Reduced production bugs to < 1 per deployment across all apps.
 • Introduced skills matrix and development plans for Engineers.
 • Matured test automation and significantly reduced regression time." };
      companyService.AddToCv(company2, cvId, RootUrl);
      var company3 = new Company() { Start = DateTime.Parse("2012-11-01"), End = DateTime.Parse("2017-01-01"), CompanyName = "Maples and Calder", Role = "Software Development Manger", Location = "Cayman Islands", Blurb = @"• Introduced Continuous Integration and Continuous Delivery.
• Reduced cycle time to 4 weeks from over 6 months.
• Reduced production bugs from 30+ to <5 per deployment.
• Trained engineers in TDD, SOLID and Design Patterns.
• Introduced coding and architectural standards and pull requests.
• Introduced Git, trunk-based development and database source control." };
      companyService.AddToCv(company3, cvId, RootUrl);
      var company4 = new Company() { Start = DateTime.Parse("2003-12-01"), End = DateTime.Parse("2007-04-01"), CompanyName = "Carlsberg UK", Role = "Systems Engineer", Location = "Leeds" };
      companyService.AddToCv(company4, cvId, RootUrl);
      var company5 = new Company() { Start = DateTime.Parse("2002-05-01"), End = DateTime.Parse("2003-12-01"), CompanyName = "Carlsberg UK", Role = "Quality System Manager", Location = "Leeds" };
      companyService.AddToCv(company5, cvId, RootUrl);
      var company6 = new Company() { Start = DateTime.Parse("2007-04-01"), End = DateTime.Parse("2007-11-01"), CompanyName = "Skipton Financial Services", Role = "Business Systems Developer", Location = "Skipton" };
      companyService.AddToCv(company6, cvId, RootUrl);
      var company7 = new Company() { Start = DateTime.Parse("2007-11-01"), End = DateTime.Parse("2009-12-01"), CompanyName = "Cascade HR", Role = "Project Web Developer", Location = "Leeds" };
      companyService.AddToCv(company7, cvId, RootUrl);
      var company8 = new Company() { Start = DateTime.Parse("2010-01-01"), End = DateTime.Parse("2011-12-01"), CompanyName = "Maples and Calder", Role = "Software Developer", Location = "Leeds" };
      companyService.AddToCv(company8, cvId, RootUrl);
      var company9 = new Company() { Start = DateTime.Parse("2011-12-01"), End = DateTime.Parse("2012-11-01"), CompanyName = "Maples and Calder", Role = "eCommerce Software Team Leader", Location = "Leeds" };
      companyService.AddToCv(company9, cvId, RootUrl);

      //Assert
      var companies = companyService.GetForCv(cvId,RootUrl);
      
      foreach(var company in companies)
      {
        Assert.AreEqual($"{RootUrl}/cv/{company.CvId}", company.CvUrl);
        Assert.AreEqual($"{RootUrl}/cv/{company.CvId}/companies/{company.Id}", company.Url);
      }
    }
  }
}
