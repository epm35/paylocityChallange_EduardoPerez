using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA_Automation.UI.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;

        public LoginPage(IPage page) => _page = page;

        private ILocator UsernameInput => _page.Locator("id=Username");
        private ILocator PasswordInput => _page.Locator("id=Password");
        private ILocator LoginButton => _page.Locator("text='Log In'");

        public async Task NavigateAsync()
        {
            await _page.GotoAsync("https://wmxrwq14uc.execute-api.us-east-1.amazonaws.com/Prod/Account/LogIn");
        }

        public async Task LoginAsync(string username, string password)
        {
            await UsernameInput.FillAsync(username);
            await PasswordInput.FillAsync(password);
            await LoginButton.ClickAsync();
        }
    }
}
