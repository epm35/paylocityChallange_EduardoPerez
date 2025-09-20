using NUnit.Framework;
using RestSharp;
using System;

namespace QA_Automation.API.TestBase
{
    public class BaseApiTest
    {
        protected RestClient Client;

        [SetUp]
        public void Setup()
        {
            var options = new RestClientOptions("https://wmxrwq14uc.execute-api.us-east-1.amazonaws.com/Prod/api")
            {
                ThrowOnAnyError = false,
                Timeout = TimeSpan.FromSeconds(5)
            };

            Client = new RestClient(options);

            Client.AddDefaultHeader("Authorization", "Basic VGVzdFVzZXI4MDc6N0NrO3NTRWtlXXAv");
            Client.AddDefaultHeader("Content-Type", "application/json");
        }

        [TearDown]
        public void Cleanup()
        {
            Client?.Dispose();
        }
    }
}