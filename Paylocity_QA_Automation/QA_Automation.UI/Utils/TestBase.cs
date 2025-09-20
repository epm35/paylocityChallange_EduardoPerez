using Microsoft.Playwright;
using NUnit.Framework;
using QA_Automation.UI.Pages;
using System.Globalization;
using System.Threading.Tasks;

namespace QA_Automation.UI.Utils
{
    public class TestBase
    {
        protected IPlaywright PlaywrightInstance;
        protected IBrowser Browser;
        protected IBrowserContext Context;
        protected IPage Page;

        [SetUp]
        public async Task Setup()
        {
            // Create Playwright instance
            PlaywrightInstance = await Playwright.CreateAsync();

            // Launch Chromium with headless = false for debugging
            Browser = await PlaywrightInstance.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 250
            });

            // Create a new browser context and page
            Context = await Browser.NewContextAsync();
            Page = await Context.NewPageAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            if (Context != null)
                await Context.CloseAsync();
            if (Browser != null)
                await Browser.CloseAsync();
            PlaywrightInstance?.Dispose();
        }

        public async Task AddEmployeeFlowAsync(string username, string password, string firstName, string lastName, string dependents)
        {
            var loginPage = new LoginPage(Page);
            var employeePage = new EmployeePage(Page);

            await loginPage.NavigateAsync();
            await loginPage.LoginAsync(username, password);

            await employeePage.ClickAddEmployeeAsync();
            await employeePage.FillEmployeeForm(firstName, lastName, dependents);
            await employeePage.SubmitAsync();
        }

        protected decimal ToDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            return decimal.Parse(
                value.Replace(",", "").Trim()
            );
        }

        protected record ExpectedValues(decimal Salary, decimal GrossPay, decimal BenefitsCost, decimal NetPay);

        protected ExpectedValues CalculateExpectedCompensation(int dependents)
        {
            //const decimal AnualSalary = 52000; //26*2000

            const decimal GrossPay = 2000; 
            const decimal EmployeeBenefit = 1000;
            const decimal DependentBenefit = 500;

            decimal salary = GrossPay * 26; //26 * 2000
            decimal gross = GrossPay;
            decimal benefits = Math.Round((EmployeeBenefit + (DependentBenefit * dependents))/26,2);
            decimal net = gross - benefits;

            return new ExpectedValues(salary, gross, benefits, net);
        }


    }
}