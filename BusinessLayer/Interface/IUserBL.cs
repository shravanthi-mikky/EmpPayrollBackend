using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        public RegistrationModel Register(RegistrationModel registrationModel);

        public string Login(LoginModel loginModel);
        public string ForgetPassword(string Emailid);

        public bool ResetPassword(string email, string newpassword, string confirmpassword);
    }
}
