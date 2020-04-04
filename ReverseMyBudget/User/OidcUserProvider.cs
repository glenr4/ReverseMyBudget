using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace ReverseMyBudget
{
    internal class OidcUserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _http;
        private readonly string userClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public OidcUserProvider(IHttpContextAccessor http)
        {
            _http = http;
        }

        Guid IUserProvider.UserId => GetUserId();

        private Guid GetUserId()
        {
            try
            {
                var userId = _http.HttpContext.User.Claims.First(a => a.Type == userClaimType);

                return new Guid(userId.Value);
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("Missing User Identifier Claim", ex);
            }
        }
    }
}