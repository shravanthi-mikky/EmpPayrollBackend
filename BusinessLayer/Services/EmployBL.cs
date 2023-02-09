using BusinessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class EmployBL : IEmployBL
    {
        IEmployRL iEmployRL;
        public EmployBL(IEmployRL iEmployRL)
        {
            this.iEmployRL = iEmployRL;
        }
        public EmployModel AddEmploy(EmployModel emp)
        {
            try
            {
                return iEmployRL.AddEmploy(emp);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public EmployModel UpdateEmploy(EmployModel employ)
        {
            try
            {
                return iEmployRL.UpdateEmploy(employ);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteEmploy(int EmployID)
        {
            try
            {
                return iEmployRL.DeleteEmploy(EmployID);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public EmployModel RetriveEmploy(long EmployeeId)
        {
            try
            {
                return iEmployRL.RetriveEmploy(EmployeeId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<EmployModel> GetAllEmployees()
        {
            try
            {
                return iEmployRL.GetAllEmployees();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool exportData()
        {
            try
            {
                return iEmployRL.exportData();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
