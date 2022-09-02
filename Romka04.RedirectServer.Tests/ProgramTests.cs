using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using FluentAssertions;

namespace Romka04.RedirectServer
{
    public class ProgramTests
    {
        [TestCase("roman", "/")]
        [TestCase("Roman", "/")]
        [TestCase("ROMAN", "/")]
        [TestCase("roman", "/some.other.url")]
        [TestCase("roman", "/some")]
        [TestCase("Roman", "/some.other.url")]
        [TestCase("ROMAN", "/some.other.url")]
        public async Task Redirect_ProperUser_Should_RedirectToProperUri(string username, string url)
        {
            await using var app = new RedirectApplication();

            var httpClient = app.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Remote-User", username);

            var response = await httpClient.SendAsync(request);

            response.Should().HaveStatusCode(HttpStatusCode.Redirect);
            response.Headers.Location.Should().Be("http://heimdall.roman-assol.site");
        }

        [Test]
        public async Task Redirect_NoUserName_Should_ReturnBadRequest()
        {
            await using var app = new RedirectApplication();

            var httpClient = app.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/");

            var response = await httpClient.SendAsync(request);

            response.Should().HaveStatusCode(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Be("\"HTTP Header 'Remote-User' is expected\"");
        }
        
        [TestCase("unknown-user")]
        public async Task Redirect_UnknownUser_Should_ReturnNotFound(string username)
        {
            await using var app = new RedirectApplication();

            var httpClient = app.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/");
            request.Headers.Add("Remote-User", username);

            var response = await httpClient.SendAsync(request);

            response.Should().HaveStatusCode(HttpStatusCode.NotFound);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Be($"\"Redirect rule for Remote-User '{username}' has not found\"");
        }

        [SetUp]
        public void Setup()
        { }
    }

    class RedirectApplication
        : WebApplicationFactory<Program>
    { }
}