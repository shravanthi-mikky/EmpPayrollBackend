using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EmployPayroll.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserBL iUserBL;
        public UserController(IUserBL iUserBL)
        {
            this.iUserBL = iUserBL;
        }
        
        [HttpPost("Register")]
        public IActionResult AddUser(RegistrationModel registrationModel)
        {
            try
            {
                var result = iUserBL.Register(registrationModel);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Registration Successfull", Data = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Registration Unsuccessfull" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, message = e.Message });
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginModel loginModel)
        {
            try
            {
                var result = iUserBL.Login(loginModel);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Login Successfull", Data = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Login Unsuccessfull" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, message = e.Message });
            }
        }
        //[Authorize]
        [HttpPost("ForgetPassword")]
        public IActionResult ForgetPassword(string email)
        {
            try
            {
                string token = iUserBL.ForgetPassword(email);
                if (token != null)
                {
                    return Ok(new { success = true, Message = "Please check your Email.Token sent succesfully." });
                }
                else
                {
                    return this.BadRequest(new { Success = false, Message = "Email not registered" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpPost("Reset")]
        public IActionResult ResetPassword(string email, string newpassword, string confirmpassword)
        {
            try
            {
                //var email = User.FindFirst("Email").Value.ToString();
                var data = iUserBL.ResetPassword(email, newpassword, confirmpassword);
                if (data != null)
                {
                    if (newpassword == confirmpassword)
                    {
                        return this.Ok(new { Success = true, message = "Your password has been changed sucessfully" });
                    }
                    else
                    {
                        return this.Ok(new { Success = true, message = "Password are not matched" });
                    }
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Unable to reset password.Please try again" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}
