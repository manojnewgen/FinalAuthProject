using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace FinalAuthProject.Tests
{
    [TestFixture]
    public class SampleControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public SampleControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();
        }

        [Test]
        [TestCase("AdminPolicy", "/api/sample/admin", HttpStatusCode.OK)]
        [TestCase("UserPolicy", "/api/sample/user", HttpStatusCode.OK)]
        [TestCase("GuestPolicy", "/api/sample/guest", HttpStatusCode.OK)]
        [TestCase("InvalidPolicy", "/api/sample/admin", HttpStatusCode.Forbidden)]
        public async Task Endpoint_AccessBasedOnRole_ReturnsExpectedStatus(string role, string url, HttpStatusCode expectedStatus)
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", GenerateFakeJwtToken(role));

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(expectedStatus));
        }

        private string GenerateFakeJwtToken(string role)
        {
            // In a real test, you might want to use a proper JWT generator
            // This is simplified for demonstration
            return $"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." +
                   $"eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwicm9sZSI6InswfSIsImlhdCI6MTUxNjIzOTAyMn0." +
                   $"SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c".Replace("{0}", role);
        }
    }

    // Fake policy evaluator for testing
    public class FakePolicyEvaluator : IPolicyEvaluator
    {
        public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            var principal = new ClaimsPrincipal();
            var ticket = new AuthenticationTicket(principal, "Test");
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        public Task<PolicyAuthorizationResult> AuthorizeAsync(
            AuthorizationPolicy policy,
            AuthenticateResult authenticationResult,
            HttpContext context,
            object? resource)
        {
            // Extract role from the fake token
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var role = token.Split('.')[1].Split(',')[2].Split(':')[1].Trim('"');

            if (policy.Requirements.Any(r => r.GetType().Name.StartsWith(role)))
            {
                return Task.FromResult(PolicyAuthorizationResult.Success());
            }

            return Task.FromResult(PolicyAuthorizationResult.Forbid());
        }
    }
}