using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IEmployRL
    {
        public EmployModel AddEmploy(EmployModel emp);
        public EmployModel RetriveEmploy(long EmployeeId);
        public bool DeleteEmploy(int EmployID);
        public bool exportData();
        public EmployModel UpdateEmploy(EmployModel employ);
        public List<EmployModel> GetAllEmployees();
    }
}
