using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRL : IUserRL
    {
        private IConfiguration config;
        SqlConnection sqlConnection;
        string ConnString = "Data Source=LAPTOP-2UH1FDRP\\MSSQLSERVER01;Initial Catalog=EmpAngularjs;Integrated Security=True;";
        public UserRL(IConfiguration config)
        {
            this.config = config;
        }
        public RegistrationModel Register(RegistrationModel registrationModel)
        {
            //sqlConnection = new SqlConnection(this.config.GetConnectionString("BookStoreDB"));
            sqlConnection = new SqlConnection(ConnString);

            using (sqlConnection)
                try
                {
                    //var password = this.EncryptPassword(registrationModel.Password);
                    var password = registrationModel.Password;
                    SqlCommand sqlCommand = new SqlCommand("dbo.SP_Register", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@Fullname", registrationModel.Fullname);
                    sqlCommand.Parameters.AddWithValue("@Email", registrationModel.Email);
                    sqlCommand.Parameters.AddWithValue("@Mobile", registrationModel.Mobile);
                    sqlCommand.Parameters.AddWithValue("@Password", registrationModel.Password);

                    int result = sqlCommand.ExecuteNonQuery();
                    if (result > 0)
                        return registrationModel;
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
        }
        //JWT Method
        public string GenerateJWTToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(config["Jwt:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("Email", email) }),
                Expires = DateTime.UtcNow.AddDays(11),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public string Login(LoginModel loginModel)
        {
            // sqlConnection = new SqlConnection(this.config.GetConnectionString("BookStoreDB"));
            //sqlConnection = new SqlConnection(ConnString);
            using (sqlConnection = new SqlConnection(ConnString))
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("dbo.SP_Login", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@Email", loginModel.Email);
                    sqlCommand.Parameters.AddWithValue("@Password", loginModel.Password);

                    SqlDataReader rd = sqlCommand.ExecuteReader();
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            loginModel.Email = Convert.ToString(rd["Email"] == DBNull.Value ? default : rd["Email"]);
                            loginModel.Password = Convert.ToString(rd["Password"] == DBNull.Value ? default : rd["Password"]);
                        }
                        var token = this.GenerateJWTToken(loginModel.Email);
                        return token;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);

                }
                finally { sqlConnection.Close(); }
        }
        
        public string ForgetPassword(string Emailid)
        {
            try
            {
                sqlConnection = new SqlConnection(ConnString);
                SqlCommand com = new SqlCommand("SpForgetPass", sqlConnection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Email", Emailid);
                sqlConnection.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        Emailid = Convert.ToString(rd["Email"] == DBNull.Value ? default : rd["Email"]);
                    }
                    var token = this.GenerateJWTToken(Emailid);
                    new MSMQ_Model().sendData2Queue(token);
                    return token;
                }
                sqlConnection.Close();
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool ResetPassword(string email, string newpassword, string confirmpassword)
        {
            try
            {
                if (newpassword == confirmpassword)
                {
                    sqlConnection = new SqlConnection(ConnString);
                    SqlCommand com = new SqlCommand("SpReset", sqlConnection);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Email", email);
                    com.Parameters.AddWithValue("@Password", newpassword);
                    sqlConnection.Open();
                    SqlDataReader rd = com.ExecuteReader();
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            email = Convert.ToString(rd["Email"] == DBNull.Value ? default : rd["Email"]);
                            newpassword = Convert.ToString(rd["Password"] == DBNull.Value ? default : rd["Password"]);
                        }
                        return true;
                    }
                    return true;

                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        
    }
}
