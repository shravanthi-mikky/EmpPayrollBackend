using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Grid;
using System;
using System.Collections.Generic;
using System.IO;

namespace EmployPayroll.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployController : ControllerBase
    {
        IEmployBL iEmployBL;
        public EmployController(IEmployBL iEmployBL)
        {
            this.iEmployBL = iEmployBL;
        }

        string ConnString = "Server=localhost;Port=5432;Database=EmpAngularjs;Username=postgres; Password=Mickey@27;Integrated Security=True;";


        [HttpPost("AddEmployee")]
        public IActionResult AddEmploy(EmployModel EmpModel)
        {
            try
            {
                var result = iEmployBL.AddEmploy(EmpModel);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Employ Details are Added", Data = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Unable to add details" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, message = e.Message });
            }

        }

        [HttpPut("Update")]
        public IActionResult UpdateEmploy(EmployModel employ)
        {
            try
            {
                var reg = this.iEmployBL.UpdateEmploy(employ);
                if (reg != null)

                {
                    return this.Ok(new { Success = true, message = "Employ Updated Sucessfull", Response = reg });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Employ details not updated" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }

        [HttpDelete("Delete")]
        public IActionResult DeleteEmploy(int EmployID)
        {
            try
            {
                var reg = this.iEmployBL.DeleteEmploy(EmployID);
                if (reg != null)

                {
                    return this.Ok(new { Success = true, message = "Employ Deleted Sucessfull", Response = reg });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Unable to delete" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }
        //[Authorize]
        [HttpGet("Get")]
        public IActionResult GetAllEmployees()
        {
            try
            {
                var reg = this.iEmployBL.GetAllEmployees();
                if (reg != null)

                {
                    return this.Ok(new { Success = true, message = "All Employ Details", Response = reg });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Unable to get details" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }

        //[Authorize]
        [HttpPost("Export")]
        public IActionResult exportData()
        {
            try
            {
                var reg = this.iEmployBL.exportData();
                if (reg != null)

                {
                    return this.Ok(new { Success = true, message = "Exported", Response = reg });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Unable to Export" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }

        // [Authorize]
        [HttpGet("GetEmployById")]
        public IActionResult RetriveEmploy(int employId)
        {
            try
            {
                var reg = this.iEmployBL.RetriveEmploy(employId);
                if (reg != null)

                {
                    return this.Ok(new { Success = true, message = "Employ Details", Response = reg });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Unable to get Employ details" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }

        

      }
}
