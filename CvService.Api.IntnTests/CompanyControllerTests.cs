using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;

namespace CvService.Api.IntnTests
{
  [TestClass]
  public class CompanyControllerTests
  {
    private const string CvName = "Jared Holgate";
    private const string CvTagLine = "DevOps and Software Engineer";
    private const string CvBlurb = "Passionate DevOps and Software Engineer with 15+ years of experience, looking to deliver scalable, valuable, high quality products. Skilled in Software Engineering, Continuous Integration, Continuous Delivery, Agile and DevOps. At Maples acted as Architect and Lead Engineers for 6 teams. Implemented best practices, such as TDD, CI and CD. Mentored and trained the teams on SOLID and Design Patterns. Introduced technologies such as Elastic Search, RabbitMQ, Angular, React, Azure and .NET Core. Matured the CD pipeline with automated environment provisioning, database provisioning, SAST, DAST, SonarQube static analysis, release notes etc.";

    private readonly CustomWebApplicationFactory<Startup> _factory;

    public CompanyControllerTests()
    {
      _factory = new CustomWebApplicationFactory<Startup>();
    }

    [TestMethod]
    public void CanAddCompany()
    {
      //Arrange
      var client = _factory.CreateClient();
      var cv = new { Name = "Test CV 1", Blurb = "Testing 1234567" };
      var postResponse  = client.PostAsync("/cv", new StringContent(JsonConvert.SerializeObject(cv), Encoding.UTF8, "application/json")).Result;
      dynamic postResult = JObject.Parse(postResponse.Content.ReadAsStringAsync().Result);
      var cvId = postResult.id;

      //Act
      var company = new { Start = DateTime.Parse("2000-08-01"), End = DateTime.Parse("2002-05-01"), CompanyName = "Carlsberg UK", Role = "Quality Assurance Technician", Location = "Leeds", Blurb = "Blah, Blah, Blah" };
      var newCompany = client.PostAsync($"/cv/{cvId}/companies", new StringContent(JsonConvert.SerializeObject(company), Encoding.UTF8, "application/json")).Result;

      //Assert
      var getResponse = client.GetAsync($"/cv/{cvId}/companies").Result;
      var getResult = getResponse.Content.ReadAsStringAsync().Result;
      dynamic companies = JArray.Parse(getResponse.Content.ReadAsStringAsync().Result);
      var result = companies[0];

      Assert.AreEqual(company.Start, (DateTime)result.start);
      Assert.AreEqual(company.End, (DateTime?)result.end);
      Assert.AreEqual(company.CompanyName, (string)result.companyName);
      Assert.AreEqual(company.Role, (string)result.role);
      Assert.AreEqual(company.Location, (string)result.location);
      Assert.AreEqual(company.Blurb, (string)result.blurb);
      Assert.AreEqual((int)cvId, (int)result.cvId);
    }

    [TestMethod]
    public void UpdateCompany()
    {
      //Arrange
      var client = _factory.CreateClient();
      var cv = new { Name = "Test CV 1", Blurb = "Testing 1234567" };
      var postResponse = client.PostAsync("/cv", new StringContent(JsonConvert.SerializeObject(cv), Encoding.UTF8, "application/json")).Result;
      dynamic postResult = JObject.Parse(postResponse.Content.ReadAsStringAsync().Result);
      var cvId = postResult.id;
      var company = new { Start = DateTime.Parse("2000-08-01"), End = DateTime.Parse("2002-05-01"), CompanyName = "Carlsberg UK", Role = "Quality Assurance Technician", Location = "Leeds", Blurb = "Blah, Blah, Blah" };
      var postCompanyResponse = client.PostAsync($"/cv/{cvId}/companies", new StringContent(JsonConvert.SerializeObject(company), Encoding.UTF8, "application/json")).Result;
      dynamic newCompany = JObject.Parse(postCompanyResponse.Content.ReadAsStringAsync().Result);
      var companyId = newCompany.id;

      //Act
      var companyUpdate = new { Start = DateTime.Parse("2000-08-02"), End = DateTime.Parse("2002-05-02"), CompanyName = "Carlsberg US", Role = "Quality Assurance Monkey", Location = "Montreal", Blurb = "Nah, Nah, Nah" };
      var putResponse = client.PutAsync($"/company/{companyId}", new StringContent(JsonConvert.SerializeObject(companyUpdate), Encoding.UTF8, "application/json")).Result;

      //Assert
      var getResponse = client.GetAsync($"/company/{companyId}").Result;
      dynamic result = JObject.Parse(getResponse.Content.ReadAsStringAsync().Result);

      Assert.AreEqual(companyUpdate.Start, (DateTime)result.start);
      Assert.AreEqual(companyUpdate.End, (DateTime?)result.end);
      Assert.AreEqual(companyUpdate.CompanyName, (string)result.companyName);
      Assert.AreEqual(companyUpdate.Role, (string)result.role);
      Assert.AreEqual(companyUpdate.Location, (string)result.location);
      Assert.AreEqual(companyUpdate.Blurb, (string)result.blurb);
      Assert.AreEqual((int)cvId, (int)result.cvId);
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
      var company = new { Start = DateTime.Parse("2000-08-01"), End = DateTime.Parse("2002-05-01"), CompanyName = "Carlsberg UK", Role = "Quality Assurance Technician", Location = "Leeds", Blurb = "Blah, Blah, Blah" };
      var postCompanyResponse = client.PostAsync($"/cv/{cvId}/companies", new StringContent(JsonConvert.SerializeObject(company), Encoding.UTF8, "application/json")).Result;
      dynamic newCompany = JObject.Parse(postCompanyResponse.Content.ReadAsStringAsync().Result);
      var companyId = newCompany.id;

      //Act
      var putResponse = client.DeleteAsync($"/company/{companyId}").Result;

      //Assert
      var getResponse = client.GetAsync($"/cv/{cvId}/companies").Result;
      var result = getResponse.Content.ReadAsStringAsync().Result;
      Assert.AreEqual("[]", result);
    }
  }
}
