using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPG.SI.TadbirPay.Infrastructure.Helper;
using TPG.SI.TadbirPay.URLTest.Abstractions;
using TPG.SI.TadbirPay.URLTest.DTOs.Admin;

namespace TPG.SI.TadbirPay.URLTest.Tests.Admin.BankGateway
{
    [Order(5)]
    public class BankGatewayTest : IntegrationTestBase
    {
        public BankGatewayTest(TadbirPayWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact,Order(1)]
        public async Task CreateBankGateway_Should_Work()
        {
            //Arrange
            var tenantId = await GetTestId("Tenant");
            var bankId = await GetTestId("Bank");
            var route = CreateAdminbBaseRoute("BankGateway","");
            var body = CreateRequestBody<testBankGatewayDto>(AdminFile, "CreateBankGateway");
            body.TenantId = tenantId;
            body.BankId = bankId;

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, body);

            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        }

        [Fact,Order(2)]
        public async Task CreateBankGateway_WithUnuniqueSheba_Should_REturn_BadRequest_With_TrueData()
        {
            //Arrange
            var tenantId = await GetTestId("Tenant");
            var bankId = await GetTestId("Bank");
            var route = CreateAdminbBaseRoute("BankGateway", "");
            var body = CreateRequestBody<testBankGatewayDto>(AdminFile, "CreateBankGateway");
            body.TenantId = tenantId;
            body.BankId = bankId;

            var expected = string.Format("مقدار وارد شده برای {0} تکراری می باشد", body.ShebaNumber);

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, body);
            var actual = await response.Content.ReadAsStringAsync();

