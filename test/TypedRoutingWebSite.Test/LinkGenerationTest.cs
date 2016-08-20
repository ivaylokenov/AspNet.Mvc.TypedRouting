namespace TypedRoutingWebSite.Test
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using System.IO;
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
        public async Task NonAreaToAreaLinkGenerationShouldWorkCorrectly()
        {
            var response = await this.client.GetAsync("/Home/ToArea");

            var result = await response.Content.ReadAsStringAsync();

            Assert.Equal("/Admin/Area", result.Trim());
        }
        
        [Fact]
        public async Task AreaToAreaLinkGenerationShouldWorkCorrectly()
        {
            var response = await this.client.GetAsync("/Admin/Area/ToOther");

            var result = await response.Content.ReadAsStringAsync();

            Assert.Equal("/Other/OtherArea", result.Trim());
        }
        
        [Fact]
        public async Task AreaToNonAreaLinkGenerationShouldWorkCorrectly()
        {
            var response = await this.client.GetAsync("/Admin/Area");

            var result = await response.Content.ReadAsStringAsync();

            Assert.Equal("/Manage/AddPhoneNumber", result.Trim());
        }
    }
}
