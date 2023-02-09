using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
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
        SqlConnection sqlConnection1;
        NpgsqlConnection sqlConnection;
        //string ConString = "Data Source=LAPTOP-2UH1FDRP\\MSSQLSERVER01;Initial Catalog=EmpAngularjs;Integrated Security=True;";
        string ConnString = "Server=localhost;Port=5432;Database=EmpAngularjs;Username=postgres; Password=Mickey@27;Integrated Security=True;";
        public UserRL(IConfiguration config)
        {
            this.config = config;
        }
        public RegistrationModel Register(RegistrationModel registrationModel)
        {
            //sqlConnection = new SqlConnection(this.config.GetConnectionString("BookStoreDB"));
            sqlConnection = new NpgsqlConnection(ConnString);

            using (sqlConnection)
                try
                {
                    //var password = this.EncryptPassword(registrationModel.Password);
                    //var password = registrationModel.Password;
                    NpgsqlCommand sqlCommand = new NpgsqlCommand("Call SP_Register(:Fullname,:Email,:Mobile,:Password)", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.Text;

                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("Fullname", DbType.String).Value = registrationModel.Fullname;
                    sqlCommand.Parameters.AddWithValue("Email", DbType.String).Value = registrationModel.Email;
                    sqlCommand.Parameters.AddWithValue("Mobile", DbType.Int32).Value = registrationModel.Mobile;
                    sqlCommand.Parameters.AddWithValue("Password", DbType.String).Value = registrationModel.Password;

                    int result = sqlCommand.ExecuteNonQuery();
                    if (result!=null)
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
            using (sqlConnection = new NpgsqlConnection(ConnString))
                try
                {
                    string query = "select Id from Users where Email='" + loginModel.Email + "' and Password='" + loginModel.Password + "';";
                    NpgsqlCommand sqlCommand = new NpgsqlCommand(query, sqlConnection);
                    
                    sqlConnection.Open();


                    var result = sqlCommand.ExecuteScalar();
                    if(result != null)
                    { 
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
                sqlConnection = new NpgsqlConnection(ConnString);
                NpgsqlCommand com = new NpgsqlCommand("SpForgetPass", sqlConnection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Email", Emailid);
                sqlConnection.Open();
                NpgsqlDataReader rd = com.ExecuteReader();
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
                    sqlConnection = new NpgsqlConnection(ConnString);
                    NpgsqlCommand com = new NpgsqlCommand("SpReset", sqlConnection);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Email", email);
                    com.Parameters.AddWithValue("@Password", newpassword);
                    sqlConnection.Open();
                    NpgsqlDataReader rd = com.ExecuteReader();
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
