using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVMConsumeWCFWebService.Models.Contracts;

namespace WpfAppMVVMConsumeWCFWebService.Models.Implementations
{
    public class Employee : IEmployee
    {
        public int EmployeeId { get; set; }
        public string EmployeEmail { get; set; }
        public string EmployeName { get; set; }
        public double EmployeSalary { get; set; }
    }
}
