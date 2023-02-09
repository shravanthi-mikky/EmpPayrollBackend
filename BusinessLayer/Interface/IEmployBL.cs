using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IEmployBL
    {
        public EmployModel AddEmploy(EmployModel emp);
        public EmployModel RetriveEmploy(long EmployeeId);
        public EmployModel UpdateEmploy(EmployModel employ);
        public bool DeleteEmploy(int EmployID);

        public bool exportData();
        public List<EmployModel> GetAllEmployees();
    }
}
