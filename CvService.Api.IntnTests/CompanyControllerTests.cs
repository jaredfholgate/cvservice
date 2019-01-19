using CvService.Tests.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace CvService.Api.IntnTests
{
  [TestClass]
  public class CompanyControllerTests : BaseTest
  {
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
      var cvId = CreateCvAndGetId(client);

      //Act
      var company = new { Start = DateTime.Parse("2000-08-01"), End = DateTime.Parse("2002-05-01"), CompanyName = "Carlsberg UK", Role = "Quality Assurance Technician", Location = "Leeds", Blurb = "Blah, Blah, Blah" };
      var postResponse = client.PostAsync($"/cv/{cvId}/companies", new StringContent(JsonConvert.SerializeObject(company), Constants.Encoding, Constants.MediaType)).Result;

      //Assert
      Assert.AreEqual(HttpStatusCode.Created, postResponse.StatusCode);
      var getResponse = client.GetAsync($"/cv/{cvId}/companies").Result;
      Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);
      dynamic companies = JArray.Parse(getResponse.Content.ReadAsStringAsync().Result);
      var result = companies[0];

      Assert.AreEqual(company.Start, (DateTime)result.start);
      Assert.AreEqual(company.End, (DateTime?)result.end);
      Assert.AreEqual(company.CompanyName, (string)result.companyName);
      Assert.AreEqual(company.Role, (string)result.role);
      Assert.AreEqual(company.Location, (string)result.location);
      Assert.AreEqual(company.Blurb, (string)result.blurb);
      Assert.AreEqual(cvId, (int)result.cvId);
    }

    [TestMethod]
    public void UpdateCompany()
    {
      //Arrange
      var client = _factory.CreateClient();
      var cvId = CreateCvAndGetId(client);
      var companyId = CreateCompanyAndGetId(client, cvId);

      //Act
      var companyUpdate = new { Start = DateTime.Parse("2000-08-02"), End = DateTime.Parse("2002-05-02"), CompanyName = "Carlsberg US", Role = "Quality Assurance Monkey", Location = "Montreal", Blurb = "Nah, Nah, Nah" };
      var putResponse = client.PutAsync($"/company/{companyId}", new StringContent(JsonConvert.SerializeObject(companyUpdate), Constants.Encoding, Constants.MediaTyp)).Result;

      //Assert
      Assert.AreEqual(HttpStatusCode.OK, putResponse.StatusCode);
      var getResponse = client.GetAsync($"/company/{companyId}").Result;
      Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);
      dynamic result = JObject.Parse(getResponse.Content.ReadAsStringAsync().Result);

      Assert.AreEqual(companyUpdate.Start, (DateTime)result.start);
      Assert.AreEqual(companyUpdate.End, (DateTime?)result.end);
      Assert.AreEqual(companyUpdate.CompanyName, (string)result.companyName);
      Assert.AreEqual(companyUpdate.Role, (string)result.role);
      Assert.AreEqual(companyUpdate.Location, (string)result.location);
      Assert.AreEqual(companyUpdate.Blurb, (string)result.blurb);
      Assert.AreEqual(cvId, (int)result.cvId);
    }

    [TestMethod]
    public void DeleteCv()
    {
      //Arrange
      var client = _factory.CreateClient();
      var cvId = CreateCvAndGetId(client);
      var companyId = CreateCompanyAndGetId(client, cvId);

      //Act
      var deleteResponse = client.DeleteAsync($"/company/{companyId}").Result;

      //Assert
      Assert.AreEqual(HttpStatusCode.NoContent, deleteResponse.StatusCode);
      var getResponse = client.GetAsync($"/cv/{cvId}/companies").Result;
      var result = getResponse.Content.ReadAsStringAsync().Result;
      Assert.AreEqual("[]", result);
    }
  }
}
