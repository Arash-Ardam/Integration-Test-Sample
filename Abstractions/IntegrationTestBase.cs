using Amazon.Runtime;
using IdentityModel.Client;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPG.SI.TadbirPay.URLTest.DTOs;
using static StackExchange.Redis.Role;
//using static System.Net.WebRequestMethods;

namespace TPG.SI.TadbirPay.URLTest.Abstractions
{
    public class IntegrationTestBase : IClassFixture<TadbirPayWebApplicationFactory>, IDisposable
    {
        protected readonly TadbirPayWebApplicationFactory _factory;
        protected readonly HttpClient _httpClient;
        protected Guid LastTestDoneActionId;
        protected int Tenants;
        private SqlConnection _dbConnection;
    
        public string PaymentFile { get; set; }
            = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Tests", "Payment", "PaymentContent.json"));

        public string VerifyPaymentFile { get; set; }
            = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Tests", "Payment", "VerifyPaymentContent.json"));

        public string GetMethodsFile { get; set; }
        = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Tests", "Payment", "GetMethodsContent.json"));

        public string AdminFile { get; set; }
        = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Tests", "Admin", "AdminTestContent.json"));

        public IntegrationTestBase(TadbirPayWebApplicationFactory factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();
            _httpClient.SetBearerToken("eyJhbGciOiJSUzI1NiIsImtpZCI6IkJEQTFFRURDNjNDQkVDNDY4N0Q5MzdDNThCM0ZBQjYxIiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE2OTg1NzY5ODUsImV4cCI6MTcwMjE3Njk4NSwiaXNzIjoiaHR0cHM6Ly9zaS1sYWItaWRwLmV0YWRiaXIuY29tIiwiYXVkIjpbIlRQRy5TSS5JRFAuQXBpIiwiVFBHLlNJLlRhZGJpclBheS5BcGkiXSwiY2xpZW50X2lkIjoiVGFkYmlyUGF5LkFwaS5Td2FnZ2VyIiwic3ViIjoiNzRjYWNkNjItOGRiNy00N2NiLWI0YjgtYmZkYzY0MWFiMTRiIiwiYXV0aF90aW1lIjoxNjk4MjI4OTYyLCJpZHAiOiJsb2NhbCIsInRlbmFudCI6IlRhZGJpclBhcmRheiIsIm5hbWUiOiLZhdiv24zYsSDYs9uM2LPYqtmFIiwicm9sZSI6WyJjYXJ0YWJsZWFwcHJvdmVyLnJvbGUiLCJXaXRoZHJhd2FsLmNsaWVudC5yb2xlIiwiaXBkcC1hZG1pbiIsImNhcnRhYmxlYWRtaW4ucm9sZSJdLCJlbWFpbCI6ImEucGFuYWhpYW5AZXRhZGJpci5jb20iLCJqdGkiOiJFNEE3OTU5Mjk1MTk0OTE3RUE5NEVGQjRCRTc5Q0U5NCIsInNpZCI6IkM2MjU4MzI1NUIzOUM0RDRDOTIwMTk2OUIyRjVFNzEzIiwiaWF0IjoxNjk4NTc2OTg1LCJzY29wZSI6WyJUYWRiaXJQYXkuQWRtaW5pc3RyYXRvci5BcGkuU2NvcGUiLCJUYWRiaXJQYXkuQ2FydGFibGUuQXBpLlNjb3BlIiwiVGFkYmlyUGF5LldpdGhkcmF3YWwuQXBpLlNjb3BlIiwiVGFkYmlyUGF5LkdhdGV3YXkuQXBpLlNjb3BlIiwiVFBHLlNJLklEUC5BcGkiXSwiYW1yIjpbInB3ZCJdfQ.A4NCfyB4DtCFJMqpE-OTvubn9HY9etNANqKCZ6FBFcf20GwN3QwmKnyPbkry6xargjrN_XJM33FwRPApUrqBTMioQbsnT2Y9Rp7UKfMdglwbQDqXeGUm9V76p_fm10aFm8nzunV5Bf-JsHo2HREn4QMyT1ihazGFP16TnQGWS2Bgq1ktNYiO8BZa35QeHnCQBbzLNN-bNV1mw8SUmqJ3dKLZm8Jip0cOa_HwVRMpiZsLgpz_YLSGK_Zl5oXr1J-k_TF_E-O9m5Y-c5eiD9k7TyCp1IAkCAVWeR7shlvIFF4qwFg15slWtCq6gvD4lK4U3uJZWmE54UHqrnrWFlROgg");
        }

        protected Uri CreatePaymentBaseRoute(string route)
        {
            return new Uri($"{_factory.Server.BaseAddress}api/v1-Payment/Payment/{route}");
        }