            //Assert
            using (new AssertionScope())
            {
                actual.Should().BeEquivalentTo(expected);
                (response.StatusCode).Should().Be(HttpStatusCode.BadRequest);
            }
        }


        [Fact, Order(3)]
        public async Task GetBankGateway_WithValidId_Should_Work()
        {
            //Arrange
            var testId = await GetTestId("BankGateway");
            var route = CreateAdminbBaseRoute("BankGateway", $"{testId}");
            var expected = CreateRequestBody<testBankGatewayDto>(AdminFile, "GetBankGateway");
            expected.id = testId;

            //Act
            var response = await _httpClient.GetAsync(route);
            var actual = CreateRequestBody<testBankGatewayDto>(await response.Content.ReadAsStringAsync(),null);

            //Assert
            using (new AssertionScope())
            {
                (response.StatusCode).Should().Be(HttpStatusCode.OK);
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Fact, Order(4)]
        public async Task GetBankGateway_WithInvalidId_Should_Return_NotFound()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("BankGateway", $"{Guid.NewGuid()}");

            //Act
            var response = await _httpClient.GetAsync(route);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }



        [Fact,Order(5)]
        public async Task UpdateBankGateway_Should_Work()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("BankGateway", "");
            var body = CreateRequestBody<testBankGatewayDto>(AdminFile, "CreateBankGateway");
            body.TenantId = await GetTestId("Tenant"); 
            body.BankId = await GetTestId("Bank");
            body.id = await GetTestId("BankGateway");
            body.Title = "TestGateway";
            body.IsEnable = false;

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, body);

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact,Order(6)]
        public async Task UpdateBankGateway_WithInvalidId_Should_Return_NotFound()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("BankGateway", "");
            var body = CreateRequestBody<testBankGatewayDto>(AdminFile, "CreateBankGateway");
            body.id = Guid.NewGuid();
            body.Title = "TestGateway";
            body.IsEnable = false;

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, body);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Fact,Order(7)]
        public async Task BankGatewayChangeStatus_Should_Work()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("BankGateway", "ChangeStatus");
            var body = new
            {
                id = await GetTestId("BankGateway"),
                isEnable = true,
            };

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, body);

            //Assert
            (response.StatusCode).Should().Be(HttpStatusCode.Accepted);
        }

        [Fact,Order(8)]
        public async Task BankGatewayChangeStatus_WithInvalidId_Should_Return_NotFound()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("BankGateway", "ChangeStatus");
            var body = new
            {
                id = Guid.NewGuid(),
                isEnable = true,
            };

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, body);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact,Order(9)]
        public async Task AddCartableUser_Should_Work()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("BankGateway", "AddCartableUser");
            var body = CreateRequestBody<testBankGatewayUserDto>(AdminFile, "CartableUser");
            body.BankGatewayId = await GetTestId("BankGateway");

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, body);

            //Assert
            Assert.Equal(HttpStatusCode.Created,response.StatusCode);
        }

        [Fact,Order(10)]
        public async Task AddDouplicateCartableUser_Should_Return_BadRequest()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("BankGateway", "AddCartableUser");
            var body = CreateRequestBody<testBankGatewayUserDto>(AdminFile, "CartableUser");
            body.BankGatewayId = await GetTestId("BankGateway");

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, body);

            //Assert
            using (new AssertionScope())
            {
                (response.StatusCode).Should().Be(HttpStatusCode.BadRequest);
                (await response.Content.ReadAsStringAsync()).Should().Be($"این کاربر قبلا بع عنوان {body.UserType.ToDisplay()} به این حساب اضافه شده است.");
            }
        }

        [Fact,Order(11)]
        public async Task FindCartableUserById_Should_Work()
        {
            //Arrange
            var testId = await GetTestId("TestCartableUser");
            var route = CreateAdminbBaseRoute("BankGateway", $"FindGatewayUserById/{testId}");
            var expected = CreateRequestBody<testBankGatewayUserDto>(AdminFile, "TestCartableUser");

            expected.Id = testId;
            expected.BankGatewayId = await GetTestId("BankGateway");

            //Act
            var response = await _httpClient.GetAsync(route);
            var actual = CreateRequestBody<testBankGatewayUserDto>(await response.Content.ReadAsStringAsync(), null);

            //Assert
            using (new AssertionScope())
            {
                (response.StatusCode).Should().Be(HttpStatusCode.OK);
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Fact,Order(12)]
        public async Task FindCartableUser_WithInvalidId_Should_Return_NotFound()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("BankGateway", $"FindGatewayUserById/{Guid.NewGuid()}");


            //Act
            var response = await _httpClient.GetAsync(route);

            //Assert
            (response.StatusCode).Should().Be(HttpStatusCode.NotFound);
        }

        [Fact,Order(13)]
        public async Task GetGatewayUser_Should_Work()
        {
            //Arrange
            var testId = await GetTestId("BankGateway");
            var route = CreateAdminbBaseRoute("BankGateway", $"GetGatewayUser/{testId}");
            var expected = CreateRequestBody<testBankGatewayUserDto>(AdminFile, "TestCartableUser");

            expected.Id = await GetTestId("TestCartableUser");
            expected.BankGatewayId = testId;

            //Act
            var response = await _httpClient.GetAsync(route);
            var actual = CreateRequestBody<List<testBankGatewayUserDto>>(await response.Content.ReadAsStringAsync(), null);

            //Assert
            using (new AssertionScope())
            {
                (response.StatusCode).Should().Be(HttpStatusCode.OK);
                (actual[0]).Should().BeEquivalentTo(expected);
            }
        }

        [Fact,Order(14)]
        public async Task GetGatewayUser_WithInvalidId_Should_Return_NotFound()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("BankGateway", $"GetGatewayUser/{Guid.NewGuid()}");

            //Act
            var response = await _httpClient.GetAsync(route);

            //Assert
            (response.StatusCode).Should().Be(HttpStatusCode.NotFound);
        }

        [Fact,Order(15)]
        public async Task UpdateCartableUser_WithDouplicateUserType_Should_Return_BarRequest_WithTrueData()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("BankGateway", "EditCartableUser");
            var body = CreateRequestBody<testBankGatewayUserDto>(AdminFile, "TestCartableUser");

            body.Id = await GetTestId("TestCartableUser");
            body.BankGatewayId = await GetTestId("BankGateway");
            body.FullName = "Updated";

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, body);

            //Assert
            using (new AssertionScope())
            {
                (response.StatusCode).Should().Be(HttpStatusCode.BadRequest);
                (await response.Content.ReadAsStringAsync()).Should().Be($"این کاربر قبلا بع عنوان {body.UserType.ToDisplay()} به این حساب اضافه شده است.");
            }
        }

        [Fact,Order(16)]
        public async Task UpdateCartableUser_Should_Work()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("BankGateway", "EditCartableUser");
            var body = CreateRequestBody<testBankGatewayUserDto>(AdminFile, "TestCartableUser");

            body.Id = await GetTestId("TestCartableUser");
            body.BankGatewayId = await GetTestId("BankGateway");
            body.FullName = "Updated";
            body.UserType = 0;

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, body);
            
            //Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact,Order(17)]
        public async Task DeleteBankGateway_Should_Work()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("BankGateway", $"{await GetTestId("BankGateway")}/remove");

            //Act
            var response = await _httpClient.DeleteAsync(route);

            //Assert
            using (new AssertionScope())
            {
                (response.StatusCode).Should().Be(HttpStatusCode.Accepted);
                (await GetTestId("BankGateway")).Should().Be(Guid.Empty);
            }
        }

        [Fact,Order(18)]
        public async Task DeleteBankGateway_WithInvalidId_Should_Return_NotFound()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("BankGateway", $"{Guid.NewGuid()}/remove");

            //Act
            var response = await _httpClient.DeleteAsync(route);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
        }
    }
}
