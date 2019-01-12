using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace CvService.Api.IntnTests
{
  [TestClass]
  public class CvControllerTests
  {
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

      var getResponse = client.GetAsync("/cv").Result;
      var result = getResponse.Content.ReadAsStringAsync().Result;

      Assert.IsTrue(result.Contains("\"name\":\"Test CV 1\",\"blurb\":\"Testing 1234567\""));
    }
  }
}
