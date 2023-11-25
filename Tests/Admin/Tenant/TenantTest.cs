using k8s.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPG.SI.TadbirPay.URLTest.Abstractions;
using TPG.SI.TadbirPay.URLTest.DTOs;
using TPG.SI.TadbirPay.URLTest.DTOs.Admin;

namespace TPG.SI.TadbirPay.URLTest.Tests.Admin.Tenant
{
    [Order(2)]
    public class TenantTest : IntegrationTestBase
    {
        public TenantTest(TadbirPayWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact, Order(1)]
        public async Task CreateTenant_Should_Work()
        {
            //Arrange
            Uri route = CreateAdminbBaseRoute("Tenant", "");
            var body = new testCreateTenantDto()
            {
                Code = "Test",
                Name = "TenantTest",
                ConnectionString = "",
                IsEnable = true,
            };

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, body);
            var actual = (await response.Content.ReadAsStringAsync()).Trim('"');
            var expected = (await GetTestId("TestTenant")).ToString();

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            //FluentAssertion
            actual.Should().BeEquivalentTo(expected);

        }

        [Fact,Order(2)]
        public async Task TenantChangeStatus_Should_Work()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("Tenant", "ChangeStatus");
            var body = new
            {
                id = await GetTestId("TestTenant"),
                isEnable = false
            };

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, body);

            //Assert
            Assert.Equal(HttpStatusCode.Accepted,response.StatusCode);
        }

        [Fact,Order(3)]
        public async Task TenantChangeStatus_WithInvalidId_Should_Return_BadRequest()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("Tenant", "ChangeStatus");
            var body = new
            {
                id = Guid.NewGuid(),
                isEnable = false
            };

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, body);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, Order(4)]
        public async Task UpdateTenant_Should_Work()
        {
            //Arrange
            Uri route = CreateAdminbBaseRoute("Tenant", "");
            var body = new testUpdateTenantDto()
            {
                Code = "Test",
                Name = "TenantTest",
                Id = await GetTestId("TestTenant"),
                IsEnable = true,
                ConnectionString = "Server=172.16.30.144,1433;Database=TPG.SI.TadbirPay.TenantTest;User Id=si-user;Password=tadbir@123;TrustServerCertificate=True"
            };

            var expected = (await GetTestId("TestTenant")).ToString();

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, body);
            var actual = (await response.Content.ReadAsStringAsync()).Trim('"');

            //Assertions
            Assert.Equal(HttpStatusCode.Accepted,response.StatusCode);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact, Order(5)]
        public async Task UpdateTenant_WithInvalidId_Should_Return_BadRequest()
        {
            //Arrange
            Uri route = CreateAdminbBaseRoute("Tenant", "");
            var body = new testUpdateTenantDto()
            {
                Id = Guid.NewGuid(),
                Code = "Test",
                Name = "TenantTest",
                IsEnable = true,
            };

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, body);  
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact , Order(6)]
        public async Task GetTenantById_Should_Work()
        {
            //Arrange
            var tenantId = await GetTestId("TestTenant");
            Uri route = CreateAdminbBaseRoute("Tenant", $"{tenantId}/find");
            var expected = CreateRequestBody<testTenantDetailDto>(AdminFile, "GetTestTenant");

            //Act
            var response = await _httpClient.GetAsync(route);
            var actual = CreateRequestBody<testTenantDetailDto>(await response.Content.ReadAsStringAsync(), null);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact , Order(7)]
        public async Task GetTenantByInvalidId_Should_Return_BadRequest()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("Tenant", $"{Guid.NewGuid()}/find");

            //Act
            var response = await _httpClient.GetAsync(route);
           
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest,response.StatusCode);
        }

        [Fact, Order(8)]
        public async Task AnyTenantByIdExist_Should_Work()
        {
            //Arrange
            var tenantId = await GetTestId("TestTenant");
            Uri route = CreateAdminbBaseRoute("Tenant", $"{tenantId}/Any");

            //Act
            var response = await _httpClient.GetAsync(route);
            var actual = response.Content.ReadAsStringAsync().Result;

            //Assert
            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
            actual.Should().BeEquivalentTo("true");
        }

        [Fact, Order(9)]
        public async Task CountTenants_Should_Work()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("Tenant", "Count");
            var expected = await getTenantsCount();
            //Act
            var response = await _httpClient.GetAsync(route);
            var actual = await response.Content.ReadAsStringAsync();

            //GroupAssert
            using (new AssertionScope())
            {
                (Convert.ToInt32(actual)).Should().Be(expected);
                (response.StatusCode).Should().Be(HttpStatusCode.OK);
            }
            
        }

        [Fact, Order(10)]
        public async Task RemoveTenant_Should_Work()
        {
            //Arrange
            var tenantId = await GetTestId("TestTenant");
            var route = CreateAdminbBaseRoute("Tenant", $"{tenantId}/remove");

            //Act
            var response = await _httpClient.DeleteAsync(route);
            var actual = await GetTestId("TestTenant");

            //Assert
            using (new AssertionScope())
            {
                actual.Should().Be(Guid.Empty);
                (response.StatusCode).Should().Be(HttpStatusCode.Accepted);
            }

        }

        [Fact, Order(11)]
        public async Task RemoveTenant_WithInvalidId_Should_Return_NotFound()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("Tenant", $"{Guid.NewGuid}/remove");

            //Act
            var response = await _httpClient.DeleteAsync(route);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


    }
}
