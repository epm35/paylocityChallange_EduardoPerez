using NUnit.Framework;
using RestSharp;
using System;

namespace QA_Automation.API.TestBase
{
    public class BaseApiTest
    {
        protected RestClient Client;
        string API_URL = Environment.GetEnvironmentVariable("Paylocity_Challenge_URL");
        string Token = Environment.GetEnvironmentVariable("Paylocity_Challenge_Token");

        [SetUp]
        public void Setup()
        {
            //var options = new RestClientOptions("https://wmxrwq14uc.execute-api.us-east-1.amazonaws.com/Prod/api")
            var options = new RestClientOptions(API_URL)
            {
                ThrowOnAnyError = false,
                Timeout = TimeSpan.FromSeconds(5)
            };

            Client = new RestClient(options);

            Client.AddDefaultHeader("Authorization", Token);
            Client.AddDefaultHeader("Content-Type", "application/json");
        }

        [TearDown]
        public void Cleanup()
        {
            Client?.Dispose();
        }
    }
}