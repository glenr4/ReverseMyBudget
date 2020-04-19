using Microsoft.AspNetCore.Http;
using ReverseMyBudget.Domain;
using System;

namespace ReverseMyBudget
{
    public class TestUserProvider : IUserProvider
    {
        private IHttpContextAccessor _http;

        public TestUserProvider(IHttpContextAccessor http)
        {
            _http = http;
        }

        public Guid UserId => GetTestUser();

        private Guid GetTestUser()
        {
            _http.HttpContext.Request.Headers.TryGetValue("TestUser", out var result);

            try
            {
                return new Guid(result.ToString());
            }
            catch
            {
                return Guid.Empty;
            }
        }
    }
}