        protected Uri CreateAdminbBaseRoute(string test,string route) 
        {
            return new Uri($"{_factory.Server.BaseAddress}api/v1-Admin/{test}/{route}");
        }

        /// <summary>
        /// returns a requestBody based on RequestContent section and destination object
        /// </summary>
        /// <returns></returns>
        protected requestType CreateRequestBody<requestType>(string file,string section) where requestType : class
        {
            if (null == section)
            {
                return JsonConvert.DeserializeObject<requestType>(file);
            }
            var body = JsonConvert.DeserializeObject<requestType>(JObject.Parse(file)[section].ToString());

            return body;
        }

        #region Get Id By sqlConnection methods
        
        /// <summary>
        /// Get data from db by executing specified query
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>

        public async Task<Guid> GetTestId(string action)
        {
                InitDbConnection();
                await EnsureDbConnectionOpen();
                await SetLastTestDoneActionIdFromDb(action);
                await EnsureDbConnectionClosed();
            
            return LastTestDoneActionId;
        }

        private async Task SetLastTestDoneActionIdFromDb(string action)
        {
            using (var command = _dbConnection.CreateCommand())
            {
                switch (action)
                {
                    case "Payment":
                        command.CommandText = "use [TadbirPay.4a37b615-3da2-5c39-fff9-ebed20031da9] select Id from dbo.WithdrawalOrder where Description = 'This is a Test Payment Request' ";
                        break;
                    case "Transaction":
                        command.CommandText = "use [TadbirPay.4a37b615-3da2-5c39-fff9-ebed20031da9] select id from dbo.WithdrawalTransaction where DestinationAccountOwner = 'Infra' ";
                        break;
                    case "Application":
                        command.CommandText = "use [TPG.SI.TadbirPay] select id from dbo.Application where Name = 'TadbirPayTestApp'";
                        break;
                    case "Tenant":
                        command.CommandText = "use [TPG.SI.TadbirPay] select id from dbo.Tenant where Name = 'TadbirPardaz'";
                        break;
                    case "Bank":
                        command.CommandText = "use [TPG.SI.TadbirPay] select id from dbo.Bank where  ProviderCode = 099";
                        break;
                    case "BankGateway":
                        command.CommandText = "use [TPG.SI.TadbirPay] select Id from dbo.BankGateway where Title = 'TestGateway'";
                        break;
                    case "TenantApplication":
                        command.CommandText = "use [TPG.SI.TadbirPay] select id from dbo.TenantApplication where TenantId = '4A37B615-3DA2-5C39-FFF9-EBED20031DA9'";
                        break;
                    case "TestApplication":
                        command.CommandText = "use [TPG.SI.TadbirPay] select id from dbo.Application where Name = 'AdminTest'";
                        break;
                    case "TestTenant":
                        command.CommandText = "use [TPG.SI.TadbirPay] select id from dbo.Tenant where Code = 'test'";
                        break;
                    case "TestCartableUser":
                        command.CommandText = "use [TPG.SI.TadbirPay] select id from dbo.BankGatewayUsers where FullName = 'testApprover'";
                        break;
                    case "TempBank":
                        command.CommandText = "use [TPG.SI.TadbirPay] select id from dbo.Bank where  ProviderCode = 007";
                        break;
                    default:
                        command.CommandText = action;
                        break;

                }

                using (var reader = await command.ExecuteReaderAsync())
                {
                    LastTestDoneActionId = await GetLastId(reader);
                }
            }
        }

        private async Task<Guid> GetLastId(SqlDataReader reader)
        {
            var lastReadId = Guid.Empty;
            
            while (await reader.ReadAsync())
            {
                lastReadId = reader.GetGuid(0);
            }

            return lastReadId;
        }

        public void InitDbConnection()
        {
            var connectionString = "Server=.;Database=TPG.SI.TadbirPay;User Id=si-user;Password=tadbir@123;TrustServerCertificate=True";

            _dbConnection = new SqlConnection(connectionString);            
        }

        public async Task EnsureDbConnectionOpen()
        {
            if (_dbConnection == null)
            {
                throw new Exception();
            }

            if(_dbConnection.State != System.Data.ConnectionState.Open)
            {
              await _dbConnection.OpenAsync();
            }
        }

        public async Task EnsureDbConnectionClosed()
        {
            if (_dbConnection == null)
            {
                throw new Exception();
            }

            if (_dbConnection.State != System.Data.ConnectionState.Closed)
            {
                await _dbConnection.CloseAsync();
            }
        }

        public async Task CloseConnectionAsync()
        {
            if (_dbConnection == null)
            {
                throw new Exception();
            }

            if (_dbConnection.State != System.Data.ConnectionState.Closed)
            {
                await _dbConnection.CloseAsync();
            }
        }

        public void Dispose()
        {
            _dbConnection?.Dispose();
        }

    }
    #endregion
}
