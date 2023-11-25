using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TPG.SI.TadbirPay.URLTest.Abstractions;
using TPG.SI.TadbirPay.URLTest.DTOs.Admin;

namespace TPG.SI.TadbirPay.URLTest.Tests.Admin.TenantApplication
{
    [Order(4)]
    public class TenantApplicationTest : IntegrationTestBase
    {
        public TenantApplicationTest(TadbirPayWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact, Order(1)]
        public async Task CreateTenantApplication_Should_Return_Created()
        {
            //Arrange
            Uri route = CreateAdminbBaseRoute("TenantApplication", "");
            var body = new testCreateTenantApplicationDto()
            {
                TenantId = await GetTestId("Tenant"),
                ApplicationId = await GetTestId("Application"),
                IsEnable = true
            };

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, body);
            var actual = response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        }

        [Fact, Order(2)]
        public async Task CreateTenantApplication_WithInvalidTenant_Should_Return_BadRequest()
        {
            //Arrange
            var applicationDbId = await GetTestId("Application");
            Uri route = CreateAdminbBaseRoute("TenantApplication", "");
            var body = new testCreateTenantApplicationDto()
            {
                TenantId = Guid.NewGuid(),
                ApplicationId = applicationDbId,
                IsEnable = true
            };

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, body);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, Order(3)]
        public async Task CreateTenantApplication_WithInvalidApplication_Should_Return_BadRequest()
        {
            //Arrange
            var tenantDbId = await GetTestId("Tenant");
            Uri route = CreateAdminbBaseRoute("TenantApplication", "");
            var body = new testCreateTenantApplicationDto()
            {
                TenantId = tenantDbId,
                ApplicationId = Guid.NewGuid(),
                IsEnable = true
            };

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, body);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, Order(4)]
        public async Task CreateTenantApplication_WithExcistingTenantApplication_Should_Return_BadRequest_With_TrueData()
        {
            //Arrange
            Uri route = CreateAdminbBaseRoute("TenantApplication", "");
            var body = new testCreateTenantApplicationDto()
            {
                TenantId = await GetTestId("Tenant"),
                ApplicationId = await GetTestId("Application"),
                IsEnable = true
            };

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, body);
            var actual = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            ////FluentAssertion
            //actual.Should().BeEquivalentTo("این برنامه قبلا به مشتری تخصیص داده شده است.");
        }

        [Fact, Order(5)]
        public async Task TenantApplicationChangeStatus_Should_Work()
        {
            //Arrange
            Uri route = CreateAdminbBaseRoute("TenantApplication", "ChangeStatus");
            var body = new
            {
                id = await GetTestId("TenantApplication"),
                isEnable = false
            };

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, body);

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact, Order(6)]
        public async Task TenantApplicationChangeStatus_WithInvalidId_Should_Return_NotFound()
        {
            //Arrange
            Uri route = CreateAdminbBaseRoute("TenantApplication", "ChangeStatus");
            var body = new
            {
                id = Guid.NewGuid(),
                isEnable = false
            };

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, body);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Fact , Order(7)]
        public async Task UpdateTenantApplication_Should_Return_Accepted()
        {
            //Arrange
            Uri route = CreateAdminbBaseRoute("TenantApplication", "");
            var body = new testUpdateTenantApplicationDto()
            {
                Id = await GetTestId("TenantApplication"),
                TenantId = await GetTestId("Tenant"),
                ApplicationId = await GetTestId("Application"),
                IsEnable = true
            };

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, body);
            var actual = (await response.Content.ReadAsStringAsync()).Trim('"');

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            //FluentAssertion
            actual.Should().BeEquivalentTo(body.Id.ToString());
        }

        [Fact , Order(8)]
        public async Task UpdateTenantApplication_WithInvalidId_Should_Return_BadRequest()
        {
            //Arrange
            Uri route = CreateAdminbBaseRoute("TenantApplication", "");
            var body = new testUpdateTenantApplicationDto()
            {
                Id = Guid.NewGuid(),
                TenantId = await GetTestId("Tenant"),
                ApplicationId = await GetTestId("Application"),
                IsEnable = true
            };

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, body);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, Order(9)]
        public async Task GetTenantApplicationFromId_Should_Work()
        {
            //Arrange
            var tenantApplicationId = await GetTestId("TenantApplication");
            Uri route = CreateAdminbBaseRoute("TenantApplication", $"{tenantApplicationId}");
            var expexted = new testTenantApplicationDto()
            {
                Id = tenantApplicationId,
                ApplicationId = await GetTestId("Application"),
                TenantId = await GetTestId("Tenant"),
                Application = "",
                Tenant = "TadbirPardaz",
                IsEnable = true
            };

            //Act
            var response = await _httpClient.GetAsync(route);
            var actual = CreateRequestBody<testTenantApplicationDto>(await response.Content.ReadAsStringAsync(), null);

            //Assert
            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
            //FluentAssertion
            actual.Should().BeEquivalentTo(expexted);
        }

        [Fact, Order(10)]
        public async Task GetTenantApplicationFromId_WithInvalidId_Should_Return_NotFound()
        {
            //Arrange
            Uri route = CreateAdminbBaseRoute("TenantApplication", $"{Guid.NewGuid()}");

            //Act
            var response = await _httpClient.GetAsync(route);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);  
        }

        [Fact, Order(11)]
        public async Task DeleteTenantApplication_Should_Work()
        {
            //Arrange
            Uri route = CreateAdminbBaseRoute("TenantApplication", $"{await GetTestId("TenantApplication")}/remove");

            //Act
            var response = await _httpClient.DeleteAsync(route);

            //GroupeAssert
            var applicationId = await GetTestId("Application");
            var executionQuery = "use [TPG.SI.TadbirPay] select id from dbo.TenantApplication where ApplicationId = '{0}' ";
            var actual = await GetTestId(string.Format(executionQuery, applicationId));

            using (new AssertionScope())
            {
                response.StatusCode.Should().Be(HttpStatusCode.Accepted);
                actual.Should().Be(Guid.Empty);
            }
        }

        [Fact, Order(12)]
        public async Task DeleteTenantApplication_WithInvalidId_Should_Return_NotFound()
        {
            //Arrange
            Uri route = CreateAdminbBaseRoute("TenantApplication", $"{Guid.NewGuid()}/remove");

            //Act
            var response = await _httpClient.DeleteAsync(route);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
        }
    }
}
