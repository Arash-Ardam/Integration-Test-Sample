using FluentAssertions;
using Nest;
using System.Net;
using System.Net.Http.Json;
using System.ServiceModel;
using TPG.SI.TadbirPay.URLTest.Abstractions;
using TPG.SI.TadbirPay.URLTest.DTOs.Payment;
using Xunit.Extensions.Ordering;

namespace TPG.SI.TadbirPay.URLTest.Tests.Payment
{
    [Order(6)]
    public class PaymentTests : IntegrationTestBase
    {
        public PaymentTests(TadbirPayWebApplicationFactory factory) : base(factory)
        {
        }


        #region PostMethod Tests (Order 1-12)
        [Fact,Order(1)]
        public async Task CreatePayment_Should_Return_Created()
        {
            //Arrange
            

            Uri route = CreatePaymentBaseRoute("CreatePayment");
            var requestBody = CreateRequestBody<TestRequestPaymentDTO>(PaymentFile,"createPayment");

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, requestBody);

            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        }

        [Fact,Order(2)]
        public async Task CreatePayment_Should_Return_BadRequest()
        {
            //Arrange
            Uri route = CreatePaymentBaseRoute("CreatePayment");
            var requestBody = new { };

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, requestBody);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            
        }

        [Fact,Order(3)]
        public async Task AddTransactions1_Should_Return_Created()
        {
            //Arrange
            Uri route = CreatePaymentBaseRoute("AddTransactions");
            var requestBody = CreateRequestBody<testRequestTransactionDTO>(PaymentFile, "addTransactions1");
            requestBody.Id = await GetTestId("Payment");

            //Act
            var response = await _httpClient.PostAsJsonAsync(route,requestBody);

            //Assert
            Assert.Equal(HttpStatusCode.Created,response.StatusCode);
        }

        

        [Fact,Order(4)]
        public async Task AddTransactions_With_notEqualTransactionNumber_Should_Return_BadRequest_And_TrueContent()
        {
            //Arrange
            Uri route = CreatePaymentBaseRoute("AddTransactions");
            var requestBody = CreateRequestBody<testRequestTransactionDTO>(PaymentFile, "addTransactionsListOutOfRange");
            requestBody.Id = await GetTestId("Payment");

            var expectedMessage = "تعداد ردیف تراکنش بیشتر از تعداد ثبت شده در درخواست پرداخت می باشد.";

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, requestBody);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest,response.StatusCode);
            Assert.Equal(expectedMessage,content);
        }

        [Fact,Order(5)]
        public async Task AddTransactions_With_notEqualTotalAmount_Should_Return_BadRequest_And_TrueContent()
        {
            //Arrange
            Uri route = CreatePaymentBaseRoute("AddTransactions");
            var requestBody = CreateRequestBody<testRequestTransactionDTO>(PaymentFile, "addTransactionsAmountOutOfRange");
            requestBody.Id = await GetTestId("Payment") ;

            var expectedMessage = "مبلغ کل تراکنش بیشتر از مبلغ ثبت شده در درخواست پرداخت می باشد.";

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, requestBody);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(expectedMessage ,content);
        }

        [Fact,Order(6)]
        public async Task AddTransactions_With_DouplicateIban_Should_Return_BadRequest_And_TrueContent()
        {
            //Arrange
            Uri route = CreatePaymentBaseRoute("AddTransactions");
            var requestBody = CreateRequestBody<testRequestTransactionDTO>(PaymentFile, "addTransactionsDouplicateIban");
            requestBody.Id = await GetTestId("Payment") ;

            var expectedMessage = $"شماره شباهای ]{"IR5000020000300006053"}[ تکراری می باشند.";

            //Act
            var response = await _httpClient.PostAsJsonAsync(route,requestBody);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(expectedMessage,content);
        }

        [Fact,Order(7)]
        public async Task AddTransactions_With_invalidId_Should_Return_BadRequest_And_TrueContent()
        {
            //Arrange
            Uri route = CreatePaymentBaseRoute("AddTransactions");
            var requestBody = CreateRequestBody<testRequestTransactionDTO>(PaymentFile, "addTransactions1");

            var expectedMessage = "شناسه درخواست پرداخت نامعتبر است";

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, requestBody);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(expectedMessage,content);
        }

        

        [Theory,Order(8)]
        [InlineData("3fa85f64-5717-4562-b3fc-2c963f66afa6")]
        public async Task VerifyOrder_WithInValidId_Should_Return_BadRequest_With_True_Data(string invalidId)
        {
            //Arrange
            Uri route = CreatePaymentBaseRoute($"VerifyOrder/{invalidId}");
            var expectedMessage = "شناسه درخواست پرداخت نامعتبر است";

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, invalidId);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest,response.StatusCode);
            Assert.Equal(expectedMessage, content);
        }

        [Fact, Order(9)]
        public async Task VerifyOrder_With_NotCompeleted_TransactionsList_and_TotalAmount_Should_Return_BadRequest_With_True_Data()
        {
            //Arrange
            var id = await GetTestId("Payment");
            Uri route = CreatePaymentBaseRoute($"VerifyOrder/{id}");
            var expectedMessage = "مبلغ کل تراکنش و تعداد ردیف تراکنش با درخواست پرداخت ثبت شده مغایرت دارد.";

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, id);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(expectedMessage, content);
        }

        [Fact, Order(10)]
        public async Task VerifyOrder_With_NotCompeletedTransactionsAmount_Should_Return_BadRequest_With_True_Data()
        {
            //Arrange VerifyPeyment 
            var createPaymentBody = CreateRequestBody<TestRequestPaymentDTO>(VerifyPaymentFile, "createPayment");
            var createPaymentRoute = CreatePaymentBaseRoute("CreatePayment");
            var paymentResponse = await _httpClient.PostAsJsonAsync(createPaymentRoute, createPaymentBody);
            var donepaymentid = await paymentResponse.Content.ReadAsStringAsync();
            var paymentId = Guid.Parse((donepaymentid).Trim('"'));

            var createTransactionsBody = CreateRequestBody<testRequestTransactionDTO>(VerifyPaymentFile, "addTransactions");
            createTransactionsBody.Id = paymentId;
            var createTransactionRoute = CreatePaymentBaseRoute("AddTransactions");
            await _httpClient.PostAsJsonAsync(createTransactionRoute, createTransactionsBody);

            //Arrange
            Uri route = CreatePaymentBaseRoute($"VerifyOrder/{paymentId}");
            var expectedMessage = "مبلغ کل تراکنش بیشتر از مبلغ ثبت شده در درخواست پرداخت می باشد.";

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, paymentId);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(expectedMessage, content);
        }

        [Fact, Order(11)]
        public async Task AddTransactions2_Should_Return_Created()
        {
            //Arrange

            Uri route = CreatePaymentBaseRoute("AddTransactions");
            var requestBody = CreateRequestBody<testRequestTransactionDTO>(PaymentFile, "addTransactions2");
            requestBody.Id = await GetTestId("Payment");

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, requestBody);

            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact, Order(12)]
        public async Task VerifyOrder_Should_Return_OK_With_True_Data()
        {
            //Arrange
            var id = await GetTestId("Payment");
            Uri route = CreatePaymentBaseRoute($"VerifyOrder/{id}");
            var expectedMessage = "عملیات با موفقیت انجام شد.";

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, id);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedMessage, content);
        }
        #endregion

        #region Get methods

        [Fact, Order(13)]
        public async Task GetAccounts_Should_Return_Ok_with_True_Data()
        {
            //Arrange
            Uri route = CreatePaymentBaseRoute("GetAccounts");
            var expected = CreateRequestBody<testWithdrawalGatewayListDTO>(GetMethodsFile, "GetAccounts");

            //Act
            var response = await _httpClient.GetAsync(route);
            var actual = CreateRequestBody<List<testWithdrawalGatewayDTO>>(await response.Content.ReadAsStringAsync(), null);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //Fluent Assertion
            actual.Should().BeEquivalentTo(expected.Items);
        }

        [Fact, Order(14)]
        public async Task InquiryById_Should_Return_Ok_With_True_Data()
        {
            //Arrange
            var testId = await GetTestId("Payment");
            Uri route = CreatePaymentBaseRoute($"InquiryById/{testId}");
            var expected = CreateRequestBody<testWithdrawalOrderDTO>(GetMethodsFile, "InquiryById");

            foreach (var item in expected.Transactions)
            {
                item.WithdrawalOrderId = testId;
            }

            foreach (var item in expected.ChangeHistory)
            {
                item.WithdrawalOrderId = testId;
            }

            //Act
            var response = await _httpClient.GetAsync(route);
            var actualMessage = await response.Content.ReadAsStringAsync();
            var actual = CreateRequestBody<testWithdrawalOrderDTO>(actualMessage, null);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //Fluent Assertion
            actual.Should().BeEquivalentTo(expected);
            
        }

        [Fact, Order(15)]
        public async Task FindPaymentById_Should_Return_Ok_With_True_Data()
        {
            //Arrange
            var testId = await GetTestId("Payment");
            Uri route = CreatePaymentBaseRoute($"{testId}/find");
            var expected = CreateRequestBody<testWithdrawalOrderDTO>(GetMethodsFile, "InquiryById");

            foreach (var item in expected.Transactions)
            {
                item.WithdrawalOrderId = testId;
            }

            foreach (var item in expected.ChangeHistory)
            {
                item.WithdrawalOrderId = testId;
            }

            //Act
            var response = await _httpClient.GetAsync(route);
            var actual = CreateRequestBody<testWithdrawalOrderDTO>(await response.Content.ReadAsStringAsync(), null);

            //Assert
            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
            //Fluent Assertion
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact, Order(16)]
        public async Task TransactionInquiryById_Should_Return_ok_With_True_Data()
        {
            //Arrange
            Uri route = CreatePaymentBaseRoute($"TransactionInquiryById/{await GetTestId("Transaction")}");
            var expected = CreateRequestBody<testWithdrawalTransactionDto>(GetMethodsFile, "TransactionInquiryById");

            //Act
            var response = await _httpClient.GetAsync(route);
            var t = await response.Content.ReadAsStringAsync();
            var actual = CreateRequestBody<testWithdrawalTransactionDto>(await response.Content.ReadAsStringAsync(), null);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //Fluent Assertion
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact, Order(17)]
        public async Task GetPaymentHistory_Should_Return_OK_With_True_Data()
        {
            //Arrange
            var paymentId = await GetTestId("Payment");

            Uri route = CreatePaymentBaseRoute($"History/{paymentId}");
            var expected = CreateRequestBody<List<testWithdrawalOrderLogDto>>(GetMethodsFile, "PaymentLog");
            foreach (var item in expected)
            {
                item.WithdrawalOrderId = paymentId;
            }

            //Act
            var response = await _httpClient.GetAsync(route);
            var actual = CreateRequestBody<List<testWithdrawalOrderLogDto>>(await response.Content.ReadAsStringAsync(), null);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //Fluent Assertion
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact, Order(18)]
        public async Task InquiryById_Should_Return_NotFound()
        {
            //Arrange
            Uri route = CreatePaymentBaseRoute($"InquiryById/0A706FE9-45EB-4CA2-9236-AE56A0062D1F");

            //Act
            var response = await _httpClient.GetAsync(route);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact, Order(19)]
        public async Task FindPaymentById_Should_Return_NotFound_With_True_Data()
        {
            //Arrange
            Uri route = CreatePaymentBaseRoute("0A706FE9-45EB-4CA2-9236-AE56A0062D1F/find");

            //Act
            var response = await _httpClient.GetAsync(route);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact, Order(20)]
        public async Task TransactionInquiryById_Should_Return_NotFound_With_True_Data()
        {
            //Arrange
            Uri route = CreatePaymentBaseRoute("TransactionInquiryById/0A706FE9-45EB-4CA2-9236-AE56A0062D1F");

            //Act
            var response = await _httpClient.GetAsync(route);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact, Order(21)]
        public async Task GetPaymentHistory_Should_Return_NotFound_With_True_Data()
        {
            //Arrange
            Uri route = CreatePaymentBaseRoute($"History/0A706FE9-45EB-4CA2-9236-AE56A0062D1F");

            //Act
            var response = await _httpClient.GetAsync(route);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        #endregion
    }
}
