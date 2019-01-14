using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CvService.Api.IntnTests
{
  [TestClass]
  public class CvControllerTests
  {
    private const string CvName = "Jared Holgate";
    private const string CvTagLine = "DevOps and Software Engineer";
    private const string CvBlurb = "Passionate DevOps and Software Engineer with 15+ years of experience, looking to deliver scalable, valuable, high quality products. Skilled in Software Engineering, Continuous Integration, Continuous Delivery, Agile and DevOps. At Maples acted as Architect and Lead Engineers for 6 teams. Implemented best practices, such as TDD, CI and CD. Mentored and trained the teams on SOLID and Design Patterns. Introduced technologies such as Elastic Search, RabbitMQ, Angular, React, Azure and .NET Core. Matured the CD pipeline with automated environment provisioning, database provisioning, SAST, DAST, SonarQube static analysis, release notes etc.";

    private readonly CustomWebApplicationFactory<Startup> _factory;

    public CvControllerTests()
    {
      _factory = new CustomWebApplicationFactory<Startup>();
    }

    [TestMethod]
    public void CanAddCv()
    {
      var client = _factory.CreateClient();
      var cv = new { Name = "Test CV 1", Blurb = "Testing 1234567" };
      var postResponse  = client.PostAsync("/cv", new StringContent(JsonConvert.SerializeObject(cv), Encoding.UTF8, "application/json")).Result;
      var postResult = postResponse.Content.ReadAsStringAsync().Result;

      Assert.AreEqual("{\"id\":1,\"name\":\"Test CV 1\",\"tagLine\":null,\"blurb\":\"Testing 1234567\",\"url\":\"http://localhost/cv/1\",\"companiesUrl\":\"http://localhost/cv/1/companies\",\"skillsUrl\":\"http://localhost/cv/1/skills\"}", postResult);

      var getResponse = client.GetAsync("/cv").Result;
      var result = getResponse.Content.ReadAsStringAsync().Result;

      Assert.AreEqual("[{\"id\":1,\"name\":\"Test CV 1\",\"tagLine\":null,\"blurb\":\"Testing 1234567\",\"url\":\"http://localhost/cv/1\",\"companiesUrl\":\"http://localhost/cv/1/companies\",\"skillsUrl\":\"http://localhost/cv/1/skills\"}]", result);
    }

    [TestMethod]
    public void UpdateCv()
    {
      //Arrange
      var client = _factory.CreateClient();
      var cv = new { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb };
      var postResponse = client.PostAsync("/cv", new StringContent(JsonConvert.SerializeObject(cv), Encoding.UTF8, "application/json")).Result;
      dynamic postResult = JObject.Parse(postResponse.Content.ReadAsStringAsync().Result);
      var cvId = postResult.id;

      //Act
      var cvUpdate = new { Name = "Jared Holgate 2", TagLine = "DevOps and Software Engineer 2", Blurb = "Blah, Blah, Blah." };
      var putResponse = client.PutAsync($"/cv/{cvId}", new StringContent(JsonConvert.SerializeObject(cvUpdate), Encoding.UTF8, "application/json")).Result;

      //Assert
      var getResponse = client.GetAsync($"/cv/{cvId}").Result;
      dynamic result = JObject.Parse(getResponse.Content.ReadAsStringAsync().Result);
      Assert.AreEqual("Jared Holgate 2", result.name.ToString());
      Assert.AreEqual("DevOps and Software Engineer 2", result.tagLine.ToString());
      Assert.AreEqual("Blah, Blah, Blah.", result.blurb.ToString());
    }

    [TestMethod]
    public void DeleteCv()
    {
      //Arrange
      var client = _factory.CreateClient();
      var cv = new { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb };
      var postResponse = client.PostAsync("/cv", new StringContent(JsonConvert.SerializeObject(cv), Encoding.UTF8, "application/json")).Result;
      dynamic postResult = JObject.Parse(postResponse.Content.ReadAsStringAsync().Result);
      var cvId = postResult.id;

      //Act
      var putResponse = client.DeleteAsync($"/cv/{cvId}").Result;

      //Assert
      var getResponse = client.GetAsync($"/cv").Result;
      var result = getResponse.Content.ReadAsStringAsync().Result;
      Assert.AreEqual("[]", result);
    }

    [TestMethod]
    public void CanGetFullCv()
    {
      //Arrange
      var client = _factory.CreateClient();
      var cv = new { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb };
      var postResponse = client.PostAsync("/cv", new StringContent(JsonConvert.SerializeObject(cv), Encoding.UTF8, "application/json")).Result;
      dynamic postResult = JObject.Parse(postResponse.Content.ReadAsStringAsync().Result);
      var cvId = postResult.id;

      var companies = new List<dynamic>();
      companies.Add(new { Start = DateTime.Parse("2000-08-01"), End = DateTime.Parse("2002-05-01"), CompanyName = "Carlsberg UK", Role = "Quality Assurance Technician", Location = "Leeds" });
      companies.Add(new { Start = DateTime.Parse("2017-01-01"), End = (DateTime?)null, CompanyName = "Maples Group", Role = "Senior Software Engineering Manager (DevOps)", Location = "Montreal", Blurb = @"• Introduced micro service and event sourcing architecture.
• Innovated with infrastructure automation,
        containers and cloud.
• Implemented application monitoring and identity management.
• Reduced cycle time to < 2 weeks for all apps.
• Reduced production bugs to < 1 per deployment across all apps.
 • Introduced skills matrix and development plans for Engineers.
 • Matured test automation and significantly reduced regression time." });
      companies.Add(new { Start = DateTime.Parse("2012-11-01"), End = DateTime.Parse("2017-01-01"), CompanyName = "Maples and Calder", Role = "Software Development Manger", Location = "Cayman Islands", Blurb = @"• Introduced Continuous Integration and Continuous Delivery.
• Reduced cycle time to 4 weeks from over 6 months.
• Reduced production bugs from 30+ to <5 per deployment.
• Trained engineers in TDD, SOLID and Design Patterns.
• Introduced coding and architectural standards and pull requests.
• Introduced Git, trunk-based development and database source control." });
      
      companies.Add(new { Start = DateTime.Parse("2003-12-01"), End = DateTime.Parse("2007-04-01"), CompanyName = "Carlsberg UK", Role = "Systems Engineer", Location = "Leeds" });
      companies.Add(new { Start = DateTime.Parse("2002-05-01"), End = DateTime.Parse("2003-12-01"), CompanyName = "Carlsberg UK", Role = "Quality System Manager", Location = "Leeds" });
      companies.Add(new { Start = DateTime.Parse("2007-04-01"), End = DateTime.Parse("2007-11-01"), CompanyName = "Skipton Financial Services", Role = "Business Systems Developer", Location = "Skipton" });
      companies.Add(new { Start = DateTime.Parse("2007-11-01"), End = DateTime.Parse("2009-12-01"), CompanyName = "Cascade HR", Role = "Project Web Developer", Location = "Leeds" });
      companies.Add(new { Start = DateTime.Parse("2010-01-01"), End = DateTime.Parse("2011-12-01"), CompanyName = "Maples and Calder", Role = "Software Developer", Location = "Leeds" });
      companies.Add(new { Start = DateTime.Parse("2011-12-01"), End = DateTime.Parse("2012-11-01"), CompanyName = "Maples and Calder", Role = "eCommerce Software Team Leader", Location = "Leeds" });

      foreach(var company in companies)
      {
        var newCompany = client.PostAsync($"/cv/{cvId}/companies", new StringContent(JsonConvert.SerializeObject(company), Encoding.UTF8, "application/json")).Result;
      }

      var skills = new List<dynamic>();

      skills.Add(new { Name = "Continuous Delivery", Blurb = "Expert •••••", Order = 5 });
      skills.Add(new { Name = "DevOps", Blurb = "Expert •••••", Order = 3 });
      skills.Add(new { Name = "Agile", Blurb = "Experienced ••••", Order = 8 });
      skills.Add(new { Name = "Software Engineering", Blurb = "Expert •••••", Order = 1 });
      skills.Add(new { Name = "People Development", Blurb = "Expert •••••", Order = 2 });
      skills.Add(new { Name = "Continuous Integration", Blurb = "Expert •••••", Order = 4 });
      skills.Add(new { Name = "Infrastructure Automation", Blurb = "Good ••••", Order = 6 });
      skills.Add(new { Name = "Containers and Orchestration", Blurb = "Good ••••", Order = 7 });
      skills.Add(new { Name = "Problem Solving", Blurb = "Expert •••••", Order = 9 });
      skills.Add(new { Name = "Scalable Architecture", Blurb = "Experienced ••••", Order = 10 });
      skills.Add(new { Name = "Quality", Blurb = "Expert •••••", Order = 11 });
      skills.Add(new { Name = "Security", Blurb = "Experienced ••••", Order = 12 });
      skills.Add(new { Name = "TDD, Solid and Design Patterns", Blurb = "Expert •••••", Order = 13 });
      skills.Add(new { Name = "Microsoft .NET Stack, TFS, Azure DevOps and Octopus Deploy", Blurb = "Expert •••••", Order = 14 });
      skills.Add(new { Name = "Java, Ruby, Go, NodeJS, React, Angular, Jenkins", Blurb = "Learning ••••", Order = 15 });
      skills.Add(new { Name = "Microsoft Azure", Blurb = "Experienced ••••", Order = 16 });
      skills.Add(new { Name = "AWS and Google Cloud", Blurb = "Learning •••", Order = 17 });

      foreach(var skill in skills)
      {
        var newSkill = client.PostAsync($"/cv/{cvId}/skills", new StringContent(JsonConvert.SerializeObject(skill), Encoding.UTF8, "application/json")).Result;
      }

      //Assert
      var getResponse = client.GetAsync($"/cv/{cvId}/full").Result;
      var getResult = getResponse.Content.ReadAsStringAsync().Result;
      dynamic result = JObject.Parse(getResponse.Content.ReadAsStringAsync().Result);

      Assert.AreEqual(CvName, result.name.ToString());
      Assert.AreEqual(CvTagLine, result.tagLine.ToString());
      Assert.AreEqual(CvBlurb, result.blurb.ToString());
      Assert.AreEqual(9, result.companies.Count);
      Assert.AreEqual(17, result.skills.Count);

      var date = DateTime.Parse("3000-01-01");
      foreach (var company in result.companies)
      {
        Assert.IsTrue((DateTime)company.start < date);
        date = (DateTime)company.start;
        Assert.IsNotNull(company.cvUrl);
        Assert.IsNotNull(company.url);
      }

      var order = 0;
      foreach (var skill in result.skills)
      {
        Assert.IsTrue((int)skill.order > order);
        order = (int)skill.order;
        Assert.IsNotNull(skill.cvUrl);
        Assert.IsNotNull(skill.url);
      }
    }
       
    //[TestMethod] //This method is used as a one off to populate some example data.
    public void GenerateData()
    {
      //Arrange
      var client = new HttpClient
      {
        BaseAddress = new Uri("https://cvservicetest.azurewebsites.net")
      };
      var cv = new { Name = CvName, TagLine = CvTagLine, Blurb = CvBlurb };
      var postResponse = client.PostAsync("/cv", new StringContent(JsonConvert.SerializeObject(cv), Encoding.UTF8, "application/json")).Result;
      dynamic postResult = JObject.Parse(postResponse.Content.ReadAsStringAsync().Result);
      var cvId = postResult.id;

      var companies = new List<dynamic>();
      companies.Add(new { Start = DateTime.Parse("2000-08-01"), End = DateTime.Parse("2002-05-01"), CompanyName = "Carlsberg UK", Role = "Quality Assurance Technician", Location = "Leeds" });
      companies.Add(new { Start = DateTime.Parse("2017-01-01"), End = (DateTime?)null, CompanyName = "Maples Group", Role = "Senior Software Engineering Manager (DevOps)", Location = "Montreal", Blurb = @"• Introduced micro service and event sourcing architecture.
• Innovated with infrastructure automation,
        containers and cloud.
• Implemented application monitoring and identity management.
• Reduced cycle time to < 2 weeks for all apps.
• Reduced production bugs to < 1 per deployment across all apps.
 • Introduced skills matrix and development plans for Engineers.
 • Matured test automation and significantly reduced regression time." });
      companies.Add(new { Start = DateTime.Parse("2012-11-01"), End = DateTime.Parse("2017-01-01"), CompanyName = "Maples and Calder", Role = "Software Development Manger", Location = "Cayman Islands", Blurb = @"• Introduced Continuous Integration and Continuous Delivery.
• Reduced cycle time to 4 weeks from over 6 months.
• Reduced production bugs from 30+ to <5 per deployment.
• Trained engineers in TDD, SOLID and Design Patterns.
• Introduced coding and architectural standards and pull requests.
• Introduced Git, trunk-based development and database source control." });

      companies.Add(new { Start = DateTime.Parse("2003-12-01"), End = DateTime.Parse("2007-04-01"), CompanyName = "Carlsberg UK", Role = "Systems Engineer", Location = "Leeds" });
      companies.Add(new { Start = DateTime.Parse("2002-05-01"), End = DateTime.Parse("2003-12-01"), CompanyName = "Carlsberg UK", Role = "Quality System Manager", Location = "Leeds" });
      companies.Add(new { Start = DateTime.Parse("2007-04-01"), End = DateTime.Parse("2007-11-01"), CompanyName = "Skipton Financial Services", Role = "Business Systems Developer", Location = "Skipton" });
      companies.Add(new { Start = DateTime.Parse("2007-11-01"), End = DateTime.Parse("2009-12-01"), CompanyName = "Cascade HR", Role = "Project Web Developer", Location = "Leeds" });
      companies.Add(new { Start = DateTime.Parse("2010-01-01"), End = DateTime.Parse("2011-12-01"), CompanyName = "Maples and Calder", Role = "Software Developer", Location = "Leeds" });
      companies.Add(new { Start = DateTime.Parse("2011-12-01"), End = DateTime.Parse("2012-11-01"), CompanyName = "Maples and Calder", Role = "eCommerce Software Team Leader", Location = "Leeds" });

      foreach (var company in companies)
      {
        var newCompany = client.PostAsync($"/cv/{cvId}/companies", new StringContent(JsonConvert.SerializeObject(company), Encoding.UTF8, "application/json")).Result;
      }

      var skills = new List<dynamic>();

      skills.Add(new { Name = "Continuous Delivery", Blurb = "Expert •••••", Order = 5 });
      skills.Add(new { Name = "DevOps", Blurb = "Expert •••••", Order = 3 });
      skills.Add(new { Name = "Agile", Blurb = "Experienced ••••", Order = 8 });
      skills.Add(new { Name = "Software Engineering", Blurb = "Expert •••••", Order = 1 });
      skills.Add(new { Name = "People Development", Blurb = "Expert •••••", Order = 2 });
      skills.Add(new { Name = "Continuous Integration", Blurb = "Expert •••••", Order = 4 });
      skills.Add(new { Name = "Infrastructure Automation", Blurb = "Good ••••", Order = 6 });
      skills.Add(new { Name = "Containers and Orchestration", Blurb = "Good ••••", Order = 7 });
      skills.Add(new { Name = "Problem Solving", Blurb = "Expert •••••", Order = 9 });
      skills.Add(new { Name = "Scalable Architecture", Blurb = "Experienced ••••", Order = 10 });
      skills.Add(new { Name = "Quality", Blurb = "Expert •••••", Order = 11 });
      skills.Add(new { Name = "Security", Blurb = "Experienced ••••", Order = 12 });
      skills.Add(new { Name = "TDD, Solid and Design Patterns", Blurb = "Expert •••••", Order = 13 });
      skills.Add(new { Name = "Microsoft .NET Stack, TFS, Azure DevOps and Octopus Deploy", Blurb = "Expert •••••", Order = 14 });
      skills.Add(new { Name = "Java, Ruby, Go, NodeJS, React, Angular, Jenkins", Blurb = "Learning ••••", Order = 15 });
      skills.Add(new { Name = "Microsoft Azure", Blurb = "Experienced ••••", Order = 16 });
      skills.Add(new { Name = "AWS and Google Cloud", Blurb = "Learning •••", Order = 17 });

      foreach (var skill in skills)
      {
        var newSkill = client.PostAsync($"/cv/{cvId}/skills", new StringContent(JsonConvert.SerializeObject(skill), Encoding.UTF8, "application/json")).Result;
      }

      //Assert
      var getResponse = client.GetAsync($"/cv/{cvId}/full").Result;
      var getResult = getResponse.Content.ReadAsStringAsync().Result;
      dynamic result = JObject.Parse(getResponse.Content.ReadAsStringAsync().Result);

      Assert.AreEqual(CvName, result.name.ToString());
      Assert.AreEqual(CvTagLine, result.tagLine.ToString());
      Assert.AreEqual(CvBlurb, result.blurb.ToString());
      Assert.AreEqual(9, result.companies.Count);
      Assert.AreEqual(17, result.skills.Count);
    }
  }
}
