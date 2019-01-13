using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
  }
}
