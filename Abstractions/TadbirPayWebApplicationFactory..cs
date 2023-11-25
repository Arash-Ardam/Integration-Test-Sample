using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TPG.SI.TadbirPay.URLTest.Abstractions
{
    public class TadbirPayWebApplicationFactory : WebApplicationFactory<Program>
    {
        private const string _testEnvironment = "Test";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment(_testEnvironment);
            builder.ConfigureServices(testContextAceessor => testContextAceessor.AddSingleton<IHttpContextAccessor>
            (services =>
            {
                var accessor = new Moq.Mock<IHttpContextAccessor>();

                var context = new DefaultHttpContext();
                context.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("192.168.13.12");

                context.User.AddIdentity(new ClaimsIdentity(new List<Claim> { new Claim("tenant", "TadbirPardaz") }));

                accessor.Setup(x => x.HttpContext).Returns(context);
                return accessor.Object;
            }
            ));
        }
    }
}
