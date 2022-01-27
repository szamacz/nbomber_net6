using Xunit;
using System;
using System.Threading.Tasks;
using NBomber;
using NBomber.Contracts;
using NBomber.CSharp;
using System.Net.Http;

namespace LoadTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            using var httpClient = new HttpClient();

            var step = Step.Create("fetch_html_page", async context =>
            {
                var response = await httpClient.GetAsync("https://nbomber.com");

                return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
            });

            var scenario = ScenarioBuilder
                .CreateScenario("simple_http", step)
                .WithLoadSimulations(new[]
                {
                    Simulation.KeepConstant(copies: 1, during: TimeSpan.FromSeconds(2))
                });

            NBomberRunner
                .RegisterScenarios(scenario)
                .Run();
        }
    }
}