using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVMConsumeWCFWebService.Models.Contracts
{
    public interface IEmployee
    {
        int EmployeeId { get; set; }
        string EmployeEmail { get; set; }
        string EmployeName { get; set; }
        double EmployeSalary { get; set; }
    }
}
