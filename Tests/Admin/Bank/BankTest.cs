using TPG.SI.TadbirPay.URLTest.Abstractions;
using TPG.SI.TadbirPay.URLTest.DTOs.Admin;

namespace TPG.SI.TadbirPay.URLTest.Tests.Admin.Bank
{
    [Order(3)]
    public class BankTest : IntegrationTestBase
    {
        public BankTest(TadbirPayWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact, Order(1)]
        public async Task CreateBank_Should_Work()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("Bank", "");
            var body = CreateRequestBody<testCreateBankDto>(AdminFile, "Bank");

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, body);

            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact, Order(2)]
        public async Task CreateBank_WithDoplicateCode_Should_Return_BadRequest_WithTrueData()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("Bank", "");
            var body = new testCreateBankDto()
            {
                Code = "099",
            };
            var expected = string.Format("مقدار وارد شده برای {0} تکراری می باشد", body.Code);

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, body);
            var actual = await response.Content.ReadAsStringAsync();

            //Assert
            using (new AssertionScope())
            {
                actual.Should().Be(expected);
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [Fact,Order(3)]
        public async Task BankChangeStatus_Should_Work()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("Bank", "ChangeStatus");
            var body = new
            {
                id = await GetTestId("TempBank"),
                isEnable = false,
            };

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, body);

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact,Order(4)]
        public async Task BankChangeStatus_WithInvalidId_Should_Return_NotFound()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("Bank", "ChangeStatus");
            var body = new
            {
                id = Guid.NewGuid(),
                isEnable = false,
            };

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, body);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact, Order(5)]
        public async Task UpdateBank_Should_Work()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("Bank", "");
            var body = CreateRequestBody<testBankDto>(AdminFile, "UpdateBank");
            body.Id = await GetTestId("TempBank");

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, body);

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact, Order(6)]
        public async Task UpdateBank_WithInvalidId_Should_Return_NotFound()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("Bank", "");
            var body = CreateRequestBody<testBankDto>(AdminFile, "UpdateBank");
            body.Id = Guid.NewGuid();

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, body);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact, Order(7)]
        public async Task GetBankById_Should_Work()
        {
            //Arrange
            var bankId = await GetTestId("Bank");
            var route = CreateAdminbBaseRoute("Bank", $"{bankId}");
            var expected = CreateRequestBody<testBankDto>(AdminFile, "GetBank");

            //Act
            var response = await _httpClient.GetAsync(route);
            var actual = CreateRequestBody<testBankDto>(await response.Content.ReadAsStringAsync(), null);

            //Assert
            using (new AssertionScope())
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Fact, Order(8)]
        public async Task GetBankByInvalidId_Should_Return_NotFound()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("Bank", $"{Guid.NewGuid()}");

            //Act
            var response = await _httpClient.GetAsync(route);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact, Order(9)]
        public async Task DeleteBank_Should_Work()
        {
            //Arrange
            var tempId = await GetTestId("TempBank");
            var route = CreateAdminbBaseRoute("Bank", $"{tempId}/remove");

            //Act
            var response = await _httpClient.DeleteAsync(route);

            //Assert
            using (new AssertionScope())
            {
                var deleted = await GetTestId("TempBank");

                deleted.Should().Be(Guid.Empty);
                response.StatusCode.Should().Be(HttpStatusCode.Accepted);
            }
        }

        [Fact, Order(10)]
        public async Task DeleteBank_WithInvalidId_Should_Return_NotFound()
        {
            //Arrange
            var route = CreateAdminbBaseRoute("Bank", $"{Guid.NewGuid()}/remove");

            //Act
            var response = await _httpClient.DeleteAsync(route);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

    }
}
