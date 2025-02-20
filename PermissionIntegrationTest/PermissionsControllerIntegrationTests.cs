using Application.Dtos;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace IntegrationTest
{
    [TestClass]
    public class PermissionsControllerIntegrationTests
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;

        [TestInitialize]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>()
             .WithWebHostBuilder(builder =>
             {
                 builder.ConfigureAppConfiguration((context, config) =>
                 {
                     var projectDir = Directory.GetCurrentDirectory();
                     var configPath = Path.Combine(projectDir, "..", "API", "appsettings.json");

                     config.AddJsonFile(configPath, optional: false, reloadOnChange: true);
                 });
             });


            _client = _factory.CreateClient();
        }

        [TestMethod]
        public async Task AddPermission_ShouldReturnOk()
        {
            var request = new AddPermissionRequest
            {
                EmployeeForeName = "Test",
                EmployeeSurName = "User",
                PermissionTypeId = 1
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/permissions", content);

            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
