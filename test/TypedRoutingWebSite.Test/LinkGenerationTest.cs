namespace TypedRoutingWebSite.Test
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading.Tasks;
    using Xunit;

    public class LinkGenerationTest
    {
        private readonly TestServer server;
        private readonly HttpClient client;

        public LinkGenerationTest()
        {
            var webHost = new WebHostBuilder()
                .UseStartup<Startup>()
                .UseContentRoot(SolutionPathUtility.GetProjectPath(
                    "samples", 
                    typeof(Startup).GetTypeInfo().Assembly));

            this.server = new TestServer(webHost);
            this.client = this.server.CreateClient();
        }

        [Fact]
        public async Task NonAreaToAreaLinkGeneration_ShouldWorkCorrectly()
        {
            var response = await this.client.GetAsync("/Home/ToArea");

            var result = await response.Content.ReadAsStringAsync();

            Assert.Equal("/Admin/Area", result.Trim());
        }
        
        [Fact]
        public async Task AreaToAreaLinkGeneration_ShouldWorkCorrectly()
        {
            var response = await this.client.GetAsync("/Admin/Area/ToOther");

            var result = await response.Content.ReadAsStringAsync();

            Assert.Equal("/Other/OtherArea", result.Trim());
        }
        
        [Fact]
        public async Task AreaToNonAreaLinkGeneration_ShouldWorkCorrectly()
        {
            var response = await this.client.GetAsync("/Admin/Area");

            var result = await response.Content.ReadAsStringAsync();

            Assert.Equal("/Manage/AddPhoneNumber", result.Trim());
        }

        [Fact]
        public async Task RazorLinkGeneration_ShouldWorkCorrectly()
        {
            var response = await this.client.GetAsync("/Home/Razor");

            var result = await response.Content.ReadAsStringAsync();

            Assert.Equal("<a href=\"/WithParameter/1\">Test</a>", result.Trim());
        }
    }
}
