namespace QA_Automation.API.Models
{
    public class EmployeeModel
    {
        public string SortKey { get; set; }
        public string Username { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Dependants { get; set; }
        public DateTime Expiration { get; set; }
        public float Salary { get; set; }
        public float Gross { get; set; }
        public float BenefitsCost { get; set; }
        public float Net { get; set; }
     
    }
}