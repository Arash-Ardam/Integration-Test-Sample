using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TPG.SI.TadbirPay.URLTest.Abstractions;
using TPG.SI.TadbirPay.URLTest.DTOs.Admin;

namespace TPG.SI.TadbirPay.URLTest.Tests.Admin.Application
{
    [Order(1)]
    public class ApplicationTest : IntegrationTestBase
    {
        public ApplicationTest(TadbirPayWebApplicationFactory factory) : base(factory)
        {
        }


        [Fact, Order(1)]
        public async Task CreateApllication_Should_Return_Created()
        {
            //Arrange
            Uri route = CreateAdminbBaseRoute("Application", "");
            var requestBody = CreateRequestBody<testCreateApplicationDTO>(AdminFile, "CreateApplication");

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, requestBody);
            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact, Order(2)]
        public async Task Create_notUniqueCode_Apllication_Should_Return_BadRequest()
        {
            //Arrange
            Uri route = CreateAdminbBaseRoute("Application", "");
            var requestBody = CreateRequestBody<testCreateApplicationDTO>(AdminFile, "CreateApplication");

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, requestBody);
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact , Order(3)]
        public async Task FindApplicationById_Should_Return_Ok_With_True_Data()
        {
            //Arrange
            var applicationId = await GetTestId("TestApplication");
            var route = CreateAdminbBaseRoute("Application", $"{applicationId}");
            var expected = CreateRequestBody<testCreateApplicationDTO>(AdminFile, "CreateApplication");
            expected.id = applicationId;

            //Act
            var response = await _httpClient.GetAsync(route);
            var actual = CreateRequestBody<testCreateApplicationDTO>(await response.Content.ReadAsStringAsync(), null);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //Fluent Assertion
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact,Order(4)]
        public async Task ApplicationChangeStatus_Should_Work()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("Application", "ChangeStatus");
            var body = new
            {
                id = await GetTestId("TestApplication"),
                isEnable = false,
            };

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, body);

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact,Order(5)]
        public async Task ApplicationChangeStatus_WithInvalidId_Should_Return_BadRequest()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("Application", "ChangeStatus");
            var body = new
            {
                id = Guid.NewGuid(),
                isEnable = false,
            };

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, body);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact , Order(6)]
        public async Task UpdateExcistingApplication_Should_Return_Accepted_With_True_Data()
        {
            //Arrange
            var applicationId = await GetTestId("TestApplication");
            var route = CreateAdminbBaseRoute("Application", "");
            var requestBody = CreateRequestBody<testCreateApplicationDTO>(AdminFile, "CreateApplication");
            requestBody.id = applicationId;
            requestBody.Code = "UpdatedAdminTest-app";

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, requestBody);
            var actual = (await response.Content.ReadAsStringAsync()).Trim('"');

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            //Fluent Assertion
            actual.Should().BeEquivalentTo(applicationId.ToString());
        }

        [Fact, Order(7)]
        public async Task UpdateApplication_With_InvalidId_Should_Return_BadRequest_With_True_Data()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("Application", "");
            var requestBody = CreateRequestBody<testCreateApplicationDTO>(AdminFile, "CreateApplication");
            requestBody.id = Guid.NewGuid();
            requestBody.Name = "AdminTest2";

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, requestBody);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact , Order(8)]
        public async Task DeleteExcistingApplication_Should_Return_Accepted_With_True_Data()
        {
            //Arrange
            var applicationId = await GetTestId("TestApplication");
            var route = CreateAdminbBaseRoute("Appllication", $"{applicationId}/remove");
            //Act
            var response = await _httpClient.DeleteAsync(route);
            var actual = await response.Content.ReadAsStringAsync();
            var emptyId = GetTestId("TestApplication");

            //GroupAssert
            using (new AssertionScope())
            {
                emptyId.Should().Be(null);
                response.StatusCode.Should().Be(HttpStatusCode.Accepted);
                actual.Should().BeEquivalentTo(applicationId.ToString());
            }
        }

        

    }
}
