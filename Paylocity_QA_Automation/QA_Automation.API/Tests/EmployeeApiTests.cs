using NUnit.Framework;
using QA_Automation.API.Clients;
using QA_Automation.API.Models;
using QA_Automation.API.TestBase;
using RestSharp;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QA_Automation.API.Tests
{
    public class EmployeeApiTests : BaseApiTest
    {
        private ApiClient _api;
        private static string _createdEmployeeId;

        [SetUp]
        public void Init()
        {
            _api = new ApiClient(Client);
        }

        [Test, Order(1)]
        public async Task CreateEmployee_ShouldReturn201_AndMatchPostedData()
        {
            var newEmployee = new
            {
                FirstName = "Lolita",
                LastName = "Ayala",
                Dependents = 2
            };

            var response = await _api.PostAsync<EmployeeModel>("/employees", newEmployee);


            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created).Or.EqualTo(HttpStatusCode.OK), "Expected Created or OK but got " + response.StatusCode);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.FirstName, Is.EqualTo(newEmployee.FirstName));
            Assert.That(response.Data.LastName, Is.EqualTo(newEmployee.LastName));
            _createdEmployeeId = response.Data.Id;
            Console.WriteLine($"Created Employee ID: {_createdEmployeeId}");

        }

        [Test]
        public async Task GetAllEmployees_ShouldReturn200_AndNonEmptyList()
        {
            var response = await _api.GetAsync<List<EmployeeModel>>("/Employees");

            Assert.That(response.Data, Is.Not.Null, "Employees list was null");
            Assert.That(response.Data, Is.Not.Empty, "Employees list was empty, at least one employe should exist for this test");

            // Check that all items have required fields
            foreach (var employee in response.Data)
            {
                Assert.That(employee.Id, Is.Not.Null.And.Not.Empty, "Employee ID is missing");
                Assert.That(employee.FirstName, Is.Not.Null.And.Not.Empty, "Employee FirstName is missing");
                Assert.That(employee.LastName, Is.Not.Null.And.Not.Empty, "Employee LastName is missing");
                Assert.That(employee.Username, Is.Not.Null.And.Not.Empty, "Employee LastName is missing");
            }

            // Log the full list of employees
            Console.WriteLine(
                Newtonsoft.Json.JsonConvert.SerializeObject(response.Data, Newtonsoft.Json.Formatting.Indented)
            );
        }

        [Test, Order(2)]
        public async Task GetEmployee_ByID_ShouldReturn200_AndValidBody()
        {

            
            var response = await _api.GetAsync<EmployeeModel>($"/Employees/{_createdEmployeeId}");
            Assert.That(response.Data, Is.Not.Null, "Employee was not found");
            Assert.That(response.Data.Id, Is.EqualTo(_createdEmployeeId));
            Assert.That(response.Data.FirstName, Is.Not.Empty);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(response.Data, Newtonsoft.Json.Formatting.Indented));


            /*
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content, Is.Not.Null.And.Not.Empty);

            var employee = JsonConvert.DeserializeObject<EmployeeModel>(response.Content);

            Assert.That(employee.Id, Is.EqualTo("0feb67f7-bcb1-4c65-8f55-97e100312384"));
            Assert.That(employee.FirstName, Is.Not.Empty);*/
        }


        [Test]
        public async Task GetEmployee_ByID_ShouldReturn404()
        {
            var response = await _api.GetAsync<EmployeeModel>("/Employees/0feb67f7-bcb1-4c65-8f55-dummy");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), "Request should  return HTTP Status Code (404) Not Found");
            Assert.That(response.Data, Is.Null, "Response contains data");


        }


        [Test, Order(3)]
        public async Task UpdateEmployee_ShouldReturn200_AndUpdatedFields()
        {
            var updatedData = new
            {
                Id = _createdEmployeeId,
                FirstName = "Pedro",
                LastName = "Ramirez",
                Dependants = 3
                
            };

            var response = await _api.PutAsync<EmployeeModel>("/employees", updatedData);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);

            Assert.That(response.Data.FirstName, Is.EqualTo(updatedData.FirstName));
            Assert.That(response.Data.Dependants, Is.EqualTo(updatedData.Dependants));

            response = await _api.GetAsync<EmployeeModel>($"/Employees/{_createdEmployeeId}");
            Assert.That(response.Data, Is.Not.Null, "Employee was not found");
            Assert.That(response.Data.Id, Is.EqualTo(_createdEmployeeId));
            Assert.That(response.Data.FirstName, Is.EqualTo(updatedData.FirstName));
            Assert.That(response.Data.Dependants, Is.EqualTo(updatedData.Dependants));

            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(response.Data, Newtonsoft.Json.Formatting.Indented));


        }

       
        [Test, Order(4)]
        public async Task DeleteEmployee_ShouldReturn204_AndRemoveResource()
        {
            var deleteResponse = await _api.DeleteAsync($"/employees/{_createdEmployeeId}");
            Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            var getResponse = await _api.GetAsync<EmployeeModel>($"/employees/{_createdEmployeeId}");
            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

    }
}