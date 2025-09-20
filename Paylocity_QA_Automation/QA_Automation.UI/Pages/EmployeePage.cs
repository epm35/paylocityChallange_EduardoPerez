using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QA_Automation.UI.Pages
{
    public class EmployeePage
    {
        private readonly IPage _page;

        public EmployeePage(IPage page) => _page = page;

        private ILocator AddEmployeeButton => _page.Locator("id=add");
        private ILocator FirstNameInput => _page.Locator("id=firstName");
        private ILocator LastNameInput => _page.Locator("id=lastName");
        private ILocator DependentsInput => _page.Locator("id=dependants");
        private ILocator AddButton => _page.Locator("id=addEmployee");
        private ILocator ErrorDependentsMessage => _page.Locator("id=errorDependents");

        private ILocator EmployeeAddedList => _page.Locator("//table[@id='employeesTable']/tbody/tr");
        private ILocator DeleteIconInRow(ILocator row) => row.Locator("i.fa-times");

        public async Task ClickAddEmployeeAsync() => await AddEmployeeButton.ClickAsync();

        public async Task FillEmployeeForm(string firstName, string lastName, string dependents)
        {
            await FirstNameInput.FillAsync(firstName);
            await LastNameInput.FillAsync(lastName);
            await DependentsInput.FillAsync(dependents);
        }

        public async Task SubmitAsync() => await AddButton.ClickAsync();

        public async Task<string> GetDependentsErrorAsync()
        {
            return await ErrorDependentsMessage.InnerTextAsync(new LocatorInnerTextOptions { Timeout = 3000 });
        }

        public async Task<List<Dictionary<string, string>>> GetEmployeesAsync()
        {
            var employees = new List<Dictionary<string, string>>();

            // Wait until at least one row is rendered
            await EmployeeAddedList.First.WaitForAsync(new LocatorWaitForOptions { Timeout = 5000 });

            int rowCount = await EmployeeAddedList.CountAsync();

            for (int i = 0; i < rowCount; i++)
            {
                var row = EmployeeAddedList.Nth(i);
                var cells = row.Locator("td");
                int cellCount = await cells.CountAsync();

                if (cellCount == 1 && rowCount == 1)
                {
                    var text = (await cells.Nth(0).InnerTextAsync()).Trim();
                    if (text.Equals("No employees found", StringComparison.OrdinalIgnoreCase))
                        return employees;
                }

                employees.Add(new Dictionary<string, string>
                {
                    ["Id"] = await cells.Nth(0).InnerTextAsync(),
                    ["LastName"] = await cells.Nth(1).InnerTextAsync(),
                    ["FirstName"] = await cells.Nth(2).InnerTextAsync(),
                    ["Dependents"] = await cells.Nth(3).InnerTextAsync(),
                    ["Salary"] = await cells.Nth(4).InnerTextAsync(),
                    ["GrossPay"] = await cells.Nth(5).InnerTextAsync(),
                    ["BenefitsCost"] = await cells.Nth(6).InnerTextAsync(),
                    ["NetPay"] = await cells.Nth(7).InnerTextAsync()
                });
            }

            return employees;
        }
        
        public async Task ClickEditForEmployeeAsync(string firstName, string lastName)
        {
            int rowCount = await EmployeeAddedList.CountAsync();
            for (int i = 0; i < rowCount; i++)
            {
                var row = EmployeeAddedList.Nth(i);
                var rowFirstName = await row.Locator("td").Nth(2).InnerTextAsync();
                var rowLastName = await row.Locator("td").Nth(1).InnerTextAsync();

                if (rowFirstName == firstName && rowLastName == lastName)
                {
                    var editIcon = row.Locator("i.fa-edit");

                    await editIcon.ClickAsync(new LocatorClickOptions
                    {
                        Timeout = 3000,
                        Force = true
                    });

                    return;
                }
            }

            throw new Exception($"Employee {firstName} {lastName} not found for editing.");
        }

        public async Task UpdateEmployeeForm(string newFirstName, string newLastName, string newDependents)
        {
            await _page.Locator("id=firstName").FillAsync(newFirstName);
            await _page.Locator("id=lastName").FillAsync(newLastName);
            await _page.Locator("id=dependants").FillAsync(newDependents);
        }

        public async Task SaveChangesAsync()
        {
            await _page.Locator("id=updateEmployee").ClickAsync();

        }

        public async Task<bool> ClickDeleteForEmployeeAsync(string firstName, string lastName)
        {
            int rowCount = await EmployeeAddedList.CountAsync();

            for (int i = 0; i < rowCount; i++)
            {
                var row = EmployeeAddedList.Nth(i);
                var rowFirstName = await row.Locator("td").Nth(2).InnerTextAsync();
                var rowLastName = await row.Locator("td").Nth(1).InnerTextAsync();
                var idDeleted = await row.Locator("td").Nth(0).InnerTextAsync();

                if (rowFirstName == firstName && rowLastName == lastName)
                {
                    var deleteIcon = DeleteIconInRow(row);
                     await deleteIcon.ClickAsync(new LocatorClickOptions { Force = true });

                    if (await _page.Locator("text=Delete employee record for").IsVisibleAsync())
                    {
                        await _page.Locator("button:has-text('Delete')").ClickAsync();
                    }
                    //await WaitForTableFullyRenderedAsync();

                    var idDeletededAfter = await row.Locator("td").Nth(0).InnerTextAsync();
                    if (idDeleted != idDeletededAfter) 
                    {
                        return false;
                    }
                        
                    return true;
                }
            }

            throw new Exception($"Employee {firstName} {lastName} not found for deletion.");
        }

        public async Task WaitForTableFullyRenderedAsync(int stableMs = 500, int timeoutMs = 5000)
        {
            var deadline = DateTime.UtcNow.AddMilliseconds(timeoutMs);
            int lastCount = -1;
            DateTime lastChange = DateTime.UtcNow;

            while (DateTime.UtcNow < deadline)
            {
                int currentCount = await EmployeeAddedList.CountAsync();

                if (currentCount != lastCount)
                {
                    lastCount = currentCount;
                    lastChange = DateTime.UtcNow;
                }
                else if ((DateTime.UtcNow - lastChange).TotalMilliseconds >= stableMs)
                {
                    // Row count hasn't changed for stableMs, it means table is stable
                    return;
                }

                await _page.WaitForTimeoutAsync(100);
            }

            throw new TimeoutException("Table did not finish rendering in time.");
        }


    }


}